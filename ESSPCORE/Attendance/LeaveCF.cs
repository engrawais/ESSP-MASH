using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class VMLeaveCFSelection : VMEmpSelection
    {
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }
    }
    public class VMLeaveCF
    {
        public List<VMLeaveCFChild> LeaveCFChild { get; set; }
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
    public class VMLeaveCFChild
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypeName { get; set; }
        public float TotalLeave { get; set; }
        public float CollapseLeave { get; set; }
        public float CarryForward { get; set; }
        public int FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }

    }
}
