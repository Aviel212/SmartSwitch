﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerSide
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartSwitchDbContext : DbContext
    {
        public SmartSwitchDbContext()
            : base("name=SmartSwitchDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tblOneTimeTask> tblOneTimeTasks { get; set; }
        public virtual DbSet<tblPlug> tblPlugs { get; set; }
        public virtual DbSet<tblPowerUsageSample> tblPowerUsageSamples { get; set; }
        public virtual DbSet<tblRepeatedTask> tblRepeatedTasks { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
    }
}
