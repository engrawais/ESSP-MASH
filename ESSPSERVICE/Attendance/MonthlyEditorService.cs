using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.EF;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPSERVICE.Generic;
using ESSPREPO.Generic;
using System.Linq.Expressions;
using System.Drawing.Text;
using ESSPCORE.Attendance;

namespace ESSPSERVICE.Attendance
{
    public class MonthlyEditorService : IMonthlyEditorService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<MonthData> MonthlyDataRepository;
        public MonthlyEditorService(IDDService dDService, IEmpSelectionService empSelectionService, IRepository<MonthData> monthlyDataRepository)
        {
            DDService = dDService;
            EmpSelectionService = empSelectionService;
            MonthlyDataRepository = monthlyDataRepository;
        }

        public List<VAT_MonthlySummary> AttendanceDetails()
        {
            throw new NotImplementedException();
        }

        //public List<MonthData> AttendanceDetails()
        //{

        //}

        public VMEditMonthlyCreate GetCreate1()
        {
            VMEditMonthlyCreate obj = new VMEditMonthlyCreate();
            return obj;
        }
        public VMEditMonthlyCreate GetCreate2(VMEditMonthlyCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, 
            int?[] SelectedEmploymentTypeIds, int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, 
            int?[] SelectedDesignationIds, int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser)
        {
                VMEmpSelection vmEmpSelection = EmpSelectionService.GetStepTwo(SelectedCompanyIds,
                                                SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
                                                SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
                                                SelectedCrewIds, SelectedShiftIds, es.EmpNo,LoggedInUser);
                es.Criteria = vmEmpSelection.Criteria;
                es.CriteriaName = vmEmpSelection.CriteriaName;
                es.EmpNo = vmEmpSelection.EmpNo;
                es.Employee = vmEmpSelection.Employee.Where(aa=>aa.Status=="Active").ToList();
                return es;
            }


        public VMJobCardCreate GetIndex()
        {
            VMJobCardCreate obj = new VMJobCardCreate();
            return obj;
        }

        public VMEditMonthlyAttendance GetMonthlyAttendanceAttributes(List<VAT_MonthlySummary> MonthlyAttendance, int PayrollID)
        {
            VMEditMonthlyAttendance entries = new VMEditMonthlyAttendance();
            List<EditMonthlyAttendanceList> list = new List<EditMonthlyAttendanceList>();
            int count = 1;
            foreach (var item in MonthlyAttendance)
            {
                EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
                eal.EmployeeID = (int)item.EmployeeID;
                eal.No = count;
                count++;
                eal.EmpNo = item.OEmpID;
                eal.EmpName = item.EmployeeName;
                eal.JobTitleName = item.JobTitleName;
                eal.TotalDays = 0;
                eal.PaidDays = 0;
                eal.AbsentDays = 0;
                
                eal.RestDays = (float)item.RestDays;
                eal.LeaveDays = (float)item.LeaveDays;
                if (item.AbsentDays != null)
                {
                    eal.AbsentDays = (float)item.AbsentDays;
                }
                if (item.WorkDays != null)
                {
                    eal.PaidDays = (float)item.WorkDays;
                }
                if (item.TotalDays != null)
                {
                    eal.TotalDays = (float)item.TotalDays;
                }
                if (item.WOPLeavesDays != null)
                {
                    eal.LWOPDays = (float)item.WOPLeavesDays;
                }
                //if (item.TNOT != null)
                //{
                //    eal.NOT = ATAssistant.GetTimeHours(item.TNOT);
                //}
                if(item.EncashbaleSingleOT!=null)
                {
                    eal.SingleEncashableOT = ATAssistant.GetTimeHours(item.EncashbaleSingleOT);
                }
                //if (item.TROT != null)
                //{
                //    eal.ROT = ATAssistant.GetTimeHours(item.TROT);
                //}
                if (item.EncashbaleDoubleOT != null)
                {
                    eal.DoubleEncashableOT = ATAssistant.GetTimeHours(item.EncashbaleDoubleOT);
                }
                //if (item.TGZOT != null)
                //{
                //    eal.GOT = ATAssistant.GetTimeHours(item.TGZOT);
                //}
                if (item.CPLConversionOT != null)
                {
                    eal.CPLOT = ATAssistant.GetTimeHours(item.CPLConversionOT);
                }
                //if (item.TotalOT != null)
                //{
                //    eal.TotalOT = ATAssistant.GetTimeHours(item.TotalOT);
                //}
                if (item.Remarks == null)
                    eal.Remarks = "";
                else
                    eal.Remarks = item.Remarks;
                list.Add(eal);
            }
            //list.Add(GetTotalCount(list));
           // entries.Locations = locs;
            entries.MonthlyList = list.OrderBy(aa => aa.EmployeeID).ToList();
            entries.Count = list.Count;
            entries.PayrolPeriodID = PayrollID;
            //entries.PayrollName = PRName;
            return entries;
        }

        public EditMonthlyAttendanceList GetTotalCount(List<EditMonthlyAttendanceList> MonthlyAttendance)
        {
            EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
            eal.EmployeeID = 999999;
            eal.EmpNo = "";
            eal.EmpName = "Total: " + MonthlyAttendance.Count;
            eal.TotalDays = (float)MonthlyAttendance.ToList().Select(c => c.TotalDays).Sum();
            eal.PaidDays = (float)MonthlyAttendance.ToList().Select(c => c.PaidDays).Sum();
            eal.AbsentDays = (float)MonthlyAttendance.ToList().Select(c => c.AbsentDays).Sum();
            eal.LeaveDays = (float)MonthlyAttendance.ToList().Select(c => c.LeaveDays).Sum();
            return eal;
        }
        public EditMonthlyAttendanceList GetEditMonthlyAttendanceList(int EmpID, int prid, string TotalDays, string PaidDays, string AbsentDays, string Remarks)
        {
            EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
            if (TotalDays != null && TotalDays != "")
                eal.TotalDays = (float)Convert.ToDouble(TotalDays);
            else
                eal.TotalDays = 0;
            if (PaidDays != null && PaidDays != "")
                eal.PaidDays = (float)Convert.ToDouble(PaidDays);
            else
                eal.PaidDays = 0;

            if (AbsentDays != null && AbsentDays != "")
                eal.AbsentDays = (float)Convert.ToDouble(AbsentDays);
            else
                eal.AbsentDays = 0;
            if (Remarks != null && Remarks != "")
            {
                eal.Remarks = Remarks;
            }
            else
                eal.Remarks = "";
            return eal;
        }

        public string CheckMonthRecordIsEdited(MonthData att, EditMonthlyAttendanceList editlist)
        {
            if(att.WorkDays == null)
                att.WorkDays = 0;
            if (att.TotalDays == null)
                att.TotalDays = 0;
            if (att.AbsentDays == null)
                att.AbsentDays = 0;
            if (att.WOPLeavesDays == null)
                att.WOPLeavesDays = 0;
            if (att.TNOT == null)
                att.TNOT = 0;
            if (att.TROT == null)
                att.TROT = 0;
            if (att.TGZOT == null)
                att.TGZOT = 0;
            string edited = "No";
            if (editlist.PaidDays != att.WorkDays)
                edited = "Time";
            if (editlist.TotalDays != att.TotalDays)
                edited = "Time";
            if (editlist.AbsentDays != att.AbsentDays)
                edited = "Time";
          
            return edited;
        }
    }
}
