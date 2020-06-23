using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Reports
{
    /// <summary>
    /// This is the interface of Reporting of ESSP
    /// </summary>
    /// <remarks>For some Reports of TMS services have been used to get the balance of the employee.</remarks>
    public interface IAttReportingService
    {
        /// <summary>
        /// Get the leave balance of employee given to him at the start of the financial year
        /// </summary>
        /// <param name="list">Get the employee information </param>
        /// <param name="FinYearID">Get financial year of which leave balance is to be displayed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VMLeaveBalance> GetYearlyLeaveBalance(List<VHR_EmployeeProfile> list, int FinYearID);
        /// <summary>
        /// This method gets the Leave consumed in the month giving the parameter of the dates .
        /// </summary>
        /// <param name="list"></param>
        /// <param name="FinYearID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VMMonthlyLeaveBalance> GetMonthlyLeaveBalance(List<VHR_EmployeeProfile> list, int FinYearID, DateTime StartDate, DateTime EndDate);
        /// <summary>
        /// This methods downloads the report directly in the excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="user"></param>
        /// <param name="StartDate">parameter for the starting of date</param>
        /// <param name="EndDate">parameter for the ending of the date</param>
        /// <returns></returns>
        /// <remarks>This report when opened in the report viewer for the whole location took too much time that why user can directly download the report just by clicking on the report.</remarks>
        string GetDailyReportInExcel(List<VHR_EmployeeProfile> list, VMLoggedUser user, DateTime StartDate, DateTime EndDate);
        /// <summary>
        /// Gets the converted daily att summary from daily attendance data.
        /// </summary>
        /// <param name="attdata">The daily attenance data list</param>
        /// <returns></returns>
        List<VMDailyAttSummary> GetConvertedDailyAttSummary(List<VAT_DailyAttendance> attdata);
        // List<VMEmployeeAttendanceOther> GetEmployeeAttendanceOther(List<VAT_DailyAttendanceOther> list);
    }
}
