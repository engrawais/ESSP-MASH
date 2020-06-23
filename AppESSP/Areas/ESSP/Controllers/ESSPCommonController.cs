using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.ESSP;
using ESSPREPO.Generic;
using ESSPSERVICE.ESSP;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.ESSP.Controllers
{
    public class ESSPCommonController : Controller
    {
        // GET: ESSP/ESSPJobCard
        IJobCardESSPService JobCardESSPService;
        ILeavesESSPService LeaveApplicationESSPService;
        IEntityService<VEP_JobCardApplication> VEPJobCardApplicationService;
        IEntityService<VAT_LeaveApplication> VATLeaveApplicationService;
        IRepository<JobCardAppFlow> JobCardAppFlowService;
        public List<string> ToasterMessages = new List<string>();
        IDDService DDService;
        public ESSPCommonController(IJobCardESSPService jobCardESSPService, IEntityService<JobCardApp> jobcardappService, IDDService dDService, IEntityService<JobCardDetail> jobcarddetailservice
          , IEntityService<VEP_JobCardApplication> vEPJobCardApplicationService, IEntityService<VAT_LeaveApplication> vatLeaveApplicationService
            , IRepository<JobCardAppFlow> jobCardAppFlowService, ILeavesESSPService leaveApplicationESSPService)
        {
            JobCardESSPService = jobCardESSPService;
            VEPJobCardApplicationService = vEPJobCardApplicationService;
            VATLeaveApplicationService = vatLeaveApplicationService;
            JobCardAppFlowService = jobCardAppFlowService;
            LeaveApplicationESSPService = leaveApplicationESSPService;
            DDService = dDService;
        }


        [HttpGet]
        public ActionResult CommentView(string FormName, int? PID)
        {
            VMESSPCommon vmEsspCommon = new VMESSPCommon();
            vmEsspCommon.FormName = FormName;
            vmEsspCommon.PID = PID;
            if (vmEsspCommon.FormName == "JobCard")
            {
                ViewBag.StageID = new SelectList(DDService.GetJobCardStage().ToList().Where(aa => aa.PJobCardStageID != "P").ToList(), "PJobCardStageID", "JobCardStageName", "A");
            }
            else
            {
                Expression<Func<VAT_LeaveApplication, bool>> SpecificEntries97 = c => c.PLeaveAppID == PID;
                VAT_LeaveApplication vatLeaveApplication = VATLeaveApplicationService.GetIndexSpecific(SpecificEntries97).First();
                // Leave Stage D is for recomendation 
                if (vatLeaveApplication.LeaveStageID =="D")
                {
                    // Leave Stage L is for revert to LM
                    ViewBag.StageID = new SelectList(DDService.GetLeaveStage().ToList().Where(aa => aa.PLeaveStageID == "L").OrderByDescending(aa => aa.PLeaveStageID).ToList(), "PLeaveStageID", "LeaveStageName", "L");
                }
                // Leave Stage R is for Rejected 
                else if (vatLeaveApplication.LeaveStageID=="P")
                {
                    ViewBag.StageID = new SelectList(DDService.GetLeaveStage().ToList().Where(aa => aa.PLeaveStageID == "R" ).OrderByDescending(aa => aa.PLeaveStageID).ToList(), "PLeaveStageID", "LeaveStageName", "R");
                }
                else if (vatLeaveApplication.LeaveStageID == "L")
                {
                    ViewBag.StageID = new SelectList(DDService.GetLeaveStage().ToList().Where(aa => aa.PLeaveStageID == "R").OrderByDescending(aa => aa.PLeaveStageID).ToList(), "PLeaveStageID", "LeaveStageName", "R");
                }


            }
            return View();
        }
        [HttpPost]
        public ActionResult CommentView(VMESSPCommon vmESSPCommon)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            // Save Entry in Flow\
            string Message = "";
            switch (vmESSPCommon.FormName)
            {
                case "JobCard":
                    if (vmESSPCommon.StageID == "A")
                    {
                        Message = JobCardESSPService.ApproveJobCard(vmESSPCommon, LoggedInUser, Message);
                        ToasterMessages.Add("Job Card successfully Approved !");
                        Session["ToasterMessages"] = ToasterMessages;
                    }
                    else
                    {
                        JobCardESSPService.RejectJobCard(vmESSPCommon, LoggedInUser);
                        ToasterMessages.Add("Job card Rejected successfully !");
                        Session["ToasterMessages"] = ToasterMessages;

                    }
                    break;
                case "Leave":
                    if (vmESSPCommon.StageID == "D")
                    {
                        LeaveApplicationESSPService.RecommendLeaveApplication(vmESSPCommon, LoggedInUser, Message);
                        ToasterMessages.Add("Leave successfully Approved !");
                        Session["ToasterMessages"] = ToasterMessages;
                    }
                    else if (vmESSPCommon.StageID == "R")
                    {
                        LeaveApplicationESSPService.RejectLeaveApplication(vmESSPCommon, LoggedInUser);
                        ToasterMessages.Add("Leave Rejected successfully !");
                        Session["ToasterMessages"] = ToasterMessages;
                    }
                    else if (vmESSPCommon.StageID == "L")
                    {
                        LeaveApplicationESSPService.RevertToLMLeaveApplication(vmESSPCommon, LoggedInUser);
                        ToasterMessages.Add("Leave Reverted successfully !");
                        Session["ToasterMessages"] = ToasterMessages;
                    }
                    break;
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}