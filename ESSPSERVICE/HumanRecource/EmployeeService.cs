using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using ESSPREPO.Generic;
using System.Linq.Expressions;
using ESSPCORE.HumanResource;
using ESSPCORE.Reporting;
using ESSPCORE.Common;
using ESSPSERVICE.Helper;

namespace ESSPSERVICE.HumanRecource
{
    /// <summary>
    /// Implemetation of I5EmployeeService
    /// </summary>
    /// <remarks></remarks>
    public class EmployeeService : IEmployeeService
    {
        IDDService DDService;
        IRepository<Employee> EmployeeRepository;
        IRepository<EmployeeCrewChange> EmployeeCrewChangeRepository;
        IRepository<EmployeeImage> EmployeeImageRepository;
        IRepository<VHR_EmployeeProfile> VHREmployeeProfileReporsitory;
        IGetSpecificEmployeeService GetSpecificEmployeeService;
        IUnitOfWork UnitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<VHR_EmployeeProfile> vHREmployeeProfileReporsitory,
            IRepository<Employee> employeeRepository, IRepository<EmployeeImage> employeeImageRepository, IGetSpecificEmployeeService getSpecificEmployeeService,
            IRepository<EmployeeCrewChange> employeeCrewChangeRepository)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            EmployeeRepository = employeeRepository;
            VHREmployeeProfileReporsitory = vHREmployeeProfileReporsitory;
            EmployeeImageRepository = employeeImageRepository;
            GetSpecificEmployeeService = getSpecificEmployeeService;
            EmployeeCrewChangeRepository = employeeCrewChangeRepository;
        }
        public VMEmployee GetEdit(int id)
        {
            // Instantiating the view model of VMEmployee
            VMEmployee vmEmployee = new VMEmployee();

            //Getting the  the Single entry of Employee from Employee Table

            Employee employeeObj = EmployeeRepository.GetSingle(id);
            vmEmployee.PEmployeeID = employeeObj.PEmployeeID;
            vmEmployee.EmployeeName = employeeObj.EmployeeName;
            vmEmployee.OfficialEmail = employeeObj.OfficialEmailID;
            vmEmployee.TelephoneNo = employeeObj.TelephoneNo;
            vmEmployee.LocationID = employeeObj.LocationID;
            vmEmployee.OEmpID = employeeObj.OEmpID;
            vmEmployee.EmployeeName = employeeObj.EmployeeName;
            vmEmployee.LineManagerID = employeeObj.LineManagerID;
            vmEmployee.ShiftID = employeeObj.ShiftID;
            vmEmployee.CardNo = employeeObj.CardNo;
            vmEmployee.CrewID = employeeObj.CrewID;
            if (employeeObj.Crew != null)
                vmEmployee.OldCrewName = employeeObj.Crew.CrewName;
            vmEmployee.DepartmentID = employeeObj.COUID;
            vmEmployee.SectionID = employeeObj.OUID;
            vmEmployee.JobTitleID = employeeObj.JobTitleID;
            vmEmployee.DesigationID = employeeObj.DesigationID;
            vmEmployee.GradeID = employeeObj.GradeID;
            vmEmployee.LocationID = employeeObj.LocationID;
            vmEmployee.Gender = employeeObj.Gender;
            if (employeeObj.DOJ != null)
            {
                vmEmployee.DOJ = (DateTime)employeeObj.DOJ;
            }
            if (employeeObj.DateOfBirth != null)
            {
                vmEmployee.DateOfBirth = (DateTime)employeeObj.DateOfBirth;
            }
            vmEmployee.Address = employeeObj.Address;
            vmEmployee.Status = employeeObj.Status;
            vmEmployee.ValidDate = employeeObj.ValidDate;
            vmEmployee.FaceTemp = employeeObj.FaceTemp;
            vmEmployee.FPTemp = employeeObj.FPTemp;
            vmEmployee.FPID = employeeObj.FPID;
            vmEmployee.ProcessAttendance = employeeObj.ProcessAttendance;
            vmEmployee.HasOneStep = employeeObj.HasOneStep;
            vmEmployee.OTPolicyID = employeeObj.OTPolicyID;
            vmEmployee.OfficialEmail = employeeObj.OfficialEmailID;
            vmEmployee.EmploymentTypeID = employeeObj.EmploymentTypeID;
            vmEmployee.ALPolicyID = employeeObj.ALPolicyID;
            vmEmployee.SLPolicyID = employeeObj.SLPolicyID;
            vmEmployee.CLPolicyID = employeeObj.CLPolicyID;
            vmEmployee.CPLPolicyID = employeeObj.CPLPolicyID;
            vmEmployee.EALPolicyID = employeeObj.EALPolicyID;
            vmEmployee.CMEPolicyID = employeeObj.CMEPolicyID;
            //Getting the Crew history of employee of the Employee when he changes the crew of the employee.

            Expression<Func<EmployeeCrewChange, bool>> SpecificEntries = c => c.CrewID == employeeObj.CrewID && c.EmpID == employeeObj.PEmployeeID;
            List<EmployeeCrewChange> dbEmployeeCrewChanges = EmployeeCrewChangeRepository.FindBy(SpecificEntries).OrderByDescending(aa => aa.StartDate).ToList();

            //If the count of the the crew change history is more than zero then it gets the Start Date and End date of previous crew
            if (dbEmployeeCrewChanges.Count > 0)
            {
                vmEmployee.CrewStartDate = dbEmployeeCrewChanges[0].StartDate;
                vmEmployee.CrewEndDate = DateTime.Now;
            }
            // If count is equal to zero then crew start date will be equal to Date of Joining
            else
            {
                if (employeeObj.DOJ != null)
                    vmEmployee.CrewStartDate = employeeObj.DOJ;
                else
                    vmEmployee.CrewStartDate = DateTime.Now;
                vmEmployee.CrewEndDate = DateTime.Now;
            }
            return vmEmployee;
        }

        public byte[] GetImageFromDataBase(int id)
        {
            //Gets employee image from the database 
            Expression<Func<EmployeeImage, bool>> SpecificEntries = c => c.PEmpID == id;
            List<EmployeeImage> images = EmployeeImageRepository.FindBy(SpecificEntries);

            if (images.Count > 0)
                return images[0].EmpPic;
            else
                return null;
        }

        public List<VHR_EmployeeProfile> GetIndex(VMLoggedUser vmf)
        {
            //Gets the employee list showing them location wise through get specific employee service
            return GetSpecificEmployeeService.GetSpecificEmployees(vmf);
        }
        public void PostCreate(VMEmployee obj, VMLoggedUser LoggedInUser)
        {
            // Gets the single emoployee from the database 
            Employee employeeObj = new Employee();
            employeeObj.OEmpID = obj.OEmpID;
            employeeObj.EmployeeName = obj.EmployeeName;
            employeeObj.FPID = Convert.ToInt32(obj.OEmpID);
            employeeObj.ShiftID = obj.ShiftID;
            employeeObj.CardNo = obj.CardNo;
            employeeObj.JobTitleID = null;
            employeeObj.GradeID = 13;
            employeeObj.CompanyID = 1;
            employeeObj.LocationID = 211;
            employeeObj.OUID = null;
            employeeObj.COUID = null;
            employeeObj.DesigationID = null;
            employeeObj.DateOfBirth = obj.DateOfBirth;
            employeeObj.DOJ = obj.DOJ;
            employeeObj.FatherName = obj.FatherName;
            employeeObj.CNIC = obj.CNIC;
            employeeObj.EmploymentTypeID = null;
            employeeObj.CrewID = obj.CrewID;
            employeeObj.Status = "Active";
            employeeObj.Gender = obj.Gender;
            employeeObj.CNIC = obj.CNIC;
            employeeObj.ProcessAttendance = true;
            employeeObj.HasOneStep = true;
            EmployeeRepository.Add(employeeObj);
            EmployeeRepository.Save();
            //UnitOfWork.Commit();
        }
        public void PostEdit(VMEmployee obj, VMLoggedUser LoggedInUser)
        {
            // Gets the single emoployee from the database 
            Employee employeeObj = EmployeeRepository.GetSingle(obj.PEmployeeID);
            //If the changing Crew is not equal to the crew id already in the database then system update the the previous value with the new one 
            if (employeeObj.CrewID != null && employeeObj.CrewID != obj.CrewID)
            {
                //Update last entry
                Expression<Func<EmployeeCrewChange, bool>> SpecificEntries = c => c.CrewID == employeeObj.CrewID && c.EmpID == employeeObj.PEmployeeID;
                List<EmployeeCrewChange> dbEmployeeCrewChanges = EmployeeCrewChangeRepository.FindBy(SpecificEntries).OrderByDescending(aa => aa.StartDate).ToList();
                if (dbEmployeeCrewChanges.Count > 0)
                {
                    dbEmployeeCrewChanges[0].EndDate = obj.CrewEndDate;
                    dbEmployeeCrewChanges[0].StartDate = obj.CrewStartDate;
                    EmployeeCrewChangeRepository.Edit(dbEmployeeCrewChanges[0]);
                    EmployeeCrewChangeRepository.Save();
                }
                //Save Employee Crew History New
                EmployeeCrewChange dbEmployeeCrewChange = new EmployeeCrewChange();
                dbEmployeeCrewChange.CreatedBy = LoggedInUser.PUserID;
                dbEmployeeCrewChange.CreatedDate = DateTime.Now;
                dbEmployeeCrewChange.CrewID = obj.CrewID;
                dbEmployeeCrewChange.EmpID = obj.PEmployeeID;
                if (dbEmployeeCrewChanges.Count > 0)
                    dbEmployeeCrewChange.StartDate = dbEmployeeCrewChanges[0].EndDate.Value.AddDays(1);
                else
                    dbEmployeeCrewChange.StartDate = DateTime.Today;
                EmployeeCrewChangeRepository.Add(dbEmployeeCrewChange);
                EmployeeCrewChangeRepository.Save();
            }
            employeeObj.LineManagerID = obj.LineManagerID;
            employeeObj.EmployeeName = obj.EmployeeName;
            employeeObj.CrewID = obj.CrewID;
            employeeObj.JobTitleID = obj.JobTitleID;
            employeeObj.DesigationID = obj.DesigationID;
            employeeObj.LocationID = 211;
            employeeObj.OUID = obj.SectionID;
            employeeObj.COUID = obj.DepartmentID;
            employeeObj.DateOfBirth = obj.DateOfBirth;
            employeeObj.ResignDate = obj.ResignDate;
            employeeObj.ValidDate = obj.ValidDate;
            employeeObj.DOJ = obj.DOJ;
            employeeObj.GradeID = obj.GradeID;
            employeeObj.Gender = obj.Gender;
            employeeObj.Status = obj.Status;
            employeeObj.TelephoneNo = obj.TelephoneNo;
            employeeObj.OfficialEmailID = obj.OfficialEmail;
            employeeObj.Address = obj.Address;
            employeeObj.EmploymentTypeID = obj.EmploymentTypeID;
            employeeObj.ShiftID = obj.ShiftID;
            employeeObj.CardNo = obj.CardNo;
            employeeObj.FPID = obj.FPID;
            employeeObj.ValidDate = obj.ValidDate;
            
            employeeObj.FaceTemp = obj.FaceTemp;
            employeeObj.FPTemp = obj.FPTemp;
            employeeObj.OfficialEmailID = obj.OfficialEmail;
            employeeObj.ProcessAttendance = obj.ProcessAttendance;
            employeeObj.HasOneStep = obj.HasOneStep;
            employeeObj.OTPolicyID = obj.OTPolicyID;
            employeeObj.ALPolicyID = obj.ALPolicyID;
            employeeObj.SLPolicyID = obj.SLPolicyID;
            employeeObj.CLPolicyID = obj.CLPolicyID;
            employeeObj.CPLPolicyID = obj.CPLPolicyID;
            employeeObj.EALPolicyID = obj.EALPolicyID;
            employeeObj.CMEPolicyID = obj.CMEPolicyID;
            EmployeeRepository.Edit(employeeObj);
            UnitOfWork.Commit();
        }

        public void SaveImageInDatabase(byte[] img, int empID)
        {
            //Gets Employee id to Upload image of specific Employee
            Expression<Func<EmployeeImage, bool>> SpecificEntries = c => c.PEmpID == empID;
            List<EmployeeImage> images = EmployeeImageRepository.FindBy(SpecificEntries);
            //If count of the images is greater than zero System update the Old image with the new one
            if (images.Count > 0)
            {
                //Instantiating the Employee image
                EmployeeImage empImage = new EmployeeImage();
                empImage.PEmpID = empID;
                empImage.EmpPic = img;
                EmployeeImageRepository.Edit(empImage);
                UnitOfWork.Commit();
            }
            else
            {
                // If there is no image of employee in the database 
                EmployeeImage empImage = new EmployeeImage();
                empImage.PEmpID = empID;
                empImage.EmpPic = img;
                EmployeeImageRepository.Add(empImage);
                UnitOfWork.Commit();
            }
        }
        public VHR_EmployeeProfile GetDetail(int id)
        {
            //Instantiating the Employee profile view 
            VHR_EmployeeProfile vmEmployeeProfile = new VHR_EmployeeProfile();
            //Getting the entry of the specific employee to his details 
            VHR_EmployeeProfile employeeObj = VHREmployeeProfileReporsitory.GetSingle(id);
            vmEmployeeProfile.PEmployeeID = employeeObj.PEmployeeID;
            vmEmployeeProfile.OEmpID = employeeObj.OEmpID;
            vmEmployeeProfile.EmployeeName = employeeObj.EmployeeName;
            vmEmployeeProfile.TelephoneNo = employeeObj.TelephoneNo;
            vmEmployeeProfile.OfficialEmailID = employeeObj.OfficialEmailID;
            vmEmployeeProfile.CrewName = employeeObj.CrewName;
            vmEmployeeProfile.LineManagerID = employeeObj.LineManagerID;
            vmEmployeeProfile.FatherName = employeeObj.CardNo;
            vmEmployeeProfile.DOJ = employeeObj.ValidDate;
            vmEmployeeProfile.DateOfBirth = employeeObj.DateOfBirth;
            vmEmployeeProfile.Gender = employeeObj.Gender;
            vmEmployeeProfile.GradeName = employeeObj.GradeName;
            vmEmployeeProfile.CNIC = employeeObj.CNIC;
            vmEmployeeProfile.Address = employeeObj.Address;
            vmEmployeeProfile.LocationName = employeeObj.LocationName;
            vmEmployeeProfile.DesignationName = employeeObj.DesignationName;
            vmEmployeeProfile.EmailID = employeeObj.EmailID;
            return vmEmployeeProfile;
        }
    }
}
