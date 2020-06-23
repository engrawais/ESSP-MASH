using ESSPCORE.Common;
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
    public class ShiftController : Controller
    {
        // GET: Attendance/Shift
        IEntityService<Shift> ShiftService;
        IEntityService<VAT_Shift> VATShiftService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public ShiftController(IEntityService<Shift> shiftService, IEntityService<Employee> employeeService, IDDService dDService, IEntityService<VAT_Shift> vATShiftService)
        {
            ShiftService = shiftService;
            DDService = dDService;
            VATShiftService = vATShiftService;
            EmployeeService = employeeService;
        }
        public ActionResult Index()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_Shift> list = DDService.GetShift(LoggedInUser);
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            Shift obj = new Shift();
            obj.BreakMin = 0;
            obj.HalfDayBreakMin = 0;
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(Shift obj)
        {
            if (obj.ShiftName==null|| obj.ShiftName=="")
                 ModelState.AddModelError("ShiftName", "Shift name cannot be empty");
            if (ShiftService.GetIndex().Where(aa=>aa.ShiftName==obj.ShiftName).Count()>0)
                ModelState.AddModelError("ShiftName", "Shift name must be unique");
            if (ModelState.IsValid)
            {

                ShiftService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Shift obj = ShiftService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Shift obj)
        {
            if (obj.ShiftName == null || obj.ShiftName == "")
                ModelState.AddModelError("ShiftName", "Shift name cannot be empty");
            if (ShiftService.GetIndex().Where(aa => aa.ShiftName == obj.ShiftName && aa.PShiftID != obj.PShiftID).Count() > 0)
                ModelState.AddModelError("ShiftName", "Shift name must be unique");
            if (ModelState.IsValid)
            {
                ShiftService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(ShiftService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Shift obj)
        {
            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.ShiftID == obj.PShiftID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
            { ModelState.AddModelError("EarlyIn", "You cannot inactive due to attachment of active employees"); }
            if (ModelState.IsValid)
            {
                ShiftService.PostDelete(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }

        #region -- Private Method--
        private void HelperMethod(Shift obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LocationID = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
            ViewBag.DayOff1 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName", obj.DayOff1);
            ViewBag.DayOff2 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName", obj.DayOff2);
        }
        #endregion
    }
}