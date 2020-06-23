using AppESSP.App_Start;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class CrewController : Controller
    {
        // GET: HumanResource/Crew
        IEntityService<Crew> CrewService;
        IEntityService<VHR_Crew> VHRCrewService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public CrewController(IEntityService<Crew> crewService, IEntityService<Employee> employeeService, IDDService dDService, IEntityService<VHR_Crew> vHRCrewService)
        {
            CrewService = crewService;
            EmployeeService = employeeService;
            DDService = dDService;
            VHRCrewService = vHRCrewService;
        }
        public ActionResult Index()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VHR_Crew> tempList = new List<VHR_Crew>();
            List<VHR_Crew> list = VHRCrewService.GetIndex();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                foreach (var item in LoggedInUser.UserLoctions)
                {
                    tempList.AddRange(list.Where(aa => aa.LocationID == item.LocationID).ToList());
                }
            }
            else
                tempList = list.ToList();
            return View(tempList);
        }
        [HttpGet]

        public ActionResult Create()
        {
            Crew obj = new Crew();
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(Crew obj)
        {
            if (obj.CrewName == null || obj.CrewName == "")
                ModelState.AddModelError("CrewName", "Crew name cannot be empty");
            if (CrewService.GetIndex().Where(aa => aa.CrewName == obj.CrewName).Count() > 0)
                ModelState.AddModelError("CrewName", "Crew name must be unique");
            if (ModelState.IsValid)
            {
                CrewService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Crew, AuditTypeCommon.Add, obj.PCrewID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Crew obj = CrewService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Crew obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.CrewID == obj.PCrewID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot Inactive due to attachment of active employees");
            }
            if (obj.CrewName == null || obj.CrewName == "")
                ModelState.AddModelError("CrewName", "Crew name cannot be empty");
            if (CrewService.GetIndex().Where(aa => aa.CrewName == obj.CrewName && aa.PCrewID != obj.PCrewID).Count() > 0)
                ModelState.AddModelError("CrewName", "Crew name must be unique");
            if (ModelState.IsValid)
            {
                CrewService.PostEdit(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Crew, AuditTypeCommon.Edit, obj.PCrewID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(CrewService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Crew obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.CrewID == obj.PCrewID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                CrewService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.Crew, AuditTypeCommon.Delete, obj.PCrewID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        #region -- Private Method--
        private void HelperMethod(Crew obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LocationID = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
        }
        #endregion

    }
}