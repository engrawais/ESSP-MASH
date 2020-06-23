using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Generic
{
    public interface IDDService
    {

        List<Company> GetCompany();
        List<Catagory> GetCatagory();
        List<Company> GetCompany(VMLoggedUser loggedUser);
        List<VHR_Location> GetLocation();
        List<VHR_Location> GetLocation(VMLoggedUser loggedUser);
        List<LocationCommon> GetCommanLocation();
        List<LocationCommon> GetCommanLocation(VMLoggedUser loggedUser);
        List<VHR_OrganizationalUnit> GetOU();
        List<OrganizationalUnit> GetOUSimple();
        List<VHR_OrganizationalUnit> GetOU(VMLoggedUser loggedUser);
        List<JTCommon> GetCommonJobTitle();
        List<JTCommon> GetCommonJobTitle(VMLoggedUser loggedUser);
        List<OUCommon> GetOUCommon();
        List<OUCommon> GetOUCommon(VMLoggedUser loggedUser);
        List<VHR_JobTitle> GetJobTitle();
        List<VHR_JobTitle> GetJobTitle(VMLoggedUser loggedUser);
        List<Crew> GetCrew();
        List<Crew> GetCrew(VMLoggedUser loggedUser);
        List<VHR_Grade> GetGrade();
        List<VHR_Grade> GetGrade(VMLoggedUser loggedUser);
        List<VHR_EmploymentType> GetEmploymentType();
        List<VHR_EmploymentType> GetEmploymentType(VMLoggedUser loggedUser);
        List<VHR_Designation> GetDesignation();
        List<VHR_Designation> GetDesignation(VMLoggedUser loggedUser);
        List<OTPolicy> GetOTPolicy();
        List<ReaderType> GetReaderType();
        List<VHR_EmployeeProfile> GetEmployeeInfo();
        List<VHR_EmployeeProfile> GetEmployeeInfo(VMLoggedUser loggedUser);
        List<VAT_Shift> GetShift();
        List<VAT_ShiftChanged> GetShiftChange(VMLoggedUser loggedUser);
        List<VAT_Shift> GetShift(VMLoggedUser loggedUser);
        List<JobCardType> GetJobCardType();
        List<JobCardStage> GetJobCardStage();
        List<LeaveStage> GetLeaveStage();
        List<DaysName> GetDaysName();
        List<LeaveType> GetLeaveType();
        List<ReaderDutyCode> GetReaderDutyCode();
        List<RosterType> GetRosterType();
        List<FinancialYear> GetFinancialYear();
        List<PayrollPeriod> GetPayrollPeriod();
        List<PayrollPeriod> GetAllPayrollPeriod();
        List<Reader> GetReader();
        List<AppUser> GetUser();
        List<VHR_AppUser> GetVHRAppUser();
        List<LocationCommon> CommonLocationOU();
        List<LeavePolicy> GetLeavePolicy();
        bool IsDateLieBetweenActivePayroll(DateTime attDataFrom);
        List<AppUserRole> GetUserRole();
        List<AppUserAccessType> GetUserAccessType();
        List<AppUserLocation> GetUserLocation(Expression<Func<AppUserLocation, bool>> predicate);
        List<AppUserDepartment> GetUserDepartment(Expression<Func<AppUserDepartment, bool>> predicate);
        List<VHR_EmployeeCrewChange> GetSpecificEmployeeHistory(Expression<Func<VHR_EmployeeCrewChange, bool>> predicate);
        List<VHR_EmployeeProfile> GetSpecificEmployee(Expression<Func<VHR_EmployeeProfile, bool>> predicate);
        List<VHR_EmployeeProfile> GetReportingToEmployees(VMLoggedUser LoggedInUser);
        List<AppUser> GetSpecificUser(Expression<Func<AppUser, bool>> predicate);
        List<Notification> GetNotifications(Expression<Func<Notification, bool>> predicate);
        void SaveAuditLog(int UserID, AuditFormAttendance form, AuditTypeCommon type, int pID, IPHostEntry ipHostEntry);
        void SaveNotification(int UserID, string url, int notificationTypeID, bool status, int? EmployeeID, int? PID);
        void DeleteNotification(Expression<Func<Notification, bool>> predicate);
        void ProcessMonthlyAttendance(DateTime dts, int EmpID, string EmpNo);
        void ProcessMonthlyERPAttendance(DateTime dts, int EmpID);
        void ProcessDailyAttendance(DateTime dts, DateTime dte, int EmpID, string EmpNo);
        void GenerateEmail(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID);
        void GenerateEmailForPerformance(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID);
        void GenerateEmailForRecruitment(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID);
       VHR_UserEmployee GetEmployeeUser(int? UserID, int? EmpID);
        List<Holiday> GetHolidays();
        List<MonthOTStage> GetMonthOTStage();
        List<VHR_UserCommonLocation> GetUserCommonLocations(VMLoggedUser LoggedInUser);
        List<VHR_AppUser> GetSpecificUsers(Expression<Func<VHR_AppUser, bool>> predicate);
      
    }
}
