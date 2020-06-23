using AppESSP.Areas.Reporting.BusinessLogic;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Reporting.Controllers
{
    public class ReportManagerController : Controller
    {
        IDDService DDService;
        public ReportManagerController(IDDService dDService)
        {
            DDService = dDService;
        }
        // GET: Reporting/ReportManager
        public ActionResult Index()
        {
            VMSelectedFilter vmf = new VMSelectedFilter();
            Session["FiltersModel"] = vmf;
            PayrollPeriod prp = ATAssistant.GetPayrollPeriodObject(DateTime.Today, DDService.GetPayrollPeriod());
            if (vmf.DateFrom == null)
                vmf.DateFrom = prp.PRStartDate;
            if (vmf.DateTo == null)
                vmf.DateTo = prp.PREndDate;
            return View(vmf);
        }

        #region -- Load Partial View --
        public ActionResult CommonOU()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetOUCommon(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.POUCommonID;
                obj.FilterName = item.OUCommonName;
                obj.IsSlected = vmf.SelectedCommonOU.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                vm.Add(obj);
            }
            return View(vm);
        }
        public ActionResult Company()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetCompany(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PCompanyID;
                obj.FilterName = item.CompanyName;
                obj.IsSlected = vmf.SelectedCompany.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                vm.Add(obj);
            }
            return View(vm);
        }
        public ActionResult Location()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetLocation(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PLocationID;
                obj.FilterName = item.LocationName;
                obj.IsSlected = vmf.SelectedLocation.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                //obj.FieldOneID = item.CompanyID;
                //obj.FieldOneName = item.CompanyName;
                vm.Add(obj);
            }
            //vm = CompanyVMHelper(vm, vmf);
            return View(vm);
        }
        public ActionResult Grade()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetGrade(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PGradeID;
                obj.FilterName = item.GradeName;
                obj.IsSlected = vmf.SelectedGrade.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                vm.Add(obj);
            }
            vm = CompanyVMHelper(vm, vmf);
            return View(vm);
        }
        public ActionResult JobTitle()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetJobTitle(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PJobTitleID;
                obj.FilterName = item.JobTitleName;
                obj.IsSlected = vmf.SelectedJobTitle.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                //obj.FieldOneID = item.CompanyID;
               // obj.FieldOneName = item.CompanyName;
                vm.Add(obj);
            }
            //vm = CompanyVMHelper(vm, vmf);
            return View(vm);
        }
        public ActionResult Position()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetDesignation(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PDesignationID;
                obj.FilterName = item.DesignationName;
                obj.IsSlected = vmf.SelectedPosition.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                obj.FieldTwoID = item.JobTitleID;
                obj.FieldTwoName = item.JobTitleName;
                vm.Add(obj);
            }
            vm = CompanyVMHelper(vm, vmf);

            List<VMFilterAttribute> tempVM = new List<VMFilterAttribute>();
            if (vmf.SelectedJobTitle.Count > 0)
            {
                foreach (var item in vmf.SelectedJobTitle)
                {
                    tempVM.AddRange(vm.Where(aa => aa.FieldTwoID == item.FilterID).ToList());
                }
                vm = tempVM.ToList();
            }
            else
                tempVM = vm.ToList();
            return View(tempVM);
        }
        public ActionResult OrganizationalUnit()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetOU(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.POUID;
                obj.FilterName = item.OUName;
                obj.IsSlected = vmf.SelectedOrganizationalUnit.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                obj.FieldThreeID = item.OUCommonID;
                obj.FieldThreeName = item.OUCommonName;
                vm.Add(obj);
            }
            vm = CompanyVMHelper(vm, vmf);
            vm = LocationVMHelper(vm, vmf);
            List<VMFilterAttribute> tempVM = new List<VMFilterAttribute>();
            if (vmf.SelectedCommonOU.Count > 0)
            {
                foreach (var item in vmf.SelectedCommonOU)
                {
                    tempVM.AddRange(vm.Where(aa => aa.FieldThreeID == item.FilterID).ToList());
                }
                vm = tempVM.ToList();
            }
            else
                tempVM = vm.ToList();
            return View(tempVM);
        }
        public ActionResult Crew()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetCrew(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PCrewID;
                obj.FilterName = item.CrewName;
                obj.IsSlected = vmf.SelectedCrew.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                vm.Add(obj);
            }
            return View(vm);
        }
        public ActionResult Shift()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetShift(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PShiftID;
                obj.FilterName = item.ShiftName;
                obj.IsSlected = vmf.SelectedShift.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                obj.FieldTwoID = item.LocationID;
                obj.FieldTwoName = item.LocationName;
                vm.Add(obj);
            }
            vm = LocationVMHelper(vm, vmf);

            return View(vm);
        }
        public ActionResult Employee()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;

            List<VHR_EmployeeProfile> tempVM = new List<VHR_EmployeeProfile>();
            List<VHR_EmployeeProfile> VM = DDService.GetEmployeeInfo(LoggedInUser).ToList();
            if (vmf.SelectedCommonOU.Count > 0)
            {
                foreach (var item in vmf.SelectedCommonOU)
                {
                    tempVM.AddRange(VM.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            if (vmf.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in vmf.SelectedOrganizationalUnit)
                {
                    tempVM.AddRange(VM.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            if (vmf.SelectedShift.Count > 0)
            {
                foreach (var item in vmf.SelectedShift)
                {
                    tempVM.AddRange(VM.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            if (vmf.SelectedCrew.Count > 0)
            {
                foreach (var item in vmf.SelectedCrew)
                {
                    tempVM.AddRange(VM.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();

            if (vmf.SelectedJobTitle.Count > 0)
            {
                foreach (var item in vmf.SelectedJobTitle)
                {
                    tempVM.AddRange(VM.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            if (vmf.SelectedGrade.Count > 0)
            {
                foreach (var item in vmf.SelectedGrade)
                {
                    tempVM.AddRange(VM.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            if (vmf.SelectedPosition.Count > 0)
            {
                foreach (var item in vmf.SelectedPosition)
                {
                    tempVM.AddRange(VM.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                VM = tempVM.ToList();
            }
            else
                tempVM = VM.ToList();
            tempVM.Clear();
            foreach (var item in VM)
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PEmployeeID;
                obj.FilterName = item.EmployeeName + "(" + item.OUCommonName + ")";
                obj.IsSlected = vmf.SelectedEmployee.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                obj.FieldOneID = item.CompanyID;
                //obj.FieldOneName = item.CompanyName;
                obj.FieldTwoID = item.LocationID;
                obj.FieldTwoName = item.LocationName;
                obj.FieldThreeName = item.OEmpID;
                vm.Add(obj);
            }
            vm = CompanyVMHelper(vm, vmf);
            vm = LocationVMHelper(vm, vmf);
            return View(vm);
        }
        public ActionResult EmployementType()
        {
            List<VMFilterAttribute> vm = new List<VMFilterAttribute>();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            foreach (var item in DDService.GetEmploymentType(LoggedInUser).ToList())
            {
                VMFilterAttribute obj = new VMFilterAttribute();
                obj.FilterID = item.PEmploymentTypeID;
                obj.FilterName = item.EmploymentTypeName;
                obj.IsSlected = vmf.SelectedEmployementType.Where(aa => aa.FilterID == obj.FilterID).Count() > 0 ? true : false;
                //obj.FieldOneID = item.CompanyID;
                //obj.FieldOneName = item.CompanyName;
                vm.Add(obj);
            }
            vm = CompanyVMHelper(vm, vmf);
            return View(vm);
        }
        public ActionResult ReportAttendance()
        {
            return View();
        }
        #endregion

        #region -- Load Common ActionResults--
        [HttpPost]
        public ActionResult SaveValueInSession(int? id, string name, string type)
        {
            Session["FiltersModel"] = ReportFilterManager.AddValuesInSession((int)id, name, type, Session["FiltersModel"] as VMSelectedFilter);
            return Json("OK");
        }
        [HttpPost]
        public ActionResult RemoveValueFromSession(int? id, string type)
        {
            Session["FiltersModel"] = ReportFilterManager.RemoveValuesFromSession((int)id, type, Session["FiltersModel"] as VMSelectedFilter);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult SaveValueInSessionString(string id, string name, string type)
        {
            Session["FiltersModel"] = ReportFilterManager.AddValuesInSessionString(id, name, type, Session["FiltersModel"] as VMSelectedFilter);
            return Json("OK");
        }
        [HttpPost]
        public ActionResult RemoveValueFromSessionString(string id, string type)
        {
            Session["FiltersModel"] = ReportFilterManager.RemoveValuesFromSessionString(id, type, Session["FiltersModel"] as VMSelectedFilter);
            return Json("OK");
        }
        [HttpPost]
        public ActionResult SaveAllValueInSession(string type)
        {
            switch (type)
            {
                case "CommonOU":
                    foreach (var item in DDService.GetOUCommon().ToList())
                    {
                        Session["FiltersModel"] = ReportFilterManager.AddValuesInSession((int)item.POUCommonID, item.OUCommonName, type, Session["FiltersModel"] as VMSelectedFilter);
                    }
                    break;
            }
            return Json("OK");
        }
        [HttpPost]
        public ActionResult RemoveAllValueFromSession(string type)
        {
            Session["FiltersModel"] = ReportFilterManager.RemoveAllValuesFromSession(type, Session["FiltersModel"] as VMSelectedFilter);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult SaveSelectedDateInSession(string dateStart, string dateEnd)
        {
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            vmf.DateFrom = Convert.ToDateTime(dateStart);
            vmf.DateTo = Convert.ToDateTime(dateEnd);
            Session["FiltersModel"] = vmf;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Helper Methods
        private List<VMFilterAttribute> CompanyVMHelper(List<VMFilterAttribute> vm, VMSelectedFilter vmf)
        {
            List<VMFilterAttribute> tempVM = new List<VMFilterAttribute>();
            if (vmf.SelectedCompany.Count > 0)
            {
                foreach (var item in vmf.SelectedCompany)
                {
                    tempVM.AddRange(vm.Where(aa => aa.FieldOneID == item.FilterID).ToList());
                }
                vm = tempVM.ToList();
            }
            else
                tempVM = vm.ToList();
            return tempVM;
        }
        private List<VMFilterAttribute> LocationVMHelper(List<VMFilterAttribute> vm, VMSelectedFilter vmf)
        {
            List<VMFilterAttribute> tempVM = new List<VMFilterAttribute>();
            if (vmf.SelectedLocation.Count > 0)
            {
                foreach (var item in vmf.SelectedLocation)
                {
                    tempVM.AddRange(vm.Where(aa => aa.FieldTwoID == item.FilterID).ToList());
                }
                vm = tempVM.ToList();
            }
            else
                tempVM = vm.ToList();
            return tempVM;
        }
        #endregion

    }
}