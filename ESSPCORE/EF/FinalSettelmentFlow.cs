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
    
    public partial class FinalSettelmentFlow
    {
        public int PFSFlowID { get; set; }
        public Nullable<int> PayrollPeriodID { get; set; }
        public Nullable<int> FinalSettelmentID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<int> SubmittedTo { get; set; }
        public Nullable<System.DateTime> SubmittedDateTime { get; set; }
        public Nullable<int> FSStatusID { get; set; }
        public string Remarks { get; set; }
    }
}
