using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FVRcal.Models;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;
using System.Text;

namespace FVRcal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterAccountController : ControllerBase
    {
        private DatabaseContext db = new DatabaseContext();
        const String DEFAULT_PERMISSIONS = "all";

        [HttpPost]
        public int RegisterAccount(Account account)
        {
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
    }
}
