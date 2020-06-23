using AppESSP.Controllers;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class AbsentNotificationController : BaseController
    {
        IEntityService<DailyAttendance> DailyAttendanceService;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        public AbsentNotificationController(IEntityService<DailyAttendance> dailyAttendanceService, IGetSpecificEmployeeService getSpecificEmployeeService)
        {
            DailyAttendanceService = dailyAttendanceService;

            GetSpecificEmployeeService =getSpecificEmployeeService;
        }
        public ActionResult Index(DateTime? dtFrom, DateTime? dtTo)
        {
            dtFrom = DateTime.Today.AddDays(-30);
            dtTo = DateTime.Today;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VMAbsentList> vmAbsentList = new List<VMAbsentList>();
            List<VAT_DailyAttendance> dbVDailyAttendance = GetSpecificEmployeeService.GetSpecificAbsentAttendance(LoggedInUser, (DateTime)dtFrom, (DateTime)dtTo);
            foreach(var empid in dbVDailyAttendance.Select(aa=>aa.EmpID).Distinct().ToList())
            {
                List<VAT_DailyAttendance> dbTempVDailyAttendance = dbVDailyAttendance.Where(aa => aa.EmpID == empid).ToList();
                if (dbTempVDailyAttendance.Count() >= 10)
                {
                    bool IsAbContinous = false;
                    int count = 0;
                    foreach (var item in dbTempVDailyAttendance)
                    {
                        if (item.DutyCode == "D")
                        {
                            if (item.AbDays > 0)
                            {
                                count++;
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                        else if (item.DutyCode == "L")
                        {
                            count++;
                        }
                    }
                    if (count >= 10)
                    {
                        VMAbsentList vmAbsentItem = new VMAbsentList();
                        vmAbsentItem.EmpID = (int)dbTempVDailyAttendance.First().EmpID;
                        vmAbsentItem.EmpNo = dbTempVDailyAttendance.First().OEmpID;
                        vmAbsentItem.Name = dbTempVDailyAttendance.First().EmployeeName;
                        vmAbsentItem.Designation = dbTempVDailyAttendance.First().DesignationName;
                        vmAbsentItem.OUname = dbTempVDailyAttendance.First().OUName;
                        vmAbsentItem.Status = dbTempVDailyAttendance.First().Status;
                        vmAbsentItem.Location = dbTempVDailyAttendance.First().LocationName;
                        vmAbsentItem.AbDays = count;
                        vmAbsentList.Add(vmAbsentItem);
                    }

                }
            }
            return View(vmAbsentList);
        }
    }
}