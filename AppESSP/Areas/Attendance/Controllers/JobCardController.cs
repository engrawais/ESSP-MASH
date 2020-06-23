using ESSPCORE.Attendance;
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
    public class JobCardController : Controller
    {
        IDDService DDService;
        IJobCardService JobCardService;
        IEntityService<Employee> EmployeeService;
        IEntityService<JobCardDetail> JobCardDetailService;
        IEntityService<JobCardApp> JobCardAppService;
        IEntityService<Shift> ShiftService;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IEntityService<VAT_LeaveApplication> VATLeaveApplicationService;
        IEntityService<VEP_JobCardApplication> VEPJobCardApplicationService;
        IEntityService<VHR_UserEmployee> VHR_UserEmployeeService;
        public JobCardController(IDDService dDService, IJobCardService jobCardService, IEntityService<Employee> employeeService, IEntityService<JobCardApp> jobCardAppService
            , IEntityService<VAT_LeaveApplication> vATLeaveApplicationService, IEntityService<JobCardDetail> jobCardDetailService
            , IEntityService<VEP_JobCardApplication> vEPJobCardApplicationService, IEntityService<VHR_UserEmployee> vhr_UserEmployeeService
            , IEntityService<Shift> shiftService, IEntityService<PayrollPeriod> payrollPeriodService)
        {
            DDService = dDService;
            JobCardService = jobCardService;
            EmployeeService = employeeService;
            JobCardAppService = jobCardAppService;
            VATLeaveApplicationService = vATLeaveApplicationService;
            JobCardDetailService = jobCardDetailService;
            VEPJobCardApplicationService = vEPJobCardApplicationService;
            VHR_UserEmployeeService = vhr_UserEmployeeService;
            ShiftService = shiftService;
            PayrollPeriodService = payrollPeriodService;
        }
        // GET: Attendance/JobCard
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
            List<VAT_JobCardApplication> dbVAT_JobCardApplication = new List<VAT_JobCardApplication>();

            try
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                dbVAT_JobCardApplication = JobCardService.GetIndex(LoggedInUser).OrderByDescending(aa => aa.PJobCardAppID).ToList();
            }
            catch (Exception ex)
            {

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                dbVAT_JobCardApplication = dbVAT_JobCardApplication.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PJobCardAppID.ToString().Contains(searchString)).ToList();

            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(dbVAT_JobCardApplication.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ESSPPendingJobCardsIndex()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VEP_JobCardApplication> dbVAT_LeaveApplications = new List<VEP_JobCardApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                if (LoggedInUser.UserLoctions != null)
                {
                    foreach (var userLocaion in LoggedInUser.UserLoctions)
                    {
                        Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && (c.JobCardStageID == "P");
                        dbVAT_LeaveApplications.AddRange(VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.PJobCardAppID).ToList());
                    }
                }
            }
            else
            {
                Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries = c => (c.JobCardStageID == "P");
                dbVAT_LeaveApplications = VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.DateCreated).ToList();
            }
            return View(dbVAT_LeaveApplications);
        }

        public ActionResult SingleDay()
        {

            JobCardApp obj = new JobCardApp();
            obj = JobCardService.GetSingleDay();
            CreateHelper(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult SingleDay(JobCardApp obj)
        {
            try
            {
                string _EmpNo = Request.Form["EmpNo"].ToString();
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
                VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).First();
                if (_emp.Count == 0)
                {
                    ModelState.AddModelError("EmployeeID", "Emp No not exist");
                }
                else
                {
                    obj.EmployeeID = _emp.First().PEmployeeID;
                }

                if (obj.TimeStart == null || obj.TimeEnd == null)
                {
                    ModelState.AddModelError("TimeStart", "Time Start and Time End cannot be empty");
                }
                if (obj.TimeEnd <= obj.TimeStart)
                    ModelState.AddModelError("TimeEnd", "Time end cannot be less than or equal to start time .");
                Expression<Func<JobCardApp, bool>> SpecificEntries2 = aa => obj.DateStarted == aa.DateStarted && obj.TimeStart <= aa.TimeEnd && aa.TimeStart <= obj.TimeEnd && aa.EmployeeID == obj.EmployeeID && aa.JobCardStageID != "R";
                if (JobCardAppService.GetIndexSpecific(SpecificEntries2).Count() > 0)
                    ModelState.AddModelError("TimeStart", "Already exists between the time span");
                Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => obj.DateStarted >= c.PRStartDate && obj.DateStarted <= c.PREndDate && c.PeriodStageID == "C";
                List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
                if (dbPayrollPeriods.Count() > 0)
                {
                    ModelState.AddModelError("DateStarted", "Cannot enter Job card in Closed Payroll Period");
                }
                Expression<Func<Shift, bool>> SpecificEntries97 = c => c.PShiftID == employee.ShiftID;
                Shift shifts = ShiftService.GetIndexSpecific(SpecificEntries97).First();
                if (shifts.GZDays == true)
                {
                    List<Holiday> holiday = DDService.GetHolidays().Where(aa => aa.HolidayDate == obj.DateStarted).ToList();
                    if (holiday.Count > 0)
                    {
                        ModelState.AddModelError("DateStarted", "Cannot apply job card of the Gazetted Holiday");
                    }
                }
                if (ModelState.IsValid)
                {
                    obj.UserID = LoggedInUser.PUserID;
                    JobCardService.PostSingleDay(obj);
                    DDService.ProcessDailyAttendance(obj.DateStarted, obj.DateEnded, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                    DDService.ProcessMonthlyAttendance(obj.DateStarted, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                    return Json("OK", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception)
            {
            }
            CreateHelper(obj);
            return PartialView("SingleDay", obj);
        }
        public ActionResult MultipleDay()
        {
            JobCardApp obj = new JobCardApp();
            obj = JobCardService.GetMultipleDay();
            CreateHelper(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult MultipleDay(JobCardApp obj)
        {
            {
                try
                {

                    VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                    string _EmpNo = Request.Form["EmpNo"].ToString();
                    List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
                    VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).First();
                    if (_emp.Count == 0)
                    {
                        ModelState.AddModelError("EmployeeID", "Invalid Employee Location");
                    }
                    else
                    {
                        obj.EmployeeID = _emp.First().PEmployeeID;
                    }

                    if (obj.DateStarted != null && obj.DateEnded != null)
                    {
                        if (obj.DateEnded < obj.DateStarted)
                        {
                            ModelState.AddModelError("DateStarted", "Start date can never be greater than end date.");
                        }
                    }
                    Expression<Func<Shift, bool>> SpecificEntries97 = c => c.PShiftID == employee.ShiftID;
                    Shift shifts = ShiftService.GetIndexSpecific(SpecificEntries97).First();
                    if (shifts.GZDays == true)
                    {
                        List<Holiday> holiday = DDService.GetHolidays().Where(aa => aa.HolidayDate == obj.DateStarted).ToList();
                        if (holiday.Count > 0)
                        {
                            ModelState.AddModelError("DateStarted", "Cannot apply job card of the Gazetted Holiday");
                        }
                    }
                    Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => obj.DateStarted >= c.PRStartDate && obj.DateStarted <= c.PREndDate && c.PeriodStageID == "C";
                    List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
                    if (dbPayrollPeriods.Count() > 0)
                    {
                        ModelState.AddModelError("DateStarted", "Cannot enter Job card in Closed Payroll Period");
                    }
                    Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries4 = aa => aa.EmpID == obj.EmployeeID && obj.DateStarted <= aa.ToDate && aa.FromDate <= obj.DateEnded && aa.IsHalf != true;
                    if (VATLeaveApplicationService.GetIndexSpecific(SpecificEntries4).Count() > 0)
                    {
                        ModelState.AddModelError("DateStarted", "Leave already applied for one or more days");
                    }
                    Expression<Func<JobCardApp, bool>> SpecificEntries2 = aa => obj.DateStarted <= aa.DateEnded && aa.DateStarted <= obj.DateEnded && aa.EmployeeID == obj.EmployeeID;
                    if (JobCardAppService.GetIndexSpecific(SpecificEntries2).Count() > 0)
                        ModelState.AddModelError("DateStarted", "Already exists in same date");

                    if (ModelState.IsValid)
                    {
                        obj.UserID = LoggedInUser.PUserID;
                        JobCardService.PostMultipleDay(obj);
                        DDService.ProcessDailyAttendance(obj.DateStarted, obj.DateEnded, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                        DDService.ProcessMonthlyAttendance(obj.DateStarted, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                        return Json("OK", JsonRequestBehavior.AllowGet);

                    }
                    CreateHelper(obj);
                    return PartialView("MultipleDay", obj);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        /// <summary>
        /// Gets the List of OU,Location and Job card types 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create1()
        {
            VMJobCardCreate vmJobCardCreate = new VMJobCardCreate();
            vmJobCardCreate = JobCardService.GetCreate1();
            ViewBag.JobCardTypeID = new SelectList(DDService.GetJobCardType().ToList().OrderBy(aa => aa.JobCardName).ToList(), "PJobCardTypeID", "JobCardName");
            return View(vmJobCardCreate);
        }
        /// <summary>
        /// Ge the Selelected filters.
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedOUCommonIds"></param>
        /// <param name="SelectedOUIds"></param>
        /// <param name="SelectedEmploymentTypeIds"></param>
        /// <param name="SelectedLocationIds"></param>
        /// <param name="SelectedGradeIds"></param>
        /// <param name="SelectedJobTitleIds"></param>
        /// <param name="SelectedDesignationIds"></param>
        /// <param name="SelectedCrewIds"></param>
        /// <param name="SelectedShiftIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create1(VMJobCardCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            es = JobCardService.GetCreate2(es, SelectedCompanyIds, SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
            SelectedCrewIds, SelectedShiftIds, LoggedInUser);
            return View("Create2", es);
        }
        /// <summary>
        /// Applies the Job Card to the Selected employees
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedEmpIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create3(VMJobCardCreate es, int?[] SelectedEmpIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            JobCardService.PostCreate3(es, SelectedEmpIds, LoggedInUser);
            //return View(es);
            return RedirectToAction("Index");
        }
        //Deleting the Jobcard
        public ActionResult Delete(int? id)
        {
            return PartialView(JobCardAppService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(JobCardApp obj)
        {
            Expression<Func<JobCardApp, bool>> SpecificEntries2 = aa => aa.PJobCardAppID == obj.PJobCardAppID;
            JobCardAppService.GetIndexSpecific(SpecificEntries2);
            if (ModelState.IsValid)
            {
                JobCardAppService.PostDelete(obj);
                // Create Reprocess Request
                DDService.ProcessDailyAttendance(obj.DateStarted, obj.DateEnded, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                DDService.ProcessMonthlyAttendance(obj.DateStarted, (int)obj.EmployeeID, obj.EmployeeID.ToString());

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        public ActionResult SendEmailToLM()
        {
            // Send email to LM
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            Expression<Func<VHR_UserEmployee, bool>> SpecificEntries2 = aa => aa.UserStatus == true;
            List<VHR_UserEmployee> dbUsers = VHR_UserEmployeeService.GetIndexSpecific(SpecificEntries2).ToList();
            // For Job Cards

            List<VEP_JobCardApplication> dbJobCards = new List<VEP_JobCardApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                if (LoggedInUser.UserLoctions != null)
                {
                    foreach (var userLocaion in LoggedInUser.UserLoctions)
                    {
                        Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries23 = aa => aa.JobCardStageID == "P" && aa.LocationID == userLocaion.LocationID;
                        dbJobCards.AddRange(VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries23).ToList());
                    }
                }
            }
            foreach (var LineManagerID in dbJobCards.Select(aa => aa.LineManagerID).Distinct().ToList())
            {
                if (dbUsers.Where(aa => aa.PUserID == LineManagerID).Count() > 0)
                {
                    VHR_UserEmployee vHR_UserEmployee = dbUsers.First(aa => aa.PUserID == LineManagerID);
                    if (vHR_UserEmployee.OfficialEmailID != "" && vHR_UserEmployee.OfficialEmailID != null)
                    {
                        // Send email
                        DDService.GenerateEmail(vHR_UserEmployee.OfficialEmailID, "", "Payroll Period is Closing: Need your attention", ESSPText.GetPendingJCAlertEmail(vHR_UserEmployee.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.EmailAlert));
                    }
                }
            }
            return RedirectToAction("ESSPPendingJobCardsIndex");
        }

        #region -- Private Method--
        private void CreateHelper(JobCardApp obj)
        {
            ViewBag.JobCardTypeID = new SelectList(DDService.GetJobCardType().ToList().OrderBy(aa => aa.JobCardName).ToList(), "PJobCardTypeID", "JobCardName", obj.JobCardTypeID);

        }
        #endregion

    }

}