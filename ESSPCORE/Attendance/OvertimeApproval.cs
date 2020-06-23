using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class VMOvertimeApprovalSelection : VMEmpSelection
    {
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
    }
    public class VMOvertimeApproval
    {
        public List<VMOvertimeApprovalChild> OvertimeApprovalChild { get; set; }
        public int PayrollPeriodID { get; set; }

        public string PayrollPeriodName { get; set; }
        public int OvertimePolicyID { get; set; }
        public string OvertimePolicyName { get; set; }
        public List<string> ErrorMessages { get; set; }
        public int SubmittedToUserID { get; set; }
        public string OTStatusID { get; set; }

    }
    public class VMOvertimeApprovalChild
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public int NormalOT { get; set; }
        public int RestOT { get; set; }
        public int GZOT { get; set; }
        public int EncashableSingleOT { get; set; }
        public int EncashableDoubleOT { get; set; }
        public float Absents { get; set; }
        public float PaidDays { get; set; }
        public int CPLConvertedOT { get; set; }
        public double? CPLConvertedOTDays { get; set; }
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
        public int? OvertimePolicyID { get; set; }
        public string OvertimePolicyName { get; set; }
        public string StatusID { get; set; }

    }

}
