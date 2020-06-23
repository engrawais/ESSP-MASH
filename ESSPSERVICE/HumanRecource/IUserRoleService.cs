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
    /// Interface for Assiging the User Role 
    /// </summary>
    /// <remarks>User role is for creating a role to whom rigths are given and that role is furtur assigned to user to restrict or inreasing his activity in the application according to use 
    /// </remarks>
    public interface IUserRoleService
    {
        /// <summary>
        /// This method gets the index of all the user roles created 
        /// </summary>
        /// <returns>returns a view that has list of all the user roles.</returns>
        /// <remarks>index has a edit button and list of all the user roles.</remarks>
        List<AppUserRole> GetIndex();
        /// <summary>
        /// This method gets the information of created user role
        /// </summary>
        /// <param name="id">Id of the specific user role whose modification is to done.</param>
        /// <returns>returns a modal containing entries.</returns>
        /// <remarks></remarks>
        VMAppUserRole GetEdit(string id);
        /// <summary>
        /// This method post the Edited information in the database tables.
        /// </summary>
        /// <param name="obj">Object of view model of AppUserRole and saving all the modified information in the database.</param>
        /// <remarks></remarks>
        void PostEdit(VMAppUserRole obj);
    }
}
