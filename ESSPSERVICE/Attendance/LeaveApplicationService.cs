using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    //Leave Service for the time Office user to apply for the other employees.
    public class LeaveApplicationService : ILeaveApplicationService
    {
        IDDService DDService;
        IRepository<LeaveQuotaYear> LeaveQuotaYearRepo;
        IRepository<LeaveQuotaPeriod> LeaveQuotaPeriodRepo;
        IRepository<LeaveApplication> LeaveAppRepo;
        IRepository<LeaveData> LeaveDataRepo;
        IRepository<Employee> EmployeeRepo;
        IRepository<LeaveCPLEmpBalance> LeaveCPLEmpBalanceRepo;
        IRepository<VAT_RosterDetail> RosterDetailRepo;
        IRepository<LeaveApplicationFlow> ApplicationFlowRepository;
        public LeaveApplicationService(IDDService dDService, IRepository<LeaveQuotaYear> leaveQuotaYearService,
       IRepository<LeaveQuotaPeriod> leaveQuotaPeriodService,
       IRepository<Employee> employeeService, IRepository<LeaveApplication> leaveAppService,
       IRepository<LeaveData> leaveDataService, IRepository<VAT_RosterDetail> rosterDetailRepo, IRepository<LeaveApplicationFlow> applicationFlowRepository,
       IRepository<LeaveCPLEmpBalance> leaveCPLBalanceRepo)
        {
            DDService = dDService;
            LeaveQuotaYearRepo = leaveQuotaYearService;
            LeaveQuotaPeriodRepo = leaveQuotaPeriodService;
            EmployeeRepo = employeeService;
            LeaveAppRepo = leaveAppService;
            LeaveDataRepo = leaveDataService;
            RosterDetailRepo = rosterDetailRepo;
            ApplicationFlowRepository = applicationFlowRepository;
            LeaveCPLEmpBalanceRepo = leaveCPLBalanceRepo;
        }

        public void CreateLeave(LeaveApplication lvapplication, LeaveType lvType, VMLoggedUser user, LeavePolicy lvPolicy)
        {
            try
            {
                //Saves the Leave application vlaue in the leave application table on creation of the leave
                lvapplication.LeaveDate = DateTime.Today;
                int _userID = user.PUserID;
                lvapplication.CreatedBy = _userID;
                lvapplication.Active = true;
                LeaveAppRepo.Add(lvapplication);
                LeaveAppRepo.Save();
                //Creates the leave Data of the leave application to be implemented in the leave application
                AddLeaveToLeaveData(lvapplication, lvType, lvPolicy);
                //Checks for the leave application balance 
                BalanceLeaves(lvapplication, lvType, AssistantLeave.GetPayRollPeriodID(DDService.GetPayrollPeriod().ToList(), lvapplication.FromDate));
                LeaveAppRepo.Edit(lvapplication);
                LeaveAppRepo.Save();
                AddLeaveToAttData(lvapplication, lvType);
            }
            catch (Exception)
            {
            }
        }
        #region -- Add Leaves--
        //Check Duplication of Leave for a date
        /// <summary>
        /// Checks the duplicate leave.
        /// </summary>
        /// <param name="lvappl">The lvappl.</param>
        /// <returns></returns>
        public bool CheckDuplicateLeave(LeaveApplication lvappl)
        {
            List<LeaveApplication> _Lv = new List<LeaveApplication>();
            DateTime _DTime = new DateTime();
            DateTime _DTimeLV = new DateTime();
            //Check for the leave that they are already applied or not and if leave stage is rejected then it will allow the leave to be created.
            Expression<Func<LeaveApplication, bool>> SpecificEntries = aa => aa.EmpID == lvappl.EmpID && aa.LeaveStageID != "R";
            _Lv = LeaveAppRepo.FindBy(SpecificEntries);
            foreach (var item in _Lv)
            {
                _DTime = item.FromDate;
                _DTimeLV = lvappl.FromDate;
                while (_DTime <= item.ToDate)
                {
                    while (_DTimeLV <= lvappl.ToDate)
                    {
                        if (_DTime == _DTimeLV)
                            return false;
                        _DTimeLV = _DTimeLV.AddDays(1);
                    }
                    _DTime = _DTime.AddDays(1);
                }
            }
            return true;
        }

        public bool CheckLeaveBalance(LeaveApplication _lvapp, LeavePolicy LeaveType)
        {
            bool balance = false;
            decimal RemainingLeaves;
            if (LeaveType.UpdateBalance == true)
            {
                //Get the list of leave quota year in which balance of the leave is present .in the specific Financial year
                List<LeaveQuotaYear> _lvConsumed = new List<LeaveQuotaYear>();
                Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = aa => aa.EmployeeID == _lvapp.EmpID && aa.FinancialYearID == _lvapp.FinancialYearID && aa.LeaveTypeID == _lvapp.LeaveTypeID;
                _lvConsumed = LeaveQuotaYearRepo.FindBy(SpecificEntries);
                if (_lvConsumed.Count > 0)
                {
                    if (_lvapp.LeaveTypeID == 1)
                    {
                        if (_lvapp.IsAccum == true)
                        {
                            RemainingLeaves = (decimal)_lvConsumed.FirstOrDefault().CFRemaining;
                            if ((RemainingLeaves - Convert.ToDecimal(_lvapp.NoOfDays)) >= 0)
                            {
                                balance = true;
                            }
                            else
                                balance = false;
                        }
                        else
                        {
                            RemainingLeaves = (decimal)_lvConsumed.FirstOrDefault().GrandRemaining;
                            if ((RemainingLeaves - Convert.ToDecimal(_lvapp.NoOfDays)) >= 0)
                            {
                                balance = true;
                            }
                            else
                                balance = false;
                        }
                    }
                    else
                    {
                        RemainingLeaves = (decimal)_lvConsumed.FirstOrDefault().GrandRemaining;
                        if ((RemainingLeaves - Convert.ToDecimal(_lvapp.NoOfDays)) >= 0)
                        {
                            balance = true;
                        }
                        else
                            balance = false;
                    }
                }
                else
                    balance = false;
            }
            else
                balance = true;

            return balance;

        }

        public bool AddLeaveToAttData(LeaveApplication lvappl, LeaveType lvType)
        {
            try
            //Add leave to Att Data function where after applying of leave system calls this function and reprocess the attendance.
            {
                DDService.ProcessDailyAttendance(lvappl.FromDate, lvappl.ToDate, lvappl.EmpID, lvappl.EmpID.ToString());
                DDService.ProcessMonthlyAttendance(lvappl.FromDate, (int)lvappl.EmpID, lvappl.EmpID.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public bool AddLeaveToLeaveData(LeaveApplication lvappl, LeaveType lvType, LeavePolicy lvPolicy)
        {
            //Add leave to Leave data for the implemetation in the reports
            DateTime datetime = new DateTime();
            datetime = lvappl.FromDate;
            //Gets the Specific employee and his shift
            Expression<Func<Employee, bool>> SpecificEntries = aa => aa.PEmployeeID == lvappl.EmpID;
            Employee emp = EmployeeRepo.FindBy(SpecificEntries).First();
            Shift shift = emp.Shift;
            //Gets the roster or crew information of the Employee
            Expression<Func<VAT_RosterDetail, bool>> SpecificEntries2 = aa => aa.CriteriaData == emp.CrewID && aa.RosterDate >= lvappl.FromDate && aa.RosterDate <= lvappl.ToDate;
            List<VAT_RosterDetail> rosterDetails = RosterDetailRepo.FindBy(SpecificEntries2);
            for (int i = 0; i < lvappl.CalenderDays; i++)
            {
                //If the user is BCL Employee his rest day count would be not be true
                if (lvPolicy.CountRestDays != true)
                {
                    //If roster is applied to the employee 
                    if (rosterDetails.Where(aa => aa.RosterDate == datetime).Count() > 0)
                    {
                        VAT_RosterDetail rd = rosterDetails.First(aa => aa.RosterDate == datetime);
                        //IF work minutes of the roster are greater than 0 it means that roster is applied system call the Leave data function and apply leave for that day.
                        if (rd.WorkMin > 0)
                            AddLeaveData(lvappl, lvType, datetime);
                    }
                    else
                    {
                        //IF current day is not 
                        if (!CurrentDayNotRest(shift, datetime))
                            AddLeaveData(lvappl, lvType, datetime);
                    }
                }
                else
                {
                    AddLeaveData(lvappl, lvType, datetime);
                }
                datetime = datetime.AddDays(1);
            }
            return true;
        }
        public bool BalanceLeaves(LeaveApplication lvappl, LeaveType LeaveType, int PayrollPeriodID)
        {
            bool isAccLeave = false;
            List<LeaveQuotaYear> _lvConsumedYear = new List<LeaveQuotaYear>();
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = aa => aa.EmployeeID == lvappl.EmpID && aa.FinancialYearID == lvappl.FinancialYearID && aa.LeaveTypeID == lvappl.LeaveTypeID;
            _lvConsumedYear = LeaveQuotaYearRepo.FindBy(SpecificEntries);
            float _NoOfDays = lvappl.NoOfDays;
            if (_lvConsumedYear.Count > 0)
            {
                // For Yearly Leave Quota
                if (lvappl.LeaveTypeID == 1) // AL
                {
                    if (lvappl.IsAccum==true)
                    {
                        _lvConsumedYear.FirstOrDefault().CFRemaining = (float)(_lvConsumedYear.FirstOrDefault().CFRemaining - _NoOfDays);
                        _lvConsumedYear.FirstOrDefault().GrandRemaining = (float)(_lvConsumedYear.FirstOrDefault().GrandRemaining - _NoOfDays);
                        if (_lvConsumedYear.FirstOrDefault().CFRemaining < 0)
                        {
                            _lvConsumedYear.FirstOrDefault().YearlyRemaining = (float)(_lvConsumedYear.FirstOrDefault().YearlyRemaining + _lvConsumedYear.FirstOrDefault().CFRemaining);
                            //_lvConsumedYear.FirstOrDefault().GrandRemaining = (float)(_lvConsumedYear.FirstOrDefault().GrandRemaining + _lvConsumedYear.FirstOrDefault().CFRemaining);
                            _lvConsumedYear.FirstOrDefault().CFRemaining = 0;
                        }
                    }
                    else
                    {
                        if (_lvConsumedYear.FirstOrDefault().YearlyRemaining > 0)
                            _lvConsumedYear.FirstOrDefault().YearlyRemaining = (float)(_lvConsumedYear.FirstOrDefault().YearlyRemaining - _NoOfDays);
                        _lvConsumedYear.FirstOrDefault().GrandRemaining = (float)(_lvConsumedYear.FirstOrDefault().GrandRemaining - _NoOfDays);
                    }
                }
                else if (lvappl.LeaveTypeID == 4) //CPL
                {
                    if (_lvConsumedYear.FirstOrDefault().YearlyRemaining > 0)
                        _lvConsumedYear.FirstOrDefault().YearlyRemaining = (float)(_lvConsumedYear.FirstOrDefault().YearlyRemaining - _NoOfDays);
                    _lvConsumedYear.FirstOrDefault().GrandRemaining = (float)(_lvConsumedYear.FirstOrDefault().GrandRemaining - _NoOfDays);
                    // Subtract leave from LeaveCPLBalance
                    Expression<Func<LeaveCPLEmpBalance, bool>> SpecificEntrie3 = aa => aa.EmployeeID == lvappl.EmpID && aa.IsExpire == false && aa.RemainingDays > 0;
                    List<LeaveCPLEmpBalance> dbLeaveCPLBalances = LeaveCPLEmpBalanceRepo.FindBy(SpecificEntrie3).OrderBy(aa => aa.PLeaveCPLEmpBalanceID).ToList();
                    if (dbLeaveCPLBalances.Count == 1)
                    {
                        LeaveCPLEmpBalance dbLeaveCPLBalance = dbLeaveCPLBalances.First();
                        dbLeaveCPLBalance.Used = (float)(dbLeaveCPLBalance.Used + _NoOfDays);
                        dbLeaveCPLBalance.RemainingDays = (float)(dbLeaveCPLBalance.RemainingDays - _NoOfDays);
                        LeaveCPLEmpBalanceRepo.Edit(dbLeaveCPLBalance);
                        LeaveCPLEmpBalanceRepo.Save();
                    }
                    else if (dbLeaveCPLBalances.Count > 1)
                    {
                        double? NoOfdays = _NoOfDays;
                        foreach (var dbLeaveCPLBalance in dbLeaveCPLBalances)
                        {
                            if (dbLeaveCPLBalance.RemainingDays > 0 && NoOfdays > 0)
                            {
                                dbLeaveCPLBalance.Used = (float)(dbLeaveCPLBalance.Used + NoOfdays);
                                dbLeaveCPLBalance.RemainingDays = (float)(dbLeaveCPLBalance.RemainingDays - NoOfdays);
                                if (dbLeaveCPLBalance.RemainingDays < 0)
                                {
                                    NoOfdays = -1.0 * dbLeaveCPLBalance.RemainingDays;
                                    dbLeaveCPLBalance.Used = (float)(dbLeaveCPLBalance.Used - NoOfdays);
                                    dbLeaveCPLBalance.RemainingDays = 0;
                                }
                                else
                                    NoOfdays = 0;
                                LeaveCPLEmpBalanceRepo.Edit(dbLeaveCPLBalance);
                                LeaveCPLEmpBalanceRepo.Save();
                            }
                        }
                    }
                }
                else // All others
                {
                    if (_lvConsumedYear.FirstOrDefault().YearlyRemaining > 0)
                        _lvConsumedYear.FirstOrDefault().YearlyRemaining = (float)(_lvConsumedYear.FirstOrDefault().YearlyRemaining - _NoOfDays);
                    _lvConsumedYear.FirstOrDefault().GrandRemaining = (float)(_lvConsumedYear.FirstOrDefault().GrandRemaining - _NoOfDays);
                }
                LeaveQuotaYearRepo.Edit(_lvConsumedYear.First());
                LeaveQuotaYearRepo.Save();

                LeaveQuotaPeriod atLQp = new LeaveQuotaPeriod();
                Expression<Func<LeaveQuotaPeriod, bool>> SpecificEntrie2 = aa => aa.EmployeeID == lvappl.EmpID && aa.PayrollPeriodID == PayrollPeriodID && aa.LeaveTypeID == lvappl.LeaveTypeID;

                if (LeaveQuotaPeriodRepo.FindBy(SpecificEntrie2).Count() > 0)
                    atLQp = LeaveQuotaPeriodRepo.FindBy(SpecificEntrie2).First();
                else
                {
                    atLQp.EmployeeID = lvappl.EmpID;
                    atLQp.LeaveTypeID = lvappl.LeaveTypeID;
                    atLQp.PayrollPeriodID = PayrollPeriodID;
                    atLQp.ConsumedDays = 0;
                    atLQp.StartNoOfDays = _lvConsumedYear.FirstOrDefault().GrandRemaining + _NoOfDays;
                    LeaveQuotaPeriodRepo.Add(atLQp);
                    LeaveQuotaPeriodRepo.Save();
                }
                atLQp = AssistantLeave.AddBalancceMonthQuota(_lvConsumedYear, lvappl, atLQp);
                LeaveQuotaPeriodRepo.Edit(atLQp);
                // Check for Post edit
                LeaveQuotaPeriodRepo.Save();

            }
            return isAccLeave;
        }
        private void AddLeaveData(LeaveApplication lvappl, LeaveType lvType, DateTime datetime)
        {
            string _EmpDate = lvappl.EmpID + datetime.Date.ToString("yyMMdd");
            LeaveData _LVData = new LeaveData();
            _LVData.EmpID = lvappl.EmpID;
            if (lvappl.IsHalf == true)
                _LVData.HalfLeave = true;
            else
                _LVData.HalfLeave = false;
            _LVData.PLeaveDataEmpDate = _EmpDate;
            _LVData.Remarks = lvappl.LeaveReason;
            _LVData.IsAccum = lvappl.IsAccum;
            _LVData.LeaveAppID = lvappl.PLeaveAppID;
            _LVData.AttDate = datetime.Date;
            _LVData.LeaveTypeID = lvappl.LeaveTypeID;
            try
            {
                LeaveDataRepo.Add(_LVData);
                LeaveDataRepo.Save();
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region -- Delete Leaves --
        public void DeleteFromLVData(LeaveApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                Expression<Func<LeaveData, bool>> SpecificEntrie = aa => aa.EmpID == _EmpID && aa.AttDate == Date.Date;
                if (LeaveDataRepo.FindBy(SpecificEntrie).Count() > 0)
                {
                    var _id = LeaveDataRepo.FindBy(SpecificEntrie).FirstOrDefault().PLeaveDataEmpDate;

                    LeaveData lvvdata = LeaveDataRepo.GetSingle(_id);
                    //lvvdata.Active = false;
                    LeaveDataRepo.Delete(lvvdata);
                    LeaveDataRepo.Save();
                }
                Date = Date.AddDays(1);
            }
        }

        public void UpdateLeaveBalance(LeaveApplication lvappl, int PayrollPeriodID)
        {
            try
            {
                float LvDays = (float)lvappl.NoOfDays;
                List<LeaveQuotaYear> _lvConsumed = new List<LeaveQuotaYear>();
                Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = aa => aa.EmployeeID == lvappl.EmpID && aa.FinancialYearID == lvappl.FinancialYearID && aa.LeaveTypeID == lvappl.LeaveTypeID;
                _lvConsumed = LeaveQuotaYearRepo.FindBy(SpecificEntries);
                if (_lvConsumed.Count > 0)
                {
                    if (lvappl.LeaveTypeID == 1)
                    {
                        if (lvappl.IsAccum==true)
                        {
                            _lvConsumed.FirstOrDefault().GrandRemaining = (float)(_lvConsumed.FirstOrDefault().GrandRemaining + LvDays);
                            _lvConsumed.FirstOrDefault().CFRemaining = (float)(_lvConsumed.FirstOrDefault().CFRemaining + LvDays);
                        }
                        else
                        {
                            if (_lvConsumed.FirstOrDefault().YearlyRemaining >= 0)
                                _lvConsumed.FirstOrDefault().YearlyRemaining = (float)(_lvConsumed.FirstOrDefault().YearlyRemaining + LvDays);
                            _lvConsumed.FirstOrDefault().GrandRemaining = (float)(_lvConsumed.FirstOrDefault().GrandRemaining + LvDays);
                            if (_lvConsumed.FirstOrDefault().YearlyRemaining > _lvConsumed.FirstOrDefault().YearlyTotal)
                            {
                                _lvConsumed.FirstOrDefault().CFRemaining = _lvConsumed.FirstOrDefault().YearlyRemaining - _lvConsumed.FirstOrDefault().YearlyTotal;
                                _lvConsumed.FirstOrDefault().YearlyRemaining = _lvConsumed.FirstOrDefault().YearlyTotal;
                            }
                        }
                    }
                    else if (lvappl.LeaveTypeID == 4)
                    {
                        if (_lvConsumed.FirstOrDefault().YearlyRemaining > 0)
                            _lvConsumed.FirstOrDefault().YearlyRemaining = (float)(_lvConsumed.FirstOrDefault().YearlyRemaining + LvDays);
                        _lvConsumed.FirstOrDefault().GrandRemaining = (float)(_lvConsumed.FirstOrDefault().GrandRemaining + LvDays);
                        // Add leave from LeaveCPLBalance
                        Expression<Func<LeaveCPLEmpBalance, bool>> SpecificEntrie3 = aa => aa.EmployeeID == lvappl.EmpID && aa.IsExpire == false;
                        List<LeaveCPLEmpBalance> dbLeaveCPLBalances = LeaveCPLEmpBalanceRepo.FindBy(SpecificEntrie3).OrderBy(aa => aa.PLeaveCPLEmpBalanceID).ToList();
                        if (dbLeaveCPLBalances.Count == 1)
                        {
                            LeaveCPLEmpBalance dbLeaveCPLBalance = dbLeaveCPLBalances.First();
                            dbLeaveCPLBalance.Used = (float)(dbLeaveCPLBalance.Used - lvappl.NoOfDays);
                            dbLeaveCPLBalance.RemainingDays = (float)(dbLeaveCPLBalance.RemainingDays + lvappl.NoOfDays);
                            LeaveCPLEmpBalanceRepo.Edit(dbLeaveCPLBalance);
                            LeaveCPLEmpBalanceRepo.Save();
                        }
                        else if (dbLeaveCPLBalances.Count > 1)
                        {
                            double? NoOfdays = lvappl.NoOfDays;
                            foreach (var dbLeaveCPLBalance in dbLeaveCPLBalances.OrderByDescending(aa => aa.PLeaveCPLEmpBalanceID).ToList())
                            {
                                if (NoOfdays > 0 && dbLeaveCPLBalance.Used > 0)
                                {
                                    dbLeaveCPLBalance.Used = (float)(dbLeaveCPLBalance.Used - NoOfdays);
                                    dbLeaveCPLBalance.RemainingDays = (float)(dbLeaveCPLBalance.RemainingDays + NoOfdays);
                                    if (dbLeaveCPLBalance.Used < 0)
                                    {
                                        NoOfdays = -1.0 * dbLeaveCPLBalance.Used;
                                        dbLeaveCPLBalance.Used = 0;
                                        dbLeaveCPLBalance.RemainingDays = dbLeaveCPLBalance.TotalDays;
                                    }
                                    else
                                        NoOfdays = 0;
                                    LeaveCPLEmpBalanceRepo.Edit(dbLeaveCPLBalance);
                                    LeaveCPLEmpBalanceRepo.Save();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_lvConsumed.FirstOrDefault().YearlyRemaining > 0)
                            _lvConsumed.FirstOrDefault().YearlyRemaining = (float)(_lvConsumed.FirstOrDefault().YearlyRemaining + LvDays);
                        _lvConsumed.FirstOrDefault().GrandRemaining = (float)(_lvConsumed.FirstOrDefault().GrandRemaining + LvDays);
                    }
                    LeaveQuotaYearRepo.Edit(_lvConsumed.FirstOrDefault());
                    LeaveQuotaYearRepo.Save();
                    LeaveQuotaPeriod atLQp = new LeaveQuotaPeriod();
                    Expression<Func<LeaveQuotaPeriod, bool>> SpecificEntrie2 = aa => aa.EmployeeID == lvappl.EmpID && aa.PayrollPeriodID == PayrollPeriodID && aa.LeaveTypeID == lvappl.LeaveTypeID;

                    if (LeaveQuotaPeriodRepo.FindBy(SpecificEntrie2).Count() > 0)
                        atLQp = LeaveQuotaPeriodRepo.FindBy(SpecificEntrie2).First();
                    atLQp.ConsumedDays = atLQp.ConsumedDays - lvappl.NoOfDays;
                    atLQp.RemainingDays = atLQp.StartNoOfDays - atLQp.ConsumedDays;

                    LeaveQuotaPeriodRepo.Edit(atLQp);
                    LeaveQuotaPeriodRepo.Save();
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region -- Delete Half Leaves --
        public void DeleteHLFromLVData(LeaveApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                Expression<Func<LeaveData, bool>> SpecificEntrie = aa => aa.EmpID == _EmpID && aa.AttDate == Date.Date;
                if (LeaveDataRepo.FindBy(SpecificEntrie).Count() > 0)
                {
                    var _id = LeaveDataRepo.FindBy(SpecificEntrie).FirstOrDefault().PLeaveDataEmpDate;
                    LeaveData lvvdata = LeaveDataRepo.GetSingle(_id);
                    //lvvdata.Active = false;
                    LeaveDataRepo.Delete(lvvdata);
                    LeaveDataRepo.Save();
                }
                Date = Date.AddDays(1);
            }
        }
        #endregion
        public bool HasLeaveQuota(int empID, LeavePolicy leaveType, int FinYearID)
        {
            bool check = false;
            List<LeaveQuotaYear> _lvConsumed = new List<LeaveQuotaYear>();
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = aa => aa.EmployeeID == empID && aa.FinancialYearID == FinYearID && aa.LeaveTypeID == leaveType.LeaveTypeID;
            _lvConsumed = LeaveQuotaYearRepo.FindBy(SpecificEntries);
            if (_lvConsumed.Count > 0)
            {

                check = true;
            }
            return check;
        }
        public float CalculateNoOfDays(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy)
        {
            List<Holiday> holidays = new List<Holiday>();
            holidays = DDService.GetHolidays();
            float val = 0;
            if (lvapplication.IsHalf == true)
                val = (float)0.5;
            else
            {
                val = (lvapplication.ToDate - lvapplication.FromDate).Days + 1;
                Expression<Func<Employee, bool>> SpecificEntries = aa => aa.PEmployeeID == lvapplication.EmpID;
                Employee emp = EmployeeRepo.FindBy(SpecificEntries).FirstOrDefault();
                Shift shift = emp.Shift;
                Expression<Func<VAT_RosterDetail, bool>> SpecificEntries2 = aa => aa.CriteriaData == emp.CrewID && aa.RosterDate >= lvapplication.FromDate && aa.RosterDate <= lvapplication.ToDate;
                List<VAT_RosterDetail> rosterDetails = RosterDetailRepo.FindBy(SpecificEntries2);
                if ((lvPolicy.CountRestDays != true || lvPolicy.CountGZDays != true) && lvPolicy.PLeavePolicyID != 0)
                {
                    DateTime dts = lvapplication.FromDate;
                    while (dts <= lvapplication.ToDate)
                    {
                        if (rosterDetails.Where(aa => aa.RosterDate == dts).Count() > 0)
                        {
                            VAT_RosterDetail rd = rosterDetails.First(aa => aa.RosterDate == dts);

                            if (holidays.Where(aa => aa.HolidayDate == dts).Count() > 0 && shift.GZDays == true)
                            {
                                if (lvPolicy.CountGZDays == false)
                                {
                                    val = val - 1;
                                }
                            }
                            else
                            {
                                if (rd.WorkMin == 0)
                                    val = val - 1;
                            }
                        }
                        else
                        {

                            if (holidays.Where(aa => aa.HolidayDate == dts).Count() > 0 && shift.GZDays == true)
                            {
                                if (lvPolicy.CountGZDays == false)
                                {
                                    val = val - 1;
                                }
                            }
                            else
                            {
                                if (CurrentDayNotRest(shift, dts))
                                    val = val - 1;
                            }
                        }
                        dts = dts.AddDays(1);
                    }
                }
            }
            return val;
        }
        private bool CurrentDayNotRest(Shift ss, DateTime dts)
        {
            bool holiday = false;
            switch (dts.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (ss.MonMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Tuesday:
                    if (ss.TueMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Wednesday:
                    if (ss.WedMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Thursday:
                    if (ss.ThuMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Friday:
                    if (ss.FriMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Saturday:
                    if (ss.SatMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Sunday:
                    if (ss.SunMin == 0)
                        holiday = true;
                    break;
            }

            return holiday;
        }
        public float CalculateCalenderDays(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy)
        {
            float val = 0;
            if (lvapplication.IsHalf == true)
                val = (float)0.5;
            else
            {
                val = (lvapplication.ToDate - lvapplication.FromDate).Days + 1;
            }
            return val;
        }
        public DateTime? GetReturnDate(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy)
        {
            DateTime returnDate = lvapplication.ToDate.AddDays(1);
            DateTime EndDate = lvapplication.ToDate.AddDays(7);
            if (lvapplication.IsHalf == true)
                return returnDate;
            else
            {
                Expression<Func<Employee, bool>> SpecificEntries = aa => aa.PEmployeeID == lvapplication.EmpID;
                Employee emp = EmployeeRepo.FindBy(SpecificEntries).First();
                Shift ss = emp.Shift;
                Expression<Func<VAT_RosterDetail, bool>> SpecificEntries2 = aa => aa.CriteriaData == emp.CrewID && aa.RosterDate >= lvapplication.FromDate && aa.RosterDate <= EndDate;
                List<VAT_RosterDetail> rosterDetails = RosterDetailRepo.FindBy(SpecificEntries2);
                if (lvPolicy.CountRestDays != true)
                {
                    DateTime dts = returnDate;
                    while (true)
                    {
                        if (rosterDetails.Where(aa => aa.RosterDate == dts).Count() > 0)
                        {
                            VAT_RosterDetail rd = rosterDetails.First(aa => aa.RosterDate == dts);
                            if (rd.WorkMin > 0)
                                return dts;
                        }
                        else
                        {
                            if (CurrentDayNotRest(ss, dts) == false)
                                return dts;

                        }
                        dts = dts.AddDays(1);
                    }

                }
                return returnDate;
            }
        }
        public LeaveApplication GetDelete(int id)
        {
            return LeaveAppRepo.GetSingle(id);
        }

        public void PostDelete(LeaveApplication lvApp)
        {
            LeaveAppRepo.Delete(lvApp);
            LeaveAppRepo.Save();
        }
        public bool CheckForALConsectiveDay(LeaveApplication lvapplication)
        {
            DateTime dt = lvapplication.FromDate.AddDays(-1);
            Expression<Func<LeaveApplication, bool>> SpecificEntries2 = aa => aa.ToDate == dt && aa.EmpID == lvapplication.EmpID && aa.LeaveTypeID == 1 && (aa.LeaveStageID == "P" || aa.LeaveStageID == "A" || aa.LeaveStageID == null);
            if (LeaveAppRepo.FindBy(SpecificEntries2).Count > 0)
                return true;
            else
                return false;
        }

    }
}
