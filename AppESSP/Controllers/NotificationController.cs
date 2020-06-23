using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Controllers
{
    public class NotificationController : BaseController
    {
        IEntityService<VHR_EmployeeProfile> VHREmployeeService;
        IEntityService<VHR_OrganizationalUnit> VHROrganizationalUnitService;
        IEntityService<VHR_JobTitle> VHRJobTitleService;
        IEntityService<Notification> NotificationService;
        IEntityService<LeaveQuotaYear> LeaveQuotaYearService;

       
        IDDService DDService;
        public NotificationController(IEntityService<VHR_EmployeeProfile> vHREmployeeService, IDDService dDService,
            IEntityService<VHR_OrganizationalUnit> vHROrganizationalUnitService, IEntityService<VHR_JobTitle> vHRJobTitleService,
            IEntityService<Notification> notificationService, IEntityService<LeaveQuotaYear> leaveQuotaYearService
           )
        {
            VHREmployeeService = vHREmployeeService;
            DDService = dDService;
            VHROrganizationalUnitService = vHROrganizationalUnitService;
            VHRJobTitleService = vHRJobTitleService;
            NotificationService = notificationService;
            LeaveQuotaYearService = leaveQuotaYearService;
        }
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetSystemNotification()
        {
            return await Task.Run(() =>
            {
                StringBuilder list = new StringBuilder();
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                List<VHR_EmployeeProfile> vmList = new List<VHR_EmployeeProfile>();
                int NotificationCount = 0;
                VMNotification vmNotification = new VMNotification();
                if (LoggedInUser.UserAccessTypeID == 2 || LoggedInUser.UserAccessTypeID == 1 || LoggedInUser.UserAccessTypeID == 4 || LoggedInUser.UserAccessTypeID == 3)
                {
                    if (LoggedInUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.Status == "Active";
                        vmList = VHREmployeeService.GetIndexSpecific(SpecificEntries);
                    }
                    else if (LoggedInUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
                    {
                        if (LoggedInUser.UserLoctions != null)
                        {
                            foreach (var userLocaion in LoggedInUser.UserLoctions)
                            {
                                Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && c.Status == "Active" && c.Status == "Active";
                                vmList.AddRange(VHREmployeeService.GetIndexSpecific(SpecificEntries));
                            }
                        }
                    }
                    if (vmList.Count > 0)
                    {
                        if (vmList.Where(aa => aa.ShiftID == null).Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Shifts are not assigned to " + vmList.Where(aa => aa.ShiftID == null).Count().ToString() + " employees.", "Incomplete employee Information", GenerateLinkForSystemNotifications("Shift").ToString()));
                        }
                        if (vmList.Where(aa => aa.LineManagerID == null).Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-commit", "btn border-warning text-warning btn-flat btn-rounded btn-icon btn-sm",
                                "Line Managers are not assigned to " + vmList.Where(aa => aa.LineManagerID == null).Count().ToString() + " employees.", "Incomplete employee Information", GenerateLinkForSystemNotifications("LM").ToString()));
                        }
                        if (vmList.Where(aa => aa.CrewID == null).Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Crews are not assigned to " + vmList.Where(aa => aa.CrewID == null).Count().ToString() + " employees.", "Incomplete employee Information", GenerateLinkForSystemNotifications("Crew").ToString()));
                        }
                        if (vmList.Where(aa => aa.FPID == null).Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "FPIDs are not assigned to " + vmList.Where(aa => aa.FPID == null).Count().ToString() + " employees.", "Incomplete employee Information", GenerateLinkForSystemNotifications("FPID").ToString()));
                        }
                        // Ccheck for employees wjo dows not have leave balance

                        int FinYearID = ATAssistant.GetFinancialYearID(DateTime.Today, DDService.GetFinancialYear());
                        Expression<Func<LeaveQuotaYear, bool>> SpecificEntries2 = c => c.FinancialYearID == FinYearID;
                        int EmpWithNoQuota = AppAssistant.GetEmployeeWithNoLeaveQuota(LeaveQuotaYearService.GetIndexSpecific(SpecificEntries2).ToList(), vmList);
                        if (EmpWithNoQuota > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Leave Quota are not assigned to " + EmpWithNoQuota.ToString() + " employees.", "Incomplete employee Information", GenerateLinkForSystemNotifications("LQ").ToString()));
                        }

                    }

                    if (LoggedInUser.UserAccessTypeID == 3)
                    {
                        Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries2 = c => c.OUCommonID == null;
                        List<VHR_OrganizationalUnit> OUList = VHROrganizationalUnitService.GetIndexSpecific(SpecificEntries2);
                        if (OUList.Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-merge", "btn border-success text-success btn-flat btn-rounded btn-icon btn-sm",
                                "Common OU are not assigned to " + OUList.Count().ToString() + " orgaizational units.", "Incomplete Organizatinal units Information", GenerateLinkForSystemNotifications("OrganizationalUnit").ToString()));
                        }
                        Expression<Func<VHR_JobTitle, bool>> SpecificEntries3 = c => c.JTCommonID == null;
                        List<VHR_JobTitle> JTList = VHRJobTitleService.GetIndexSpecific(SpecificEntries3);
                        if (JTList.Count() > 0)
                        {
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Common Job titles are not assigned to " + JTList.Count().ToString() + " job titles.", "Incomplete Job Titles Information", GenerateLinkForSystemNotifications("JobTitle").ToString()));
                        }
                    }
                }
                Expression<Func<Notification, bool>> SpecificEntries4 = c => c.UserID == LoggedInUser.PUserID && c.Status == true;
                List<Notification> NotificationList = NotificationService.GetIndexSpecific(SpecificEntries4).OrderByDescending(aa => aa.PNotificationID).ToList();

                foreach (var item in NotificationList.Select(x => new { x.NotificationTypeID, x.NotificationURL }).Distinct())
                {
                    switch (item.NotificationTypeID)
                    {
                        ///ESSP LEAVE  NOTIFICATIONS
                        case 1: //Pending
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-merge", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Pending Leave Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " pending leave application", item.NotificationURL));
                            break;
                        case 2: //Recommend
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Recommended Leave Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " recommended leave application", item.NotificationURL));
                            break;
                                              case 3: //Reject
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Rejected Leave Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " rejected leave application", item.NotificationURL));
                            break;
                        case 4: //Approved
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Approved Leave Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " approved leave application", item.NotificationURL));
                            break;
                        case 5: //ReverttoLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Reverted Leave Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " reverted leave application", item.NotificationURL));
                            break;
                        ///ESSP LEAVE  NOTIFICATIONS
                        case 10: //Pending JC
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-merge", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Pending Job Card Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " pending job card", item.NotificationURL));
                            break;
                        case 11: //Approved jc
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Approved Job Card Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " approved job card", item.NotificationURL));
                            break;
                        case 12: //Reject jc
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Rejected Job Card Request", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " rejected job card", item.NotificationURL));
                            break;


                        /// OBJECTIVE SETTING NOTIFICATIONS
                        //case 4: //Pending
                        //    NotificationCount++;
                        //    list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                        //        "Individual Objective Setting Launched", "Please click above to proceed. ", item.NotificationURL));
                        //    break;
                        //case 5: //SubmittedToLM
                        //    NotificationCount++;
                        //    list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                        //        "Objective Setting for Review", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " employee's objectives for review.", item.NotificationURL));
                        //    break;
                        case 6: //Recommend
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Individual Objective Settings for Approval", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " employee's objectives for review.", item.NotificationURL));
                            break;
                        case 7: //RevertToEmployee
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Individual Objectives are Reverted", "Please click above to proceed. ", item.NotificationURL));
                            break;
                        case 8: //RevertToLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Objective Settings Reverted", "Please click above to proceed. ", item.NotificationURL));
                            break;
                        case 9: //Approved
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Objectives Sign-off", "Please click above to proceed. ", item.NotificationURL));
                            break;
                        case 13: //Notification to employee when LM recommends the Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Objectives have been Recommended by Line Manager", "Objectives Approval in Process.", item.NotificationURL));
                            break;
                        case 17: //Notification to LM when Employee Agrees with Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Agrees with the Objective.", "", item.NotificationURL));
                            break;
                        case 18://Notification to LM when Employee DisAgrees with Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Disagrees with the Objective", "", item.NotificationURL));
                            break;
                        /// Annual Appraisals NOTIFICATIONS
                        case 401: //Pending
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Self Appraisal Launched", "Please click above to proceed ", item.NotificationURL));
                            break;
                        case 402: //SubmittedToLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Self Appraisal waiting for your decision", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 403: //Recommend
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Managers submit annual appraisals", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 404: //RevertToEmployee
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Annual Appraisal is Reverted", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 405: //RevertToLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Annual Appraisal is Reverted", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 406: //Approved
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Annual Appraisal Approved", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 407: //Notification to employee when LM recommends the Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Objectives have been Recommended by Line Manager", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 408: //Notification to LM when Employee Agrees with Annual Appraisal
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Agrees with the Annual Appraisal.", "", item.NotificationURL));
                            break;
                        case 409://Notification to LM when Employee DisAgrees with Annual Appraisal
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Disagrees with the Annual Appraisal", "", item.NotificationURL));
                            break;
                        case 410://BELL CURVE OK BY DIRECTOR/HOD
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Performance Curve Has been Approved by the HOD", "Please click above to proceed. ", item.NotificationURL));
                            break;
                        ////////////////////////FEEDBACK MEETING NOTIFICATIONS
                        case 14: //Open
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Performance Feedback Meeting Launched", "Click above to proceed", item.NotificationURL));
                            break;
                        case 15: //CLOSED BY LM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Performance Feedback Meeting is Closed By LM", "Click here to close Performance Feedback Meeting ", item.NotificationURL));
                            break;
                        case 16: //Closed
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Performance Feedback Meeting is Closed", "", item.NotificationURL));
                            break;

                        /// MID YEAR REVIEW NOTIFICATIONS
                        case 501: //Pending
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-branch", "btn border-info text-info btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review Launched", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 502: //SubmittedToLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Pending for Review", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 503: //Recommend
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review for Approval", "Please click above .", item.NotificationURL));
                            break;
                        case 504: //RevertToEmployee
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review is Reverted", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 505: //RevertToLM
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review Reverted", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 506: //Approved
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review Sign-off", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 507: //Notification to employee when LM recommends the Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Mid-Year Review Has been Recommended by Line Manager", "Please click above to proceed.", item.NotificationURL));
                            break;
                        case 508: //Notification to LM when Employee Agrees with Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Agrees with the Mid-Year Review.", "", item.NotificationURL));
                            break;
                        case 509://Notification to LM when Employee DisAgrees with Objective.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Disagrees with the Mid-Year Review", "", item.NotificationURL));
                            break;



                        ////Recruitment Position Approval notifications
                        case 111: //Pending
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Pending Position Approval", "Please click at above to approve or revert.", item.NotificationURL));
                            break;
                        case 116: //Approved
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Position Approved", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " approved position.Please click above to view approved position", item.NotificationURL));
                            break;
                        case 117: //Reject
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Position Reverted", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " reverted position.Please click above to view and again submitt.", item.NotificationURL));
                            break;
                        ////Recruitment Position Requisition notifications
                        case 200: //InitiateER
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Pending Employee Requisition", "Please click at above to approve or revert.", item.NotificationURL));
                            break;
                        case 201: //ApprovedER
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Requisition Approval", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " approved employee requisition.Please click above to check candidates for initial shortlisting.", item.NotificationURL));
                            break;
                        case 202: //Revert ER
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Requisition Reverted", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " reverted employee requisition.Please click above to view and again submitt.", item.NotificationURL));
                            break;
                        case 206: //Open Shortliting Stage
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Short-listing Open", "Please click at here to view requisition and start shortliting.", item.NotificationURL));
                            break;
                        case 203: //Initial Shortlisting
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Initial Shortliting", "Please click at above for initial shortlisting.", item.NotificationURL));
                            break;
                        case 204: //Final Shortlisting
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Final Shortlisting", "Please click at above for Final shortlisting.", item.NotificationURL));
                            break;
                        case 205: //Interview Schedule
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Final Short-listing Closed", "Please click above to schedule interview for final shortlisted candidates.", item.NotificationURL));
                            break;
                        case 207: //Test Schedule 
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Final Short-listing Closed", "Please click above to schedule test for final shortlisted candidates.", item.NotificationURL));
                            break;
                        case 218: //Marks Enter
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Test Done", "Please click at above to enter marks of candidates.", item.NotificationURL));
                            break;
                        case 211: //Test Submission
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Marks Entry Done", "Please click at above to submit marks to HR.", item.NotificationURL));
                            break;
                        case 210: //Interview Schedule after Test 
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Test Stage Completed", "Please click at above to schedule interview.", item.NotificationURL));
                            break;
                        case 212: //Interview Remarks Entry
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Interview Done", "Please click at above to add remarks against each interview.", item.NotificationURL));
                            break;
                        case 213: //Interview Submission
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Interview Stage Completed", "Please click at above to create merit list.", item.NotificationURL));
                            break;
                        case 214: //Interview Submission
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Merit List Submission", "Please click at above to approve merit list.", item.NotificationURL));
                            break;
                        case 215: //Interview Submission
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Merit List Approved", "Please click at above to see approve merit list.", item.NotificationURL));
                            break;
                        // NOTIFICATION FOR EMPLOYEE PROBATION
                        //RecommendByLM = 601,
                        //RecommendByLM1 = 602,
                        //Approve = 603,
                        //Reject = 604,
                        //RevertToLM = 605,
                        //RevertToLM1 = 606
                        case 601: //Notification to Line Manager +1 when LM recommends the Probation.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee's Probation Recommended by Line Manager", "Evaluate the Employee's Probation.", item.NotificationURL));
                            break;
                        case 602: //Notification to Director when LM+1 Approves the Probation.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee's Probation Approved and Recommended by Line Manager +1 ", "Evaluate the Employee's Probation.", item.NotificationURL));
                            break;
                        case 604: //Notification to LM For Pending Probation.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",

                            "Employee's Probation Pending for Recommendation", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " pending Probation Evaluation", item.NotificationURL));

                            break;
                        case 605: //Notification to LM when LM+1 Reverts the Probation.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee's Probation Reverted by Line Manager +1 ", "Re-Evaluate the Employee's Probation.", item.NotificationURL));
                            break;

                        case 606: //Notification to LM+1 when Director Reverts the Probation.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee's Probation Reverted By Director", "Re-Evaluate the Employee's Probation.", item.NotificationURL));
                            break;
                        case 607://Notification to LM that employee has been Hired
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee has been Hired by the HR Officer", ".", item.NotificationURL));
                            break;
                        case 608://Notification to LM that employee extension is expiring
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Extension is going to expire soon", ".", item.NotificationURL));
                            break;
                        case 609://Notification to HR 
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee Probation Has been Rejected By the Director", ".", item.NotificationURL));
                            break;
                        case 603://Notification to HR 
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Employee has successfully been probated by Director", "Click above to see the decision", item.NotificationURL));
                            break;
                        // NOTIFICATION FOR Feedback Session
                        //Pending = 1000,
                        //Submitted = 1001,
                        case 1000: //Notification to Employee When Session Starts.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                "Feedback Evalution Pending", "Feedback is pending at your desk.Please click at above to view", item.NotificationURL));
                            break;
                        case 1001: //Notification to Creator When Submitted.
                            NotificationCount++;
                            list.Append(GenerateSingleNotification("icon-git-pull-request", "btn border-primary text-primary btn-flat btn-rounded btn-icon btn-sm",
                                  "Feedback Evaluation Submitted", "You have " + NotificationList.Where(aa => aa.NotificationTypeID == item.NotificationTypeID).Count().ToString() + " submitted feedback from employees.", item.NotificationURL));
                            break;
                    }
                }
                vmNotification.Notification = list.ToString();
                vmNotification.NotificationCount = NotificationCount.ToString();
                return Json(vmNotification, JsonRequestBehavior.AllowGet);
            });
        }

        private StringBuilder GenerateLinkForSystemNotifications(string Criteria)
        {
            StringBuilder link = new StringBuilder();
            switch (Criteria)
            {
                case "Shift":
                    link.Append("/HumanResource/Employee/LoadNotificationEmployee?Criteria=Shift");
                    break;
                case "LM":
                    link.Append("/HumanResource/Employee/LoadNotificationEmployee?Criteria=LM");
                    break;
                case "Crew":
                    link.Append("/HumanResource/Employee/LoadNotificationEmployee?Criteria=Crew");
                    break;
                case "OrganizationalUnit":
                    link.Append("/HumanResource/OrganizationalUnit/LoadNotificationEmployee?Criteria=OrganizationalUnit");
                    break;
                case "JobTitle":
                    link.Append("/HumanResource/JobTitle/LoadNotificationEmployee?Criteria=JobTitle");
                    break;
                case "FPID":
                    link.Append("/HumanResource/Employee/LoadNotificationEmployee?Criteria=FPID");
                    break;
                case "LQ":
                    link.Append("/Attendance/LeaveQuota/Index");
                    break;
                case "Probation":
                    link.Append("/Performance/ProbationManager/ExtensionEmployeesIndex");
                    break;
            }
            return link;
        }
        private StringBuilder GenerateSingleNotification(string ClassName, string ButtonClass, string MessageTitle, string MessageDescription, string Link)
        {
            StringBuilder list = new StringBuilder();
            list.Append("<li class='media'>");
            list.Append("<div class='media-left'>");
            list.Append("<a href = '#' class='" + ButtonClass + "'><i class='" + ClassName + "'></i></a>");
            list.Append("</div>");
            list.Append("<div class='media-body'>");
            list.Append("<a href='" + Link + "'>" + MessageTitle + "</a>");
            list.Append("<div class='media-annotation'>" + MessageDescription + "</div>");
            list.Append(" </div>");
            list.Append(" </li>");
            return list;

        }
    }
}