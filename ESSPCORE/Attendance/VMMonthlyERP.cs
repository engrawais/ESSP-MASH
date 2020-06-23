using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
  public class VMMonthlyERP
    {
        public int EmpID { get; set; }
        public string OEmpID { get; set; }
        public string EmpName { get; set; }
        public string OUName { get; set; }
        public string LocationName { get; set; }
        public string DesignationName { get; set; }
        public float LWOPDays { get; set; }
        public int SingleOT { get; set; }
        public int DoubleOT { get; set; }
        public string Status { get; set; }

    }
    public class VMOTStatusManager
    {
        public int? LocationID { get; set; }
        public string LocationName { get; set; }
        public int? PayrollPeriodID { get; set; }
        public int? TotalEmps { get; set; }
        public int? PendingAtTM { get; set; }
        public int? PendingAtHO { get; set; }
        public int? Approved { get; set; }
        public int? Reject { get; set; }
    }
}
