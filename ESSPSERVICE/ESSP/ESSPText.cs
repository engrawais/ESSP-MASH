using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.ESSP
{
    public static class ESSPText
    {
        /// <summary>
        /// This method Gets the pending Leaves text
        /// </summary>
        /// <param name="vlvapplication"> Gets the information of the applied leave.</param>
        /// <param name="ToName">To whom Email is being sent</param>
        /// <returns>generates a mail to the User </returns>
        /// <remarks>This methods get the Text of the email that is being sent to the user by just calling this functions.</remarks>
        public static string GetPendingLeaveText(VAT_LeaveApplication vlvapplication, string ToName)
        {
            string Body = "<head><style>body{font-family: calibri;}table{font-family: calibri; border-collapse: collapse; width: 100%;font-size:13px;}td, th{border: 1px solid #dddddd; text-align: left; padding: 8px;}</style></head>" +
               "<body>Dear " + ToName + ", <br/><br/>Leave Application # " + vlvapplication.PLeaveAppID.ToString() + " was submitted by " + vlvapplication.EmployeeName + " [" + vlvapplication.DesignationName + "] is <strong>Pending</strong> for your approval.";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetLeaveDetails(vlvapplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary.</p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        public static string GetPendingLeaveAlertEmail(string ToName)
        {
            string Body = "<head><style>body{font-family: calibri;}table{font-family: calibri; border-collapse: collapse; width: 100%;font-size:13px;}td, th{border: 1px solid #dddddd; text-align: left; padding: 8px;}</style></head>" +
               "<body>Dear " + ToName + ", <br/><br/> There are pending leave application at your ESSP Desk which are awaiting for your action.";
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        public static string GetPendingJCAlertEmail(string ToName)
        {
            string Body = "<head><style>body{font-family: calibri;}table{font-family: calibri; border-collapse: collapse; width: 100%;font-size:13px;}td, th{border: 1px solid #dddddd; text-align: left; padding: 8px;}</style></head>" +
               "<body>Dear " + ToName + ", <br/><br/> There are pending Job Cards application at your ESSP Desk which are awaiting for your action.";
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        /// <summary>
        /// This method gets the Text for the Approved leave application
        /// </summary>
        /// <param name="vlvapplication">Leave Application id</param>
        /// <param name="ToName">To whom the email is being sent.</param>
        /// <param name="LMName">Line Managers Name</param>
        /// <param name="LMDesignation">line Manger's Designation</param>
        /// <returns>Generates the Email to employee</returns>
        /// <remarks>This method has the all the text that is required for the email by just calling this function in the controller</remarks>
        public static string GetApprovedLeaveText(VAT_LeaveApplication vlvapplication, string ToName, string LMName, string LMDesignation)
        {
            string Body = GetHeader().ToString() +
                "<body>Dear " + ToName + ", <br/><br/>Your Leave Application # " + vlvapplication.PLeaveAppID.ToString() + " has been <strong>Approved</strong> by " + LMName + " [" + LMDesignation + "].";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetLeaveDetails(vlvapplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
   
        /// <summary>
        /// Gets the Text for the leave that is rejected by the Line manager.
        /// </summary>
        /// <param name="vlvapplication">Get the leave application id.</param>
        /// <param name="ToName">To whom email is being sent.</param>
        /// <param name="LMName">Line managers Name who is sending the email.</param>
        /// <param name="LMDesignation">Line Manager's designation.</param>
        /// <returns></returns>
        /// <remarks>This method has the all the text that is required for the email by just calling this function in the controller</remarks>
        public static string GetRejectLeaveText(VAT_LeaveApplication vlvapplication, string ToName, string LMName, string LMDesignation)
        {
            string Body = GetHeader().ToString() +
                "<body>Dear " + ToName + ", <br/><br/>Your Leave Application # " + vlvapplication.PLeaveAppID.ToString() + " has been <strong>Rejected</strong> by " + LMName + " [" + LMDesignation + "].";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetLeaveDetails(vlvapplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        /// <summary>
        /// Gets the Header for all the Email
        /// </summary>
        /// <returns>A header containing Generic Text</returns>
        /// <remarks>As all of the emails have smae header so we have created a generic class for the header so that code cannot be written again and again just call them through this function.</remarks>
        private static string GetHeader()
        {
            string Body = "<head><style>body{font-family: calibri;}table{font-family: calibri; border-collapse: collapse; width: 100%;font-size:13px;}td, th{border: 1px solid #dddddd; text-align: left; padding: 8px;}</style></head>";
            return Body;
        }
        /// <summary>
        /// Get the details of the leave 
        /// </summary>
        /// <param name="vlvapplication">Gets the Leave Application ID</param>
        /// <returns>Returns a table containg the information of leave that is applied.</returns>
        /// <remarks>Creates a table in the Email that contains all the necessary ionformation for the leave application.</remarks>
        private static string GetLeaveDetails(VAT_LeaveApplication vlvapplication)
        {
            string type = vlvapplication.LeaveTypeName;
            if (vlvapplication.IsHalf == true)
            {
                type = type + " - Half";
                if (vlvapplication.FirstHalf == true)
                    type = type + "(First)";
                else
                    type = type + "(Second)";

            }
            string Body = "<table><tr><td>LEAVE TYPE</td><td>" + type + "</td></tr>"
                + "<tr><td>STARTING DATE</td><td>" + vlvapplication.FromDate.ToString("dd-MMM-yyyy") + "</td></tr>"
                + "<tr><td>ENDING DATE</td><td>" + vlvapplication.ToDate.ToString("dd-MMM-yyyy") + "</td></tr>"
                + "<tr><td>RETURN DATE</td><td>" + vlvapplication.ReturnDate.Value.ToString("dd-MMM-yyyy") + "</td></tr>"
                //+ "<tr><td>TOTAL DAYS</td><td>" + vlvapplication.NoOfDays.ToString() + "</td></tr>"
                + "<tr><td>WORKING DAYS</td><td>" + vlvapplication.NoOfDays.ToString() + "</td></tr></table>";
            return Body;
        }
        /// <summary>
        /// Gets the Pening Job cards Text
        /// </summary>
        /// <param name="vJobCardApplication">gets the pending jobcard id</param>
        /// <param name="ToName">To whom the jobcard is being sent.</param>
        /// <returns>Generates a mail.</returns>
        /// <remarks>This method has the all the text that is required for the email by just calling this function in the controller</remarks>
        public static string GetPendingJCText(VAT_JobCardApplication vJobCardApplication, string ToName)
        {
            string Body = "<head><style>body{font-family: calibri;}table{font-family: calibri; border-collapse: collapse; width: 100%;font-size:13px;}td, th{border: 1px solid #dddddd; text-align: left; padding: 8px;}</style></head>" +
               "<body>Dear " + ToName + ", <br/><br/>Job Card Application # " + vJobCardApplication.PJobCardAppID.ToString() + " was submitted by " + vJobCardApplication.EmployeeName + " [" + vJobCardApplication.DesignationName + "] is <strong>Pending</strong> for your approval.";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetJobCardDetails(vJobCardApplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        /// <summary>
        /// Get the text for the approved leave application.
        /// </summary>
        /// <param name="vJobCardApplication">Gets Jobcard Information of the user.</param>
        /// <param name="ToName">To whom email is being sent</param>
        /// <param name="LMName">line Mangeer who is generating the email</param>
        /// <param name="LMDesignation">Line Mangers Designation</param>
        /// <returns>Generates an email.</returns>
        /// <remarks>This method has the all the text that is required for the email by just calling this function in the controller</remarks>
        public static string GetApprovedJCText(VAT_JobCardApplication vJobCardApplication, string ToName, string LMName, string LMDesignation)
        {
            string Body = GetHeader().ToString() +
                "<body>Dear " + ToName + ", <br/><br/>Your Job Card Application # " + vJobCardApplication.PJobCardAppID.ToString() + " has been <strong>Approved</strong> by " + LMName + " [" + LMDesignation + "].";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetJobCardDetails(vJobCardApplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary. </p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        public static string GetRejectJCText(VAT_JobCardApplication vJobCardApplication, string ToName, string LMName, string LMDesignation)
        {
            string Body = GetHeader().ToString() +
                "<body>Dear " + ToName + ", <br/><br/>Your Job Card Application # " + vJobCardApplication.PJobCardAppID.ToString() + " has been <strong>Rejected</strong> by " + LMName + " [" + LMDesignation + "].";
            Body = Body + "<br/><br/> Further Details are:<br/><br/>" + GetJobCardDetails(vJobCardApplication).ToString();
            Body = Body + "<br/>You can access the ESSP account through following link: <br/>" + "http://essp-portal/";
            Body = Body + "<br/><br/><p>***This is System generated email. Please do not reply.***</p><p>Please print this email only if necessary.</p><p>Thank You!</p>" + "</body>";
            return Body;
        }
        /// <summary>
        /// Gets the details of the Jobcard of the Employee.
        /// </summary>
        /// <param name="vJobCardApplication">id of the specific Jobcard to display its details</param>
        /// <returns>A table with all the necessary information in the table.</returns>
        /// <remarks>Shows a table that containg the minutes ,start time,end time of the jobcard.</remarks>
        private static string GetJobCardDetails(VAT_JobCardApplication vJobCardApplication)
        {
            string ST = "";
            string ET = "";
            string WorkMin = "";
            if (vJobCardApplication.TimeStart != null)
                ST = vJobCardApplication.TimeStart.Value.Hours.ToString("00") + ":" + vJobCardApplication.TimeStart.Value.Minutes.ToString("00");
            if (vJobCardApplication.TimeEnd != null)
                ET = vJobCardApplication.TimeEnd.Value.Hours.ToString("00") + ":" + vJobCardApplication.TimeEnd.Value.Minutes.ToString("00");
            if (vJobCardApplication.Minutes > 0)
            {
                TimeSpan ts = new TimeSpan(0, vJobCardApplication.Minutes.Value, 0);
                WorkMin = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00");
            }
            string Body = "<table><tr><td>JOB CARD TYPE</td><td>" + vJobCardApplication.JobCardName + "</td></tr>"
                + "<tr><td>START DATE</td><td>" + vJobCardApplication.DateStarted.ToString("dd-MMM-yyyy") + "</td></tr>"
                + "<tr><td>END DATE</td><td>" + vJobCardApplication.DateEnded.ToString("dd-MMM-yyyy") + "</td></tr>"
                + "<tr><td>START TIME</td><td>" + ST + "</td></tr>"
                + "<tr><td>END TIME</td><td>" + ET + "</td></tr>"
                + "<tr><td>MINUTES</td><td>" + WorkMin + "</td></tr></table>";
            return Body;
        }
    }
    public static class ESSPEmailSubject
    {
        public static string OBSPending { get { return "Objective Setting Submission Started"; } set { } }
        public static string MYRPending { get { return "Mid-Year Review has Started"; } set { } }
    }
}
