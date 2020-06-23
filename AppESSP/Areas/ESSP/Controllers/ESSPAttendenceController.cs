using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.ESSP.Controllers
{
    public class ESSPAttendenceController : Controller
    {
        // GET: ESSP/ESSPAttendence
        #region -- Controller Initialization --
        IEntityService<DailyAttendance> DailyAttendanceService;
        IDDService DDService;
        //Controller Constructor
        public ESSPAttendenceController(IDDService ddService, IEntityService<DailyAttendance> dailyAttendanceService)
        {
            DailyAttendanceService = dailyAttendanceService;
            DDService = ddService;
        }
        #endregion
        #region -- Controller Main View Actions  --
        public ActionResult Index()
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            VMESSPAttendence vmESSPAttendence = new VMESSPAttendence();
            vmESSPAttendence.VMDailyAttendence = new List<DailyAttendance>();
            vmESSPAttendence.DateStart = DateTime.Today.AddDays(-7);
            vmESSPAttendence.DateEnd = DateTime.Today.AddDays(-1);
            vmESSPAttendence.EmpID = vmf.UserEmpID;
            Expression<Func<DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= vmESSPAttendence.DateStart && c.AttDate <= vmESSPAttendence.DateEnd && c.EmpID == vmESSPAttendence.EmpID;
            vmESSPAttendence.VMDailyAttendence = DailyAttendanceService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.AttDate).ToList();
            ViewBag.EmpID = new SelectList(DDService.GetReportingToEmployees(vmf).ToList(), "PEmployeeID", "EmployeeName", vmf.UserEmpID);
            vmESSPAttendence =LoadHeaderValues(vmESSPAttendence);
            return View(vmESSPAttendence);
        }
        [HttpPost]
        public ActionResult Index(VMESSPAttendence obj)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            DateTime dt = new DateTime(2018, 07, 01);//Date must be extrected or modified from Database
            if (obj.DateStart < dt)
            {
                obj.DateStart = dt;

            }
            if (obj.DateEnd >= DateTime.Today)
            {
                obj.DateEnd = DateTime.Today.AddDays(-1);
            }
            Expression<Func<DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= obj.DateStart && c.AttDate <= obj.DateEnd && c.EmpID == obj.EmpID;
            obj.VMDailyAttendence = DailyAttendanceService.GetIndexSpecific(SpecificEntries).OrderByDescending(aa => aa.AttDate).ToList();
            obj =LoadHeaderValues(obj);
            ViewBag.EmpID = new SelectList(DDService.GetEmployeeInfo(vmf).ToList(), "PEmployeeID", "EmployeeName");
            return View(obj);
        }
        #endregion
        #region -- Controller Private  Methods--
        private VMESSPAttendence LoadHeaderValues(VMESSPAttendence obj)
        {
            if (obj.VMDailyAttendence.Count > 0)
            {
                obj.WorkingDays = obj.VMDailyAttendence.Where(aa => aa.DutyCode != "R" && aa.DutyCode != "G").Count();
                obj.PresentDays = obj.VMDailyAttendence.Sum(aa => aa.PDays);
                obj.AbsentDays = obj.VMDailyAttendence.Sum(aa => aa.AbDays);
                obj.LeaveDays = obj.VMDailyAttendence.Sum(aa => aa.LeaveDays);
                obj.RestDays = obj.VMDailyAttendence.Where(aa => aa.DutyCode == "R" || aa.DutyCode == "G").Count();
                obj.LateInDays = obj.VMDailyAttendence.Where(aa => aa.LateIn > 0).Count();
                obj.EarlyOutDays = obj.VMDailyAttendence.Where(aa => aa.EarlyOut > 0).Count();
            }
            return obj;
        }
        #endregion
    }
}