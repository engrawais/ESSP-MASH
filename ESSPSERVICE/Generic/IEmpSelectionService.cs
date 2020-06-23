using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Generic
{
    public interface IEmpSelectionService
    {
        /// <summary>
        /// Gets the OU,Locations,Designations,Grade,Jobtitle,Crew and Shifts for the purpose of the filtering.
        /// </summary>
        /// <param name="LoggedInUser">Gets the logged in user</param>
        /// <returns></returns>

        VMEmpSelection GetStepOne(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Get the Information of the seleted filter and get the employee related to the selected filters 
        /// </summary>
        /// <param name="SelectedCompanyIds">Selected Common OU's if any </param>
        /// <param name="SelectedOUCommonIds">Selected Common OU's if any </param>
        /// <param name="SelectedOUIds">Selected OU's if any </param>
        /// <param name="SelectedEmploymentTypeIds">Selected Employement Types if any </param>
        /// <param name="SelectedLocationIds">Selected Locations if any </param>
        /// <param name="SelectedGradeIds">Selected Grades if any </param>
        /// <param name="SelectedJobTitleIds">Selected Job titles if any </param>
        /// <param name="SelectedDesignationIds">Selected Designations if any </param>
        /// <param name="SelectedCrewIds">Selected Crews if any </param>
        /// <param name="SelectedShiftIds">Selected Common OU's if any </param>
        /// <param name="EmpNo">Selected Common OU's if any </param>
        /// <param name="LoggedInUser">Gets the logged in the user</param>
        /// <returns></returns>
        VMEmpSelection GetStepTwo(int?[] SelectedCompanyIds,int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, string EmpNo, VMLoggedUser LoggedInUser);
    }
}
