using ESSPCORE.Attendance;
using ESSPCORE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    public interface ILeaveCFService
    {
        //VMLeaveQuotaSelection GetEdit(int id);
        //VMLeaveQuota GetDelete(int id);
        /// <summary>
        /// Gets the List of all the locations,OU,employee type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters to get employee location wise or single.</returns>
        /// <remarks></remarks>
        VMLeaveCFSelection GetCreate1();
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
        VMLeaveCFSelection GetCreate2(VMLeaveCFSelection es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Show selected employee information and their total leave carry forward leaves.
        /// </summary>
        /// <param name="es"></param>
        /// <param name="SelectedEmployeeIds"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMLeaveCF GetCreate3(VMLeaveCFSelection es, int?[] SelectedEmployeeIds, VMLeaveCF vm);
        /// <summary>
        /// Save the changes made in create 3.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMLeaveCF GetCreate4(VMLeaveCF vm);


    }
}
