using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;

namespace ESSPSERVICE.ESSP
{
    public class JobCardESSPService : IJobCardESSPService
    {
        IDDService DDService;
        IRepository<JobCardApp> JobCardAppRepository;
        IRepository<JobCardDetail> JobCardDetailReporsitory;
        IRepository<JobCardAppFlow> JobCardAppFlowReporsitory;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IRepository<VAT_JobCardFlow> VATJobCardAppFlowReporsitory;
        IRepository<VEP_JobCardApplication> VEPJobCardApplicationReporsitory;
        IRepository<VAT_JobCardApplication> VATJobCardApplicationReporsitory;
        IUnitOfWork UnitOfWork;
        public JobCardESSPService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<VEP_JobCardApplication> vEPJobCardApplicationReporsitory, IRepository<JobCardApp> jobCardAppRepository, IRepository<JobCardDetail> jobCardDetailReporsitory
            , IRepository<JobCardAppFlow> jobCardAppFlowReporsitory, IRepository<VAT_JobCardApplication> vATJobCardApplicationReporsitory
            , IRepository<VAT_JobCardFlow> vatJobCardAppFlowReporsitory, IEntityService<PayrollPeriod> payrollPeriodService)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            JobCardAppRepository = jobCardAppRepository;
            JobCardDetailReporsitory = jobCardDetailReporsitory;
            VEPJobCardApplicationReporsitory = vEPJobCardApplicationReporsitory;
            JobCardAppFlowReporsitory = jobCardAppFlowReporsitory;
            VATJobCardApplicationReporsitory = vATJobCardApplicationReporsitory;
            VATJobCardAppFlowReporsitory = vatJobCardAppFlowReporsitory;
            PayrollPeriodService = payrollPeriodService;
        }

        public List<VEP_JobCardApplication> GetIndex(VMLoggedUser LoggedInUser)
        {
            //Gets the list Job card of the employee that are in Approved,Pending or Rejected.
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries = c => (c.EmployeeID == LoggedInUser.UserEmpID && (c.JobCardStageID == "A" || c.JobCardStageID == "P" || c.JobCardStageID == "R" || c.JobCardStageID == null));
            return VEPJobCardApplicationReporsitory.FindBy(SpecificEntries);
            //return new List<VEP_JobCardApplication>();
        }
        public List<VEP_JobCardApplication> GetPendingJobCardRequests(VMLoggedUser LoggedInUser)
        {
            //Gets the list Job Card of the employee that are in Pending.
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries = c => (c.LineManagerID == LoggedInUser.PUserID && c.JobCardStageID == "P");
            return VEPJobCardApplicationReporsitory.FindBy(SpecificEntries);
        }
        public List<VEP_JobCardApplication> GetEmpJobCardHistory(VMLoggedUser LoggedInUser)
        {
            //Gets the Job Card List for the Line Manager containing all the Job Cards that he has  Approved or Rejected.
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries = c => (c.LineManagerID == LoggedInUser.PUserID && (c.JobCardStageID == "A" || c.JobCardStageID == "R"));
            return VEPJobCardApplicationReporsitory.FindBy(SpecificEntries);
        }
        public string ApproveJobCard(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser, string Message)
        {
            //Gets the First entry and comment box for the approval of the Job Card Application
            JobCardApp jobCardApp = JobCardAppRepository.GetSingle((int)vmESSPCommon.PID);

            //Changes the stage of JobCard to "A"(Approved).
            //if (DDService.IsDateLieBetweenActivePayroll(jobCardApp.DateStarted))
            //{

            Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => jobCardApp.DateStarted >= c.PRStartDate && jobCardApp.DateStarted<= c.PREndDate && c.PeriodStageID == "C";
            List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
            if (dbPayrollPeriods.Count() > 0)
            {
                Message = "Cannot Approve Job Cards of Closed Payroll Period";

            }
            if (Message == "")
            {
                jobCardApp.JobCardStageID = "A";
                JobCardAppRepository.Edit(jobCardApp);
                UnitOfWork.Commit();
                //Gets the LM and Employee Information
                VHR_UserEmployee LMUser = DDService.GetEmployeeUser(jobCardApp.LineManagerID, null);
                VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, jobCardApp.EmployeeID);
                // Add notifcation to Employee end about the Approval of the Job Card
                DDService.SaveNotification((int)EmpUser.PUserID, "/ESSP/ESSPJobCard/Index",
                    Convert.ToInt32(NotificationTypeJCEnum.JCApproved), true, jobCardApp.LineManagerID, jobCardApp.PJobCardAppID);
                // Create Entry in the Job card flow.
                SaveJobCardFlow((int)EmpUser.PUserID, (int)jobCardApp.LineManagerID, jobCardApp.JobCardStageID, jobCardApp.PJobCardAppID, "", vmESSPCommon.Comment);
                // Save Email in notification email about the approval of the jobcard that is being sent to the employeee through Service.
                Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries2 = c => (c.PJobCardAppID == jobCardApp.PJobCardAppID);
                VAT_JobCardApplication dbVAT_JobCardApplication = VATJobCardApplicationReporsitory.FindBy(SpecificEntries2).First();
                DDService.GenerateEmail(EmpUser.OfficialEmailID, "", "Job Card Application # " + jobCardApp.PJobCardAppID.ToString(),
                       ESSPText.GetApprovedJCText(dbVAT_JobCardApplication, EmpUser.UserEmployeeName, LMUser.UserEmployeeName, LMUser.DesignationName), LoggedInUser.PUserID, Convert.ToInt32(NotificationTypeJCEnum.JCApproved));
                // Create Reprocess Request
                DDService.ProcessDailyAttendance(jobCardApp.DateStarted, jobCardApp.DateEnded, (int)jobCardApp.EmployeeID, jobCardApp.EmployeeID.ToString());
                DDService.ProcessMonthlyAttendance(jobCardApp.DateStarted, (int)jobCardApp.EmployeeID, jobCardApp.EmployeeID.ToString());
                // Disable Notifications of Pending Job Card
                int notiTypeID1 = Convert.ToInt32(NotificationTypeJCEnum.JCPending);
                Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == jobCardApp.PJobCardAppID);
                DDService.DeleteNotification(SpecificEntries);
            }

            return Message;
        }
        public void RejectJobCard(VMESSPCommon vmESSPCommon, VMLoggedUser LoggedInUser)
        {
            //Gets the First entry and comment box for the approval of the Job Card Application
            JobCardApp jobCardApp = JobCardAppRepository.GetSingle((int)vmESSPCommon.PID);
            //Changes the stage of JobCard to "R"(Rejected).
            jobCardApp.JobCardStageID = "R";
            JobCardAppRepository.Edit(jobCardApp);
            UnitOfWork.Commit();
            // Add notifcation to Employee end about the Rejection of the Job Card
            VHR_UserEmployee LMUser = DDService.GetEmployeeUser(jobCardApp.LineManagerID, null);
            VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, jobCardApp.EmployeeID);
            DDService.SaveNotification((int)EmpUser.PUserID, "/ESSP/ESSPJobCard/Index",
               Convert.ToInt32(NotificationTypeJCEnum.JCRejected), true, jobCardApp.EmployeeID, jobCardApp.PJobCardAppID);
            // Create Entry in the Job card flow.
            SaveJobCardFlow(EmpUser.PUserID, LMUser.PUserID, jobCardApp.JobCardStageID, jobCardApp.PJobCardAppID, "", vmESSPCommon.Comment);
            // Save Email in notification email about the Rejection of the jobcard that is being sent to the employeee through Service.
            Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries2 = c => (c.PJobCardAppID == jobCardApp.PJobCardAppID);
            VAT_JobCardApplication dbVAT_JobCardApplication = VATJobCardApplicationReporsitory.FindBy(SpecificEntries2).First();
            DDService.GenerateEmail(EmpUser.OfficialEmailID, "", "Job Card Application # " + jobCardApp.PJobCardAppID.ToString(),
                    ESSPText.GetRejectJCText(dbVAT_JobCardApplication, EmpUser.UserEmployeeName, LMUser.UserEmployeeName, LMUser.DesignationName), LoggedInUser.PUserID, Convert.ToInt32(NotificationTypeJCEnum.JCRejected));
            // Disable Notifications of Pending Job Card
            int notiTypeID1 = Convert.ToInt32(NotificationTypeJCEnum.JCPending);
            Expression<Func<Notification, bool>> SpecificEntries = c => (c.UserID == LoggedInUser.PUserID && c.Status == true && (c.NotificationTypeID == notiTypeID1) && c.PID == jobCardApp.PJobCardAppID);
            DDService.DeleteNotification(SpecificEntries);
        }
        public void SaveJobCardFlow(int SubmittedTo, int SubmittedBy, string Status, int JobcardAppID, string Remarks, string Comment)
        {
            //Creates entry in the Job card Flow table of the Job Card Application  
            JobCardAppFlow dbJobCardFlow = new JobCardAppFlow();
            dbJobCardFlow.JobCardAppID = JobcardAppID;
            dbJobCardFlow.JobCardStageID = Status;
            dbJobCardFlow.SubmittedByUserID = SubmittedBy;
            dbJobCardFlow.CreatedDate = DateTime.Now;
            dbJobCardFlow.SubmittedToUserID = SubmittedTo;
            if (Remarks == null || Remarks == "")
            {
                dbJobCardFlow.Remarks = Comment;
            }
            else
            {
                dbJobCardFlow.Remarks = Remarks;
            }
            JobCardAppFlowReporsitory.Add(dbJobCardFlow);
            JobCardAppFlowReporsitory.Save();
        }

        public void PostCreate(JobCardApp obj, VMLoggedUser LoggedInUser)
        {
            try
            {
                //Save the value provided in the Get of Jobcard and save them in the Jobcard app Table.
                obj.DateCreated = DateTime.Now;
                obj.JobCardStageID = "P";
                obj.LineManagerID = LoggedInUser.LineManagerID;
                obj.Remarks = obj.Remarks;
                obj.JobCardTypeID = obj.JobCardTypeID;
                obj.DateStarted = obj.DateStarted;
                obj.DateEnded = obj.DateEnded;
                JobCardAppRepository.Add(obj);
                JobCardAppRepository.Save();
                // Add Notification to Line Manager that he has a Pending Job card request
                DDService.SaveNotification((int)obj.LineManagerID, "/ESSP/ESSPJobCard/PendingJobCardIndex",
                    Convert.ToInt32(NotificationTypeJCEnum.JCPending), true, obj.EmployeeID, obj.PJobCardAppID);
                SaveJobCardFlow((int)obj.LineManagerID, (int)LoggedInUser.PUserID, obj.JobCardStageID, obj.PJobCardAppID, obj.Remarks, "");
                // Save entry in  Notification email about the Submission of leave application that is sent to LIne manager through the Service
                VHR_UserEmployee LMUser = DDService.GetEmployeeUser(obj.LineManagerID, null);
                VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, obj.EmployeeID);
                Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries = c => (c.PJobCardAppID == obj.PJobCardAppID);
                VAT_JobCardApplication dbVAT_JobCardApplication = VATJobCardApplicationReporsitory.FindBy(SpecificEntries).First();
                DDService.GenerateEmail(LMUser.OfficialEmailID, "", "Job Card Application # " + obj.PJobCardAppID.ToString(),
                   ESSPText.GetPendingJCText(dbVAT_JobCardApplication, LMUser.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NotificationTypeJCEnum.JCPending));
            }
            catch (Exception ex)
            {

            }
        }
        public JobCardApp GetDelete(int id)
        {
            return JobCardAppRepository.GetSingle(id);
        }
        public void PostDelete(JobCardApp obj)
        {
            //Get the Specific id of the jobcard that is to be deleted.
            Expression<Func<JobCardDetail, bool>> SpecificEntries = c => (c.JobCardAppID == obj.PJobCardAppID);
            //delete all the values of the jobcard that are present in the jobcard details.
            List<JobCardDetail> jcd = JobCardDetailReporsitory.FindBy(SpecificEntries);
            foreach (var jcds in jcd)
            {
                JobCardDetailReporsitory.Delete(jcds);
            }
            //Gets the list of Jobcard flows that are created in the database and have same jobcard app id as the job card to be deleted has.
            Expression<Func<JobCardAppFlow, bool>> SpecificEntries2 = c => (c.JobCardAppID == obj.PJobCardAppID);
            List<JobCardAppFlow> jcf = JobCardAppFlowReporsitory.FindBy(SpecificEntries2);
            foreach (var jcfs in jcf)
            {
                JobCardAppFlowReporsitory.Delete(jcfs);
            }

            JobCardAppRepository.Delete(obj);

            UnitOfWork.Commit();
        }
        public void SingleDayPostCreate(JobCardApp obj, VMLoggedUser LoggedInUser)
        {
            try
            {
                //Save the value provided in the Get of Jobcard and save them in the Jobcard app Table.
                obj.DateCreated = DateTime.Today;
                obj.JobCardStageID = "P";
                obj.LineManagerID = LoggedInUser.LineManagerID;
                obj.Remarks = obj.Remarks;
                obj.TimeStart = obj.TimeStart;
                obj.TimeEnd = obj.TimeEnd;
                obj.JobCardTypeID = obj.JobCardTypeID;
                obj.DateEnded = obj.DateStarted;
                obj.DateStarted = obj.DateStarted;
                obj.DateCreated = obj.DateCreated;
                if (obj.TimeEnd != null && obj.TimeEnd != null)
                {
                    obj.Minutes = (short)((obj.TimeEnd.Value - obj.TimeStart.Value).TotalMinutes);
                }

                JobCardAppRepository.Add(obj);
                JobCardAppRepository.Save();
                // Add Notification to Line Manager that he has a Pending Job card request
                DDService.SaveNotification((int)obj.LineManagerID, "/ESSP/ESSPJobCard/PendingJobCardIndex",
                    Convert.ToInt32(NotificationTypeJCEnum.JCPending), true, obj.EmployeeID, obj.PJobCardAppID);
                SaveJobCardFlow((int)obj.LineManagerID, (int)LoggedInUser.PUserID, obj.JobCardStageID, obj.PJobCardAppID, obj.Remarks, "");
                // Save entry in  Notifcation email about the Submission of leave application that is sent to Line manager through the Service
                VHR_UserEmployee LMUser = DDService.GetEmployeeUser(obj.LineManagerID, null);
                VHR_UserEmployee EmpUser = DDService.GetEmployeeUser(null, obj.EmployeeID);
                Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries = c => (c.PJobCardAppID == obj.PJobCardAppID);
                VAT_JobCardApplication dbVAT_JobCardApplication = VATJobCardApplicationReporsitory.FindBy(SpecificEntries).First();
                DDService.GenerateEmail(LMUser.OfficialEmailID, "", "Job Card Application # " + obj.PJobCardAppID.ToString(),
                                   ESSPText.GetPendingJCText(dbVAT_JobCardApplication, LMUser.UserEmployeeName), LoggedInUser.PUserID, Convert.ToInt32(NotificationTypeJCEnum.JCPending));
            }
            catch (Exception ex)
            {

            }
        }

        public VMESSPJobCardDetail GetJobCardEmpDetail(int? id, VMLoggedUser LoggedInUser)
        {    //Gets the Specific JobCard flow id
            Expression<Func<VAT_JobCardFlow, bool>> SpecificEntries = c => c.JobCardAppID == id;
            List<VAT_JobCardFlow> vatJobCardFlows = VATJobCardAppFlowReporsitory.FindBy(SpecificEntries);
            //Gets the Specific Jobcard Application id whose Detail is to shown.
            Expression<Func<VEP_JobCardApplication, bool>> SpecificEntries2 = c => c.PJobCardAppID == id;
            VEP_JobCardApplication dbVEP_JobCard = VEPJobCardApplicationReporsitory.FindBy(SpecificEntries2).First();
            //View Model of the Job card Application to show Specific job card's Data in the View.
            VMESSPJobCardDetail vmesspJobCardDetail = new VMESSPJobCardDetail();
            vmesspJobCardDetail.DBVATjobcardflow = vatJobCardFlows.ToList();
            vmesspJobCardDetail.EmployeeName = dbVEP_JobCard.EmployeeName;
            vmesspJobCardDetail.DesignationName = dbVEP_JobCard.DesignationName;
            vmesspJobCardDetail.JobCardName = dbVEP_JobCard.JobCardName;
            vmesspJobCardDetail.JobCardStageName = dbVEP_JobCard.JobCardStageName;
            vmesspJobCardDetail.DateCreated = dbVEP_JobCard.DateCreated;
            vmesspJobCardDetail.DateStarted = dbVEP_JobCard.DateStarted;
            vmesspJobCardDetail.DateEnded = dbVEP_JobCard.DateEnded;
            vmesspJobCardDetail.TimeEnd = dbVEP_JobCard.TimeEnd;
            vmesspJobCardDetail.TimeStart = dbVEP_JobCard.TimeStart;
            vmesspJobCardDetail.Minutes = dbVEP_JobCard.Minutes;
            vmesspJobCardDetail.Remarks = dbVEP_JobCard.Remarks;
            vmesspJobCardDetail.LMName = LoggedInUser.LMEmployeeName;

            return vmesspJobCardDetail;
        }
    }
}
