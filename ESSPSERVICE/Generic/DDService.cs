using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using System.Linq.Expressions;
using ESSPCORE.Common;
using ESSPSERVICE.Helper;
using System.Net;
using ESSPSERVICE.Attendance;

namespace ESSPSERVICE.Generic
{
    public class DDService : IDDService
    {
        IUnitOfWork _unitOfWork;
        IRepository<VHR_Location> LocationRepository;
        IRepository<LocationCommon> CommonLocationRepository;
        IRepository<OTPolicy> OTPolicyRepository;
        IRepository<ReaderType> ReaderTypeRepository;
        IRepository<ReaderDutyCode> ReaderDutyCodeRepository;
        IRepository<VHR_EmployeeProfile> EmployeeRepository;
        IRepository<OrganizationalUnit> OURepository;
        IRepository<VHR_OrganizationalUnit> OrganizationalUnitRepository;
        IRepository<JobCardType> JobCardTypeRepository;
        IRepository<VAT_Shift> ShiftRepository;
        IRepository<VAT_ShiftChanged> ShiftChangedRepository;
        IRepository<Crew> CrewRepository;
        IRepository<DaysName> DaysNameRepository;
        IRepository<LeaveType> LeaveTypeRepository;
        IRepository<VHR_Grade> GradeRepository;
        IRepository<VHR_EmploymentType> EmploymentTypeRepository;
        IRepository<OUCommon> OUCommonRepository;
        IRepository<VHR_JobTitle> JobTitleRepository;
        IRepository<VHR_Designation> DesignationRepository;
        IRepository<PayrollPeriod> PayrollPeriodRepository;
        IRepository<FinancialYear> FinancialYearRepository;
        IRepository<AppUser> AppUserRepository;
        IRepository<LeavePolicy> LeavePolicyRepository;
        IRepository<AppUserRole> AppUserRoleRepository;
        IRepository<AppUserAccessType> AppUserAccessTypeRepository;
        IRepository<AppUserLocation> AppUserLocationRepository;
        IRepository<AppUserDepartment> AppUserDepartmentRepository;
        IRepository<Company> CompanyRepository;
        IRepository<LocationCommon> CommonLocationOURepository;
        IRepository<JTCommon> CommonJobTitleRepository;
        IRepository<RosterType> RosterTypeRepository;
        IRepository<AuditLog> AuditLogRepository;
        IRepository<Reader> ReaderRepository;
        IRepository<LeaveStage> LeaveStageRepository;
        IRepository<Notification> NotificationRepository;
        IRepository<Catagory> CatagoryRepository;
        IRepository<ProcessRequest> ProcessRequestRepository;
        IRepository<VHR_EmployeeCrewChange> VHREmployeeCrewChangeRepository;
        IRepository<NotificationEmail> NotificationEmailRepository;
        IRepository<VHR_UserEmployee> VHRUserEmployeeRepository;
        IRepository<Holiday> HolidayRepository;
        IRepository<MonthOTStage> MonthOTStageRepository;
        IRepository<VHR_UserCommonLocation> VHRUserCommonLocationRepository;
        IRepository<JobCardStage> JobCardStageRepository;
        IRepository<VHR_AppUser> VHRAppUserRepository;
       public DDService(IUnitOfWork unitOfWork, IRepository<Catagory> catagoryRepository, 
        IRepository<VHR_Location> locationRepository, IRepository<LocationCommon> commonLocationOURepository, IRepository<LocationCommon> commonLocationRepository,
            IRepository<VHR_OrganizationalUnit> organizationalUnitRepository, IRepository<JobCardType> jobCardTypeRepository,
            IRepository<VAT_Shift> shiftRepository, IRepository<Crew> crewRepository,
            IRepository<VHR_Grade> gradeRepository, IRepository<VHR_EmploymentType> employmentTypeRepository, IRepository<DaysName> daysNameRepository,
            IRepository<ReaderDutyCode> readerDutyCodeRepository, IRepository<LeaveType> leaveTypeRepository,
            IRepository<OUCommon> oUCommonRepository, IRepository<VHR_JobTitle> jobTitleRepository, IRepository<VHR_Designation> designationRepository
            , IRepository<PayrollPeriod> payrollPeriodRepository, IRepository<FinancialYear> financialYearRepository, IRepository<OTPolicy> oTPolicyRepository
              , IRepository<AppUser> userRepository, IRepository<LeavePolicy> leavePolicyRepository, IRepository<AppUserRole> appUserRoleRepository,
            IRepository<AppUserAccessType> appUserAccessTypeRepository, IRepository<AppUserLocation> appUserLocationRepository
            , IRepository<Company> companyRepository, IRepository<JTCommon> commonJobTitleRepository, IRepository<RosterType> rosterTypeRepository,
            IRepository<AuditLog> auditLogRepository, IRepository<Reader> readerRepository, IRepository<LeaveStage> leaveStageRepository,
            IRepository<Notification> notificationRepository, IRepository<ProcessRequest> processRequestRepository,
              IRepository<VHR_EmployeeCrewChange> vHREmployeeCrewChangeRepository,
              IRepository<NotificationEmail> notificationEmailRepository, IRepository<VHR_UserEmployee> vHRUserEmployeeRepository,
              IRepository<Holiday> holidayRepository, IRepository<MonthOTStage> monthOTStageRepository, IRepository<VAT_ShiftChanged> shiftChangedRepository,
              IRepository<VHR_UserCommonLocation> vHRUserCommonLocationRepository, IRepository<JobCardStage> jobCardStageRepository, IRepository<OrganizationalUnit> oURepository, IRepository<VHR_AppUser> vHRAppUserRepository
           , IRepository<ReaderType> readerTypeRepository, IRepository<VHR_EmployeeProfile> employeeRepository, IRepository<AppUserDepartment> appUserDepartmentRepository
         )
        {
            _unitOfWork = unitOfWork;
            ReaderTypeRepository = readerTypeRepository;
            CommonLocationRepository = commonLocationRepository;
            LocationRepository = locationRepository;
            OrganizationalUnitRepository = organizationalUnitRepository;
            JobCardTypeRepository = jobCardTypeRepository;
            ShiftRepository = shiftRepository;
            CrewRepository = crewRepository;
            GradeRepository = gradeRepository;
            EmploymentTypeRepository = employmentTypeRepository;
            DaysNameRepository = daysNameRepository;
            ReaderDutyCodeRepository = readerDutyCodeRepository;
            LeaveTypeRepository = leaveTypeRepository;
            OUCommonRepository = oUCommonRepository;
            JobTitleRepository = jobTitleRepository;
            DesignationRepository = designationRepository;
            PayrollPeriodRepository = payrollPeriodRepository;
            FinancialYearRepository = financialYearRepository;
            OTPolicyRepository = oTPolicyRepository;
            AppUserRepository = userRepository;
            LeavePolicyRepository = leavePolicyRepository;
            AppUserRoleRepository = appUserRoleRepository;
            AppUserAccessTypeRepository = appUserAccessTypeRepository;
            AppUserLocationRepository = appUserLocationRepository;
            CompanyRepository = companyRepository;
            CommonJobTitleRepository = commonJobTitleRepository;
            RosterTypeRepository = rosterTypeRepository;
            AuditLogRepository = auditLogRepository;
            ReaderRepository = readerRepository;
            LeaveStageRepository = leaveStageRepository;
            NotificationRepository = notificationRepository;
            CommonLocationOURepository = commonLocationOURepository;
            CatagoryRepository = catagoryRepository;
            ProcessRequestRepository = processRequestRepository;
            VHREmployeeCrewChangeRepository = vHREmployeeCrewChangeRepository;
            NotificationEmailRepository = notificationEmailRepository;
            VHRUserEmployeeRepository = vHRUserEmployeeRepository;
            HolidayRepository = holidayRepository;
            MonthOTStageRepository = monthOTStageRepository;
            ShiftChangedRepository = shiftChangedRepository;
            VHRUserCommonLocationRepository = vHRUserCommonLocationRepository;
            JobCardStageRepository = jobCardStageRepository;
            EmployeeRepository = employeeRepository;
            OURepository = oURepository;
            AppUserDepartmentRepository = appUserDepartmentRepository;
        }

       
        public List<Reader> GetReader()
        {
            return ReaderRepository.GetAll().OrderBy(aa => aa.ReaderName).ToList();
        }
        public List<LocationCommon> GetCommanLocation()
        {
            return CommonLocationRepository.GetAll().OrderBy(aa => aa.CommonLocationName).ToList();
        }
        public List<Crew> GetCrew()
        {
            return CrewRepository.GetAll().OrderBy(aa => aa.CrewName).ToList();
        }

