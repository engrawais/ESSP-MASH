using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class HolidayController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<Holiday> HolidayService;
        IEntityService<DeviceData> DeviceDataService;
        public HolidayController(IEntityService<Holiday> holidayService, IEntityService<DeviceData> deviceDataService)
        {
            HolidayService = holidayService;
            DeviceDataService = deviceDataService;
        }
        public ActionResult Index()
        {
            List<Holiday> list = HolidayService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Holiday obj)
        {
            if (HolidayService.GetIndex().Where(aa => aa.HolidayDate == obj.HolidayDate && aa.HolidayID != obj.HolidayID).Count() > 0)
                ModelState.AddModelError("HolidayDate", "Already exists in same date");
            if (ModelState.IsValid)
            {
                obj.CreatedDate = DateTime.Now;
                HolidayService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return PartialView(HolidayService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Holiday obj)
        {
            if (ModelState.IsValid)
            {
                HolidayService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(HolidayService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Holiday obj)
        {
            HolidayService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}