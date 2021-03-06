﻿// <auto-generated />
using System;
using FVRcal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FVRcal.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FVRcal.Models.Account", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("firstname")
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("lastname")
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("password")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("permissions")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("salt")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("usercode")
                        .HasColumnType("varchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("username")
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("user_id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FVRcal.Models.Storage", b =>
                {
                    b.Property<int>("storage_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("flags")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("storage_id");

                    b.ToTable("Storage");
                });

            modelBuilder.Entity("FVRcal.Models.Storage_Type", b =>
                {
                    b.Property<int>("st_type_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("type")
                        .HasColumnType("varchar(12)")
                        .HasMaxLength(12);

                    b.HasKey("st_type_id");

                    b.ToTable("Storage_Type");
                });
#pragma warning restore 612, 618
        }
    }
}
