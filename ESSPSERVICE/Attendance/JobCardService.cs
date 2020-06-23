using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Attendance;
using ESSPSERVICE.Generic;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;

namespace ESSPSERVICE.Attendance
{/// <summary>
///Job Card for the TimeOffice user i-e Creation,and Deletion.
/// </summary>
    class JobCardService : IJobCardService
    {
        IDDService DDService;
        IEmpSelectionService EmpSelectionService;
        IRepository<VAT_JobCardApplication> VATJobCardApplicationRepository;

        IRepository<JobCardApp> JobCardAppRepository;
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IEntityService<Shift> ShiftService;
        IUnitOfWork UnitOfWork;
        public JobCardService(IDDService dDService, IEmpSelectionService empSelectionService, IRepository<VAT_JobCardApplication> vATJobCardApplicationRepository
            , IRepository<JobCardApp> jobCardAppRepository, IUnitOfWork unitOfWork, IEntityService<PayrollPeriod> payrollPeriodService
        , IEntityService<Shift> shiftService)
        {
            DDService = dDService;
            EmpSelectionService = empSelectionService;
            VATJobCardApplicationRepository = vATJobCardApplicationRepository;
            JobCardAppRepository = jobCardAppRepository;
            PayrollPeriodService = payrollPeriodService;
            ShiftService = shiftService;
            UnitOfWork = unitOfWork;
        }
        //
        public VMJobCardCreate GetCreate1()
        {
            VMJobCardCreate obj = new VMJobCardCreate();
            return obj;
        }
        public VMJobCardCreate GetCreate2(VMJobCardCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
            int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
            int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser)
        {
            VMEmpSelection vmEmpSelection = EmpSelectionService.GetStepTwo(SelectedCompanyIds,
                                            SelectedOUCommonIds, SelectedOUIds, SelectedEmploymentTypeIds,
                                            SelectedLocationIds, SelectedGradeIds, SelectedJobTitleIds, SelectedDesignationIds,
                                            SelectedCrewIds, SelectedShiftIds, es.EmpNo, LoggedInUser);
            es.Criteria = vmEmpSelection.Criteria;
            es.CriteriaName = vmEmpSelection.CriteriaName;
            es.EmpNo = vmEmpSelection.EmpNo;
            es.Employee = vmEmpSelection.Employee;
            es.JobCardTypeName = DDService.GetJobCardType().Where(aa => aa.PJobCardTypeID == es.JobCardTypeID).First().JobCardName;
            return es;
        }
        public VMJobCardCreate GetDelete(int id)
        {
            throw new NotImplementedException();
        }

        public VMJobCardCreate GetEdit(int id)
        {
            throw new NotImplementedException();
        }

        public List<VAT_JobCardApplication> GetIndex(VMLoggedUser LoggedInUser)
        {
            List<VHR_EmployeeProfile> employees = DDService.GetEmployeeInfo(LoggedInUser); // Get All Employees from database
            List<VAT_JobCardApplication> dbJobCardList = new List<VAT_JobCardApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                if (LoggedInUser.UserLoctions != null)
                {
                    foreach (var userLocaion in LoggedInUser.UserLoctions)
                    {
                        Expression<Func<VAT_JobCardApplication, bool>> SpecificEntries = c => c.LocationID == userLocaion.LocationID && (c.JobCardStageID == "A" || c.JobCardStageID == null);
                        dbJobCardList.AddRange(VATJobCardApplicationRepository.FindBy(SpecificEntries));
                    }
                }
            }
            else
                dbJobCardList = VATJobCardApplicationRepository.GetAll();
            return dbJobCardList;
        }
        public JobCardApp GetMultipleDay()
        {
            JobCardApp obj = new JobCardApp();
            return obj;
        }

        public JobCardApp GetSingleDay()
        {
            JobCardApp obj = new JobCardApp();
            return obj;
        }

        public ServiceMessage PostCreate(VMJobCardCreate obj)
        {
            throw new NotImplementedException();
        }

        public void PostCreate3(VMJobCardCreate es, int?[] employeeIds, VMLoggedUser LoggedInUser)
        {
            string Message = "";
            foreach (var empid in employeeIds)
            {
                VHR_EmployeeProfile employee = DDService.GetEmployeeInfo(LoggedInUser).Where(aa => aa.PEmployeeID == empid).First();
                Expression<Func<Shift, bool>> SpecificEntries97 = c => c.PShiftID == employee.ShiftID;
                Shift shifts = ShiftService.GetIndexSpecific(SpecificEntries97).First();
                if (shifts.GZDays == true)
                {
                    List<Holiday> holiday = DDService.GetHolidays().Where(aa => aa.HolidayDate == es.JobDateFrom).ToList();
                    if (holiday.Count > 0)
                    {
                        Message = "Cannot apply job card of the Gazetted Holiday";
                    }
                }
                Expression<Func<PayrollPeriod, bool>> SpecificEntries96 = c => es.JobDateFrom >= c.PRStartDate && es.JobDateFrom <= c.PREndDate && c.PeriodStageID == "C";
                List<PayrollPeriod> dbPayrollPeriods = PayrollPeriodService.GetIndexSpecific(SpecificEntries96).ToList();
                if (dbPayrollPeriods.Count() > 0)
                {
                    Message = "Cannot enter Job card in Closed Payroll Period";
                }
                if (Message == "")
                {
                    JobCardApp jcApp = new JobCardApp();
                    jcApp.DateCreated = DateTime.Now;
                    jcApp.DateEnded = es.JobDateTo;
                    jcApp.DateStarted = es.JobDateFrom;
                    jcApp.EmployeeID = empid;
                    jcApp.Minutes = es.TotalMinutes;
                    jcApp.Remarks = es.Remarks;
                    jcApp.JobCardTypeID = es.JobCardTypeID;
                    jcApp.UserID = LoggedInUser.PUserID;
                    JobCardAppRepository.Add(jcApp);
                    JobCardAppRepository.Save();
                    DDService.ProcessDailyAttendance(jcApp.DateStarted, jcApp.DateEnded, (int)jcApp.EmployeeID, jcApp.EmployeeID.ToString());
                    DDService.ProcessMonthlyAttendance(jcApp.DateStarted, (int)jcApp.EmployeeID, jcApp.EmployeeID.ToString());
                }
                else
                {
                }
            }
        }

        public ServiceMessage PostDelete(JobCardApp obj)
        {
            JobCardAppRepository.Delete(obj);
            JobCardAppRepository.Save();
            return new ServiceMessage();
        }

        public ServiceMessage PostEdit(VMJobCardCreate obj)
        {
            throw new NotImplementedException();
        }

        public void PostMultipleDay(JobCardApp obj)
        {
            obj.DateCreated = DateTime.Now;
            JobCardAppRepository.Add(obj);
            UnitOfWork.Commit();
        }

        public void PostSingleDay(JobCardApp obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateEnded = obj.DateStarted;
            if (obj.TimeEnd != null && obj.TimeEnd != null)
            {
                obj.Minutes = (short)((obj.TimeEnd.Value - obj.TimeStart.Value).TotalMinutes);
            }
            JobCardAppRepository.Add(obj);
            UnitOfWork.Commit();
        }
    }
}
