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
    
    public partial class DailyAttendanceEmailLog
    {
        public int PDailyAttendanceEmailLogID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string EmailID { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> EmailSent { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public string EmpName { get; set; }
        public string Remarks { get; set; }
    }
}
