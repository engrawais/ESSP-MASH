using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.ESSP
{
    public class VMESSPJobCard
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFrom{ get; set; }
        public DateTime DateTo { get; set; }
        public int JobCardApplicationID { get; set; }
        public string JobCardTypeName { get; set; }
        public string JobCardStatusName { get; set; }
        public string LineManagerID { get; set; }
        public string LineManagerName { get; set; }
    }
    public class VMESSPAttendence
    {
        public int? EmpID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double? WorkingDays { get; set; }
        public double? PresentDays { get; set; }
        public double? AbsentDays { get; set; }
        public double? LeaveDays { get; set; }
        public double? RestDays { get; set; }
        public int LateInDays { get; set; }
        public int EarlyOutDays { get; set; }
        public List<DailyAttendance> VMDailyAttendence { get; set; }
    }
}
