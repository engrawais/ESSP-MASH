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
    
    public partial class VAT_RosterDetail
    {
        public Nullable<int> CriteriaData { get; set; }
        public string CrewName { get; set; }
        public string ShiftName { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<byte> RotaTypeID { get; set; }
        public string LocationName { get; set; }
        public string RosterTypeName { get; set; }
        public Nullable<System.DateTime> DateStarted { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> RotaAppID { get; set; }
        public Nullable<System.DateTime> DateEnded { get; set; }
        public Nullable<short> ShiftID { get; set; }
        public Nullable<System.DateTime> RosterDate { get; set; }
        public Nullable<short> WorkMin { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.TimeSpan> DutyTime { get; set; }
        public string DutyCode { get; set; }
        public string CriteriaValueDate { get; set; }
    }
}
