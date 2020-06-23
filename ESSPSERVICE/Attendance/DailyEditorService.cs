using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using ESSPREPO.Generic;
using ESSPCORE.Attendance;
using System.Linq.Expressions;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Implemntation of IDailyEditorService
    /// </summary>
    /// <remarks></remarks>
    public class DailyEditorService : IDailyEditorService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<DailyAttendance> DailyAttendanceRepo;
        IRepository<DailyAttendanceEdit> DailyAttendanceEditRepo;
        IRepository<ShiftChangedEmp> ShiftChangedEmpRepo;
        IRepository<ShiftChanged> ShiftChangedRepo;
        IRepository<Shift> ShiftRepo;
        public DailyEditorService(IDDService dDService, IEmpSelectionService empSelectionService, IRepository<DailyAttendance> dailyAttendanceService,
            IRepository<DailyAttendanceEdit> dailyAttendanceEditService, IRepository<ShiftChangedEmp> shiftChangedEmpService, IRepository<Shift> shiftService,
            IRepository<ShiftChanged> shiftChangedService)
        {
            DDService = dDService;
            EmpSelectionService = empSelectionService;
            DailyAttendanceRepo = dailyAttendanceService;
            DailyAttendanceEditRepo = dailyAttendanceEditService;
            ShiftChangedEmpRepo = shiftChangedEmpService;
            ShiftRepo = shiftService;
            ShiftChangedRepo = shiftChangedService;
        }

        public List<DailyAttendance> GetIndex()
        {
            return DailyAttendanceRepo.GetAll();
        }
        public bool CheckRecordIsEdited(DailyAttendance att, EditAttendanceList editlist)
        {
            //check for attendance is edited
            bool edited = false;
            TimeSpan breakmin = new TimeSpan();
            if (att.BreakMin > 0)
                breakmin = new TimeSpan(0, (int)att.BreakMin, 0);
            else
                breakmin = new TimeSpan(0, 0, 0);
            if (att.DutyCode != editlist.DutyCode)
                edited = true;
            if (att.DutyTime != editlist.DutyTime)
                edited = true;
            if (att.ShifMin != editlist.ShiftTime.TotalMinutes)
                edited = true;
            if (att.TimeIn != null)
            {
                if (editlist.TimeIn != null)
                {
                    if (att.TimeIn.Value.TimeOfDay.Hours.ToString("00") + att.TimeIn.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeIn.Hours.ToString("00") + editlist.TimeIn.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeIn != null)
                    if (editlist.TimeIn.TotalMinutes > 0)
                        edited = true;
            }
            if (att.TimeOut != null)
            {
                if (editlist.TimeOut != null)
                {
                    if (att.TimeOut.Value.TimeOfDay.Hours.ToString("00") + att.TimeOut.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeOut.Hours.ToString("00") + editlist.TimeOut.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeOut != null)
                    if (editlist.TimeOut.TotalMinutes > 0)
                        edited = true;
            }
            if (att.DutyCode != editlist.DutyCode)
                edited = true;

            return edited;
        }

        public TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;
                TimeSpan _currentTime = new TimeSpan();
                if (p != "")
                {
                    for (int i = 0; i < stringLength; i += chunkSize)
                    {
                        count++;
                        if (count == 1)
                        {
                            hour = p.Substring(i, chunkSize);
                        }
                        if (count == 2)
                        {
                            min = p.Substring(i, chunkSize);
                        }
                        if (i + chunkSize > stringLength)
                        {
                            chunkSize = stringLength - i;
                        }
                    }
                    _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                }
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }

        public VMEditAttendanceDateWise EditDateWiseEntries(DateTime _AttDataTo, string Selection, string ShiftList, string LocationList, string GroupList, string DepartmentList, string SectionList)
        {
            VMEditAttendanceDateWise vm = new VMEditAttendanceDateWise();
            int selectedID = 0;
            string CriteriaRBName = "";
            string Criteria = "";
            List<DailyAttendance> dailyAttendance = new List<DailyAttendance>();
            switch (Selection)
            {
                case "rbAll":
                    Expression<Func<DailyAttendance, bool>> rbAll = aa => aa.AttDate == _AttDataTo;
                    dailyAttendance = DailyAttendanceRepo.FindBy(rbAll);
                    CriteriaRBName = "rbAll";
                    break;
                case "rbShift":
                    selectedID = Convert.ToInt32(ShiftList);
                    CriteriaRBName = "rbShift";
                    Expression<Func<DailyAttendance, bool>> rbShift = aa => aa.AttDate == _AttDataTo && aa.Employee.ShiftID == selectedID;
                    dailyAttendance = DailyAttendanceRepo.FindBy(rbShift);
                    if (dailyAttendance.Count > 0)
                        Criteria = "Shift: " + dailyAttendance.First().Employee.Shift.ShiftName;
                    break;
                case "rbLocation":
                    selectedID = Convert.ToInt32(LocationList);
                    CriteriaRBName = "rbLocation";
                    Expression<Func<DailyAttendance, bool>> rbLocation = aa => aa.AttDate == _AttDataTo && aa.Employee.LocationID == selectedID;
                    dailyAttendance = DailyAttendanceRepo.FindBy(rbLocation);
                    if (dailyAttendance.Count > 0)
                        Criteria = "Location: " + dailyAttendance.First().Employee.Location.LocationName;
                    break;

                case "rbGroup":
                    selectedID = Convert.ToInt32(GroupList);
                    CriteriaRBName = "rbGroup";
                    Expression<Func<DailyAttendance, bool>> rbGroup = aa => aa.AttDate == _AttDataTo && aa.Employee.CrewID == selectedID;

                    dailyAttendance = DailyAttendanceRepo.FindBy(rbGroup);
                    if (dailyAttendance.Count > 0)
                        Criteria = "Group: " + dailyAttendance.First().Employee.Crew.CrewName;
                    break;
                case "rbDepartment":
                    selectedID = Convert.ToInt32(DepartmentList);
                    CriteriaRBName = "rbDepartment";
                    Expression<Func<DailyAttendance, bool>> rbDepartment = aa => aa.AttDate == _AttDataTo && aa.Employee.OrganizationalUnit.OUCommon.POUCommonID == selectedID;

                    dailyAttendance = DailyAttendanceRepo.FindBy(rbDepartment);
                    if (dailyAttendance.Count > 0)
                        Criteria = "Common OU: " + dailyAttendance.First().Employee.OrganizationalUnit.OUCommon.OUCommonName;
                    break;
                case "rbSection":
                    selectedID = Convert.ToInt32(SectionList);
                    CriteriaRBName = "rbSection";
                    Expression<Func<DailyAttendance, bool>> rbSection = aa => aa.AttDate == _AttDataTo && aa.Employee.OUID == selectedID;

                    dailyAttendance = DailyAttendanceRepo.FindBy(rbSection);
                    if (dailyAttendance.Count > 0)
                        Criteria = "Organizational Unit: " + dailyAttendance.First().Employee.OrganizationalUnit.OUName;
                    break;
            }
            vm = GetAttendanceAttributesDateWise(dailyAttendance, _AttDataTo, Criteria, selectedID);
            vm.CriteriaRBName = CriteriaRBName;
            return vm;
        }

        public AttEditSingleEmployee GetAttendanceAttributes(List<DailyAttendance> dailyAttendance, DateTime dtFrom, DateTime dtTo, int empID)
        {
            AttEditSingleEmployee entries = new AttEditSingleEmployee();
            Expression<Func<DailyAttendance, bool>> SpecificEntries = c => c.EmpID == empID && c.AttDate >= dtFrom && c.AttDate <= dtTo;
            dailyAttendance = DailyAttendanceRepo.FindBy(SpecificEntries);
            if (dailyAttendance.Count() > 0)
            {
                entries.EmployeeID = (int)dailyAttendance.FirstOrDefault().EmpID;
                entries.EmpNo = dailyAttendance.FirstOrDefault().EmpNo;
                entries.EmpName = dailyAttendance.FirstOrDefault().Employee.EmployeeName;
                entries.DateFrom = dtFrom.ToString("dd-MMM-yyyy");
                entries.DateTo = dtTo.ToString("dd-MMM-yyyy");
                entries.OUName = dailyAttendance.FirstOrDefault().Employee.OrganizationalUnit.OUName;
                entries.JobTitleName = dailyAttendance.FirstOrDefault().Employee.JobTitle.JobTitleName;
                List<EditAttendanceListDateWise> list = new List<EditAttendanceListDateWise>();
                int i = 1;
                foreach (var item in dailyAttendance.OrderBy(aa => aa.AttDate))
                {
                    EditAttendanceListDateWise eal = new EditAttendanceListDateWise();
                    eal.EmployeeID = (int)item.EmpID;
                    eal.EmpNo = item.EmpNo;
                    eal.No = i;
                    i++;
                    eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                    eal.DutyTime = item.DutyTime.Value.Hours.ToString("00") + item.DutyTime.Value.Minutes.ToString("00");
                    eal.EmpDate = item.EmpDate;
                    eal.DutyCode = item.DutyCode;
                    if (item.Remarks == null)
                        eal.SystemRemarks = "";
                    else
                        eal.SystemRemarks = item.Remarks;
                    TimeSpan shiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                    eal.ShiftTime = shiftTime.Hours.ToString("00") + shiftTime.Minutes.ToString("00");
                    if (item.TimeIn != null)
                        eal.TimeIn = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                    else
                        eal.TimeIn = "";
                    if (item.TimeOut != null)
                        eal.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                    else
                        eal.TimeOut = "";
                    if (item.WorkMin > 0)
                    {
                        TimeSpan WorkTime = new TimeSpan(0, (int)item.WorkMin, 0);
                        eal.WorkMinutes = WorkTime.Hours.ToString("00") + ":" + WorkTime.Minutes.ToString("00");
                    }
                    else
                        eal.WorkMinutes = "";
                    if (item.DutyCode == "G")
                    {
                        string OT = "";
                        if (item.GZOTMin > 0)
                        {
                            TimeSpan GZTime = new TimeSpan(0, (int)item.GZOTMin, 0);
                            OT = GZTime.Hours.ToString("00") + ":" + GZTime.Minutes.ToString("00");
                        }
                        if (item.ExtraMin > 0)
                        {
                            TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                            OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                        }
                        eal.OTMin = OT;
                    }
                    else
                    {
                        string OT = "";
                        if (item.OTMin > 0)
                        {

                            TimeSpan OTTime = new TimeSpan(0, (int)item.OTMin, 0);
                            OT = OTTime.Hours.ToString("00") + ":" + OTTime.Minutes.ToString("00");
                        }
                        if (item.ExtraMin > 0)
                        {
                            TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                            OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                        }
                        eal.OTMin = OT;
                    }
                    if (item.ApprovedOT > 0)
                    {
                        TimeSpan ot = new TimeSpan(0, (int)item.ApprovedOT, 0);
                        eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                    }
                    if (item.ApprovedDoubleOT > 0)
                    {
                        TimeSpan ot = new TimeSpan(0, (int)item.ApprovedDoubleOT, 0);
                        eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                    }
                    if (item.ApprovedCPL > 0)
                    {
                        TimeSpan ot = new TimeSpan(0, (int)item.ApprovedCPL, 0);
                        eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                    }
                    list.Add(eal);
                }
                entries.list = list;
            }
            return entries;
        }

        public VMEditAttendanceDateWise GetAttendanceAttributesDateWise(List<DailyAttendance> dailyAttendance, DateTime dtTo, string Criteria, int CriteriaData)
        {
            VMEditAttendanceDateWise entries = new VMEditAttendanceDateWise();
            List<EditAttendanceListDateWise> list = new List<EditAttendanceListDateWise>();
            int i = 1;
            foreach (var item in dailyAttendance)
            {
                EditAttendanceListDateWise eal = new EditAttendanceListDateWise();
                eal.EmployeeID = (int)item.EmpID;
                eal.No = i;
                i++;
                eal.EmpNo = item.EmpNo;
                eal.EmpName = item.Employee.EmployeeName;
                eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                eal.DutyTime = item.DutyTime.Value.Hours.ToString("00") + item.DutyTime.Value.Minutes.ToString("00");
                eal.EmpDate = item.EmpDate;
                eal.DutyCode = item.DutyCode;
                eal.SystemRemarks = item.Remarks;
                TimeSpan shiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                eal.ShiftTime = shiftTime.Hours.ToString("00") + shiftTime.Minutes.ToString("00");
                if (item.TimeIn != null)
                    eal.TimeIn = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                if (item.TimeOut != null)
                    eal.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                if (item.WorkMin > 0)
                {
                    TimeSpan WorkTime = new TimeSpan(0, (int)item.WorkMin, 0);
                    eal.WorkMinutes = WorkTime.Hours.ToString("00") + ":" + WorkTime.Minutes.ToString("00");
                }
                else
                    eal.WorkMinutes = "0000";
                if (item.DutyCode == "G")
                {
                    string OT = "";
                    if (item.GZOTMin > 0)
                    {
                        TimeSpan GZTime = new TimeSpan(0, (int)item.GZOTMin, 0);
                        OT = GZTime.Hours.ToString("00") + ":" + GZTime.Minutes.ToString("00");
                    }
                    if (item.ExtraMin > 0)
                    {
                        TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                        OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                    }
                    eal.OTMin = OT;
                }
                else
                {
                    string OT = "";
                    if (item.OTMin > 0)
                    {
                        TimeSpan OTTime = new TimeSpan(0, (int)item.OTMin, 0);
                        OT = OTTime.Hours.ToString("00") + ":" + OTTime.Minutes.ToString("00");
                    }
                    if (item.ExtraMin > 0)
                    {
                        TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                        OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                    }
                    eal.OTMin = OT;
                }
                if (item.ApprovedOT > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedOT, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }
                if (item.ApprovedDoubleOT > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedDoubleOT, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }
                if (item.ApprovedCPL > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedCPL, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }

                list.Add(eal);
            }
            entries.list = list.OrderBy(aa => aa.EmpName).ToList();
            entries.Count = list.Count;
            entries.Criteria = Criteria;
            entries.CriteriaData = CriteriaData;
            entries.Date = dtTo.ToString("dd-MMM-yyyy");
            return entries;
        }
        private VMEditAttendanceDateWise GetAttendanceAttributesDateWise(List<VAT_DailyAttendance> dailyAttendance, DateTime dtTo, string Criteria, int CriteriaData)
        {
            VMEditAttendanceDateWise entries = new VMEditAttendanceDateWise();
            List<EditAttendanceListDateWise> list = new List<EditAttendanceListDateWise>();
            int i = 1;
            foreach (var item in dailyAttendance)
            {
                EditAttendanceListDateWise eal = new EditAttendanceListDateWise();
                eal.EmployeeID = (int)item.EmpID;
                eal.No = i;
                i++;
                eal.EmpNo = item.OEmpID;
                eal.EmpName = item.EmployeeName;
                eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                eal.DutyTime = item.DutyTime.Value.Hours.ToString("00") + item.DutyTime.Value.Minutes.ToString("00");
                eal.EmpDate = item.EmpDate;
                eal.DutyCode = item.DutyCode;
                eal.SystemRemarks = item.Remarks;
                TimeSpan shiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                eal.ShiftTime = shiftTime.Hours.ToString("00") + shiftTime.Minutes.ToString("00");
                if (item.TimeIn != null)
                    eal.TimeIn = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                if (item.TimeOut != null)
                    eal.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                if (item.WorkMin > 0)
                {
                    TimeSpan WorkTime = new TimeSpan(0, (int)item.WorkMin, 0);
                    eal.WorkMinutes = WorkTime.Hours.ToString("00") + ":" + WorkTime.Minutes.ToString("00");
                }
                else
                    eal.WorkMinutes = "0000";
                if (item.DutyCode == "G")
                {
                    string OT = "";
                    if (item.GZOTMin > 0)
                    {
                        TimeSpan GZTime = new TimeSpan(0, (int)item.GZOTMin, 0);
                        OT = GZTime.Hours.ToString("00") + ":" + GZTime.Minutes.ToString("00");
                    }
                    if (item.ExtraMin > 0)
                    {
                        TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                        OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                    }
                    eal.OTMin = OT;
                }
                else
                {
                    string OT = "";
                    if (item.OTMin > 0)
                    {
                        TimeSpan OTTime = new TimeSpan(0, (int)item.OTMin, 0);
                        OT = OTTime.Hours.ToString("00") + ":" + OTTime.Minutes.ToString("00");
                    }
                    if (item.ExtraMin > 0)
                    {
                        TimeSpan ExtraMin = new TimeSpan(0, (int)item.ExtraMin, 0);
                        OT = ExtraMin.Hours.ToString("00") + ":" + ExtraMin.Minutes.ToString("00");
                    }
                    eal.OTMin = OT;
                }
                if (item.ApprovedOT > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedOT, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }
                if (item.ApprovedDoubleOT > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedDoubleOT, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }
                if (item.ApprovedCPL > 0)
                {
                    TimeSpan ot = new TimeSpan(0, (int)item.ApprovedCPL, 0);
                    eal.ApprovedOTMin = ot.Hours.ToString("00") + ot.Minutes.ToString("00");
                }

                list.Add(eal);
            }
            entries.list = list.OrderBy(aa => aa.EmpName).ToList();
            entries.Count = list.Count;
            entries.Criteria = Criteria;
            entries.CriteriaData = CriteriaData;
            entries.Date = dtTo.ToString("dd-MMM-yyyy");
            return entries;
        }

        public EditAttendanceList GetEditAttendanceList(string empDate, string DutyCode, string DutyTime, string ShiftTime, string TimeIn, string TimeOut, string Remarks)
        {
            EditAttendanceList el = new EditAttendanceList();
            el.EmpDate = empDate;
            el.DutyCode = DutyCode;
            el.DutyTime = ConvertTime(DutyTime);
            el.ShiftTime = ConvertTime(ShiftTime);
            if (TimeIn != "")
                el.TimeIn = ConvertTime(TimeIn);
            if (TimeOut != "")
                el.TimeOut = ConvertTime(TimeOut);
            el.Remarks = Remarks;
            return el;
        }

        public List<DailyAttendance> GetEmployeeAttendance(int empID, DateTime dtFrom, DateTime dtTo)
        {
            List<DailyAttendance> dailyAttendance = new List<DailyAttendance>();
            Expression<Func<DailyAttendance, bool>> SpecificEntries = aa => aa.EmpID == empID && aa.AttDate >= dtFrom && aa.AttDate <= dtTo;
            dailyAttendance = DailyAttendanceRepo.FindBy(SpecificEntries);
            return dailyAttendance;
        }

        #region -- Process Manual Entry --
        DailyAttendance _OldAttData = new DailyAttendance();
        DailyAttendance _NewAttData = new DailyAttendance();
        DailyAttendanceEdit _ManualEditData = new DailyAttendanceEdit();

        //Replace New TimeIn and Out with Old TimeIN and Out in Attendance Data
        public void ManualAttendanceProcess(string EmpDate, string JobCardName, bool JobCardStatus, DateTime NewTimeIn, DateTime NewTimeOut, string NewDutyCode, int _UserID, TimeSpan _NewDutyTime, string _Remarks, short _ShiftMins)
        {
            Expression<Func<DailyAttendance, bool>> SpecificEntries = aa => aa.EmpDate == EmpDate;

            _OldAttData = DailyAttendanceRepo.FindBy(SpecificEntries).First();
            if (_OldAttData != null)
            {
                if (JobCardStatus == false)
                {
                    SaveOldAttData(_OldAttData, _UserID);
                    if (SaveNewAttData(NewTimeIn, NewTimeOut, NewDutyCode, _NewDutyTime, _Remarks, _ShiftMins))
                    {
                        _OldAttData.TimeIn = NewTimeIn;
                        _OldAttData.TimeOut = NewTimeOut;
                        _OldAttData.Tin0 = NewTimeIn;
                        _OldAttData.Tout0 = NewTimeOut;
                        _OldAttData.DutyCode = NewDutyCode;
                        _OldAttData.DutyTime = _NewDutyTime;
                        _OldAttData.WorkMin = 0;
                        _OldAttData.LateIn = 0;
                        _OldAttData.LateOut = 0;
                        _OldAttData.EarlyIn = 0;
                        _OldAttData.EarlyOut = 0;
                        _OldAttData.OTMin = 0;
                        _OldAttData.GZOTMin = 0;
                        _OldAttData.ExtraMin = 0;
                        _OldAttData.TotalShortMin = 0;

                        _OldAttData.Tin0 = null;
                        _OldAttData.Tout0 = null;
                        _OldAttData.Tin1 = null;
                        _OldAttData.Tout1 = null;
                        _OldAttData.Tin2 = null;
                        _OldAttData.Tout2 = null;
                        _OldAttData.Tin3 = null;
                        _OldAttData.Tout3 = null;
                        _OldAttData.Tin4 = null;
                        _OldAttData.Tout4 = null;
                        _OldAttData.Tin5 = null;
                        _OldAttData.Tout5 = null;
                        _OldAttData.Tin6 = null;
                        _OldAttData.Tout6 = null;
                        _OldAttData.Tin6 = null;
                        _OldAttData.Tout6 = null;
                        _OldAttData.Tin7 = null;
                        _OldAttData.Tout7 = null;
                        _OldAttData.Tin8 = null;
                        _OldAttData.Tout8 = null;
                        _OldAttData.Tin9 = null;
                        _OldAttData.Tout9 = null;
                        _OldAttData.Tin10 = null;
                        _OldAttData.Tout10 = null;
                        _OldAttData.Tin11 = null;
                        _OldAttData.Tout11 = null;
                        _OldAttData.Tin12 = null;
                        _OldAttData.Tout2 = null;
                        _OldAttData.Tin13 = null;
                        _OldAttData.Tout13 = null;
                        _OldAttData.Tin14 = null;
                        _OldAttData.Tout14 = null;
                        _OldAttData.Tin15 = null;
                        _OldAttData.Tout15 = null;
                        _OldAttData.DutyCode = NewDutyCode;
                        _OldAttData.DutyTime = _NewDutyTime;
                        _OldAttData.ShifMin = _ShiftMins;
                        switch (_OldAttData.DutyCode)
                        {
                            case "D":
                                _OldAttData.StatusAB = true;
                                if (_OldAttData.StatusHL == true)
                                {
                                    _OldAttData.AbDays = 0.5;
                                    _OldAttData.PDays = 0;
                                    _OldAttData.LeaveDays = 0.5;
                                    _OldAttData.DutyCode = "L";
                                }
                                else
                                {
                                    _OldAttData.AbDays = 1;
                                    _OldAttData.PDays = 0;
                                    _OldAttData.LeaveDays = 0;
                                }
                                _OldAttData.StatusP = false;
                                _OldAttData.StatusMN = true;
                                _OldAttData.StatusDO = false;
                                _OldAttData.StatusGZ = false;
                                _OldAttData.StatusLeave = false;
                                _OldAttData.StatusOT = false;
                                _OldAttData.OTMin = null;
                                _OldAttData.EarlyIn = null;
                                _OldAttData.EarlyOut = null;
                                _OldAttData.LateIn = null;
                                _OldAttData.LateOut = null;
                                _OldAttData.WorkMin = null;
                                _OldAttData.GZOTMin = null;
                                break;
                            case "G":
                                _OldAttData.StatusAB = false;
                                _OldAttData.StatusP = false;
                                _OldAttData.StatusMN = true;
                                _OldAttData.StatusDO = false;
                                _OldAttData.StatusGZ = true;
                                _OldAttData.StatusLeave = false;
                                _OldAttData.StatusOT = false;
                                _OldAttData.OTMin = null;
                                _OldAttData.EarlyIn = null;
                                _OldAttData.EarlyOut = null;
                                _OldAttData.LateIn = null;
                                _OldAttData.LateOut = null;
                                _OldAttData.WorkMin = null;
                                _OldAttData.GZOTMin = null;
                                _OldAttData.PDays = 0;
                                _OldAttData.AbDays = 0;
                                _OldAttData.LeaveDays = 0;
                                break;
                            case "R":
                                _OldAttData.StatusAB = false;
                                _OldAttData.StatusP = false;
                                _OldAttData.StatusMN = true;
                                _OldAttData.StatusDO = true;
                                _OldAttData.StatusGZ = false;
                                _OldAttData.StatusLeave = false;
                                _OldAttData.StatusOT = false;
                                _OldAttData.OTMin = null;
                                _OldAttData.EarlyIn = null;
                                _OldAttData.EarlyOut = null;
                                _OldAttData.LateIn = null;
                                _OldAttData.LateOut = null;
                                _OldAttData.WorkMin = null;
                                _OldAttData.GZOTMin = null;
                                _OldAttData.PDays = 0;
                                _OldAttData.AbDays = 0;
                                _OldAttData.LeaveDays = 0;
                                break;
                        }
                        ProcessDailyAttendance(_OldAttData, _Remarks);
                    }
                }
            }
        }

        //Save Old and New Attendance Data in Manual Attendance Table
        private bool SaveNewAttData(DateTime _NewTimeIn, DateTime _NewTimeOut, string _NewDutyCode, TimeSpan _NewDutyTime, string _remarks, short _ShiftMins)
        {
            bool check = false;
            _ManualEditData.NewTimeIn = _NewTimeIn;
            _ManualEditData.NewTimeOut = _NewTimeOut;
            _ManualEditData.NewDutyCode = _NewDutyCode;
            _ManualEditData.NewDutyTime = _NewDutyTime;
            _ManualEditData.EditDateTime = DateTime.Now;
            _ManualEditData.NewDutyTime = _NewDutyTime;
            _ManualEditData.NewRemarks = "[" + _remarks + "]";
            _ManualEditData.NewShiftMin = _ShiftMins;
            try
            {
                DailyAttendanceEditRepo.Add(_ManualEditData);
                DailyAttendanceEditRepo.Save();
                check = true;
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        private void SaveOldAttData(DailyAttendance _OldAttData, int Userid)
        {
            try
            {
                _ManualEditData.OldDutyCode = _OldAttData.DutyCode;
                _ManualEditData.OldTimeIn = _OldAttData.TimeIn;
                _ManualEditData.OldTimeOut = _OldAttData.TimeOut;
                _ManualEditData.OldDutyTime = _OldAttData.DutyTime;
                _ManualEditData.OldOTMin = _OldAttData.ApprovedOT;
                _ManualEditData.EmpDate = _OldAttData.EmpDate;
                _ManualEditData.UserID = Userid;
                _ManualEditData.EditDateTime = DateTime.Now;
                _ManualEditData.EmpID = _OldAttData.EmpID;
                _ManualEditData.OldRemarks = _OldAttData.Remarks;
            }
            catch (Exception ex)
            {

            }
        }

        //Work Times calculation controller
        private void ProcessDailyAttendance(DailyAttendance _attData, string Remarks)
        {
            try
            {
                DailyAttendance attendanceRecord = _attData;
                Employee employee = attendanceRecord.Employee;
                List<ShiftChangedEmp> _shiftEmpCh = new List<ShiftChangedEmp>();
                Expression<Func<ShiftChangedEmp, bool>> SpecificEntries = aa => aa.EmpID == _attData.EmpID;
                _shiftEmpCh = ShiftChangedEmpRepo.FindBy(SpecificEntries);
                List<Shift> shifts = new List<Shift>();
                Expression<Func<Shift, bool>> SpecificEntries2 = aa => aa.PShiftID == _attData.Employee.ShiftID;
                shifts = ShiftRepo.FindBy(SpecificEntries2);
                List<ShiftChanged> cshifts = new List<ShiftChanged>();
                Expression<Func<ShiftChanged, bool>> SpecificEntries3 = aa => aa.ShiftID == _attData.Employee.ShiftID;
                cshifts = ShiftChangedRepo.FindBy(SpecificEntries3);
                //Employee employee = _attData.Employee;
                if (_attData.StatusLeave == true)
                    _attData.ShifMin = 0;
                //If TimeIn and TimeOut are not null, then calculate other Atributes
                if (_attData.TimeIn != null && _attData.TimeOut != null)
                {
                    Shift _shift = ATAssistant.GetEmployeeChangedShift(_attData.Employee, _shiftEmpCh.Where(aa => aa.EmpID == _attData.EmpID).ToList(), _attData.AttDate.Value, shifts);
                    MyShift shift = ATAssistant.GetEmployeeShift(_shift);
                    if (_attData.StatusHL == true)
                    {
                        _attData.ShifMin = ATAssistant.CalculateShiftMinutes(shift, _attData.AttDate.Value.DayOfWeek);
                        _attData.ShifMin = (short)(_attData.ShifMin / 2);
                    }
                    //If TimeIn = TimeOut then calculate according to DutyCode
                    if (_attData.TimeIn == _attData.TimeOut)
                    {
                        CalculateInEqualToOut(_attData);
                    }
                    else
                    {
                        if (_attData.DutyTime == new TimeSpan(0, 0, 0))
                        {
                            ATWorkMinCalculator.CalculateOpenShiftTimes(_attData, shift, _attData.Employee.OTPolicy);
                        }
                        ATWorkMinCalculator.CalculateShiftTimes(_attData, shift, _attData.Employee.OTPolicy);
                    }
                }
                else
                {
                    CalculateInEqualToOut(_attData);
                }
            }
            catch (Exception ex)
            {
            }
            if (Remarks != "")
                _attData.Remarks = _attData.Remarks + "[" + Remarks + "]";
            DailyAttendanceRepo.Edit(_attData);
            DailyAttendanceRepo.Save();
        }

        TimeSpan OpenShiftThresholdStart = new TimeSpan(17, 00, 00);
        TimeSpan OpenShiftThresholdEnd = new TimeSpan(11, 00, 00);

        private void CalculateInEqualToOut(DailyAttendance attendanceRecord)
        {
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[DO]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[GZ]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[G-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[R-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Manual]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[M]", "");
            switch (attendanceRecord.DutyCode)
            {
                case "G":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = true;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.OTMin = 0;
                    attendanceRecord.PDays = 0;
                    attendanceRecord.AbDays = 0;
                    attendanceRecord.LeaveDays = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[GZ][M]";
                    break;
                case "R":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = false;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.OTMin = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.StatusDO = true;
                    attendanceRecord.PDays = 0;
                    attendanceRecord.AbDays = 0;
                    attendanceRecord.LeaveDays = 0;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[DO][M]";
                    break;
                case "D":
                    if (attendanceRecord.StatusLeave == true)
                    {
                        attendanceRecord.AbDays = 0;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 1;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.OTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[M]";
                    }
                    else if (attendanceRecord.StatusHL == true)
                    {
                        attendanceRecord.AbDays = 0.5;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 0.5;
                        attendanceRecord.StatusHL = true;
                        attendanceRecord.StatusAB = true;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.OTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[HA][M]";
                    }
                    else
                    {
                        attendanceRecord.AbDays = 1;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 0;
                        attendanceRecord.StatusAB = true;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.OTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[Absent][M]";
                    }
                    break;
            }
        }

        public VMEditAttendanceDateWise GetTMDashboardAttendance(DateTime date, string Criteria, List<VAT_DailyAttendance> dailyAttendance)
        {
            VMEditAttendanceDateWise vm = new VMEditAttendanceDateWise();
            int selectedID = 0;
            string CriteriaRBName = "";
            if (Criteria == "Absent")
                vm = GetAttendanceAttributesDateWise(dailyAttendance.Where(aa => aa.AbDays > 0).ToList(), date, Criteria, selectedID);
            if (Criteria == "Missing")
                vm = GetAttendanceAttributesDateWise(dailyAttendance.Where(aa => (aa.TimeIn != null && aa.TimeOut == null) || (aa.TimeIn == null && aa.TimeOut != null)).ToList(), date, Criteria, selectedID);
            if (Criteria == "LateIn")
                vm = GetAttendanceAttributesDateWise(dailyAttendance.Where(aa => aa.LateIn > 240).ToList(), date, Criteria, selectedID);
            if (Criteria == "EarlyOut")
                vm = GetAttendanceAttributesDateWise(dailyAttendance.Where(aa => aa.EarlyOut > 240).ToList(), date, Criteria, selectedID);
            vm.CriteriaRBName = CriteriaRBName;
            return vm;
        }
        #endregion
    }
}
