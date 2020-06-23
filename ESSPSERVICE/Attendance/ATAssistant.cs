using ESSPCORE.Attendance;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{/// <summary>
/// Static class for the Attendance 
/// </summary>
   public static  class ATAssistant
    {
        /// <summary>
        /// Gets the payroll period Start date for the reports that are generated monthly from payroll start date to payroll end date.
        /// Also to check that the leave is applied within the opened payroll period.
        /// </summary>
        /// <param name="dateFrom">Date to be selected fron the reports filters.</param>
        /// <param name="list">List of all the payroll periods.</param>
        /// <returns></returns>
        public static int GetPayrollPeriodIDStart(DateTime dateFrom, List<PayrollPeriod> list)
        {
            foreach (var item in list)
            {
                if (dateFrom == item.PRStartDate)
                    return item.PPayrollPeriodID;
            }
            return 0;
        }
        /// <summary>
        /// End Date of the payroll period for the reporting to be precise from the start and to the end of the payroll period
        /// Also to check that the leave is applied within the opened payroll period
        /// </summary>
        /// <param name="dateFrom">Date filter to be given in the reports</param>
        /// <param name="list">List of all the payroll Periods.</param>
        /// <returns></returns>
        public static int GetPayrollPeriodIDEnd(DateTime dateFrom, List<PayrollPeriod> list)
        {
            foreach (var item in list)
            {
                if (dateFrom == item.PREndDate)
                    return item.PPayrollPeriodID;
            }
            return 0;
        }
        /// <summary>
        /// If employee don't apply the date filter then Payroll period start date and end date will automatically be assigned to him.
        /// 
        /// </summary>
        /// <param name="today">Parameter for the todays date</param>
        /// <param name="list">List of all the payroll periods from the database.</param>
        /// <returns></returns>
        public static PayrollPeriod GetPayrollPeriodObject(DateTime today, List<PayrollPeriod> list)
        {
            if(list.Where(aa=> today >= aa.PRStartDate && today <= aa.PREndDate).Count()>0)
            {
                return list.Where(aa => today >= aa.PRStartDate && today <= aa.PREndDate).First();
            }
            else
            {
                PayrollPeriod prp = new PayrollPeriod();
                prp.PRStartDate = DateTime.Today.AddDays(-1);
                prp.PREndDate = DateTime.Now;
                return prp;
            }
        }
        /// <summary>
        /// To be used in the Roster to convert the time to the the hours 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;

                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    count++;
                    if (count == 1)
                    {
                        hour = p.Substring(i, chunkSize);
                    }
                    if (count == 2)
                    {
                        min = p.Substring(i, chunkSize);
                    }
                    if (i + chunkSize > stringLength)
                    {
                        chunkSize = stringLength - i;
                    }
                }
                TimeSpan _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }
        /// <summary>
        /// Getting Finanacial Year for different puposes like for reportingleaves 
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int GetFinancialYearID(DateTime dateTo, List<FinancialYear> list)
        {
            if (list.Where(aa =>  dateTo >= aa.FYStartDate && dateTo <=aa.FYEndDate ).Count() > 0)
            {
                return list.Where(aa => dateTo >= aa.FYStartDate && dateTo <= aa.FYEndDate).First().PFinancialYearID;
            }
            else
            {
                return 0;
            }
        }

        #region -- Helper Function--

        public static string ReturnDayOfWeek(DayOfWeek dayOfWeek)
            {
                string _DayName = "";
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _DayName = "Monday";
                        break;
                    case DayOfWeek.Tuesday:
                        _DayName = "Tuesday";
                        break;
                    case DayOfWeek.Wednesday:
                        _DayName = "Wednesday";
                        break;
                    case DayOfWeek.Thursday:
                        _DayName = "Thursday";
                        break;
                    case DayOfWeek.Friday:
                        _DayName = "Friday";
                        break;
                    case DayOfWeek.Saturday:
                        _DayName = "Saturday";
                        break;
                    case DayOfWeek.Sunday:
                        _DayName = "Sunday";
                        break;
                }
                return _DayName;
            }

            public static TimeSpan CalculateShiftEndTime(Shift shift, DayOfWeek dayOfWeek)
            {
                Int16 workMins = 0;
                try
                {
                    switch (dayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            workMins = shift.MonMin;
                            break;
                        case DayOfWeek.Tuesday:
                            workMins = shift.TueMin;
                            break;
                        case DayOfWeek.Wednesday:
                            workMins = shift.WedMin;
                            break;
                        case DayOfWeek.Thursday:
                            workMins = shift.ThuMin;
                            break;
                        case DayOfWeek.Friday:
                            workMins = shift.FriMin;
                            break;
                        case DayOfWeek.Saturday:
                            workMins = shift.SatMin;
                            break;
                        case DayOfWeek.Sunday:
                            workMins = shift.SunMin;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return shift.StartTime + (new TimeSpan(0, workMins, 0));
            }

        internal static int GetTimeHours(short? actualOT)
        {
            if (actualOT > 0)
            {
                TimeSpan ts = new TimeSpan(0,(int)actualOT,0);
                return (int)ts.TotalHours;
            }
            else return 0;
        }

        public static Int16 CalculateShiftMinutes(MyShift shift, DayOfWeek dayOfWeek)
            {
                Int16 workMins = 0;
                try
                {
                    switch (dayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            workMins = shift.MonMin;
                            break;
                        case DayOfWeek.Tuesday:
                            workMins = shift.TueMin;
                            break;
                        case DayOfWeek.Wednesday:
                            workMins = shift.WedMin;
                            break;
                        case DayOfWeek.Thursday:
                            workMins = shift.ThuMin;
                            break;
                        case DayOfWeek.Friday:
                            workMins = shift.FriMin;
                            break;
                        case DayOfWeek.Saturday:
                            workMins = shift.SatMin;
                            break;
                        case DayOfWeek.Sunday:
                            workMins = shift.SunMin;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return workMins;
            }

       

        public static DateTime CalculateShiftEndTime(Shift shift, DateTime _AttDate, TimeSpan _DutyTime)
            {
                Int16 workMins = 0;
                try
                {
                    switch (_AttDate.Date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            workMins = shift.MonMin;
                            break;
                        case DayOfWeek.Tuesday:
                            workMins = shift.TueMin;
                            break;
                        case DayOfWeek.Wednesday:
                            workMins = shift.WedMin;
                            break;
                        case DayOfWeek.Thursday:
                            workMins = shift.ThuMin;
                            break;
                        case DayOfWeek.Friday:
                            workMins = shift.FriMin;
                            break;
                        case DayOfWeek.Saturday:
                            workMins = shift.SatMin;
                            break;
                        case DayOfWeek.Sunday:
                            workMins = shift.SunMin;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                DateTime _datetime = new DateTime();
                TimeSpan _Time = new TimeSpan(0, workMins, 0);
                _datetime = _AttDate.Date.Add(_DutyTime);
                _datetime = _datetime.Add(_Time);
                return _datetime;
            }

            #endregion
        /// <summary>
        /// No Reference Found neither any usage is dicovered.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
            public static int ReturnDayNoOfWeek(DayOfWeek dayOfWeek)
            {
                int _DayName = 0;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _DayName = 2;
                        break;
                    case DayOfWeek.Tuesday:
                        _DayName = 3;
                        break;
                    case DayOfWeek.Wednesday:
                        _DayName = 4;
                        break;
                    case DayOfWeek.Thursday:
                        _DayName = 5;
                        break;
                    case DayOfWeek.Friday:
                        _DayName = 6;
                        break;
                    case DayOfWeek.Saturday:
                        _DayName = 7;
                        break;
                    case DayOfWeek.Sunday:
                        _DayName = 1;
                        break;
                }
                return _DayName;
            }
        /// <summary>
        /// Calculate the shifts end time in the att funtion 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <param name="shiftMins"></param>
        /// <returns></returns>
            internal static DateTime CalculateShiftEndTimeWithAttData(DateTime dateTime, TimeSpan timeSpan, short? shiftMins)
            {
                DateTime _datetime = new DateTime();
                int shiftMin = (int)shiftMins;
                TimeSpan _Time = new TimeSpan(0, shiftMin, 0);
                _datetime = dateTime.Date.Add(timeSpan);
                _datetime = _datetime.Add(_Time);
                return _datetime;
            }
        /// <summary>
        /// If the employee changes his/her Shift then employee shift change function is called.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="empshiftCh"></param>
        /// <param name="currentDate"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        internal static Shift GetEmployeeChangedShift(Employee emp, List<ShiftChangedEmp> empshiftCh, DateTime currentDate, List<Shift> shifts)
            {
            Shift shift = emp.Shift;
                foreach (var item in empshiftCh)
                {
                    if (item.EndDate == null)
                    {
                        if (currentDate >= item.StartDate)
                        {
                            shift = shifts.First(aa => aa.PShiftID == item.ShiftID);
                        }
                    }
                    else
                    {
                        if (currentDate >= item.StartDate && currentDate <= item.EndDate)
                        {
                            shift = shifts.First(aa => aa.PShiftID == item.ShiftID);
                        }

                    }
                }
                return shift;
            }
        /// <summary>
        /// Gets all the information of the employee from shift form that is related to him.
        /// </summary>
        /// <param name="att_Shift">objects for the shift </param>
        /// <returns></returns>
            public static MyShift GetEmployeeShift(Shift att_Shift)
            {
                MyShift shift = new MyShift();
                shift.ShftID = att_Shift.PShiftID;
                shift.StartTime = att_Shift.StartTime;
                shift.DayOff1 = att_Shift.DayOff1;
                shift.DayOff2 = att_Shift.DayOff2;
                shift.MonMin = att_Shift.MonMin;
                shift.TueMin = att_Shift.TueMin;
                shift.WedMin = att_Shift.WedMin;
                shift.ThuMin = att_Shift.ThuMin;
                shift.FriMin = att_Shift.FriMin;
                shift.SatMin = att_Shift.SatMin;
                shift.SunMin = att_Shift.SunMin;
                shift.LateIn = (short)att_Shift.LateIn;
                shift.EarlyIn = (short)att_Shift.EarlyIn;
                shift.EarlyOut = (short)att_Shift.EarlyOut;
                shift.LateOut = (short)att_Shift.LateOut;
                shift.OverTimeMin = (short)att_Shift.OverTimeMin;
                shift.MinHrs = (short)att_Shift.MinHrs;
                shift.HasBreak = (bool)att_Shift.HasBreak;
                shift.BreakMin = (short)att_Shift.BreakMin;
                shift.HalfDayBreakMin = (short)att_Shift.HalfDayBreakMin;
                shift.GZDays = (bool)att_Shift.GZDays;
                shift.OpenShift = (bool)att_Shift.OpenShift;
                shift.RoundOffWorkMin = (bool)att_Shift.RoundOffWorkMin;
                shift.SubtractOTFromWork = (bool)att_Shift.SubtractOTFromWork;
                shift.SubtractEIFromWork = (bool)att_Shift.SubtractEIFromWork;
                shift.AddEIInOT = (bool)att_Shift.AddEIInOT;
                shift.PresentAtIN = (bool)att_Shift.PresentAtIN;
                shift.CalDiffOnly = (bool)att_Shift.CalDiffOnly;

                return shift;
            }
        /// <summary>
        /// Gets the payroll period from the database. 
        /// </summary>
        /// <param name="dts">Gets the date time for the payroll period </param>
        /// <param name="periods">Gets list of all the payroll periods </param>
        /// <returns></returns>
        internal static PayrollPeriod GetPayrollPeriod(DateTime dts, List<PayrollPeriod> periods)
        {
            return periods.Where(aa => dts  >= aa.PRStartDate && dts <= aa.PREndDate).FirstOrDefault();
        }
    }
}
