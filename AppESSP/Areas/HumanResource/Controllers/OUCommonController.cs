using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class OUCommonController : Controller
    {
        // GET: HumanResource/OUCommon
        IEntityService<OUCommon> OUCommonService;
        IDDService DDService;
        public OUCommonController(IEntityService<OUCommon> oUCommonService, IDDService dDService)
        {
            OUCommonService = oUCommonService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<OUCommon> list = OUCommonService.GetIndex();
            return View(list);
        }


        [HttpGet]

        public ActionResult Create()
        {
            OUCommon obj = new OUCommon();
            DDHelper(obj);
            return View();
        }
        [HttpPost]
        public ActionResult Create(OUCommon obj)
        {
            if (obj.OUCommonName == null || obj.OUCommonName == "")
                ModelState.AddModelError("OUCommonName", "Ou Common name cannot be empty");
            if (OUCommonService.GetIndex().Where(aa => aa.OUCommonName == obj.OUCommonName).Count() > 0)
                ModelState.AddModelError("OUCommon", "OU Common name must be unique");
            if (ModelState.IsValid)
            {
                OUCommonService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            DDHelper(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            OUCommon obj = OUCommonService.GetEdit((int)id);
            DDHelper(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(OUCommon obj)
        {
            if (obj.OUCommonName == null || obj.OUCommonName == "")
                ModelState.AddModelError("OUCommonName", "Ou Common name cannot be empty");
            if (OUCommonService.GetIndex().Where(aa => aa.OUCommonName == obj.OUCommonName && aa.POUCommonID != obj.POUCommonID).Count() > 0)
                ModelState.AddModelError("OUCommonName", "OU Common name must be unique");
            if (ModelState.IsValid)
            {
                OUCommonService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            DDHelper(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(OUCommonService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(OUCommon obj)
        {
            if (ModelState.IsValid)
            {
                OUCommonService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return PartialView("Delete", obj);
        }
        #region
        private void DDHelper(OUCommon obj)
        {
            //ViewBag.OTPolicyID = new SelectList(DDService.GetOTPolicy().ToList().OrderBy(aa => aa.OTPolicyName).ToList(), "POTPolicyID", "OTPolicyName", obj.OTPolicyID);
            //ViewBag.ALPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 1).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.ALPolicyID);
            //ViewBag.SLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 3).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.SLPolicyID);
            //ViewBag.CLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 2).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CLPolicyID);
            //ViewBag.CPLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 4).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CPLPolicyID);

        }
        #endregion
    }
}