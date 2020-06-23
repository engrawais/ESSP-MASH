using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class VMLeaveQuotaSelection : VMEmpSelection
    {
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }
        public string OvertimePolicyName { get; set; }
    }
    public  class VMLeaveQuota
    {
        public List<VMLeaveQuotaChild> LeaveQuotaChild { get; set; }
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
    public class VMLeaveQuotaChild
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitleName { get; set; }
        public string DOJ { get; set; }
        public float CL { get; set; }
        public float AL { get; set; }
        public float SL { get; set; }
        public float EAL { get; set; }
        public float CME { get; set; }
        public float CPL { get; set; }
        public float ACCL { get; set; }
        public float ALBalance { get; set; }
        public float CollapseLeave { get; set; }
        public float CarryForward { get; set; }
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }

    }
    
}
