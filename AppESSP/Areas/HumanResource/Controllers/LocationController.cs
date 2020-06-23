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
    public class LocationController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<Location> LocationService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        IEntityService<VHR_Location> VHRLocation;
        public LocationController(IEntityService<Location> locationService, IEntityService<Employee> employeeService, IDDService dDService, IEntityService<VHR_Location> vHRLocation)
        {
            LocationService = locationService;
            EmployeeService = employeeService;
            DDService = dDService;
            VHRLocation = vHRLocation;
        }
        public ActionResult Index()
        {
            List<VHR_Location> list = VHRLocation.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            Location obj = new Location();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(Location obj)
        {
            if (obj.LocationName == null || obj.LocationName == "")
                ModelState.AddModelError("CrewName", "Crew name cannot be empty");
            if (LocationService.GetIndex().Where(aa => aa.LocationName == obj.LocationName).Count() > 0)
                ModelState.AddModelError("CrewName", "Crew name must be unique");
            if (ModelState.IsValid)
            {
                LocationService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Locations, AuditTypeCommon.Add, obj.PLocationID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        public ActionResult Edit(int? id)
        {
            Location obj = LocationService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Location obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.LocationID == obj.PLocationID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            }
            if (ModelState.IsValid)
            {
                LocationService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        public ActionResult Delete(int? id)
        {
            return PartialView(LocationService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Location obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.LocationID == obj.PLocationID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                LocationService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Locations, AuditTypeCommon.Delete, obj.PLocationID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        private void HelperMethod(Location obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.CommonLocationID = new SelectList(DDService.GetCommanLocation(LoggedInUser).ToList().OrderBy(aa => aa.CommonLocationName).ToList(), "PCommonLocationID", "CommonLocationName", obj.CommonLocationID);
        }
    }
}