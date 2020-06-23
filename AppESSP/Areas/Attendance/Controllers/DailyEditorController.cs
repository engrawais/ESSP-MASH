using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESSPCORE.Attendance;
using System.Linq.Expressions;
using AppESSP.Controllers;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class DailyEditorController : BaseController
    {
        IDDService DDService;
        IDailyEditorService DailyAttendanceEditorService;
        IEntityService<DailyAttendance> DailyAttendanceService;
        IEmpSelectionService EmpSelectionService;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        public DailyEditorController(IDDService dDService, IDailyEditorService dailyAttendanceEditorService, IEmpSelectionService empSelectionService,
            IEntityService<DailyAttendance> dailyAttendanceService, IGetSpecificEmployeeService getSpecificEmployeeService)
        {
            DDService = dDService;
            DailyAttendanceEditorService = dailyAttendanceEditorService;
            EmpSelectionService = empSelectionService;
            DailyAttendanceService = dailyAttendanceService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
        }
        // GET: Attendance/
        public ActionResult Index()
        {
            CreateHelper();
            return View();
        }

        #region -- Edit Attendance Single Employee for Multiple Dates --
        // Employee Multiple Date 
        public ActionResult EditMultipleEntries(List<DailyAttendance> dailyAttendance)
        {
            try
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                string empNo = Request.Form["EmpNo"].ToString();
                int empID = 0;
                List<VHR_EmployeeProfile> emps = DDService.GetEmployeeInfo(LoggedInUser).ToList();
                DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                DateTime _AttDataTo = Convert.ToDateTime(Request.Form["DateTo"].ToString());
                if (_AttDataFrom > _AttDataTo)
                {
                    ViewBag.Message = "Start date must be less than end date!";
                }
                else
                {
                    if (DDService.IsDateLieBetweenActivePayroll(_AttDataFrom))
                    {
                        if (emps.Where(aa => aa.OEmpID == empNo).Count() > 0)
                        {
                            empID = emps.Where(aa => aa.OEmpID == empNo).First().PEmployeeID;
                            return View(DailyAttendanceEditorService.GetAttendanceAttributes(dailyAttendance, _AttDataFrom, _AttDataTo, empID));
                        }
                        ViewBag.Message = "Employee not found";
                    }
                    else
                        ViewBag.Message = "Payroll Period is closed";
                }
                CreateHelper();
                return View("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult SaveEditEntries()
        {
            bool edited = false;
            int empID = Convert.ToInt32(Request.Form["empID"].ToString());
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            DateTime dtFrom = Convert.ToDateTime(Request.Form["dateFrom"].ToString());
            DateTime dtTo = Convert.ToDateTime(Request.Form["dateTo"].ToString());
            List<DailyAttendance> oldAttendance = DailyAttendanceEditorService.GetEmployeeAttendance(empID, dtFrom, dtTo);
            for (int i = 1; i <= oldAttendance.Count; i++)
            {
                string empDate = Request.Form["EmpDate-" + i.ToString()].ToString();
                DailyAttendance att = oldAttendance.First(aa => aa.EmpDate == empDate);
                string DutyCode = Request.Form["DutyCode-" + i.ToString()].ToString();
                string DutyTime = Request.Form["DutyTime-" + i.ToString()].ToString();
                string ShiftTime = Request.Form["ShiftTime-" + i.ToString()].ToString();
                string TimeIn = Request.Form["TimeIn-" + i.ToString()].ToString();
                string TimeOut = Request.Form["TimeOut-" + i.ToString()].ToString();
                string Remarks = Request.Form["Remarks-" + i.ToString()].ToString();
                EditAttendanceList editlist = DailyAttendanceEditorService.GetEditAttendanceList(empDate, DutyCode, DutyTime, ShiftTime, TimeIn, TimeOut, Remarks);
                if (DailyAttendanceEditorService.CheckRecordIsEdited(att, editlist))
                {
                    edited = true;
                    DateTime _NewTimeIn = new DateTime();
                    DateTime _NewTimeOut = new DateTime();
                    _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);

                    if (editlist.TimeIn != null && editlist.TimeOut != null)
                    {
                        if (editlist.TimeOut < editlist.TimeIn)
                        {
                            _NewTimeOut = att.AttDate.Value.AddDays(1) + editlist.TimeOut;
                        }
                        else
                        {
                            _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                        }
                        DailyAttendanceEditorService.ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, LoggedInUser.PUserID, editlist.DutyTime, Remarks, (short)editlist.ShiftTime.TotalMinutes);
                    }
                    else
                    {
                        if (editlist.TimeIn.TotalMinutes > 0)
                            _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);
                        if (editlist.TimeOut.TotalMinutes > 0)
                            _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                        DailyAttendanceEditorService.ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, LoggedInUser.PUserID, editlist.DutyTime, Remarks, (short)editlist.ShiftTime.TotalMinutes);
                    }
                }

            }
            // Process Monthly Attendance
            if(edited==true)
            {
                DDService.ProcessMonthlyAttendance(dtFrom, empID, empID.ToString());
            }
            List<DailyAttendance> dailyAttendance = new List<DailyAttendance>();
            dailyAttendance = DailyAttendanceEditorService.GetEmployeeAttendance(empID, dtFrom, dtTo);
            return View("EditMultipleEntries", DailyAttendanceEditorService.GetAttendanceAttributes(dailyAttendance, dtFrom, dtTo, empID));
        }
        #endregion

        #region -- Edit Attendance Multiple Employees for Single Dates --
        // Criteria Based Single Date
        public ActionResult EditDateWiseEntries()
        {
            try
            {
                DateTime _AttDataTo = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                if (DDService.IsDateLieBetweenActivePayroll(_AttDataTo))
                {
                    string allEmployees = Request.Form["RosterSelectionRB"].ToString();
                    string shift = Request.Form["ShiftList"].ToString();
                    string group = Request.Form["GroupList"].ToString();
                    string deoartment = Request.Form["DepartmentList"].ToString();
                    string section = Request.Form["SectionList"].ToString();
                    string location = Request.Form["LocationList"].ToString();
                    VMEditAttendanceDateWise vm = DailyAttendanceEditorService.EditDateWiseEntries(_AttDataTo, Request.Form["RosterSelectionRB"].ToString(),
                    Request.Form["ShiftList"].ToString(), Request.Form["LocationList"].ToString(),
                    Request.Form["GroupList"].ToString(), Request.Form["DepartmentList"].ToString(),
                    Request.Form["SectionList"].ToString());
                    return View(vm);
                }
                else
                {
                    CreateHelper();
                    ViewBag.Message = "Payroll Period is closed";
                    return View("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveEditEntriesDateWise(AttEditSingleEmployee Model)
        {
            try
            {
                int count = Convert.ToInt32(Request.Form["Count"].ToString());
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DateTime dt = Convert.ToDateTime(Request.Form["Date"].ToString());
                string Criteria = Request.Form["Criteria"].ToString();
                int CriteriaData = Convert.ToInt32(Request.Form["CriteriaData"].ToString());
                string CriteriaRBName = (Request.Form["CriteriaRBName"].ToString());
                List<DailyAttendance> oldAttendance = new List<DailyAttendance>();
                Expression<Func<DailyAttendance, bool>> rbAll = aa => aa.AttDate == dt;
                oldAttendance = DailyAttendanceService.GetIndexSpecific(rbAll);
                string Message = "";
                for (int i = 1; i <= count; i++)
                {
                    //int EmpID = Convert.ToInt32(Request.Form["Row" + i.ToString()].ToString());
                    string empDate = Request.Form["EmpDate-" + i.ToString()].ToString();
                    DailyAttendance att = oldAttendance.First(aa => aa.EmpDate == empDate);
                    string DutyCode = Request.Form["DutyCode-" + i.ToString()].ToString();
                    string DutyTime = Request.Form["DutyTime-" + i.ToString()].ToString();
                    string ShiftTime = Request.Form["ShiftTime-" + i.ToString()].ToString();
                    string TimeIn = Request.Form["TimeIn-" + i.ToString()].ToString();
                    string TimeOut = Request.Form["TimeOut-" + i.ToString()].ToString();
                    string Remarks = Request.Form["Remarks-" + i.ToString()].ToString();
                    EditAttendanceList editlist = DailyAttendanceEditorService.GetEditAttendanceList(empDate, DutyCode, DutyTime, ShiftTime, TimeIn,TimeOut, Remarks);

                    if (DailyAttendanceEditorService.CheckRecordIsEdited(att, editlist))
                    {
                        DateTime _NewTimeIn = new DateTime();
                        DateTime _NewTimeOut = new DateTime();
                        _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);

                        if (editlist.TimeIn != null && editlist.TimeOut != null)
                        {
                            if (editlist.TimeOut < editlist.TimeIn)
                            {
                                _NewTimeOut = att.AttDate.Value.AddDays(1) + editlist.TimeOut;
                            }
                            else
                            {
                                _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                            }
                            DailyAttendanceEditorService.ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, LoggedInUser.PUserID, editlist.DutyTime, Remarks, (short)editlist.ShiftTime.TotalMinutes);
                        }
                        else
                        {
                            if (editlist.TimeIn.TotalMinutes > 0)
                                _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);
                            if (editlist.TimeOut.TotalMinutes > 0)
                                _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                            DailyAttendanceEditorService.ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, LoggedInUser.PUserID, editlist.DutyTime, Remarks, (short)editlist.ShiftTime.TotalMinutes);
                        }
                        DDService.ProcessMonthlyAttendance(dt, (int)att.EmpID, att.EmpNo);
                    }
                    else
                    {

                    }
                }
                VMEditAttendanceDateWise vm = DailyAttendanceEditorService.EditDateWiseEntries(dt, CriteriaRBName.ToString(), CriteriaData.ToString(), CriteriaData.ToString(), CriteriaData.ToString(), CriteriaData.ToString(), CriteriaData.ToString());
                return View("EditDateWiseEntries", vm);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult PreviousEntry(string CriteriaRBName,DateTime? dt,string CriteriaData)
        {
            VMEditAttendanceDateWise vm = DailyAttendanceEditorService.EditDateWiseEntries(dt.Value.AddDays(-1), CriteriaRBName,
                    CriteriaData, CriteriaData,
                    CriteriaData, CriteriaData,
                    CriteriaData);
            return View("EditDateWiseEntries", vm);
        }
        public ActionResult NextEntry(string CriteriaRBName, DateTime? dt, string CriteriaData)
        {
            VMEditAttendanceDateWise vm = DailyAttendanceEditorService.EditDateWiseEntries(dt.Value.AddDays(1), CriteriaRBName,
                    CriteriaData, CriteriaData,
                    CriteriaData, CriteriaData,
                    CriteriaData);
            return View("EditDateWiseEntries", vm);
        }
        #endregion
        private void CreateHelper()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.JobCardType = new SelectList(DDService.GetJobCardType().OrderBy(aa => aa.JobCardName).ToList(), "PJobCardTypeID", "JobCardName");
            //ViewBag.RosterType = new SelectList(DDService.ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName");
            ViewBag.ShiftList = new SelectList(DDService.GetShift(LoggedInUser).ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName");
            ViewBag.LocationList = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName");
            ViewBag.GroupList = new SelectList(DDService.GetCrew(LoggedInUser).ToList().OrderBy(aa => aa.CrewName).ToList(), "PCrewID", "CrewName");
            ViewBag.SectionList = new SelectList(DDService.GetOU(LoggedInUser).ToList().OrderBy(aa => aa.OUName).ToList(), "POUID", "OUName");
            ViewBag.DepartmentList = new SelectList(DDService.GetOUCommon(LoggedInUser).ToList().OrderBy(aa => aa.OUCommonName).ToList(), "POUCommonID", "OUCommonName");
        }

        public ActionResult LoadEmployeeMonthlyAttendance(int? EmpID, int? PRID)
        {
            PayrollPeriod prp = DDService.GetPayrollPeriod().Where(aa => aa.PPayrollPeriodID == PRID).First();
            return View("EditMultipleEntries", DailyAttendanceEditorService.GetAttendanceAttributes(new List<DailyAttendance>(), (DateTime)prp.PRStartDate, (DateTime)prp.PREndDate, (int)EmpID));
        }

        // Dashboard
        #region Dashboard Links
        public ActionResult LoadTMDashboardAttendance(DateTime? date,string Criteria)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            VMEditAttendanceDateWise vm = DailyAttendanceEditorService.GetTMDashboardAttendance((DateTime)date, Criteria, GetSpecificEmployeeService.GetSpecificAttendance(LoggedInUser,(DateTime)date));
            return View("EditDateWiseEntries",vm);
        }
        #endregion
    }
}