using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Payroll.Controllers
{
    public class FinancialYearController : Controller
    {
        IEntityService<FinancialYear> FinancialYearService;
        IDDService DDService;
        public FinancialYearController(IEntityService<FinancialYear> financialYearService, IDDService dDService)
        {
            FinancialYearService = financialYearService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<FinancialYear> list = FinancialYearService.GetIndex();
            return View(list);
        }
        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FinancialYear obj)
        {
            if (obj.FYName == null || obj.FYName == "")
                ModelState.AddModelError("FYName", "Financial Year name cannot be empty");
            if (FinancialYearService.GetIndex().Where(aa => aa.FYName == obj.FYName).Count() > 0)
                ModelState.AddModelError("FYName", "Financial Year name must be unique");
            if (obj.FYStartDate != null && obj.FYEndDate != null)
            {
                if (obj.FYEndDate == obj.FYStartDate)
                { }
                else
                {
                    if (obj.FYEndDate < obj.FYStartDate)
                    {
                        ModelState.AddModelError("FYStartDate", "Start Date can never be greater than end date.");
                    }
                }
            }
            if (FinancialYearService.GetIndex().Where(aa => aa.FYStartDate == obj.FYStartDate && aa.PFinancialYearID != obj.PFinancialYearID).Count() > 0)
                ModelState.AddModelError("FYStartDate", "Already exists in same date");
            if (ModelState.IsValid)
            {
                FinancialYearService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            return PartialView(FinancialYearService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(FinancialYear obj)
        {
            if (ModelState.IsValid)
            {
                FinancialYearService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(FinancialYearService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(FinancialYear obj)
        {
          
            if (ModelState.IsValid)
            {
                FinancialYearService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }

    }
}