        public List<DaysName> GetDaysName()
        {
            return DaysNameRepository.GetAll().OrderBy(aa => aa.DayName).ToList();
        }

        public List<Catagory> GetCatagory()
        {
            return CatagoryRepository.GetAll().OrderBy(aa => aa.CatagoryName).ToList();
        }

        public List<VHR_EmployeeProfile> GetEmployeeInfo()
        {
            return EmployeeRepository.GetAll().Where(bb => bb.Status == "Active").OrderBy(aa => aa.EmployeeName).ToList();
        }
        public List<Company> GetCompany()
        {
            return CompanyRepository.GetAll().OrderBy(aa => aa.CompanyName).ToList();
        }
        public List<LocationCommon> CommonLocationOU()
        {
            //return CommonLocationOURepository.GetAll().OrderBy(aa => aa.CommonLocationName).ToList();
            List<LocationCommon> dbLocationCommon = new List<LocationCommon>();
            CommonLocationOURepository.Add(new LocationCommon { PCommonLocationID = 0, CommonLocationName = "---------" });
            dbLocationCommon.AddRange(CommonLocationOURepository.GetAll());
            return dbLocationCommon;
        }

        public List<JTCommon> GetCommonJobTitle()
        {
            return CommonJobTitleRepository.GetAll().OrderBy(aa => aa.JTCommonName).ToList();
        }
        public List<LocationCommon> GetCommanLocation(VMLoggedUser loggedUser)
        {
            List<LocationCommon> list = new List<LocationCommon>();
            List<LocationCommon> tempList = new List<LocationCommon>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = CommonLocationRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = CommonLocationRepository.GetAll();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {

                        tempList.AddRange(list.Where(aa => aa.PCommonLocationID == userLocaion.PUserLocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = CommonLocationRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.CrewID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PCommonLocationID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.CommonLocationName).ToList();
        }
        public List<VHR_Location> GetLocation(VMLoggedUser loggedUser)
        {
            List<VHR_Location> locations = new List<VHR_Location>();
            List<VHR_Location> tempLocations = new List<VHR_Location>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                locations = LocationRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    locations = LocationRepository.GetAll();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        tempLocations.AddRange(locations.Where(aa => aa.PLocationID == userLocaion.LocationID));
                    }
                    locations = tempLocations.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    locations = LocationRepository.GetAll();
                    foreach (var locid in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.LocationID).Distinct())
                    {
                        tempLocations.AddRange(locations.Where(aa => aa.PLocationID == locid));
                    }
                    locations = tempLocations.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                locations = LocationRepository.GetAll();
                foreach (var locid in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.LocationID).Distinct())
                {
                    tempLocations.AddRange(locations.Where(aa => aa.PLocationID == locid));
                }
                locations = tempLocations.ToList();
            }
            return locations.OrderBy(aa => aa.LocationName).ToList();
        }

        public List<VHR_OrganizationalUnit> GetOU(VMLoggedUser loggedUser)
        {
            List<VHR_OrganizationalUnit> list = new List<VHR_OrganizationalUnit>();
            List<VHR_OrganizationalUnit> tempList = new List<VHR_OrganizationalUnit>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
                list = OrganizationalUnitRepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
                    list = OrganizationalUnitRepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
                    list = OrganizationalUnitRepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.OUID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.POUID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
                list = OrganizationalUnitRepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.OUID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.POUID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.OUName).ToList();
        }
        public List<VHR_EmploymentType> GetEmploymentType()
        {
            return EmploymentTypeRepository.GetAll().OrderBy(aa => aa.EmploymentTypeName).ToList();
        }

        public List<VHR_Grade> GetGrade()
        {
            return GradeRepository.GetAll().OrderBy(aa => aa.GradeName).ToList();
        }

        public List<VHR_Location> GetLocation()
        {
            return LocationRepository.GetAll().OrderBy(aa => aa.LocationName).ToList();
        }
        public List<OrganizationalUnit> GetOUSimple()
        {
            Expression<Func<OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
            return OURepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
        }

        public List<VHR_OrganizationalUnit> GetOU()
        {
            Expression<Func<VHR_OrganizationalUnit, bool>> SpecificEntries = c => c.Status == true;
            return OrganizationalUnitRepository.FindBy(SpecificEntries).OrderBy(aa => aa.OUName).ToList();
        }

        public List<OUCommon> GetOUCommon()
        {
            List<OUCommon> list = new List<OUCommon>();
            list.Add(new OUCommon { POUCommonID = 0, OUCommonName = "--------" });
            list.AddRange(OUCommonRepository.GetAll());
            return list;
            //return OUCommonRepository.GetAll().OrderBy(aa => aa.OUCommonName).ToList();
        }

        public List<VHR_EmployeeProfile> GetEmployeeInfo(VMLoggedUser loggedUser)
        {
            List<VHR_EmployeeProfile> list = new List<VHR_EmployeeProfile>();
            List<VHR_EmployeeProfile> tempList = new List<VHR_EmployeeProfile>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = EmployeeRepository.GetAll().ToList();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = EmployeeRepository.GetAll().ToList();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        tempList.AddRange(list.Where(aa => aa.LocationID == userLocaion.LocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = EmployeeRepository.GetAll().ToList();
                    foreach (var userDepartment in loggedUser.UserDepartments)
                    {
                        tempList.AddRange(list.Where(aa => aa.OUCommonID == userDepartment.DepartmentID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser);
            }
            return list.OrderBy(aa => aa.EmployeeName).ToList();
        }

        public List<Company> GetCompany(VMLoggedUser loggedUser)
        {
            List<Company> list = new List<Company>();
            List<Company> tempList = new List<Company>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = CompanyRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = CompanyRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.CompanyID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PCompanyID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = CompanyRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.CompanyID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PCompanyID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.CompanyName).ToList();
        }

        public List<JTCommon> GetCommonJobTitle(VMLoggedUser loggedUser)
        {
            List<JTCommon> list = new List<JTCommon>();
            List<JTCommon> tempList = new List<JTCommon>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = CommonJobTitleRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = CommonJobTitleRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    //foreach (var id in EmpList.Select(aa => aa.JTCommonID).Distinct())
                    //{
                    //    tempList.AddRange(list.Where(aa => aa.PJTCommonID == id));
                    //}
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = CommonJobTitleRepository.GetAll();
                //foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.JTCommonID).Distinct())
                //{
                //    tempList.AddRange(list.Where(aa => aa.PJTCommonID == id));
                //}
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.JTCommonName).ToList();
        }

        public List<OUCommon> GetOUCommon(VMLoggedUser loggedUser)
        {
            List<OUCommon> list = new List<OUCommon>();
            List<OUCommon> tempList = new List<OUCommon>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = OUCommonRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = OUCommonRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.OUID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.POUCommonID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = OUCommonRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userDepartment in loggedUser.UserDepartments)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.OUCommonID == userDepartment.DepartmentID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.OUCommonID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.POUCommonID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = OUCommonRepository.GetAll();
                //foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.OUCommonID).Distinct())
                //{
                //    tempList.AddRange(list.Where(aa => aa.POUCommonID == id));
                //}
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.OUCommonName).ToList();
        }


        public List<VHR_JobTitle> GetJobTitle(VMLoggedUser loggedUser)
        {
            List<VHR_JobTitle> list = new List<VHR_JobTitle>();
            List<VHR_JobTitle> tempList = new List<VHR_JobTitle>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                Expression<Func<VHR_JobTitle, bool>> SpecificEntries = c => c.Status == true;
                list = JobTitleRepository.FindBy(SpecificEntries).OrderBy(aa => aa.JobTitleName).ToList();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    Expression<Func<VHR_JobTitle, bool>> SpecificEntries2 = c => c.Status == true;
                    list = JobTitleRepository.FindBy(SpecificEntries2).OrderBy(aa => aa.JobTitleName).ToList();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.JobTitleID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PJobTitleID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    Expression<Func<VHR_JobTitle, bool>> SpecificEntries = c => c.Status == true;
                    list = JobTitleRepository.FindBy(SpecificEntries).OrderBy(aa => aa.JobTitleName).ToList();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.JobTitleID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PJobTitleID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                Expression<Func<VHR_JobTitle, bool>> SpecificEntries = c => c.Status == true;
                list = JobTitleRepository.FindBy(SpecificEntries).OrderBy(aa => aa.JobTitleName).ToList();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.JobTitleID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PJobTitleID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.JobTitleName).ToList();
        }

        public List<Crew> GetCrew(VMLoggedUser loggedUser)
        {
            List<Crew> list = new List<Crew>();
            List<Crew> tempList = new List<Crew>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = CrewRepository.GetAll();
            }
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                list = CrewRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = CrewRepository.GetAll();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {

                        tempList.AddRange(list.Where(aa => aa.LocationID == userLocaion.LocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = CrewRepository.GetAll();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.CrewID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PCrewID == id));
                    }
                    list = tempList.ToList();
                }
                return list.OrderBy(aa => aa.CrewName).ToList();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = CrewRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.CrewID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PCrewID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.CrewName).ToList();
        }

        public List<VHR_Grade> GetGrade(VMLoggedUser loggedUser)
        {
            List<VHR_Grade> list = new List<VHR_Grade>();
            List<VHR_Grade> tempList = new List<VHR_Grade>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = GradeRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = GradeRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.GradeID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PGradeID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = GradeRepository.GetAll();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.GradeID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PGradeID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = GradeRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.GradeID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PGradeID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.GradeName).ToList();
        }

        public List<VHR_EmploymentType> GetEmploymentType(VMLoggedUser loggedUser)
        {
            List<VHR_EmploymentType> list = new List<VHR_EmploymentType>();
            List<VHR_EmploymentType> tempList = new List<VHR_EmploymentType>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = EmploymentTypeRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = EmploymentTypeRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.EmploymentTypeID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PEmploymentTypeID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = EmploymentTypeRepository.GetAll();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.EmploymentTypeID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PEmploymentTypeID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = EmploymentTypeRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.EmploymentTypeID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PEmploymentTypeID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.EmploymentTypeName).ToList();
        }

        public List<VHR_Designation> GetDesignation(VMLoggedUser loggedUser)
        {
            List<VHR_Designation> list = new List<VHR_Designation>();
            List<VHR_Designation> tempList = new List<VHR_Designation>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = DesignationRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = DesignationRepository.GetAll();
                    List<VHR_EmployeeProfile> EmpList = new List<VHR_EmployeeProfile>();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        Expression<Func<VHR_EmployeeProfile, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID;
                        EmpList.AddRange(EmployeeRepository.FindBy(SpecificEntries).ToList());
                    }
                    foreach (var id in EmpList.Select(aa => aa.DesigationID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PDesignationID == id));
                    }
                    list = tempList.ToList();
                }
            }
             else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                if (loggedUser.UserDepartments != null)
                {
                    list = DesignationRepository.GetAll();
                    foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.DesigationID).Distinct())
                    {
                        tempList.AddRange(list.Where(aa => aa.PDesignationID == id));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = DesignationRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.DesigationID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PDesignationID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.DesignationName).ToList();
        }
        public List<ReaderDutyCode> GetReaderDutyCode()
        {
            return ReaderDutyCodeRepository.GetAll().OrderBy(aa => aa.ReaderDutyName).ToList();
        }

        public List<ReaderType> GetReaderType()
        {
            List<ReaderType> readerTypes = ReaderTypeRepository.GetAll();
            return readerTypes.OrderBy(aa => aa.ReaderTypeName).ToList();
        }
        public List<JobCardType> GetJobCardType()
        {
            return JobCardTypeRepository.GetAll().Where(aa => aa.PJobCardTypeID != 2).OrderBy(aa => aa.JobCardName).ToList();
        }

        public List<LeaveType> GetLeaveType()
        {
            return LeaveTypeRepository.GetAll().Where(aa=>aa.PLeaveTypeID== 1 || aa.PLeaveTypeID == 2 || aa.PLeaveTypeID == 3 || aa.PLeaveTypeID == 4 || aa.PLeaveTypeID == 5 || aa.PLeaveTypeID == 6 || aa.PLeaveTypeID == 7 || aa.PLeaveTypeID == 8 || aa.PLeaveTypeID == 9 || aa.PLeaveTypeID == 10 || aa.PLeaveTypeID == 11 || aa.PLeaveTypeID == 12).OrderBy(aa => aa.LeaveTypeName).ToList();
        }


        public List<OTPolicy> GetOTPolicy()
        {
            return OTPolicyRepository.GetAll().OrderBy(aa => aa.OTPolicyName).ToList();
        }
        public List<FinancialYear> GetFinancialYear()
        {
            return FinancialYearRepository.GetAll().OrderBy(aa => aa.FYName).ToList();
        }
        public List<PayrollPeriod> GetPayrollPeriod()
        {
            Expression<Func<PayrollPeriod, bool>> SpecificEntries = c => c.PeriodStageID == "O";
            return PayrollPeriodRepository.FindBy(SpecificEntries).OrderBy(aa => aa.PRName).ToList();
        }
        public List<PayrollPeriod> GetAllPayrollPeriod()
        {
            return PayrollPeriodRepository.GetAll().OrderBy(aa => aa.PRName).ToList();
        }
        public List<AppUser> GetUser()
        {
            return AppUserRepository.GetAll().Where(aa => aa.UserStatus == true).OrderBy(aa => aa.UserName).ToList();
        }
        public List<VHR_AppUser> GetVHRAppUser()
        {
            return VHRAppUserRepository.GetAll().Where(aa => aa.UserStatus == true).OrderBy(aa => aa.UserName).ToList();
        }
        public List<LeavePolicy> GetLeavePolicy()
        {
            return LeavePolicyRepository.GetAll().OrderBy(aa => aa.LeavePolicyName).ToList();
        }
        public List<AppUserRole> GetUserRole()
        {
            return AppUserRoleRepository.GetAll().OrderBy(aa => aa.UserRoleName).ToList();
        }
        public List<AppUserAccessType> GetUserAccessType()
        {
            return AppUserAccessTypeRepository.GetAll().OrderBy(aa => aa.UserAccessTypeName).ToList();
        }
        public List<AppUserLocation> GetUserLocation(Expression<Func<AppUserLocation, bool>> predicate)
        {
            return AppUserLocationRepository.FindBy(predicate);
        }
        public List<AppUserDepartment> GetUserDepartment(Expression<Func<AppUserDepartment, bool>> predicate)
        {
            return AppUserDepartmentRepository.FindBy(predicate);
        }

        public List<VAT_Shift> GetShift()
        {
            return ShiftRepository.GetAll().OrderBy(aa => aa.ShiftName).ToList();
        }


        List<VHR_Designation> IDDService.GetDesignation()
        {
            return DesignationRepository.GetAll().OrderBy(aa => aa.DesignationName).ToList();
        }

        List<VHR_JobTitle> IDDService.GetJobTitle()
        {
            return JobTitleRepository.GetAll().OrderBy(aa => aa.JobTitleName).ToList();
        }


        public List<VHR_EmployeeProfile> GetSpecificEmployee(Expression<Func<VHR_EmployeeProfile, bool>> predicate)
        {
            return EmployeeRepository.FindBy(predicate);
        }
        public List<AppUser> GetSpecificUser(Expression<Func<AppUser, bool>> predicate)
        {
            return AppUserRepository.FindBy(predicate).OrderBy(aa => aa.UserName).ToList();
        }
        public List<VHR_EmployeeCrewChange> GetSpecificEmployeeHistory(Expression<Func<VHR_EmployeeCrewChange, bool>> predicate)
        {
            return VHREmployeeCrewChangeRepository.FindBy(predicate);
        }


        public List<VAT_Shift> GetShift(VMLoggedUser loggedUser)
        {
            List<VAT_Shift> list = new List<VAT_Shift>();
            List<VAT_Shift> tempList = new List<VAT_Shift>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = ShiftRepository.GetAll();
            }
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.DepartmentBased))
            {
                list = ShiftRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = ShiftRepository.GetAll();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        tempList.AddRange(list.Where(aa => aa.LocationID == userLocaion.LocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = ShiftRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.ShiftID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PShiftID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.ShiftName).ToList();
        }
        public List<VAT_ShiftChanged> GetShiftChange(VMLoggedUser loggedUser)
        {
            List<VAT_ShiftChanged> list = new List<VAT_ShiftChanged>();
            List<VAT_ShiftChanged> tempList = new List<VAT_ShiftChanged>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = ShiftChangedRepository.GetAll();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = ShiftChangedRepository.GetAll();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        tempList.AddRange(list.Where(aa => aa.LocationID == userLocaion.LocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = ShiftChangedRepository.GetAll();
                foreach (var id in EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), loggedUser).Select(aa => aa.ShiftID).Distinct())
                {
                    tempList.AddRange(list.Where(aa => aa.PShiftChangedID == id));
                }
                list = tempList.ToList();
            }
            return list.OrderBy(aa => aa.ShiftName).ToList();
        }

        public List<RosterType> GetRosterType()
        {
            return RosterTypeRepository.GetAll().OrderBy(aa => aa.RosterTypeName).ToList();
        }

        public void SaveAuditLog(int UserID, AuditFormAttendance form, AuditTypeCommon type, int pID, IPHostEntry ipHostEntry)
        {
            AuditLog obj = new AuditLog();
            obj.AuditDateTime = DateTime.Now;
            obj.AuditFormID = Convert.ToInt16(form);
            obj.AuditTypeID = Convert.ToInt16(type);
            obj.AuditUserID = UserID;
            if (pID > 0)
                obj.PID = pID.ToString();
            if (ipHostEntry.HostName != null)
                obj.MachineName = ipHostEntry.HostName;
            if (ipHostEntry != null)
            {
                foreach (IPAddress IP in ipHostEntry.AddressList)
                {
                    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        obj.IPAddress = Convert.ToString(IP);
                    }
                }
            }
            AuditLogRepository.Add(obj);
            _unitOfWork.Commit();
        }

        public List<LeaveStage> GetLeaveStage()
        {
            return LeaveStageRepository.GetAll();
        }

        public void SaveNotification(int UserID, string url, int notificationTypeID, bool status, int? EmployeeID, int? PID)
        {
            NotificationRepository.ToggleEFValidations(false);
            Notification nf = new Notification();
            nf.NotificationTypeID = notificationTypeID;
            nf.CreatedDate = DateTime.Now;
            nf.UserID = UserID;
            nf.Status = status;
            nf.NotificationURL = url;
            nf.EmployeeID = EmployeeID;
            nf.PID = PID;
            NotificationRepository.Add(nf);
            NotificationRepository.Save();
            NotificationRepository.ToggleEFValidations(true);
        }

        public void DeleteNotification(Expression<Func<Notification, bool>> predicate)
        {
            foreach (var item in NotificationRepository.FindBy(predicate).ToList())
            {
                item.Status = false;
                item.InactiveDate = DateTime.Now;
                NotificationRepository.Edit(item);
                NotificationRepository.Save();
            }
        }

        public void ProcessMonthlyAttendance(DateTime dts, int EmpID, string EmpNo)
        {
            try
            {
                List<PayrollPeriod> periods = PayrollPeriodRepository.GetAll();
                PayrollPeriod prp = ATAssistant.GetPayrollPeriod(dts, periods);
                if (prp != null && prp.PPayrollPeriodID > 0)
                {
                    ProcessRequest pr = new ProcessRequest();
                    pr.CreatedDate = DateTime.Now;
                    pr.SystemGenerated = true;
                    pr.Criteria = "E";
                    pr.DateFrom = (DateTime)prp.PRStartDate;
                    pr.DateTo = (DateTime)prp.PREndDate;
                    pr.EmpID = EmpID;
                    pr.EmpNo = EmpNo;
                    pr.PeriodTag = "M";
                    pr.ProcessingDone = false;
                    ProcessRequestRepository.Add(pr);
                    ProcessRequestRepository.Save();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void ProcessMonthlyERPAttendance(DateTime dts, int EmpID)
        {
            try
            {
                List<PayrollPeriod> periods = PayrollPeriodRepository.GetAll();
                PayrollPeriod prp = ATAssistant.GetPayrollPeriod(dts, periods);

                Expression<Func<ProcessRequest, bool>> SpecificEntries = c => c.DateFrom == prp.PRStartDate && c.DateTo == prp.PREndDate && c.Criteria == "C" && c.PeriodTag == "E";
                if (ProcessRequestRepository.FindBy(SpecificEntries).Count == 0)
                {
                    if (prp != null && prp.PPayrollPeriodID > 0)
                    {
                        ProcessRequest pr = new ProcessRequest();
                        pr.CreatedDate = DateTime.Now;
                        pr.SystemGenerated = true;
                        pr.Criteria = "C";
                        pr.DateFrom = (DateTime)prp.PRStartDate;
                        pr.DateTo = (DateTime)prp.PREndDate;
                        pr.EmpID = EmpID;
                        pr.EmpNo = EmpID.ToString();
                        pr.PeriodTag = "E";
                        pr.ProcessingDone = false;
                        ProcessRequestRepository.Add(pr);
                        ProcessRequestRepository.Save();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void ProcessDailyAttendance(DateTime dts, DateTime dte, int EmpID, string EmpNo)
        {
            try
            {
                int Days = (dte - dts).Days;
                if (Days < 100)
                {
                    ProcessRequest pr = new ProcessRequest();
                    pr.CreatedDate = DateTime.Now;
                    pr.SystemGenerated = true;
                    pr.Criteria = "E";
                    pr.DateFrom = dts;
                    pr.DateTo = dte;
                    pr.EmpID = EmpID;
                    pr.EmpNo = EmpNo;
                    pr.PeriodTag = "D";
                    pr.ProcessingDone = false;
                    ProcessRequestRepository.Add(pr);
                    ProcessRequestRepository.Save();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public List<Notification> GetNotifications(Expression<Func<Notification, bool>> predicate)
        {
            return NotificationRepository.FindBy(predicate);
        }

        public void GenerateEmail(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID)
        {

            NotificationEmail notificationEmail = new NotificationEmail();
            notificationEmail.ToAddress = TO;
            if (notificationEmail.ToAddress != null && notificationEmail.ToAddress != "")
            {

                notificationEmail.CCAddress = CC;
                notificationEmail.Subject = Subject;
                notificationEmail.Body = Body;
                notificationEmail.IsSent = false;
                notificationEmail.CreatedDateTime = DateTime.Now;
                notificationEmail.CreatedByUserID = CreatedByuserID;
                notificationEmail.NotificationTypeID = NotiTypeID;
                NotificationEmailRepository.Add(notificationEmail);
                NotificationEmailRepository.Save();
            }
        }
        public void GenerateEmailForPerformance(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID)
        {

            NotificationEmail notificationEmail = new NotificationEmail();
            notificationEmail.ToAddress = TO;
            if (notificationEmail.ToAddress != null && notificationEmail.ToAddress != "")
            {

                notificationEmail.CCAddress = CC;
                notificationEmail.Subject = Subject;
                notificationEmail.Body = Body;
                notificationEmail.IsSent = false;
                notificationEmail.CreatedDateTime = DateTime.Now;
                notificationEmail.CreatedByUserID = CreatedByuserID;
                notificationEmail.NotificationTypeID = NotiTypeID;
                NotificationEmailRepository.Add(notificationEmail);
                NotificationEmailRepository.Save();
            }
        }

        public void GenerateEmailForRecruitment(string TO, string CC, string Subject, string Body, int CreatedByuserID, int NotiTypeID)
        {

            NotificationEmail notificationEmail = new NotificationEmail();
            notificationEmail.ToAddress = TO;
            if (notificationEmail.ToAddress != null && notificationEmail.ToAddress != "")
            {

                notificationEmail.CCAddress = CC;
                notificationEmail.Subject = Subject;
                notificationEmail.Body = Body;
                notificationEmail.IsSent = false;
                notificationEmail.CreatedDateTime = DateTime.Now;
                notificationEmail.CreatedByUserID = CreatedByuserID;
                notificationEmail.NotificationTypeID = NotiTypeID;
                NotificationEmailRepository.Add(notificationEmail);
                NotificationEmailRepository.Save();
            }
        }
     
        public VHR_UserEmployee GetEmployeeUser(int? UserID, int? EmpID)
        {
            if (UserID != null)
            {
                Expression<Func<VHR_UserEmployee, bool>> SpecificEntries2 = c => c.PUserID == UserID;
                return VHRUserEmployeeRepository.FindBy(SpecificEntries2).First();
            }
            else
            {
                Expression<Func<VHR_UserEmployee, bool>> SpecificEntries = c => c.UserEmpID == EmpID;
                return VHRUserEmployeeRepository.FindBy(SpecificEntries).First();
            }
        }

        public List<Holiday> GetHolidays()
        {
            return HolidayRepository.GetAll();
        }

        public List<MonthOTStage> GetMonthOTStage()
        {
            return MonthOTStageRepository.GetAll();
        }

        public List<VHR_UserCommonLocation> GetUserCommonLocations(VMLoggedUser LoggedInUser)
        {
            Expression<Func<VHR_UserCommonLocation, bool>> SpecificEntries2 = c => c.UserID == LoggedInUser.PUserID;
            return VHRUserCommonLocationRepository.FindBy(SpecificEntries2).ToList();
        }

        public List<JobCardStage> GetJobCardStage()
        {
            return JobCardStageRepository.GetAll().ToList();
        }

        public List<VHR_EmployeeProfile> GetReportingToEmployees(VMLoggedUser LoggedInUser)
        {
            List<VHR_EmployeeProfile> list = new List<VHR_EmployeeProfile>();
           // list = EmployeeLM.GetReportingEmployees(EmployeeRepository.GetAll(), LoggedInUser);
            //list = list.Distinct().ToList();
            // Remove Duplication
            return list.OrderBy(aa => aa.EmployeeName).ToList();
        }

        public bool IsDateLieBetweenActivePayroll(DateTime date)
        {
            List<PayrollPeriod> ActivePRPeriods = PayrollPeriodRepository.GetAll().Where(aa => aa.PeriodStageID == "O").ToList();
            if (ActivePRPeriods.Count() > 0)
            {
               // if (ActivePRPeriods.Where(aa => date >= aa.PRStartDate && date <= aa.PREndDate).Count() > 0)
                    return true;
                //else
                  //  return false;
            }
            return false;
        }

        public List<VHR_AppUser> GetSpecificUsers(Expression<Func<VHR_AppUser, bool>> predicate)
        {
            return VHRAppUserRepository.FindBy(predicate);
        }
      
    }
}
