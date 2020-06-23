using AppESSP.Controllers;
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
    public class OvertimeApprovalController : BaseController
    {
        // GET: Attendance/LeaveCF
        IDDService DDService;
        IOvertimeApprovalService OvertimeAprrovalService;
        public OvertimeApprovalController(IDDService dDService, IOvertimeApprovalService overtimeAprrovalService)
        {
            DDService = dDService;
            OvertimeAprrovalService = overtimeAprrovalService;
        }
        public ActionResult Index(int? payrollPeriodID)
        {

            if (payrollPeriodID == null || payrollPeriodID == 0)
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                List<VMOvertimeApprovalChild> vmList = OvertimeAprrovalService.GetIndex(LoggedInUser, DDService.GetPayrollPeriod().Where(aa => aa.PeriodStageID == "O").First().PPayrollPeriodID);
                ViewBag.payrollPeriodID = new SelectList(DDService.GetAllPayrollPeriod().ToList().OrderByDescending(aa => aa.PRStartDate).ToList(), "PPayrollPeriodID", "PRName", payrollPeriodID);

                return View(vmList);
            }
            else
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                List<VMOvertimeApprovalChild> vmList = OvertimeAprrovalService.GetIndex(LoggedInUser, (int)payrollPeriodID);
                ViewBag.payrollPeriodID = new SelectList(DDService.GetAllPayrollPeriod().ToList().OrderByDescending(aa => aa.PRStartDate).ToList(), "PPayrollPeriodID", "PRName", payrollPeriodID);

                return View(vmList);
            }

        }
        public ActionResult Create1()
        {
            VMOvertimeApprovalSelection vmJobCardCreate = new VMOvertimeApprovalSelection();
            vmJobCardCreate = OvertimeAprrovalService.GetCreate1();
            ViewBag.PayrollPeriodID = new SelectList(DDService.GetPayrollPeriod().ToList().OrderBy(aa => aa.PRName).ToList(), "PPayrollPeriodID", "PRName");
            return View(vmJobCardCreate);
        }
        [HttpPost]
        public ActionResult Create1(VMOvertimeApprovalSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            es = OvertimeAprrovalService.GetCreate2(es, SelectedCompanyIds, SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
            SelectedCrewIds, SelectedShiftIds, LoggedInUser);
            return View("Create2", es);
        }
        [HttpPost]
        public ActionResult Create3(VMOvertimeApprovalSelection es, int?[] SelectedEmpIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            //H HR Admin //U   HR Normal
            VMOvertimeApproval vmOvertimeApproval = new VMOvertimeApproval();
            vmOvertimeApproval = OvertimeAprrovalService.GetCreate3(es, SelectedEmpIds, vmOvertimeApproval);
            ViewBag.SubmittedToUserID = ViewBag.LineManagerID = new SelectList(AppAssistant.GetLineManagers(DDService.GetUser().Where(aa => aa.UserRoleID == "H").ToList()), "PUserID", "UserName", LoggedInUser.LineManagerID);
            ViewBag.OTStatusID = new SelectList(DDService.GetMonthOTStage().Where(aa => aa.PMonthDataOTStageID == "H" || aa.PMonthDataOTStageID == "A"), "PMonthDataOTStageID", "MonthDataOTStageName", "H");
            return View(vmOvertimeApproval);
        }
        [HttpPost]
        public ActionResult Create4(VMOvertimeApproval vmOvertimeApproval, int?[] SelectedEmpIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VMOvertimeApprovalChild> vmOvertimeApprovalChildEntries = new List<VMOvertimeApprovalChild>();
            foreach (var empID in SelectedEmpIds)
            {
                VMOvertimeApprovalChild vmOvertimeApprovalChild = new VMOvertimeApprovalChild();
                string EncashableSingleOT = Request.Form["ENCS-" + empID.Value.ToString()].ToString();
                string EncashableDoubleOT = Request.Form["ENCD-" + empID.Value.ToString()].ToString();
                //string AbsentDays = Request.Form["ABDays-" + empID.Value.ToString()].ToString();
                //string CPLHours = Request.Form["CPLH-" + empID.Value.ToString()].ToString();
                string CPLHours = "0";
                vmOvertimeApprovalChild.EmpID = (int)empID;
                vmOvertimeApprovalChild.EncashableSingleOT = Convert.ToInt32(EncashableSingleOT);
                vmOvertimeApprovalChild.EncashableDoubleOT = Convert.ToInt32(EncashableDoubleOT);
                vmOvertimeApprovalChild.CPLConvertedOT = Convert.ToInt32(CPLHours);
                //vmOvertimeApprovalChild.Absents = Convert.ToInt32(AbsentDays);
                vmOvertimeApprovalChildEntries.Add(vmOvertimeApprovalChild);
            }
            vmOvertimeApproval.OvertimeApprovalChild = vmOvertimeApprovalChildEntries.ToList();
            VMOvertimeApproval vm = OvertimeAprrovalService.GetCreate4(vmOvertimeApproval, LoggedInUser);
            //if (vm.ErrorMessages.Count == 0)
            return RedirectToAction("Index");
            //else
            //    return View("Create3", vm);
        }
    }
}