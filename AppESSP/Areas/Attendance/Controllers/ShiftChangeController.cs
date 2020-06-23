
﻿using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class ShiftChangeController : Controller
    {
        IEntityService<ShiftChanged> ShiftChangedService;
        IEntityService<VAT_ShiftChanged> VATShiftChangedService;
        IDDService DDService;
        public ShiftChangeController(IEntityService<ShiftChanged> shiftchangedService, IDDService dDService, IEntityService<VAT_ShiftChanged> vATShiftChangedService)
        {
            ShiftChangedService = shiftchangedService;
            DDService = dDService;
            VATShiftChangedService = vATShiftChangedService;
        }
        public ActionResult Index()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_ShiftChanged> list = DDService.GetShiftChange(LoggedInUser);
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()

        {
            ShiftChanged obj = new ShiftChanged();
            obj.BreakMin = 0;
            obj.HalfDayBreakMin = 0;
            HelperMethod(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(ShiftChanged obj)
        {
            if (obj.ChangedShiftDesc == null || obj.ChangedShiftDesc == "")
                ModelState.AddModelError("ChangedShiftDesc", "Title cannot be empty");
            if (ShiftChangedService.GetIndex().Where(aa => aa.ChangedShiftDesc == obj.ChangedShiftDesc).Count() > 0)
                ModelState.AddModelError("ChangedShiftDesc", "Title must be unique");
            if (obj.DateStart != null && obj.DateEnd != null)
            {
                if (obj.DateEnd == obj.DateStart)
                { }
                else
                {
                    if (obj.DateEnd <= obj.DateStart)
                    {
                        ModelState.AddModelError("DateStart", "Start Date can never be greater than end date.");
                    }
                }
            }
            //if (ShiftChangedService.GetIndex().Where(aa => aa.DateStart == obj.DateStart && aa.PShiftChangedID != obj.PShiftChangedID).Count() > 0)
            //    ModelState.AddModelError("DateStart", "Already exists in same date");
            if (ModelState.IsValid)
            {
                ShiftChangedService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ShiftChanged obj = ShiftChangedService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(ShiftChanged obj)
        {
            if (ModelState.IsValid)
            {
                ShiftChangedService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(ShiftChangedService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(ShiftChanged obj)
        {
            ShiftChangedService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        #region -- Private Method--
        private void HelperMethod(ShiftChanged obj)
        {

            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LocationID = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
            ViewBag.ShiftID = new SelectList(DDService.GetShift(LoggedInUser).ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName",obj.ShiftID);
            ViewBag.DayOff1 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName",obj.DayOff1);
            ViewBag.DayOff2 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName", obj.DayOff2);
        }
        #endregion
    }
}