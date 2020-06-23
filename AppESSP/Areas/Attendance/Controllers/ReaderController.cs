using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class ReaderController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<Reader> ReaderService;
        IEntityService<VAT_Reader> VATReaderService;
        IEntityService<DeviceData> DeviceDataService;
        IDDService DDService;
        public ReaderController(IEntityService<Reader> readerServiceService, IEntityService<DeviceData> deviceDataService,IDDService dDService, IEntityService<VAT_Reader> vATReaderService)
        {
            VATReaderService = vATReaderService;
            ReaderService = readerServiceService;
            DeviceDataService = deviceDataService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<VAT_Reader> list = VATReaderService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            HelperMethod(new Reader());
            return View();
        }
        [HttpPost]
        public ActionResult Create(Reader obj)
        {
            if (obj.IpAdd != null)
            {
                if (obj.IpAdd.Length > 15)
                    ModelState.AddModelError("IpAdd", "String length exceeds!");
                Match match = Regex.Match(obj.IpAdd, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (!match.Success)
                {
                    ModelState.AddModelError("IpAdd", "Enter a valid IP Address");
                }
            }

            if (!string.IsNullOrEmpty(obj.IpPort.ToString()))
            {
                if (obj.IpPort.ToString().Length > 4)
                    ModelState.AddModelError("IpPort", "String length exceeds!");

            }

            if (obj.ReaderName == null || obj.ReaderName == "")
                ModelState.AddModelError("ReaderName", "Reader Name cannot be empty");
            if (ReaderService.GetIndex().Where(aa => aa.ReaderName == obj.ReaderName).Count() > 0)
                ModelState.AddModelError("ReaderName", "Reader Name must be unique");
            if (ModelState.IsValid)
            {

                ReaderService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            if (ModelState.IsValid)
            {
                ReaderService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Reader obj = ReaderService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Reader obj)
        {
            if (obj.IpAdd != null)
            {
                if (obj.IpAdd.Length > 15)
                    ModelState.AddModelError("IpAdd", "String length exceeds!");
                Match match = Regex.Match(obj.IpAdd, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (!match.Success)
                {
                    ModelState.AddModelError("IpAdd", "Enter a valid IP Address");
                }
            }

            if (!string.IsNullOrEmpty(obj.IpPort.ToString()))
            {
                if (obj.IpPort.ToString().Length > 4)
                    ModelState.AddModelError("IpPort", "String length exceeds!");

            }
            if (obj.ReaderName == null || obj.ReaderName == "")
                ModelState.AddModelError("ReaderName", "Reader Name cannot be empty");
            if (ReaderService.GetIndex().Where(aa => aa.ReaderName == obj.ReaderName && aa.PReaderID != obj.PReaderID).Count() > 0)
                ModelState.AddModelError("ReaderName", "Reader Name must be unique");
            if (ModelState.IsValid)
            {
                ReaderService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(ReaderService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(Reader obj)
        {
            ReaderService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #region -- Private Method--
        private void HelperMethod(Reader obj)
        {
            ViewBag.LocID = new SelectList(DDService.GetLocation().ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName",obj.LocID);
            ViewBag.RdrTypeID = new SelectList(DDService.GetReaderType().ToList().OrderBy(aa => aa.ReaderTypeName).ToList(), "PReaderTypeID", "ReaderTypeName",obj.RdrTypeID);
            ViewBag.ReaderDutyCodeID = new SelectList(DDService.GetReaderDutyCode().ToList().OrderBy(aa => aa.ReaderDutyName).ToList(), "PReaderDutyID", "ReaderDutyName",obj.ReaderDutyCodeID);
        }
        #endregion
    }
}