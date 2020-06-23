using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Generic
{
    public interface IGetSpecificEmployeeService
    {
        /// <summary>
        /// This method gets the information of the employee according to his access type i-e. 
        /// if the employee is given the rights of Specific location then all the location emloyees will be shown on his index.
        /// </summary>
        /// <param name="vmf">gets the information of the logged in User</param>
        /// <returns></returns>
        List<VHR_EmployeeProfile> GetSpecificEmployees(VMLoggedUser vmf);
        /// <summary>
        /// This method gets the attendance of the employees whose access is given to specific employee.i-e.
        /// if emloyee is given the access of specific location then he can se the attendance of the 
        /// location that he has the access and cannot make the modification in the attendance of employees of other locations.
        /// </summary>
        /// <param name="vmf">Gets the logged in User.</param>
        /// <param name="date">Gets the date of the attendance. </param>
        /// <returns></returns>
        List<VAT_DailyAttendance> GetSpecificAttendance(VMLoggedUser vmf,DateTime date);
        /// <summary>
        /// Gets the employee of only specific location and give access to user of that location to modify the overtime of that 
        /// specific employee.
        /// </summary>
        /// <param name="vmf">Gets the logged in user </param>
        /// <returns></returns>
        List<VAT_DailyOvertime> GetSpecificDailyOT(VMLoggedUser vmf);
        List<VAT_DailyAttendance> GetSpecificAbsentAttendance(VMLoggedUser vmf, DateTime dateFrom,DateTime dateTo);
    }
}
