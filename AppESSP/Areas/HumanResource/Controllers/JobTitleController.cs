using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class JobTitleController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<JobTitle> JobTitleService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public JobTitleController(IEntityService<JobTitle> jobtitleService, IEntityService<Employee> employeeService, IDDService dDService)
        {
            JobTitleService = jobtitleService;
            EmployeeService = employeeService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<JobTitle> list = JobTitleService.GetIndex();
            return View(list);
        }
        [HttpGet]
       
        public ActionResult Create()
        {
            JobTitle obj = new JobTitle();
            EditHelper(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(JobTitle obj)
        {
            if (obj.JobTitleName == null || obj.JobTitleName == "")
                ModelState.AddModelError("JobTitleName", "Job Title Name cannot be empty");
            if (JobTitleService.GetIndex().Where(aa => aa.JobTitleName == obj.JobTitleName).Count() > 0)
                ModelState.AddModelError("JobTitleName", "Job Title Name must be unique");
            if (ModelState.IsValid)
            {
                JobTitleService.PostCreate(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.JobTitle, AuditTypeCommon.Add, obj.PJobTitleID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            EditHelper(obj);
            return PartialView("Create", obj);
        }
        public ActionResult Edit(int? id)
        {
            JobTitle obj = JobTitleService.GetEdit((int)id);
            EditHelper(obj);
            return PartialView(obj);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(JobTitle obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.JobTitleID == obj.PJobTitleID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            }
            if (ModelState.IsValid)
            {
                JobTitleService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            EditHelper(obj);
            return PartialView("Edit", obj);
        }
        public ActionResult LoadNotificationEmployee(string Criteria)
        {
            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
            List<JobTitle> vmList = new List<JobTitle>();
            vmList = JobTitleService.GetIndex().Where(aa => aa.JTCommonID == null).ToList();

            return View("Index", vmList);
        }
        #region
        public ActionResult Delete(int? id)
        {
            return PartialView(JobTitleService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(JobTitle obj)
        {


            Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.JobTitleID == obj.PJobTitleID;
            if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                ModelState.AddModelError("Status", "You cannot Delete due to attachment of active employees");

            if (ModelState.IsValid)
            {
                JobTitleService.PostDelete(obj);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DDService.SaveAuditLog(LoggedInUser.PUserID, AuditFormAttendance.JobTitle, AuditTypeCommon.Delete, obj.PJobTitleID, App_Start.AppAssistant.GetClientMachineInfo());
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
        private void EditHelper(JobTitle obj)
        {
            ViewBag.JTCommonID = new SelectList(DDService.GetCommonJobTitle().ToList().OrderBy(aa => aa.JTCommonName).ToList(), "PJTCommonID", "JTCommonName", obj.JTCommonID);

        }
        #endregion
    }
}
   