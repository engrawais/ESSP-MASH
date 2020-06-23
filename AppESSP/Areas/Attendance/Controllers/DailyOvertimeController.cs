using AppESSP.Controllers;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
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
    public class DailyOvertimeController : BaseController
    {
        // GET: Attendance/DailyOvertimeEditor
        IEntityService<DailyOvertime> DailyOvertimeService;
        IEntityService<DailyAttendance> DailyAttendanceService;
        IEntityService<VAT_DailyOvertime> VATDailyOvertimeService;
        IEntityService<Employee> EmployeeService;
        IEntityService<VHR_EmployeeProfile> vHR_EmployeeProfile;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IEntityService<LeaveCPLPool> LeaveCPLPoolService;
        IEntityService<LeaveCPLEmpBalance> LeaveCPLEmpBalanceService;
        IEntityService<LeaveQuotaYear> LeaveQuotaYearService;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        IDDService DDService;
        public DailyOvertimeController(IEntityService<DailyOvertime> dailyOvertimeService, IEntityService<VAT_DailyOvertime> vATDailyOvertimeService
            , IDDService dDService, IEntityService<Employee> employeeService, IEntityService<DailyAttendance> dailyAttendanceService, IGetSpecificEmployeeService getSpecificEmployeeService,
            IEntityService<LeaveCPLPool> dbLeavePoolCPL, IEntityService<LeaveCPLEmpBalance> dbLeaveBalanceCPL, IEntityService<LeaveQuotaYear> leaveQuotaYearService
            , IEntityService<PayrollPeriod> payrollPeriodService, IEntityService<VHR_EmployeeProfile> vHREmployeeProfile)
        {
            VATDailyOvertimeService = vATDailyOvertimeService;
            DailyOvertimeService = dailyOvertimeService;
            EmployeeService = employeeService;
            DDService = dDService;
            DailyAttendanceService = dailyAttendanceService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
            LeaveCPLPoolService = dbLeavePoolCPL;
            LeaveCPLEmpBalanceService = dbLeaveBalanceCPL;
            LeaveQuotaYearService = leaveQuotaYearService;
            PayrollPeriodService = payrollPeriodService;
            vHR_EmployeeProfile = vHREmployeeProfile;
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
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_DailyOvertime> list = GetSpecificEmployeeService.GetSpecificDailyOT(LoggedInUser).OrderByDescending(a => a.OTDate).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(aa => aa.OEmpID.Contains(searchString) || aa.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || aa.PDailyOTID.ToString().Contains(searchString)).ToList();

            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            DailyOvertime obj = new DailyOvertime();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(DailyOvertime obj)
        {
            string _EmpNo = Request.Form["EmpNo"].ToString();
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries121 = c => c.OEmpID == _EmpNo;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VHR_EmployeeProfile> _emp = vHR_EmployeeProfile.GetIndexSpecific(SpecificEntries121);
            if (obj.OTDate == null || obj.OTDate <= DateTime.Today.AddDays(-200))
                ModelState.AddModelError("OTDate", "OT Date must be valid");
            else
            {
                if (!DDService.IsDateLieBetweenActivePayroll(obj.OTDate.Value))
                    ModelState.AddModelError("OTDate", "Payroll Period is Closed for this date");
            }
            if ((obj.SingleEncashableOT == null && obj.SingleEncashableOT <= 0) && (obj.DoubleEncashbaleOT == null && obj.DoubleEncashbaleOT <= 0) && (obj.CPLOT == null && obj.CPLOT <= 0))
                ModelState.AddModelError("SingleEncashableOT", "Overtime must be valid");
            if (_emp.Count == 0)
            {
                ModelState.AddModelError("OTDate", "Emp No not exist");
            }
            else
            {
                // int? EmpLocID = _emp.First().LocationID;
                //if (LoggedInUser.UserLoctions.Where(aa => aa.LocationID == EmpLocID).Count() == 0)
                //ModelState.AddModelError("OTDate", "You do not have rights to add overtime for this employee");
                int? EmpDepID = _emp.First().OUCommonID;
                if (LoggedInUser.UserDepartments.Where(aa => aa.DepartmentID == EmpDepID).Count() == 0)
                    ModelState.AddModelError("OTDate", "You do not have rights to add overtime for this employee");
                obj.EmployeeID = _emp.First().PEmployeeID;
                Expression<Func<DailyOvertime, bool>> SpecificEntries2 = c => c.EmployeeID == obj.EmployeeID && c.OTDate == obj.OTDate;
                if (DailyOvertimeService.GetIndexSpecific(SpecificEntries2).Count > 0)
                    ModelState.AddModelError("OTDate", "Already have OT for this date");
            }

            if (ModelState.IsValid)
            {
                if (obj.SingleEncashableOT > 0)
                {
                    obj.SingleEncashableOT = obj.SingleEncashableOT * 60;
                }
                if (obj.DoubleEncashbaleOT > 0)
                {
                    obj.DoubleEncashbaleOT = obj.DoubleEncashbaleOT * 60;
                }
                if (obj.CPLOT > 0)
                {
                    obj.CPLOT = obj.CPLOT * 60;
                }
                obj.AddedByUserID = LoggedInUser.PUserID;
                obj.AddedDate = DateTime.Now;
                DailyOvertimeService.PostCreate(obj);
                // Update Attendance Data
                Expression<Func<DailyAttendance, bool>> SpecificEntries2 = c => c.EmpID == obj.EmployeeID && c.AttDate == obj.OTDate;
                List<DailyAttendance> attDatas = DailyAttendanceService.GetIndexSpecific(SpecificEntries2).ToList();
                if (attDatas.Count > 0)
                {
                    DailyAttendance attdata = attDatas.First();
                    if (obj.SingleEncashableOT > 0)
                    {
                        attdata.ApprovedOT = (short)obj.SingleEncashableOT;

                    }
                    if (obj.DoubleEncashbaleOT > 0)
                    {
                        attdata.ApprovedDoubleOT = (short)obj.DoubleEncashbaleOT;

                    }
                    if (obj.CPLOT > 0)
                    {
                        attdata.ApprovedCPL = (short)obj.CPLOT;
                    }
                    DailyAttendanceService.PostEdit(attdata);
                    DDService.ProcessMonthlyAttendance((DateTime)obj.OTDate, (int)obj.EmployeeID, obj.EmployeeID.ToString());
                    // Update Employee Pool
                    LeaveCPLPool dbLeavePoolCPL = new LeaveCPLPool();
                    Expression<Func<LeaveCPLPool, bool>> SpecificEntries3 = c => c.EmployeeID == obj.EmployeeID;

                    if (LeaveCPLPoolService.GetIndexSpecific(SpecificEntries3).Count() > 0)
                    {

                        dbLeavePoolCPL = LeaveCPLPoolService.GetIndexSpecific(SpecificEntries3).First();
                        dbLeavePoolCPL.LastEntryDateTime = DateTime.Now;
                        dbLeavePoolCPL.RemainingHours = dbLeavePoolCPL.RemainingHours + (obj.CPLOT / 60);
                        if (dbLeavePoolCPL.RemainingHours == null)
                        {
                            dbLeavePoolCPL.RemainingHours = 0;
                            dbLeavePoolCPL.RemainingHours = (dbLeavePoolCPL.RemainingHours) + (obj.CPLOT / 60);
                        }                      
                        dbLeavePoolCPL.TotalHours =(dbLeavePoolCPL.TotalHours) + (obj.CPLOT / 60);
                        if (dbLeavePoolCPL.TotalHours == null)
                        {
                            dbLeavePoolCPL.TotalHours = 0;
                            dbLeavePoolCPL.TotalHours =( dbLeavePoolCPL.TotalHours) + (obj.CPLOT / 60);
                        }
                        dbLeavePoolCPL.CPLDays = 0;
                        LeaveCPLPoolService.PostEdit(dbLeavePoolCPL);
                    }
                    else
                    {
                        dbLeavePoolCPL = new LeaveCPLPool();
                        dbLeavePoolCPL.EmployeeID = obj.EmployeeID;
                        dbLeavePoolCPL.LastEntryDateTime = DateTime.Now;
                        dbLeavePoolCPL.RemainingHours = obj.CPLOT / 60;
                        dbLeavePoolCPL.TotalHours = obj.CPLOT / 60;
                        dbLeavePoolCPL.CPLDays = 0;
                        LeaveCPLPoolService.PostCreate(dbLeavePoolCPL);
                    }
                    // Update CPL
                    if (dbLeavePoolCPL.RemainingHours >= 4)
                    {

                        int total = (int)(dbLeavePoolCPL.RemainingHours / 4);
                        float remaining = (float)(dbLeavePoolCPL.RemainingHours - (total * 4));
                        float days = (float)(total / 2.0);
                        // Add into CPL Balance
                        LeaveCPLEmpBalance dbLeaveBalanceCPL = new LeaveCPLEmpBalance();
                        dbLeaveBalanceCPL.EmployeeID = obj.EmployeeID;
                        dbLeaveBalanceCPL.EntryDateTime = DateTime.Now;
                        dbLeaveBalanceCPL.ExpireDate = DateTime.Today.AddDays(60);
                        dbLeaveBalanceCPL.IsExpire = false;
                        dbLeaveBalanceCPL.RemainingDays = days;
                        dbLeaveBalanceCPL.TotalDays = days;
                        dbLeaveBalanceCPL.Used = 0;
                        LeaveCPLEmpBalanceService.PostCreate(dbLeaveBalanceCPL);
                        // update CPL Pool
                        dbLeavePoolCPL.RemainingHours = dbLeavePoolCPL.RemainingHours - (days * 8);
                        //dbLeavePoolCPL.RemainingHours = dbLeavePoolCPL.RemainingHours + remaining;
                        LeaveCPLPoolService.PostEdit(dbLeavePoolCPL);
                        // Get PayrollPeriod
                        PayrollPeriod dbPayrollPeriod = ATAssistant.GetPayrollPeriodObject(obj.OTDate.Value, DDService.GetAllPayrollPeriod());
                        // Update Days in Leave Quota
                        LeaveQuotaYear dbLeaveQuotaYear = new LeaveQuotaYear();
                        Expression<Func<LeaveQuotaYear, bool>> SpecificEntries4 = c => c.FinancialYearID == dbPayrollPeriod.FinancialYearID && c.EmployeeID == obj.EmployeeID && c.LeaveTypeID == 4;
                        if (LeaveQuotaYearService.GetIndexSpecific(SpecificEntries4).Count > 0)
                        {
                            dbLeaveQuotaYear = LeaveQuotaYearService.GetIndexSpecific(SpecificEntries4).First();
                            dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + days;
                            dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + days;
                            dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + days;
                            dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + days;
                            LeaveQuotaYearService.PostEdit(dbLeaveQuotaYear);
                        }
                        else
                        {
                            dbLeaveQuotaYear.EmployeeID = obj.EmployeeID;
                            dbLeaveQuotaYear.FinancialYearID = dbPayrollPeriod.FinancialYearID;
                            dbLeaveQuotaYear.LeaveTypeID = 4;
                            dbLeaveQuotaYear.GrandTotal = 0;
                            dbLeaveQuotaYear.GrandRemaining = 0;
                            dbLeaveQuotaYear.YearlyTotal = 0;
                            dbLeaveQuotaYear.YearlyRemaining = 0;
                            dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + days;
                            dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + days;
                            dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + days;
                            dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + days;
                            LeaveQuotaYearService.PostCreate(dbLeaveQuotaYear);
                        }
                    }
                }
                //DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Crew, AuditTypeCommon.Add, obj.PDailyOTID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(DailyOvertimeService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(DailyOvertime obj)
        {
            Expression<Func<PayrollPeriod, bool>> SpecificEntries = c => obj.OTDate >= c.PRStartDate && obj.OTDate <= c.PREndDate && c.PeriodStageID == "C";
            List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries).ToList();
            if (dbPayrollPeriods.Count() > 0)
                ModelState.AddModelError("OTDate", "Cannot Delete Overtime of Closed Payroll Period");

            if (ModelState.IsValid)
            {
                DailyOvertimeService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return PartialView("Delete", obj);
        }
    }
}