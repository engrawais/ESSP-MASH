using AppESSP.Areas.Attendance.Helper;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.Attendance.Controllers
{
    public class RosterController : Controller
    {
        IDDService DDService;
        IEntityService<Shift> ShiftService;
        IEntityService<RosterApplication> RosterApplicationService;
        IEntityService<RosterDetail> RosterDetailService;
        IEntityService<VAT_RosterApplication> VATRosterApplicationService;
        IEntityService<VAT_RosterDetail> VATRosterDetailService;
        IRosterService RosterService;
        public RosterController(IDDService dDService, IEntityService<RosterApplication> rosterApplicationService, IEntityService<RosterDetail> rosterDetailService,
            IRosterService rosterService, IEntityService<VAT_RosterApplication> vATRosterApplicationService, IEntityService<Shift> shiftService, IEntityService<VAT_RosterDetail> vATRosterDetailService)
        {
            DDService = dDService;
            RosterApplicationService = rosterApplicationService;
            RosterDetailService = rosterDetailService;
            RosterService = rosterService;
            VATRosterApplicationService = vATRosterApplicationService;
            ShiftService = shiftService;
            VATRosterDetailService = vATRosterDetailService;
        }


        public ActionResult ViewRosterDetail(int? id)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            List<VAT_RosterDetail> vATRosterDetailList = new List<VAT_RosterDetail>();
            Expression<Func<VAT_RosterDetail, bool>> SpecificEntries = c => c.RotaAppID == id;
            vATRosterDetailList = VATRosterDetailService.GetIndexSpecific(SpecificEntries);

            return View(vATRosterDetailList);
        }


        // GET: Attendance/Roster
        public ActionResult Index()
        {
            return RedirectToAction("RosterAppIndex");
        }

        public ActionResult RosterAppIndex(FormCollection form)
        {
            List<VAT_RosterApplication> RosterApplicationsList = new List<VAT_RosterApplication>();
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            RosterApplicationsList = RosterService.GetIndex(LoggedInUser);
            return View(RosterApplicationsList);
        }

        #region --Create Roster---
        [HttpGet]
        public ActionResult Create1()
        {
            VMRosterApplication vm = new VMRosterApplication();
            CreateHelper();
            ViewBag.ErrorList = "";
            return View("Create1", vm);
        }


        
        [HttpPost]
        public ActionResult Create1(VMRosterApplication obj)
        {
            List<string> ValidationError = new List<string>();
            // check for validation
            Expression<Func<RosterApplication, bool>> SpecificEntries = item => obj.DateStarted < item.DateEnded && item.DateEnded > item.DateStarted && item.CriteriaData == obj.CrewID;
            List<RosterApplication> tempVM = RosterApplicationService.GetIndexSpecific(SpecificEntries);
            if (tempVM.Count > 0)
            {
                    ValidationError.Add("Rosters already created with same criteria");
            }
            if (ValidationError.Count == 0)
            {
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                VMRosterModel vm = RosterService.PostCreate1(obj, LoggedInUser);
                return View("Create2", vm);
            }
            else
            {
                ViewBag.ErrorList = ValidationError.ToList();
                CreateHelper();
                return View("Create1", obj);
            }

        }

        [HttpPost]
        public ActionResult Create2(VMRosterModel vm)
        {
            List<RosterAttributes> rosterAttributeList = new List<RosterAttributes>();
            VAT_Shift _selectedShift = DDService.GetShift().Where(aa => aa.PShiftID == vm.ShiftID).First();
            int _RotaAppID = Convert.ToInt32(Request.Form["RotaAppID"].ToString());


            for (int i = 1; i <= vm.NoOfDays; i++)
            {
                rosterAttributeList.Add(new RosterAttributes()
                {
                    DutyTime = ATAssistant.ConvertTime(Request.Form["DT-" + i.ToString()].ToString()),
                    DutyDate = Convert.ToDateTime(Request.Form["Date-" + i.ToString()]),
                    WorkMin = Convert.ToInt32(Request.Form["WM-" + i.ToString()])
                });
            }
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            RosterService.PostCreate2(vm, rosterAttributeList);
            ViewBag.ErrorList = "";
            CreateHelper();
            return RedirectToAction("Index");
        }

        #endregion

        #region --Detail View Roster---
        public ActionResult RosterDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RosterDetailsCustom _RosterDetails = new RosterDetailsCustom();
            //_RosterDetails.RosterDetails = db.RosterDetails.Where(aa => aa.RosterAppID == id).ToList();
            //RosterApp _RosterApp = new RosterApp();
            //_RosterApp = db.RosterApps.First(aa => aa.RotaApplD == id);
            ////_RosterDetails
            //int CrewID = Convert.ToInt32(Request.Form["CrewList"].ToString());
            Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == id;
            var rosterdetails = RosterDetailService.GetIndexSpecific(SpecificEntries);
            RosterApplication rosterApp = RosterApplicationService.GetEdit((int)id);
            VAT_RosterApplication vATrosterApp = VATRosterApplicationService.GetEdit((int)id);
            ViewBag.Header = "For Shift: " + vATrosterApp.ShiftName + ", Group: " + vATrosterApp.CrewName;
            return View(CalculateRosterDetails(rosterdetails, rosterApp).OrderByDescending(aa => aa.DutyDate));
        }

        private List<RosterDetailAttributes> CalculateRosterDetails(List<RosterDetail> rosterdetails, RosterApplication rosterApp)
        {
            //List<RosterDetailModel> rdm = new List<RosterDetailModel>();
            List<RosterDetailAttributes> rda = new List<RosterDetailAttributes>();
            //Shift shift = new Shift();
            //shift = db.Shifts.First(aa => aa.ShiftID == rosterApp.ShiftID);
            DateTime currentDate = rosterApp.DateStarted.Value;
            List<RosterDetail> tempRotaDetails = new List<RosterDetail>();
            while (currentDate <= rosterApp.DateEnded)
            {
                RosterDetailAttributes rdaS = new RosterDetailAttributes();
                tempRotaDetails = rosterdetails.Where(aa => aa.RosterDate == currentDate).ToList();
                if (tempRotaDetails.Count > 0)
                {
                    rdaS.Changed = true;
                    rdaS.Day = tempRotaDetails.FirstOrDefault().RosterDate.Value.ToString("dddd");
                    rdaS.DutyCode = tempRotaDetails.FirstOrDefault().DutyCode;
                    rdaS.DutyDate = tempRotaDetails.FirstOrDefault().RosterDate.Value;
                    rdaS.DutyTime = (TimeSpan)tempRotaDetails.FirstOrDefault().DutyTime;
                    rdaS.WorkMin = (short)tempRotaDetails.FirstOrDefault().WorkMin;
                }
                else
                {
                    //rdaS.Changed = false;
                    //rdaS.Day = currentDate.ToString("dddd");
                    //int wrkMin = CalculateDutyCode(shift, currentDate);
                    //if (wrkMin == 0)
                    //    rdaS.DutyCode = "R";
                    //else
                    //    rdaS.DutyCode = "D";
                    //rdaS.DutyDate = currentDate;
                    //rdaS.DutyTime = shift.StartTime;
                    //rdaS.WorkMin = wrkMin;
                }
                rda.Add(rdaS);
                currentDate = currentDate.AddDays(1);
            }
            return rda;
        }

        #endregion


        #region --Edit Roster---
        public ActionResult RosterEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterApplication rosterApp = RosterApplicationService.GetEdit((int)id);
            Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == id;
            List<RosterDetail> rosterDetails = RosterDetailService.GetIndexSpecific(SpecificEntries);
            return View(CalculateRosterEditEntries(rosterApp, rosterDetails));
        }


        public ActionResult EditRosterSave(FormCollection form)
        {
            int noOfDays = Convert.ToInt16(Request.Form["noOfDays"].ToString());
            List<RosterAttributes> rosters = new List<RosterAttributes>();
            int _RotaAppID = Convert.ToInt32(Request.Form["RotaAppID"].ToString());

            DeleteRosterList(_RotaAppID);
            for (int i = 1; i < noOfDays; i++)
            {
                rosters.Add(new RosterAttributes()
                {
                    DutyTime = ATAssistant.ConvertTime(Request.Form["DT-" + i.ToString()].ToString()),
                    DutyDate = Convert.ToDateTime(Request.Form["Date-" + i.ToString()]),
                    WorkMin = Convert.ToInt32(Request.Form["WM-" + i.ToString()])
                });
            }
            SaveEditRosterEntries(rosters, _RotaAppID);

            ViewBag.ErrorList = "";
            CreateHelper();
            return RedirectToAction("Index");
        }

        private void DeleteRosterList(int _RotaAppID)
        {
            Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == _RotaAppID;
            List<RosterDetail> rosterDetail = RosterDetailService.GetIndexSpecific(SpecificEntries);
            foreach (var item in rosterDetail)
            {
                RosterDetailService.PostDelete(item);
            }
        }

        private void SaveEditRosterEntries(List<RosterAttributes> rosters, int _RotaAppID)
        {
            RosterApplication rotaApp = RosterApplicationService.GetEdit(_RotaAppID);
            VAT_Shift shift = DDService.GetShift().Where(aa => aa.PShiftID == rotaApp.ShiftID).First();

            foreach (var roster in rosters)
            {
                //if (isRosterValueChanged(roster, shift))
                {
                    RosterDetail rosterDetail = new RosterDetail();
                    rosterDetail.CriteriaValueDate = rotaApp.RosterCriteria.ToString() + rotaApp.CriteriaData.ToString() + roster.DutyDate.ToString("yyMMdd");
                    //rosterDetail.CompanyID = rotaApp.CompanyID;
                    rosterDetail.OpenShift = shift.OpenShift;
                    //rosterDetail.UserID = rotaApp.UserID;
                    rosterDetail.RosterAppID = _RotaAppID;
                    if (roster.WorkMin == 0)
                    {
                        rosterDetail.DutyCode = "R";
                    }
                    else
                    {
                        rosterDetail.DutyCode = "D";
                    }
                    if (roster.DutyTime == new TimeSpan(0, 0, 0))
                    {
                        rosterDetail.OpenShift = true;
                    }
                    else
                    {
                        rosterDetail.OpenShift = false;
                    }
                    rosterDetail.DutyTime = roster.DutyTime;
                    rosterDetail.WorkMin = (short)roster.WorkMin;
                    rosterDetail.RosterDate = roster.DutyDate;
                    RosterDetailService.PostCreate(rosterDetail);
                }
            }
        }

        private VMRosterModel CalculateRosterEditEntries(RosterApplication rosterApp, List<RosterDetail> rosterDetails)
        {
            VAT_Shift shift = DDService.GetShift().Where(aa => aa.PShiftID == rosterApp.ShiftID).First();
            VMRosterModel _objmodel = new VMRosterModel();
            int i = 1;
            try
            {
                _objmodel._RosterAttributes = new List<RosterAttributes>();
                DateTime _StartDate = (DateTime)rosterApp.DateStarted;
                _objmodel.RotaAppID = rosterApp.RotaAppID;
                _objmodel.ShiftName = shift.ShiftName;
                switch (rosterApp.RosterCriteria)
                {
                    case "S":

                        break;
                    case "C":
                        _objmodel.CriteriaValueName = DDService.GetCrew().Where(aa => aa.PCrewID == rosterApp.CriteriaData).First().CrewName;
                        break;
                    case "T":

                        break;
                    case "employee":

                        break;
                }
                while (_StartDate <= rosterApp.DateEnded)
                {
                    string _day = _StartDate.Date.ToString("dddd");
                    string _date = _StartDate.Date.ToString("dd-MMM-yyyy");
                    string _DTime = "";
                    TimeSpan _DutyTime = new TimeSpan();
                    int _WorkMin = 0;
                    if (rosterDetails.Where(aa => aa.RosterDate == _StartDate).Count() > 0)
                    {
                        // from roster details
                        RosterDetail rotaDetail = rosterDetails.First(aa => aa.RosterDate == _StartDate);
                        _DTime = rotaDetail.DutyTime.Value.Hours.ToString("00") + rotaDetail.DutyTime.Value.Minutes.ToString("00");
                        _WorkMin = (int)rotaDetail.WorkMin;
                        _DutyTime = (TimeSpan)rotaDetail.DutyTime;

                    }
                    else
                    {
                        //from shift

                        _DTime = shift.StartTime.Hours.ToString("00") + shift.StartTime.Minutes.ToString("00");
                        _WorkMin = CalculateShiftWorkMins(_StartDate, shift);
                        _DutyTime = shift.StartTime;
                    }
                    _objmodel._RosterAttributes.Add(new RosterAttributes { ID = i, DateString = _date, Day = _day, DutyDate = _StartDate.Date, DutyTimeString = _DTime, DutyTime = _DutyTime, WorkMin = _WorkMin });
                    _StartDate = _StartDate.AddDays(1);
                    i++;
                }
                _objmodel.NoOfDays = i;
                return _objmodel;
            }
            catch (Exception ex)
            {
                return _objmodel;
            }
        }

        private int CalculateShiftWorkMins(DateTime _StartDate, VAT_Shift shift)
        {
            int workMins = 0;
            switch (_StartDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    workMins = shift.MonMin;
                    break;
                case DayOfWeek.Tuesday:
                    workMins = shift.TueMin;
                    break;
                case DayOfWeek.Wednesday:
                    workMins = shift.WedMin;
                    break;
                case DayOfWeek.Thursday:
                    workMins = shift.ThuMin;
                    break;
                case DayOfWeek.Friday:
                    workMins = shift.FriMin;
                    break;
                case DayOfWeek.Saturday:
                    workMins = shift.SatMin;
                    break;
                case DayOfWeek.Sunday:
                    workMins = shift.SunMin;
                    break;
            }
            return workMins;
        }

        #endregion


        #region --Continue Roster---

        public ActionResult RosterContinue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMRosterContinue vm = new VMRosterContinue();
            vm.RotaAppID = (int)id;
            return View(vm);
        }
        [HttpPost]
        public ActionResult RosterContinue(VMRosterContinue vm)
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            VMRosterModel vmRoster = RosterService.ContinueRoster(vm);
            return View("Create2", vmRoster);
        }
        #endregion

        #region --Delete Roster---

        public ActionResult RosterDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMRosterApplication vm = new VMRosterApplication();
            vm.RotaApplD = (int)id;
            //DeleteRoster((int)id);
            return View("Delete", vm);
        }
        [HttpPost]
        public ActionResult Delete(VMRosterApplication obj)
        {
            RosterApplication RApp = new RosterApplication();
            //User uid = new User();
            RApp = RosterApplicationService.GetEdit(obj.RotaApplD);
            try
            {
                Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == obj.RotaApplD;
                List<RosterDetail> RAppDetail = RosterDetailService.GetIndexSpecific(SpecificEntries);
                foreach (var item in RAppDetail)
                {
                    RosterDetailService.PostDelete(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            RosterApplicationService.PostDelete(RApp);
            return RedirectToAction("Index");
        }

        #endregion
        public ActionResult OpenEmployeeRoster(int ShiftID, int CrewID)

        {
            Expression<Func<VAT_RosterApplication, bool>> SpecificEntries1 = c => c.CriteriaData == CrewID;
            var rosterApps = VATRosterApplicationService.GetIndexSpecific(SpecificEntries1);
            Expression<Func<RosterApplication, bool>> SpecificEntries2 = c => c.CriteriaData == CrewID;
            var rosterApps2 = RosterApplicationService.GetIndexSpecific(SpecificEntries2);
            if (rosterApps.Count>0) {
                VAT_RosterApplication rosterApp = rosterApps.First();
                Expression<Func<RosterDetail, bool>> SpecificEntries = c => c.RosterAppID == rosterApp.RotaAppID;
                var rosterdetails = RosterDetailService.GetIndexSpecific(SpecificEntries);
                
                ViewBag.Header = "For Shift: " + rosterApp.ShiftName + ", Group: " + rosterApp.CrewName;
                return View("RosterDetail", CalculateRosterDetails(rosterdetails, rosterApps2.First()).OrderByDescending(aa => aa.DutyDate));
            }
            else
            {

                Shift obj = ShiftService.GetEdit((int)ShiftID);
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                ViewBag.LocationID = new SelectList(DDService.GetLocation(LoggedInUser).ToList().OrderBy(aa => aa.LocationName).ToList(), "PLocationID", "LocationName", obj.LocationID);
                ViewBag.DayOff1 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName", obj.DayOff1);
                ViewBag.DayOff2 = new SelectList(DDService.GetDaysName().ToList().OrderBy(aa => aa.DayName).ToList(), "DaysNameID", "DayName", obj.DayOff2);
                return View("ShiftView", obj);
            }
            
        }


        #region -- Common Helper
        public void CreateHelper()
        {
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            ViewBag.RosterTypeID = new SelectList(DDService.GetRosterType(), "PRosterTypeID", "RosterTypeName");
            ViewBag.ShiftID = new SelectList(DDService.GetShift(LoggedInUser), "PShiftID", "ShiftName");
            ViewBag.CrewID = new SelectList(DDService.GetCrew(LoggedInUser).ToList(), "PCrewID", "CrewName");
        }
        #endregion
    }
}