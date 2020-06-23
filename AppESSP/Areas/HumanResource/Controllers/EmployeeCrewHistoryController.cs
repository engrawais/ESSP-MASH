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
    public class EmployeeCrewHistoryController : Controller
    {

        IEntityService<VHR_EmployeeCrewChange> VHREmployeeCrewChangeService;
        IEntityService<EmployeeCrewChange> EmployeeCrewChangeService;
        IDDService DDService;
        public EmployeeCrewHistoryController(IDDService dDService,
      IEntityService<VAT_LeaveApplication> vATLeaveApplicationService, IEntityService<VHR_EmployeeCrewChange> vHREmployeeCrewChangeService,
         IEntityService<EmployeeCrewChange> employeeCrewChangeService)
        {
            EmployeeCrewChangeService = employeeCrewChangeService;
            VHREmployeeCrewChangeService = vHREmployeeCrewChangeService;
            DDService = dDService;
        }
        // GET: HumanResource/EmployeeCrewHistory
        public ActionResult Index(int? id)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VHR_EmployeeCrewChange> list = new List<VHR_EmployeeCrewChange>();

            Expression<Func<VHR_EmployeeCrewChange, bool>> SpecificEntries = c => c.EmpID == id;
            list.AddRange(VHREmployeeCrewChangeService.GetIndexSpecific(SpecificEntries));

            return View(list);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            VHR_EmployeeCrewChange obj = VHREmployeeCrewChangeService.GetEdit((int)id);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(VHR_EmployeeCrewChange obj)
        {

            if (ModelState.IsValid)
            {
                EmployeeCrewChange dbEmployeeCrewChange = EmployeeCrewChangeService.GetEdit(obj.PEmployeeCrewChangeID);
                dbEmployeeCrewChange.StartDate = obj.StartDate;
                dbEmployeeCrewChange.EndDate = obj.EndDate;
                EmployeeCrewChangeService.PostEdit(dbEmployeeCrewChange);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(VHREmployeeCrewChangeService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(VHR_EmployeeCrewChange obj)
        {
         if (ModelState.IsValid)
            {
                EmployeeCrewChange dbEmployeeCrewChange = EmployeeCrewChangeService.GetDelete(obj.PEmployeeCrewChangeID);
                dbEmployeeCrewChange.StartDate = obj.StartDate;
                dbEmployeeCrewChange.EndDate = obj.EndDate;
                EmployeeCrewChangeService.PostDelete(dbEmployeeCrewChange);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
              return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return PartialView("Delete", obj);
        }
    }
}