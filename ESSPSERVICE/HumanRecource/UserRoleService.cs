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
    public class UserRoleService : IUserRoleService
    {
        IDDService DDService;
        IRepository<AppUserRole> AppUserRoleRepository;
        IRepository<AppUserTM> AppUserTMRepository;
        IUnitOfWork UnitOfWork;
        public UserRoleService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<AppUserRole> appUserRoleRepository,
            IRepository<AppUserTM> appUserTMRepository)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            AppUserRoleRepository = appUserRoleRepository;
            AppUserTMRepository = appUserTMRepository;
        }

        public VMAppUserRole GetEdit(string id)
        {
            // Instantiating the VMAppUserRole 
            VMAppUserRole vmAppUserRole = new VMAppUserRole();
            // Get AppUserRole
            AppUserRole objAppUserRole = AppUserRoleRepository.GetSingle(id);
            // Get AppUserTMS
            Expression<Func<AppUserTM, bool>> SpecificEntries = c => c.UserRoleID == id;
            List<AppUserTM> listAppUserTMS = AppUserTMRepository.FindBy(SpecificEntries);
            vmAppUserRole.PVMUserRoleID = objAppUserRole.PUserRoleID;
            vmAppUserRole.VMUserRoleName = objAppUserRole.UserRoleName;
            vmAppUserRole.PAppUserTMSID = 0;
            // If there is entry in AppUserTms
            if (listAppUserTMS.Count>0)
            {

            //intantiate and returns the firs element of AppUserTMS
                AppUserTM objAppUserTMS = listAppUserTMS.First();
                vmAppUserRole.PAppUserTMSID = objAppUserTMS.PAppUserTMSID;
                vmAppUserRole.MLeave = objAppUserTMS.MLeave;
                vmAppUserRole.LeavePolicy = objAppUserTMS.LeavePolicy;
                vmAppUserRole.LeaveApplication = objAppUserTMS.LeaveApplication;
                vmAppUserRole.LeaveQuota = objAppUserTMS.LeaveQuota;
                vmAppUserRole.LeaveCF = objAppUserTMS.LeaveCF;
                vmAppUserRole.MShift = objAppUserTMS.MShift;
                vmAppUserRole.Shift = objAppUserTMS.Shift;
                vmAppUserRole.ShiftChange = objAppUserTMS.ShiftChange;
                vmAppUserRole.ShiftChangeEmp = objAppUserTMS.ShiftChangeEmp;
                vmAppUserRole.Roster = objAppUserTMS.Roster;
                vmAppUserRole.MOvertime = objAppUserTMS.MOvertime;
                vmAppUserRole.OvertimePolicy = objAppUserTMS.OvertimePolicy;
                vmAppUserRole.OvertimeAP = objAppUserTMS.OvertimeAP;
                vmAppUserRole.OvertimeENCPL = objAppUserTMS.OvertimeENCPL;
                vmAppUserRole.MAttendanceEditor = objAppUserTMS.MAttendanceEditor;
                vmAppUserRole.JobCard = objAppUserTMS.JobCard;
                vmAppUserRole.DailyAttEditor = objAppUserTMS.DailyAttEditor;
                vmAppUserRole.MonthlyAttEditor = objAppUserTMS.MonthlyAttEditor;
                vmAppUserRole.CompanyStructure = objAppUserTMS.CompanyStructure;
                vmAppUserRole.MSettings = objAppUserTMS.MSettings;
                vmAppUserRole.Reader = objAppUserTMS.Reader;
                vmAppUserRole.Holiday = objAppUserTMS.Holiday;
                vmAppUserRole.DownloadTime = objAppUserTMS.DownloadTime;
                vmAppUserRole.ServiceLog = objAppUserTMS.ServiceLog;
                vmAppUserRole.MUser = objAppUserTMS.MUser;
                vmAppUserRole.AppUser = objAppUserTMS.AppUser;
                vmAppUserRole.AppUserRole = objAppUserTMS.AppUserRole;
                vmAppUserRole.Employee = objAppUserTMS.Employee;
                vmAppUserRole.Crew = objAppUserTMS.Crew;
                vmAppUserRole.JTCommon = objAppUserTMS.JTCommon;
                vmAppUserRole.OUCommon = objAppUserTMS.OUCommon;
                vmAppUserRole.FinancialYear = objAppUserTMS.FinancialYear;
                vmAppUserRole.PayrollPeriod = objAppUserTMS.PayrollPeriod;
                vmAppUserRole.TMSAdd = objAppUserTMS.TMSAdd;
                vmAppUserRole.JTCommon = objAppUserTMS.JTCommon;
                vmAppUserRole.MCompany = objAppUserTMS.MCompany;
                vmAppUserRole.MAttendance = objAppUserTMS.MAttendance;
                vmAppUserRole.TMSEdit = objAppUserTMS.TMSEdit;
                vmAppUserRole.TMSView = objAppUserTMS.TMSView;
                vmAppUserRole.TMSDelete = objAppUserTMS.TMSDelete;
                vmAppUserRole.Reports= objAppUserTMS.Reports;
                vmAppUserRole.RMS = objAppUserTMS.RMS;
                vmAppUserRole.RMSPosition = objAppUserTMS.RMSPosition;
                vmAppUserRole.RMSRequisition = objAppUserTMS.RMSRequisition;
                vmAppUserRole.RMSShortlisting = objAppUserTMS.RMSShortlisting;
                vmAppUserRole.RMSInterviewManagement = objAppUserTMS.RMSInterviewManagement;
                vmAppUserRole.RMSHiringNote = objAppUserTMS.RMSHiringNote;
                vmAppUserRole.RMSReporting = objAppUserTMS.RMSReporting;
                vmAppUserRole.PMS = objAppUserTMS.PMS;
                vmAppUserRole.PMSBellCurve = objAppUserTMS.PMSBellCurve;
                vmAppUserRole.PMSCompetency = objAppUserTMS.PMSCompetency;
                vmAppUserRole.PMSSetting = objAppUserTMS.PMSSetting;
                vmAppUserRole.PMSCycle = objAppUserTMS.PMSCycle;
            }
            return vmAppUserRole;

        }

        public void PostEdit(VMAppUserRole obj)
        {
            //Instantiate the AppUserTMS
            AppUserTM objAppUserTMS = new AppUserTM();
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
            objAppUserTMS.JTCommon = obj.JTCommon;
            objAppUserTMS.MCompany = obj.MCompany;
            objAppUserTMS.MAttendance = obj.MAttendance;
            objAppUserTMS.Holiday = obj.Holiday;
            objAppUserTMS.DownloadTime = obj.DownloadTime;
            objAppUserTMS.ServiceLog = obj.ServiceLog;
            objAppUserTMS.MUser = obj.MUser;
            objAppUserTMS.AppUser = obj.AppUser;
            objAppUserTMS.AppUserRole = obj.AppUserRole;
            objAppUserTMS.Employee = obj.Employee;
            objAppUserTMS.Crew = obj.Crew;
            objAppUserTMS.JTCommon = obj.JTCommon;
            objAppUserTMS.OUCommon = obj.OUCommon;
            objAppUserTMS.FinancialYear = obj.FinancialYear;
            objAppUserTMS.PayrollPeriod = obj.PayrollPeriod;
            objAppUserTMS.Reports = obj.Reports;
            objAppUserTMS.TMSAdd = obj.TMSAdd;
            objAppUserTMS.TMSEdit = obj.TMSEdit;
            objAppUserTMS.TMSView = obj.TMSView;
            objAppUserTMS.TMSDelete = obj.TMSDelete;
            objAppUserTMS.UserRoleID = obj.PVMUserRoleID;
            objAppUserTMS.RMS = obj.RMS;
            objAppUserTMS.RMSPosition = obj.RMSPosition;
            objAppUserTMS.RMSRequisition = obj.RMSRequisition;
            objAppUserTMS.RMSShortlisting = obj.RMSShortlisting;
            objAppUserTMS.RMSInterviewManagement = obj.RMSInterviewManagement;
            objAppUserTMS.RMSHiringNote = obj.RMSHiringNote;
            objAppUserTMS.RMSReporting = obj.RMSReporting;
            objAppUserTMS.PMS = obj.PMS;
            objAppUserTMS.PMSBellCurve = obj.PMSBellCurve;
            objAppUserTMS.PMSCompetency = obj.PMSCompetency;
            objAppUserTMS.PMSSetting = obj.PMSSetting;
            objAppUserTMS.PMSCycle = obj.PMSCycle;
            objAppUserTMS.PermissionForAppUser = false;
            //If the tms user idd is zero the will add new entry in AppuserTMS
            if (obj.PAppUserTMSID==0)
            {
                AppUserTMRepository.Add(objAppUserTMS);
                UnitOfWork.Commit();
            }
            //if count is greater than 1 then system will update the entry in the AppUserTMS
            else
            {
                objAppUserTMS.PAppUserTMSID = obj.PAppUserTMSID;
                AppUserTMRepository.Edit(objAppUserTMS);
                UnitOfWork.Commit();
            }
        }


        /// <summary>
        /// This method gets the index of all the user roles created
        /// </summary>
        /// <returns>returns a view that has list of all the user roles.</returns>
        /// <remarks>index has a edit button and list of all the user roles.</remarks>
        List<AppUserRole> IUserRoleService.GetIndex()
        {
            return AppUserRoleRepository.GetAll();
        }
    }
}
