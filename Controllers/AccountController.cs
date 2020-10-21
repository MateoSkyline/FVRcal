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
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace FVRcal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }
        private readonly ApplicationSettings _appSettings;


        public AccountController(IOptions<ApplicationSettings> appSettings, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
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

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var link = Url.Action(nameof(VerifyEmail), "Account", new { userID = applicationUser.Id, token }, Request.Scheme, Request.Host.ToString());

                    SendMail(applicationUser.Email, "FVRcal Verification", $"<a href=\"{link}\">Verify Email</a>");
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string userID, string token)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) return Ok();
            return BadRequest();
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
            if(account.Email != null)
            {
                string newEmail = account.Email;
                string oldEmail = actualUser.Email;
                string token = await _userManager.GenerateChangeEmailTokenAsync(actualUser, newEmail);
                string link = Url.Action(nameof(VerifyChangedEmail), "Account", new { userID = actualUser.Id, newMail = newEmail, token }, Request.Scheme, Request.Host.ToString());
                SendMail(newEmail, "FVRcal Email Verification", $"<a href=\"{link}\">Verify this Email</a>");
            }

            if (!problems)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("VerifyChangedEmail")]
        public async Task<IActionResult> VerifyChangedEmail(string userID, string newMail, string token)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) return BadRequest();
            var result = await _userManager.ChangeEmailAsync(user, newMail, token);
            if (result.Succeeded) return Ok();
            return BadRequest();
        }


        /*
         
         Sending mails
         
         */

        public bool SendMail(string email, string subject, string message)
        {
            try
            {
                string login = Configuration["EmailCredentials:Email"];
                string password = Configuration["EmailCredentials:Password"];
                new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(login, password)
                }.Send(new MailMessage { From = new MailAddress(login, "FVRcal"), To = { email }, Subject = subject, Body = message, BodyEncoding = Encoding.UTF8, IsBodyHtml = true }) ;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
