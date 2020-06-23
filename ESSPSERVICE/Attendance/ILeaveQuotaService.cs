using ESSPCORE.Attendance;
using ESSPCORE.Common;
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
    /// Interface for Leave Quota
    /// </summary>
    /// <remarks></remarks>
    public interface ILeaveQuotaService
    {
        //VMLeaveQuotaSelection GetEdit(int id);
        //VMLeaveQuota GetDelete(int id);
        /// <summary>
        /// This method shows the list of all the leave quotas when they are selected from the filter
        /// </summary>
        /// <param name="FinancialYearID">get the leave quota of specific financial year</param>
        /// <param name="LoggedInUser">get logged in user who has access to specific location</param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VMLeaveQuotaChild> GetIndex(int FinancialYearID, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Gets the List of all the locations,OU,employee type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters to get employee location wise or single.</returns>
        /// <remarks></remarks>
        VMLeaveQuotaSelection GetCreate1();
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
        VMLeaveQuotaSelection GetCreate2(VMLeaveQuotaSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Shows the list of all leave quota of selected employees.
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedEmployeeIds">parameter of Selected Employee ids</param>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMLeaveQuota GetCreate3(VMLeaveQuotaSelection es, int?[] SelectedEmployeeIds, VMLeaveQuota vm);

        /// <summary>
        /// This method save all the changes in leave quota if failed show message not created
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMLeaveQuota GetCreate4(VMLeaveQuota vm);
    }
}
