using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace AppESSP.Areas.Attendance.Controllers
{
    public class LeaveQuotaController : Controller
    {
        IDDService DDService;
        ILeaveQuotaService LeaveQuotaService;
        IEntityService<LeaveCPLEmpBalance> LeaveCPLBalanceService;
        IEntityService<LeaveQuotaYear> LeaveQuotaYearService;
        public LeaveQuotaController(IDDService dDService, ILeaveQuotaService leaveQuotaService,
            IEntityService<LeaveCPLEmpBalance> leaveCPLBalanceService, IEntityService<LeaveQuotaYear> leaveQuotaYearService)
        {
            DDService = dDService;
            LeaveQuotaService = leaveQuotaService;
            LeaveCPLBalanceService = leaveCPLBalanceService;
            LeaveQuotaYearService = leaveQuotaYearService;
            // GET: Attendance/JobCard
        }
        public ActionResult Index(int? FinancialYearID)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (FinancialYearID == null)
                FinancialYearID = DDService.GetFinancialYear().OrderByDescending(aa => aa.PFinancialYearID).First().PFinancialYearID;
            List<VMLeaveQuotaChild> vmList = new List<VMLeaveQuotaChild>();
            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().ToList().OrderBy(aa => aa.FYName).ToList(), "PFinancialYearID", "FYName", FinancialYearID);
            vmList = LeaveQuotaService.GetIndex((int)FinancialYearID, LoggedInUser);
            return View(vmList);
        }
        public ActionResult Create1()
        {
            VMLeaveQuotaSelection vmJobCardCreate = new VMLeaveQuotaSelection();
            vmJobCardCreate = LeaveQuotaService.GetCreate1();
            Helper();
            return View(vmJobCardCreate);
        }
        [HttpPost]
        public ActionResult Create2(VMLeaveQuotaSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            es = LeaveQuotaService.GetCreate2(es, SelectedCompanyIds, SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
            SelectedCrewIds, SelectedShiftIds, LoggedInUser);
            return View(es);
        }
        [HttpPost]
        public ActionResult Create3(VMLeaveQuotaSelection es, int?[] SelectedEmpIds)
        {
            VMLeaveQuota vmLeaveQuota = new VMLeaveQuota();
            vmLeaveQuota = LeaveQuotaService.GetCreate3(es, SelectedEmpIds, vmLeaveQuota);
            return View(vmLeaveQuota);
        }
        [HttpPost]
        public ActionResult Create4(VMLeaveQuota vmLeaveQuota, int?[] SelectedEmpIds)
        {
            List<VMLeaveQuotaChild> vmLeaveQuotaChildEntries = new List<VMLeaveQuotaChild>();
            foreach (var empID in SelectedEmpIds)
            {
                VMLeaveQuotaChild vmLeaveQuotaChild = new VMLeaveQuotaChild();
                string AL = Request.Form["AL-" + empID.Value.ToString()].ToString();
                string CL = Request.Form["CL-" + empID.Value.ToString()].ToString();
                string SL = Request.Form["SL-" + empID.Value.ToString()].ToString();
                string EAL = Request.Form["EAL-" + empID.Value.ToString()].ToString();
                string CME = Request.Form["CME-" + empID.Value.ToString()].ToString();
                string ALCop = Request.Form["ALCop-" + empID.Value.ToString()].ToString();
                string ALCF = Request.Form["ALCF-" + empID.Value.ToString()].ToString();
                vmLeaveQuotaChild.EmpID = (int)empID;
                vmLeaveQuotaChild.AL = (float)Convert.ToDouble(AL);
                vmLeaveQuotaChild.CL = (float)Convert.ToDouble(CL);
                vmLeaveQuotaChild.SL = (float)Convert.ToDouble(SL);
                vmLeaveQuotaChild.EAL = (float)Convert.ToDouble(EAL);
                vmLeaveQuotaChild.CME = (float)Convert.ToDouble(CME);
                vmLeaveQuotaChild.CollapseLeave = (float)Convert.ToDouble(ALCop);
                vmLeaveQuotaChild.CarryForward = (float)Convert.ToDouble(ALCF);

                vmLeaveQuotaChildEntries.Add(vmLeaveQuotaChild);
            }
            vmLeaveQuota.LeaveQuotaChild = vmLeaveQuotaChildEntries.ToList();
            VMLeaveQuota vm = LeaveQuotaService.GetCreate4(vmLeaveQuota);
            if (vm.ErrorMessages.Count == 0)
                return RedirectToAction("Index");
            else
                return View("Create3", vm);
        }

        public ActionResult CPLCreate(int? id)
        {
            LeaveCPLEmpBalance obj = new LeaveCPLEmpBalance();
            obj.EmployeeID = id;
            CPLHelper(obj);
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult CPLCreate(LeaveCPLEmpBalance dbLeaveCPLBalance)
        {
            //if (dbLeaveCPLBalance.RemainingDays==null || dbLeaveCPLBalance.RemainingDays == 0)
            //    ModelState.AddModelError("CPLBalance", "CPL Balance must be greated than 0");
            //if (dbLeaveCPLBalance.ExpireDate ==null)
            //    ModelState.AddModelError("ExpireDate", "Expire Date cannot be null");
            //if (ModelState.IsValid)
            //{
            //    // Get PayrollPeriod
            //    PayrollPeriod dbPayrollPeriod = ATAssistant.GetPayrollPeriodObject(dbLeaveCPLBalance.EntryDateTime.Value, DDService.GetAllPayrollPeriod());
            //    //dbLeaveCPLBalance.Used = 0;
            //    dbLeaveCPLBalance.RemainingDays = dbLeaveCPLBalance.RemainingDays;
            //    dbLeaveCPLBalance.EntryDateTime = dbPayrollPeriod.PREndDate.Value.AddDays(1);
            //    LeaveCPLBalanceService.PostCreate(dbLeaveCPLBalance);
            //    // Update Leave Quota
            //    Expression<Func<LeaveQuotaYear, bool>> SpecificEntries3 = c => c.FinancialYearID == dbPayrollPeriod.FinancialYearID && c.EmployeeID == dbLeaveCPLBalance.EmployeeID && c.LeaveTypeID == 4;
            //    if (LeaveQuotaYearService.GetIndexSpecific(SpecificEntries3).Count > 0)
            //    {
            //        LeaveQuotaYear dbLeaveQuotaYear = LeaveQuotaYearService.GetIndexSpecific(SpecificEntries3).First();
            //        dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + dbLeaveCPLBalance.CPLBalance;
            //        LeaveQuotaYearService.PostEdit(dbLeaveQuotaYear);
            //    }
            //    else
            //    {
            //        LeaveQuotaYear dbLeaveQuotaYear = new LeaveQuotaYear();
            //        dbLeaveQuotaYear.EmployeeID = dbLeaveCPLBalance.EmployeeID;
            //        dbLeaveQuotaYear.FinancialYearID = dbPayrollPeriod.FinancialYearID;
            //        dbLeaveQuotaYear.LeaveTypeID = 4;
            //        dbLeaveQuotaYear.GrandTotal = 0;
            //        dbLeaveQuotaYear.GrandRemaining = 0;
            //        dbLeaveQuotaYear.YearlyTotal = 0;
            //        dbLeaveQuotaYear.YearlyRemaining = 0;
            //        dbLeaveQuotaYear.GrandTotal = dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.GrandRemaining = dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.YearlyTotal = dbLeaveCPLBalance.CPLBalance;
            //        dbLeaveQuotaYear.YearlyRemaining = dbLeaveCPLBalance.CPLBalance;
            //        LeaveQuotaYearService.PostCreate(dbLeaveQuotaYear);
            //    }
            //    return Json("OK", JsonRequestBehavior.AllowGet);

            //}
            CPLHelper(dbLeaveCPLBalance);
            return PartialView("Create", dbLeaveCPLBalance);
        }
        public ActionResult LeaveQuotaDetail(int? id)
        {
            Expression<Func<LeaveCPLEmpBalance, bool>> SpecificEntries2 = aa => aa.EmployeeID == id;
            List<LeaveCPLEmpBalance> leaveCPLBalances = LeaveCPLBalanceService.GetIndexSpecific(SpecificEntries2);
            ViewBag.EmpID = id;
            return View(leaveCPLBalances);
        }
        public ActionResult CPLDetailEdit(int? id)
        {
            return PartialView(LeaveCPLBalanceService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult CPLDetailEdit(LeaveCPLEmpBalance obj)
        {
            LeaveCPLEmpBalance dbLeaveCPLBalance = LeaveCPLBalanceService.GetEdit(obj.PLeaveCPLEmpBalanceID);
            //if (obj.RemainingDays < dbLeaveCPLBalance.RemainingDays)
            //    ModelState.AddModelError("RemainingDays", "You can only increase balance");
            //if (ModelState.IsValid)
            //{
            //    if (obj.RemainingDays != dbLeaveCPLBalance.RemainingDays)
            //    {
            //        double? diff = obj.RemainingDays - dbLeaveCPLBalance.RemainingDays;
            //        dbLeaveCPLBalance.RemainingDays = dbLeaveCPLBalance.RemainingDays + diff;
            //        dbLeaveCPLBalance.RemainingDays = obj.RemainingDays;
            //        LeaveCPLBalanceService.PostEdit(dbLeaveCPLBalance);
            //        // Update LeaveQuota

            //        PayrollPeriod dbPayrollPeriod = DDService.GetAllPayrollPeriod().Where(aa => aa.PPayrollPeriodID == dbLeaveCPLBalance.PayrollPeriodID).First();
            //        Expression<Func<LeaveQuotaYear, bool>> SpecificEntries3 = c => c.FinancialYearID == dbPayrollPeriod.FinancialYearID && c.EmployeeID == dbLeaveCPLBalance.EmployeeID && c.LeaveTypeID == 4;
            //        if (LeaveQuotaYearService.GetIndexSpecific(SpecificEntries3).Count > 0)
            //        {
            //            LeaveQuotaYear dbLeaveQuotaYear = LeaveQuotaYearService.GetIndexSpecific(SpecificEntries3).First();
            //            dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + diff;
            //            dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + diff;
            //            dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + diff;
            //            dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + diff;
            //            LeaveQuotaYearService.PostEdit(dbLeaveQuotaYear);
            //        }
            //    }
            //    return Json("OK", JsonRequestBehavior.AllowGet);
            //}
            return PartialView("CPLDetailEdit", obj);
        }
        private void Helper()
        {

            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().ToList().OrderBy(aa => aa.FYName).ToList(), "PFinancialYearID", "FYName");
        }
        private void CPLHelper(LeaveCPLEmpBalance leaveCPLBalance)
        {
            //ViewBag.PayrollPeriodID = new SelectList(DDService.GetPayrollPeriod().ToList().OrderBy(aa => aa.PPayrollPeriodID).ToList(), "PPayrollPeriodID", "PRName", leaveCPLBalance.PayrollPeriodID);
        }

    }
}