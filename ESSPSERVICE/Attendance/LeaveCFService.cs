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
    /// Leave Carry forward Serivce 
    /// </summary>
    public class LeaveCFService : ILeaveCFService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<LeaveCarryForward> LeaveCarryForwardReporsitory;
        public LeaveCFService(IDDService dDService, IRepository<LeaveCarryForward> leaveCarryForwardReporsitory, IEmpSelectionService empSelectionService)
        {
            DDService = dDService;
            LeaveCarryForwardReporsitory = leaveCarryForwardReporsitory;
            EmpSelectionService = empSelectionService;
        }
        /// <summary>
        /// Gets the filters fo the creation of leave carry forward
        /// </summary>
        /// <returns></returns>
        public VMLeaveCFSelection GetCreate1()
        {
            VMLeaveCFSelection obj = new VMLeaveCFSelection();
            return obj;
        }      /// <summary>
               ///  This method gets the information of selected filter
               /// </summary>
               /// <param name="es"> Gets the information of Selected employee for Leave Carry Forward. </param>
               /// <param name="SelectedOUCommonIds">Parameter of Common OU</param>
               /// <param name="SelectedOUIds">Parameter of OU </param>
               /// <param name="SelectedEmploymentTypeIds">Selected Employee types</param>
               /// <param name="SelectedLocationIds">Selected Locations</param>
               /// <param name="SelectedGradeIds">Selected Grades </param>
               /// <param name="SelectedJobTitleIds">Selected Job titles</param>
               /// <param name="SelectedDesignationIds">Slected Designations</param>
               /// <param name="SelectedCrewIds">Selected Crews./param>
               /// <param name="SelectedShiftIds">Selected Shifts</param>
               /// <param name="LoggedInUser">Logged in User </param>
               /// <returns></returns>
               /// <remarks></remarks>
        public VMLeaveCFSelection GetCreate2(VMLeaveCFSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
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
        /// <summary>
        /// Shows the list of all Leave Carry Forward of selected employees.
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedEmployeeIds">Parameter of Selected Employee ids</param>
        /// <param name="vmLeaveCF"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public VMLeaveCF GetCreate3(VMLeaveCFSelection es, int?[] SelectedEmployeeIds, VMLeaveCF vmLeaveCF)
        {
            List<VMLeaveCFChild> vmLeaveCFChildList = new List<VMLeaveCFChild>();
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo(); // Get All Employees from database
            FinancialYear financialYear = DDService.GetFinancialYear().First(aa => aa.PFinancialYearID == es.FinancialYearID); // Get selected financial year
            foreach (int empid in SelectedEmployeeIds)
            {

                VMLeaveCFChild vmLeaveCFChild = new VMLeaveCFChild();
                VHR_EmployeeProfile employee = employees.First(aa => aa.PEmployeeID == empid);// Get Specific Employee

                vmLeaveCFChild.EmpID = employee.PEmployeeID;
                vmLeaveCFChild.EmpNo = employee.OEmpID;
                vmLeaveCFChild.EmployeeName = employee.EmployeeName;
                vmLeaveCFChild.FinancialYearID = financialYear.PFinancialYearID;
                vmLeaveCFChild.FinancialYearName = financialYear.FYName;
                vmLeaveCFChild.TotalLeave = 20;
                vmLeaveCFChild.LeaveTypeName = "AL";
                vmLeaveCFChild.CarryForward = 15;
                vmLeaveCFChild.CollapseLeave = 5;
                vmLeaveCFChildList.Add(vmLeaveCFChild);
            }
            vmLeaveCF.FinancialYearID = financialYear.PFinancialYearID;
            vmLeaveCF.FinancialYearName = financialYear.FYName;
            vmLeaveCF.LeaveCFChild = vmLeaveCFChildList;
            return vmLeaveCF;
        }

        /// <summary>
        /// This method save all the changes in leave Carry Forward if failed show message not created
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public VMLeaveCF GetCreate4(VMLeaveCF vm)
        {
            return new VMLeaveCF();
        }

    }
}
