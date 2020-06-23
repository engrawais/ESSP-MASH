using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Helper;

namespace ESSPSERVICE.Generic
{
    public class GetSpecificEmployeeService : IGetSpecificEmployeeService
    {
        IRepository<VHR_EmployeeProfile> VHREmployeeProfileReporsitory;
        IRepository<VAT_DailyAttendance> VATDailyAttendance;
        IRepository<VAT_DailyOvertime> VATDailyOvertime;

        public GetSpecificEmployeeService(IRepository<VHR_EmployeeProfile> vHREmployeeProfileReporsitory,
            IRepository<VAT_DailyAttendance> vATDailyAttendance, IRepository<VAT_DailyOvertime> vATDailyOvertime)
        {
            VATDailyOvertime = vATDailyOvertime;
            VHREmployeeProfileReporsitory = vHREmployeeProfileReporsitory;
            VATDailyAttendance = vATDailyAttendance;
        }
        public List<VHR_EmployeeProfile> GetSpecificEmployees(VMLoggedUser vmf)
        {
            List<VHR_EmployeeProfile> vmList = new List<VHR_EmployeeProfile>();
            if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                vmList = VHREmployeeProfileReporsitory.GetAll().Where(aa => aa.Status == "Active" && aa.CompanyID != 3).ToList();
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (vmf.UserLoctions != null)
                {
                    foreach (var userLocaion in vmf.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        vmList.AddRange(VHREmployeeProfileReporsitory.FindBy(SpecificEntries).Where(aa => aa.Status == "Active").OrderBy(aa => aa.JobTitleID).ToList());
                    }
                }
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (vmf.UserDepartments != null)
                {
                    foreach (var userDepartment in vmf.UserDepartments)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.OUCommonID == userDepartment.DepartmentID;
                        vmList.AddRange(VHREmployeeProfileReporsitory.FindBy(SpecificEntries).Where(aa => aa.Status == "Active").OrderBy(aa => aa.JobTitleID).ToList());
                    }
                }
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                vmList = EmployeeLM.GetReportingEmployees(VHREmployeeProfileReporsitory.GetAll().Where(aa => aa.Status == "Active").OrderBy(aa => aa.JobTitleID).ToList(), vmf);
            }
            return vmList;
        }
        public List<VAT_DailyAttendance> GetSpecificAttendance(VMLoggedUser vmf, DateTime date)
        {
            List<VAT_DailyAttendance> vmList = new List<VAT_DailyAttendance>();
            if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate == date;
                vmList = VATDailyAttendance.FindBy(SpecificEntries);
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (vmf.UserLoctions != null)
                {
                    foreach (var userLocaion in vmf.UserLoctions)
                    {
                        Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && c.AttDate == date;
                        vmList.AddRange(VATDailyAttendance.FindBy(SpecificEntries));
                    }
                }
            }
            //else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            //{
            //    vmList = EmployeeLM.GetReportingEmployees(VHREmployeeProfileReporsitory.GetAll(), vmf);
            //}
            return vmList;
        }
        public List<VAT_DailyAttendance> GetSpecificAbsentAttendance(VMLoggedUser vmf, DateTime dateFrom, DateTime DateTo)
        {
            List<VAT_DailyAttendance> vmList = new List<VAT_DailyAttendance>();
            if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= dateFrom && c.AttDate <= DateTo && c.DutyCode == "D" && c.Status == "Active";
                vmList = VATDailyAttendance.FindBy(SpecificEntries);
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (vmf.UserLoctions != null)
                {
                    foreach (var userLocaion in vmf.UserLoctions)
                    {
                        Expression<Func<VAT_DailyAttendance, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && c.AttDate >= dateFrom && c.AttDate <= DateTo && c.DutyCode == "D" && c.Status == "Active";
                        vmList.AddRange(VATDailyAttendance.FindBy(SpecificEntries));
                    }
                }
            }
            //else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            //{
            //    vmList = EmployeeLM.GetReportingEmployees(VHREmployeeProfileReporsitory.GetAll(), vmf);
            //}
            return vmList;
        }
        public List<VAT_DailyOvertime> GetSpecificDailyOT(VMLoggedUser vmf)
        {
            List<VAT_DailyOvertime> vmList = new List<VAT_DailyOvertime>();
            if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                //Expression<Func<VAT_DailyOvertime, bool>> SpecificEntries = c => c.AttDate == date;
                //vmList = VATDailyAttendance.FindBy(SpecificEntries);
                vmList = VATDailyOvertime.GetAll();
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (vmf.UserLoctions != null)
                {
                    foreach (var userLocaion in vmf.UserLoctions)
                    {
                        Expression<Func<VAT_DailyOvertime, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        vmList.AddRange(VATDailyOvertime.FindBy(SpecificEntries));
                    }
                }
            }
            else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (vmf.UserDepartments != null)
                {
                    foreach (var userDepartment in vmf.UserDepartments)
                    {
                        Expression<Func<VAT_DailyOvertime, bool>> SpecificEntries = c => c.OUCommonID == userDepartment.DepartmentID;
                        vmList.AddRange(VATDailyOvertime.FindBy(SpecificEntries));
                    }
                }
            }
            //else if (vmf.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            //{
            //    vmList = EmployeeLM.GetReportingEmployees(VHREmployeeProfileReporsitory.GetAll(), vmf);
            //}
            return vmList;
        }
    }
}
