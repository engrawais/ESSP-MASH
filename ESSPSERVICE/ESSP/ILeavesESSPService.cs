using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.ESSP
{
    /// <summary>
    /// Interface for ESSP leave Applications 
    /// </summary>
    /// <remarks>This interface contains all the methods like deletion of the leave but the policies and ither methods related to leave are being called through controller that hits the leave application service of attendance.  </remarks>
    public interface ILeavesESSPService
    {

        /// <summary>
        /// Gets the list of all the leave of employee
        /// </summary>
        /// <param name="LoggedInUser">Information of the user whose leaves are to be displayed</param>
        /// <returns>returns a list with stages of leave of employee</returns>
        /// <remarks></remarks>
        List<VAT_LeaveApplication> GetIndex(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Get List of all the pending leaves for the line manager
        /// </summary>
        /// <param name="LoggedInUser">Logged in user if he is employee or line manager.</param>
        /// <returns>List containing all the pending leave of hLine manger's employees.</returns>
        /// <remarks></remarks>
        List<VAT_LeaveApplication> GetPendingLeaveRequests(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Gets the list of all the leave of Line manager's employee that he approved or rejected.
        /// </summary>
        /// <param name="LoggedInUser">Get the Logged in user.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VAT_LeaveApplication> GetEmpLeaveHistory(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Show all the information of the Leave 
        /// </summary>
        /// <param name="id">Get the primary key of Specific leave that is to be edited.</param>
        /// <param name="LoggedInUser"></param>
        /// <returns>return a view containing inforamtion of the leaves. </returns>
        /// <remarks>This view specifically show all the information of the employee's leave his remarks leave id,from date,to date .</remarks>
        VMESSPLeaveDetails GetESSPLeaveEmpDetail(int? id, VMLoggedUser LoggedInUser);
        /// <summary>
        /// This method approves the leave of the employee 
        /// </summary>
        /// <param name="vmESSPCommon">Common function parameter for the comment and stage of the leave.</param>
        /// <param name="user">User whose leave is being approved</param>
        /// <remarks>This method save all the information of the leave, cut off all of his balance checks the leave balance and policy of the leave and implements the impact of the leave by showing the leave in the repors and daily attendance editors by reprocessing it .</remarks>
        string RecommendLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser user,string Message);
        /// <summary>
        /// This method rejects the leave of the employee.
        /// </summary>
        /// <param name="vmESSPCommon">Common function for the cooment and changing the stage of the leave.</param>
        /// <param name="user">Get the user of the employee whose leave is to be rejected.</param>
        /// <remarks>This method save all the information of the leave and dont have impact on the leave reports and just the leave is rejected with comment. </remarks>
        void RejectLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser user);
        /// <summary>
        /// This method get all the necessary for the leave application.
        /// </summary>
        /// <returns>returns a modal containing all the required fields.</returns>
        /// <remarks></remarks>
        LeaveApplication GetCreate();
        /// <summary>
        /// This method post all the information of the leave in leave Application table and leave datatable.
        /// </summary>
        /// <param name="lvapplication">parameter for leave creation</param>
        /// <param name="lvType">Get the leave type </param>
        /// <param name="user">User who is applying the leave .</param>
        /// <remarks></remarks>
        void CreateLeave(LeaveApplication lvapplication, LeaveType lvType, VMLoggedUser user);
        /// <summary>
        /// This method shows the modal and deletes the leave application.
        /// </summary>
        /// <param name="id">Primary key id of the leave that is to be deleted.</param>
        /// <returns>returns a modal containing the messge for confirmation</returns>
        /// <remarks></remarks>
        LeaveApplication GetDelete(int id);
        /// <summary>
        /// This method deletes the leave from Leave Application table and Leave data table 
        /// </summary>
        /// <param name="obj">Post Leave related information that is to be deleted.</param>
        /// <remarks></remarks>
        void PostDelete(LeaveApplication obj);
        void UpdatePathName(int? id, string filename);
        void RevertToLMLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser);
    }
}
