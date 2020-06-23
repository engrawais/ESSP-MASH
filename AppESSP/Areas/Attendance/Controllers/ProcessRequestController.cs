using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class ProcessRequestController : Controller
    {
        // GET: Attendance/ProcessRequest
        IEntityService<ProcessRequest> ProcessRequestService;
        IEntityService<Employee> EmployeeService;
        IDDService DDService;
        public ProcessRequestController(IEntityService<ProcessRequest> processRequestService, IEntityService<Employee> employeeService, IDDService dDService)
        {
            ProcessRequestService = processRequestService;
            DDService = dDService;
            EmployeeService = employeeService;
        }
        public ActionResult Index()
        {
            Expression<Func<ProcessRequest, bool>> SpecificEntries = c => c.ProcessingDone == false;
            List<ProcessRequest> list = ProcessRequestService.GetIndexSpecific(SpecificEntries).OrderByDescending(a => a.CreatedDate).ToList(); ;
            return View(list);
        }
        public ActionResult Create()
        {

            try
            {
                //QueryBuilder qb = new QueryBuilder();
                //String query = qb.QueryForCompanyViewLinq(LoggedInUser);
                ViewBag.PeriodTag = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Daily", Value = "D"},
                new SelectListItem { Selected = false, Text = "Monthly", Value = "M"},
                new SelectListItem { Selected = false, Text = "Summary", Value = "S"},

            }, "Value", "Text", 1);
                ViewBag.CriteriaID = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Company", Value = "C"},
                new SelectListItem { Selected = false, Text = "Location", Value = "L"},
                new SelectListItem { Selected = false, Text = "Employee", Value = "E"},

            }, "Value", "Text", 1);
                ViewBag.ProcessCats = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Yes", Value = "1"},
                new SelectListItem { Selected = false, Text = "No", Value = "0"},

            }, "Value", "Text", 1);
                ProcessRequest obj = new ProcessRequest();
                HelperMethod(obj);
                return View(obj);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Create(ProcessRequest obj)
        {
            try
            {

                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                switch (obj.Criteria)
                {
                    case "C":
                        obj.Criteria = "C";

                        break;
                    case "L":
                        obj.Criteria = "L";
                        break;
                    case "A":
                        obj.Criteria = "A";
                        break;
                    case "E":
                        {
                            obj.Criteria = "E";
                            obj.ProcessCat = false;
                            string _EmpNo = Request.Form["EmpNo"].ToString();
                            List<VHR_EmployeeProfile> empss = new List<VHR_EmployeeProfile>();
                            Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.OEmpID == obj.EmpNo;
                            empss = DDService.GetSpecificEmployee(SpecificEntries).ToList();
                            if (empss.Count() == 0)
                                ModelState.AddModelError("EmpNo", "Employee number does not exist");
                            else
                            {
                                obj.EmpID = empss.First().PEmployeeID;
                                obj.EmpNo = empss.First().OEmpID;
                            }
                        }
                        break;
                }

                obj.UserID = LoggedInUser.UserEmpID;
                obj.ProcessCat = false;
                obj.ProcessingDone = false;
                //att_processrequest.WhenToProcess = DateTime.Today;
                obj.CreatedDate = DateTime.Now;
                if (obj.DateTo < obj.DateFrom) 
                    ModelState.AddModelError("DateTo","End date can never be less tha Start Date." );
                if (obj.DateTo > AppAssistant.MaxDate)
                    ModelState.AddModelError("DateTo", "Date cannot be greater than " + AppAssistant.MaxDate.ToString("dd-MM-yyyy"));
                //if (obj.DateFrom < AppAssistant.MinDate)
                //    ModelState.AddModelError("DateTo", "Date cannot be less than " + AppAssistant.MinDate.ToString("dd-MM-yyyy"));

                if (ModelState.IsValid)
                {
                    obj.SystemGenerated = false;
                    ProcessRequestService.PostCreate(obj);
                    //if (obj.Criteria == "E" && obj.PeriodTag == "D")
                    //{
                    //    //DateTime dts = new DateTime(obj.DateFrom.Year, obj.DateFrom.Month, 1);
                    //    //DateTime dte = DateTime.Today;
                    //    //if (DateTime.Today.Month != obj.DateFrom.Month)
                    //    //{
                    //    //    int daysInMonth = System.DateTime.DaysInMonth(obj.DateFrom.Year, obj.DateFrom.Month);
                    //    //    dte = new DateTime(obj.DateFrom.Year, obj.DateFrom.Month, daysInMonth);

                    //    //}
                    //    //ProcessSupportFunc.ProcessAttendanceRequestMonthly((DateTime)dts, (DateTime)dte, obj.EmpID.ToString());
                    //}
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                HelperMethod(obj);
                return PartialView("Create", obj);
            }
            catch (Exception ex)

            {

                HelperMethod(obj);
                return PartialView("Create", obj);
            }

        }
        #region -- Private Method--
        private void HelperMethod(ProcessRequest obj)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.LocationID = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
        }
        #endregion
    }
}