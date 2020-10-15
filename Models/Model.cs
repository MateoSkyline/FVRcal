using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;

namespace FVRcal.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Storage> Storage { get; set; }
        public DbSet<Storage_Type> Storage_Type { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL("server=192.168.0.80;port=3306;database=fvrcal;user=root;password=SeksSylcia01012020;");
    }

    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string SecuritySalt { get; set; }
    }

    public class Storage
    {
        [Key]
        public int Storage_id { get; set; }
        public int User_Id { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }
        [StringLength(64)]
        public string Flags { get; set; }
    }

    public class Storage_Type
    {
        [Key]
        public int St_Type_Id { get; set; }
        [StringLength(12)]
        public string Type { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
