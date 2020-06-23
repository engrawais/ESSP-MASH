using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class LeaveCFController : Controller
    {
        // GET: Attendance/LeaveCF
        IDDService DDService;
        ILeaveCFService LeaveCFService;
        public LeaveCFController(IDDService dDService, ILeaveCFService leaveCFService)
        {
            DDService = dDService;
            LeaveCFService = leaveCFService;
        }
        public ActionResult Index()
        {
            List<VMLeaveCFChild> vmList = new List<VMLeaveCFChild>();
            return View(vmList);
        }
        public ActionResult Create1()
        {
            VMLeaveCFSelection vmJobCardCreate = new VMLeaveCFSelection();
            vmJobCardCreate = LeaveCFService.GetCreate1();
            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().ToList().OrderBy(aa => aa.FYName).ToList(), "PFinancialYearID", "FYName");
            return View(vmJobCardCreate);
        }
        [HttpPost]
        public ActionResult Create1(VMLeaveCFSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            es = LeaveCFService.GetCreate2(es, SelectedCompanyIds, SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
            SelectedCrewIds, SelectedShiftIds, LoggedInUser);
            return View("Create2", es);
        }
        [HttpPost]
        public ActionResult Create3(VMLeaveCFSelection es, int?[] SelectedEmpIds)
        {
            VMLeaveCF vmLeaveCF = new VMLeaveCF();
            vmLeaveCF = LeaveCFService.GetCreate3(es, SelectedEmpIds, vmLeaveCF);
            return View(vmLeaveCF);
        }
        [HttpPost]
        public ActionResult Create4(VMLeaveCF vmLeaveCF, int?[] SelectedEmpIds)
        {
            List<VMLeaveCFChild> vmLeaveCFChildEntries = new List<VMLeaveCFChild>();
            foreach (var empID in SelectedEmpIds)
            {
                VMLeaveCFChild vmLeaveCFChild = new VMLeaveCFChild();
                string CollapseLeave = Request.Form["CollapseLeave-" + empID.Value.ToString()].ToString();
                string CarryForward = Request.Form["CarryForward-" + empID.Value.ToString()].ToString();
                vmLeaveCFChild.EmpID = (int)empID;
                vmLeaveCFChild.CollapseLeave = (float)Convert.ToDouble(CollapseLeave);
                vmLeaveCFChild.CarryForward = (float)Convert.ToDouble(CarryForward);
                vmLeaveCFChildEntries.Add(vmLeaveCFChild);
            }
            vmLeaveCF.LeaveCFChild = vmLeaveCFChildEntries.ToList();
            VMLeaveCF vm = LeaveCFService.GetCreate4(vmLeaveCF);
            if (vm.ErrorMessages.Count == 0)
                return View("Index");
            else
                return View("Create3", vm);
        }
    }
}