using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPREPO.Generic;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.ESSP
{
    public class LeavesESSPService : ILeavesESSPService
    {
        IDDService DDService;
        IRepository<LeaveApplication> LeaveApplicationRepository;
        IRepository<VAT_LeaveApplication> VEPESSPLeaveApplicationReporsitory;
        IRepository<LeaveApplicationFlow> LeaveApplicationFlow;
        IRepository<VAT_LeaveApplicationFlow> VATLeaveApplicationFlowReporsitory;
        IRepository<LeaveData> LeaveDataRepo;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        ILeaveApplicationService LeaveApplicationService;
        public List<string> ToasterMessages = new List<string>();
        IUnitOfWork UnitOfWork;


        public LeavesESSPService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<VAT_LeaveApplication> vEPESSPLeaveApplicationReporsitory,
        IRepository<LeaveApplicationFlow> leaveApplicationFlow, IRepository<LeaveData> leaveDataRepo,
        IRepository<LeaveApplication> jobESSPLeaveRepository, IRepository<VAT_LeaveApplicationFlow> vatLeaveApplicationFlowReporsitory,
        ILeaveApplicationService leaveApplicationService, IEntityService<PayrollPeriod> payrollPeriodService)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            LeaveApplicationRepository = jobESSPLeaveRepository;
            LeaveApplicationFlow = leaveApplicationFlow;
            PayrollPeriodService = payrollPeriodService;
            LeaveApplicationService = leaveApplicationService;
            VEPESSPLeaveApplicationReporsitory = vEPESSPLeaveApplicationReporsitory;
            LeaveDataRepo = leaveDataRepo;
            VATLeaveApplicationFlowReporsitory = vatLeaveApplicationFlowReporsitory;
        }
        public List<VAT_LeaveApplication> GetIndex(VMLoggedUser LoggedInUser)
        {
            //Gets the leave application list of the employee that are in Approved,Pending or Rejected.
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => (c.EmpID == LoggedInUser.UserEmpID && (c.LeaveStageID == "A" || c.LeaveStageID == "P" || c.LeaveStageID == "R" || c.LeaveStageID == "D" || c.LeaveStageID == null || c.LeaveStageID == "L"));
            return VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries);
            //return new List<VAT_LeaveApplication>();
        }
        public List<VAT_LeaveApplication> GetPendingLeaveRequests(VMLoggedUser LoggedInUser)
        {
            //Gets the leave application list  of the employee that are in Pending.
            //Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries97 = c => c.LineManagerID == LoggedInUser.PUserID;
            //List <VAT_LeaveApplication> vatLeaveApplications = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries97).ToList();
            //foreach(var item in vatLeaveApplications)
            //{
            //    if(item.LeaveStageID)==
            //}
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => (c.LineManagerID == LoggedInUser.PUserID && (c.LeaveStageID == "P" || c.LeaveStageID == "D" || c.LeaveStageID == "L"));
            return VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries);
        }

        public List<VAT_LeaveApplication> GetEmpLeaveHistory(VMLoggedUser LoggedInUser)
        {
            //Gets the Leave Appplication List for the Line Manager containing all the leave applications that he has  Approved or Rejected.

            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries = c => (c.LineManagerID == LoggedInUser.PUserID && (c.LeaveStageID == "A" || c.LeaveStageID == "R" || c.LeaveStageID == "D"));
            return VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries);
        }

        public void CreateLeave(LeaveApplication lvapplication, LeaveType lvType, VMLoggedUser LoggedInUser)
        {
            try
            {
                //gets the information of leave and saving entries from logged in user to leaveApplication.
                lvapplication.LeaveDate = DateTime.Today;
                int _userID = (int)LoggedInUser.PUserID;
                lvapplication.LineManagerID = (int)LoggedInUser.LineManagerID;
                lvapplication.CreatedBy = _userID;
                lvapplication.Active = true;
                lvapplication.LeaveStageID = "P";
                lvapplication.SubmittedByUserID = LoggedInUser.PUserID;
                LeaveApplicationRepository.Add(lvapplication);
                LeaveApplicationRepository.Save();
                // Add notification to Line manager's end that he has a pending leave Request.
                DDService.SaveNotification(lvapplication.LineManagerID, "/ESSP/ESSPLeaveApp/PendingLeaveApplicationIndex",
                Convert.ToInt32(NTLeaveEnum.LeavePending), true, lvapplication.EmpID, lvapplication.PLeaveAppID);
                SaveLeaveApplicationFlow(lvapplication.LineManagerID, _userID, lvapplication.LeaveStageID, lvapplication.PLeaveAppID, lvapplication.LeaveReason, "");
                // Save entry in Notification Email table for where the email is generated through service.
                VHR_UserEmployee LMUser = DDService.GetEmployeeUser(lvapplication.LineManagerID, null);
                VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, lvapplication.EmpID);
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries2 = c => c.PLeaveAppID == lvapplication.PLeaveAppID;
                VAT_LeaveApplication vlvApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries2).First();
                DDService.GenerateEmail(LMUser.OfficialEmailID, "", "Leave Application # " + lvapplication.PLeaveAppID.ToString(),
                    ESSPText.GetPendingLeaveText(vlvApplication, LMUser.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.LeavePending));
            }
            catch (Exception ex)
            {

            }
        }

        public LeaveApplication GetCreate()
        {
            throw new NotImplementedException();
        }

        public LeaveApplication GetDelete(int id)
        {
            return LeaveApplicationRepository.GetSingle(id);
        }
        public void PostDelete(LeaveApplication obj)
        {
            //Deletes the leave Application entry
            LeaveApplicationRepository.Delete(obj);
            UnitOfWork.Commit();
        }
        public void SaveLeaveApplicationFlow(int SubmittedTo, int SubmittedBy, string Status, int LeaveID, string LeaveReason, string Comment)
        {
            //Save the Values in the Leave Application flow table.
            LeaveApplicationFlow dbLeaveApplicationFlow = new LeaveApplicationFlow();
            dbLeaveApplicationFlow.LeaveAppID = LeaveID;
            dbLeaveApplicationFlow.LeaveStageID = Status;
            dbLeaveApplicationFlow.SubmittedByUserID = SubmittedBy;
            dbLeaveApplicationFlow.CreatedDate = DateTime.Now;
            if (LeaveReason == null || LeaveReason == "")
            {
                dbLeaveApplicationFlow.Remarks = Comment;
            }
            else
            {
                dbLeaveApplicationFlow.Remarks = LeaveReason;
            }
            dbLeaveApplicationFlow.SubmittedToUserID = SubmittedTo;
            LeaveApplicationFlow.Add(dbLeaveApplicationFlow);
            LeaveApplicationFlow.Save();
        }
        public VMESSPLeaveDetails GetESSPLeaveEmpDetail(int? id, VMLoggedUser LoggedInUser)
        {
            //Gets the Specific Leave flow id
            Expression<Func<VAT_LeaveApplicationFlow, bool>> SpecificEntries = c => c.LeaveAppID == id;
            List<VAT_LeaveApplicationFlow> vatLeaveApplicationFlows = VATLeaveApplicationFlowReporsitory.FindBy(SpecificEntries);
            //Gets the Specific Leave id whose Detail is to shown.
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries2 = c => c.PLeaveAppID == id;
            VAT_LeaveApplication dbVAT_LeaveApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries2).First();
            //View Model of the leave to show Specific Leave's Data in the View.
            VMESSPLeaveDetails vMESSPLeaveDetail = new VMESSPLeaveDetails();
            vMESSPLeaveDetail.DBVATLeaveApplicationFlow = vatLeaveApplicationFlows;
            vMESSPLeaveDetail.LeaveTypeName = dbVAT_LeaveApplication.LeaveTypeName;
            vMESSPLeaveDetail.PLeaveAppID = dbVAT_LeaveApplication.PLeaveAppID;
            vMESSPLeaveDetail.DesignationName = dbVAT_LeaveApplication.DesignationName;
            vMESSPLeaveDetail.ReturnDate = dbVAT_LeaveApplication.ReturnDate;
            vMESSPLeaveDetail.ToDate = dbVAT_LeaveApplication.ToDate;
            vMESSPLeaveDetail.FromDate = dbVAT_LeaveApplication.FromDate;
            vMESSPLeaveDetail.IsAccum = dbVAT_LeaveApplication.IsAccum;
            vMESSPLeaveDetail.IsHalf = dbVAT_LeaveApplication.IsHalf;
            vMESSPLeaveDetail.FirstHalf = dbVAT_LeaveApplication.FirstHalf;
            vMESSPLeaveDetail.LeaveAddress = dbVAT_LeaveApplication.LeaveAddress;
            vMESSPLeaveDetail.LeaveDate = dbVAT_LeaveApplication.LeaveDate;

            vMESSPLeaveDetail.LeaveTypeID = dbVAT_LeaveApplication.LeaveTypeID;
            if (dbVAT_LeaveApplication.IsAccum == true)
            {
                vMESSPLeaveDetail.LeaveTypeName = "Accumulated";
            }
            vMESSPLeaveDetail.LeaveTypeName = dbVAT_LeaveApplication.LeaveTypeName;
            vMESSPLeaveDetail.LeaveStageID = dbVAT_LeaveApplication.LeaveStageID;
            vMESSPLeaveDetail.CalenderDays = dbVAT_LeaveApplication.CalenderDays;
            vMESSPLeaveDetail.LeaveReason = dbVAT_LeaveApplication.LeaveReason;
            vMESSPLeaveDetail.NoOfDays = dbVAT_LeaveApplication.NoOfDays;
            vMESSPLeaveDetail.RejectRemarks = dbVAT_LeaveApplication.RejectRemarks;
            vMESSPLeaveDetail.EmployeeName = dbVAT_LeaveApplication.EmployeeName;
            vMESSPLeaveDetail.ApprovedBy = dbVAT_LeaveApplication.ApprovedBy;
            vMESSPLeaveDetail.LMName = LoggedInUser.LMEmployeeName;

            return vMESSPLeaveDetail;
        }
        public void UpdatePathName(int? id, string filename)
        {
            Expression<Func<LeaveApplication, bool>> SpecificEntries28 = aa => aa.PLeaveAppID == id;
            LeaveApplication lvapplication = LeaveApplicationRepository.FindBy(SpecificEntries28).First();
            lvapplication.PathName = filename;

            LeaveApplicationRepository.Edit(lvapplication);
            LeaveApplicationRepository.Save();

        }


        #region LINE MANAGER'S ACTIONS
        public string RecommendLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser, string Message)
        {
            //gets the First entry and comment box for the approval of the leave application 
            LeaveApplication lvapplication = LeaveApplicationRepository.GetSingle((int)vmESSPCommon.PID);
            //if (DDService.IsDateLieBetweenActivePayroll(lvapplication.FromDate))
            //{
            //Gets Employee Information about the access of the location and company of the employee.
            List<VHR_EmployeeProfile> _emp = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.PEmployeeID == lvapplication.EmpID).ToList();
            VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.PEmployeeID == lvapplication.EmpID).First();
            //Gets Employee Leave policy on the Type of leave applied
            LeavePolicy lvPolicy = AssistantLeave.GetEmployeeLeavePolicyID(_emp, lvapplication.LeaveTypeID, DDService.GetLeavePolicy().ToList());

            Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => lvapplication.FromDate >= c.PRStartDate && lvapplication.FromDate <= c.PREndDate && c.PeriodStageID == "C";
            List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
            if (dbPayrollPeriods.Count() > 0)
            {
                Message = "Cannot Approve leaves of Closed Payroll Period";

            }
            if (Message == "")
            {
                if (employee.HasOneStep == false)
                {
                    if (LoggedInUser.LineManagerID == null && (lvapplication.LeaveStageID == "P" || lvapplication.LeaveStageID == "D"))
                    {

                        ApprovalCode(vmESSPCommon, LoggedInUser, lvapplication, lvPolicy);
                    }
                    else if (LoggedInUser.LineManagerID != null && (lvapplication.LeaveStageID == "P"))
                    {
                        //gets the information of leave and saving entries from logged in user to leaveApplication.

                        int _userID = (int)LoggedInUser.PUserID;
                        lvapplication.LineManagerID = (int)LoggedInUser.LineManagerID;
                        lvapplication.SubmittedByUserID = LoggedInUser.PUserID;
                        lvapplication.LeaveStageID = "D";
                        LeaveApplicationRepository.Edit(lvapplication);
                        LeaveApplicationRepository.Save();
                        // Add notification to Line manager's end that he has a pending leave Request.
                        DDService.SaveNotification(lvapplication.LineManagerID, "/ESSP/ESSPLeaveApp/PendingLeaveApplicationIndex",
                        Convert.ToInt32(NTLeaveEnum.LeaveRecommend), true, lvapplication.EmpID, lvapplication.PLeaveAppID);
                        SaveLeaveApplicationFlow(lvapplication.LineManagerID, _userID, lvapplication.LeaveStageID, lvapplication.PLeaveAppID, lvapplication.LeaveReason, "");
                        // Save entry in Notification Email table for where the email is generated through service.
                        VHR_UserEmployee LMUser = DDService.GetEmployeeUser(lvapplication.LineManagerID, null);
                        VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, lvapplication.EmpID);
                        Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries2 = c => c.PLeaveAppID == lvapplication.PLeaveAppID;
                        VAT_LeaveApplication vlvApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries2).First();
                        DDService.GenerateEmail(LMUser.OfficialEmailID, "", "Leave Application # " + lvapplication.PLeaveAppID.ToString(),
                            ESSPText.GetPendingLeaveText(vlvApplication, LMUser.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.LeavePending));

                        // Disable Notification of the pending Leave.
                        int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeavePending);
                        Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == lvapplication.PLeaveAppID);
                        DDService.DeleteNotification(SpecificEntries);

                    }
                    else if (LoggedInUser.LineManagerID != null && (lvapplication.LeaveStageID == "D"))
                    {
                        ApprovalCode(vmESSPCommon, LoggedInUser, lvapplication, lvPolicy);
                    }
                }
                else
                {
                    if (lvapplication.LeaveStageID == "P")
                    {
                        ApprovalCode(vmESSPCommon, LoggedInUser, lvapplication, lvPolicy);
                    }

                }
                
            }
            return Message;
        }
        public void RejectLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser)
        {
            //gets the First entry and comment box for the approval of the leave application 
            LeaveApplication lvapplication = LeaveApplicationRepository.GetSingle((int)vmESSPCommon.PID);
            //Changes Leave Stage ID to "A" (Approved).
            lvapplication.LeaveStageID = "R";
            LeaveApplicationRepository.Edit(lvapplication);
            UnitOfWork.Commit();
            //Generated the Notification to the Employee about the rejection of the leave application.
            VHR_UserEmployee LMUser = DDService.GetEmployeeUser(lvapplication.LineManagerID, null);
            VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, lvapplication.EmpID);
            DDService.SaveNotification((int)EmpUser.PUserID, "/ESSP/ESSPLeaveApp/Index",
               Convert.ToInt32(NTLeaveEnum.LeaveRejected), true, lvapplication.EmpID, lvapplication.PLeaveAppID);
            SaveLeaveApplicationFlow(EmpUser.PUserID, LMUser.PUserID, lvapplication.LeaveStageID, lvapplication.PLeaveAppID, "", vmESSPCommon.Comment);
            //Generate Email to the employee that leave is Rejected.
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries3 = c => c.PLeaveAppID == lvapplication.PLeaveAppID;
            VAT_LeaveApplication vlvApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries3).First();
            DDService.GenerateEmail(EmpUser.OfficialEmailID, "", "Leave Application # " + lvapplication.PLeaveAppID.ToString(),
                   ESSPText.GetRejectLeaveText(vlvApplication, EmpUser.UserEmployeeName, LMUser.UserEmployeeName, LMUser.DesignationName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.LeaveRejected));
            // Disable Notification ofPending Leave from the Line Manager's End.
            int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeavePending);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == lvapplication.PLeaveAppID);
            DDService.DeleteNotification(SpecificEntries);
            // Disable Notification ofPending Leave from the Line Manager's End.
            int notiTypeID2 = Convert.ToInt32(NTLeaveEnum.LeaveReverttoLM);
            Expression<Func<Notification, bool>> SpecificEntries2 = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID2) && c.PID == lvapplication.PLeaveAppID);
            DDService.DeleteNotification(SpecificEntries2);
        }
        #endregion

        #region LINE MANAGER'S +1 ACTIONS


        public void RevertToLMLeaveApplication(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser)
        {
            //gets the First entry and comment box for the approval of the leave application 
            LeaveApplication lvapplication = LeaveApplicationRepository.GetSingle((int)vmESSPCommon.PID);
            //Changes Leave Stage ID to "A" (Approved).
            lvapplication.LeaveStageID = "L";
            lvapplication.LineManagerID = (int)lvapplication.SubmittedByUserID;
            LeaveApplicationRepository.Edit(lvapplication);
            UnitOfWork.Commit();
            //Generated the Notification to the Employee about the rejection of the leave application.
            VHR_UserEmployee LMUser = DDService.GetEmployeeUser(lvapplication.LineManagerID, null);
            VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, lvapplication.EmpID);
            DDService.SaveNotification((int)lvapplication.LineManagerID, "/ESSP/ESSPLeaveApp/PendingLeaveApplicationIndex",
               Convert.ToInt32(NTLeaveEnum.LeaveReverttoLM), true, LMUser.UserEmpID, lvapplication.PLeaveAppID);
            SaveLeaveApplicationFlow((int)LMUser.PUserID, (int)LoggedInUser.PUserID, lvapplication.LeaveStageID, lvapplication.PLeaveAppID, "", vmESSPCommon.Comment);
            //Generate Email to the employee that leave is Rejected.
            Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries3 = c => c.PLeaveAppID == lvapplication.PLeaveAppID;
            VAT_LeaveApplication vlvApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries3).First();
            DDService.GenerateEmail(EmpUser.OfficialEmailID, "", "Leave Application # " + lvapplication.PLeaveAppID.ToString(),
                   ESSPText.GetRejectLeaveText(vlvApplication, EmpUser.UserEmployeeName, LMUser.UserEmployeeName, LMUser.DesignationName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.LeaveRejected));
            // Disable Notification ofPending Leave from the Line Manager's End.
            int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeaveRecommend);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == lvapplication.PLeaveAppID);
            DDService.DeleteNotification(SpecificEntries);
        }
        public void ApprovalCode(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser, LeaveApplication lvapplication, LeavePolicy lvPolicy)
        {
            if (LeaveApplicationService.CheckLeaveBalance(lvapplication, lvPolicy))
            {
                //Changes Leave Stage ID to "A" (Approved).
                lvapplication.LeaveStageID = "A";
                //Gets the leave Type through generic service.
                LeaveType lvType = DDService.GetLeaveType().First(aa => aa.PLeaveTypeID == lvapplication.LeaveTypeID);
                LeaveApplicationService.BalanceLeaves(lvapplication, lvType, AssistantLeave.GetPayRollPeriodID(DDService.GetPayrollPeriod().ToList(), lvapplication.FromDate));
                LeaveApplicationRepository.Edit(lvapplication);
                LeaveApplicationRepository.Save();
                UnitOfWork.Commit();
                //Adds leave to leave data frim where its impact is generated on the reports.
                LeaveApplicationService.AddLeaveToLeaveData(lvapplication, lvType, lvPolicy);
                //Add Leaves to Att Data where Daily and Monthy Reporcessing of attendance occurs on the day
                LeaveApplicationService.AddLeaveToAttData(lvapplication, lvType);
                VHR_UserEmployee LMUser = DDService.GetEmployeeUser(lvapplication.LineManagerID, null);
                VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, lvapplication.EmpID);
                // Add notification to the Employee that leave has been approved.
                DDService.SaveNotification((int)EmpUser.PUserID, "/ESSP/ESSPLeaveApp/Index",
                    Convert.ToInt32(NTLeaveEnum.LeaveApproved), true, lvapplication.LineManagerID, lvapplication.PLeaveAppID);
                //Add entry in the flow table
                SaveLeaveApplicationFlow((int)EmpUser.PUserID, lvapplication.LineManagerID, lvapplication.LeaveStageID, lvapplication.PLeaveAppID, "", vmESSPCommon.Comment);

                //Save Email in the Notification Email table from where through services email is generated to employee about the approval of the leave Application.
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries3 = c => c.PLeaveAppID == lvapplication.PLeaveAppID;
                VAT_LeaveApplication vlvApplication = VEPESSPLeaveApplicationReporsitory.FindBy(SpecificEntries3).First();
                DDService.GenerateEmail(EmpUser.OfficialEmailID, "", "Leave Application # " + lvapplication.PLeaveAppID.ToString(),
                        ESSPText.GetApprovedLeaveText(vlvApplication, EmpUser.UserEmployeeName, LMUser.UserEmployeeName, LMUser.DesignationName), LoggedInUser.PUserID, Convert.ToInt32(NTLeaveEnum.LeaveApproved));

                // Disable Notification of the pending Leave.
                int notiTypeID1 = Convert.ToInt32(NTLeaveEnum.LeavePending);
                Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == lvapplication.PLeaveAppID);
                DDService.DeleteNotification(SpecificEntries);
               
                    // Disable Notification of the pending Leave.
                    int notiTypeID2 = Convert.ToInt32(NTLeaveEnum.LeaveRecommend);
                    Expression<Func<Notification, bool>> SpecificEntries2 = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID2) && c.PID == lvapplication.PLeaveAppID);
                    DDService.DeleteNotification(SpecificEntries2);
               
            }
        }
        #endregion
    }
}
