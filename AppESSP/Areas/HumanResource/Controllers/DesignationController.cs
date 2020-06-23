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
    public class DesignationController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<Designation> DesignationService;
        IEntityService<VHR_Designation> VHRDesignationService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public DesignationController(IEntityService<Designation> designationService, IEntityService<Employee> employeeService, IDDService dDService, IEntityService<VHR_Designation> vHRDesignationService)
        {
            DesignationService = designationService;
            EmployeeService = employeeService;
            VHRDesignationService = vHRDesignationService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<VHR_Designation> list = VHRDesignationService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            Designation obj = new Designation();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(Designation obj)
        {
            if (obj.DesignationName == null || obj.DesignationName == "")
                ModelState.AddModelError("DesignationName", "Crew name cannot be empty");
            if (DesignationService.GetIndex().Where(aa => aa.DesignationName == obj.DesignationName).Count() > 0)
                ModelState.AddModelError("DesignationName", "Crew name must be unique");
            if (ModelState.IsValid)
            {
                DesignationService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Positions, AuditTypeCommon.Add, obj.PDesignationID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Designation obj = DesignationService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Designation obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.CrewID == obj.PDesignationID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot Inactive due to attachment of active employees");
            }
            if (obj.DesignationName == null || obj.DesignationName == "")
                ModelState.AddModelError("DesignationName", "Crew name cannot be empty");
            if (DesignationService.GetIndex().Where(aa => aa.DesignationName == obj.DesignationName && aa.PDesignationID != obj.PDesignationID).Count() > 0)
                ModelState.AddModelError("DesignationName", "Crew name must be unique");
            if (ModelState.IsValid)
            {
                DesignationService.PostEdit(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Positions, AuditTypeCommon.Edit, obj.PDesignationID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(DesignationService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Designation obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.DesigationID == obj.PDesignationID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                DesignationService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Positions, AuditTypeCommon.Delete, obj.PDesignationID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        private void HelperMethod(Designation obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.JobTitleID = new SelectList(DDService.GetJobTitle(LoggedInUser).ToList().OrderBy(aa => aa.JobTitleName).ToList(), "PJobTitleID", "JobTitleName", obj.JobTitleID);
        }
    }
}