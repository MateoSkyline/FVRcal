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

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<Storage_Type> Storage_Type { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL("server=192.168.0.80;port=3306;database=fvrcal;user=root;password=SeksSylcia01012020;");
    }

    public class Account
    {
        [Key]
        public int user_id { get; set; }
        [StringLength(60)]
        public string firstname { get; set; }
        [StringLength(60)]
        public string lastname { get; set; }
        [StringLength(120)]
        [Required]
        public string email { get; set; }
        [StringLength(30)]
        public string username { get; set; }
        [StringLength(6)]
        public string usercode { get; set; }
        [StringLength(128)]
        public string password { get; set; }
        [StringLength(16)]
        public string salt { get; set; }
        [StringLength(256)]
        public string permissions { get; set; }
    }

    public class Storage
    {
        [Key]
        public int storage_id { get; set; }
        public int user_id { get; set; }
        public int type { get; set; }
        public DateTime time { get; set; }
        [StringLength(64)]
        public string flags { get; set; }
    }

    public class Storage_Type
    {
        [Key]
        public int st_type_id { get; set; }
        [StringLength(12)]
        public string type { get; set; }
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
