using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Interface for the approval of overtime approval.
    /// </summary>
    /// <remarks></remarks>
    class OvertimeApprovalService : IOvertimeApprovalService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<MonthData> MonthDataReporsitory;
        IRepository<OTApprovedHistory> OTApprovedHistoryReporsitory;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        IRepository<LeaveCPLEmpBalance> LeaveCPLBalanceRepository;
        IRepository<LeaveQuotaYear> LeaveQuotaYearRepository;
        public OvertimeApprovalService(IDDService dDService, IRepository<MonthData> monthDataReporsitory, IEmpSelectionService empSelectionService,
            IGetSpecificEmployeeService getSpecificEmployeeService, IRepository<OTApprovedHistory> oTApprovedHistoryReporsitory,
            IRepository<LeaveCPLEmpBalance> leaveCPLBalanceRepository, IRepository<LeaveQuotaYear> leaveQuotaYearRepository)
        {
            DDService = dDService;
            MonthDataReporsitory = monthDataReporsitory;
            EmpSelectionService = empSelectionService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
            OTApprovedHistoryReporsitory = oTApprovedHistoryReporsitory;
            LeaveCPLBalanceRepository = leaveCPLBalanceRepository;
            LeaveQuotaYearRepository = leaveQuotaYearRepository;
        }
        public List<VMOvertimeApprovalChild> GetIndex(VMLoggedUser user, int PayrollPeriodID)
        {
            List<VHR_EmployeeProfile> dbEmployees = GetSpecificEmployeeService.GetSpecificEmployees(user);
            List<MonthData> monthDatas = new List<MonthData>();
            if (user.UserRoleID == "U") // HR Normal
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == PayrollPeriodID && c.MonthDataStageID == "P" && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                monthDatas = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            else if (user.UserRoleID == "H") // HR Admin
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == PayrollPeriodID && c.MonthDataStageID == "A" && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                monthDatas = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            else if (user.UserRoleID == "A") // Admin
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == PayrollPeriodID && c.MonthDataStageID == "P" && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                monthDatas = MonthDataReporsitory.FindBy(SpecificEntries);
            }

            List<VMOvertimeApprovalChild> VMOvertimeApprovalChildList = new List<VMOvertimeApprovalChild>();
            PayrollPeriod payrollPeriod = DDService.GetAllPayrollPeriod().Where(aa => aa.PPayrollPeriodID == PayrollPeriodID).First();
            foreach (var dbEmployee in dbEmployees)
            {
                VMOvertimeApprovalChild vmOvertimeApprovalChild = new VMOvertimeApprovalChild();
                if (monthDatas.Where(aa => aa.EmployeeID == dbEmployee.PEmployeeID).Count() > 0)
                {
                    vmOvertimeApprovalChild = GetConveretedOTList(vmOvertimeApprovalChild, dbEmployee, monthDatas.First(aa => aa.EmployeeID == dbEmployee.PEmployeeID), payrollPeriod);
                    VMOvertimeApprovalChildList.Add(vmOvertimeApprovalChild);
                }
            }
            return VMOvertimeApprovalChildList;
        }
        public VMOvertimeApprovalSelection GetCreate1()
        {
            VMOvertimeApprovalSelection obj = new VMOvertimeApprovalSelection();
            return obj;
        }
        public VMOvertimeApprovalSelection GetCreate2(VMOvertimeApprovalSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser)
        {
            List<MonthData> dbMontData = new List<MonthData>();
            VMEmpSelection vmEmpSelection = EmpSelectionService.GetStepTwo(SelectedCompanyIds,
                                            SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
                                            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
                                            SelectedCrewIds, SelectedShiftIds, es.EmpNo, LoggedInUser);
            es.Criteria = vmEmpSelection.Criteria;
            es.CriteriaName = vmEmpSelection.CriteriaName;
            es.EmpNo = vmEmpSelection.EmpNo;
            if (LoggedInUser.UserRoleID == "U")// HR Normal
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == es.PayrollPeriodID && (c.MonthDataStageID == "P" || c.MonthDataStageID == null) && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                dbMontData = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            else if (LoggedInUser.UserRoleID == "H")// HR Admin
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == es.PayrollPeriodID && (c.MonthDataStageID == "H") && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                dbMontData = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            else if (LoggedInUser.UserRoleID == "A")// Admin
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == es.PayrollPeriodID && (c.MonthDataStageID == "P") && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                dbMontData = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            else
            {
                Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == es.PayrollPeriodID && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
                dbMontData = MonthDataReporsitory.FindBy(SpecificEntries);
            }
            List<VHR_EmployeeProfile> dbEmployees = new List<VHR_EmployeeProfile>();
            foreach (var item in vmEmpSelection.Employee)
            {
                if (dbMontData.Where(aa => aa.EmployeeID == item.PEmployeeID).Count() > 0)
                    dbEmployees.Add(item);
            }
            es.Employee = dbEmployees;
            es.PayrollPeriodName = DDService.GetPayrollPeriod().Where(aa => aa.PPayrollPeriodID == es.PayrollPeriodID).First().PRName;
            return es;
        }

        public VMOvertimeApproval GetCreate3(VMOvertimeApprovalSelection es, int?[] SelectedEmployeeIds, VMOvertimeApproval vmOvertimeApproval)
        {
            List<MonthData> monthDatas = new List<MonthData>();
            List<OTPolicy> dbOTPolicies = DDService.GetOTPolicy();
            List<VMOvertimeApprovalChild> vmOvertimeApprovalChildList = new List<VMOvertimeApprovalChild>();
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo();
            PayrollPeriod payrollPeriod = DDService.GetPayrollPeriod().First(aa => aa.PPayrollPeriodID == es.PayrollPeriodID); // Get selected Payroll Period
            Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == payrollPeriod.PPayrollPeriodID && (c.EncashbaleSingleOT > 0 || c.EncashbaleDoubleOT > 0 || c.CPLConversionOT > 0);
            monthDatas = MonthDataReporsitory.FindBy(SpecificEntries);
            foreach (int empid in SelectedEmployeeIds)
            {
                if (monthDatas.Where(aa => aa.EmployeeID == empid).Count() > 0)
                {
                    MonthData monthData = new MonthData();
                    monthData = monthDatas.First(aa => aa.EmployeeID == empid);
                    VMOvertimeApprovalChild vmOvertimeApprovalChild = new VMOvertimeApprovalChild();
                    VHR_EmployeeProfile employee = employees.First(aa => aa.PEmployeeID == empid);// Get Specific Employee
                    vmOvertimeApprovalChild = GetConveretedOTList(vmOvertimeApprovalChild, employee, monthData, payrollPeriod);
                    vmOvertimeApprovalChildList.Add(vmOvertimeApprovalChild);
                }
            }
            vmOvertimeApproval.PayrollPeriodID = payrollPeriod.PPayrollPeriodID;
            vmOvertimeApproval.PayrollPeriodName = payrollPeriod.PRName;
            vmOvertimeApproval.OvertimeApprovalChild = vmOvertimeApprovalChildList;
            return vmOvertimeApproval;
        }
        public VMOvertimeApprovalChild GetConveretedOTList(VMOvertimeApprovalChild vmOvertimeApprovalChild, VHR_EmployeeProfile employee, MonthData monthData, PayrollPeriod payrollPeriod)
        {
            vmOvertimeApprovalChild.EmpID = employee.PEmployeeID;
            vmOvertimeApprovalChild.EmpNo = employee.OEmpID;
            vmOvertimeApprovalChild.EmployeeName = employee.EmployeeName;
            vmOvertimeApprovalChild.OvertimePolicyID = employee.OTPolicyID;
            vmOvertimeApprovalChild.OvertimePolicyName = employee.OTPolicyName;
            vmOvertimeApprovalChild.EncashableSingleOT = ATAssistant.GetTimeHours(monthData.EncashbaleSingleOT);
            vmOvertimeApprovalChild.EncashableDoubleOT = ATAssistant.GetTimeHours(monthData.EncashbaleDoubleOT);
            vmOvertimeApprovalChild.CPLConvertedOT = ATAssistant.GetTimeHours(monthData.CPLConversionOT);
            vmOvertimeApprovalChild.NormalOT = ATAssistant.GetTimeHours(monthData.TNOT);
            vmOvertimeApprovalChild.RestOT = ATAssistant.GetTimeHours(monthData.TROT);
            vmOvertimeApprovalChild.GZOT = ATAssistant.GetTimeHours(monthData.TGZOT);
            vmOvertimeApprovalChild.CPLConvertedOTDays = monthData.CPLConvertedDays;
            vmOvertimeApprovalChild.PayrollPeriodID = payrollPeriod.PPayrollPeriodID;
            vmOvertimeApprovalChild.PayrollPeriodName = payrollPeriod.PRName;
            vmOvertimeApprovalChild.StatusID = monthData.MonthDataStageID;
            return vmOvertimeApprovalChild;
        }
        public VMOvertimeApproval GetCreate4(VMOvertimeApproval vm, VMLoggedUser LoggedInUser)
        {
            PayrollPeriod payrollPeriod = DDService.GetPayrollPeriod().First(aa => aa.PPayrollPeriodID == vm.PayrollPeriodID); // Get selected Payroll Period
            Expression<Func<MonthData, bool>> SpecificEntries = c => c.PayrollPeriodID == payrollPeriod.PPayrollPeriodID;
            List<MonthData> monthDatas = MonthDataReporsitory.FindBy(SpecificEntries);
            // Disable EF Validations
            OTApprovedHistoryReporsitory.ToggleEFValidations(false);
            MonthDataReporsitory.ToggleEFValidations(false);
            LeaveCPLBalanceRepository.ToggleEFValidations(false);
            LeaveQuotaYearRepository.ToggleEFValidations(false);
            foreach (var otChild in vm.OvertimeApprovalChild)
            {
                try
                {
                    if (monthDatas.Where(aa => aa.EmployeeID == otChild.EmpID).Count() > 0)
                    {
                        MonthData monthData = new MonthData();
                        monthData = monthDatas.First(aa => aa.EmployeeID == otChild.EmpID);
                        // Save History
                        OTApprovedHistory dbOTApprovedHistory = new OTApprovedHistory();
                        dbOTApprovedHistory.CreatedDateTime = DateTime.Now;
                        dbOTApprovedHistory.EmpID = monthData.EmployeeID;
                        dbOTApprovedHistory.MonthDataID = (int)monthData.PMonthDataID;
                        dbOTApprovedHistory.NewCPLConverted = (short)(otChild.CPLConvertedOT * 60);
                        dbOTApprovedHistory.NewSingleOT = (short)(otChild.EncashableSingleOT * 60);
                        dbOTApprovedHistory.NewDoubleOT = (short)(otChild.EncashableDoubleOT * 60);
                        dbOTApprovedHistory.OldCPLConverted = monthData.CPLConversionOT;
                        dbOTApprovedHistory.OldSingleOT = monthData.EncashbaleSingleOT;
                        dbOTApprovedHistory.OldDoubleOT = monthData.EncashbaleDoubleOT;
                        dbOTApprovedHistory.SubmittedByID = LoggedInUser.PUserID;
                        dbOTApprovedHistory.SubmittedToID = vm.SubmittedToUserID;
                        if (vm.OTStatusID == "A")
                        {
                            dbOTApprovedHistory.SubmittedToID = null;
                        }
                        dbOTApprovedHistory.OTStageID = vm.OTStatusID;
                        OTApprovedHistoryReporsitory.Add(dbOTApprovedHistory);
                        OTApprovedHistoryReporsitory.Save();
                        // Update Monthly Attendance
                        monthData.CPLConversionOT = (short)(otChild.CPLConvertedOT * 60);
                        monthData.EncashbaleSingleOT = (short)(otChild.EncashableSingleOT * 60);
                        monthData.EncashbaleDoubleOT = (short)(otChild.EncashableDoubleOT * 60);
                        monthData.CPLConvertedDays = GetCPLDaysFromHours(otChild.CPLConvertedOT);
                        if (LoggedInUser.UserRoleID == "H")// HR Admin
                        {
                            if (vm.OTStatusID == "A")
                            {
                                monthData.MonthDataStageID = "A";
                                monthData.SubmittedToUserID = null;
                                monthData.SubmittedByUserID = LoggedInUser.PUserID;
                            }
                            else
                            {
                                monthData.SubmittedToUserID = vm.SubmittedToUserID;
                                monthData.SubmittedByUserID = LoggedInUser.PUserID;
                                monthData.MonthDataStageID = "H";
                            }
                        }
                        else if (LoggedInUser.UserRoleID == "U")// HR Normal
                        {
                            monthData.SubmittedToUserID = vm.SubmittedToUserID;
                            monthData.SubmittedByUserID = LoggedInUser.PUserID;
                            monthData.MonthDataStageID = "H";
                        }
                        MonthDataReporsitory.Edit(monthData);
                        MonthDataReporsitory.Save();

                        if (LoggedInUser.UserRoleID == "H")// HR Admin
                        {
                            if (vm.OTStatusID == "A")
                            {
                                //// Update Days in LeaveCPLBalance table
                                //LeaveCPLEmpBalance dbLeaveCPLBalance = new LeaveCPLEmpBalance();
                                //Expression<Func<LeaveCPLEmpBalance, bool>> SpecificEntries2 = c => c.PayrollPeriodID == payrollPeriod.PPayrollPeriodID && c.EmployeeID == monthData.EmployeeID;
                                //if (LeaveCPLBalanceRepository.FindBy(SpecificEntries2).Count > 0)
                                //{
                                //    dbLeaveCPLBalance = LeaveCPLBalanceRepository.FindBy(SpecificEntries2).First();
                                //    dbLeaveCPLBalance.CPLBalance = monthData.CPLConvertedDays;
                                //    dbLeaveCPLBalance.RemainingBalance = dbLeaveCPLBalance.CPLBalance;
                                //    dbLeaveCPLBalance.Used = 0;
                                //    LeaveCPLBalanceRepository.Edit(dbLeaveCPLBalance);
                                //}
                                //else
                                //{
                                //    dbLeaveCPLBalance.CPLBalance = monthData.CPLConvertedDays;
                                //    dbLeaveCPLBalance.EmployeeID = monthData.EmployeeID;
                                //    dbLeaveCPLBalance.ExpireDate = payrollPeriod.PREndDate.Value.AddDays(60);
                                //    dbLeaveCPLBalance.IsExpire = false;
                                //    dbLeaveCPLBalance.PayrollPeriodID = payrollPeriod.PPayrollPeriodID;
                                //    dbLeaveCPLBalance.RemainingBalance = dbLeaveCPLBalance.CPLBalance;
                                //    dbLeaveCPLBalance.StartDate = payrollPeriod.PREndDate.Value.AddDays(1);
                                //    dbLeaveCPLBalance.Used = 0;
                                //    LeaveCPLBalanceRepository.Add(dbLeaveCPLBalance);
                                //}
                                //LeaveCPLBalanceRepository.Save();

                                //if (monthData.IsCPLAdded == null || monthData.IsCPLAdded == false)
                                //{
                                //    // Update Days in Leave Quota
                                //    LeaveQuotaYear dbLeaveQuotaYear = new LeaveQuotaYear();
                                //    Expression<Func<LeaveQuotaYear, bool>> SpecificEntries3 = c => c.FinancialYearID == payrollPeriod.FinancialYearID && c.EmployeeID == monthData.EmployeeID && c.LeaveTypeID == 4;
                                //    if (LeaveQuotaYearRepository.FindBy(SpecificEntries3).Count > 0)
                                //    {
                                //        dbLeaveQuotaYear = LeaveQuotaYearRepository.FindBy(SpecificEntries3).First();
                                //        dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + monthData.CPLConvertedDays;
                                //        LeaveQuotaYearRepository.Edit(dbLeaveQuotaYear);
                                //    }
                                //    else
                                //    {
                                //        dbLeaveQuotaYear.EmployeeID = monthData.EmployeeID;
                                //        dbLeaveQuotaYear.FinancialYearID = payrollPeriod.FinancialYearID;
                                //        dbLeaveQuotaYear.LeaveTypeID = 4;
                                //        dbLeaveQuotaYear.GrandTotal = 0;
                                //        dbLeaveQuotaYear.GrandRemaining = 0;
                                //        dbLeaveQuotaYear.YearlyTotal = 0;
                                //        dbLeaveQuotaYear.YearlyRemaining = 0;
                                //        dbLeaveQuotaYear.GrandTotal = dbLeaveQuotaYear.GrandTotal + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.GrandRemaining = dbLeaveQuotaYear.GrandRemaining + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.YearlyTotal = dbLeaveQuotaYear.YearlyTotal + monthData.CPLConvertedDays;
                                //        dbLeaveQuotaYear.YearlyRemaining = dbLeaveQuotaYear.YearlyRemaining + monthData.CPLConvertedDays;
                                //        LeaveQuotaYearRepository.Add(dbLeaveQuotaYear);
                                //    }
                                //    LeaveQuotaYearRepository.Save();
                                //}
                                // update monthly data
                                monthData.IsCPLAdded = true;
                                MonthDataReporsitory.Edit(monthData);
                                MonthDataReporsitory.Save();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            // Enable Validation
            // Disable EF Validations
            OTApprovedHistoryReporsitory.ToggleEFValidations(true);
            MonthDataReporsitory.ToggleEFValidations(true);
            LeaveCPLBalanceRepository.ToggleEFValidations(true);
            LeaveQuotaYearRepository.ToggleEFValidations(true);
            return new VMOvertimeApproval();
        }

        private double? GetCPLDaysFromHours(int cPLConvertedOT)
        {
            double cplDays = 0;
            if (cPLConvertedOT > 0)
            {
                int RoundOffHour = cPLConvertedOT / 8;
                double Balance = cPLConvertedOT - RoundOffHour * 8;
                if (Balance < 2)
                    cplDays = RoundOffHour;
                else if (Balance > 2 && Balance < 6)
                    cplDays = RoundOffHour + 0.5;
                else
                    cplDays = RoundOffHour + 1.0;
            }
            return cplDays;
        }
    }
}


