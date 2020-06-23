using AppESSP.Controllers;
using ESSPCORE.Attendance;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class DDController : BaseController
    {
        IEntityService<VHR_EmployeeProfile> EmployeeService;
        IEntityService<LeaveQuotaYear> LeaveQuotaYearService;
        IDDService DDService;
        public DDController(IEntityService<VHR_EmployeeProfile> employeeService,
      IDDService ddService, IEntityService<LeaveQuotaYear> leaveQuotaYearService)
        {
            EmployeeService = employeeService;
            LeaveQuotaYearService = leaveQuotaYearService;
            DDService = ddService;
        }

        public ActionResult GetEmployeeInfo(string EmpNo)
        {
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificData = c => c.OEmpID == EmpNo;
            List<VHR_EmployeeProfile> list = EmployeeService.GetIndexSpecific(SpecificData);
            if (list.Count() > 0)
            {
                //"EName").value = data[0].EmployeeName;
                //document.getElementById("EDesignation").value = data[0].EmployeeName;
                //document.getElementById("EOU").value = data[0].EmployeeName;
                //document.getElementById("EType").value = data[0].EmployeeName;
                //document.getElementById("EGrade").value = data[0].EmployeeName;
                //document.getElementById("EDOJ")
                if (list.FirstOrDefault().DOJ != null)
                {
                    var subCategoryToReturn = list.Select(A => new
                    {
                        EName = A.EmployeeName,
                        EDesignation = A.DesignationName,
                        EOU = A.OUName,
                        EDOJ = A.DOJ.Value.ToString("dd-MMM-yyyy")
                    });

                    return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var subCategoryToReturn = list.Select(A => new
                    {
                        EName = A.EmployeeName,
                        EDesignation = A.DesignationName,
                        EOU = A.OUName,
                        //EDOJ = A.DOJ.Value.ToString("dd-MMM-yyyy")
                    });
                    return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json("No", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReplacementEmployeeInfo(string ReplacementEmpNo)
        {
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificData = c => c.OEmpID == ReplacementEmpNo;
            List<VHR_EmployeeProfile> list = EmployeeService.GetIndexSpecific(SpecificData);
            if (list.Count() > 0)
            {
                //"EName").value = data[0].EmployeeName;
                //document.getElementById("EDesignation").value = data[0].EmployeeName;
                //document.getElementById("EOU").value = data[0].EmployeeName;
                //document.getElementById("EType").value = data[0].EmployeeName;
                //document.getElementById("EGrade").value = data[0].EmployeeName;
                //document.getElementById("EDOJ")
                var subCategoryToReturn = list.Select(S => new
                {
                    ERepName = S.EmployeeName,
                    ERepDesignation = S.DesignationName,
                });
                return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);
            }
            else
                return Json("No", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpLeavePolicies(string EmpNo)
        {
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificData = c => c.OEmpID == EmpNo;
            List<VHR_EmployeeProfile> list = EmployeeService.GetIndexSpecific(SpecificData);
            if (list.Count() > 0)
            {
                string CLPolicy = "";
                string ALPolicy = "";
                string SLPolicy = "";
                string CPLPolicy = "";
                if (list[0].CLPolicyID != null)
                {
                    CLPolicy = "Casual: " + list[0].CLPolicyName;

                }
                else
                    CLPolicy = "Casual: No Policy Defined";
                if (list[0].SLPolicyID != null)
                {
                    SLPolicy = "Sick: " + list[0].SLPolicyName;

                }
                else
                    SLPolicy = "Sick: No Policy Defined";
                if (list[0].ALPolicyID != null)
                {
                    ALPolicy = "Annual: " + list[0].ALPolicyName;

                }
                else
                    ALPolicy = "Annual: No Policy Defined";
                if (list[0].CPLPolicyID != null)
                {
                    CPLPolicy = "CPL: " + list[0].CPLPolicyName;

                }
                else
                    CPLPolicy = "CPL: No Policy Defined";

                return Json(CLPolicy + "@" + SLPolicy + "@" + ALPolicy + "@" + CPLPolicy, JsonRequestBehavior.AllowGet);

            }
            else
                return Json("No", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpLeaveBalance(string EmpNo, int? FinancialYearID)
        {
            Expression<Func<VHR_EmployeeProfile, bool>> SpecificData = c => c.OEmpID == EmpNo;
            List<VHR_EmployeeProfile> emps = EmployeeService.GetIndexSpecific(SpecificData);
            VMLeaveBalance vm = new VMLeaveBalance();
            if (emps.Count() > 0)
            {
                int EmployeeID = emps[0].PEmployeeID;
                Expression<Func<LeaveQuotaYear, bool>> SpecificData2 = c => c.EmployeeID == EmployeeID && c.FinancialYearID==FinancialYearID;
                List<LeaveQuotaYear> LvQuota = LeaveQuotaYearService.GetIndexSpecific(SpecificData2);
                //1   Annual
                //2   Casual
                //3   Sick
                //4   CPL
                if (LvQuota.Where(aa => aa.LeaveTypeID == 2).Count() > 0)
                {
                    vm.BalanceCL = LvQuota.First(aa => aa.LeaveTypeID == 2).GrandRemaining;
                    vm.TotalCL = LvQuota.First(aa => aa.LeaveTypeID == 2).GrandTotal;
                }
                if (LvQuota.Where(aa => aa.LeaveTypeID == 1).Count() > 0)
                {
                    vm.BalanceAL = LvQuota.First(aa => aa.LeaveTypeID == 1).YearlyRemaining;
                    vm.TotalAL = LvQuota.First(aa => aa.LeaveTypeID == 1).YearlyTotal;
                    vm.BalanceAccum = LvQuota.First(aa => aa.LeaveTypeID == 1).CFRemaining;
                    vm.TotalAccum = LvQuota.First(aa => aa.LeaveTypeID == 1).CFFromLastYear;
                }
                if (LvQuota.Where(aa => aa.LeaveTypeID == 3).Count() > 0)
                {
                    vm.BalanceSL = LvQuota.First(aa => aa.LeaveTypeID == 3).GrandRemaining;
                    vm.TotalSL = LvQuota.First(aa => aa.LeaveTypeID == 3).GrandTotal;
                }
                if (LvQuota.Where(aa => aa.LeaveTypeID == 4).Count() > 0)
                {
                    vm.BalanceCPL = LvQuota.First(aa => aa.LeaveTypeID == 4).GrandRemaining;
                    vm.TotalCPL = LvQuota.First(aa => aa.LeaveTypeID == 4).GrandTotal;
                }
                if (LvQuota.Where(aa => aa.LeaveTypeID == 11).Count() > 0)
                {
                    vm.BalanceEAL = LvQuota.First(aa => aa.LeaveTypeID == 11).GrandRemaining;
                    vm.TotalEAL = LvQuota.First(aa => aa.LeaveTypeID == 11).GrandTotal;
                }
                if (LvQuota.Where(aa => aa.LeaveTypeID == 12).Count() > 0)
                {
                    vm.BalanceCME = LvQuota.First(aa => aa.LeaveTypeID == 12).GrandRemaining;
                    vm.TotalCME = LvQuota.First(aa => aa.LeaveTypeID == 12).GrandTotal;
                }

                return PartialView("LeaveBalance", vm);
            }
            else
                return Json("No", JsonRequestBehavior.AllowGet);
        }
     

      
    }
}