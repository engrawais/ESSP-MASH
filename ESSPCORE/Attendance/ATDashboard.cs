using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public enum UserGraphType
    {
        Single = 1,
        SimpleLM = 2,// For Employee
        HasMultipleOU = 3, // For Organizational Unit
        HasMultipleCommonOU = 4// For Common OU
    }
    public class VMAttendanceDashboard
    {
        public UserGraphType UserGraphType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GraphType { get; set; } // LI,LO,EI,LO
        public int ID { get; set; }
        public int EmpID { get; set; }
    }
    public class VMTMSDetail
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public int NoOfDays { get; set; }
        public int TotalTime { get; set; }
    }
    public class DMParentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameWithDetail { get; set; }
        public int Count { get; set; }
    }
    public class DMPieChartParentModel
    {
        public List<DMParentModel> ChildList { get; set; }
        public List<VAT_DailyAttendance> AttData { get; set; }
        public int ID { get; set; }
        public string HeaderLeft { get; set; }
        public string HeaderRight { get; set; }
        public string HeaderDescription { get; set; }
        public string TBLHeaderID { get; set; }
        public string TBLHeaderName { get; set; }
        public string TBLHeaderCount { get; set; }
    }
    public class VMTimeOfficeDashboard
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<VMPVTMDashboard> VMPVTMDashboard { get; set; }
    }
    public class VMPVTMDashboard
    {
        public string Date { get; set; }
        public DateTime DateValue { get; set; }
        public string AbsentsPercentLabel { get; set; }
        public string AbsentsPercentWidth { get; set; }
        public string AbsentsPercentDesc { get; set; }
        public string AbsentsLink { get; set; }
        public string MissingPercentLabel { get; set; }
        public string MissingPercentWidth { get; set; }
        public string MissingPercentDesc { get; set; }
        public string MissingLink { get; set; }
        public string ShiftLateInPercentLabel { get; set; }
        public string ShiftLateInPercentWidth { get; set; }
        public string ShiftLateInPercentDesc { get; set; }
        public string ShiftLateInLink { get; set; }
        public string ShiftEarlyOutPercentLabel { get; set; }
        public string ShiftEarlyOutPercentWidth { get; set; }
        public string ShiftEarlyOutPercentDesc { get; set; }
        public string ShiftEarlyOutLink { get; set; }
    }
}
