
using AppESSP.Helper;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.ESSP;
using ESSPSERVICE.Generic;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{

    public class LeaveApplicationController : Controller
    {
        // GET: HumanRecource/Leave Application
        ILeaveApplicationService LeaveApplicationService;
        IEntityService<VHR_EmployeeProfile> VHR_EmployeeProfile;
        IEntityService<VAT_LeaveApplication> VATLeaveApplicationService;
        IEntityService<VHR_UserEmployee> VHR_UserEmployeeService;
        IEntityService<DailyAttendance> DailyAttendanceService;
        IEntityService<Holiday> HolidayService;
        IEntityService<Shift> ShiftService;
        IEntityService<VAT_JobCardApplication> JobcardAppService;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IDDService DDService;
        public LeaveApplicationController(ILeaveApplicationService leaveapplicationService, IEntityService<DeviceData> deviceDataService, IDDService dDService,
        IEntityService<VAT_LeaveApplication> vATLeaveApplicationService, IEntityService<DailyAttendance> dailyAttendanceService
            , IEntityService<VAT_JobCardApplication> jobcardAppService, IEntityService<VHR_UserEmployee> vHR_UserEmployeeService
            , IEntityService<PayrollPeriod> payrollPeriodService, IEntityService<Holiday> holidayService, IEntityService<Shift> shiftService
            , IEntityService<VHR_EmployeeProfile> vHR_EmployeeProfile)
        {
            LeaveApplicationService = leaveapplicationService;
            HolidayService = holidayService;
            ShiftService = shiftService;
            DDService = dDService;
            VATLeaveApplicationService = vATLeaveApplicationService;
            DailyAttendanceService = dailyAttendanceService;
            JobcardAppService = jobcardAppService;
            VHR_UserEmployeeService = vHR_UserEmployeeService;
            PayrollPeriodService = payrollPeriodService;
            VHR_EmployeeProfile = vHR_EmployeeProfile;
        }
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            List<VAT_LeaveApplication> dbVAT_LeaveApplications = new List<VAT_LeaveApplication>();

            try
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;

                if (LoggedInUser.UserAccessTypeID == 2)
                {
                    if (LoggedInUser.UserLoctions != null)
                    {
                        // Get all leave application based on User locations
                        foreach (var userLocaion in LoggedInUser.UserLoctions)
                        {
                            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && (c.LeaveStageID == "A" || c.LeaveStageID == null);
                            dbVAT_LeaveApplications.AddRange(VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.PLeaveAppID).ToList());
                        }
                    }
                }
                if (LoggedInUser.UserAccessTypeID == 4)
                {
                    if (LoggedInUser.UserDepartments != null)
                    {
                        // Get all leave application based on User locations
                        foreach (var userDepartment in LoggedInUser.UserDepartments)
                        {
                            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => c.OUCommonID == userDepartment.DepartmentID && (c.LeaveStageID == "A" || c.LeaveStageID == null);
                            dbVAT_LeaveApplications.AddRange(VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.PLeaveAppID).ToList());
                        }
                    }
                }
                else
                {
                    // Get all approved leave applications in case of admin location user
                    Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => (c.LeaveStageID == "A" || c.LeaveStageID == null);
                    dbVAT_LeaveApplications = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.LeaveDate).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                dbVAT_LeaveApplications = dbVAT_LeaveApplications.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PLeaveAppID.ToString().Contains(searchString)).ToList();

            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(dbVAT_LeaveApplications.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ESSPPendingLeaveIndex()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_LeaveApplication> dbVAT_LeaveApplications = new List<VAT_LeaveApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                if (LoggedInUser.UserLoctions != null)
                {
                    foreach (var userLocaion in LoggedInUser.UserLoctions)
                    {
                        Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && (c.LeaveStageID == "P");
                        dbVAT_LeaveApplications.AddRange(VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.PLeaveAppID).ToList());
                    }
                }
            }
            else
            {
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => (c.LeaveStageID == "P");
                dbVAT_LeaveApplications = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.LeaveDate).ToList();
            }
            return View(dbVAT_LeaveApplications);
        }
        [HttpGet]
        public ActionResult Create()
        {
            LeaveApplication dbLeaveApplication = new LeaveApplication();
            DateTime dt = DateTime.Today.AddDays(-10);
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => c.LeaveDate == dt;
            List<VAT_LeaveApplication> dbLvApplication = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries);
            if (dbLvApplication.Count > 0)
                dbLeaveApplication.PLeaveAppID = dbLvApplication.OrderByDescending(aa => aa.PLeaveAppID).First().PLeaveAppID + 1;
            else
            {
                //dbLeaveApplication.PLeaveAppID = VATLeaveApplicationService.GetIndex().OrderByDescending(aa => aa.PLeaveAppID).First().PLeaveAppID + 1;
            }
            HelperMethod(dbLeaveApplication);
            return View(dbLeaveApplication);
        }
        public ActionResult ViewLeaveApplication(int? id)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            VAT_LeaveApplication vAT_LeaveApplication = new VAT_LeaveApplication();
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => c.PLeaveAppID == id;
            return View(VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).First());
        }
        [HttpPost]
        public ActionResult Create(LeaveApplication lvapplication)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (lvapplication.FromDate.Date > lvapplication.ToDate.Date)
                ModelState.AddModelError("FromDate", "From Date should be smaller than To Date");
          
         
            FinancialYear dbFinancialYear = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == lvapplication.FinancialYearID).First();
            if (lvapplication.ToDate > dbFinancialYear.FYEndDate || lvapplication.ToDate < dbFinancialYear.FYStartDate)
                ModelState.AddModelError("FromDate", "To Date must lie in selected financial year");
            Expression<Func<PayrollPeriod, bool>> SpecificEntries3 = c => c.PeriodStageID == "O";
            PayrollPeriod dbpayrollperiod = PayrollPeriodService.GetIndexSpecific(SpecificEntries3).First();
            if (lvapplication.FromDate < dbpayrollperiod.PRStartDate)
            {
                ModelState.AddModelError("FromDate", "Cannot Create leaves in the Closed Payroll Period");
            }

            string _EmpNo = Request.Form["EmpNo"].ToString();
            List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries121 = c => c.OEmpID == _EmpNo;
            VHR_EmployeeProfile employee = VHR_EmployeeProfile.GetIndexSpecific(SpecificEntries121).First();
            lvapplication.EmpID = employee.PEmployeeID;


            var RBValue = Request.Form["HalfLvHA"];
            if (RBValue == "false")
                lvapplication.IsHalf = false;
            else
                lvapplication.IsHalf = true;
            LeavePolicy lvPolicy = new LeavePolicy();
            LeaveType lvType = DDService.GetLeaveType().First(aa => aa.PLeaveTypeID == lvapplication.LeaveTypeID);
            if (_emp.Count == 0)
            {
                ModelState.AddModelError("EmpID", "Invalid Department Access or Employee Resigned.");

            }

            else
            {
                if (_emp.First().Status == "Resigned")
                {
                    if (_emp.First().ResignDate <= lvapplication.ToDate)
                        ModelState.AddModelError("ToDate", "Cannot Apply leaves of Resigned Employee.");
                }
                lvapplication.EmpID = employee.PEmployeeID;
                lvPolicy = AssistantLeave.GetEmployeeLeavePolicyID(_emp, lvapplication.LeaveTypeID, DDService.GetLeavePolicy().ToList());
                //lvType = db.Att_LeaveType.First(aa => aa.LvTypeID == lvapplication.LeaveTypeID);
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = aa => aa.EmpID == lvapplication.EmpID && lvapplication.FromDate >= aa.FromDate && lvapplication.FromDate <= aa.ToDate && lvapplication.ToDate > aa.FromDate;
                if (VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).Count() > 0)
                {
                    ModelState.AddModelError("FromDate", "Duplicate leave applied for one or more days");
                }

            }
            
            if (lvapplication.IsHalf == false && lvapplication.IsDeducted == false)
            {
                List<DailyAttendance> att = new List<DailyAttendance>();
                Expression<Func<DailyAttendance, bool>> SpecificEntries2 = aa => aa.EmpID == lvapplication.EmpID && aa.AttDate >= lvapplication.FromDate && aa.AttDate <= lvapplication.ToDate;
                att = DailyAttendanceService.GetIndexSpecific(SpecificEntries2);
                if (att.Count > 0)
                {
                    foreach (var at in att)
                    {
                        if (at.TimeIn != null || at.TimeOut != null)
                            ModelState.AddModelError("LeaveTypeID", "This employee has attendance for Specific day, Please clear his attendance first to proceed further");
                    }
                }
            }
            // CL cannot be taken after next to AL consective day
            if (lvapplication.LeaveTypeID == 2)
            {
                if (LeaveApplicationService.CheckForALConsectiveDay(lvapplication))
                    ModelState.AddModelError("FromDate", "You have applied AL leave for previous date");
            }
            
                float noofDays = LeaveApplicationService.CalculateNoOfDays(lvapplication, lvType, lvPolicy);
            
            if (lvapplication.LeaveTypeID == 11)
            {
                if (noofDays < lvPolicy.MinimumDays)
                {
                    ModelState.AddModelError("LeaveTypeID", "Cannot Apply Academic for Less than" + lvPolicy.MinimumDays.ToString() + " days");
                }
            }
            {
                if (noofDays < lvPolicy.MinimumDays)
                {
                    ModelState.AddModelError("LeaveTypeID", "Cannot Apply CME/Workshop for Less than" + lvPolicy.MinimumDays.ToString() + " days");
                }
            }
            if (lvapplication.IsHalf != true)
            {
                Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries1 = aa => aa.OEmpID == _EmpNo && aa.DateStarted == lvapplication.FromDate && aa.DateStarted <= lvapplication.ToDate && aa.DateEnded > lvapplication.FromDate;
                if (JobcardAppService.GetIndexSpecific(SpecificEntries1).Count() > 0)
                {
                    ModelState.AddModelError("FromDate", "Job card exist for same date");
                }
            }
            Expression<Func<Shift, bool>> SpecificEntries97 = c => c.PShiftID == employee.ShiftID;
            Shift shifts = ShiftService.GetIndexSpecific(SpecificEntries97).First();
            if (shifts.GZDays == true)
            {
                List<Holiday> holiday = DDService.GetHolidays().Where(aa => aa.HolidayDate == lvapplication.FromDate).ToList();
                if (holiday.Count > 0)
                {
                    ModelState.AddModelError("FromDate", "Cannot apply leave of the Gazetted Holiday");
                }
            }
            Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => lvapplication.FromDate >= c.PRStartDate && lvapplication.FromDate <= c.PREndDate && c.PeriodStageID == "C";
            List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
            if (dbPayrollPeriods.Count() > 0)
            {
                ModelState.AddModelError("FromDate", "Cannot enter leaves in Closed Payroll Period");
            }
            //if (!DDService.IsDateLieBetweenActivePayroll(lvapplication.FromDate))
            //    ModelState.AddModelError("FromDate", "Payroll Period is Closed for this date");
            if (ModelState.IsValid)
            {
                if (LeaveApplicationService.CheckDuplicateLeave(lvapplication))
                {
                    // max days
                   
                    float CalenderDays = LeaveApplicationService.CalculateCalenderDays(lvapplication, lvType, lvPolicy);
                    lvapplication.ReturnDate = LeaveApplicationService.GetReturnDate(lvapplication, lvType, lvPolicy);
                    lvapplication.NoOfDays = noofDays;
                    lvapplication.CalenderDays = CalenderDays;
                    int _UserID = LoggedInUser.PUserID;
                    lvapplication.CreatedBy = _UserID;
                    if (lvPolicy.PLeavePolicyID == 0)
                    {
                        LeaveApplicationService.CreateLeave(lvapplication, lvType, LoggedInUser, lvPolicy);
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //check for employee eligible for leave
                        if (AssistantLeave.EmployeeEligbleForLeave(lvPolicy, _emp))
                            if (lvPolicy.UpdateBalance == true)
                            {
                                if (LeaveApplicationService.HasLeaveQuota(lvapplication.EmpID, lvPolicy, (int)lvapplication.FinancialYearID))
                                {
                                    if (LeaveApplicationService.CheckLeaveBalance(lvapplication, lvPolicy))
                                    {
                                        //if (LeaveApplicationService.CheckForMaxMonthDays(lvapplication, lvPolicy, LvProcessController.GetFinancialYearID(db.PR_FinancialYear.ToList(), lvapplication.FromDate)))
                                        {

                                            LeaveApplicationService.CreateLeave(lvapplication, lvType, LoggedInUser, lvPolicy);
                                            return Json("OK", JsonRequestBehavior.AllowGet);
                                        }
                                        //else
                                        //    ModelState.AddModelError("FromDate", "Leave Monthly Quota Exceeds");
                                    }
                                    else
                                        ModelState.AddModelError("LeaveTypeID", "Leave Balance Exceeds!!!");
                                }
                                else
                                    ModelState.AddModelError("LeaveTypeID", "Leave Quota does not exist");
                            }
                            else
                            {
                                LeaveApplicationService.CreateLeave(lvapplication, lvType, LoggedInUser, lvPolicy);
                                return Json("OK", JsonRequestBehavior.AllowGet);

                            }
                        else
                        {
                            ModelState.AddModelError("LeaveTypeID", "Employee is not eligible for leave");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("FromDate", "This Employee already has leave in this range of dates");
                }
            }
            HelperMethod(lvapplication);
            return PartialView("Create", lvapplication);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(LeaveApplicationService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(LeaveApplication lvapplication)
        {
            Expression<Func<PayrollPeriod, bool>> SpecificEntries = c => lvapplication.FromDate >= c.PRStartDate && lvapplication.FromDate <= c.PREndDate && c.PeriodStageID == "C";
            List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries).ToList();
            if (dbPayrollPeriods.Count() > 0)
            {
                ModelState.AddModelError("LeaveDate", "Cannot Delete leaves from Closed Payroll Period");
                return PartialView("Delete", lvapplication);

            }
            else
            {
                LeaveApplicationService.DeleteFromLVData(lvapplication);
                LeaveApplicationService.UpdateLeaveBalance(lvapplication, AssistantLeave.GetPayRollPeriodID(DDService.GetPayrollPeriod(), lvapplication.FromDate));
                LeaveApplicationService.PostDelete(lvapplication);
                //ProcessSupportFunc.ProcessAttendanceRequest((DateTime)lvapplication.FromDate, (DateTime)lvapplication.ToDate, (int)lvapplication.EmpID, lvapplication.EmpID.ToString());
                //ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(lvapplication.FromDate.Year, lvapplication.FromDate.Month, 1), DateTime.Today, lvapplication.EmpID.ToString());
                DDService.ProcessDailyAttendance(lvapplication.FromDate, lvapplication.ToDate, (int)lvapplication.EmpID, lvapplication.EmpID.ToString());
                return Json("OK", JsonRequestBehavior.AllowGet);


            }
        }

        public ActionResult SendEmailToLM()
        {
            // Send email to LM
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            Expression<Func<VHR_UserEmployee, bool>> SpecificEntries2 = aa => aa.UserStatus == true;
            List<VHR_UserEmployee> dbUsers = VHR_UserEmployeeService.GetIndexSpecific(SpecificEntries2).ToList();
            // For Leaves
            List<VAT_LeaveApplication> dbLVApplications = new List<VAT_LeaveApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                if (LoggedInUser.UserLoctions != null)
                {
                    foreach (var userLocaion in LoggedInUser.UserLoctions)
                    {
                        Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries23 = aa => aa.LeaveStageID == "P" && aa.LocationID == userLocaion.LocationID;
                        dbLVApplications.AddRange(VATLeaveApplicationService.GetIndexSpecific(SpecificEntries23).ToList());
                    }
                }
            }
            foreach (var LineManagerID in dbLVApplications.Select(aa => aa.LineManagerID).Distinct().ToList())
            {
                if (dbUsers.Where(aa => aa.PUserID == LineManagerID).Count() > 0)
                {
                    VHR_UserEmployee vHR_UserEmployee = dbUsers.First(aa => aa.PUserID == LineManagerID);
                    if (vHR_UserEmployee.OfficialEmailID != "" && vHR_UserEmployee.OfficialEmailID != null)
                    {
                        // Send email
                        DDService.GenerateEmail(vHR_UserEmployee.OfficialEmailID, "", "Payroll Period is Closing: Need your attention", ESSPText.GetPendingLeaveAlertEmail(vHR_UserEmployee.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.EmailAlert));
                    }
                }
            }
            return RedirectToAction("ESSPPendingLeaveIndex");
        }
        #region -- Private Method--
        private void HelperMethod(LeaveApplication obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LeaveTypeID = new SelectList(DDService.GetLeaveType().ToList().OrderBy(aa => aa.LeaveTypeName).ToList(), "PLeaveTypeID", "LeaveTypeName");
            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().Where(aa => aa.FYStatus == true).ToList().OrderByDescending(aa => aa.PFinancialYearID).ToList(), "PFinancialYearID", "FYName");
        }
        #endregion
    }
}