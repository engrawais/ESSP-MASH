using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;

namespace ESSPSERVICE.Attendance
{
    public class RosterService : IRosterService
    {
        IDDService DDService;
        IEntityService<RosterApplication> RosterApplicationService;
        IEntityService<RosterDetail> RosterDetailService;
        IEntityService<VAT_RosterApplication> VATRosterApplicationService;

        public RosterService(IDDService dDService, IEntityService<RosterApplication> rosterApplicationService, IEntityService<RosterDetail> rosterDetailService,
            IEntityService<VAT_RosterApplication> vATRosterApplicationService)
        {
            DDService = dDService;
            RosterApplicationService = rosterApplicationService;
            RosterDetailService = rosterDetailService;
            VATRosterApplicationService = vATRosterApplicationService;
        }

        public List<VAT_RosterApplication> GetIndex(VMLoggedUser LoggedInUser)
        {
            //Gets the list of all the roaster in the database 
            List<VAT_RosterApplication> VATRosterApplication = VATRosterApplicationService.GetIndex();
            List<VAT_RosterApplication> TempVATRosterApplication = new List<VAT_RosterApplication>();
            if (LoggedInUser.UserAccessTypeID == 2)
            {
                foreach (var item in LoggedInUser.UserLoctions)
                {
                    TempVATRosterApplication.AddRange(VATRosterApplication.Where(aa => aa.LocationID == item.LocationID).ToList());
                }
            }
            if (LoggedInUser.UserAccessTypeID == 4)
            {
                foreach (var item in LoggedInUser.UserDepartments)
                {
                    TempVATRosterApplication.AddRange(VATRosterApplication.Where(aa=>aa.UserID == item.UserID).ToList());
                }
            }
            if (LoggedInUser.UserAccessTypeID == 3)
            {
               
                    TempVATRosterApplication.AddRange(VATRosterApplication.ToList());

            }
            return TempVATRosterApplication;
        }
        public VMRosterModel PostCreate1(VMRosterApplication vm, VMLoggedUser LoggedInUser)

        {
            //Saves the entry in the database table RosterApplication 
                RosterApplication ra = new RosterApplication()
                {
                    DateStarted = vm.DateStarted,
                    DateEnded = vm.DateEnded,
                    DateCreated = DateTime.Now,
                    RosterCriteria = "C",
                    CriteriaData = vm.CrewID,
                    RotaTypeID = (byte)vm.RosterTypeID,
                    Status = true,
                    ShiftID = (byte)vm.ShiftID,
                    UserID = LoggedInUser.PUserID
                };
                RosterApplicationService.PostCreate(ra);

                return CalculateRosterFields(vm, ra.RotaAppID);

        }
        public void PostCreate2(VMRosterModel vm, List<RosterAttributes> rosterAttributeList)
        {
            List<RosterDetail> vmRosterDetail = new List<RosterDetail>();
            Expression<Func<RosterDetail, bool>> SpecificEntries = aa => aa.RosterDate >= vm.StartDate && aa.RosterDate <= vm.EndDate;
            int totalWorkmints = 0;
            string showmessage = "";
            vmRosterDetail = RosterDetailService.GetIndexSpecific(SpecificEntries);
            foreach (var rosterAttribute in rosterAttributeList)
            {
                //if (isRosterValueChanged(roster, _selectedShift))
                {
                    RosterDetail _RotaDetail = new RosterDetail();
                    _RotaDetail.CriteriaValueDate = "C" + vm.CriteriaValue.ToString() + rosterAttribute.DutyDate.ToString("yyMMdd");
                    //_RotaDetail.CompanyID = _selectedShift.CompanyID;
                    _RotaDetail.OpenShift = false;
                    _RotaDetail.RosterAppID = vm.RotaAppID;
                    if (rosterAttribute.WorkMin == 0)
                    {
                        _RotaDetail.DutyCode = "R";
                    }
                    else
                    {
                        _RotaDetail.DutyCode = "D";
                    }
                    
                    _RotaDetail.DutyTime = rosterAttribute.DutyTime;
                    _RotaDetail.WorkMin = (short)rosterAttribute.WorkMin;
                    _RotaDetail.RosterDate = rosterAttribute.DutyDate;
                    if (vmRosterDetail.Where(aa => aa.CriteriaValueDate == _RotaDetail.CriteriaValueDate).Count() == 0)
                    {
                        totalWorkmints = totalWorkmints + (int)_RotaDetail.WorkMin;
                        RosterDetailService.PostCreate(_RotaDetail);
                    }
                }
            }

           
        }


