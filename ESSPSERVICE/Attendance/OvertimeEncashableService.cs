using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    class OvertimeEncashableService : IOvertimeEncashableService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<MonthData> MonthDataReporsitory;
        public OvertimeEncashableService(IDDService dDService, IRepository<MonthData> monthDataReporsitory, IEmpSelectionService empSelectionService)
        {
            DDService = dDService;
            MonthDataReporsitory = monthDataReporsitory;
            EmpSelectionService = empSelectionService;
        }

        public VMOvertimeEncashableSelection GetCreate1()
        {
            VMOvertimeEncashableSelection obj = new VMOvertimeEncashableSelection();
            return obj;
        }
        public VMOvertimeEncashableSelection GetCreate2(VMOvertimeEncashableSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser)
        {
            VMEmpSelection vmEmpSelection = EmpSelectionService.GetStepTwo(SelectedCompanyIds,
                                            SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
                                            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
                                            SelectedCrewIds, SelectedShiftIds, es.EmpNo,LoggedInUser);
            es.Criteria = vmEmpSelection.Criteria;
            es.CriteriaName = vmEmpSelection.CriteriaName;
            es.EmpNo = vmEmpSelection.EmpNo;
            es.Employee = vmEmpSelection.Employee;
            es.PayrollPeriodName = DDService.GetPayrollPeriod().Where(aa => aa.PPayrollPeriodID == es.PayrollPeriodID).First().PRName;
            return es;
        }

        public VMOvertimeEncashable GetCreate3(VMOvertimeEncashableSelection es, int?[] SelectedEmployeeIds, VMOvertimeEncashable vmOvertimeApproval)
        {
            List<VMOvertimeEncashableChild> vmOvertimeApprovalChildList = new List<VMOvertimeEncashableChild>();
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo(); // Get All Employees from database
            PayrollPeriod payrollPeriod = DDService.GetPayrollPeriod().First(aa => aa.PPayrollPeriodID == es.PayrollPeriodID); // Get selected Payroll Period
            foreach (int empid in SelectedEmployeeIds)
            {
                VMOvertimeEncashableChild vmOvertimeApprovalChild = new VMOvertimeEncashableChild();
                VHR_EmployeeProfile employee = employees.First(aa => aa.PEmployeeID == empid);// Get Specific Employee

                vmOvertimeApprovalChild.EmpID = employee.PEmployeeID;
                vmOvertimeApprovalChild.EmpNo = employee.OEmpID;
                vmOvertimeApprovalChild.EmployeeName = employee.EmployeeName;
                vmOvertimeApprovalChild.OvertimePolicyID = employee.OTPolicyID;
                vmOvertimeApprovalChild.OvertimePolicyName = employee.OTPolicyName;
                vmOvertimeApprovalChild.ApprovedOT = 80;
                vmOvertimeApprovalChild.EncashableOT = 56;
                vmOvertimeApprovalChild.CPLConvertedOT = 24;
                vmOvertimeApprovalChild.PayrollPeriodID = payrollPeriod.PPayrollPeriodID;
                vmOvertimeApprovalChild.PayrollPeriodName = payrollPeriod.PRName;
                vmOvertimeApprovalChildList.Add(vmOvertimeApprovalChild);

            }
            vmOvertimeApproval.PayrollPeriodID = payrollPeriod.PPayrollPeriodID;
            vmOvertimeApproval.PayrollPeriodName = payrollPeriod.PRName;
            vmOvertimeApproval.OvertimeEncashableChild = vmOvertimeApprovalChildList;
            return vmOvertimeApproval;
        }

        public VMOvertimeEncashable GetCreate4(VMOvertimeEncashable vm)
        {
            return new VMOvertimeEncashable();
        }

      
    }
}


