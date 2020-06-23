using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ESSPCORE.Common
{
    public class VMLoggedUser : VHR_AppUser
    {
        public List<AppUserLocation> UserLoctions { get; set; }
        public List<AppUserDepartment> UserDepartments { get; set; }
        public List<LocationCommon> UserCommonLoctions { get; set; }
        public string ObjectiveSettingURL { get; set; }
    }
    public enum UserAccessType
    {
        Normal = 1,
        LocationBased = 2,
        AllEmployees = 3,
        DepartmentBased = 4
    }
    public enum NTLeaveEnum
    {
        LeavePending = 1,
        LeaveRecommend = 2,
        LeaveApproved = 4,
        LeaveReverttoLM=5,
        LeaveRejected = 3,
        EmailAlert = 501
    }
    public enum NTObjectiveSettingEnum
    {
        Pending = 4,
        SubmittedToLM = 5,
        Recommend = 6,
        RevertToEmployee = 7,
        RevertToLM = 8,
        Approved = 9,
        RecommendedByLM = 13,
        Agreed = 17,
        Disagreed = 18

    }
    public enum NTAppraisalEnum
    {
        Pending = 401,
        SubmittedToLM = 402,
        Recommend = 403,
        RevertToEmployee = 404,
        RevertToLM = 405,
        Approved = 406,
        RecommendedByLM = 407,
        Agreed = 408,
        Disagreed = 409,
        BellCurveOK = 410
    }
    public enum NTFeedbackMeetingEnum
    {
        Pending = 14,
        ClosedByLM = 15,
        Closed = 16,
    }
    public enum NTMidYearEnum
    {
        Pending = 501,
        SubmittedToLM = 502,
        Recommend = 503,
        RevertToEmployee = 504,
        RevertToLM = 505,
        Approved = 506,
        RecommendedByLM = 507,
        Agreed = 508,
        Disagreed = 509
    }
    public enum NotificationTypePositionApprovalEnum
    {
        Pending = 111,
        Approved = 116,
        Reject = 117
    }
    public enum NotificationTypeRequisitionApprovalEnum
    {
        InitiateER = 200,
        ApprovedER = 201,
        RevertER = 202,
        InitialShortlisting = 203,
        OpenShortlisting = 206,
        FinalShortlisting = 204,
        InterviewSchedule = 205,
        RevertShortListing = 209,
        TestSchedule = 207,
        MarksEnter = 208,
        TestSubmitted = 211,
        InterviewAfterMark = 210,
        InterviewRemarksEnter = 212,
        InterviewSubmitted = 213,
        MeritListSubmitted = 214,
        MeritListApproved = 215,
        ComensationSubmit = 216
    }
    public enum NotificationTypeJCEnum
    {
        JCPending = 10,
        JCApproved = 11,
        JCRejected = 12
    }
    public enum NotificationTypeEmail
    {
        ForgetPassword = 301
    }
    public enum NTProbationEmployee
    {
        RecommendByLM = 601,
        RecommendByLM1 = 602,
        Approve = 603,
        Pending = 604,
        RevertToLM = 605,
        RevertToLM1 = 606,
        Hired = 607,
        Intimate = 608,
        Rejected = 609
    }
    public class VMNotification
    {
        public string Notification { get; set; }
        public string NotificationCount { get; set; }
    }
    //1	Normal
    //2	Location Based
    //3	All Employees
    public enum VMFeedbackSessionNotification
    {
        Pending = 1000,
        Submitted = 1001
    }
}
