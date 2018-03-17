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
    
    public partial class TripPoints
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TripPoints()
        {
            this.Deleted = false;
        }
    
        public int Id { get; set; }
        public string TripID { get; set; }
        public string CarID { get; set; }
        public string IoTHubDeviceID { get; set; }
        public string MessageType { get; set; }
        public Nullable<long> RecordedTimeStamp { get; set; }
        public string Country { get; set; }
        public Nullable<System.DateTime> LocalTime { get; set; }
        public Nullable<bool> MidNightDrive { get; set; }
        public Nullable<decimal> Altitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<double> Speed { get; set; }
        public Nullable<double> SpeedLimit { get; set; }
        public Nullable<bool> OverSpeed { get; set; }
        public Nullable<double> RPM { get; set; }
        public Nullable<double> AccelerationX { get; set; }
        public Nullable<double> AccelerationY { get; set; }
        public Nullable<double> AccelerationZ { get; set; }
        public Nullable<double> AccelerationXYZ { get; set; }
        public Nullable<double> MAF { get; set; }
        public Nullable<double> Temp { get; set; }
        public Nullable<long> IdlingStartTime { get; set; }
        public Nullable<long> IdlingEndTime { get; set; }
        public Nullable<long> IdlingTime { get; set; }
        public Nullable<long> TripStartTime { get; set; }
        public Nullable<long> TripEndTime { get; set; }
        public Nullable<long> TripTime { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<bool> Deleted { get; set; }
    }
}
