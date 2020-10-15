using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FVRcal.Models;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace FVRcal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private DatabaseContext db = new DatabaseContext();
        private readonly ApplicationSettings _appSettings;

        public AccountController(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        public int RegisterAccount(Account account)
        {
            const String DEFAULT_PERMISSIONS = "all";
            try 
            { 
                account.salt = new Random().Next(100000000, 999999999).ToString();
                account.password = ComputeSHA256Hash(ComputeSHA256Hash(account.password) + ComputeSHA256Hash(account.salt));
                account.usercode = GenerateNewRandom();      
            
                if (EmailExists(account.email)) //This email already exists
                {
                    account.email = null;
                    return 1; //Email problem
                }
                account.permissions = DEFAULT_PERMISSIONS;
                db.Accounts.Add(account);

                db.SaveChanges(); 
                return 0; //Everything went OK and account has been created
            }
            catch (Exception)
            {
                return 2; //Problem with creating account
            }
        }

        public static string ComputeSHA256Hash(string text)
        {
            using(var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
        private string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (db.Accounts.Any(c => c.usercode == r))
            {
                r = GenerateNewRandom();
            }
            return r;
        }
        private bool EmailExists(String email)
        {
            return db.Accounts.Any(e => e.email == email);
        }
        public static string MakePasswordHash(string password, string salt)
        {
            return ComputeSHA256Hash(ComputeSHA256Hash(password) + ComputeSHA256Hash(salt));
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Account account)
        {
            var user = await db.Accounts.Where(e => e.email == account.email).SingleOrDefaultAsync();
            if(user != null && user.password == MakePasswordHash(account.password, user.salt))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.user_id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)                    
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }
        }
    }
}
