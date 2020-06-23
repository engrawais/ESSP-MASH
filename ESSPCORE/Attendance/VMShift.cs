using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{

    public class MyShift
    {
        public int ShftID { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DayOff1 { get; set; }
        public int DayOff2 { get; set; }
        public Int16 MonMin { get; set; }
        public Int16 TueMin { get; set; }
        public Int16 WedMin { get; set; }
        public Int16 ThuMin { get; set; }
        public Int16 FriMin { get; set; }
        public Int16 SatMin { get; set; }
        public Int16 SunMin { get; set; }
        public Int16 LateIn { get; set; }
        public Int16 EarlyIn { get; set; }
        public Int16 EarlyOut { get; set; }
        public Int16 LateOut { get; set; }
        public Int16 OverTimeMin { get; set; }
        public Int16 MinHrs { get; set; }
        public bool HasBreak { get; set; }
        public Int16 BreakMin { get; set; }
        public Int16 HalfDayBreakMin { get; set; }
        public bool GZDays { get; set; }
        public bool OpenShift { get; set; }
        public bool RoundOffWorkMin { get; set; }
        public bool SubtractOTFromWork { get; set; }
        public bool SubtractEIFromWork { get; set; }
        public bool AddEIInOT { get; set; }
        public bool PresentAtIN { get; set; }
        public bool CalDiffOnly { get; set; }
    }
}
