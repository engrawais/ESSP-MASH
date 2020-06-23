using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class ERPMonthlyController : Controller
    {
        // GET: Attendance/ERPMonthly
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        IEntityService<VHR_EmployeeProfile> VHREmployeeProfileService;
        IEntityService<MonthlyERPLWOP> MonthlyERPLWOPService;
        IEntityService<VAT_MonthlySummary> VAT_MonthlySummaryService;
        IEntityService<MonthlyERP> MonthlyERPService;
        IDDService DDService;
        public ERPMonthlyController(IGetSpecificEmployeeService getSpecificEmployeeService, IEntityService<VHR_EmployeeProfile> vHREmployeeProfileService
            , IEntityService<MonthlyERPLWOP> monthlyERPLWOPService, IEntityService<MonthlyERP> monthlyERPService, IDDService dDService, IEntityService<VAT_MonthlySummary> vAT_MonthlySummaryService)
        {
            GetSpecificEmployeeService = getSpecificEmployeeService;
            VHREmployeeProfileService = vHREmployeeProfileService;
            MonthlyERPLWOPService = monthlyERPLWOPService;
            MonthlyERPService = monthlyERPService;
            DDService = dDService;
            VAT_MonthlySummaryService = vAT_MonthlySummaryService;
        }
        public ActionResult Index(int? PayrollPeriodID)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (PayrollPeriodID == null)
                PayrollPeriodID = DDService.GetAllPayrollPeriod().OrderByDescending(aa=>aa.PPayrollPeriodID).First().PPayrollPeriodID;
            List<VMMonthlyERP> vmList = new List<VMMonthlyERP>();
            ViewBag.PayrollPeriodID = new SelectList(DDService.GetAllPayrollPeriod().ToList().OrderByDescending(aa => aa.PPayrollPeriodID).ToList(), "PPayrollPeriodID", "PRName", PayrollPeriodID);
            Expression<Func<MonthlyERP, bool>> SpecificEntries2 = c => c.PayrollPeriodID == PayrollPeriodID;
            List<MonthlyERP> monthlyerplist = MonthlyERPService.GetIndexSpecific(SpecificEntries2);
            Expression<Func<MonthlyERPLWOP, bool>> SpecificEntries3 = c => c.PayrollPeriodID == PayrollPeriodID;
            List<MonthlyERPLWOP> dbMonthlyERPLWOPs = MonthlyERPLWOPService.GetIndexSpecific(SpecificEntries3);
            List<VHR_EmployeeProfile> employees = GetSpecificEmployeeService.GetSpecificEmployees(LoggedInUser);
            foreach (var emp in employees)
            {
                if (monthlyerplist.Where(aa => aa.EmployeeID == emp.PEmployeeID).Count() > 0)
                {
                    MonthlyERP tmonthlyerp = monthlyerplist.First(aa => aa.EmployeeID == emp.PEmployeeID);
                    if (tmonthlyerp.SingleOTMins > 0 || tmonthlyerp.DoubleOTMins > 0 || tmonthlyerp.LWOPDays > 0)
                    {
                        VMMonthlyERP vm = new VMMonthlyERP();
                        vm.OEmpID = emp.OEmpID;
                        vm.EmpID = emp.PEmployeeID;
                        vm.EmpName = emp.EmployeeName;
                        vm.DesignationName = emp.DesignationName;
                        vm.LocationName = emp.LocationName;
                        //vm.OUName = emp.OUName;
                        vm.Status = tmonthlyerp.Remarks;
                        if (dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID).Count() > 0)
                            vm.LWOPDays = dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID && aa.IsHalf != true).Count() + (dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID && aa.IsHalf == true).Count() / 2);
                        else
                            vm.LWOPDays = 0;
                        if(tmonthlyerp.SingleOTMins>0)
                            vm.SingleOT = (int)tmonthlyerp.SingleOTMins / 60;
                        if (tmonthlyerp.DoubleOTMins > 0)
                            vm.DoubleOT = (int)tmonthlyerp.DoubleOTMins / 60;
                        vmList.Add(vm);
                    }
                }
                else if (dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID).Count() > 0)
                {
                    VMMonthlyERP vm = new VMMonthlyERP();
                    vm.OEmpID = emp.OEmpID;
                    vm.EmpID = emp.PEmployeeID;
                    vm.EmpName = emp.EmployeeName;
                    vm.DesignationName = emp.DesignationName;
                    vm.LocationName = emp.LocationName;
                    //vm.OUName = emp.OUName;
                    vm.Status = "Absent,"+dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID).First().Remarks;
                    if (dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID).Count() > 0)
                        vm.LWOPDays = dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID && aa.IsHalf != true).Count() + (dbMonthlyERPLWOPs.Where(aa => aa.EmployeeID == emp.PEmployeeID && aa.IsHalf == true).Count() / 2);
                    else
                        vm.LWOPDays = 0;
                    vmList.Add(vm);
                }
            }
            ViewBag.PRPeriodID = PayrollPeriodID;
            return View(vmList);


        }
        public ActionResult IndexSubmit(int?[] SelectedEmpIds)
        {
            int PRID = Convert.ToInt32(Request.Form["PRPeriodID"]);
            Expression<Func<MonthlyERPLWOP, bool>> SpecificEntries = c => c.PayrollPeriodID == PRID;
            List<MonthlyERPLWOP> monthlyerplwoplist = MonthlyERPLWOPService.GetIndexSpecific(SpecificEntries);
            Expression<Func<MonthlyERP, bool>> SpecificEntries2 = c => c.PayrollPeriodID == PRID;
            List<MonthlyERP> monthlyerplist = MonthlyERPService.GetIndexSpecific(SpecificEntries2);
            foreach (var emp in SelectedEmpIds)
            {
                if (monthlyerplist.Where(aa => aa.EmployeeID == emp).Count() > 0)
                {
                    MonthlyERP tmonthlyerp = monthlyerplist.First(aa => aa.EmployeeID == emp);
                    tmonthlyerp.Remarks = "Close";
                    MonthlyERPService.PostEdit(tmonthlyerp);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult OTManagerStatus(int? PayrollPeriodID)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            if (PayrollPeriodID == null)
                PayrollPeriodID = DDService.GetPayrollPeriod().First().PPayrollPeriodID;
            ViewBag.PayrollPeriodID = new SelectList(DDService.GetAllPayrollPeriod().ToList().OrderByDescending(aa => aa.PRStartDate).ToList(), "PPayrollPeriodID", "PRName", PayrollPeriodID);
            Expression<Func<VAT_MonthlySummary, bool>> SpecificEntries = c => c.PayrollPeriodID == PayrollPeriodID && c.Status=="Active";
            List<VAT_MonthlySummary> dbVAT_MonthlySummary = VAT_MonthlySummaryService.GetIndexSpecific(SpecificEntries);
            List<VMOTStatusManager> vmOTStatusManagers = new List<VMOTStatusManager>();
            foreach (var locid in dbVAT_MonthlySummary.Where(aa=>aa.EncashbaleSingleOT>0 || aa.EncashbaleDoubleOT>0|| aa.CPLConversionOT>0).Select(aa=>aa.LocationID).Distinct().ToList())
            {
                VMOTStatusManager vmOTStatusManager = new VMOTStatusManager();
                List<VAT_MonthlySummary> TempDBVAT_MonthlySummary = dbVAT_MonthlySummary.Where(aa => aa.LocationID == locid && (aa.EncashbaleSingleOT > 0 || aa.EncashbaleDoubleOT > 0 || aa.CPLConversionOT > 0)).ToList();
                vmOTStatusManager.LocationID = TempDBVAT_MonthlySummary.First().LocationID;
                vmOTStatusManager.LocationName = TempDBVAT_MonthlySummary.First().LocationName;
                vmOTStatusManager.PayrollPeriodID = PayrollPeriodID;
                vmOTStatusManager.TotalEmps = TempDBVAT_MonthlySummary.Count;
                vmOTStatusManager.PendingAtTM = TempDBVAT_MonthlySummary.Where(aa => aa.MonthDataStageID == "P" || aa.MonthDataStageID == null).Count();
                vmOTStatusManager.PendingAtHO = TempDBVAT_MonthlySummary.Where(aa => aa.MonthDataStageID == "H").Count();
                vmOTStatusManager.Approved = TempDBVAT_MonthlySummary.Where(aa => aa.MonthDataStageID == "A").Count();
                vmOTStatusManager.Reject = TempDBVAT_MonthlySummary.Where(aa => aa.MonthDataStageID == "R").Count();
                vmOTStatusManagers.Add(vmOTStatusManager);
            }
            ViewBag.PRPeriodID = PayrollPeriodID;
            return View(vmOTStatusManagers);
        }
        public ActionResult OTStatusManagerDetail(int? LocID, int? PRID, string MonthDataStage)
        {
            Expression<Func<VAT_MonthlySummary, bool>> SpecificEntries = c => c.PayrollPeriodID == PRID && c.LocationID==LocID && c.Status=="Active" && c.MonthDataStageID==MonthDataStage && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
            List<VAT_MonthlySummary> dbVAT_MonthlySummary = VAT_MonthlySummaryService.GetIndexSpecific(SpecificEntries);
            if (PRID > 0)
            {
                string PRName = DDService.GetPayrollPeriod().Where(aa => aa.PPayrollPeriodID == PRID).First().PRName;
                switch (MonthDataStage)
                {
                    case "P":
                        ViewBag.SubTitle = "OT Status Detailed for Period: " + PRName + " and Monthly Status: Pending at Time Office";
                        break;
                    case "H":
                        ViewBag.SubTitle = "OT Status Detailed for Period: " + PRName + " and Monthly Status: Pending at Head Office";
                        break;
                    case "A":
                        ViewBag.SubTitle = "OT Status Detailed for Period: " + PRName + " and Monthly Status: Approved";
                        break;
                    case "R":
                        ViewBag.SubTitle = "OT Status Detailed for Period: " + PRName + " and Monthly Status: Reject";
                        break;
                }
            }
            return View(dbVAT_MonthlySummary);
        }
    }
}