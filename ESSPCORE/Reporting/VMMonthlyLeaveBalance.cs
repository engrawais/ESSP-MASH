using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Reporting
{
 public class VMMonthlyLeaveBalance: VHR_EmployeeProfile
    {
        public double? AvailedMaternity { get; set; }
        public double? AvailedHajjLeaves { get; set; }
        public double? AvailedPaternity { get; set; }
        public double? AvailedLWOP { get; set; }
        public double? AvailedSpecialLeave { get; set; }
        public double? AvailedCL { get; set; }
        public double? BalanceCL { get; set; }
        public double? AvailedSL { get; set; }
        public double? BalanceSL { get; set; }
        public double? AvailedCPL { get; set; }
        public double? BalanceCPL { get; set; }
        public double? AvailedAL { get; set; }
        public double? BalanceAL { get; set; }
        public double? AvailedAccum { get; set; }
        public double? BalanceAccum { get; set; }
        public int EmpID { get; set; }

    }
}
