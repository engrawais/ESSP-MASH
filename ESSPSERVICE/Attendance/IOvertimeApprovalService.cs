using ESSPCORE.Attendance;
using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Interface for Overtime approval
    /// </summary>
    /// <remarks></remarks>
    public interface IOvertimeApprovalService
    {
        //VMLeaveQuotaSelection GetEdit(int id);
        //VMLeaveQuota GetDelete(int id);
        /// <summary>
        /// Gets the list of all the overtime of all the employee to faisal's users.
        /// </summary>
        /// <param name="user">Logged in user that would be HR Admin</param>
        /// <param name="Prid">Payroll period id of which overtimes are submitted to faisal.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VMOvertimeApprovalChild> GetIndex(VMLoggedUser user, int Prid);
        /// <summary>
        /// Gets the List of all the locations,OU,Employee type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters to get employee location wise or single.</returns>
        /// <remarks></remarks>
        VMOvertimeApprovalSelection GetCreate1();
        /// <summary>
        ///  This method gets the information of selected filter
        /// </summary>
        /// <param name="es"> Gets the information of Selected employee for overtime encashment. </param>
        /// <param name="SelectedOUCommonIds">Parameter of Common OU</param>
        /// <param name="SelectedOUIds">parameter of OU </param>
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
        VMOvertimeApprovalSelection GetCreate2(VMOvertimeApprovalSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
                   int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
                   int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        VMOvertimeApproval GetCreate3(VMOvertimeApprovalSelection es, int?[] SelectedEmployeeIds, VMOvertimeApproval vm);
        /// <summary>
        /// Updates the monthly attendance in the month data table 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMOvertimeApproval GetCreate4(VMOvertimeApproval vm, VMLoggedUser LoggedInUser);

    }
}
