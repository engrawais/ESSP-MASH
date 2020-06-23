//using ESSPCORE.Common;
//using ESSPCORE.EF;
//using ESSPSERVICE.Generic;
//using ESSPSERVICE.HumanRecource;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace AppESSP.Areas.HumanResource.Controllers
//{
//    public class EmployeeOtherController : Controller
//    {
//        // GET: HumanResource/EmployeeOther
//        #region -- Controller Initialization --
//        IEntityService<EmployeeOther> EmployeeOtherEntityService;
//        IEmployeeOtherService EmployeeOtherService;
//        IDDService DDService;
//        //Controller Constructor
//        public EmployeeOtherController(IEntityService<EmployeeOther> employeeOtherEntityService, IDDService ddService, IEmployeeOtherService employeeOtherService)
//        {
//            EmployeeOtherEntityService = employeeOtherEntityService;
//            EmployeeOtherService = employeeOtherService;
//            DDService = ddService;
//        }
//        #endregion
//        #region -- Controller Main View Actions  --
//        //GET: Recruitment/Candidate
//        [HttpGet]
//        public ActionResult Index()
//        {
//            VMLoggedUser vmf = Session["LoggedInUser"] as VMLoggedUser;
//            return View(EmployeeOtherService.GetIndex(vmf));
//        }
//        [HttpGet]
//        public ActionResult Create()
//        {
//            EmployeeOther obj = new EmployeeOther();
//            DDHelper(obj);
//            return View(obj);
//        }
//        [HttpPost]
//        public ActionResult Create(EmployeeOther obj)
//        {
//            if (obj.LineManagerID == null || obj.LineManagerID == 0)
//                ModelState.AddModelError("LineManagerID", "LineManager ID cannot be empty");
//            if (obj.OEmpID == null || obj.OEmpID == "")
//                ModelState.AddModelError("OEmpID", "Emp No cannot be empty");
//            if (obj.EmployeeName == null || obj.EmployeeName == "")
//                ModelState.AddModelError("EmployeeName", "Employee name name cannot be empty");
//            if (obj.FatherName == null || obj.FatherName == "")
//                ModelState.AddModelError("FatherName", "Father name name cannot be empty");
//            if (obj.FPID == null)
//                ModelState.AddModelError("FPID", "FPID cannot be empty");
//            if (obj.Status == "true")
//            {
//                obj.Status = "Active";
//            }
//            else
//            {
//                obj.Status = "Resigned";
//            }
//            if (ModelState.IsValid)
//            {
//                EmployeeOtherEntityService.PostCreate(obj);
//                return Json("OK", JsonRequestBehavior.AllowGet);
//            }
//            DDHelper(obj);
//            return PartialView("Create", obj);
//        }
//        [HttpGet]
//        public ActionResult Edit(int? id)
//        {
//            EmployeeOther obj = EmployeeOtherService.GetEdit((int)id);
//            DDHelper(obj);
//            return PartialView(obj);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [HandleError]
//        public ActionResult Edit(EmployeeOther obj)
//        {
//            if (obj.LineManagerID == null || obj.LineManagerID == 0)
//                ModelState.AddModelError("LineManagerID", "LineManager ID cannot be empty");
//            if (obj.OEmpID == null || obj.OEmpID == "")
//                ModelState.AddModelError("OEmpID", "Emp No cannot be empty");
//            if (obj.EmployeeName == null || obj.EmployeeName == "")
//                ModelState.AddModelError("EmployeeName", "Employee name name cannot be empty");
//            if (obj.FatherName == null || obj.FatherName == "")
//                ModelState.AddModelError("FatherName", "Father name name cannot be empty");
//            if (obj.FPID == null)
//                ModelState.AddModelError("FPID", "FPID cannot be empty");
//            if (ModelState.IsValid)
//            {
//                EmployeeOtherService.PostEdit(obj);
//                return Json("OK", JsonRequestBehavior.AllowGet);
//            }
//            return View("Edit", obj);
//        }
//        [HttpGet]
//        public ActionResult Delete(int? id)
//        {
//            return PartialView(EmployeeOtherEntityService.GetDelete((int)id));
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [HandleError]
//        public ActionResult Delete(EmployeeOther obj)
//        {
//            if (ModelState.IsValid)
//            {
//                EmployeeOtherEntityService.PostDelete(obj);
//                return Json("OK", JsonRequestBehavior.AllowGet);
//            }

//            return PartialView("Delete", obj);
//        }
//        #endregion
//        #region -- Controller Private  Methods--
//        private void DDHelper(EmployeeOther obj)
//        {
//            ViewBag.LineManagerID = new SelectList(AppAssistant.GetLineManagers(DDService.GetUser().Where(aa => aa.UserRoleID == "B" || aa.UserRoleID == "H" || aa.UserRoleID == "U").ToList()), "PUserID", "UserName", obj.LineManagerID);

//        }
//        #endregion
//    }
//}