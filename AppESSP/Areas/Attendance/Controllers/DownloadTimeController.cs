using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class DownloadTimeController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<DownloadTime> DownloadTimeService;
        IEntityService<DeviceData> DeviceDataService;
        public DownloadTimeController(IEntityService<DownloadTime> downloadTimeService, IEntityService<DeviceData> deviceDataService)
        {
            DownloadTimeService = downloadTimeService;
            DeviceDataService = deviceDataService;
        }
        public ActionResult Index()
        {
            List<DownloadTime> list = DownloadTimeService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DownloadTime obj)
        {
            if (ModelState.IsValid)
            {
                DownloadTimeService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return PartialView(DownloadTimeService.GetEdit((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(DownloadTime obj)
        {
            if (ModelState.IsValid)
            {
                DownloadTimeService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        public ActionResult Delete(int? id)
        {
            return PartialView(DownloadTimeService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(DownloadTime obj)
        {
            DownloadTimeService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}