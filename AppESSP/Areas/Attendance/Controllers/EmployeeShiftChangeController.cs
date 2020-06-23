using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class EmployeeShiftChangeController : Controller
    {
        // GET: Attendance/EmployeeShiftChange
        IEntityService<ShiftChangedEmp> ShiftChangedEmpService;
        IEntityService<Employee> EmployeeService;
        IEntityService<DeviceData> DeviceDataService;
        IDDService  DDService;
        public EmployeeShiftChangeController(IEntityService<ShiftChangedEmp> shiftchangedempService, 
            IEntityService<DeviceData> deviceDataService, IDDService  dDService,
            IEntityService<Employee> employeeService)
        {
            ShiftChangedEmpService = shiftchangedempService;
            DeviceDataService = deviceDataService;
            EmployeeService = employeeService;
            DDService = dDService;
        }
        public ActionResult Index()
        {
            List<ShiftChangedEmp> list = ShiftChangedEmpService.GetIndex();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            HelperMethod(new ShiftChangedEmp());
            return View();
        }
        [HttpPost]
        public ActionResult Create(ShiftChangedEmp obj)
        {
            try
            {
                string _EmpNo = Request.Form["EmpNo"].ToString();
                List<ShiftChangedEmp> empShiftTimes = new List<ShiftChangedEmp>();
                Expression<Func<Employee, bool>> SpecificEntries = c => c.OEmpID == _EmpNo;

                List<Employee> _emp = EmployeeService.GetIndexSpecific(SpecificEntries);
                if (_emp.Count == 0)
                {
                    ModelState.AddModelError("StartDate", "Emp No not exist");
                }
                else
                {
                    int empID = _emp.FirstOrDefault().PEmployeeID;
                    Expression<Func<ShiftChangedEmp, bool>> SpecificEntries2 = c => c.EmpID == empID;
                    empShiftTimes = ShiftChangedEmpService.GetIndexSpecific(SpecificEntries2);
                }
                if (_emp.FirstOrDefault().ShiftID == obj.ShiftID)
                {
                    ModelState.AddModelError("StartDate", "Duplicate Shift, This employee has assigned same shift in employee profile.");
                }
                if (obj.StartDate != null && obj.EndDate != null)
                {
                    if (obj.EndDate == obj.StartDate)
                    { }
                    else
                    {
                        if (obj.EndDate <= obj.StartDate)
                        {
                            ModelState.AddModelError("StartDate", "End Date must be greated than start date.");
                        }
                    }
                }
                empShiftTimes = empShiftTimes.OrderByDescending(aa => aa.StartDate).ToList();
                    if (empShiftTimes.Count > 0)
                    {
                        //if (empShiftTimes[0].EndDate == null)
                        //{
                        empShiftTimes[0].EndDate = obj.StartDate.Value.AddDays(-1);
                        ShiftChangedEmpService.PostEdit(empShiftTimes[0]);
                        return RedirectToAction("Index");
                        //}
                    }
                
                if (ValidationForSCE(empShiftTimes, obj))
                {
                    ModelState.AddModelError("StartDate", "Date lies between old ranges");
                }
                if (ModelState.IsValid)
                {
                    obj.EmpID = Convert.ToInt32(Request.Form["EmpNo"]);
                    obj.EmpID = _emp.FirstOrDefault().PEmployeeID;
                    obj.DateCreated = DateTime.Now;
                    ShiftChangedEmpService.PostEdit(obj);
                    return RedirectToAction("Index");
                }

                HelperMethod(obj);
                return PartialView("Create", obj);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ShiftChangedEmp obj = ShiftChangedEmpService.GetEdit((int)id);
            HelperMethod(obj);
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(ShiftChangedEmp obj)
        {
            Expression<Func<Employee, bool>> SpecificEntries = c => c.PEmployeeID == obj.EmpID;
            List<Employee> _emp = EmployeeService.GetIndexSpecific(SpecificEntries);
            int empID = _emp.FirstOrDefault().PEmployeeID;
            Expression<Func<ShiftChangedEmp, bool>> SpecificEntries2 = aa => aa.EmpID == obj.EmpID && aa.PEmpShiftChangeID != obj.PEmpShiftChangeID;
            List<ShiftChangedEmp> empShiftTimes = ShiftChangedEmpService.GetIndexSpecific(SpecificEntries2);
            if (_emp.FirstOrDefault().ShiftID == obj.ShiftID)
            {
                ModelState.AddModelError("StartDate", "Duplicate Shift, This employee has assigned same shift in employee profile.");
            }
            if (ValidationForSCE(empShiftTimes, obj))
            {
                ModelState.AddModelError("StartDate", "Date lies between old ranges");
            }
            if (ModelState.IsValid)
            {
                ShiftChangedEmpService.PostEdit(obj);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            HelperMethod(obj);
            return PartialView("Edit", obj);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return PartialView(ShiftChangedEmpService.GetDelete((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Delete(ShiftChangedEmp obj)
        {
            ShiftChangedEmpService.PostDelete(obj);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #region -- Private Method--
        private void HelperMethod(ShiftChangedEmp obj)
        {
            ViewBag.ShiftID = new SelectList(DDService.GetShift().ToList().OrderBy(aa => aa.ShiftName).ToList(), "PShiftID", "ShiftName",obj.ShiftID);
        }

        private bool ValidationForSCE(List<ShiftChangedEmp> empShiftTimes, ShiftChangedEmp att_shiftchangeemp)
        {
            try
            {
                bool check = false;
                foreach (var item in empShiftTimes)
                {
                    DateTime dts = item.StartDate.Value;
                    DateTime dte = DateTime.Now;
                    if (item.EndDate != null)
                        dte = item.EndDate.Value;
                    while (dts <= dte)
                    {
                        if (att_shiftchangeemp.StartDate == dts)
                            check = true;

                        dts = dts.AddDays(1);
                    }
                }
                return check;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}