        public VMRosterModel Roster(int id)
        {
            throw new NotImplementedException();
        }


        private VMRosterModel CalculateRosterFields(VMRosterApplication vm, int _RotaAppID)
        {
            VAT_Shift shift = DDService.GetShift().Where(aa => aa.PShiftID == vm.ShiftID).First();
            VMRosterModel _objmodel = new VMRosterModel();
            try
            {
                int endPoint = 0;
                if (vm.RosterTypeID == 2)
                    endPoint = (vm.DateEnded.Value - vm.DateStarted.Value).Days + 1;
                else if (vm.RosterTypeID == 3)
                    endPoint = 15;
                else if (vm.RosterTypeID == 4)
                {
                    endPoint = System.DateTime.DaysInMonth(vm.DateStarted.Value.Year, vm.DateEnded.Value.Month);
                }
                else if (vm.RosterTypeID == 4)
                {
                    endPoint = 84;
                }

                _objmodel._RosterAttributes = new List<RosterAttributes>();
                _objmodel.Criteria = ConvertCriteriaAbrvToFull("C");
                _objmodel.RotaAppID = _RotaAppID;
                _objmodel.CriteriaValue = vm.CrewID;
                _objmodel.ShiftID = vm.ShiftID;
                _objmodel.StartDate = vm.DateStarted.Value;
                _objmodel.EndDate = vm.DateEnded.Value;
                _objmodel.NoOfDays = endPoint;
                _objmodel.CriteriaValueName = DDService.GetCrew().Where(aa => aa.PCrewID == vm.CrewID).First().CrewName;
                _objmodel.ShiftName = shift.ShiftName;
                DateTime _StartDate = vm.DateStarted.Value;
                for (int i = 1; i <= endPoint; i++)
                {
                    string _day = _StartDate.Date.ToString("dddd");
                    string _date = _StartDate.Date.ToString("dd-MMM-yyyy");
                    string shiftStartTimeString = GetShiftStartTimeString(_StartDate.DayOfWeek, vm);
                    TimeSpan shiftStartTime = ATAssistant.ConvertTime(shiftStartTimeString);
                    int workMin = GetWorkMinutesForSpecificDates(_StartDate, vm);
                    _objmodel._RosterAttributes.Add(new RosterAttributes { ID = i, DateString = _date, Day = _day, DutyDate = _StartDate.Date, DutyTimeString = shiftStartTimeString, DutyTime = shiftStartTime, WorkMin = workMin });
                    _StartDate = _StartDate.AddDays(1);
                }
                Expression<Func<RosterApplication, bool>> SpecificEntries = c => c.RotaAppID == _RotaAppID;
                RosterApplication rosterApp = RosterApplicationService.GetIndexSpecific(SpecificEntries).First();
                rosterApp.DateEnded = _StartDate.AddDays(-1);
                RosterApplicationService.PostEdit(rosterApp);
                return _objmodel;
            }
            catch (Exception ex)
            {
                return _objmodel;
            }
        }

