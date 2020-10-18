using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FVRcal.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FVRcal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        

        public AccountController(IOptions<ApplicationSettings> appSettings, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /*
         
         Creating user accounts section
         
        */

        [HttpPost]
        [Route("Register")]
        //POST : /api/Account/Register
        public async Task<Object> PostApplicationUser(Account account)
        {
            var applicationUser = new ApplicationUser()
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                UserName = account.UserName,
                UserCode = account.UserCode
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, account.Password);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /*
         
         Login section
         
        */

        [HttpPost]
        [Route("Login")]
        //POST : /api/Account/Login
        public async Task<IActionResult> Login(Account account)
        {
            var user = await _userManager.FindByEmailAsync(account.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, account.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(3),
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

        /*
         
         User editing section
         
        */

        [HttpGet]
        [Authorize]
        [Route("UserEdit")]
        //GET: /api/Account/UserEdit
        public async Task<Object> UserEdit()
        {
            string userID = User.Claims.First(c => c.Type == "UserID").Value;
            ApplicationUser actualUser = await _userManager.FindByIdAsync(userID);
            return new
            {
                actualUser.FirstName,
                actualUser.LastName,
                actualUser.UserName,
                actualUser.PhoneNumber,
                actualUser.PhoneNumberConfirmed,
                actualUser.Email,
                actualUser.EmailConfirmed,
                actualUser.TwoFactorEnabled                
            };
        }

        [HttpPost]
        [Authorize]
        [Route("UserEdit")]
        //POST: /api/Account/UserEdit
        public async Task<IActionResult> UserEdit(ApplicationUser account)
        {
            string userID = User.Claims.First(c => c.Type == "UserID").Value;
            ApplicationUser actualUser = await _userManager.FindByIdAsync(userID);
            Boolean problems = false;
            
            if(account.FirstName != null)
            {
                actualUser.FirstName = account.FirstName;
                try { await _userManager.UpdateAsync(actualUser); } catch(Exception) { problems = true; }
            }
            if (account.LastName != null)
            {
                actualUser.LastName = account.LastName;
                try { await _userManager.UpdateAsync(actualUser); } catch (Exception) { problems = true; }
            }
            if (account.PhoneNumber != null)
            {
                actualUser.PhoneNumber = account.PhoneNumber;
                try { await _userManager.UpdateAsync(actualUser); } catch (Exception) { problems = true; }
            }
            if (account.TwoFactorEnabled != actualUser.TwoFactorEnabled)
            {
                actualUser.TwoFactorEnabled = account.TwoFactorEnabled;
                try { await _userManager.UpdateAsync(actualUser); } catch (Exception) { problems = true; }
            }
            if(account.PasswordHash != null)
            {
                var changePassword = await _userManager.ChangePasswordAsync(actualUser, account.OldPassword, account.PasswordHash);
                if(!changePassword.Succeeded) problems = true;
            }

            if (!problems)
                return Ok();
            else
                return BadRequest();
        }
    }
}
