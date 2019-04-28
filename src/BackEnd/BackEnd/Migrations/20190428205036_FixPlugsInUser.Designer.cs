﻿// <auto-generated />
using System;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackEnd.Migrations
{
    [DbContext(typeof(SmartSwitchDbContext))]
    [Migration("20190428205036_FixPlugsInUser")]
    partial class FixPlugsInUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BackEnd.Models.Plug", b =>
                {
                    b.Property<string>("Mac")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approved");

                    b.Property<bool>("IsOn");

                    b.Property<string>("Nickname");

                    b.Property<int>("Priority");

                    b.Property<string>("UserName");

                    b.HasKey("Mac");

                    b.HasIndex("UserName");

                    b.ToTable("Plug");
                });

            modelBuilder.Entity("BackEnd.Models.PowerUsageSample", b =>
                {
                    b.Property<DateTime>("SampleDate");

                    b.Property<double>("Current");

                    b.Property<string>("PlugMac");

                    b.Property<double>("Voltage");

                    b.HasKey("SampleDate");

                    b.HasIndex("PlugMac");

                    b.ToTable("PowerUsageSample");
                });

            modelBuilder.Entity("BackEnd.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeviceMac");

                    b.Property<int>("Operation");

                    b.HasKey("TaskId");

                    b.HasIndex("DeviceMac");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("BackEnd.Models.User", b =>
                {
                    b.Property<string>("UserName")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackEnd.Models.Plug", b =>
                {
                    b.HasOne("BackEnd.Models.User")
                        .WithMany("Plugs")
                        .HasForeignKey("UserName");
                });

            modelBuilder.Entity("BackEnd.Models.PowerUsageSample", b =>
                {
                    b.HasOne("BackEnd.Models.Plug")
                        .WithMany("Samples")
                        .HasForeignKey("PlugMac");
                });

            modelBuilder.Entity("BackEnd.Models.Task", b =>
                {
                    b.HasOne("BackEnd.Models.Plug", "Device")
                        .WithMany("Tasks")
                        .HasForeignKey("DeviceMac");
                });
#pragma warning restore 612, 618
        }
    }
}
