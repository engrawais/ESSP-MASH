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
    /// Interface for ESSP JobCards
    /// </summary>
    /// <remarks>This interface defines all the methods from creation to deletion.</remarks>
    public interface IJobCardESSPService
    {
        /// <summary>
        /// This method gets the list of all the job card of specific employee in ESSP 
        /// </summary>
        /// <param name="LoggedInUser">Gets the specific employee</param>
        /// <returns>returns a list containing  job cards of the employee</returns>
        /// <remarks>All the jobcards either they are pending , Approved or Reject will be present on the list </remarks>
        List<VEP_JobCardApplication> GetIndex(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Get list of all the job cards that are pending on line manager's end for approval.
        /// </summary>
        /// <param name="LoggedInUser">Get the User id of the logged in user.</param>
        /// <returns>returns a list of job card of employees of line manager with status Pending.</returns>
        /// <remarks></remarks>
        List<VEP_JobCardApplication> GetPendingJobCardRequests(VMLoggedUser LoggedInUser);
        /// <summary>
        /// Get the List of all the Jobcards that are Approved or Rejected by the Line manager.
        /// </summary>
        /// <param name="LoggedInUser">Gets the Logged in user.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        List<VEP_JobCardApplication> GetEmpJobCardHistory(VMLoggedUser LoggedInUser);
        /// <summary>
        /// This Method shows the view containing all the necessary details of the Job card .
        /// </summary>
        /// <param name="id">Primary key of the jobcard whose detail in to be seen.</param>
        /// <param name="LoggedInUser">Gets the details of logged in user used in the job card.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMESSPJobCardDetail GetJobCardEmpDetail(int? id, VMLoggedUser LoggedInUser);
        /// <summary>
        /// This method is used to approve the job card.
        /// </summary>
        /// <param name="vmESSPCommon">Gets the Common details like comment and Stage ID on the Job card on rejection or approval</param>
        /// <param name="user">Get the User of whom Job Card it being approved or rejected.</param>
        /// <remarks>This method changes the Stage of the job card ,Save the Job card in Flow table,Generate the email and Reprocess the attendance of the date on which job card is applied.</remarks>
        string ApproveJobCard(VMESSPCommon vmESSPCommon, VMLoggedUser user,string Message);
        /// <summary>
        /// This method is used to rejects the job card.
        /// </summary>
        /// <param name="vmESSPCommon">Gets the Common details like comment and Stage ID on the Job card on rejection or approval</param>
        /// <param name="user">Get the User of whom Job Card it being approved or rejected.</param>
        /// <remarks>This method changes the Stage of the job card ,Save the Job card in Flow table and Generate the email  the attendance of the date on which job card is applied.</remarks>
        void RejectJobCard(VMESSPCommon vmESSPCommon, VMLoggedUser user);
        /// <summary>
        /// This method gets all the necessary information needed to create the job card.
        /// </summary>
        /// <returns>returns a modal with information.</returns>
        /// <remarks></remarks>
        void PostCreate(JobCardApp obj, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Post the information of single day job card 
        /// </summary>
        /// <param name="obj">post information of single day jobcard </param>
        /// <param name="LoggedInUser"></param>
        /// <remarks></remarks>
        void SingleDayPostCreate(JobCardApp obj, VMLoggedUser LoggedInUser);
        /// <summary>
        /// Method for the deletion of jobcard 
        /// </summary>
        /// <param name="id">Gets the id of specific job card that is to be deleted.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        JobCardApp GetDelete(int id);
        /// <summary>
        /// Delete the job card from the flow and jocard App table
        /// </summary>
        /// <param name="obj">Information of the job card</param>
        /// <remarks></remarks>
        void PostDelete(JobCardApp obj);
    }
}
