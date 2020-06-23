using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class VMOvertimeEncashableSelection : VMEmpSelection
    {
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
    }
    public class VMOvertimeEncashable
    {
        public List<VMOvertimeEncashableChild> OvertimeEncashableChild { get; set; }
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
        public int OvertimePolicyID { get; set; }
        public string OvertimePolicyName { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
    public class VMOvertimeEncashableChild
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public int ApprovedOT { get; set; }
        public int EncashableOT { get; set; }
        public int CPLConvertedOT { get; set; }
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
        public int? OvertimePolicyID { get; set; }
        public string OvertimePolicyName { get; set; }

    }

}
