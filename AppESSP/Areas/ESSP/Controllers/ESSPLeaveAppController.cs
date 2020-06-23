using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.ESSP;
using ESSPSERVICE.Generic;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.ESSP.Controllers
{
    public class ESSPLeaveAppController : Controller
    {
        // GET: Attendance/EmpLeaveApp
        ILeavesESSPService ESSPLeaveService;
        ILeaveApplicationService LeaveApplicationService;
        IEntityService<VAT_LeaveApplication> VATLeaveApplicationService;
        IEntityService<Shift> ShiftService;
        IEntityService<VAT_LeaveApplicationFlow> LeaveApplicationFlowService;
        IEntityService<DailyAttendance> DailyAttendanceService;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IDDService DDService;
        public List<string> ToasterMessages = new List<string>();
        public ESSPLeaveAppController(ILeaveApplicationService leaveapplicationService, IEntityService<LeaveApplication> empleaveapplicationService, IEntityService<VAT_LeaveApplication> vATESSPLeaveApplication,
            IDDService dDService, ILeavesESSPService eSSPLeaveService, IEntityService<VAT_LeaveApplication> vATLeaveApplicationService, IEntityService<DailyAttendance> dailyAttendanceService
            , IEntityService<VAT_LeaveApplicationFlow> leaveApplicationFlowService, IEntityService<PayrollPeriod> payrollPeriodService, IEntityService<Shift> shiftService)
        {
            ESSPLeaveService = eSSPLeaveService;
            DDService = dDService;
            LeaveApplicationService = leaveapplicationService;
            VATLeaveApplicationService = vATLeaveApplicationService;
            DailyAttendanceService = dailyAttendanceService;
            LeaveApplicationFlowService = leaveApplicationFlowService;
            PayrollPeriodService = payrollPeriodService;
            ShiftService = shiftService;
        }
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
            int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeaveApproved);
            int notiTypeID2 = Convert.ToInt32(NTLeaveEnum.LeaveRejected);
            int notiTypeID3 = Convert.ToInt32(NTLeaveEnum.LeaveRecommend);
            int notiTypeID4 = Convert.ToInt32(NTLeaveEnum.LeaveReverttoLM);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1 || c.NotificationTypeID == notiTypeID2 || c.NotificationTypeID == notiTypeID3 || c.NotificationTypeID == notiTypeID4));
            DDService.DeleteNotification(SpecificEntries);
            List<VAT_LeaveApplication> dbVAT_LeaveApplication = ESSPLeaveService.GetIndex(LoggedInUser).OrderByDescending(aa => aa.PLeaveAppID).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                dbVAT_LeaveApplication = dbVAT_LeaveApplication.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PLeaveAppID.ToString().Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.LeaveTypeName.ToString().Contains(searchString)).ToList();
            }
            int pageSize = 500;
            int pageNumber = (page ?? 1);
            return View(dbVAT_LeaveApplication.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PendingLeaveApplicationIndex(string searchString, string currentFilter, int? page)
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
            List<VAT_LeaveApplication> dbVAT_LeaveApplication = ESSPLeaveService.GetPendingLeaveRequests(LoggedInUser).OrderByDescending(aa => aa.PLeaveAppID).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                dbVAT_LeaveApplication = dbVAT_LeaveApplication.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PLeaveAppID.ToString().Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.LeaveTypeName.ToString().Contains(searchString)).ToList();
            }
            int pageSize = 500;
            int pageNumber = (page ?? 1);
            return View(dbVAT_LeaveApplication.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult RecommendAll(int?[] SelectedLvAppIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            string Message = "";
            foreach (var item in SelectedLvAppIds)
            {
                VMESSPCommon vmESSPCommon = new VMESSPCommon();
                vmESSPCommon.PID = item;
                vmESSPCommon.Comment = "";
                Message = ESSPLeaveService.RecommendLeaveApplication(vmESSPCommon, LoggedInUser, Message);

            }
            if (Message != "")
            {
                ToasterMessages.Add(Message);
                Session["ToasterMessages"] = ToasterMessages;

            }
            return RedirectToAction("PendingLeaveApplicationIndex");
        }
        public ActionResult LeaveApplicationHistoryIndex(string searchString, string currentFilter, int? page)
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
            List<VAT_LeaveApplication> dbVAT_LeaveApplication = ESSPLeaveService.GetEmpLeaveHistory(LoggedInUser);
            if (!String.IsNullOrEmpty(searchString))
            {
                dbVAT_LeaveApplication = dbVAT_LeaveApplication.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PLeaveAppID.ToString().Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.LeaveTypeName.ToString().Contains(searchString)).ToList();
            }
            int pageSize = 500;
            int pageNumber = (page ?? 1);
            return View(dbVAT_LeaveApplication.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            LeaveApplication obj = new LeaveApplication();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(LeaveApplication lvapplication)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;

            if (lvapplication.LeaveAddress == "" || lvapplication.LeaveAddress == null)
                ModelState.AddModelError("LeaveAddress", "Contact number is mandatory !");
            if (lvapplication.LeaveAddress != null)
            {
                Match match = Regex.Match(lvapplication.LeaveAddress, @"^-*[0-9,\-]+$");
                if (!match.Success)
                {
                    ModelState.AddModelError("LeaveAddress", "Enter a valid Contact No");
                }
            }

            if (lvapplication.FromDate.Date > lvapplication.ToDate.Date)
                ModelState.AddModelError("FromDate", "From Date should be smaller than To Date");
            FinancialYear dbFinancialYear = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == lvapplication.FinancialYearID).First();
            if (lvapplication.ToDate >= dbFinancialYear.FYEndDate || lvapplication.ToDate <= dbFinancialYear.FYStartDate)
                ModelState.AddModelError("FromDate", "To Date must lie in selected financial year");
            if (lvapplication.ToDate > AppAssistant.MaxDate)
                ModelState.AddModelError("ToDate", "Date cannot be greater than " + AppAssistant.MaxDate.ToString("dd-MM-yyyy"));
            if (lvapplication.FromDate < AppAssistant.MinDate)
                ModelState.AddModelError("FromDate", "Date cannot be less than " + AppAssistant.MinDate.ToString("dd-MM-yyyy"));

            string _EmpNo = Request.Form["EmpNo"].ToString();
            List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
            VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).First();
            var RBValue = Request.Form["HalfLvHA"];

            if (_emp.First().LineManagerID == null)
                ModelState.AddModelError("EmpID", "There is no Line Manager associated with your profile. Please contact HR");
            {
                LeavePolicy lvPolicy = new LeavePolicy();
                LeaveType lvType = DDService.GetLeaveType().First(aa => aa.PLeaveTypeID == lvapplication.LeaveTypeID);
                if (_emp.Count == 0)
                {
                    ModelState.AddModelError("FromDate", "Emp No not exist");
                }
                else
                {
                    lvapplication.EmpID = _emp.FirstOrDefault().PEmployeeID;
                    lvPolicy = AssistantLeave.GetEmployeeLeavePolicyID(_emp, lvapplication.LeaveTypeID, DDService.GetLeavePolicy().ToList());
                    //lvType = db.Att_LeaveType.First(aa => aa.LvTypeID == lvapplication.LeaveTypeID);
                }
                if (lvapplication.IsHalf == false && lvapplication.IsDeducted == false)
                {
                    List<DailyAttendance> att = new List<DailyAttendance>();
                    Expression<Func<DailyAttendance, bool>> SpecificEntries = aa => aa.EmpID == lvapplication.EmpID && aa.AttDate >= lvapplication.FromDate && aa.AttDate <= lvapplication.ToDate;
                    att = DailyAttendanceService.GetIndexSpecific(SpecificEntries);
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
                float noofDays = LeaveApplicationService.CalculateNoOfDays(lvapplication, lvType, lvPolicy);
                if (lvapplication.LeaveTypeID == 2)
                {
                    if (LeaveApplicationService.CheckForALConsectiveDay(lvapplication))
                        ModelState.AddModelError("FromDate", "You have applied AL leave for previous date");
                }
                // Check for Minimum Days of Attachment of Sick Leave
                if (lvapplication.LeaveTypeID == 3)
                {
                    if (noofDays < lvPolicy.MinimumDays)
                    {
                        ModelState.AddModelError("LeaveTypeID", "Cannot Apply SL for Less than" + lvPolicy.MinimumDays.ToString() + " days");
                    }
                }
                if (lvapplication.LeaveTypeID == 11)
                {
                    if (noofDays < lvPolicy.MinimumDays)
                    {
                        ModelState.AddModelError("LeaveTypeID", "Cannot Apply Academic for Less than" + lvPolicy.MinimumDays.ToString() + " days");
                    }
                }
                if (lvapplication.LeaveTypeID == 12)
                {
                    if (noofDays < lvPolicy.MinimumDays)
                    {
                        ModelState.AddModelError("LeaveTypeID", "Cannot Apply CME/Workshop for Less than" + lvPolicy.MinimumDays.ToString() + " days");
                    }
                }
                //if (LeaveForHoliday(lvapplication))
                //{
                //    ModelState.AddModelError("LeaveTypeID", "This employee has Rest Or GZ for specific Day");
                //}
                //if (!CheckForLvTypeHasLvPolicyForEmp(_emp, lvType))
                //{
                //    ModelState.AddModelError("LeaveTypeID", "This employee does not have Leave Policy, Please add Leave Policy first");
                //}
                //if (!DDService.IsDateLieBetweenActivePayroll(lvapplication.FromDate))
                //    ModelState.AddModelError("FromDate", "Payroll Period is Closed for this date");
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
                if (ModelState.IsValid)
                {
                    if (LeaveApplicationService.CheckDuplicateLeave(lvapplication))
                    {
                        // max days

                        float CalenderDays = LeaveApplicationService.CalculateCalenderDays(lvapplication, lvType, lvPolicy);
                        lvapplication.ReturnDate = LeaveApplicationService.GetReturnDate(lvapplication, lvType, lvPolicy);
                        lvapplication.IsDeducted = false;
                        lvapplication.NoOfDays = noofDays;
                        lvapplication.CalenderDays = CalenderDays;
                        int _UserID = LoggedInUser.PUserID;
                        lvapplication.CreatedBy = _UserID;
                        if (lvPolicy.PLeavePolicyID == 0)
                        {
                            ESSPLeaveService.CreateLeave(lvapplication, lvType, LoggedInUser);
                            return RedirectToAction("Index");
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
                                                lvapplication.LineManagerID = (int)_emp.First().LineManagerID;
                                                ESSPLeaveService.CreateLeave(lvapplication, lvType, LoggedInUser);
                                                ToasterMessages.Add("Leave applied successfully.");
                                                Session["ToasterMessages"] = ToasterMessages;
                                                return Json(lvapplication.PLeaveAppID, JsonRequestBehavior.AllowGet);
                                            }
                                            //else
                                            //    ModelState.AddModelError("FromDate", "Leave Monthly Quota Exceeds");
                                        }
                                        else
                                            ModelState.AddModelError("LeaveTypeID", "Leave Balance Exceeds, Please check the balance");
                                    }
                                    else
                                        ModelState.AddModelError("LeaveTypeID", "Leave Quota does not exist");
                                }
                                else
                                {


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
        }
        [HttpPost]
        public ActionResult ValidateSLAttachment(LeaveApplication lvapplication)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            string _EmpNo = Request.Form["EmpNo"].ToString();
            LeavePolicy lvPolicy = new LeavePolicy();
            LeaveType lvType = DDService.GetLeaveType().First(aa => aa.PLeaveTypeID == lvapplication.LeaveTypeID);
            List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.OEmpID == _EmpNo).ToList();
            lvapplication.EmpID = _emp.FirstOrDefault().PEmployeeID;
            lvPolicy = AssistantLeave.GetEmployeeLeavePolicyID(_emp, lvapplication.LeaveTypeID, DDService.GetLeavePolicy().ToList());
            float noofDays = LeaveApplicationService.CalculateNoOfDays(lvapplication, lvType, lvPolicy);
            if (noofDays >= lvPolicy.AttachmentForDays)
                return Json("Required", JsonRequestBehavior.AllowGet);
            else
                return Json("NotRequired", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            return PartialView(ESSPLeaveService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(LeaveApplication obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries2 = c => (c.PLeaveAppID == obj.PLeaveAppID);
            VAT_LeaveApplication vlvapplication = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries2).First();
            ESSPLeaveService.PostDelete(obj);
            // Disable Notifications
            // Disable Notifications
            int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeavePending);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.EmployeeID == LoggedInUser.UserEmpID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == vlvapplication.PLeaveAppID);
            DDService.DeleteNotification(SpecificEntries);
            ToasterMessages.Add("Leave Deleted successfully.");
            Session["ToasterMessages"] = ToasterMessages;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        #region VIEWING DETAILS OF Leaves
        #region         /// FOR UPLOADING MEDICAL CERTIFICATE
        public ActionResult UploadFiles(int? id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    SaveFile(id);
                    return Json("File Uploaded Successfully");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }

        }
        public void SaveFile(int? id)
        {
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                //string filename = Path.GetFileName(Request.Files[i].FileName);  

                HttpPostedFileBase file = files[i];
                string fname;
                var checkextension = Path.GetExtension(file.FileName).ToLower();
                // Checking for Internet Explorer  

                if (checkextension == ".jpg" || checkextension == ".png")
                {
                    fname = id.ToString() + ".jpg";
                }
                else
                {
                    fname = id.ToString() + ".pdf";
                }
                // Get the complete folder path and store the file inside it.
                ESSPLeaveService.UpdatePathName(id, fname);
                fname = Path.Combine(Server.MapPath("~/UploadFiles/") + fname);
                file.SaveAs(fname);
            }
            // Returns message that successfully uploaded  
        }
        public FilePathResult OpenCertificate(int? fileName)
        {
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = aa => aa.PLeaveAppID == fileName;
            VAT_LeaveApplication vatleaveapp = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries).First();
            var dir = Server.MapPath("/UploadFiles");
            var checkextension = Path.GetExtension(vatleaveapp.PathName).ToLower();
            if (checkextension == ".pdf")
            {
                var path = Path.Combine(dir, fileName + ".pdf");
                return base.File(path, "application/pdf");
            }
            else
            {
                var path = Path.Combine(dir, fileName + ".jpg");
                return base.File(path, "image/jpeg");
            }
        }
        #endregion
        public ActionResult Detail(int? id)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            VMESSPLeaveDetails vMESSPLeaveDetail = ESSPLeaveService.GetESSPLeaveEmpDetail(id, vmf);
            return View(vMESSPLeaveDetail);
        }
        #endregion
        private void HelperMethod(LeaveApplication obj)
        {

            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.EmpNo = LoggedInUser.OEmpID;
            ViewBag.LineManagerID = new SelectList(DDService.GetUser().ToList().OrderBy(aa => aa.UserName).ToList(), "PUserID", "UserName", obj.LineManagerID);
            ViewBag.LeaveTypeID = new SelectList(DDService.GetLeaveType().ToList().Where(aa => aa.PLeaveTypeID != 4 && aa.PLeaveTypeID != 5 && aa.PLeaveTypeID != 6 && aa.PLeaveTypeID != 7 && aa.PLeaveTypeID != 8 && aa.PLeaveTypeID != 9 && aa.PLeaveTypeID != 10 && aa.PLeaveTypeID != 11 && aa.PLeaveTypeID != 12).ToList(), "PLeaveTypeID", "LeaveTypeName", obj.LeaveTypeID);
            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().Where(aa => aa.FYStatus == true).ToList().OrderByDescending(aa => aa.PFinancialYearID).ToList(), "PFinancialYearID", "FYName");
        }

    }
}