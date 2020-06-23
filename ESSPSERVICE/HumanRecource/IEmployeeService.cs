using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.HumanResource;
using ESSPCORE.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.HumanRecource
{
    /// <summary>
    /// This is the Interface of the all the employees  that are present in the database to edit information of employee. 
    /// </summary>
    /// <remarks>This interface defines the methods liek Getting all the employee in the index , Editing the employees and details of employee like if a roster is implemented to that employee or Shift. </remarks>
    public interface IEmployeeService
    {

        /// <summary>
        /// This is the method for getting index view of all the employee there are present in the database.
        /// </summary>
        /// <param name="vmf">this obect gets the information of logged in user that was created in AppUser</param>
        /// <returns>returns the index view containing the list of all the employees</returns>
        /// <remarks>This list of employees showing in this view are only those employee that are in those locations  whic are assigned to the user who is logged In .</remarks>
        List<VHR_EmployeeProfile> GetIndex(VMLoggedUser vmf);
        /// <summary>
        /// Get the Information of employee for editing
        /// </summary>
        /// <param name="id">gets the employee of specific employee whose information os to be edited.</param>
        /// <returns>returns the modal containing information of employee </returns>
        /// <remarks>if User wants to change the information of employee like his policy email id he edits the employee information</remarks>
        VMEmployee GetEdit(int id);
        /// <summary>
        /// Post the information that is being modified in the assigned table i.e. hr.Employee 
        /// </summary>
        /// <param name="obj">This is the object of View Modal of Employee that furthur post the information in the Employee table </param>
        /// <param name="vmf">object of the logged in user whose is editing the information of the user </param>
        /// <remarks>This methos  modifies the entries of the employee .</remarks>
        void PostEdit(VMEmployee obj, VMLoggedUser vmf);

        /// <summary>
        /// This method gets the image of the employee from database.
        /// </summary>
        /// <param name="id">Gets the Id of Specific employee whose image is to be loaded</param>
        /// <returns>returns an image in the Index view where the entry of an employee is created </returns>
        /// <remarks>A smalll round image showing as the first entry in the index of employees table.</remarks>
        byte[] GetImageFromDataBase(int id);

        /// <summary>
        /// This method post the image of the employee in the database 
        /// </summary>
        /// <param name="img">Object of the image to be saved in the database  </param>
        /// <param name="empID"> This object is used to save the image as the employee id because every employee has the unique id.</param>
        /// <remarks> this method save the employee's image in the database as his employee id as this id is unique for every employee so there is no chance of overwriting of image of one employee to others.</remarks>
        void SaveImageInDatabase(byte[] img, int empID);
        /// <summary>
        /// This Method gets the employees Personal,Official and Contact information
        /// </summary>
        /// <param name="id">this is object of EmpID because every details will be shown against employee's id which is unique for everyone.</param>
        /// <returns>returns a view page that has employee every information</returns>
        /// <remarks>Detail of employee that when he joined ,LM name and everyother minute detail.</remarks>
        VHR_EmployeeProfile GetDetail(int id);
        void PostCreate(VMEmployee obj, VMLoggedUser LoggedInUser);
    }
}
