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
    /// Interface for overtime encashment
    /// </summary>
    /// <remarks></remarks>
    public interface IOvertimeEncashableService
    {
        //VMLeaveQuotaSelection GetEdit(int id);
        //VMLeaveQuota GetDelete(int id);
        /// <summary>
        /// Gets the List of all the locations,OU,employee type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters to get employee location wise or single.</returns>
        /// <remarks></remarks>
        VMOvertimeEncashableSelection GetCreate1();
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
        VMOvertimeEncashableSelection GetCreate2(VMOvertimeEncashableSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
                   int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
                   int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Shows the list containing overtime details liste OT minutes of selected employees.
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedEmployeeIds"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMOvertimeEncashable GetCreate3(VMOvertimeEncashableSelection es, int?[] SelectedEmployeeIds, VMOvertimeEncashable vm);
        /// <summary>
        /// Save the information
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMOvertimeEncashable GetCreate4(VMOvertimeEncashable vm);
    }
}
