using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{
    public class VMRosterApplication
    {
        public int RotaApplD { get; set; }
        public string RosterName { get; set; }
        public Nullable<System.DateTime> DateStarted { get; set; }
        public Nullable<System.DateTime> DateEnded { get; set; }
        public int RosterTypeID { get; set; }
        public int CrewID { get; set; }
        public int ShiftID { get; set; }
        public int MonMin { get; set; }
        public int TueMin { get; set; }
        public int WedMin { get; set; }
        public int ThruMin { get; set; }
        public int FriMin { get; set; }
        public int SatMin { get; set; }
        public int SunMin { get; set; }
        public string MonStartTime { get; set; }
        public string TueStartTime { get; set; }
        public string WedStartTime { get; set; }
        public string ThruStartTime { get; set; }
        public string FriStartTime { get; set; }
        public string SatStartTime { get; set; }
        public string SunStartTime { get; set; }
    }
    public class VMRosterModel
    {
        public List<RosterAttributes> _RosterAttributes { get; set; }
        public string Criteria { get; set; }
        public int CriteriaValue { get; set; }
        public string CriteriaValueName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfDays { get; set; }
        public int RotaAppID { get; set; }
    }
    public class VMRosterContinue
    {
        public int RotaAppID { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class RosterAttributes
    {
        public int ID { get; set; }
        public string DateString { get; set; }
        public string Day { get; set; }
        public DateTime DutyDate { get; set; }
        public int WorkMin { get; set; }
        public string DutyTimeString { get; set; }
        public TimeSpan DutyTime { get; set; }
    }

    public class RosterDetailModel
    {
        public List<RosterDetailAttributes> _RosterAttributes { get; set; }
        public string Criteria { get; set; }
        public int CriteriaValue { get; set; }
        public string CriteriaValueName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateTime StartDate { get; set; }
        public int NoOfDays { get; set; }
        public int RotaAppID { get; set; }
    }
    public class RosterDetailAttributes
    {
        public string CriteriaValueDate { get; set; }
        public string Day { get; set; }
        public DateTime DutyDate { get; set; }
        public int WorkMin { get; set; }
        public TimeSpan DutyTime { get; set; }
        public string DutyCode { get; set; }
        public bool Changed { get; set; }
    }


}
