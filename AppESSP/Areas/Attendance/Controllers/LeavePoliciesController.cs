using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class LeavePoliciesController : Controller
    {
        IEntityService<LeavePolicy> LeavePolicyService;
        IEntityService<DeviceData> DeviceDataService;
        IDDService DDService;
        public LeavePoliciesController(IEntityService<LeavePolicy> leavepoliciesService, IEntityService<DeviceData> deviceDataService, IDDService dDService)
        {
            
            LeavePolicyService = leavepoliciesService;
            DeviceDataService = deviceDataService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<LeavePolicy> list = LeavePolicyService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            HelperMethod(new LeavePolicy());


            return View();
        }
        [HttpPost]
        public ActionResult Create(LeavePolicy obj)
        {
            ReadFromRadioButton(obj);
            if (obj.LeavePolicyName == null || obj.LeavePolicyName == "")
                ModelState.AddModelError("LeavePolicyName", "Leave Policy name cannot be empty");
            if (LeavePolicyService.GetIndex().Where(lp=>lp.LeavePolicyName==obj.LeavePolicyName).Count()>0)
                ModelState.AddModelError("LeavePolicyName", "Leave Policy name must be unique");
            if (ModelState.IsValid)
            {
                LeavePolicyService.PostCreate(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            LeavePolicy obj = LeavePolicyService.GetEdit((int)id);

            HelperMethod(obj);

            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(LeavePolicy obj)
        {
            ReadFromRadioButton(obj);
            ValidateModel(obj);
            if (obj.LeavePolicyName == null || obj.LeavePolicyName == "")
                ModelState.AddModelError("LeavePolicyName", "Leave Policy name cannot be empty");
            if (LeavePolicyService.GetIndex().Where(lp => lp.LeavePolicyName == obj.LeavePolicyName&& lp.PLeavePolicyID !=obj.PLeavePolicyID).Count() > 0)
                ModelState.AddModelError("LeavePolicyName", "Leave Policy name must be unique");
            if (ModelState.IsValid)
            {
                LeavePolicyService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(LeavePolicyService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(LeavePolicy obj)
        {
            LeavePolicyService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #region -- Private Method--
        private void HelperMethod(LeavePolicy obj)
        {
            ViewBag.LeaveTypeID = new SelectList(DDService.GetLeaveType().ToList().OrderBy(aa => aa.LeaveTypeName).ToList(), "PLeaveTypeID", "LeaveTypeName", obj.LeaveTypeID);
        }
        private void ValidationMethod(LeavePolicy obj)
        {

            #region --Validation--
            //Validation for Carry Forward Feilds
            if (obj.CF == true)
            {
                if (obj.CFMaxDays == null)
                    ModelState.AddModelError("CFMaxDays", "Add Days");
            }

            if (obj.ActiveAfterCustomDays == true)
            {
                if (obj.CustomDays == null)
                    ModelState.AddModelError("ActiveAfterCustomDays", "Add Days");
            }
            if (obj.LeavePolicyName == null || obj.LeavePolicyName == "")
                ModelState.AddModelError("LeavePolicyName", "Leave policy name cannot be empty");
            #endregion
        }
        private void ReadFromRadioButton(LeavePolicy obj)
        {

            #region -- Radio Buttons--
            // Adjust Activation of Leave Radio Button
            string radioActivationValue = "";
            var ActivationValue = ValueProvider.GetValue("activation");
            if (ActivationValue != null)
            {
                radioActivationValue = ActivationValue.AttemptedValue;
            }
            if (radioActivationValue == "ActiveAfterJoinDate")
            {
                obj.ActiveAfterJoinDate = true;
                obj.ActiveAfterProbation = false;
                obj.ActiveAfterCustomDays = false;
                obj.CustomDays = 0;
            }
            if (radioActivationValue == "ActiveAfterProbation")
            {
                obj.ActiveAfterProbation = true;
                obj.ActiveAfterJoinDate = false;
                obj.ActiveAfterCustomDays = false;
                obj.CustomDays = 0;
            }
            if (radioActivationValue == "ActiveAfterCustomDays")
            {
                obj.ActiveAfterCustomDays = true;
                obj.ActiveAfterProbation = false;
                obj.ActiveAfterJoinDate = false;
            }
            // Adjust Payable of Leave Radio Button
            string radioPayableValue = "";
            var PayableValue = ValueProvider.GetValue("payable");
            if (ActivationValue != null)
            {
                radioPayableValue = PayableValue.AttemptedValue;
            }
            if (radioPayableValue == "WithFullPay")
                obj.WithFullPay = true;
            else
                obj.WithFullPay = false;
            if (radioPayableValue == "WithHalfPay")
                obj.WithHalfPay = true;
            else
                obj.WithHalfPay = false;
            if (radioPayableValue == "WithOutPay")
                obj.WithOutPay = true;
            else
                obj.WithOutPay = false;
            #endregion
        }
        #endregion
    }
}
