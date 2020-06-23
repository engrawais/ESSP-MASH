using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;

namespace ESSPSERVICE.Reports
{

    public class AttReportingService : IAttReportingService
    {
        IDDService DDService;
        IRepository<LeaveQuotaYear> LeaveQuotaYearRepository;
        IRepository<LeaveData> LeaveDataRepository;
        IRepository<DailyAttendance> DailyAttendanceRepository;
        IRepository<FinancialYear> FinancialYearRepository;
        public AttReportingService(IDDService dDService, IRepository<LeaveQuotaYear> leaveQuotaYearRepository,
            IRepository<LeaveData> leaveDataRepository, IRepository<DailyAttendance> dailyAttendanceRepository, IRepository<FinancialYear> financialYearRepository)
        {
            DDService = dDService;
            LeaveQuotaYearRepository = leaveQuotaYearRepository;
            LeaveDataRepository = leaveDataRepository;
            DailyAttendanceRepository = dailyAttendanceRepository;
            FinancialYearRepository = financialYearRepository;
        }
        public List<VMLeaveBalance> GetYearlyLeaveBalance(List<VHR_EmployeeProfile> dbEmps, int FinancialYearID)
        {
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries = c => c.FinancialYearID == FinancialYearID;
            List<LeaveQuotaYear> dbLeaveQuotaYears = LeaveQuotaYearRepository.FindBy(SpecificEntries);
            Expression<Func<FinancialYear, bool>> SpecificFinancialYear = c => c.PFinancialYearID == FinancialYearID;
            FinancialYear dbFinancialYear = FinancialYearRepository.FindBy(SpecificFinancialYear).First();
            List<VMLeaveBalance> vmLeaveBalanceList = new List<VMLeaveBalance>();
            foreach (var dbEmp in dbEmps)
            {
                VMLeaveBalance vmLeaveBalance = new VMLeaveBalance();

                vmLeaveBalance.PEmployeeID = dbEmp.PEmployeeID;
                vmLeaveBalance.OEmpID = dbEmp.OEmpID;
                vmLeaveBalance.EmployeeName = dbEmp.EmployeeName;
                //vmLeaveBalance.OUName = dbEmp.OUName;
                vmLeaveBalance.LocationName = dbEmp.LocationName;
                vmLeaveBalance.JobTitleName = dbEmp.JobTitleName;
                vmLeaveBalance.EmploymentTypeName = dbEmp.EmploymentTypeName;
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // AL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmLeaveBalance.TotalAL = dbLeaveQuotaYear.YearlyTotal;
                    vmLeaveBalance.BalanceAL = dbLeaveQuotaYear.YearlyRemaining;
                    vmLeaveBalance.AvailAL = vmLeaveBalance.TotalAL - vmLeaveBalance.BalanceAL;
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // ACCU
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmLeaveBalance.TotalAccum = dbLeaveQuotaYear.CFFromLastYear;
                    vmLeaveBalance.BalanceAccum = dbLeaveQuotaYear.CFRemaining;
                    vmLeaveBalance.AvailAccum = vmLeaveBalance.TotalAccum - vmLeaveBalance.BalanceAccum;
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 2).Count() > 0) // CL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 2).First();
                    vmLeaveBalance.TotalCL = dbLeaveQuotaYear.YearlyTotal;
                    vmLeaveBalance.BalanceCL = dbLeaveQuotaYear.YearlyRemaining;
                    vmLeaveBalance.AvailCL = vmLeaveBalance.TotalCL - vmLeaveBalance.BalanceCL;
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 3).Count() > 0) //SL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 3).First();
                    vmLeaveBalance.TotalSL = dbLeaveQuotaYear.YearlyTotal;
                    vmLeaveBalance.BalanceSL = dbLeaveQuotaYear.YearlyRemaining;
                    vmLeaveBalance.AvailSL = vmLeaveBalance.TotalSL - vmLeaveBalance.BalanceSL;
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 4).Count() > 0) //CPL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 4).First();
                    vmLeaveBalance.TotalCPL = dbLeaveQuotaYear.YearlyTotal;
                    vmLeaveBalance.BalanceCPL = dbLeaveQuotaYear.YearlyRemaining;
                    vmLeaveBalance.AvailCPL = vmLeaveBalance.TotalCPL - vmLeaveBalance.BalanceCPL;
                }
                Expression<Func<LeaveData, bool>> SpecificEntries99 = c => c.LeaveTypeID == 9 && c.EmpID == dbEmp.PEmployeeID;
                List<LeaveData> dbLeaveDatas = LeaveDataRepository.FindBy(SpecificEntries99);
                vmLeaveBalance.AvailHajjLeaves = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 9).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 9).Count() / 2);

                vmLeaveBalanceList.Add(vmLeaveBalance);
            }



            return vmLeaveBalanceList;
        }
        public List<VMMonthlyLeaveBalance> GetMonthlyLeaveBalance(List<VHR_EmployeeProfile> dbEmps, int FinancialYearID, DateTime StartDate, DateTime EndDate)
        {
            Expression<Func<LeaveData, bool>> SpecificEntries = c => c.AttDate >= StartDate && c.AttDate <= EndDate;
            List<LeaveData> dbLeaveDatas = LeaveDataRepository.FindBy(SpecificEntries);
            Expression<Func<LeaveQuotaYear, bool>> SpecificEntries2 = c => c.FinancialYearID == FinancialYearID;
            List<LeaveQuotaYear> dbLeaveQuotaYears = LeaveQuotaYearRepository.FindBy(SpecificEntries2);
            List<VMMonthlyLeaveBalance> vmMonthlyLeaveBalanceList = new List<VMMonthlyLeaveBalance>();
            foreach (var dbEmp in dbEmps)
            {
                VMMonthlyLeaveBalance vmMonthlyLeaveBalance = new VMMonthlyLeaveBalance();

                vmMonthlyLeaveBalance.EmpID = dbEmp.PEmployeeID;
                vmMonthlyLeaveBalance.OEmpID = dbEmp.OEmpID;
                vmMonthlyLeaveBalance.EmployeeName = dbEmp.EmployeeName;
                vmMonthlyLeaveBalance.LocationName = dbEmp.LocationName;
                //vmMonthlyLeaveBalance.OUName = dbEmp.OUName;
                vmMonthlyLeaveBalance.JobTitleName = dbEmp.JobTitleName;
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // AL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmMonthlyLeaveBalance.BalanceAL = dbLeaveQuotaYear.YearlyRemaining;
                    vmMonthlyLeaveBalance.AvailedAL = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && (aa.IsAccum == false || aa.IsAccum == null) && aa.LeaveTypeID == 1).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && (aa.IsAccum == false || aa.IsAccum == null) && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() / 2);
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // ACCU
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmMonthlyLeaveBalance.BalanceAccum = dbLeaveQuotaYear.CFRemaining;
                    vmMonthlyLeaveBalance.AvailedAccum = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.IsAccum == true && aa.LeaveTypeID == 1).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.IsAccum == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() / 2);
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 2).Count() > 0) // CL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 2).First();
                    vmMonthlyLeaveBalance.BalanceCL = dbLeaveQuotaYear.YearlyRemaining;
                    vmMonthlyLeaveBalance.AvailedCL = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 2).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 2).Count() / 2);
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 3).Count() > 0) //SL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 3).First();
                    vmMonthlyLeaveBalance.BalanceSL = dbLeaveQuotaYear.YearlyRemaining;
                    vmMonthlyLeaveBalance.AvailedSL = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 3).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 3).Count() / 2);
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 4).Count() > 0) //CPL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 4).First();
                    vmMonthlyLeaveBalance.BalanceCPL = dbLeaveQuotaYear.YearlyRemaining;
                    vmMonthlyLeaveBalance.AvailedCPL = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 4).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 4).Count() / 2);
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 5).Count() >= 0) //LWOP
                {
                    vmMonthlyLeaveBalance.AvailedLWOP = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 5).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 5).Count() / 2);
                }

                vmMonthlyLeaveBalance.AvailedSpecialLeave = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 6).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 6).Count() / 2);


                vmMonthlyLeaveBalance.AvailedPaternity = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 7).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 7).Count() / 2);


                vmMonthlyLeaveBalance.AvailedMaternity = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 8).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 8).Count() / 2);


                vmMonthlyLeaveBalance.AvailedHajjLeaves = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false && aa.LeaveTypeID == 9).Count() + (dbLeaveDatas.Where(aa => aa.HalfLeave == true && aa.EmpID == dbEmp.PEmployeeID && aa.LeaveTypeID == 9).Count() / 2);

                vmMonthlyLeaveBalanceList.Add(vmMonthlyLeaveBalance);
            }



            return vmMonthlyLeaveBalanceList;
        }

        public string GetDailyReportInExcel(List<VHR_EmployeeProfile> list, VMLoggedUser LoggedInUser, DateTime StartDate, DateTime EndDate)
        {
            Expression<Func<DailyAttendance, bool>> SpecificEntries = c => c.AttDate >= StartDate && c.AttDate <= EndDate;
            List<DailyAttendance> dbDailyAttendance = DailyAttendanceRepository.FindBy(SpecificEntries);

            string retVal = "";
            //lvPDF is nothing but the listview control name
            string[] st = new string[5];
            DirectoryInfo di = new DirectoryInfo(@"C:\Reports\");
            if (di.Exists == false)
                di.Create();
            StreamWriter sw = new StreamWriter(@"C:\Reports\MonthlyReport" + LoggedInUser.PUserID + StartDate.Month.ToString("00") + ".xls", false);
            sw.AutoFlush = true;
            List<string> ColumnsName = new List<string>();
            ColumnsName.Add("EmpNo");
            ColumnsName.Add("Name");
            ColumnsName.Add("Designation");
            ColumnsName.Add("Department");
            ColumnsName.Add("Location");
            ColumnsName.Add("Date");
            ColumnsName.Add("DutyCode");
            ColumnsName.Add("DutyTime");
            ColumnsName.Add("ShiftTime");
            ColumnsName.Add("TimeIn");
            ColumnsName.Add("TimeOut");
            ColumnsName.Add("WorkMins");
            ColumnsName.Add("LateIn");
            ColumnsName.Add("LateOut");
            ColumnsName.Add("EarlyIn");
            ColumnsName.Add("EarlyOut");
            ColumnsName.Add("Overtime");
            ColumnsName.Add("Remarks");
            string ColumnString = "";
            for (int i = 0; i < ColumnsName.Count; i++)
            {
                ColumnString = ColumnString + ColumnsName[i] + "\t";
            }
            sw.Write(ColumnString + "\n");
            List<DailyAttendance> dbTempDailyAttendances = new List<DailyAttendance>();
            foreach (var item in list)
            {
                //foreach (var attData in dbDailyAttendance.Where(aa => aa.EmpID == item.PEmployeeID).OrderBy(aa => aa.AttDate))
                //{
                //    dbTempDailyAttendances.Add(attData);
                //    string Personal = item.OEmpID + "\t" + item.EmployeeName + "\t" + item.DesignationName + "\t" + item.OUName + "\t" + item.LocationName + "\t" + attData.AttDate.Value.ToString("dd-MMM-yyyy") + "\t";
                //    sw.Write(Personal + CalculateDATimeForExcel(attData) + "\n");
                //}

            }
            string TotalLine = "\t" + "\t" + "\t" + "\t" + "\t" + "\t";
            TotalLine = TotalLine + CalculateDATimeForExcelTotal(dbTempDailyAttendances);
            sw.WriteLine(TotalLine);
            sw.Close();
            FileInfo fil = new FileInfo(@"C:\Reports\MonthlyReport" + LoggedInUser.PUserID + StartDate.Month.ToString("00") + ".xls");
            if (fil.Exists == true)
            {
                retVal = "C:\\Reports\\MonthlyReport" + LoggedInUser.PUserID + StartDate.Month.ToString("00") + ".xls";
            }
            return retVal;
        }
        private string CalculateDATimeForExcel(DailyAttendance item)
        {
            TimeSpan ShiftTime = new TimeSpan();
            TimeSpan TimeIn = new TimeSpan();
            TimeSpan TimeOut = new TimeSpan();
            TimeSpan Work = new TimeSpan();
            TimeSpan LateIn = new TimeSpan();
            TimeSpan LateOut = new TimeSpan();
            TimeSpan EarlyIn = new TimeSpan();
            TimeSpan EarlyOut = new TimeSpan();
            TimeSpan Overtime = new TimeSpan();

            string row = "";
            string dutytime = "";
            string shifttime = "";
            string timein = "";
            string timeout = "";
            string work = "";
            string latein = "";
            string lateout = "";
            string earlyin = "";
            string earlyout = "";
            string overtime = "";
            row = row + item.DutyCode + "\t";

            dutytime = item.DutyTime.Value.TotalHours.ToString("00") + ":" + item.DutyTime.Value.Minutes.ToString("00");
            if (item.ShifMin > 0)
            {
                ShiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                int hours = (int)ShiftTime.TotalHours;
                int min = (int)(item.ShifMin - (hours * 60));
                shifttime = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.TimeIn != null)
            {
                TimeIn = item.TimeIn.Value.TimeOfDay;
                int hours = (int)TimeIn.TotalHours;
                int min = (int)TimeIn.Minutes;
                timein = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.TimeOut != null)
            {
                TimeOut = item.TimeOut.Value.TimeOfDay;
                int hours = (int)TimeOut.TotalHours;
                int min = (int)TimeOut.Minutes;
                timeout = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.WorkMin > 0)
            {
                Work = new TimeSpan(0, (int)item.WorkMin, 0);
                int hours = (int)Work.TotalHours;
                int min = (int)(item.WorkMin - (hours * 60));
                work = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.LateIn > 0)
            {
                LateIn = new TimeSpan(0, (int)item.LateIn, 0);
                int hours = (int)LateIn.TotalHours;
                int min = (int)(item.LateIn - (hours * 60));
                latein = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.LateOut > 0)
            {
                LateOut = new TimeSpan(0, (int)item.LateOut, 0);
                int hours = (int)LateOut.TotalHours;
                int min = (int)(item.LateOut - (hours * 60));
                lateout = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.EarlyIn > 0)
            {
                EarlyIn = new TimeSpan(0, (int)item.EarlyIn, 0);
                int hours = (int)EarlyIn.TotalHours;
                int min = (int)(item.EarlyIn - (hours * 60));
                earlyin = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.EarlyOut > 0)
            {
                EarlyOut = new TimeSpan(0, (int)item.EarlyOut, 0);
                int hours = (int)EarlyOut.TotalHours;
                int min = (int)(item.EarlyOut - (hours * 60));
                earlyout = hours.ToString("00") + ":" + min.ToString("00");
            }
            if (item.OTMin > 0)
            {
                Overtime = new TimeSpan(0, (int)item.OTMin, 0);
                int hours = (int)Overtime.TotalHours;
                int min = (int)(item.OTMin - (hours * 60));
                overtime = hours.ToString("00") + ":" + min.ToString("00");
            }
            row = row + dutytime + "\t"
            + shifttime + "\t"
            + timein + "\t"
            + timeout + "\t"
            + work + "\t"
            + latein + "\t"
            + lateout + "\t"
            + earlyin + "\t"
            + earlyout + "\t"
            + overtime + "\t"
            + item.Remarks;
            return row;
        }
        private string CalculateDATimeForExcelTotal(List<DailyAttendance> dbDailyAttendances)
        {
            TimeSpan ShiftTime = new TimeSpan();
            TimeSpan TimeIn = new TimeSpan();
            TimeSpan TimeOut = new TimeSpan();
            TimeSpan Work = new TimeSpan();
            TimeSpan LateIn = new TimeSpan();
            TimeSpan LateOut = new TimeSpan();
            TimeSpan EarlyIn = new TimeSpan();
            TimeSpan EarlyOut = new TimeSpan();
            TimeSpan Overtime = new TimeSpan();

            string row = "";
            string dutytime = "";
            string shifttime = "";
            string timein = "";
            string timeout = "";
            string work = "";
            string latein = "";
            string lateout = "";
            string earlyin = "";
            string earlyout = "";
            string overtime = "";
            row = row + "\t";

            dutytime = "";
            if (dbDailyAttendances.Sum(aa => aa.ShifMin) > 0)
            {
                ShiftTime = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.ShifMin), 0);
                int hours = (int)ShiftTime.TotalHours;
                shifttime = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.WorkMin) > 0)
            {
                Work = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.WorkMin), 0);
                int hours = (int)Work.TotalHours;
                work = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.LateIn) > 0)
            {
                LateIn = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.LateIn), 0);
                int hours = (int)LateIn.TotalHours;
                latein = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.LateOut) > 0)
            {
                LateOut = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.LateOut), 0);
                int hours = (int)LateOut.TotalHours;
                lateout = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.EarlyIn) > 0)
            {
                EarlyIn = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.EarlyIn), 0);
                int hours = (int)EarlyIn.TotalHours;
                earlyin = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.EarlyOut) > 0)
            {
                EarlyOut = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.EarlyOut), 0);
                int hours = (int)EarlyOut.TotalHours;
                earlyout = hours.ToString();
            }
            if (dbDailyAttendances.Sum(aa => aa.OTMin) > 0)
            {
                Overtime = new TimeSpan(0, (int)dbDailyAttendances.Sum(aa => aa.OTMin), 0);
                int hours = (int)Overtime.TotalHours;
                overtime = hours.ToString();
            }
            row = row + dutytime + "\t"
            + shifttime + "\t"
            + timein + "\t"
            + timeout + "\t"
            + work + "\t"
            + latein + "\t"
            + lateout + "\t"
            + earlyin + "\t"
            + earlyout + "\t"
            + overtime + "\t";
            return row;
        }

        public List<VMDailyAttSummary> GetConvertedDailyAttSummary(List<VAT_DailyAttendance> attdata)
        {
            List<VMDailyAttSummary> vmDailyAttSummaries = new List<VMDailyAttSummary>();
            // Get locations
            foreach (var attDate in attdata.Select(aa => aa.AttDate).Distinct().ToList())
            {
                foreach (var locid in attdata.Where(aa => aa.AttDate == attDate).Select(aa => aa.LocationID).Distinct().ToList())
                {
                    //get departments
                    foreach (var deptid in attdata.Where(aa => aa.LocationID == locid && aa.AttDate == attDate).Select(aa => aa.OUID).Distinct().ToList())
                    {
                        // count variables
                        VMDailyAttSummary vmDailyAttSummary = new VMDailyAttSummary();
                        // Get List of Att data
                        List<VAT_DailyAttendance> dbVAT_DailyAttendance = attdata.Where(aa => aa.AttDate == attDate && aa.LocationID == locid && aa.OUID == deptid).ToList();
                        vmDailyAttSummary.LocationID = dbVAT_DailyAttendance.First().LocationID;
                        vmDailyAttSummary.LocationName = dbVAT_DailyAttendance.First().LocationName;
                        vmDailyAttSummary.OUID = dbVAT_DailyAttendance.First().OUID;
                        vmDailyAttSummary.OUName = dbVAT_DailyAttendance.First().OUName;
                        vmDailyAttSummary.AttDate = dbVAT_DailyAttendance.First().AttDate;
                        vmDailyAttSummary.TotalEmps = dbVAT_DailyAttendance.Count();
                        vmDailyAttSummary.PresentEmps = dbVAT_DailyAttendance.Where(aa => aa.PDays > 0).Sum(aa => aa.PDays);
                        vmDailyAttSummary.RestEmps = dbVAT_DailyAttendance.Where(aa => aa.DutyCode == "R").Count();
                        vmDailyAttSummary.AbsentEmps = dbVAT_DailyAttendance.Sum(aa => aa.AbDays);
                        vmDailyAttSummary.LeaveEmps = dbVAT_DailyAttendance.Sum(aa => aa.LeaveDays);
                        vmDailyAttSummary.LateIN = dbVAT_DailyAttendance.Where(aa => aa.LateIn > 0).Count();
                        vmDailyAttSummary.LateOut = dbVAT_DailyAttendance.Where(aa => aa.LateOut > 0).Count();
                        vmDailyAttSummary.EarlyIN = dbVAT_DailyAttendance.Where(aa => aa.EarlyIn > 0).Count();
                        vmDailyAttSummary.EarlyOut = dbVAT_DailyAttendance.Where(aa => aa.EarlyOut > 0).Count();
                        vmDailyAttSummary.TimeBasedJC = dbVAT_DailyAttendance.Where(aa => aa.PDays > 0 && aa.JCMins != aa.ShifMin && aa.JCMins > 0).Count();
                        vmDailyAttSummary.DateBasedJC = dbVAT_DailyAttendance.Where(aa => aa.PDays > 0 && aa.JCMins == aa.ShifMin && aa.JCMins > 0).Count();
                        vmDailyAttSummaries.Add(vmDailyAttSummary);
                    }
                }
            }
            return vmDailyAttSummaries;
        }
        //public List<VMEmployeeAttendanceOther> GetEmployeeAttendanceOther(List<VAT_DailyAttendanceOther> dbEmpOtherAttendances)
        //{
        //    List<VMEmployeeAttendanceOther> vmEmployeeAttendanceOthers = new List<VMEmployeeAttendanceOther>();
        //    foreach (var dbEmpOtherAttendance in dbEmpOtherAttendances)
        //    {
        //        VMEmployeeAttendanceOther vmEmployeeAttendanceOther = new VMEmployeeAttendanceOther();
        //        vmEmployeeAttendanceOther.PEmployeeID = dbEmpOtherAttendance.PEmployeeID;
        //        vmEmployeeAttendanceOther.OEmpID = dbEmpOtherAttendance.OEmpID;
        //        vmEmployeeAttendanceOther.EmployeeName = dbEmpOtherAttendance.EmployeeName;
        //        vmEmployeeAttendanceOther.TimeIn = dbEmpOtherAttendance.EntryTime;
        //        vmEmployeeAttendanceOther.TimeOut = dbEmpOtherAttendance.EntryTime;
        //        vmEmployeeAttendanceOther.WorkMins = CalculateTotalMins(vmEmployeeAttendanceOther.TimeIn, vmEmployeeAttendanceOther.TimeOut);
        //        vmEmployeeAttendanceOthers.Add(vmEmployeeAttendanceOther);
        //    }
        //    return vmEmployeeAttendanceOthers;
        //}

        //private short CalculateTotalMins(DateTime? timeIn, DateTime? timeOut)
        //{
        //    var mins = (TimeSpan)(timeIn - timeOut);
        //    double _workHours = mins.TotalHours;
        //    var WorkMin = (short)(mins.TotalMinutes);
        //    return WorkMin;
        //}
    }
}
