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
    
    public partial class DailyOvertime
    {
        public int PDailyOTID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> OTDate { get; set; }
        public Nullable<int> SingleEncashableOT { get; set; }
        public Nullable<int> DoubleEncashbaleOT { get; set; }
        public Nullable<int> CPLOT { get; set; }
        public Nullable<int> PayrollPeriodID { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> AddedByUserID { get; set; }
        public string EmpDate { get; set; }
    }
}
