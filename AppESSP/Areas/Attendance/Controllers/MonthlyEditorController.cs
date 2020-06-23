using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class MonthlyEditorController : Controller
    {
        IDDService DDService;
        IMonthlyEditorService MonthlyEditorService;
        IRepository<VAT_MonthlySummary> VMonthlyDataRepository;
        IRepository<MonthData> MonthDataRepository;
        IRepository<MonthDataEdit> MonthDataEditRepository;
        public MonthlyEditorController(IDDService dDService, IMonthlyEditorService monthlyEditorService, IRepository<VAT_MonthlySummary> monthlyDataRepository,
            IRepository<MonthData> monthDataRepository, IRepository<MonthDataEdit> monthDataEditRepository)
        {
            DDService = dDService;
            MonthlyEditorService = monthlyEditorService;
            VMonthlyDataRepository = monthlyDataRepository;
            MonthDataRepository = monthDataRepository;
            MonthDataEditRepository = monthDataEditRepository;
        }
        // GET: Attendance/MonthlyEditor
        public ActionResult Index()
        {
            VMJobCardCreate vmJobCardCreate = new VMJobCardCreate();
            vmJobCardCreate = MonthlyEditorService.GetIndex();
            ViewBag.PayrolPeriodID = new SelectList(DDService.GetPayrollPeriod().ToList().OrderBy(aa => aa.PRName).ToList(), "PPayrollPeriodID", "PRName");
            return View(vmJobCardCreate);
        }
        public ActionResult Create1()
        {
            VMEditMonthlyCreate vmJobCardCreate = new VMEditMonthlyCreate();
            vmJobCardCreate = MonthlyEditorService.GetCreate1();
            ViewBag.PayrolPeriodID = new SelectList(DDService.GetPayrollPeriod().ToList().OrderBy(aa => aa.PRName).ToList(), "PPayrollPeriodID", "PRName");
            return View(vmJobCardCreate);
        }
        [HttpPost]
        public ActionResult Create1(VMEditMonthlyCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            es = MonthlyEditorService.GetCreate2(es, SelectedCompanyIds, SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
            SelectedCrewIds, SelectedShiftIds, LoggedInUser);
            return View("Create2", es);
        }

        public ActionResult Create2(VMEditMonthlyCreate es, int?[] SelectedEmpIds)
        {
            if (Request.Form["PayrolPeriodID"].ToString() != "")
            {
                int PayrollPID = Convert.ToInt32(Request.Form["PayrolPeriodID"].ToString());
                List<VAT_MonthlySummary> MonthlyAttendance = new List<VAT_MonthlySummary>();
                List<VAT_MonthlySummary> tempMonthlyAttendance = new List<VAT_MonthlySummary>();
                //var checkedEmps = form.GetValues("cbEmployee");
                Expression<Func<VAT_MonthlySummary, bool>> MonthlyAttendances = aa => aa.PayrollPeriodID == PayrollPID;
                MonthlyAttendance = VMonthlyDataRepository.FindBy(MonthlyAttendances);
                foreach (var item in SelectedEmpIds)
                {
                    int empid = Convert.ToInt32(item);
                    if (MonthlyAttendance.Where(aa => aa.EmployeeID == empid).Count() > 0)
                        tempMonthlyAttendance.Add(MonthlyAttendance.First(aa => aa.EmployeeID == empid));
                }
                if (tempMonthlyAttendance.Count > 0)
                    return View("Create3", MonthlyEditorService.GetMonthlyAttendanceAttributes(tempMonthlyAttendance, PayrollPID));
                else
                    return RedirectToAction("Create1");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create3(VMEditMonthlyAttendance vm)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            int count = Convert.ToInt32(Request.Form["Count"].ToString());
            List<MonthData> oldAttendance = new List<MonthData>();
            List<int?> empIds = new List<int?>();
            Expression<Func<MonthData, bool>> MonthlyAttendances = aa => aa.PayrollPeriodID == vm.PayrolPeriodID;
            oldAttendance = MonthDataRepository.FindBy(MonthlyAttendances);
            string Message = "";
            for (int i = 1; i <= count; i++)
            {
                int EmpID = Convert.ToInt32(Request.Form["Emp-" + i.ToString()].ToString());
                MonthData att = oldAttendance.First(aa => aa.EmployeeID == EmpID);
                string TotalDays = Request.Form["TD-" + i.ToString()].ToString();
                string PaidDays = Request.Form["PD-" + i.ToString()].ToString();
                string AbsentDays = Request.Form["AB-" + i.ToString()].ToString();
                string Remarks = "";
                EditMonthlyAttendanceList editlist = MonthlyEditorService.GetEditMonthlyAttendanceList(EmpID, vm.PayrolPeriodID,
                    TotalDays, PaidDays, AbsentDays, Remarks);
                string val = MonthlyEditorService.CheckMonthRecordIsEdited(att, editlist);
                if (val == "Time")
                {
                    // SavMonthlyData
                    MonthDataEdit ame = new MonthDataEdit();
                    ame.DataEditDate = DateTime.Now;
                    ame.EmployeeID = att.EmployeeID;
                    ame.NewAbsentDays = editlist.AbsentDays;
                    ame.OldAbsentDays = att.AbsentDays;
                    ame.NewTotalDays = editlist.TotalDays;
                    ame.OldTotalDays = att.TotalDays;
                    ame.NewPaidDays = editlist.PaidDays;
                    ame.OldPaidDays = att.WorkDays;
                    ame.PayrollPeriodID = att.PayrollPeriodID;
                    ame.UserID = LoggedInUser.PUserID;
                    ame.NewRemarks = Remarks;
                    MonthDataEditRepository.Add(ame);
                    MonthDataEditRepository.Save();
                    //Change in AttMonth
                    att.AbsentDays = editlist.AbsentDays;
                    att.WorkDays = editlist.PaidDays;
                    att.TotalDays = editlist.TotalDays;
                    if (att.Remarks != null)
                    {
                        if (!att.Remarks.Contains('M'))
                            att.Remarks = Remarks + "[M]";
                        else
                            att.Remarks = att.Remarks;
                    }
                    else
                        att.Remarks = "[M]";
                    MonthDataRepository.Edit(att);
                    MonthDataRepository.Save();
                    empIds.Add(att.EmployeeID);
                }
            }
            List<VAT_MonthlySummary> MonthlyAttendance = new List<VAT_MonthlySummary>();
            List<VAT_MonthlySummary> tempMonthlyAttendance = new List<VAT_MonthlySummary>();
            //var checkedEmps = form.GetValues("cbEmployee");
            Expression<Func<VAT_MonthlySummary, bool>> MonthlyAttendances2 = aa => aa.PayrollPeriodID == vm.PayrolPeriodID;
            MonthlyAttendance = VMonthlyDataRepository.FindBy(MonthlyAttendances2);
            foreach (var item in empIds)
            {
                int empid = Convert.ToInt32(item);
                if (MonthlyAttendance.Where(aa => aa.EmployeeID == empid).Count() > 0)
                    tempMonthlyAttendance.Add(MonthlyAttendance.First(aa => aa.EmployeeID == empid));
            }
            if (tempMonthlyAttendance.Count > 0)
                return View("Create3", MonthlyEditorService.GetMonthlyAttendanceAttributes(tempMonthlyAttendance, vm.PayrolPeriodID));
            ViewBag.PayrolPeriodID = new SelectList(DDService.GetPayrollPeriod().ToList().OrderBy(aa => aa.PRName).ToList(), "PPayrollPeriodID", "PRName");
            return View("Create1");

        }

        private short? GetTGZOTOTACToPolicy(short? OT, OTPolicy otPolicy)
        {
            short? val = 0;
            if (otPolicy.CalculateGZOT == true)
            {
                if (otPolicy.PerMonthOTLimitHour >= OT / 60)
                {
                    val = OT;
                }
                else if ((OT / 60) > otPolicy.PerMonthOTLimitHour)
                {
                    val = (short)(otPolicy.PerMonthOTLimitHour * 60);
                }
            }
            return val;
        }

        private short? GetRestOTACToPolicy(short? OT, OTPolicy otPolicy)
        {
            short? val = 0;
            if (otPolicy.CalculateRestOT == true)
            {
                if (otPolicy.PerMonthROTLimitHour >= OT / 60)
                {
                    val = OT;
                }
                else if ((OT / 60) > otPolicy.PerMonthROTLimitHour)
                {
                    val = (short)(otPolicy.PerMonthROTLimitHour * 60);
                }
            }
            return val;
        }

        private short? GetNormalOTACToPolicy(short? OT, OTPolicy otPolicy)
        {
            short? val = 0;
            if (otPolicy.CalculateNOT == true)
            {
                if (otPolicy.PerMonthOTLimitHour >= OT / 60)
                {
                    val = OT;
                }
                else if ((OT / 60) > otPolicy.PerMonthOTLimitHour)
                {
                    val = (short)(otPolicy.PerMonthOTLimitHour * 60);
                }
            }
            return val;
        }


        internal short? GetCPLConversionOTMinutes(OTPolicy dbOTPolicy, short? approvedOT)
        {
            short val = 0;
            int maxOTEncashableMins = (short)dbOTPolicy.EncashableOTHour * 60;
            if (approvedOT > maxOTEncashableMins)
            {
                val = (short)(approvedOT - maxOTEncashableMins);
            }
            return val;
        }

        internal short? GetEncashableOTMinutes(OTPolicy dbOTPolicy, short? approvedOT)
        {
            short val = 0;
            int maxOTEncashableMins = (short)dbOTPolicy.EncashableOTHour * 60;
            if (maxOTEncashableMins == 0)
                val = 0;
            else if (maxOTEncashableMins > approvedOT)
            {
                val = (short)maxOTEncashableMins;
            }
            else if (approvedOT < maxOTEncashableMins)
            {
                val = (short)approvedOT;
            }
            return val;
        }
    }
}