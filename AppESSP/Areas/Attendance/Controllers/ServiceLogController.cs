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
    public class ServiceLogController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<VAT_ServiceLog> ServiceLogService;
        IEntityService<DeviceData> DeviceDataService;
        IDDService DDService;
        public ServiceLogController(IEntityService<VAT_ServiceLog> serviceLogyService, IEntityService<DeviceData> deviceDataService,
             IDDService dDService)
        {
            ServiceLogService = serviceLogyService;
            DeviceDataService = deviceDataService;
            DDService = dDService;
        }
        public ActionResult Index(int? ErrorCode, int? ReaderID, DateTime? DateStart, DateTime? DateEnd)
        {
            if (ErrorCode == null)
                ErrorCode = 0;
            if (ReaderID == null)
                ReaderID = 0;
            if (DateStart == null)
                DateStart = DateTime.Today.AddDays(-1);
            if (DateEnd == null)
                DateEnd = DateTime.Now;
            else
                DateEnd = DateEnd + new TimeSpan(23,59,59);
            ViewBag.ErrorCode = new SelectList(
                                new List<SelectListItem>
                                {
                                     new SelectListItem { Text = "All", Value = "0"},
                                    new SelectListItem { Text = "Failed", Value = "1"},
                                    new SelectListItem { Text = "Success", Value = "5"},
                                }, "Value", "Text");
            List<Reader> readers = DDService.GetReader().ToList().OrderBy(aa => aa.ReaderName).ToList();
            readers.Insert(0, new Reader { PReaderID = 0, ReaderName = "All" });
            ViewBag.ReaderID = new SelectList(readers, "PReaderID", "ReaderName", ReaderID);
            Expression<Func<VAT_ServiceLog, bool>> SpecificEntries = c => c.DateTime >=DateStart && c.DateTime<=DateEnd;
            List<VAT_ServiceLog> list = ServiceLogService.GetIndexSpecific(SpecificEntries);
            if (ReaderID > 0)
                list = list.Where(aa => aa.PReaderID == ReaderID).ToList();
            if (ErrorCode > 0)
                list = list.Where(aa => aa.ErrorCode == ErrorCode).ToList();
            ViewBag.DateStart = DateStart.Value.ToString("yyyy-MM-dd");
            ViewBag.DateEnd = DateEnd.Value.ToString("yyyy-MM-dd");
            if (DateStart > DateEnd)
            {
                ViewBag.Message = "Start date must be less than end date!";
            }
            return View(list.OrderByDescending(aa=>aa.DateTime).ToList());
        }
        #region -- Private Method--
        
        #endregion
    }
}