using ESSPCORE.EF;
using ESSPCORE.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.HumanRecource
{


    /// <summary>
    ///  Represents the methods that are used to Get all the Users,Create ,Edit them.
    /// </summary>
    /// <remarks> This is the interface of User service and it is used to created the user in the application edit the specific user.User cannot be deleted from the front-end but will be delated by admin in the database.</remarks>
    public interface IUserService

    {

        /// <summary>
        /// Get index of all the users created.
        /// </summary>
        /// <returns>return  the list of user in ESSP. </returns>
        /// <remarks>This index gets the list of all the user </remarks>
        List<AppUser> GetIndex();

        /// <summary>
        /// This method is used to create the user 
        /// </summary>
        /// <returns>returns the screen showing entries that should be filled to create the user</returns>
        /// <remarks>this index shows the entries about the rights of </remarks>
        VMAppUser GetCreate();
        /// <summary>
        /// This method is used to post he user entries filled in the database. 
        /// </summary>
        /// <param name="obj">object for view model of AppUser used to post the entries </param>
        /// <param name="selectedLocIds">These are the location selected at the time of creating the user. </param>
        /// <remarks>This method posts the user entries and rights given to him.</remarks>
        void PostCreate(VMAppUser obj, int?[] SelectedIds);

        /// <summary>
        /// This method gets the information of user created in database for editing.
        /// </summary>
        /// <param name="id"> This is the uniques id generated at the time of creation of user </param>
        /// <returns>returns view that has all the information of user that he has checked at the time creation of user.</returns>
        /// <remarks>This view is used when the admin or other user wants to edit the rights of specfic user created.</remarks>
        VMAppUser GetEdit(int id);
        /// <summary>
        /// This method post the information of the user after making the changes 
        /// </summary>
        /// <param name="obj"> This is the object of View Model of AppUser through  which entries are being posted</param>
        /// <param name="selectedLocIds">If user makes changes in the locations they will be posted against each locations id.</param>
        /// <remarks>post the modification made in the user's account </remarks>
        void PostEdit(VMAppUser obj, int?[] SelectedIds);

        /// <summary>
        /// This  method is used to get the list of all the location in the database.
        /// </summary>
        /// <returns>returns the table that is used in the view of user creation and editing.</returns>
        /// <remarks>this is used to get all the location that are in the database and user have to check from these location and will be assigned to the user.</remarks>
        List<VMUserLocation> GetListofLocations();
    }
    
}
