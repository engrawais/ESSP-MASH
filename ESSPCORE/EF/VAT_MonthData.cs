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
    
    public partial class VAT_MonthData
    {
        public Nullable<int> EmployeeID { get; set; }
        public string OEmpID { get; set; }
        public long PMonthDataID { get; set; }
        public string MonthDataStageID { get; set; }
        public Nullable<double> TotalDays { get; set; }
        public Nullable<double> WorkDays { get; set; }
        public Nullable<double> PresentDays { get; set; }
        public Nullable<double> AbsentDays { get; set; }
        public Nullable<double> RestDays { get; set; }
        public Nullable<double> GZDays { get; set; }
        public Nullable<double> LeaveDays { get; set; }
        public Nullable<double> HPLeaveDays { get; set; }
        public Nullable<double> WOPLeavesDays { get; set; }
        public Nullable<short> TotalShortMins { get; set; }
        public Nullable<double> HalfLeavesDays { get; set; }
        public Nullable<double> HalfAbsentDays { get; set; }
        public Nullable<double> GZPresentDays { get; set; }
        public Nullable<double> RestPresentDays { get; set; }
        public Nullable<double> OfficialDutyDays { get; set; }
        public Nullable<short> TWorkTime { get; set; }
        public Nullable<short> TNOT { get; set; }
        public Nullable<short> TROT { get; set; }
        public Nullable<short> TGZOT { get; set; }
        public Nullable<short> TLateIn { get; set; }
        public Nullable<short> TEarlyOut { get; set; }
        public Nullable<short> TEarlyIn { get; set; }
        public Nullable<short> TLateOut { get; set; }
        public Nullable<short> TotalLossWorkMin { get; set; }
        public Nullable<short> TotalExtraWorkMin { get; set; }
        public Nullable<short> ExpectedWorkTime { get; set; }
        public string PRName { get; set; }
        public Nullable<System.DateTime> PRStartDate { get; set; }
        public Nullable<System.DateTime> PREndDate { get; set; }
        public Nullable<int> PayrollPeriodID { get; set; }
    }
}