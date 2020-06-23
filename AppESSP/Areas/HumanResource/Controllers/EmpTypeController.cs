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
    public class EmpTypeController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<EmploymentType> EmpTypeService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public EmpTypeController(IEntityService<EmploymentType> emptypeService, IEntityService<Employee> employeeService, IDDService dDService)
        {
            EmpTypeService = emptypeService;
            EmployeeService = employeeService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<EmploymentType> list = EmpTypeService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            EmploymentType obj = new EmploymentType();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(EmploymentType obj)
        {
            if (obj.EmploymentTypeName == null || obj.EmploymentTypeName == "")
                ModelState.AddModelError("EmploymentTypeName", "Employment Type Name cannot be empty");
            if (EmpTypeService.GetIndex().Where(aa => aa.EmploymentTypeName == obj.EmploymentTypeName).Count() > 0)
                ModelState.AddModelError("EmploymentTypeName", "Employment Type Name must be unique");
            if (ModelState.IsValid)
            {
                EmpTypeService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.EmploymentType, AuditTypeCommon.Add, obj.PEmploymentTypeID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            return PartialView("Create", obj);
        }
        public ActionResult Edit(int? id)
        {
            return PartialView(EmpTypeService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(EmploymentType obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.EmploymentTypeID == obj.PEmploymentTypeID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            }
            if (ModelState.IsValid)
            {
                EmpTypeService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        public ActionResult Delete(int? id)
        {
            return PartialView(EmpTypeService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(EmploymentType obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.EmploymentTypeID == obj.PEmploymentTypeID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                EmpTypeService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.EmploymentType, AuditTypeCommon.Delete, obj.PEmploymentTypeID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
    }
}