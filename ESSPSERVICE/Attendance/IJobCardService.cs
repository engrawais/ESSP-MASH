using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
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
    /// Interface for Jobcards of  Attendance.
    /// </summary>
    /// <remarks>The methods of this interface defines the creation, Deletion and editing of Job cards.</remarks>
    public interface IJobCardService
    {
        /// <summary>
        /// This method get the list of all the job cards of attendance as well as approved job cards of Employee self service portal (ESSP).
        /// </summary>
        /// <param name="LoggedInUser">Parameter of logged in user with access to specific locations.</param>
        /// <returns>Returns the list containing all the job cards of Attendance and ESSP</returns>
        /// <remarks></remarks>
        List<VAT_JobCardApplication> GetIndex(VMLoggedUser LoggedInUser);
      
        /// <summary>
        /// Gets the List of all the locations,OU,Jobcard type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters for the creation of bulk job card.</returns>
        /// <remarks></remarks>
        VMJobCardCreate GetCreate1();
        /// <summary>
        ///  This method gets the information of selected filter
        /// </summary>
        /// <param name="es"> Gets the information of job card  and employee like Start date, End date, Job card type. </param>
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
        VMJobCardCreate GetCreate2(VMJobCardCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
                    int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
                    int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        /// <summary>
        /// This method gets the information of Bulk job card like Date of creation,Job card Minutes and remarks.
        /// </summary>
        /// <param name="es">object VMJobcard </param>
        /// <param name="employeeIds"></param>
        /// <param name="loggedUser"></param>
        /// <remarks></remarks>
        void PostCreate3(VMJobCardCreate es, int?[] employeeIds, VMLoggedUser loggedUser);
        /// <summary>
        /// This methods gets all the data for eiditing purpose 
        /// </summary>
        /// <param name="id"> parameter for job card Id </param>
        /// <returns>returns a modal containing information for editing.</returns>
        /// <remarks></remarks>
        VMJobCardCreate GetEdit(int id);

        /// <summary>
        /// This method post all the information that is data edited.
        /// </summary>
        /// <param name="obj"> post all the information of bulk job card like from date, to date </param>
        /// <returns>apply all the information on seleted employees,location, OU . </returns>
        /// <remarks></remarks>
        ServiceMessage PostEdit(VMJobCardCreate obj);
        /// <summary>
        /// This method post all the information at time of creation of jobcard.
        /// </summary>
        /// <param name="obj"> gets the data og jobcard</param>
        /// <returns></returns>
        /// <remarks></remarks>
        ServiceMessage PostCreate(VMJobCardCreate obj);
        /// <summary>
        /// This method gets all the data for specific employee for deletion.
        /// </summary>
        /// <param name="id"> parameter of specific job card containing all information for deletion.</param>
        /// <returns>returns a modal containing warning for the deletion.</returns>
        /// <remarks></remarks>
        VMJobCardCreate GetDelete(int id);
        /// <summary>
        /// This method get the job card for the deletion.
        /// </summary>
        /// <param name="obj"> parameter of job card which is to be deleted containing Primary key of Jobcard PJobcardApp ID</param>
        /// <returns>returns a modal containing containing message for the deletion.</returns>
        /// <remarks></remarks>
        ServiceMessage PostDelete(JobCardApp obj);
        /// <summary>
        /// Get the job Card Information for the creation of job card on the single day
        /// </summary>
        /// <returns>returns a modal for the creation of single day job card</returns>
        /// <remarks></remarks>
        JobCardApp GetSingleDay();
        /// <summary>
        /// Get job card information for the multiple days 
        /// </summary>
        /// <returns>returns a modal containing information required for the creation of job card datewise.</returns>
        /// <remarks></remarks>
        JobCardApp GetMultipleDay();
        /// <summary>
        /// Post information of single day jobcard.
        /// </summary>
        /// <param name="obj">Parameter of Single day Job Card </param>
        /// <remarks></remarks>
        void PostSingleDay(JobCardApp obj);
        /// <summary>
        /// Post information of Multiple day jobcard.
        /// </summary>
        /// <param name="obj">Parameter of multiple day jobcard.</param>
        /// <remarks></remarks>
        void PostMultipleDay(JobCardApp obj);
    }
}
