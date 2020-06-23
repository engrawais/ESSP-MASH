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
    public class JTCommonController : Controller
    {
        // GET: HumanResource/JTCommon
        IEntityService<JTCommon> JTCommonService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public JTCommonController(IEntityService<Crew> crewService, IEntityService<Employee> employeeService, IDDService dDService
            , IEntityService<JTCommon> jTCommonService)
        {
            JTCommonService = jTCommonService;
            EmployeeService = employeeService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<JTCommon> list = JTCommonService.GetIndex();
            return View(list);
        }
        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(JTCommon obj)
        {
            if (obj.JTCommonName == null || obj.JTCommonName == "")
                ModelState.AddModelError("JTCommonName", "Common JobTitle name cannot be empty");
            if (JTCommonService.GetIndex().Where(aa => aa.JTCommonName == obj.JTCommonName).Count() > 0)
                ModelState.AddModelError("JTCommonName", "Common JobTitle must be unique");
            if (ModelState.IsValid)
            {
                JTCommonService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)

        {

            return PartialView(JTCommonService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(JTCommon obj)
        {

            if (obj.JTCommonName == null || obj.JTCommonName  == "")
                ModelState.AddModelError("JTCommonName", "Common JobTitle name cannot be empty");
            if (JTCommonService.GetIndex().Where(aa => aa.JTCommonName == obj.JTCommonName && aa.PJTCommonID!=obj.PJTCommonID).Count() > 0)
                ModelState.AddModelError("JTCommonName", "Common JobTitle name must be unique");
            if (ModelState.IsValid)
            {
                JTCommonService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(JTCommonService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(JTCommon obj)
        {
            //if (obj.Status == false)
            //{
            //    Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.DesigationID == obj.PJ;
            //    if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
            //        ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            //}
            if (ModelState.IsValid)
            {
                JTCommonService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
    }
}