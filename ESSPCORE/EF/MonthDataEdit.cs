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
    
    public partial class MonthDataEdit
    {
        public int PMonthDataEditID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> PayrollPeriodID { get; set; }
        public Nullable<System.DateTime> DataEditDate { get; set; }
        public Nullable<double> OldTotalDays { get; set; }
        public Nullable<double> OldPaidDays { get; set; }
        public Nullable<double> OldAbsentDays { get; set; }
        public Nullable<double> NewTotalDays { get; set; }
        public Nullable<double> NewPaidDays { get; set; }
        public Nullable<double> NewAbsentDays { get; set; }
        public string NewRemarks { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}