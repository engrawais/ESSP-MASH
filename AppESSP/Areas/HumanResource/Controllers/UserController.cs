using ESSPCORE.EF;
using ESSPCORE.HumanResource;
using ESSPSERVICE.Generic;
using ESSPSERVICE.HumanRecource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class UserController : Controller
    {
        // GET: HumanResource/UserRole
        IUserService UserService;
        IDDService DDService;
        public UserController(IUserService userService, IDDService dDService)
        {
            UserService = userService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<AppUser> list = UserService.GetIndex();
            return View(list);
        }
        [HttpGet]

        public ActionResult Create()
        {
            VMAppUser obj = UserService.GetCreate();
            HelperMethodCreate(obj);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(VMAppUser obj,int?[] SelectedIds)
        {
            // Validation for  balid employee number
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.OEmpID == obj.EmpNo;
            if (DDService.GetSpecificEmployee(SpecificEntries).Count() == 0)
                ModelState.AddModelError("EmpNo", "Employee number does not exist");
            else
                obj.EmpID = DDService.GetEmployeeInfo().Where(aa => aa.OEmpID == obj.EmpNo).First().PEmployeeID;
            // Validation for  duplicate user name
            Expression<Func<AppUser, bool>> SpecificEntries2 = c => c.UserName == obj.UserName;
            if (DDService.GetSpecificUser(SpecificEntries2).Count() > 0)
                ModelState.AddModelError("UserName", "UserName must be unique");
            // Validation for  location based access
            if (obj.UserAccessTypeID==2)
                if(SelectedIds == null)
                    ModelState.AddModelError("UserAccessTypeID", "Must select atleast one location");
            if (obj.UserAccessTypeID == 4)
                if (SelectedIds == null)
                    ModelState.AddModelError("UserAccessTypeID", "Must select atleast one Department");
            if (ModelState.IsValid)
            {
                //obj.Password = App_Start.AppAssistant.Encrypt(obj.Password);
                UserService.PostCreate(obj, SelectedIds);
                return RedirectToAction("Index");
            }
            HelperMethodCreate(obj);
            return View(obj);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            VMAppUser obj = UserService.GetEdit((int)id);
            HelperMethodEdit(obj);
           // obj.Password = App_Start.AppAssistant.Decrypt(obj.Password);
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(VMAppUser obj, int?[] SelectedIds)
        {
            // Validation for  duplicate user name
            Expression<Func<AppUser, bool>> SpecificEntries2 = c => c.UserName == obj.UserName;
            if (DDService.GetSpecificUser(SpecificEntries2).Count() > 1)
                ModelState.AddModelError("UserName", "UserName must be unique");
            // Validation for  location based access
            if (obj.UserAccessTypeID == 2)
                if (SelectedIds.Count() == 0)
                    ModelState.AddModelError("UserAccessTypeID", "Must select atleast one location");
            if (obj.UserAccessTypeID == 4)
                if (SelectedIds.Count() == 0)
                    ModelState.AddModelError("UserAccessTypeID", "Must select atleast one department");
            if (ModelState.IsValid)
            {
                // obj.Password = App_Start.AppAssistant.Encrypt(obj.Password);
                UserService.PostEdit(obj, SelectedIds);
            }
            //EditHelper(vmOperation);
            HelperMethodEdit(obj);
            return RedirectToAction("Index");
        }


        #region -- Private Method--
        private void HelperMethodEdit(VMAppUser obj)
        {
            ViewBag.UserAccessTypeID = new SelectList(DDService.GetUserAccessType().ToList().OrderBy(aa => aa.UserAccessTypeName).ToList(), "PUserAccessTypeID", "UserAccessTypeName", obj.UserAccessTypeID);
            ViewBag.UserRoleID = new SelectList(DDService.GetUserRole().ToList().OrderBy(aa => aa.UserRoleName).ToList(), "PUserRoleID", "UserRoleName", obj.UserRoleID);
        

        }
        private void HelperMethodCreate(VMAppUser obj)
        {
            ViewBag.UserAccessTypeID = new SelectList(DDService.GetUserAccessType().ToList().OrderBy(aa => aa.UserAccessTypeName).ToList(), "PUserAccessTypeID", "UserAccessTypeName", obj.UserAccessTypeID);
            ViewBag.UserRoleID = new SelectList(DDService.GetUserRole().ToList().OrderBy(aa => aa.UserRoleName).ToList(), "PUserRoleID", "UserRoleName", obj.UserRoleID);
            obj.UserLocations = UserService.GetListofLocations();
        }
        #endregion

    }
}
