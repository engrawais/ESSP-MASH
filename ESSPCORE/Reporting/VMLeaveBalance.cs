using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Reporting
{
    public class VMLeaveBalance : VHR_EmployeeProfile
    {
        public double? TotalCL { get; set; }
        public double? AvailCL { get; set; }
        public double? BalanceCL { get; set; }
        public double? TotalSL { get; set; }
        public double? AvailSL { get; set; }
        public double? BalanceSL { get; set; }
        public double? BalanceEAL { get; set; }
        public double? BalanceCME { get; set; }
        public double? TotalCPL { get; set; }
        public double? TotalEAL { get; set; }
        public double? TotalCME { get; set; }
        public double? AvailCPL { get; set; }
        public double? AvailEAL { get; set; }
        public double? AvailCME { get; set; }
        public double? BalanceCPL { get; set; }
        public double? TotalAL { get; set; }
        public double? AvailAL { get; set; }
        public double? BalanceAL { get; set; }
        public double? ProrataAL { get; set; }
        public double? TotalAccum { get; set; }
        public double? AvailAccum { get; set; }
        public double? AvailHajjLeaves { get; set; }
        public double? BalanceAccum { get; set; }
        public double? ProrataAccum { get; set; }
        public string ServiceLength { get; set; }
        public float LWOP { get; set; }
        public float Absents { get; set; }

    }
    //public class VMEmployeeAttendanceOther : VAT_DailyAttendanceOther
    //{
    //    public DateTime? TimeIn { get; set; }
    //    public DateTime? TimeOut { get; set; }
    //    public short WorkMins { get; set; }
    //}
}
