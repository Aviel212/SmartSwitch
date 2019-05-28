﻿// <auto-generated />
using System;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackEnd.Migrations
{
    [DbContext(typeof(SmartSwitchDbContext))]
    partial class SmartSwitchDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BackEnd.Models.Plug", b =>
                {
                    b.Property<string>("Mac")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<bool>("Approved");

                    b.Property<bool>("IsOn");

                    b.Property<string>("Nickname");

                    b.Property<int>("Priority");

                    b.Property<string>("Username");

                    b.HasKey("Mac");

                    b.HasIndex("Username");

                    b.ToTable("Plugs");
                });

            modelBuilder.Entity("BackEnd.Models.PowerUsageSample", b =>
                {
                    b.Property<DateTime>("SampleDate");

                    b.Property<double>("Current");

                    b.Property<string>("PlugMac");

                    b.Property<double>("Voltage");

                    b.HasKey("SampleDate");

                    b.HasIndex("PlugMac");

                    b.ToTable("PowerUsageSamples");
                });

            modelBuilder.Entity("BackEnd.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeviceMac");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("Operation");

                    b.Property<string>("PlugMac");

                    b.HasKey("TaskId");

                    b.HasIndex("PlugMac");

                    b.ToTable("Tasks");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Task");
                });

            modelBuilder.Entity("BackEnd.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackEnd.Models.OneTimeTask", b =>
                {
                    b.HasBaseType("BackEnd.Models.Task");

                    b.Property<DateTime>("DateToBeExecuted");

                    b.HasDiscriminator().HasValue("OneTimeTask");
                });

            modelBuilder.Entity("BackEnd.Models.RepeatedTask", b =>
                {
                    b.HasBaseType("BackEnd.Models.Task");

                    b.HasDiscriminator().HasValue("RepeatedTask");
                });

            modelBuilder.Entity("BackEnd.Models.Plug", b =>
                {
                    b.HasOne("BackEnd.Models.User")
                        .WithMany("Plugs")
                        .HasForeignKey("Username");
                });

            modelBuilder.Entity("BackEnd.Models.PowerUsageSample", b =>
                {
                    b.HasOne("BackEnd.Models.Plug")
                        .WithMany("Samples")
                        .HasForeignKey("PlugMac");
                });

            modelBuilder.Entity("BackEnd.Models.Task", b =>
                {
                    b.HasOne("BackEnd.Models.Plug")
                        .WithMany("Tasks")
                        .HasForeignKey("PlugMac");
                });
#pragma warning restore 612, 618
        }
    }
}
