using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Interface for the creation attendance leaves
    /// </summary>
    /// <remarks></remarks>
    public interface ILeaveApplicationService
    {
        /// <summary>
        /// This method checks the duplication of leaves for the specific date.
        /// </summary>
        /// <param name="lvappl">parameter for the specific leave application to get the Primary ID of leave application. </param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool CheckDuplicateLeave(LeaveApplication lvappl);
        /// <summary>
        /// Checks the leave balance of specific leave type and updates the balance.
        /// </summary>
        /// <param name="_lvapp">Object of Leave Application to get the id of specific leave whose balance is to be checked. </param>
        /// <param name="LeaveType">object of Leave Type </param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool CheckLeaveBalance(LeaveApplication _lvapp, LeavePolicy LeaveType);
        /// <summary>
        /// Add leave to Att Data table to reprocess for the implementation in the Reports.
        /// </summary>
        /// <param name="lvappl">Object of leave Application for adding a specific leave to
        /// Att data.</param>
        /// <param name="lvType">Object of leave type.</param>
        bool AddLeaveToAttData(LeaveApplication lvappl, LeaveType lvType);
        /// <summary>
        /// Gets the leave Balance of all the leave types that are in the database
        /// </summary>
        /// <param name="lvappl">Object of leave application for specific leave </param>
        /// <param name="LeaveType">Object of leave type</param>
        /// <param name="PayrollPeriodID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool BalanceLeaves(LeaveApplication lvappl, LeaveType LeaveType, int PayrollPeriodID);
        /// <summary>
        /// Deletes the leave from leave data 
        /// </summary>
        /// <param name="lvappl"> Object of specific leave </param>
        /// <remarks></remarks>
        void DeleteFromLVData(LeaveApplication lvappl);
        /// <summary>
        /// Updates the leave Balances after the deletion of leave
        /// </summary>
        /// <param name="lvappl"> Object of specific leave</param>
        /// <param name="PayrollPeriodID"></param>
        /// <remarks></remarks>
        void UpdateLeaveBalance(LeaveApplication lvappl, int PayrollPeriodID);
        /// <summary>
        /// Delete half leave from leave data 
        /// </summary>
        /// <param name="lvappl">object of specific leave </param>
        /// <remarks></remarks>
        void DeleteHLFromLVData(LeaveApplication lvappl);
        /// <summary>
        /// Checks whether the employee has leave quota or not
        /// </summary>
        /// <param name="empID">Get the employeee id whose leave quota is to be checked</param>
        /// <param name="leaveType">Gets the leave type </param>
        /// <param name="FinYearID">Financial year to check the leave quota of that specific financial year.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool HasLeaveQuota(int empID, LeavePolicy leaveType, int FinYearID);
        /// <summary>
        /// Gets the data for the deleteion of leave
        /// </summary>
        /// <param name="id">PLeaveApp ID of the specific leave.</param>
        /// <returns>returns a modal for the message upon the deletion.</returns>
        /// <remarks></remarks>
        LeaveApplication GetDelete(int id);
        /// <summary>
        /// Delete the leave entry from the leave application table
        /// </summary>
        /// <param name="lvApp">get the id and ionformation of specific leave application.</param>
        /// <remarks></remarks>
        void PostDelete(LeaveApplication lvApp);
        /// <summary>
        /// Calculate the number of days accorfding to the leave policy whether the gazetted days are included or not. 
        /// </summary>
        /// <param name="lvapplication">parameter for specific leave </param>
        /// <param name="lvType">parameter for the leave type</param>
        /// <param name="lvPolicy">parameter for the leave policy whether to calculate the gazette days or not according to the leave Policy. </param>
        /// <returns></returns>
        /// <remarks></remarks>
        float CalculateNoOfDays(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy);
        /// <summary>
        /// Creates the leave in the database after checking all of the functions.
        /// </summary>
        /// <param name="lvapplication">Parameeter for the leave application</param>
        /// <param name="lvType">Parameter for the leave type</param>
        /// <param name="user">User id od the logged in user.</param>
        /// <param name="lvPolicy">Parameter that checks the leave policy.</param>
        /// <remarks></remarks>
        void CreateLeave(LeaveApplication lvapplication, LeaveType lvType, VMLoggedUser user, LeavePolicy lvPolicy);
        /// <summary>
        /// Calculates the calendar days for the specific leave.
        /// </summary>
        /// <param name="lvapplication">parameter for the leave application</param>
        /// <param name="lvType">parameter for leave type</param>
        /// <param name="lvPolicy">parameter for the leave policy</param>
        /// <returns></returns>
        /// <remarks></remarks>
        float CalculateCalenderDays(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy);
        /// <summary>
        /// Calculate the return date of the the employee.
        /// </summary>
        /// <param name="lvapplication"></param>
        /// <param name="lvType"></param>
        /// <param name="lvPolicy"></param>
        /// <returns>returns show the date of return of the employee</returns>
        /// <remarks>Checks the leave policy and then if gazetted days are not included in the leave or rest is there in the roster of the employee shows the return date after the rest day.</remarks>
        DateTime? GetReturnDate(LeaveApplication lvapplication, LeaveType lvType, LeavePolicy lvPolicy);
        /// <summary>
        /// Add Leave to Leave data table and check whether the rest days are included or not.
        /// </summary>
        /// <param name="lvappl">parameter for the leave application</param>
        /// <param name="lvType">parameter for leave type </param>
        /// <param name="lvPolicy">parameter for leave policy</param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool AddLeaveToLeaveData(LeaveApplication lvappl, LeaveType lvType, LeavePolicy lvPolicy);
        /// <summary>
        /// Checks for AL consective day.
        /// </summary>
        /// <param name="lvapplication">The lvapplication.</param>
        /// <returns>true, if user availed AL for previous date</returns>
        bool CheckForALConsectiveDay(LeaveApplication lvapplication);
    }
}
