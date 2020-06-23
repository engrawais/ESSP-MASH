using AppESSP.App_Start;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Payroll.Controllers
{
    [CustomActionFilter]
    // GET: Payroll/PayrollPeriod
    public class PayrollPeriodController : Controller
    {
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IDDService DDService;
        public PayrollPeriodController(IEntityService<PayrollPeriod> payrollPeriodService, IDDService dDService)
        {
            PayrollPeriodService = payrollPeriodService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<PayrollPeriod> list = PayrollPeriodService.GetIndex();
            return View(list.OrderByDescending(aa=>aa.PPayrollPeriodID).ToList());
        }
        [HttpGet]

        public ActionResult Create()

        {
            PayrollPeriod obj = new PayrollPeriod();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(PayrollPeriod obj)
        {
            if (obj.PRName == null || obj.PRName == "")
                ModelState.AddModelError("PRName", "Payroll Period name cannot be empty");
            if (PayrollPeriodService.GetIndex().Where(aa => aa.PRName == obj.PRName).Count() > 0)
                ModelState.AddModelError("PRName", "Payroll Period name must be unique");
            if (obj.PRStartDate != null && obj.PREndDate != null)
            {
                if (obj.PREndDate == obj.PRStartDate)
                { }
                else
                {
                    if (obj.PREndDate < obj.PRStartDate)
                    {
                        ModelState.AddModelError("PRStartDate", "Start Date can never be greater than end date.");
                    }
                }
            }
            if (PayrollPeriodService.GetIndex().Where(aa => aa.PRStartDate == obj.PRStartDate && aa.PPayrollPeriodID != obj.PPayrollPeriodID).Count() > 0)
                ModelState.AddModelError("FYStartDate", "Already exists in same date");
            if (ModelState.IsValid)
            {
                obj.PeriodStageID = "O";
                PayrollPeriodService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            PayrollPeriod obj = PayrollPeriodService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(PayrollPeriod obj)
        {
            if (obj.PeriodStageID=="C" || obj.PeriodStageID=="O")
            {

            }
            else
            {
                ModelState.AddModelError("PeriodStageID", "Stage should be O or C.");
            }
            if (ModelState.IsValid)
            {
                if(obj.PeriodStageID=="C")
                {
                    VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                    AuditLogging.SaveAuditLog(obj, LoggedInUser.PUserID, EnumControllerNames.Payroll, EnumActionNames.Edit);
                    DDService.ProcessMonthlyERPAttendance(obj.PRStartDate.Value, LoggedInUser.UserEmpID.Value);
                }
                PayrollPeriodService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(PayrollPeriodService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(PayrollPeriod obj)
        {

            if (ModelState.IsValid)
            {
                PayrollPeriodService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Delete", obj);
        }
        #region -- Private Method--
        private void HelperMethod(PayrollPeriod obj)
        {
            ViewBag.FinancialYearID = new SelectList(DDService.GetFinancialYear().ToList().OrderByDescending(aa => aa.PFinancialYearID).ToList(), "PFinancialYearID", "FYName", obj.FinancialYearID);
        }
        #endregion

    }
}