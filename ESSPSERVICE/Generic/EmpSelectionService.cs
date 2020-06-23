using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Attendance;

namespace ESSPSERVICE.Generic
{
    public class EmpSelectionService : IEmpSelectionService
    {
        IDDService DDService;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        public EmpSelectionService(IDDService dDService, IGetSpecificEmployeeService getSpecificEmployeeService)
        {
            DDService = dDService;
            GetSpecificEmployeeService = getSpecificEmployeeService;
        }
        public VMEmpSelection GetStepOne(VMLoggedUser LoggedInUser)
        {
            VMEmpSelection obj = new VMEmpSelection();
            obj.Company = DDService.GetCompany(LoggedInUser).OrderBy(aa => aa.CompanyName).ToList();
            obj.OUCommon = DDService.GetOUCommon(LoggedInUser).OrderBy(aa => aa.OUCommonName).ToList();
            obj.OrganizationalUnit = DDService.GetOU(LoggedInUser).OrderBy(aa => aa.OUName).ToList();
            obj.EmploymentType = DDService.GetEmploymentType(LoggedInUser).OrderBy(aa => aa.EmploymentTypeName).ToList();
            obj.Location = DDService.GetLocation(LoggedInUser).OrderBy(aa => aa.LocationName).ToList();
            obj.Grade = DDService.GetGrade(LoggedInUser).OrderBy(aa => aa.GradeName).ToList();
            obj.JobTitle = DDService.GetJobTitle(LoggedInUser).OrderBy(aa => aa.JobTitleName).ToList();
            obj.Designation = DDService.GetDesignation(LoggedInUser).OrderBy(aa => aa.DesignationName).ToList();
            obj.Crew = DDService.GetCrew(LoggedInUser).OrderBy(aa => aa.CrewName).ToList();
            obj.Shift = DDService.GetShift(LoggedInUser).OrderBy(aa => aa.ShiftName).ToList();
            return obj;
        }

        public VMEmpSelection GetStepTwo(int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, string EmpNo, VMLoggedUser LoggedInUser)
        {
            VMEmpSelection obj = new VMEmpSelection();
            obj.EmpNo = EmpNo;
            List<VHR_EmployeeProfile> ViewEmps = GetSpecificEmployeeService.GetSpecificEmployees(LoggedInUser);
            List<VHR_EmployeeProfile> tempEmps = new List<VHR_EmployeeProfile>();
            if (obj.EmpNo != "" && obj.EmpNo != null)
            {

                ViewEmps = ViewEmps.Where(aa => aa.OEmpID == obj.EmpNo).ToList();
                obj.Criteria = "E";
                obj.CriteriaName = "Employee";
            }
            else
            {
                if (SelectedCompanyIds != null)
                {
                    foreach (var item in SelectedCompanyIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.CompanyID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "CC";
                    obj.CriteriaName = "Company";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedOUCommonIds != null)
                {
                    foreach (var item in SelectedOUCommonIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.OUCommonID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "M";
                    obj.CriteriaName = "Common OU";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedOUIds != null)
                {
                    foreach (var item in SelectedOUIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.OUID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "O";
                    obj.CriteriaName = "OU";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedEmploymentTypeIds != null)
                {
                    foreach (var item in SelectedEmploymentTypeIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.EmploymentTypeID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "T";
                    obj.CriteriaName = "Employment Type";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedLocationIds != null)
                {
                    foreach (var item in SelectedLocationIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.LocationID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "L";
                    obj.CriteriaName = "Location";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedGradeIds != null)
                {
                    foreach (var item in SelectedGradeIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.GradeID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "G";
                    obj.CriteriaName = "Grade";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedJobTitleIds != null)
                {
                    foreach (var item in SelectedJobTitleIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.JobTitleID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "J";
                    obj.CriteriaName = "Job Title";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedDesignationIds != null)
                {
                    foreach (var item in SelectedDesignationIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.DesigationID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "D";
                    obj.CriteriaName = "Designation";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedCrewIds != null)
                {
                    foreach (var item in SelectedCrewIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.CrewID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "C";
                    obj.CriteriaName = "Crew";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
                if (SelectedShiftIds != null)
                {
                    foreach (var item in SelectedShiftIds)
                    {
                        short id = Convert.ToInt16(item);
                        tempEmps.AddRange(ViewEmps.Where(aa => aa.ShiftID == id).ToList());
                    }
                    ViewEmps = tempEmps.ToList();
                    obj.Criteria = "S";
                    obj.CriteriaName = "Shift";
                    tempEmps.Clear();
                }
                else
                {
                    tempEmps = ViewEmps.ToList();
                    tempEmps.Clear();
                }
            }
            obj.Employee = ViewEmps.ToList();
            return obj;
        }
    }
}
