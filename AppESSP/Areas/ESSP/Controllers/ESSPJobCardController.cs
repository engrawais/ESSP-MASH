using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPREPO.Generic;
using ESSPSERVICE.ESSP;
using ESSPSERVICE.Generic;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.ESSP.Controllers
{
    public class ESSPJobCardController : Controller
    {
        // GET: ESSP/ESSPJobCard
        IJobCardESSPService JobCardESSPService;
        IEntityService<VEP_JobCardApplication> VEPJobCardApplicationService;
        IEntityService<VAT_LeaveApplication> VATLeaveApplicationService;
        IEntityService<VAT_JobCardFlow> VATJobCardFlowService;
        IEntityService<Shift> ShiftService;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IRepository<JobCardAppFlow> JobCardAppFlowService;
        public List<string> ToasterMessages = new List<string>();
        IDDService DDService;
        public ESSPJobCardController(IJobCardESSPService jobCardESSPService, IEntityService<JobCardApp> jobcardappService, IDDService dDService, IEntityService<JobCardDetail> jobcarddetailservice
          , IEntityService<VEP_JobCardApplication> vEPJobCardApplicationService, IEntityService<VAT_LeaveApplication> vatLeaveApplicationService
            , IRepository<JobCardAppFlow> jobCardAppFlowService, IEntityService<VAT_JobCardFlow> vATJobCardFlowService
            , IEntityService<PayrollPeriod> payrollPeriodService, IEntityService<Shift> shiftService)
        {
            JobCardESSPService = jobCardESSPService;
            VEPJobCardApplicationService = vEPJobCardApplicationService;
            VATLeaveApplicationService = vatLeaveApplicationService;
            JobCardAppFlowService = jobCardAppFlowService;
            VATJobCardFlowService = vATJobCardFlowService;
            ShiftService = shiftService;
            PayrollPeriodService = payrollPeriodService;
            DDService = dDService;
        }
        #region       /// INDEXES FOR JOBCARDS 

        /// MAIN INDEX FOR MY JOBCARD

        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            // Disable Notifications
            int notiTypeID1 = Convert.ToInt32(NotificationTypeJCEnum.JCApproved);
            int notiTypeID2 = Convert.ToInt32(NotificationTypeJCEnum.JCRejected);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1 || c.NotificationTypeID == notiTypeID2));
            DDService.DeleteNotification(SpecificEntries);
            List<VEP_JobCardApplication> jcapp = JobCardESSPService.GetIndex(LoggedInUser).OrderByDescending(aa => aa.PJobCardAppID).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                jcapp = jcapp.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PJobCardAppID.ToString().Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.JobCardName.ToString().Contains(searchString)).ToList();
            }
            int pageSize = 500;
            int pageNumber = (page ?? 1);
            return View(jcapp.ToPagedList(pageNumber, pageSize));
        }

        //FOR LM TO APPROVE OR REJECT JOBCARDS
        public ActionResult PendingJobCardIndex()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VEP_JobCardApplication> list = JobCardESSPService.GetPendingJobCardRequests(LoggedInUser).OrderByDescending(aa => aa.PJobCardAppID).ToList();
            return View(list);
        }
        //HISTORY FOR LM'S TO SEE WHOSE JOBCARD APPLICATION HE HAS APRROVED OR REJECTED
        public ActionResult JobCardHistoryIndex()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VEP_JobCardApplication> list = JobCardESSPService.GetEmpJobCardHistory(LoggedInUser);
            return View(list);
        }
        #endregion
        #region    //// CREATION OF JOB CARDS

        //MULTIPLE DAYS CREATE
        [HttpGet]
        public ActionResult Create()
        {
            JobCardApp obj = new JobCardApp();
            MultipleDayJobCardHelper(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(JobCardApp obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            string _EmpNo = Request.Form["EmpNo"].ToString();
            Expression<Func<Employee, bool>> SpecificEntries = c => c.OEmpID == _EmpNo;
            List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
            VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).First();
            if (_emp.Count == 0)
            {
                ModelState.AddModelError("DateStarted", "Emp No not exist");
            }
            else
            {
                obj.EmployeeID = _emp.First().PEmployeeID;
            }
         
            if (obj.Remarks == null || obj.Remarks == "")
                ModelState.AddModelError("Remarks", "Remarks are Mandatory !");
            if (obj.DateStarted != null && obj.DateEnded != null)
            {
                if (obj.DateEnded < obj.DateStarted)
                {
                    ModelState.AddModelError("DateStarted", "Start Date can never be greater than end date.");
                }
            }
            if (obj.DateStarted == null || obj.DateEnded == null)
                ModelState.AddModelError("DateStarted", "Date cannot be empty");
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries4 = aa => aa.EmpID == obj.EmployeeID && obj.DateStarted <= aa.ToDate && aa.FromDate <= obj.DateEnded && aa.IsHalf != true;
            if (VATLeaveApplicationService.GetIndexSpecific(SpecificEntries4).Count() > 0)
            {
                ModelState.AddModelError("DateStarted", "Leave already applied for one or more days");
            }
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries2 = aa => obj.DateStarted <= aa.DateEnded && aa.DateStarted <= obj.DateEnded && aa.EmployeeID == obj.EmployeeID && aa.JobCardStageID != "R";
            if (VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries2).Count() > 0)
                ModelState.AddModelError("DateStarted", "Already exists between the dates");

            if (obj.DateEnded > AppAssistant.MaxDate)
                ModelState.AddModelError("DateEnded", "Date cannot be greater than " + AppAssistant.MaxDate.ToString("dd-MM-yyyy"));
            if (obj.DateStarted < AppAssistant.MinDate)
                ModelState.AddModelError("DateStarted", "Date cannot be less than " + AppAssistant.MinDate.ToString("dd-MM-yyyy"));
            //if (!DDService.IsDateLieBetweenActivePayroll(obj.DateStarted))
            //    ModelState.AddModelError("DateStarted", "Payroll Period is Closed for this date");
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
            if (ModelState.IsValid)
            {
                JobCardESSPService.PostCreate(obj, LoggedInUser);
                ToasterMessages.Add("Job card applied successfully !");
                Session["ToasterMessages"] = ToasterMessages;
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            MultipleDayJobCardHelper(obj);
            return PartialView("Create", obj);
        }
        //SINGLE DAY CREATE
        public ActionResult SingleDay()
        {
            JobCardApp obj = new JobCardApp();
            SingleDayJobCardHelper(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult SingleDay(JobCardApp obj)
        {
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                string _EmpNo = Request.Form["EmpNo"].ToString();
                Expression<Func<Employee, bool>> SpecificEntries = c => c.OEmpID == _EmpNo;
                List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
                VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).First();
                if (_emp.Count == 0)
                {
                    ModelState.AddModelError("StartDate", "Emp No not exist");
                }
                else
                {
                    obj.EmployeeID = _emp.First().PEmployeeID;
                }
                if (obj.DateStarted == null)
                    ModelState.AddModelError("DateStarted", "Date start cannot be empty");
                if (obj.TimeStart == null || obj.TimeEnd == null)
                {
                    ModelState.AddModelError("TimeStart", "Time Start and Time End cannot be empty");
                }
                if (obj.Remarks == null || obj.Remarks == "")
                    ModelState.AddModelError("Remarks", "Reason is Mandatory !");
                if (obj.TimeEnd <= obj.TimeStart)
                    ModelState.AddModelError("TimeEnd", "Time end cannot be less than or equal to start time .");
                Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries2 = aa => obj.DateStarted == aa.DateStarted && obj.TimeStart <= aa.TimeEnd && aa.TimeStart <= obj.TimeEnd && aa.EmployeeID == obj.EmployeeID && aa.JobCardStageID != "R";
                if (VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries2).Count() > 0)
                    ModelState.AddModelError("TimeStart", "Already exists between the time span");
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries4 = aa => aa.EmpID == obj.EmployeeID && obj.DateStarted <= aa.ToDate && aa.FromDate <= obj.DateEnded && aa.IsHalf != true;
                if (VATLeaveApplicationService.GetIndexSpecific(SpecificEntries4).Count() > 0)
                {
                    ModelState.AddModelError("DateStarted", "Leave already applied for one or more days");
                }
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
                    JobCardESSPService.SingleDayPostCreate(obj, LoggedInUser);
                    ToasterMessages.Add("Job card applied successfully !");
                    Session["ToasterMessages"] = ToasterMessages;
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                SingleDayJobCardHelper(obj);
                return PartialView("SingleDay", obj);
            }
        }
        #endregion
        #region        //DELETION OF JOBCARD

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(JobCardESSPService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(JobCardApp obj)
        {

            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries2 = c => (c.PJobCardAppID == obj.PJobCardAppID);
            VEP_JobCardApplication vpjca = VEPJobCardApplicationService.GetIndexSpecific(SpecificEntries2).First();
            JobCardESSPService.PostDelete(obj);
            // Disable Notifications
            int notiTypeID1 = Convert.ToInt32(NotificationTypeJCEnum.JCPending);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.EmployeeID == LoggedInUser.UserEmpID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == vpjca.PJobCardAppID);
            DDService.DeleteNotification(SpecificEntries);
            ToasterMessages.Add("Job card Deleted successfully !");
            Session["ToasterMessages"] = ToasterMessages;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region VIEWING DETAILS OF JOBCARDS
        public ActionResult Detail(int? id)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            VMESSPJobCardDetail vMESSPJobCardDetail = JobCardESSPService.GetJobCardEmpDetail(id, vmf);
            return View(vMESSPJobCardDetail);
        }

        #endregion

        #region -- Private Helper Methods --
        private void MultipleDayJobCardHelper(JobCardApp obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.EmpNo = LoggedInUser.OEmpID;
            ViewBag.JobCardTypeID = new SelectList(DDService.GetJobCardType().ToList().Where(aa => aa.PJobCardTypeID != 1 && aa.PJobCardTypeID != 3 && aa.PJobCardTypeID != 4 && aa.PJobCardTypeID != 9 && aa.PJobCardTypeID != 10).ToList(), "PJobCardTypeID", "JobCardName");

        }
        private void SingleDayJobCardHelper(JobCardApp obj)
        {

            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.EmpNo = LoggedInUser.OEmpID;
            ViewBag.JobCardTypeID = new SelectList(DDService.GetJobCardType().ToList().Where(aa => aa.PJobCardTypeID != 1 && aa.PJobCardTypeID != 3 && aa.PJobCardTypeID != 4).ToList(), "PJobCardTypeID", "JobCardName", obj.JobCardTypeID);

        }
        #endregion
        public ActionResult ApprovedAll(int?[] SelectedJcAppIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            string Message = "";
            foreach (var item in SelectedJcAppIds)
            {
                VMESSPCommon vmESSPCommon = new VMESSPCommon();
                vmESSPCommon.PID = item;
                vmESSPCommon.Comment = "";
                Message = JobCardESSPService.ApproveJobCard(vmESSPCommon, LoggedInUser, Message);

            }
            if (Message != "")
            {
                ToasterMessages.Add(Message);
                Session["ToasterMessages"] = ToasterMessages;

            }
            return RedirectToAction("PendingJobCardIndex");
        }

    }
}