using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class AttEditSingleEmployee
    {
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string OUName { get; set; }
        public string JobTitleName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<EditAttendanceListDateWise> list { get; set; }
    }
    public class EditAttendanceList
    {
        public int No { get; set; }
        public string EmpDate { get; set; }
        public string Date { get; set; }
        public string DutyCode { get; set; }
        public TimeSpan DutyTime { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public TimeSpan WorkMinutes { get; set; }
        public TimeSpan LateIn { get; set; }
        public TimeSpan LateOut { get; set; }
        public TimeSpan EarlyIn { get; set; }
        public TimeSpan EarlyOut { get; set; }
        public TimeSpan BreakMin { get; set; }
        public TimeSpan ShiftTime { get; set; }
        public TimeSpan OTMin { get; set; }
        public string Remarks { get; set; }
    }
    public class VMEditAttendanceDateWise
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public string Criteria { get; set; }
        public string CriteriaRBName { get; set; }
        public int CriteriaData { get; set; }
        public string CriteriaDataName { get; set; }
        public List<EditAttendanceListDateWise> list { get; set; }
    }
    public class EditAttendanceListDateWise
    {
        public int No { get; set; }
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string EmpDate { get; set; }
        public string Date { get; set; }
        public string DutyCode { get; set; }
        public string DutyTime { get; set; }
        public string ShiftTime { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string WorkMinutes { get; set; }
        public string OTMin { get; set; }
        public string ApprovedOTMin { get; set; }
        public string SystemRemarks { get; set; }
        public string UserRemarks { get; set; }
    }
    public class VMEditMonthlyCreate: VMEmpSelection
    {
        public int PayrolPeriodID { get; set; }
    }
    public class VMEditMonthlyAttendance
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public int PayrolPeriodID { get; set; }
        public string PayrolPeriodName { get; set; }
        public List<EditMonthlyAttendanceList> MonthlyList { get; set; }
    }
    public class EditMonthlyAttendanceList
    {
        public int No { get; set; }
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string JobTitleName { get; set; }
        public float TotalDays { get; set; }
        public float PaidDays { get; set; }
        public float AbsentDays { get; set; }
        public float RestDays { get; set; }
        public float LeaveDays { get; set; }
        public int SingleEncashableOT { get; set; }
        public int DoubleEncashableOT { get; set; }
        public int CPLOT { get; set; }
        public string Remarks { get; set; }
        public float LWOPDays { get; set; }
    }
    public class VMAbsentList
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string OUname { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public float AbDays { get; set; }
    }
}

