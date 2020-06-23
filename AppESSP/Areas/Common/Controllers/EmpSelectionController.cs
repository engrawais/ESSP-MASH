using System.Web.Mvc;
using ESSPSERVICE.Generic;
using ESSPCORE.Common;
using System.Collections.Generic;
using ESSPCORE.EF;

namespace AppESSP.Areas.Common.Controllers
{
    public class EmpSelectionController : Controller
    {
        IEmpSelectionService EmpSelectionService;
        IDDService DDService;
        public EmpSelectionController(IEmpSelectionService empSelectionService, IDDService dDService)
        {
            EmpSelectionService = empSelectionService;
            DDService = dDService;
        }
        // GET: Common/EmpSelection
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StepOne()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            VMEmpSelection vm = EmpSelectionService.GetStepOne(LoggedInUser);
            return View(vm);
        }
        public ActionResult StepTwo(List<VHR_EmployeeProfile> emps)
        {
            return View(emps);
        }
    }
}