using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.HumanResource;
using ESSPCORE.Reporting;
using ESSPSERVICE.Generic;
using ESSPSERVICE.HumanRecource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers  
{
    public class EmployeeController : Controller
    {
        IEmployeeService EmployeeService;
        IEntityService<Employee> EmpDBService;
        IDDService DDService;
        public EmployeeController(IEmployeeService employeeService, IDDService dDService, IEntityService<Employee> empDBService)
        {
            EmployeeService = employeeService;
            DDService = dDService;
            EmpDBService = empDBService;
        }
        // GET: HumanResource/Employee
        public ActionResult Index()
        {

            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            return View(EmployeeService.GetIndex(vmf));
        }
        [HttpGet]
        public ActionResult Create()
        {
            VMEmployee obj = new VMEmployee();
            CreateHelper(obj);
            return View();
        }
        [HttpPost]
        public ActionResult Create(VMEmployee obj)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            if (ModelState.IsValid)
            {
                EmployeeService.PostCreate(obj,vmf);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Create", obj);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
      {
            VMEmployee obj = EmployeeService.GetEdit((int)id);
            EditHelper(obj);
            return PartialView(obj);
        }
        [HttpPost]
        public ActionResult Edit(VMEmployee obj)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            if (obj.LineManagerID == 0)
            {
                obj.LineManagerID = null;
            }
            if (obj.CardNo != null && obj.CardNo != "")
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.CardNo == obj.CardNo && c.PEmployeeID != obj.PEmployeeID;
                if (EmpDBService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("CardNo", "Duplicate Card Number");
            }
            if (obj.OfficialEmail != null)
            {
                Match match = Regex.Match(obj.OfficialEmail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!match.Success)
                {
                    ModelState.AddModelError("OfficialEmail", "Invalid Email Address");
                }
            }
            if (ModelState.IsValid)
            {
                EmployeeService.PostEdit(obj, vmf);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            EditHelper(obj);
            return PartialView("Edit", obj);
        }

        [HttpPost]
        public void EPImage()
        {
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null)
                    {
                        int empid = Convert.ToInt32(Request.Form["EmpID"].ToString());
                        EmployeeService.SaveImageInDatabase(ConvertToBytes(file), empid);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult RetrieveImage(int? id)
        {
            try
            {
                var img = EmployeeService.GetImageFromDataBase((int)id);
                if (img != null)
                {
                    return File(img, "image/jpg");
                }
                else
                {
                    return File("~/Theme/assets/images/image.png", "image/png");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Detail(int? id)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            VHR_EmployeeProfile vhremployeeprofile = new VHR_EmployeeProfile();
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.Status == "Active" && c.PEmployeeID == id;
            return View(DDService.GetSpecificEmployee(SpecificEntries).First());
        }
        public ActionResult LoadNotificationEmployee(string Criteria)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            List<VHR_EmployeeProfile> vmList = new List<VHR_EmployeeProfile>();
            vmList = EmployeeService.GetIndex(vmf);
            switch (Criteria)
            {
                case "Shift":
                    vmList = vmList.Where(aa => aa.ShiftID == null).ToList();
                    break;
                case "LM":
                    vmList = vmList.Where(aa => aa.LMUserName == null && aa.Status == "Active").ToList();
                    break;
                case "Crew":
                    vmList = vmList.Where(aa => aa.CrewID == null).ToList();
                    break;
                case "FPID":
                    vmList = vmList.Where(aa => aa.FPID == null).ToList();
                    break;
            }
            return View("Index", vmList);
        }
        #region - Helper

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            Image img = Image.FromStream(image.InputStream);
            Image conImage = ScaleImage(img, 230, 500);
            byte[] imageBytes = null;
            imageBytes = imgToByteArray(conImage);
            return imageBytes;
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
        public byte[] imgToByteArray(Image img)
        {
            var ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private void EditHelper(VMEmployee obj)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LineManagerID = new SelectList(AppAssistant.GetLineManagers(DDService.GetUser().ToList()), "PUserID", "UserName", obj.LineManagerID);
            ViewBag.OTPolicyID = new SelectList(DDService.GetOTPolicy().ToList().OrderBy(aa => aa.OTPolicyName).ToList(), "POTPolicyID", "OTPolicyName", obj.OTPolicyID);
            ViewBag.ShiftID = new SelectList(DDService.GetShift(vmf).ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName", obj.ShiftID);
            ViewBag.ALPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 1).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.ALPolicyID);
            ViewBag.SLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 3).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.SLPolicyID);
            ViewBag.CLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 2).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CLPolicyID);
            ViewBag.CPLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 4).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CPLPolicyID);
            ViewBag.EALPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 11).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CPLPolicyID);
            ViewBag.CMEPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 12).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CPLPolicyID);
            ViewBag.CrewID = new SelectList(DDService.GetCrew(vmf).ToList().OrderBy(aa => aa.CrewName).ToList(), "PCrewID", "CrewName", obj.CrewID);
            ViewBag.DesigationID = new SelectList(DDService.GetDesignation(vmf).ToList().OrderBy(aa => aa.DesignationName).ToList(), "PDesignationID", "DesignationName", obj.DesigationID);
            ViewBag.JobTitleID = new SelectList(DDService.GetJobTitle(vmf).ToList().OrderBy(aa => aa.JobTitleName).ToList(), "PJobTitleID", "JobTitleName", obj.JobTitleID);
            ViewBag.LocationID = new SelectList(DDService.GetLocation(vmf).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
            ViewBag.DepartmentID = new SelectList(DDService.GetOUCommon(vmf).ToList().OrderBy(aa => aa.OUCommonName).ToList(), "POUCommonID", "OUCommonName", obj.DepartmentID);
            ViewBag.SectionID = new SelectList(DDService.GetOU(vmf).ToList().OrderBy(aa => aa.OUName).ToList(), "POUID", "OUName", obj.SectionID);
            ViewBag.EmploymentTypeID = new SelectList(DDService.GetEmploymentType(vmf).ToList().OrderBy(aa => aa.EmploymentTypeName).ToList(), "PEmploymentTypeID", "EmploymentTypeName", obj.EmploymentTypeID);
            ViewBag.ShiftID = new SelectList(DDService.GetShift(vmf).ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName", obj.ShiftID);
            ViewBag.GradeID = new SelectList(DDService.GetGrade(vmf).ToList().OrderBy(aa => aa.GradeName).ToList(), "PGradeID", "GradeName", obj.GradeID);
        }
        private void CreateHelper(VMEmployee obj)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.DesigationID = new SelectList(DDService.GetDesignation(vmf).ToList().OrderBy(aa => aa.DesignationName).ToList(), "PDesignationID", "DesignationName", obj.DesigationID);
            ViewBag.JobTitleID = new SelectList(DDService.GetJobTitle(vmf).ToList().OrderBy(aa => aa.JobTitleName).ToList(), "PJobTitleID", "JobTitleName", obj.JobTitleID);
            ViewBag.LocationID = new SelectList(DDService.GetLocation(vmf).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
            ViewBag.DepartmentID = new SelectList(DDService.GetOUCommon(vmf).ToList().OrderBy(aa => aa.OUCommonName).ToList(), "POUCommonID", "OUCommonName", obj.DepartmentID);
            ViewBag.SectionID = new SelectList(DDService.GetOU(vmf).ToList().OrderBy(aa => aa.OUName).ToList(), "POUID", "OUName", obj.SectionID);
            ViewBag.EmploymentTypeID = new SelectList(DDService.GetEmploymentType(vmf).ToList().OrderBy(aa => aa.EmploymentTypeName).ToList(), "PEmploymentTypeID", "EmploymentTypeName", obj.EmploymentTypeID);
            ViewBag.ShiftID = new SelectList(DDService.GetShift(vmf).ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName", obj.ShiftID);
            ViewBag.CrewID = new SelectList(DDService.GetCrew(vmf).ToList().OrderBy(aa => aa.CrewName).ToList(), "PCrewID", "CrewName", obj.CrewID);
            ViewBag.GradeID = new SelectList(DDService.GetGrade(vmf).ToList().OrderBy(aa => aa.GradeName).ToList(), "PGradeID", "GradeName", obj.GradeID);
        }
        #endregion
    }
}