//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESSPCORE.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class OTPolicy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OTPolicy()
        {
            this.Employees = new HashSet<Employee>();
        }
    
        public short POTPolicyID { get; set; }
        public string OTPolicyName { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<bool> CalculateNOT { get; set; }
        public Nullable<bool> CalculateGZOT { get; set; }
        public Nullable<bool> CalculateRestOT { get; set; }
        public Nullable<double> PerDayOTLimitHour { get; set; }
        public Nullable<double> PerDayROTLimitHour { get; set; }
        public Nullable<double> PerDayGOTLimitHour { get; set; }
        public Nullable<double> PerMonthOTLimitHour { get; set; }
        public Nullable<double> ROTMultiplier { get; set; }
        public Nullable<double> PerMonthROTLimitHour { get; set; }
        public Nullable<double> GOTMultiplier { get; set; }
        public Nullable<double> PerMonthGOTLimitHour { get; set; }
        public Nullable<short> MinMinutesForOneHour { get; set; }
        public Nullable<bool> ConvertedToCPL { get; set; }
        public Nullable<short> EncashableOTHour { get; set; }
        public Nullable<short> CPLExpireDays { get; set; }
        public Nullable<bool> ROTEncashable { get; set; }
        public Nullable<bool> AddROTInTotalOT { get; set; }
        public Nullable<bool> GZEncashable { get; set; }
        public Nullable<bool> AddGZOTInTotalOT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
