﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShareClasses
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SunnyPointEntities : DbContext
    {
        public SunnyPointEntities()
            : base("name=SunnyPointEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountProfiles> AccountProfiles { get; set; }
        public virtual DbSet<DashboardMonthlyData> DashboardMonthlyData { get; set; }
        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<IoTHubs> IoTHubs { get; set; }
        public virtual DbSet<TripPoints> TripPoints { get; set; }
        public virtual DbSet<Trips> Trips { get; set; }
        public virtual DbSet<DashboardCurrentData> DashboardCurrentData { get; set; }
    }
}