        #region -- Helper Functions--
        private string GetShiftStartTimeString(DayOfWeek dayOfWeek, VMRosterApplication vm)
        {
            string time = "0000";
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    time = vm.MonStartTime;
                    break;
                case DayOfWeek.Tuesday:
                    time = vm.TueStartTime;
                    break;
                case DayOfWeek.Wednesday:
                    time = vm.WedStartTime;
                    break;
                case DayOfWeek.Thursday:
                    time = vm.ThruStartTime;
                    break;
                case DayOfWeek.Friday:
                    time = vm.FriStartTime;
                    break;
                case DayOfWeek.Saturday:
                    time = vm.SatStartTime;
                    break;
                case DayOfWeek.Sunday:
                    time = vm.SunStartTime;
                    break;
            }
            return time;
        }
        private int CalculateDutyCode(Shift shift, DateTime currentDate)
        {
            string dutyCode = "D";
            int workMin = 0;
            switch (currentDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    workMin = shift.MonMin;
                    break;
                case DayOfWeek.Tuesday:
                    workMin = shift.TueMin;
                    break;
                case DayOfWeek.Wednesday:
                    workMin = shift.WedMin;
                    break;
                case DayOfWeek.Thursday:
                    workMin = shift.ThuMin;
                    break;
                case DayOfWeek.Friday:
                    workMin = shift.FriMin;
                    break;
                case DayOfWeek.Saturday:
                    workMin = shift.SatMin;
                    break;
                case DayOfWeek.Sunday:
                    workMin = shift.SunMin;
                    break;
            }
            return workMin;
        }
        private bool isRosterValueChanged(RosterAttributes roster, VAT_Shift _selectedShift)
        {
            DayOfWeek day = roster.DutyDate.DayOfWeek;
            bool isChanged = roster.DutyTime == _selectedShift.StartTime ? false : true;
            switch (day)
            {
                case DayOfWeek.Monday:
                    if (roster.WorkMin != _selectedShift.MonMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Tuesday:
                    if (roster.WorkMin != _selectedShift.TueMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Wednesday:
                    if (roster.WorkMin != _selectedShift.WedMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Thursday:
                    if (roster.WorkMin != _selectedShift.ThuMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Friday:
                    if (roster.WorkMin != _selectedShift.FriMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Saturday:
                    if (roster.WorkMin != _selectedShift.SatMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Sunday:
                    if (roster.WorkMin != _selectedShift.SunMin)
                        isChanged = true;
                    break;
            }
            return isChanged;
        }
        private int GetWorkMinutesForSpecificDates(DateTime startDate, VMRosterApplication vm)
        {
            int workMin = 0;
            switch (startDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    workMin = vm.MonMin;
                    break;
                case DayOfWeek.Tuesday:
                    workMin = vm.TueMin;
                    break;
                case DayOfWeek.Wednesday:
                    workMin = vm.WedMin;
                    break;
                case DayOfWeek.Thursday:
                    workMin = vm.ThruMin;
                    break;
                case DayOfWeek.Friday:
                    workMin = vm.FriMin;
                    break;
                case DayOfWeek.Saturday:
                    workMin = vm.SatMin;
                    break;
                case DayOfWeek.Sunday:
                    workMin = vm.SunMin;
                    break;
            }
            return workMin;
        }
        private string ConvertCriteriaAbrvToFull(string _Criteria)
        {
            String Criteria = "";
            switch (_Criteria)
            {
                case "S":
                    Criteria = "Shift";
                    break;
                case "C":
                    Criteria = "Crew";
                    break;
                case "T":
                    Criteria = "Section";
                    break;
                case "E":
                    Criteria = "Employee";
                    break;
            }
            return Criteria;
        }

        public VMRosterModel ContinueRoster(VMRosterContinue vm)
        {
            RosterApplication rosterApp = RosterApplicationService.GetEdit(vm.RotaAppID);
            Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == vm.RotaAppID;
            List<RosterDetail> rosterDetails = RosterDetailService.GetIndexSpecific(SpecificEntries);
            VMRosterApplication vmRotaApp = GetRosterApplication(rosterDetails, rosterApp,vm);
            rosterApp.DateEnded = vm.EndDate;
            RosterApplicationService.PostEdit(rosterApp);
            return CalculateRosterFields(vmRotaApp, rosterApp.RotaAppID);
        }

        private VMRosterApplication GetRosterApplication(List<RosterDetail> rosterDetails, RosterApplication rosterApp, VMRosterContinue obj)
        {
            VMRosterApplication vm = new VMRosterApplication();
            vm.DateEnded = obj.EndDate;
            vm.DateStarted = rosterApp.DateEnded.Value.AddDays(1);
            vm.CrewID = (int)rosterApp.CriteriaData;
            vm.RosterTypeID = (byte)rosterApp.RotaTypeID;
            vm.RotaApplD = rosterApp.RotaAppID;
            vm.ShiftID = (int)rosterApp.ShiftID;
            vm.FriMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Friday).First());
            vm.FriStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Friday).First());
            vm.SatMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Saturday).First());
            vm.SatStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Saturday).First());
            vm.SunMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Sunday).First());
            vm.SunStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Sunday).First());
            vm.MonMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Monday).First());
            vm.MonStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Monday).First());
            vm.TueMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Tuesday).First());
            vm.TueStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Tuesday).First());
            vm.WedMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Wednesday).First());
            vm.WedStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Wednesday).First());
            vm.ThruMin = GetWorkMinutes(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Thursday).First());
            vm.ThruStartTime = GetShiftStartTime(rosterDetails.Where(aa => aa.RosterDate.Value.DayOfWeek == DayOfWeek.Thursday).First());
            return vm;
        }

        private string GetShiftStartTime(RosterDetail rosterDetail)
        {
            return rosterDetail.DutyTime.Value.TotalHours.ToString("00") + rosterDetail.DutyTime.Value.Minutes.ToString("00");
        }

        private int GetWorkMinutes(RosterDetail rosterDetail)
        {
            return (int)rosterDetail.WorkMin;
        }




        #endregion
    }
}
