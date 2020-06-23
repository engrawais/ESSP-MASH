using ESSPCORE.EF;
using ESSPCORE.HumanResource;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.HumanRecource
{
    public class UserService : IUserService
    {
        IDDService DDService;
        IRepository<AppUser> AppUserRepository;
        IRepository<AppUserTM> AppUserTMRepository;
        IRepository<AppUserLocation> AppUserLocationRepository;
        IRepository<AppUserDepartment> AppUserDepartmentRepository;
        IUnitOfWork UnitOfWork;
        public UserService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<AppUser> appUserRepository,
            IRepository<AppUserTM> appUserTMRepository, IRepository<AppUserLocation> appUserLocationRepository, IRepository<AppUserDepartment> appUserDepartmentRepository)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            AppUserRepository = appUserRepository;
            AppUserTMRepository = appUserTMRepository;
            AppUserLocationRepository = appUserLocationRepository;
            AppUserDepartmentRepository = appUserDepartmentRepository;
        }


        public VMAppUser GetCreate()
        {
            //Instatiating the VMAppUser to get location for the user.
            VMAppUser vm = new VMAppUser();
            vm.UserLocations = GetListofLocations();
            vm.UserDepartment = GetListofDepartments();
            return vm;
        }
        public List<VMUserLocation> GetListofLocations()
        {
            //Instantiating the list of the user locations
            List<VMUserLocation> vmUserLocationList = new List<VMUserLocation>();
            //If there is more than one location assigned to an employee then foreach location function will repeat and get the location and name of location
            foreach (var item in DDService.GetLocation().ToList())
            {
                VMUserLocation vmUserLocation = new VMUserLocation();
                vmUserLocation.PLocationID = item.PLocationID;
                vmUserLocation.LocationName = item.LocationName;
                vmUserLocation.IsSelected = false;
                vmUserLocationList.Add(vmUserLocation);
            }
            return vmUserLocationList;
        }
        public List<VMUserDepartment> GetListofDepartments()
        {
            //Instantiating the list of the user department
            List<VMUserDepartment> vmUserDepartmentList = new List<VMUserDepartment>();
            //If there is more than one department assigned to an employee then foreach department function will repeat and get the department and name of department
            foreach (var item in DDService.GetOUCommon().ToList())
            {
                VMUserDepartment vmUserDepartment = new VMUserDepartment();
                vmUserDepartment.POUCommonID = item.POUCommonID;
                vmUserDepartment.OUCommonName = item.OUCommonName;
                vmUserDepartment.IsSelectedDepartment = false;
                vmUserDepartmentList.Add(vmUserDepartment);
            }
            return vmUserDepartmentList;
        }
        public void PostCreate(VMAppUser vm, int?[] SelectedIds)
        {
            //Instatiating the AppUser to Post the Information of the user in App user.
            AppUser objAppUser = new AppUser();
            objAppUser.PUserID = vm.PUserID;
            objAppUser.UserName = vm.UserName;
            objAppUser.Password = vm.Password;
            objAppUser.UserStatus = vm.UserStatus;
            objAppUser.LastActiveDate = vm.LastActiveDate;
            objAppUser.EmpID = vm.EmpID;
            objAppUser.UserAccessTypeID = vm.UserAccessTypeID;
            objAppUser.UserRoleID = vm.UserRoleID;
            objAppUser.HasESSP = vm.HasESSP;
            AppUserRepository.Add(objAppUser);
            UnitOfWork.Commit();
            // Save User TMS Data creating an entry in AppUserTMS 
            AppUserTM objAppUserTMS = new AppUserTM();
            objAppUserTMS.UserID = objAppUser.PUserID;
            objAppUserTMS.PermissionForAppUser = true;
            objAppUserTMS.PAppUserTMSID = vm.PAppUserTMSID;
            objAppUserTMS.MLeave = vm.MLeave;
            objAppUserTMS.LeavePolicy = vm.LeavePolicy;
            objAppUserTMS.LeaveApplication = vm.LeaveApplication;
            objAppUserTMS.LeaveQuota = vm.LeaveQuota;
            objAppUserTMS.LeaveCF = vm.LeaveCF;
            objAppUserTMS.MShift = vm.MShift;
            objAppUserTMS.Shift = vm.Shift;
            objAppUserTMS.ShiftChange = vm.ShiftChange;
            objAppUserTMS.ShiftChangeEmp = vm.ShiftChangeEmp;
            objAppUserTMS.Roster = vm.Roster;
            objAppUserTMS.MOvertime = vm.MOvertime;
            objAppUserTMS.OvertimePolicy = vm.OvertimePolicy;
            objAppUserTMS.OvertimeAP = vm.OvertimeAP;
            objAppUserTMS.OvertimeENCPL = vm.OvertimeENCPL;
            objAppUserTMS.MAttendanceEditor = vm.MAttendanceEditor;
            objAppUserTMS.JobCard = vm.JobCard;
            objAppUserTMS.DailyAttEditor = vm.DailyAttEditor;
            objAppUserTMS.MonthlyAttEditor = vm.MonthlyAttEditor;
            objAppUserTMS.CompanyStructure = vm.CompanyStructure;
            objAppUserTMS.MSettings = vm.MSettings;
            objAppUserTMS.Reader = vm.Reader;
            objAppUserTMS.Holiday = vm.Holiday;
            objAppUserTMS.DownloadTime = vm.DownloadTime;
            objAppUserTMS.ServiceLog = vm.ServiceLog;
            objAppUserTMS.MUser = vm.MUser;
            objAppUserTMS.JTCommon = vm.JTCommon;
            objAppUserTMS.AppUser = vm.AppUser;
            objAppUserTMS.AppUserRole = vm.AppUserRole;
            objAppUserTMS.Employee = vm.Employee;
            //objAppUserTMS.OtherEmployee = vm.OtherEmployee;
            objAppUserTMS.Crew = vm.Crew;
            objAppUserTMS.OUCommon = vm.OUCommon;
            objAppUserTMS.FinancialYear = vm.FinancialYear;
            objAppUserTMS.PayrollPeriod = vm.PayrollPeriod;
            objAppUserTMS.TMSAdd = vm.TMSAdd;
            objAppUserTMS.TMSEdit = vm.TMSEdit;
            objAppUserTMS.TMSView = vm.TMSView;
            objAppUserTMS.TMSDelete = vm.TMSDelete;
            objAppUserTMS.MCompany = vm.MCompany;
            objAppUserTMS.MAttendance = vm.MAttendance;
            objAppUserTMS.Reports = vm.Reports;
            objAppUserTMS.RMS = vm.RMS;
            objAppUserTMS.RMSPosition = vm.RMSPosition;
            objAppUserTMS.RMSRequisition = vm.RMSRequisition;
            objAppUserTMS.RMSShortlisting = vm.RMSShortlisting;
            objAppUserTMS.RMSTestManagement = vm.RMSTestManagement;
            objAppUserTMS.RMSInterviewManagement = vm.RMSInterviewManagement;
            objAppUserTMS.RMSCandidateManager = vm.RMSCandidateManager;
            objAppUserTMS.RMSMeritList = vm.RMSMeritList;
            objAppUserTMS.RMSHiringNote = vm.RMSHiringNote;
            objAppUserTMS.RMSReporting = vm.RMSReporting;
            objAppUserTMS.PMS = vm.PMS;
            objAppUserTMS.PMSBellCurve = vm.PMSBellCurve;
            objAppUserTMS.PMSCompetency = vm.PMSCompetency;
            objAppUserTMS.PMSSetting = vm.PMSSetting;
            objAppUserTMS.PMSCycle = vm.PMSCycle;
            objAppUserTMS.PROBATIONEVALUATION = vm.PROBATIONEVALUATION;
            AppUserTMRepository.Add(objAppUserTMS);
            UnitOfWork.Commit();
            //If theere is location selected for the employee and selected location id is not null
            if (SelectedIds != null)
            {
                if (vm.UserAccessTypeID == 2)
                {
                    // Save values in User Locations

                    foreach (var locid in SelectedIds)
                    {
                        AppUserLocation objAppUserLocation = new AppUserLocation();
                        objAppUserLocation.LocationID = locid;
                        objAppUserLocation.UserID = objAppUser.PUserID;
                        AppUserLocationRepository.Add(objAppUserLocation);
                        UnitOfWork.Commit();
                    }
                }
                if (vm.UserAccessTypeID == 4)
                {
                    // Save values in User Locations

                    foreach (var Dept in SelectedIds)
                    {
                        AppUserDepartment objAppUserDepartment = new AppUserDepartment();
                        objAppUserDepartment.DepartmentID = Dept;
                        objAppUserDepartment.UserID = objAppUser.PUserID;
                        AppUserDepartmentRepository.Add(objAppUserDepartment);
                        UnitOfWork.Commit();
                    }
                }
            }
        }


        public VMAppUser GetEdit(int id)
        {
            //Instantiating the VMAppUser 
            VMAppUser vmAppUser = new VMAppUser();
            // Get AppUserRole
            AppUser objAppUser = AppUserRepository.GetSingle(id);
            vmAppUser.PUserID = objAppUser.PUserID;
            vmAppUser.UserName = objAppUser.UserName;
            vmAppUser.Password = objAppUser.Password;
            vmAppUser.UserStatus = objAppUser.UserStatus;
            vmAppUser.LastActiveDate = objAppUser.LastActiveDate;
            vmAppUser.EmpID = objAppUser.EmpID;
            vmAppUser.UserAccessTypeID = objAppUser.UserAccessTypeID;
            vmAppUser.UserRoleID = objAppUser.UserRoleID;
            vmAppUser.HasESSP = objAppUser.HasESSP;
            vmAppUser.PAppUserTMSID = 0;

            // Get AppUserTMS if there is any entry of the employee in the AppUser TMS table 
            Expression<Func<AppUserTM, bool>> SpecificEntries = c => c.UserID == id;
            List<AppUserTM> listAppUserTMS = AppUserTMRepository.FindBy(SpecificEntries);
            //IF count is greater than zero it means if there is entry in AppUser TMS
            if (listAppUserTMS.Count > 0)
            {
                AppUserTM objAppUserTMS = listAppUserTMS.First();
                vmAppUser.PAppUserTMSID = objAppUserTMS.PAppUserTMSID;
                vmAppUser.MLeave = objAppUserTMS.MLeave;
                vmAppUser.LeavePolicy = objAppUserTMS.LeavePolicy;
                vmAppUser.LeaveApplication = objAppUserTMS.LeaveApplication;
                vmAppUser.LeaveQuota = objAppUserTMS.LeaveQuota;
                vmAppUser.LeaveCF = objAppUserTMS.LeaveCF;
                vmAppUser.MShift = objAppUserTMS.MShift;
                vmAppUser.Shift = objAppUserTMS.Shift;
                vmAppUser.ShiftChange = objAppUserTMS.ShiftChange;
                vmAppUser.ShiftChangeEmp = objAppUserTMS.ShiftChangeEmp;
                vmAppUser.Roster = objAppUserTMS.Roster;
                vmAppUser.MOvertime = objAppUserTMS.MOvertime;
                vmAppUser.OvertimePolicy = objAppUserTMS.OvertimePolicy;
                vmAppUser.OvertimeAP = objAppUserTMS.OvertimeAP;
                vmAppUser.OvertimeENCPL = objAppUserTMS.OvertimeENCPL;
                vmAppUser.MAttendanceEditor = objAppUserTMS.MAttendanceEditor;
                vmAppUser.JobCard = objAppUserTMS.JobCard;
                vmAppUser.JTCommon = objAppUserTMS.JTCommon;
                vmAppUser.DailyAttEditor = objAppUserTMS.DailyAttEditor;
                vmAppUser.MonthlyAttEditor = objAppUserTMS.MonthlyAttEditor;
                vmAppUser.CompanyStructure = objAppUserTMS.CompanyStructure;
                vmAppUser.MSettings = objAppUserTMS.MSettings;
                vmAppUser.Reader = objAppUserTMS.Reader;
                vmAppUser.Holiday = objAppUserTMS.Holiday;
                vmAppUser.DownloadTime = objAppUserTMS.DownloadTime;
                vmAppUser.ServiceLog = objAppUserTMS.ServiceLog;
                vmAppUser.MUser = objAppUserTMS.MUser;
                vmAppUser.AppUser = objAppUserTMS.AppUser;
                vmAppUser.AppUserRole = objAppUserTMS.AppUserRole;
                vmAppUser.Employee = objAppUserTMS.Employee;
                //vmAppUser.OtherEmployee = objAppUserTMS.OtherEmployee;
                vmAppUser.Crew = objAppUserTMS.Crew;
                vmAppUser.OUCommon = objAppUserTMS.OUCommon;
                vmAppUser.FinancialYear = objAppUserTMS.FinancialYear;
                vmAppUser.PayrollPeriod = objAppUserTMS.PayrollPeriod;
                vmAppUser.TMSAdd = objAppUserTMS.TMSAdd;
                vmAppUser.TMSEdit = objAppUserTMS.TMSEdit;
                vmAppUser.TMSView = objAppUserTMS.TMSView;
                vmAppUser.TMSDelete = objAppUserTMS.TMSDelete;
                vmAppUser.MCompany = objAppUserTMS.MCompany;
                vmAppUser.MAttendance = objAppUserTMS.MAttendance;
                vmAppUser.Reports = objAppUserTMS.Reports;
                vmAppUser.RMS = objAppUserTMS.RMS;
                vmAppUser.RMSPosition = objAppUserTMS.RMSPosition;
                vmAppUser.RMSRequisition = objAppUserTMS.RMSRequisition;
                vmAppUser.RMSCandidateManager = objAppUserTMS.RMSCandidateManager;
                vmAppUser.RMSShortlisting = objAppUserTMS.RMSShortlisting;
                vmAppUser.RMSTestManagement = objAppUserTMS.RMSTestManagement;
                vmAppUser.RMSInterviewManagement = objAppUserTMS.RMSInterviewManagement;
                vmAppUser.RMSMeritList = objAppUserTMS.RMSMeritList;
                vmAppUser.RMSHiringNote = objAppUserTMS.RMSHiringNote;
                vmAppUser.RMSReporting = objAppUserTMS.RMSReporting;
                vmAppUser.PMS = objAppUserTMS.PMS;
                vmAppUser.PMSBellCurve = objAppUserTMS.PMSBellCurve;
                vmAppUser.PMSCompetency = objAppUserTMS.PMSCompetency;
                vmAppUser.PMSSetting = objAppUserTMS.PMSSetting;
                vmAppUser.PMSCycle = objAppUserTMS.PMSCycle;
                vmAppUser.PROBATIONEVALUATION = objAppUserTMS.PROBATIONEVALUATION;
            }
            // Get UserLocations
            Expression<Func<AppUserLocation, bool>> SpecificEntries2 = c => c.UserID == id;
            Expression<Func<AppUserDepartment, bool>> SpecificEntries3 = c => c.UserID == id;
            List<AppUserLocation> listAppUserLocations = DDService.GetUserLocation(SpecificEntries2);
            List<AppUserDepartment> listAppUserDepartment = DDService.GetUserDepartment(SpecificEntries3);
            List<VMUserLocation> vmUserLocationList = new List<VMUserLocation>();
            List<VMUserDepartment> vmUserDepartmentList = new List<VMUserDepartment>();
            foreach (var item in DDService.GetLocation().ToList())
            {
                VMUserLocation vmUserLocation = new VMUserLocation();
                vmUserLocation.PLocationID = item.PLocationID;
                vmUserLocation.LocationName = item.LocationName;
                if (listAppUserLocations.Where(aa => aa.LocationID == item.PLocationID).Count() > 0)
                    vmUserLocation.IsSelected = true;
                else
                    vmUserLocation.IsSelected = false;
                vmUserLocationList.Add(vmUserLocation);
            }
            vmAppUser.UserLocations = vmUserLocationList;
            foreach (var item in DDService.GetOUCommon().ToList())
            {
                VMUserDepartment vmUserDepartment = new VMUserDepartment();
                vmUserDepartment.POUCommonID = item.POUCommonID;
                vmUserDepartment.OUCommonName = item.OUCommonName;
                if (listAppUserDepartment.Where(aa => aa.DepartmentID == item.POUCommonID).Count() > 0)
                    vmUserDepartment.IsSelectedDepartment = true;
                else
                    vmUserDepartment.IsSelectedDepartment = false;
                vmUserDepartmentList.Add(vmUserDepartment);
            }
            vmAppUser.UserDepartment = vmUserDepartmentList;
            return vmAppUser;
        }

        public void PostEdit(VMAppUser obj,int?[] SelectedIds)
        {
            // Get AppUserRole and Udate the entry in AppUser table
            AppUser objAppUser = AppUserRepository.GetSingle(obj.PUserID);
            objAppUser.PUserID = obj.PUserID;
            objAppUser.UserName = obj.UserName;
            objAppUser.Password = obj.Password;
            objAppUser.UserStatus = obj.UserStatus;
            objAppUser.LastActiveDate = obj.LastActiveDate;
            objAppUser.EmpID = obj.EmpID;
            objAppUser.UserAccessTypeID = obj.UserAccessTypeID;
            objAppUser.UserRoleID = obj.UserRoleID;
            objAppUser.HasESSP = obj.HasESSP;
            AppUserRepository.Edit(objAppUser);
            UnitOfWork.Commit();
            // Get AppUserTMS  if there is any entry of the specific user is in AppUserTMS 
            Expression<Func<AppUserTM, bool>> SpecificEntries = c => c.UserID == obj.PUserID;
            List<AppUserTM> listAppUserTMS = AppUserTMRepository.FindBy(SpecificEntries);
            //IF count is greater than zero it means if there is entry in AppUser TMS
            if (listAppUserTMS.Count > 0)
            {
                //Instantiating the AppUserTMS 
                AppUserTM objAppUserTMS = listAppUserTMS.First();
                objAppUserTMS.MLeave = obj.MLeave;
                objAppUserTMS.LeavePolicy = obj.LeavePolicy;
                objAppUserTMS.LeaveApplication = obj.LeaveApplication;
                objAppUserTMS.LeaveQuota = obj.LeaveQuota;
                objAppUserTMS.LeaveCF = obj.LeaveCF;
                objAppUserTMS.MShift = obj.MShift;
                objAppUserTMS.Shift = obj.Shift;
                objAppUserTMS.JTCommon = obj.JTCommon;
                objAppUserTMS.ShiftChange = obj.ShiftChange;
                objAppUserTMS.ShiftChangeEmp = obj.ShiftChangeEmp;
                objAppUserTMS.Roster = obj.Roster;
                objAppUserTMS.MOvertime = obj.MOvertime;
                objAppUserTMS.OvertimePolicy = obj.OvertimePolicy;
                objAppUserTMS.OvertimeAP = obj.OvertimeAP;
                objAppUserTMS.OvertimeENCPL = obj.OvertimeENCPL;
                objAppUserTMS.MAttendanceEditor = obj.MAttendanceEditor;
                objAppUserTMS.JobCard = obj.JobCard;
                objAppUserTMS.DailyAttEditor = obj.DailyAttEditor;
                objAppUserTMS.MonthlyAttEditor = obj.MonthlyAttEditor;
                objAppUserTMS.CompanyStructure = obj.CompanyStructure;
                objAppUserTMS.MSettings = obj.MSettings;
                objAppUserTMS.Reader = obj.Reader;
                objAppUserTMS.Holiday = obj.Holiday;
                objAppUserTMS.DownloadTime = obj.DownloadTime;
                objAppUserTMS.ServiceLog = obj.ServiceLog;
                objAppUserTMS.MUser = obj.MUser;
                objAppUserTMS.AppUser = obj.AppUser;
                objAppUserTMS.AppUserRole = obj.AppUserRole;
                objAppUserTMS.Employee = obj.Employee;
                //objAppUserTMS.OtherEmployee = obj.OtherEmployee;
                objAppUserTMS.Crew = obj.Crew;
                objAppUserTMS.OUCommon = obj.OUCommon;
                objAppUserTMS.FinancialYear = obj.FinancialYear;
                objAppUserTMS.PayrollPeriod = obj.PayrollPeriod;
                objAppUserTMS.TMSAdd = obj.TMSAdd;
                objAppUserTMS.TMSEdit = obj.TMSEdit;
                objAppUserTMS.TMSView = obj.TMSView;
                objAppUserTMS.TMSDelete = obj.TMSDelete;
                objAppUserTMS.MCompany = obj.MCompany;
                objAppUserTMS.MAttendance = obj.MAttendance;
                objAppUserTMS.Reports = obj.Reports;
                objAppUserTMS.RMS = obj.RMS;
                objAppUserTMS.RMSPosition = obj.RMSPosition;
                objAppUserTMS.RMSRequisition = obj.RMSRequisition;
                objAppUserTMS.RMSShortlisting = obj.RMSShortlisting;
                objAppUserTMS.RMSTestManagement = obj.RMSTestManagement;
                objAppUserTMS.RMSInterviewManagement = obj.RMSInterviewManagement;
                objAppUserTMS.RMSCandidateManager = obj.RMSCandidateManager;
                objAppUserTMS.RMSMeritList = obj.RMSMeritList;
                objAppUserTMS.RMSHiringNote = obj.RMSHiringNote;
                objAppUserTMS.RMSReporting = obj.RMSReporting;
                objAppUserTMS.PMS = obj.PMS;
                objAppUserTMS.PMSBellCurve = obj.PMSBellCurve;
                objAppUserTMS.PMSCompetency = obj.PMSCompetency;
                objAppUserTMS.PMSSetting = obj.PMSSetting;
                objAppUserTMS.PMSCycle = obj.PMSCycle;
                objAppUserTMS.PROBATIONEVALUATION = obj.PROBATIONEVALUATION;
                AppUserTMRepository.Edit(objAppUserTMS);
                UnitOfWork.Commit();
            }
            //If there is no entry in AppUserTMS of the specific user then it is created here.
            else
            {
                // Save User TMS Data
                AppUserTM objAppUserTMS = new AppUserTM();
                objAppUserTMS.UserID = objAppUser.PUserID;
                objAppUserTMS.PermissionForAppUser = true;
                objAppUserTMS.PAppUserTMSID = obj.PAppUserTMSID;
                objAppUserTMS.MLeave = obj.MLeave;
                objAppUserTMS.LeavePolicy = obj.LeavePolicy;
                objAppUserTMS.LeaveApplication = obj.LeaveApplication;
                objAppUserTMS.LeaveQuota = obj.LeaveQuota;
                objAppUserTMS.LeaveCF = obj.LeaveCF;
                objAppUserTMS.MShift = obj.MShift;
                objAppUserTMS.Shift = obj.Shift;
                objAppUserTMS.ShiftChange = obj.ShiftChange;
                objAppUserTMS.ShiftChangeEmp = obj.ShiftChangeEmp;
                objAppUserTMS.Roster = obj.Roster;
                objAppUserTMS.MOvertime = obj.MOvertime;
                objAppUserTMS.OvertimePolicy = obj.OvertimePolicy;
                objAppUserTMS.OvertimeAP = obj.OvertimeAP;
                objAppUserTMS.OvertimeENCPL = obj.OvertimeENCPL;
                objAppUserTMS.MAttendanceEditor = obj.MAttendanceEditor;
                objAppUserTMS.JobCard = obj.JobCard;
                objAppUserTMS.DailyAttEditor = obj.DailyAttEditor;
                objAppUserTMS.MonthlyAttEditor = obj.MonthlyAttEditor;
                objAppUserTMS.CompanyStructure = obj.CompanyStructure;
                objAppUserTMS.MSettings = obj.MSettings;
                objAppUserTMS.Reader = obj.Reader;
                objAppUserTMS.Holiday = obj.Holiday;
                objAppUserTMS.DownloadTime = obj.DownloadTime;
                objAppUserTMS.ServiceLog = obj.ServiceLog;
                objAppUserTMS.MUser = obj.MUser;
                objAppUserTMS.JTCommon = obj.JTCommon;
                objAppUserTMS.AppUser = obj.AppUser;
                objAppUserTMS.AppUserRole = obj.AppUserRole;
                objAppUserTMS.Employee = obj.Employee;
                //objAppUserTMS.OtherEmployee = obj.OtherEmployee;
                objAppUserTMS.Crew = obj.Crew;
                objAppUserTMS.OUCommon = obj.OUCommon;
                objAppUserTMS.FinancialYear = obj.FinancialYear;
                objAppUserTMS.PayrollPeriod = obj.PayrollPeriod;
                objAppUserTMS.TMSAdd = obj.TMSAdd;
                objAppUserTMS.TMSEdit = obj.TMSEdit;
                objAppUserTMS.TMSView = obj.TMSView;
                objAppUserTMS.TMSDelete = obj.TMSDelete;
                objAppUserTMS.MCompany = obj.MCompany;
                objAppUserTMS.MAttendance = obj.MAttendance;
                objAppUserTMS.Reports = obj.Reports;
                objAppUserTMS.RMS = obj.RMS;
                objAppUserTMS.RMSPosition = obj.RMSPosition;
                objAppUserTMS.RMSRequisition = obj.RMSRequisition;
                objAppUserTMS.RMSShortlisting = obj.RMSShortlisting;
                objAppUserTMS.RMSTestManagement = obj.RMSTestManagement;
                objAppUserTMS.RMSInterviewManagement = obj.RMSInterviewManagement;
                objAppUserTMS.RMSCandidateManager = obj.RMSCandidateManager;
                objAppUserTMS.RMSMeritList = obj.RMSMeritList;
                objAppUserTMS.RMSHiringNote = obj.RMSHiringNote;
                objAppUserTMS.RMSReporting = obj.RMSReporting;
                objAppUserTMS.PMS = obj.PMS;
                objAppUserTMS.PMSBellCurve = obj.PMSBellCurve;
                objAppUserTMS.PMSCompetency = obj.PMSCompetency;
                objAppUserTMS.PMSSetting = obj.PMSSetting;
                objAppUserTMS.PMSCycle = obj.PMSCycle;
                objAppUserTMS.PROBATIONEVALUATION = obj.PROBATIONEVALUATION;
                AppUserTMRepository.Add(objAppUserTMS);
                UnitOfWork.Commit();
            }
            // Get UserLocations
            Expression<Func<AppUserLocation, bool>> SpecificEntries2 = c => c.UserID == obj.PUserID;
            List<AppUserLocation> listAppUserLocations = DDService.GetUserLocation(SpecificEntries2);
            // Get UserDepartment
            Expression<Func<AppUserDepartment, bool>> SpecificEntries3 = c => c.UserID == obj.PUserID;
            List<AppUserDepartment> listAppUserDepartments = DDService.GetUserDepartment(SpecificEntries3);
            // Delete User Locations
            foreach (var loc in DDService.GetLocation().ToList())
            {
                if (SelectedIds != null && SelectedIds.Contains(loc.PLocationID) == false)
                {
                    if (listAppUserLocations.Where(aa => aa.LocationID == loc.PLocationID).Count() > 0)
                    {
                        Expression<Func<AppUserLocation, bool>> SpecificEntries1 = c => c.LocationID == loc.PLocationID && c.UserID == obj.PUserID;
                        AppUserLocation dbAppUserLocation = AppUserLocationRepository.FindBy(SpecificEntries1).FirstOrDefault();
                        AppUserLocationRepository.Delete(dbAppUserLocation);
                        AppUserLocationRepository.Save();
                    }
                }
            }
            //Get Departments         
            //Updates the entry of the Location that are selected again 
            foreach (var loc in DDService.GetOUCommon().ToList())
            {
                if (SelectedIds != null && SelectedIds.Contains(loc.POUCommonID) == false)
                {
                    if (listAppUserDepartments.Where(aa => aa.DepartmentID == loc.POUCommonID).Count() > 0)
                    {
                        Expression<Func<AppUserDepartment, bool>> SpecificEntries1 = c => c.DepartmentID == loc.POUCommonID && c.UserID == obj.PUserID;
                        AppUserDepartment dbAppUserDepartment = AppUserDepartmentRepository.FindBy(SpecificEntries1).FirstOrDefault();
                        AppUserDepartmentRepository.Delete(dbAppUserDepartment);
                        AppUserDepartmentRepository.Save();
                    }
                }
            }
            //Updates the entry of the Department that are selected again 

        }
        /// <summary>
        /// Get index of all the users created.
        /// </summary>
        /// <returns>return  the list of user in ESSP. </returns>
        /// <remarks>This index gets the list of all the user </remarks>
        List<AppUser> IUserService.GetIndex()

        {
            return AppUserRepository.GetAll();
        }
    }
}
