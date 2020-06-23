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
    public class GradeController : Controller
    {
        // GET: HumanRecource/Designation
        IEntityService<Grade> GradeService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public GradeController(IEntityService<Grade> gradeService, IEntityService<Employee> employeeService, IDDService dDService)
        {
            GradeService = gradeService;
            EmployeeService = employeeService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<Grade> list = GradeService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Grade obj = GradeService.GetEdit((int)id);
            EditHelper(obj);
            return PartialView(obj); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Grade obj)
        {
            if (obj.Status == false)
            {
                Expression<Func<Employee, bool>> SpecificEntries = c => c.Status == "Active" && c.GradeID == obj.PGradeID;
                if (EmployeeService.GetIndexSpecific(SpecificEntries).Count() > 0)
                    ModelState.AddModelError("Status", "You cannot inactive due to attachment of active employees");
            }
            if (ModelState.IsValid)
            {
                GradeService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            EditHelper(obj);
            return PartialView("Edit", obj);
        }
#region
        private void EditHelper(Grade obj)
        {
            ViewBag.OTPolicyID = new SelectList(DDService.GetOTPolicy().ToList().OrderBy(aa => aa.OTPolicyName).ToList(), "POTPolicyID", "OTPolicyName", obj.OTPolicyID);
            ViewBag.ALPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 1).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.ALPolicyID);
            ViewBag.SLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 3).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.SLPolicyID);
            ViewBag.CLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 2).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CLPolicyID);
            ViewBag.CPLPolicyID = new SelectList(DDService.GetLeavePolicy().Where(aa => aa.LeaveTypeID == 4).ToList().OrderBy(aa => aa.LeavePolicyName).ToList(), "PLeavePolicyID", "LeavePolicyName", obj.CPLPolicyID);

        }
#endregion
    }
}