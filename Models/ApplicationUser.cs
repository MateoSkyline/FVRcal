using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FVRcal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(6)")]
        public string UserCode { get; set; }
        [Column(TypeName = "nvarchar(60)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(60)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(9)")]
        public string SecuritySalt { get; set; }
    }
}
