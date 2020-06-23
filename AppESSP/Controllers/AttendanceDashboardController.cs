using AppESSP.Areas.Reporting.BusinessLogic;
using AppESSP.Areas.Reporting.BusinessLogic.Attendance;
using AppESSP.Helper;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using ESSPSERVICE.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Controllers
{
    public class AttendanceDashboardController : BaseController
    {
        IEntityService<VHR_EmployeeProfile> VHREmployeeProfileService;
        IEntityService<VAT_DailyAttendance> VATDailyAttendanceService;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        IDDService DDService;
        public DateTime dateS { get; set; }
        public DateTime dateE { get; set; }
        public AttendanceDashboardController(IEntityService<VHR_EmployeeProfile> vHREmployeeProfileService,
            IEntityService<VAT_DailyAttendance> vATDailyAttendanceService, IGetSpecificEmployeeService getSpecificEmployeeService,
            IDDService dDService)
        {
            VHREmployeeProfileService = vHREmployeeProfileService;
            VATDailyAttendanceService = vATDailyAttendanceService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
            DDService = dDService;
        }
        // GET: AttendanceDashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainContainer()
        {
            // Set First Time Dashboard Session
            BootstrapDashboardSession();
            VMAttendanceDashboard vm = new VMAttendanceDashboard();
            vm = Session["VMATDashboard"] as VMAttendanceDashboard;
            return View(vm);
        }
        // Gnereate Bar Chart based upon values stored in session
        public ActionResult LoadPieChart()
        {
            VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
            dateS = vmDashboard.StartDate;
            dateE = vmDashboard.EndDate;
            DMPieChartParentModel vm = new DMPieChartParentModel();
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_DailyAttendance> AttList = new List<VAT_DailyAttendance>();
            // Apply User Access Role
            if (LoggedInUser.UserAccessTypeID == 1) // Normal
            {
                List<VHR_EmployeeProfile> emps = EmployeeLM.GetReportingEmployees(VHREmployeeProfileService.GetIndex(), LoggedInUser);
                string query = QueryBuilder.GetReportQueryForLoggedUser(LoggedInUser, emps);
                if (query != "")
                    query = " and " + query;
                DataTable dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AttDate >= " + "'" + vmDashboard.StartDate.ToString("yyyy-MM-dd") + "'" + " and AttDate <= " + "'" + vmDashboard.EndDate.ToString("yyyy-MM-dd") + "'" + " ) " + query);
                AttList = dataTable.ToList<VAT_DailyAttendance>();
            }
            else if (LoggedInUser.UserAccessTypeID == 2) // Location Access
            {
                foreach (var item in LoggedInUser.UserLoctions)
                {
                    Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= dateS && c.AttDate <= dateE && c.LocationID == item.LocationID;
                    AttList.AddRange(VATDailyAttendanceService.GetIndexSpecific(SpecificEntries));
                }
            }
            else if (LoggedInUser.UserAccessTypeID == 3) // All
            {
                Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= dateS && c.AttDate <= dateE;
                AttList = VATDailyAttendanceService.GetIndexSpecific(SpecificEntries);
            }
            else if (LoggedInUser.UserAccessTypeID == 4) // Department
            {
                foreach (var item in LoggedInUser.UserDepartments)
                {
                    Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= dateS && c.AttDate <= dateE && c.OUCommonID == item.DepartmentID;
                    AttList.AddRange(VATDailyAttendanceService.GetIndexSpecific(SpecificEntries));
                }
            }
            //Filter based on Graph Type
            vm = DashboardManager.ApplyGraphTypeItems(vm, AttList, vmDashboard.GraphType);
            // Filter based on Graph For
            vm = DashboardManager.ApplyGraphForItems(vm, vmDashboard.UserGraphType, vmDashboard);
            if (vm.ChildList != null && vm.ChildList.Count() > 0)
            {
                return PartialView("RenderPieChart", vm);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }
        // save buttons values in session
        public ActionResult SaveBtnEventsInSession(string GraphType)
        {
            VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
            if (GraphType != "")
                vmDashboard.GraphType = GraphType;
            Session["VMATDashboard"] = vmDashboard;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveLabelEventsInSession(int? id)
        {
            VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
            if (id != 0)
                vmDashboard.ID = (int)id;
            if (vmDashboard.UserGraphType == UserGraphType.HasMultipleCommonOU)
                vmDashboard.UserGraphType = UserGraphType.HasMultipleOU;
            else if (vmDashboard.UserGraphType == UserGraphType.HasMultipleOU)
                vmDashboard.UserGraphType = UserGraphType.SimpleLM;
            else if (vmDashboard.UserGraphType == UserGraphType.SimpleLM)
                vmDashboard.UserGraphType = UserGraphType.Single;
            Session["VMATDashboard"] = vmDashboard;
            return Json(vmDashboard.GraphType.ToString(), JsonRequestBehavior.AllowGet);
        }
        //Save Attendance 
        public ActionResult SaveSelectedDateInSession(string dateStart, string dateEnd)
        {
            VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
            if (vmDashboard == null)
            {
                vmDashboard = new VMAttendanceDashboard();
            }
            vmDashboard.StartDate = Convert.ToDateTime(dateStart);
            vmDashboard.EndDate = Convert.ToDateTime(dateEnd);
            Session["VMATDashboard"] = vmDashboard;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> LoadEmployeeAttendance(int? id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                List<VAT_DailyAttendance> AttList = new List<VAT_DailyAttendance>();
                VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
                dateS = vmDashboard.StartDate;
                dateE = vmDashboard.EndDate;
                Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= dateS && c.AttDate <= dateE && c.EmpID == id;
                AttList = VATDailyAttendanceService.GetIndexSpecific(SpecificEntries);
                string HeaderLeft = "";
                string HeaderRight = AttList.First().EmployeeName + " (" + AttList.First().DesignationName + ")";
                string HeaderDescription = "Organization Unit: " + AttList.First().OUName;
                switch (vmDashboard.GraphType)
                {
                    case "LateIn":
                        HeaderLeft = "Late In Details";
                        AttList = AttList.Where(aa => aa.LateIn > 0).ToList();
                        break;
                    case "LateOut":
                        HeaderLeft = "Late Out Details";
                        AttList = AttList.Where(aa => aa.LateOut > 0).ToList();
                        break;
                    case "EarlyIn":
                        HeaderLeft = "Early In Details";
                        AttList = AttList.Where(aa => aa.EarlyIn > 0).ToList();
                        break;
                    case "EarlyOut":
                        HeaderLeft = "Early Out Details";
                        AttList = AttList.Where(aa => aa.EarlyOut > 0).ToList();
                        break;
                    case "Absent":
                        HeaderLeft = "Absent Details";
                        AttList = AttList.Where(aa => aa.AbDays > 0).ToList();
                        break;
                    case "Leave":
                        HeaderLeft = "Leaves Details";
                        AttList = AttList.Where(aa => aa.LeaveDays > 0).ToList();
                        break;
                    case "OfficialDuty":
                        HeaderLeft = "Official Duty Details";
                        AttList = AttList.Where(aa => aa.TimeIn == null && aa.TimeOut == null && aa.Remarks != null && aa.Remarks.Contains("JC:")).ToList();
                        break;
                }
                ViewBag.HeaderLeft = HeaderLeft;
                ViewBag.HeaderRight = HeaderRight;
                ViewBag.HeaderDescription = HeaderDescription;
                return View("EmployeeDetail", AttList);
            });
        }
        private void BootstrapDashboardSession()

        {
            VMAttendanceDashboard vmATDashboard = new VMAttendanceDashboard();
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            vmATDashboard.StartDate = DateTime.Today.AddDays(-7);
            vmATDashboard.EndDate = DateTime.Today;
            vmATDashboard.EmpID = (int)LoggedInUser.UserEmpID;
            if (LoggedInUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                List<VHR_EmployeeProfile> emps = EmployeeLM.GetReportingEmployees(VHREmployeeProfileService.GetIndex(), LoggedInUser);
                var dss = emps.Select(aa => aa.OUID).Distinct();
                if (emps.Count == 1)
                    vmATDashboard.UserGraphType = UserGraphType.Single;
                else if (emps.Select(aa => aa.OUID).Distinct().Count() == 1)
                    vmATDashboard.UserGraphType = UserGraphType.SimpleLM;
                else if (emps.Select(aa => aa.OUID).Distinct().Count() > 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleOU;
                else if (emps.Select(aa => aa.OUID).Distinct().Count() == 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleOU;
                else if (emps.Select(aa => aa.OUID).Distinct().Count() > 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleCommonOU;
                else
                    vmATDashboard.UserGraphType = UserGraphType.SimpleLM;
            }
            if (LoggedInUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                List<VHR_EmployeeProfile> emps = EmployeeLM.GetReportingEmployees(VHREmployeeProfileService.GetIndex(), LoggedInUser);
                var dss = emps.Select(aa => aa.OUCommonID).Distinct();
                if (emps.Count == 1)
                    vmATDashboard.UserGraphType = UserGraphType.Single;
                else if (emps.Select(aa => aa.OUCommonID).Distinct().Count() == 1)
                    vmATDashboard.UserGraphType = UserGraphType.SimpleLM;
                else if (emps.Select(aa => aa.OUCommonID).Distinct().Count() > 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleOU;
                else if (emps.Select(aa => aa.OUCommonID).Distinct().Count() == 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleOU;
                else if (emps.Select(aa => aa.OUCommonID).Distinct().Count() > 1)
                    vmATDashboard.UserGraphType = UserGraphType.HasMultipleCommonOU;
                else
                    vmATDashboard.UserGraphType = UserGraphType.SimpleLM;
            }
            else
                vmATDashboard.UserGraphType = UserGraphType.HasMultipleCommonOU;
            vmATDashboard.GraphType = "LateIn";
            Session["VMATDashboard"] = vmATDashboard;
        }
        public async Task<ActionResult> TimeOfficeDashboard()
        {
            return await Task.Run<ActionResult>(() =>
            {
                VMTimeOfficeDashboard vmTimeOfficeDashboard = new VMTimeOfficeDashboard();
                VMAttendanceDashboard vmDashboard = Session["VMATDashboard"] as VMAttendanceDashboard;
                if (vmDashboard == null)
                {
                    vmTimeOfficeDashboard.StartDate = DateTime.Today.AddDays(-6);
                    vmTimeOfficeDashboard.EndDate = DateTime.Today;
                }
                else
                {
                    vmTimeOfficeDashboard.StartDate = vmDashboard.StartDate;
                    vmTimeOfficeDashboard.EndDate = vmDashboard.EndDate;
                }
                List<VMPVTMDashboard> vmList = new List<VMPVTMDashboard>();
                DateTime dt = vmTimeOfficeDashboard.StartDate;
                while (dt <= vmTimeOfficeDashboard.EndDate)
                {
                    if (dt <= DateTime.Today)
                    {
                        VMPVTMDashboard vm = new VMPVTMDashboard();
                        vm.Date = dt.ToString("dd-MMM-yyyy");
                        vm.DateValue = dt;
                        VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                        List<VAT_DailyAttendance> dbVDailyAttendance = GetSpecificEmployeeService.GetSpecificAttendance(LoggedInUser, dt);
                        int TotalEmps = dbVDailyAttendance.Count();
                        if (TotalEmps > 0)
                        {
                            bool IsNotZero = false;
                            int Percentage = 0;
                            // Get Absent Details
                            int Absent = dbVDailyAttendance.Where(aa => aa.AbDays > 0).Count();
                            if (Absent > 0)
                                IsNotZero = true;
                            Percentage = (Absent * 100) / TotalEmps;
                            vm.AbsentsPercentWidth = Percentage.ToString() + "%";
                            vm.AbsentsPercentLabel = Percentage.ToString() + "%";
                            vm.AbsentsPercentDesc = Percentage.ToString() + "% Absentism";
                            vm.AbsentsLink = "";
                            // Get Missing Details
                            int Missing = dbVDailyAttendance.Where(aa => (aa.TimeIn != null && aa.TimeOut == null) || (aa.TimeIn == null && aa.TimeOut != null)).Count();
                            if (Missing > 0)
                                IsNotZero = true;
                            Percentage = (Missing * 100) / TotalEmps;
                            vm.MissingPercentWidth = Percentage.ToString() + "%";
                            vm.MissingPercentLabel = Percentage.ToString() + "%";
                            vm.MissingPercentDesc = Percentage.ToString() + "% Missing";
                            // Get Late IN 240 Details
                            int LateIn = dbVDailyAttendance.Where(aa => aa.LateIn > 240).Count();
                            if (LateIn > 0)
                                IsNotZero = true;
                            Percentage = (LateIn * 100) / TotalEmps;
                            vm.ShiftLateInPercentWidth = Percentage.ToString() + "%";
                            vm.ShiftLateInPercentLabel = Percentage.ToString() + "%";
                            vm.ShiftLateInPercentDesc = Percentage.ToString() + "% Missing";
                            // Get EarlyOut 240 Details
                            int EarlyOut = dbVDailyAttendance.Where(aa => aa.EarlyOut > 240).Count();
                            if (EarlyOut > 0)
                                IsNotZero = true;
                            Percentage = (EarlyOut * 100) / TotalEmps;
                            vm.ShiftEarlyOutPercentWidth = Percentage.ToString() + "%";
                            vm.ShiftEarlyOutPercentLabel = Percentage.ToString() + "%";
                            vm.ShiftEarlyOutPercentDesc = Percentage.ToString() + "% Missing";
                            if (IsNotZero == true)
                                vmList.Add(vm);
                        }
                    }
                    dt = dt.AddDays(1);
                }
                vmTimeOfficeDashboard.VMPVTMDashboard = vmList.OrderByDescending(aa => aa.DateValue).ToList();
                return View(vmTimeOfficeDashboard);
            });
        }
        //public ActionResult PVTMDashboard(DateTime? date)
        //{
        //    VMPVTMDashboard vm = new VMPVTMDashboard();
        //    vm.Date = date.Value.ToString("dd-MMM-yyyy");
        //    VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
        //    List<VAT_DailyAttendance> dbVDailyAttendance = GetSpecificEmployeeService.GetSpecificAttendance(LoggedInUser, (DateTime)date);
        //    int TotalEmps = dbVDailyAttendance.Count();
        //    if (TotalEmps > 0)
        //    {
        //        int Percentage = 0;
        //        // Get Absent Details
        //        int Absent = dbVDailyAttendance.Where(aa => aa.AbDays > 0).Count();
        //        Percentage = (Absent * 100) / TotalEmps;
        //        vm.AbsentsPercentWidth = Percentage.ToString() + "%";
        //        vm.AbsentsPercentLabel = Percentage.ToString() + "%";
        //        vm.AbsentsPercentDesc = Percentage.ToString() + "% Absentism";
        //        vm.AbsentsLink = "";
        //        // Get Missing Details
        //        int Missing = dbVDailyAttendance.Where(aa => (aa.TimeIn != null && aa.TimeOut == null) || (aa.TimeIn == null && aa.TimeOut != null)).Count();
        //        Percentage = (Missing * 100) / TotalEmps;
        //        vm.MissingPercentWidth = Percentage.ToString() + "%";
        //        vm.MissingPercentLabel = Percentage.ToString() + "%";
        //        vm.MissingPercentDesc = Percentage.ToString() + "% Missing";
        //        // Get Late IN 240 Details
        //        int LateIn = dbVDailyAttendance.Where(aa => aa.LateIn > 240).Count();
        //        Percentage = (LateIn * 100) / TotalEmps;
        //        vm.ShiftLateInPercentWidth = Percentage.ToString() + "%";
        //        vm.ShiftLateInPercentLabel = Percentage.ToString() + "%";
        //        vm.ShiftLateInPercentDesc = Percentage.ToString() + "% Missing";
        //        // Get EarlyOut 240 Details
        //        int EarlyOut = dbVDailyAttendance.Where(aa => aa.EarlyOut > 240).Count();
        //        Percentage = (EarlyOut * 100) / TotalEmps;
        //        vm.ShiftEarlyOutPercentDesc = Percentage.ToString() + "%";
        //        vm.ShiftEarlyOutPercentLabel = Percentage.ToString() + "%";
        //        vm.ShiftEarlyOutPercentDesc = Percentage.ToString() + "% Missing";
        //    }
        //    return View(vm);
        //}
    }

}