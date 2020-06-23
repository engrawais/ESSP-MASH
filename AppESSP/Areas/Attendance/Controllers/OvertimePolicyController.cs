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
    public class OvertimePolicyController : Controller
    {
        // GET: Attendance/OvertimePolicy
        IEntityService<OTPolicy> OTPolicyService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public OvertimePolicyController(IEntityService<OTPolicy> oTPolicyService, IDDService dDService, IEntityService<Employee> employeeService)
         {
            OTPolicyService = oTPolicyService;
            DDService = dDService;
            EmployeeService = employeeService;

        }
        public ActionResult Index()
        {
            List<OTPolicy> list = OTPolicyService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(OTPolicy obj)
        {
            if (obj.OTPolicyName == null || obj.OTPolicyName == "")
                ModelState.AddModelError("OTPolicyName", "Overtime Policy Name cannot be empty");
            if (OTPolicyService.GetIndex().Where(aa => aa.OTPolicyName == obj.OTPolicyName).Count() > 0)
                ModelState.AddModelError("OTPolicyName", "Overtime Policy Name must be unique");
            if (ModelState.IsValid)
            {

                OTPolicyService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            OTPolicy obj = OTPolicyService.GetEdit((int)id);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(OTPolicy obj)
        {
            if (obj.OTPolicyName == null || obj.OTPolicyName == "")
                ModelState.AddModelError("OTPolicyName", "Overtime Policy Name cannot be empty");
            if (OTPolicyService.GetIndex().Where(aa => aa.OTPolicyName == obj.OTPolicyName &&aa.POTPolicyID != obj.POTPolicyID).Count() > 0)
                ModelState.AddModelError("OTPolicyName", "Overtime Policy Name must be unique");
            if (ModelState.IsValid)
            {
                OTPolicyService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(OTPolicyService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(OTPolicy obj)
        {
            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.OTPolicyID == obj.POTPolicyID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
            { ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees"); }
            if (ModelState.IsValid)
            {
                OTPolicyService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
    }
}