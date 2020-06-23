
using AppESSP.App_Start;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using ESSPSERVICE.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace AppESSP.Controllers
{
    [CustomActionFilter]
    public class HomeController : BaseController
    {
        IEntityService<VHR_AppUser> VHRAppUserService;
        IEntityService<AppUser> AppUserService;
        IEntityService<AppUserLocation> AppUserLocationService;
        IEntityService<AppUserDepartment> AppUserDepartmentService;
        IEntityService<VHR_EmployeeProfile> VHREmployeeProfileService;

        public List<string> ToasterMessages = new List<string>();
        IDDService DDService;
        //IRMSRepository<AreaOfInterest> AreaOfInterestService;
        public HomeController(IEntityService<VHR_AppUser> vHRAppUserService, IEntityService<AppUserLocation> appUserLocationService,
            IEntityService<VHR_EmployeeProfile> vHREmployeeProfileService, IDDService dDService, IEntityService<AppUser> appUserService, IEntityService<AppUserDepartment> appUserDepartmentService
          )
        {
            VHRAppUserService = vHRAppUserService;
            AppUserService = appUserService;
            AppUserLocationService = appUserLocationService;
            VHREmployeeProfileService = vHREmployeeProfileService;
            DDService = dDService;
            AppUserDepartmentService = appUserDepartmentService;
        }
        //public ActionResult LoadDashboard()
        //{
        //    return View("Index");
        //}
        // GET: Home
        public ActionResult Index()
        {
            if (Session["LoggedInUser"] != null)
                return RedirectToAction("MainContainer", "AttendanceDashboard");
            else
                return View("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            AppUser Obj = new AppUser();
            if (Session["LoggedInUser"] != null)
                return RedirectToAction("MainContainer", "AttendanceDashboard");
            else
                return View(Obj);
        }
        [HttpPost]
        public ActionResult Login(AppUser Obj)
        {
            Session["VMATDashboard"] = "";
            //string EncryptedPassword = App_Start.AppAssistant.Encrypt(Obj.Password);
            Expression<Func<VHR_AppUser, bool>> SpecificEntries1 = aa => aa.UserName == Obj.UserName && aa.Password == Obj.Password && aa.UserStatus == true;
            if (VHRAppUserService.GetIndexSpecific(SpecificEntries1).Count() > 0)
            {
                VHR_AppUser vm = VHRAppUserService.GetIndexSpecific(SpecificEntries1).First();
                Expression<Func<AppUserLocation, bool>> SpecificEntries = c => c.UserID == vm.PUserID;
                Expression<Func<AppUserDepartment, bool>> SpecificEntries2 = c => c.UserID == vm.PUserID;
                VMLoggedUser vmLoggedUser = GetLoggedInUser(vm, AppUserLocationService.GetIndexSpecific(SpecificEntries),AppUserDepartmentService.GetIndexSpecific(SpecificEntries2));
                Session["LoggedInUser"] = vmLoggedUser;
                //DDService.SaveAuditLog(vm.PUserID, AuditFormAttendance.Home, AuditTypeCommon.LogIN, 0, App_Start.AppAssistant.GetClientMachineInfo());
                if (vm.UserRoleID == "U")
                {
                    return RedirectToAction("TimeOfficeDashboard", "AttendanceDashboard");
                }
                else
                    return RedirectToAction("MainContainer", "AttendanceDashboard");
            }
            else
            {

                ModelState.AddModelError("Password", "The username or password is incorrect");
                return View("Login", Obj);
            }

        }
        [HttpGet]
        public ActionResult LoadDashboard()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (LoggedInUser.UserRoleID == "U")
            {
                return RedirectToAction("TimeOfficeDashboard", "AttendanceDashboard");
            }
            else
                return RedirectToAction("MainContainer", "AttendanceDashboard");
        }
        private VMLoggedUser GetLoggedInUser(VHR_AppUser vm, List<AppUserLocation> userLocationList, List<AppUserDepartment> userDepartmentList)
        {
            VMLoggedUser obj = new VMLoggedUser();
            obj.PUserID = vm.PUserID;
            obj.UserEmpID = vm.UserEmpID;
            obj.UserName = vm.UserName;
            obj.UserStatus = vm.UserStatus;
            obj.UserLastActiveDate = vm.UserLastActiveDate;
            obj.UserAccessTypeID = vm.UserAccessTypeID;
            obj.UserRoleID = vm.UserRoleID;
            obj.UserEmployeeName = vm.UserEmployeeName;
            obj.UserFPID = vm.UserFPID;
            obj.UserJobTitleID = vm.UserJobTitleID;
            obj.UserLocationID = vm.UserLocationID;
            obj.UserJobTitleName = vm.UserJobTitleName;
            obj.UserLocationName = vm.UserLocationName;
            obj.MLeave = vm.MLeave;
            obj.HasESSP = vm.HasESSP;
            obj.LeavePolicy = vm.LeavePolicy;
            obj.LeaveApplication = vm.LeaveApplication;
            obj.LeaveQuota = vm.LeaveQuota;
            obj.LeaveCF = vm.LeaveCF;
            obj.MShift = vm.MShift;
            obj.Shift = vm.Shift;
            obj.ShiftChange = vm.ShiftChange;
            obj.ShiftChangeEmp = vm.ShiftChangeEmp;
            obj.Roster = vm.Roster;
            obj.MOvertime = vm.MOvertime;
            obj.OvertimePolicy = vm.OvertimePolicy;
            obj.OvertimeAP = vm.OvertimeAP;
            obj.OvertimeENCPL = vm.OvertimeENCPL;
            obj.MAttendanceEditor = vm.MAttendanceEditor;
            obj.JobCard = vm.JobCard;
            obj.DailyAttEditor = vm.DailyAttEditor;
            obj.MonthlyAttEditor = vm.MonthlyAttEditor;
            obj.CompanyStructure = vm.CompanyStructure;
            obj.MSettings = vm.MSettings;
            obj.Reader = vm.Reader;
            obj.Holiday = vm.Holiday;
            obj.DownloadTime = vm.DownloadTime;
            obj.ServiceLog = vm.ServiceLog;
            obj.MUser = vm.MUser;
            obj.AppUser = vm.AppUser;
            obj.AppUserRole = vm.AppUserRole;
            obj.Employee = vm.Employee;
            //obj.OtherEmployee = vm.OtherEmployee;
            obj.Crew = vm.Crew;
            obj.OUCommon = vm.OUCommon;
            obj.JTCommon = vm.JTCommon;
            obj.FinancialYear = vm.FinancialYear;
            obj.PayrollPeriod = vm.PayrollPeriod;
            obj.TMSAdd = vm.TMSAdd;
            obj.TMSEdit = vm.TMSEdit;
            obj.TMSView = vm.TMSView;
            obj.TMSDelete = vm.TMSDelete;
            obj.LineManagerID = vm.LineManagerID;
            obj.MCompany = vm.MCompany;
            obj.MAttendance = vm.MAttendance;
            obj.Reports = vm.Reports;
            obj.OEmpID = vm.OEmpID;
            obj.UserLoctions = userLocationList;
            obj.UserDepartments = userDepartmentList;
            obj.LineManagerID = vm.LineManagerID;
            obj.LMEmployeeName = vm.LMEmployeeName;
            obj.RMS = vm.RMS;
            obj.RMSPosition = vm.RMSPosition;
            obj.RMSRequisition = vm.RMSRequisition;
            obj.RMSShortlisting = vm.RMSShortlisting;
            obj.RMSTestManagement = vm.RMSTestManagement;
            obj.RMSInterviewManagement = vm.RMSInterviewManagement;
            obj.RMSCandidateManager = vm.RMSCandidateManager;
            obj.RMSMeritList = vm.RMSMeritList;
            obj.RMSHiringNote = vm.RMSHiringNote;
            obj.RMSReporting = vm.RMSReporting;
            obj.PMS = vm.PMS;
            obj.PMSBellCurve = vm.PMSBellCurve;
            obj.PMSCompetency = vm.PMSCompetency;
            obj.PMSSetting = vm.PMSSetting;
            obj.PMSCycle = vm.PMSCycle;
            obj.PROBATIONEVALUATION = vm.PROBATIONEVALUATION;
            obj.FeedbackSession = vm.FeedbackSession;
            return obj;
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(VHR_AppUser obj)
        {
            if (obj.OfficialEmailID == null || obj.OfficialEmailID == "")
                ModelState.AddModelError("OfficialEmailID", "Official Email Address is  Mandatory");
            Expression<Func<VHR_AppUser, bool>> SpecificEntries = c => c.OfficialEmailID == obj.OfficialEmailID && c.UserStatus == true;
            List<VHR_AppUser> dbVHR_AppUsers = VHRAppUserService.GetIndexSpecific(SpecificEntries);
            if (VHRAppUserService.GetIndexSpecific(SpecificEntries).ToList().Count > 0)
            {
                if (ModelState.IsValid)
                {

                    var Password = App_Start.AppAssistant.Decrypt(dbVHR_AppUsers.First().Password);
                    var callbackUrl = "http://essp-portal/";
                    DDService.GenerateEmail(dbVHR_AppUsers.First().OfficialEmailID, "", "Forget Password", "<html><head><meta content=\"text/html; charset = utf - 8\" /></head><body><p>Dear Employee, " + " </p>" +
                        "<p>This is with reference to your request for forgetting password at Employee Self Service Portal. </p>" +
                        "<p>UserName : <u><strong>" + dbVHR_AppUsers.First().UserName + "</u></strong><p>" +
                        "<p>Password : <u><strong>" + Password + "</u></strong><p>" +
                        " <p>Please click <a href=\"" + callbackUrl + "\">here</a> to login to your profile.</p>" +
                        "<div><strong> Best Regards</strong></div><div><strong>Bestway Cement Limited</strong> </div></body></html>", dbVHR_AppUsers.First().PUserID, Convert.ToInt32(NotificationTypeEmail.ForgetPassword));

                }
                return RedirectToAction("EmailSent", "Home");
            }
            else
            {
                ModelState.AddModelError("OfficialEmailID", "Not registered email");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            //DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Home, AuditTypeCommon.LogOut, 0, App_Start.AppAssistant.GetClientMachineInfo());
            Session["LoggedInUser"] = null;
            Session["LoggedInUser"] = null;
            Session["VMATDashboard"] = null;
            Session["FiltersModel"] = null;
            return View("Login");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            // throw new System.ArgumentException("Parameter cannot be null", "original");
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(AppUser obj)
        {
            string Password = Request.Form["Password"];
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            AppUser user = AppUserService.GetEdit(LoggedInUser.PUserID);
            user.Password = App_Start.AppAssistant.Encrypt(Password);
            AppUserService.PostEdit(user);
            return RedirectToAction("MainContainer", "AttendanceDashboard");
        }
        public ActionResult ClearToasterSession()
        {
            try
            {
                Session["ToasterMessages"] = new List<string>();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult EmailSent()
        {
            return View();
        }
    }
}