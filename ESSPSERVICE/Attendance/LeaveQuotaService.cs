using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Attendance;
using ESSPSERVICE.Generic;
using ESSPREPO.Generic;
using System.Linq.Expressions;
using ESSPCORE.Common;
using ESSPCORE.EF;

namespace ESSPSERVICE.Attendance
{
    class LeaveQuotaService : ILeaveQuotaService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<LeaveQuotaYear> LeaveQuotaYearReporsitory;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        public LeaveQuotaService(IDDService dDService, IRepository<LeaveQuotaYear> leaveQuotaYearReporsitory, IEmpSelectionService empSelectionService,
        IGetSpecificEmployeeService getSpecificEmployeeService)
        {
            DDService = dDService;
            LeaveQuotaYearReporsitory = leaveQuotaYearReporsitory;
            EmpSelectionService = empSelectionService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
        }
        public List<VMLeaveQuotaChild> GetIndex(int FinancialYearID, VMLoggedUser LoggedInUser)
        {
            List<VMLeaveQuotaChild> vmLeaveQuotaChildList = new List<VMLeaveQuotaChild>();
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo(LoggedInUser); // Get All Employees from database
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = c => c.FinancialYearID == FinancialYearID;
            List<LeaveQuotaYear> leaveQuotaYearList = LeaveQuotaYearReporsitory.FindBy(SpecificEntries);// Get all Leave quota Balances for specific financial year
            foreach (var item in employees)
            {
                if (leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID).Count() > 0)
                {
                    VMLeaveQuotaChild vmLeaveQuotaChild = new VMLeaveQuotaChild();
                    vmLeaveQuotaChild.EmpID = item.PEmployeeID;
                    vmLeaveQuotaChild.EmpNo = item.OEmpID;
                    vmLeaveQuotaChild.EmployeeName = item.EmployeeName;
                    vmLeaveQuotaChild.AL = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 1).ToList());
                    vmLeaveQuotaChild.CL = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 2).ToList());
                    vmLeaveQuotaChild.SL = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 3).ToList());
                    vmLeaveQuotaChild.CPL = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 4).ToList());
                    vmLeaveQuotaChild.EAL = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 11).ToList());
                    vmLeaveQuotaChild.CME = GetRemainingLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 12).ToList());
                    vmLeaveQuotaChild.ACCL = GetRemainingACCLeaveBalance(leaveQuotaYearList.Where(aa => aa.EmployeeID == item.PEmployeeID && aa.LeaveTypeID == 1).ToList());
                    vmLeaveQuotaChild.JobTitleName = item.JobTitleName;
                    vmLeaveQuotaChildList.Add(vmLeaveQuotaChild);
                }
            }
            return vmLeaveQuotaChildList;
        }

        private float GetRemainingACCLeaveBalance(List<LeaveQuotaYear> list)
        {
            if (list.Count > 0 && list.First().CFRemaining != null)
                return (float)list.First().CFRemaining;
            else
                return 0;
        }

        private float GetRemainingLeaveBalance(List<LeaveQuotaYear> list)
        {
            if (list.Count > 0)
                return (float)list.First().GrandRemaining;
            else
                return 0;
        }

        public VMLeaveQuotaSelection GetCreate1()
        {
            VMLeaveQuotaSelection obj = new VMLeaveQuotaSelection();
            return obj;
        }
        public VMLeaveQuotaSelection GetCreate2(VMLeaveQuotaSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser)
        {
            VMEmpSelection vmEmpSelection = EmpSelectionService.GetStepTwo(SelectedCompanyIds,
                                            SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
                                            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
                                            SelectedCrewIds, SelectedShiftIds, es.EmpNo, LoggedInUser);
            es.Criteria = vmEmpSelection.Criteria;
            es.CriteriaName = vmEmpSelection.CriteriaName;
            es.EmpNo = vmEmpSelection.EmpNo;
            es.Employee = vmEmpSelection.Employee;
            es.FinancialYearName = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == es.FinancialYearID).First().FYName;
            return es;
        }

        public VMLeaveQuota GetCreate3(VMLeaveQuotaSelection es, int?[] SelectedEmployeeIds, VMLeaveQuota vmLeaveQuota)
        {
            List<VMLeaveQuotaChild> vmLeaveQuotaChildList = new List<VMLeaveQuotaChild>();
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo(); // Get All Employees from database
            List<LeavePolicy> leavePolicyList = DDService.GetLeavePolicy(); // Get all leave policies
            FinancialYear financialYear = DDService.GetFinancialYear().First(aa => aa.PFinancialYearID == es.FinancialYearID); // Get selected financial year
            List<FinancialYear> financialYearCloseList = DDService.GetFinancialYear().Where(aa => aa.FYStatus == false).OrderByDescending(aa => aa.FYEndDate).ToList();
            List<LeaveQuotaYear> leaveQuotaPreviousYearList = new List<LeaveQuotaYear>();
            // Get Old Financial Year
            if (financialYearCloseList.Count > 0)
            {
                int LastFinYearID = financialYearCloseList.FirstOrDefault().PFinancialYearID;
                Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = c => c.FinancialYearID == LastFinYearID && c.LeaveTypeID == 1;
                leaveQuotaPreviousYearList = LeaveQuotaYearReporsitory.FindBy(SpecificEntries);// Get all Leave quota Balances for specific financial year
            }
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries2 = c => c.FinancialYearID == es.FinancialYearID;
            List<LeaveQuotaYear> leaveQuotaYearList = LeaveQuotaYearReporsitory.FindBy(SpecificEntries2);// Get all Leave quota Balances for specific financial year
            foreach (int empid in SelectedEmployeeIds)
            {
                if (leaveQuotaYearList.Where(aa => aa.EmployeeID == empid).Count() == 0)
                {
                    VMLeaveQuotaChild vmLeaveQuotaChild = new VMLeaveQuotaChild();
                    VHR_EmployeeProfile employee = employees.First(aa => aa.PEmployeeID == empid);// Get Specific Employee
                    float ALDays = GetTotalDays(leavePolicyList.Where(aa => aa.PLeavePolicyID == employee.ALPolicyID).ToList(), employee, financialYear);
                    float CLDays = GetTotalDays(leavePolicyList.Where(aa => aa.PLeavePolicyID == employee.CLPolicyID).ToList(), employee, financialYear);
                    float SLDays = GetTotalDays(leavePolicyList.Where(aa => aa.PLeavePolicyID == employee.SLPolicyID).ToList(), employee, financialYear);
                    float EALDays = GetTotalDays(leavePolicyList.Where(aa => aa.PLeavePolicyID == employee.EALPolicyID).ToList(), employee, financialYear);
                    float CMEDays = GetTotalDays(leavePolicyList.Where(aa => aa.PLeavePolicyID == employee.CMEPolicyID).ToList(), employee, financialYear);
                    vmLeaveQuotaChild.EmpID = employee.PEmployeeID;
                    vmLeaveQuotaChild.EmpNo = employee.OEmpID;
                    vmLeaveQuotaChild.EmployeeName = employee.EmployeeName;
                    vmLeaveQuotaChild.JobTitleName = employee.JobTitleName;
                    vmLeaveQuotaChild.FinancialYearID = financialYear.PFinancialYearID;
                    vmLeaveQuotaChild.FinancialYearName = financialYear.FYName;
                    if (employee.DOJ != null)
                        vmLeaveQuotaChild.DOJ = employee.DOJ.Value.ToString("dd-MMM-yyyy");
                    vmLeaveQuotaChild.AL = ALDays;
                    vmLeaveQuotaChild.CL = CLDays;
                    vmLeaveQuotaChild.SL = SLDays;
                    vmLeaveQuotaChild.EAL = EALDays;
                    vmLeaveQuotaChild.CME = CMEDays;
                    // Get LeaveBalance for Previous Year
                    if (leaveQuotaPreviousYearList.Where(aa => aa.EmployeeID == employee.PEmployeeID).Count() > 0)
                    {
                        vmLeaveQuotaChild.CollapseLeave = 0;
                        vmLeaveQuotaChild.ALBalance = GetALLeaveBalance(leaveQuotaPreviousYearList.Where(aa => aa.EmployeeID == employee.PEmployeeID).ToList());
                        vmLeaveQuotaChild.CarryForward = vmLeaveQuotaChild.ALBalance - vmLeaveQuotaChild.CollapseLeave;
                    }
                    else
                    {
                        vmLeaveQuotaChild.CarryForward = 0;
                        vmLeaveQuotaChild.CollapseLeave = 0;
                        vmLeaveQuotaChild.ALBalance = 0;
                    }
                    vmLeaveQuotaChildList.Add(vmLeaveQuotaChild);
                }
            }
            vmLeaveQuota.FinancialYearID = financialYear.PFinancialYearID;
            vmLeaveQuota.FinancialYearName = financialYear.FYName;
            vmLeaveQuota.LeaveQuotaChild = vmLeaveQuotaChildList;
            return vmLeaveQuota;
        }

        private float GetALLeaveBalance(List<LeaveQuotaYear> list)
        {
            if (list.Count > 0)
            {
                float bal = 0;
                if (list[0].GrandRemaining > 0)
                    bal = bal + (float)list[0].GrandRemaining;
                if (list[0].CFRemaining > 0)
                    bal = bal + (float)list[0].CFRemaining;

                return bal;
            }
            else
                return 0;
        }

        public VMLeaveQuota GetCreate4(VMLeaveQuota vm)
        {
            List<VMLeaveQuotaChild> LeaveQuotaChildCompleted = new List<VMLeaveQuotaChild>();
            vm.ErrorMessages = new List<string>();
            if (vm.LeaveQuotaChild.Count > 0)
            {
                foreach (var item in vm.LeaveQuotaChild)
                {
                    string errorMessage = "";
                    if (!SaveALLeaveQuotaYearEntry(item.AL, 1, item.EmpID, vm.FinancialYearID, item.CarryForward) == true) // Save AL in DB
                        errorMessage = errorMessage + "AL not Created";
                    if (!SaveLeaveQuotaYearEntry(item.CL, 2, item.EmpID, vm.FinancialYearID)) // Save CL in DB
                        errorMessage = errorMessage + "CL not Created";
                    if (!SaveLeaveQuotaYearEntry(item.SL, 3, item.EmpID, vm.FinancialYearID)) // Save SL in DB
                        errorMessage = errorMessage + "SL not Created";
                    if (!SaveLeaveQuotaYearEntry(item.EAL, 11, item.EmpID, vm.FinancialYearID)) // Save SL in DB
                        errorMessage = errorMessage + "EAL not Created";
                    if (!SaveLeaveQuotaYearEntry(item.CME, 12, item.EmpID, vm.FinancialYearID)) // Save SL in DB
                        errorMessage = errorMessage + "CME not Created";
                    if (errorMessage != "")
                        vm.ErrorMessages.Add("Employee No: " + item.EmpNo + " , " + errorMessage);
                    else
                        LeaveQuotaChildCompleted.Add(item);
                }
                foreach (var item in LeaveQuotaChildCompleted)
                {
                    vm.LeaveQuotaChild.Remove(item);
                }
                return vm;
            }
            else
                return vm;
        }

        private bool SaveALLeaveQuotaYearEntry(float TotalLV, int LeaveTypeID, int EmpID, int FinancialYearID, float CF)
        {
            try
            {
                LeaveQuotaYear leaveQuotaYear = new LeaveQuotaYear();
                leaveQuotaYear.EmployeeID = EmpID;
                leaveQuotaYear.CFFromLastYear = 0;
                leaveQuotaYear.FinancialYearID = FinancialYearID;
                leaveQuotaYear.GrandRemaining = TotalLV;
                leaveQuotaYear.GrandTotal = TotalLV;
                leaveQuotaYear.LeaveTypeID = (byte)LeaveTypeID;
                leaveQuotaYear.YearlyRemaining = TotalLV;
                leaveQuotaYear.YearlyTotal = TotalLV;
                leaveQuotaYear.CFRemaining = CF;
                leaveQuotaYear.CFFromLastYear = CF;
                LeaveQuotaYearReporsitory.Add(leaveQuotaYear);
                LeaveQuotaYearReporsitory.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool SaveLeaveQuotaYearEntry(float TotalLV, int LeaveTypeID, int EmpID, int FinancialYearID)
        {
            try
            {
                LeaveQuotaYear leaveQuotaYear = new LeaveQuotaYear();
                leaveQuotaYear.EmployeeID = EmpID;
                leaveQuotaYear.CFFromLastYear = 0;
                leaveQuotaYear.FinancialYearID = FinancialYearID;
                leaveQuotaYear.GrandRemaining = TotalLV;
                leaveQuotaYear.GrandTotal = TotalLV;
                leaveQuotaYear.LeaveTypeID = (byte)LeaveTypeID;
                leaveQuotaYear.YearlyRemaining = TotalLV;
                leaveQuotaYear.YearlyTotal = TotalLV;
                LeaveQuotaYearReporsitory.Add(leaveQuotaYear);
                LeaveQuotaYearReporsitory.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private float GetTotalDays(List<LeavePolicy> leavePolicies, VHR_EmployeeProfile employee, FinancialYear financialYear)
        {
            if (leavePolicies.Count == 0)
                return 0;
            else
            {
                LeavePolicy leavePolicy = leavePolicies.First();
                if (leavePolicy.ProRata == true)
                {
                    // get months between 
                    if (employee.DOJ.Value > financialYear.FYStartDate)
                    {
                        float months = 0;
                        float leaves = 0;

                        // Get month of first Salary
                        if (employee.DOJ.Value.Month == financialYear.FYStartDate.Value.Month)
                        {
                            months = 12;
                        }
                        else
                        {

                            int FirstSalaryMonth = employee.DOJ.Value.Month;
                            if (employee.DOJ.Value.Year == financialYear.FYStartDate.Value.Year)
                            {
                                months = 13 - FirstSalaryMonth + 6;
                            }
                            else
                            {
                                months = 7 - FirstSalaryMonth;
                            }
                        }
                        leaves = (months * leavePolicy.TotalDays.Value) / 12;
                        return (int)(leaves);
                    }
                    else
                        return ((leavePolicy.TotalDays == null) ? 0 : (float)leavePolicy.TotalDays);
                }
                else
                    return ((leavePolicy.TotalDays == null) ? 0 : (float)leavePolicy.TotalDays);
            }
        }

    }
}
