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
    
    public partial class VAT_LeaveApplicationFlow
    {
        public int PESSPLeaveApplicationID { get; set; }
        public int LeaveAppID { get; set; }
        public int SubmittedByUserID { get; set; }
        public int SubmittedToUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LeaveStageID { get; set; }
        public string Remarks { get; set; }
        public Nullable<byte> LeaveTypeID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string LeaveStageName { get; set; }
        public string ByEmployeeName { get; set; }
        public Nullable<int> ByEmpID { get; set; }
        public string ToEmployeeName { get; set; }
        public Nullable<int> ToEmpID { get; set; }
    }
}
