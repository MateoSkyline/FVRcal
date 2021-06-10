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

    }

    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    //public class Teams
    //{
    //    public int Id { get; set; }
    //    [StringLength(60)]
    //    public string Name { get; set; }
    //    [StringLength(2000)]
    //    public string Description { get; set; }
    //    public int Type_Id { get; set; }
    //    public int Status_Id { get; set; }
    //    public int Visibility { get; set; }
    //    public decimal Commission { get; set; }
    //}

    //public class TeamRoles
    //{
    //    public int Id { get; set; }
    //    [StringLength(60)]
    //    public string Name { get; set; }
    //}

    //public class TeamAssignments
    //{
    //    public int Id { get; set; }
    //    public int Team_Id { get; set; }
    //    public int User_Id { get; set; }
    //    public int Role_Id { get; set; }
    //    public decimal Share { get; set; }
    //}

    //public class Orders
    //{
    //    public int Id { get; set; }
    //    public int Team_Id { get; set; }
    //    [StringLength(120)]
    //    public string Name { get; set; }
    //    [StringLength(2000)]
    //    public string Description { get; set; }
    //    [StringLength(90)]
    //    public string Recipient { get; set; }
    //    public decimal Commission { get; set; }
    //    public decimal Amount { get; set; }
    //    public int Currency_Id { get; set; }
    //    public DateTime Date { get; set; }
    //}

    //public class OrderAssignments
    //{
    //    public int Id { get; set; }
    //    public int Order_Id { get; set; }
    //    public int User_Id { get; set; }
    //    public int Role_Id { get; set; }
    //    public decimal Share { get; set; }
    //}
}
