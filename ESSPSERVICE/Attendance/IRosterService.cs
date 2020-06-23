using ESSPCORE.Attendance;
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
    /// Represents the methods that are used to create the roster of the employees.
    /// </summary>
    /// <remarks></remarks>
    public interface IRosterService
    {
        /// <summary>
        /// Gets the list of all the roster applied by the user.
        /// </summary>
        /// <param name="LoggedInUser"> this parameter gets the logged in user</param>
        /// <returns>shows the rosters of the locations whom logged in user has access to</returns>
        /// <remarks>this list will show roster of only those locations whose access is given to logged in user </remarks>
        List<VAT_RosterApplication> GetIndex(VMLoggedUser LoggedInUser);
        /// <summary>
        /// This method post the information related to the roster like name minutes etc.
        /// </summary>
        /// <param name="obj">objecto of view model of the roster which gets all the information of roster like roster id and other information</param>
        /// <param name="LoggedInUser">get the logged in user </param>
        /// <returns>return the view with all necessary infornmation used for the creation of roster</returns>
        /// <remarks></remarks>
        VMRosterModel PostCreate1(VMRosterApplication obj, VMLoggedUser LoggedInUser);
        /// <summary>
        /// This method shows the roster apllied details like works minutes throughout the selected time span day wise 
        /// </summary>
        /// <param name="vm">gets the values of that specific roster</param>
        /// <param name="rosterAttributeList">gets the list of attributes of roster like work minutes </param>
        /// <remarks></remarks>
        void PostCreate2(VMRosterModel vm, List<RosterAttributes> rosterAttributeList);
        /// <summary>
        /// this method continues the roster by getting the date and apply it to the specific roster.
        /// </summary>
        /// <param name="vm">Gets the specific roster id that we need to continue.</param>
        /// <returns>Returns a modal with date field.</returns>
        /// <remarks></remarks>
        VMRosterModel ContinueRoster(VMRosterContinue vm);

    }
}
