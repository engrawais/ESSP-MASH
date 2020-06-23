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
    public class UserRoleController : Controller
    {
        // GET: HumanResource/UserRole
        IUserRoleService UserRoleService;
        IDDService DDService;
        public UserRoleController(IUserRoleService userRoleService,  IDDService dDService)
        {
            UserRoleService = userRoleService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            return View(UserRoleService.GetIndex().ToList());
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(UserRoleService.GetEdit(id));
        }
      
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(VMAppUserRole obj)
        {
            if (ModelState.IsValid)
            {
                UserRoleService.PostEdit(obj);
            }
            //EditHelper(vmOperation);
            return RedirectToAction("Index");
        }
    }

}