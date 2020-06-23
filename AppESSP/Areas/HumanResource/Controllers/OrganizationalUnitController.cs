using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class OrganizationalUnitController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<OrganizationalUnit> OrganizationalUnitService;
        IEntityService<Employee> EmployeeService;
        IEntityService<VHR_OrganizationalUnit> VHROrganizationalUnit;
        IDDService DDService;
        public OrganizationalUnitController(IEntityService<OrganizationalUnit> organizationalunitService, IEntityService<Employee> employeeService, IDDService dDService, IEntityService<VHR_OrganizationalUnit> vHROrganizationalUnit)
        {
            OrganizationalUnitService = organizationalunitService;
            EmployeeService = employeeService;
            DDService = dDService;
            VHROrganizationalUnit = vHROrganizationalUnit;
        }
        public ActionResult Index()
        {
            List<VHR_OrganizationalUnit> list = VHROrganizationalUnit.GetIndex();
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            OrganizationalUnit obj = OrganizationalUnitService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(OrganizationalUnit obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.OUID == obj.POUID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            }
            if (ModelState.IsValid)
            {
                OrganizationalUnitService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        public ActionResult Create()
        {
            OrganizationalUnit obj = new OrganizationalUnit();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(OrganizationalUnit obj)
        {
            if (obj.OUName == null || obj.OUName == "")
                ModelState.AddModelError("OUName", "Organizational Unit cannot be empty");
            if (OrganizationalUnitService.GetIndex().Where(aa => aa.OUName == obj.OUName).Count() > 0)
                ModelState.AddModelError("OUName", "Organizational Unit must be unique");
            if (ModelState.IsValid)
            {
                OrganizationalUnitService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.OrganizationalUnits, AuditTypeCommon.Add, obj.POUID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        public ActionResult Delete(int? id)
        {
            return PartialView(OrganizationalUnitService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(OrganizationalUnit obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.OUID == obj.POUID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                OrganizationalUnitService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.OrganizationalUnits, AuditTypeCommon.Delete, obj.POUID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        public ActionResult LoadNotificationEmployee(string Criteria)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            List<OrganizationalUnit> vmList = new List<OrganizationalUnit>();
            vmList = OrganizationalUnitService.GetIndex().Where(aa => aa.OUCommonID == null).ToList();

            return View("Index", vmList);
        }
        private void HelperMethod(OrganizationalUnit obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.OUCommonID = new SelectList(DDService.GetOUCommon(LoggedInUser).ToList().OrderBy(aa => aa.OUCommonName).ToList(), "POUCommonID", "OUCommonName", obj.OUCommonID);
        }
    }
}