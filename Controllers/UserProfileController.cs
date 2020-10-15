using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FVRcal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FVRcal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private DatabaseContext db = new DatabaseContext();
        private UserManager<Account> _userManager;

        public UserProfileController(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<Object> GetUserProfile()
        {
            var papiez = User.Claims;
            var d0pa = User.Identity.IsAuthenticated;
            int userID = Int32.Parse(User.Claims.First(c => c.Type == "UserID").Value);
            Account user = await db.Accounts.Where(a => a.user_id == userID).SingleOrDefaultAsync();
            return new
            {
                user.firstname,
                user.lastname,
                user.email,
                user.username,
                user.usercode
            };
        }
    }
}
