//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DashboardCurrentData
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public Nullable<int> CurrentMonthScore { get; set; }
        public Nullable<int> PreviousMonthScore { get; set; }
        public Nullable<int> CurrentMonthNegativeEvents { get; set; }
        public Nullable<int> PreviousMonthNegativeEvents { get; set; }
        public Nullable<int> CurrentMonthDayTrips { get; set; }
        public Nullable<int> PreviousMonthDayTrips { get; set; }
        public Nullable<double> CurrentMonthDayMileage { get; set; }
        public Nullable<double> PreviousMonthDayMileage { get; set; }
        public Nullable<int> HardBreaks { get; set; }
        public Nullable<int> HardAccelerations { get; set; }
        public Nullable<int> OverSpeed { get; set; }
        public Nullable<double> Day_NegativeEvents { get; set; }
        public Nullable<double> Midnight_NegativeEvents { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<bool> Deleted { get; set; }

    }
}