﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FVRcal.Models
{
    public class UserEditModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TwoFactorEnabled { get; set; }
        public string PasswordHash { get; set; }
        public string OldPassword { get; set; }
    }
}