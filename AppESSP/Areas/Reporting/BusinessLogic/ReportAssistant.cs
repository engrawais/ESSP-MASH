using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppESSP.Areas.Reporting.BusinessLogic
{
    public static class ReportAssistant
    {
        public static List<VMLeaveBalance> GetYearlyLeaveBalance(List<VHR_EmployeeProfile> dbEmps, FinancialYear dbFinYear, List<LeaveQuotaYear> dbLeaveQuotaYears, List<LeavePolicy> dbLeavePolicies,
            List<LeaveData> dbLeaveDatas, ABESSPEntities db)
        {
            List<VMLeaveBalance> vmLeaveBalanceList = new List<VMLeaveBalance>();
            foreach (var dbEmp in dbEmps)
            {
                float TotalLvDays = 21;
                VMLeaveBalance vmLeaveBalance = new VMLeaveBalance();
                if (dbEmp.ALPolicyID != null && dbLeavePolicies.Where(aa => aa.PLeavePolicyID == dbEmp.ALPolicyID).Count() > 0)
                    TotalLvDays = (float)dbLeavePolicies.First(aa => aa.PLeavePolicyID == dbEmp.ALPolicyID).TotalDays;
                vmLeaveBalance.PEmployeeID = dbEmp.PEmployeeID;
                vmLeaveBalance.EmployeeName = dbEmp.EmployeeName;
                vmLeaveBalance.DOJ = dbEmp.DOJ;
                vmLeaveBalance.ServiceLength = GetServiceLength(dbEmp.DOJ, dbEmp.ResignDate);
                if (dbEmp.Status == "Resigned")
                {
                    vmLeaveBalance.OEmpID = dbEmp.OEmpID + " (Resigned Date: " + dbEmp.ResignDate.Value.ToString("dd-MM-yyyy") + ")";
                    int PayrollPeriodIDEnd = ATAssistant.GetPayrollPeriodIDEnd(dbEmp.ResignDate.Value, db.PayrollPeriods.ToList());
                    List<MonthData> monthDatas = db.MonthDatas.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.AbsentDays > 0).ToList();
                    if (monthDatas.Count() > 0)
                        vmLeaveBalance.Absents = (float)monthDatas.Sum(aa => aa.AbsentDays);

                }
                else
                    vmLeaveBalance.OEmpID = dbEmp.OEmpID;
               // vmLeaveBalance.OUName = dbEmp.OUName;
                vmLeaveBalance.LocationName = dbEmp.LocationName;
                vmLeaveBalance.JobTitleName = dbEmp.JobTitleName;
                vmLeaveBalance.EmploymentTypeName = dbEmp.EmploymentTypeName;
                vmLeaveBalance.LWOP = dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == false).Count() + (dbLeaveDatas.Where(aa => aa.EmpID == dbEmp.PEmployeeID && aa.HalfLeave == true).Count() / 2);
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // AL
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmLeaveBalance.TotalAL = dbLeaveQuotaYear.YearlyTotal;
                    vmLeaveBalance.BalanceAL = dbLeaveQuotaYear.YearlyRemaining;
                    vmLeaveBalance.AvailAL = vmLeaveBalance.TotalAL - vmLeaveBalance.BalanceAL;
                    if (dbEmp.Status == "Resigned")
                    {
                        // Check for if employee join in same financial year
                        float Divider = 1;
                        float WorkingDays = 0;
                        if (dbEmp.DOJ.Value >= dbFinYear.FYStartDate)
                        {
                            Divider = (dbFinYear.FYEndDate.Value - dbEmp.DOJ.Value).Days + 1;
                            WorkingDays = (dbEmp.ResignDate.Value - dbEmp.DOJ.Value).Days + 1;
                        }
                        else
                        {
                            Divider = 365;
                            WorkingDays = (dbEmp.ResignDate.Value - dbFinYear.FYStartDate.Value).Days + 1;
                        }
                        float ProRataLeave = (float)(vmLeaveBalance.TotalAL * WorkingDays) / Divider;
                        double decimalValue = Math.Round((double)ProRataLeave, 1);
                        vmLeaveBalance.ProrataAL = decimalValue;
                    }
                }
                if (dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).Count() > 0) // ACCU
                {
                    LeaveQuotaYear dbLeaveQuotaYear = dbLeaveQuotaYears.Where(aa => aa.EmployeeID == dbEmp.PEmployeeID && aa.LeaveTypeID == 1).First();
                    vmLeaveBalance.TotalAccum = dbLeaveQuotaYear.CFFromLastYear;
                    vmLeaveBalance.BalanceAccum = dbLeaveQuotaYear.CFRemaining;
                    vmLeaveBalance.AvailAccum = vmLeaveBalance.TotalAccum - vmLeaveBalance.BalanceAccum;
                    vmLeaveBalance.ProrataAccum = vmLeaveBalance.BalanceAccum;
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
                vmLeaveBalanceList.Add(vmLeaveBalance);
            }



            return vmLeaveBalanceList;
        }

        private static string GetServiceLength(DateTime? dOJ, DateTime? resignDate)
        {
            if (resignDate == null)
                resignDate = DateTime.Today;
            if (dOJ == null)
                dOJ = DateTime.Today;
            DateDifference dateDifference = new DateDifference(dOJ.Value, resignDate.Value);
            return dateDifference.ToString();
        }

        public static List<VHR_EmployeeProfile> GetEmployeeInfo(VMLoggedUser loggedUser, List<VHR_EmployeeProfile> dbEmps)
        {
            List<VHR_EmployeeProfile> list = new List<VHR_EmployeeProfile>();
            List<VHR_EmployeeProfile> tempList = new List<VHR_EmployeeProfile>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                list = dbEmps.ToList();
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    list = dbEmps.ToList();
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        tempList.AddRange(list.Where(aa => aa.LocationID == userLocaion.LocationID));
                    }
                    list = tempList.ToList();
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                list = EmployeeLM.GetReportingEmployees(dbEmps.ToList(), loggedUser);
            }
            return list.OrderBy(aa => aa.EmployeeName).ToList();
        }
    }
    public class DateDifference
    {
        /// <summary>
        /// defining Number of days in month; index 0=> january and 11=> December
        /// february contain either 28 or 29 days, that's why here value is -1
        /// which wil be calculate later.
        /// </summary>
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// contain from date
        /// </summary>
        private DateTime fromDate;

        /// <summary>
        /// contain To Date
        /// </summary>
        private DateTime toDate;

        /// <summary>
        /// this three variable for output representation..
        /// </summary>
        private int year;
        private int month;
        private int day;

        public DateDifference(DateTime d1, DateTime d2)
        {
            int increment;

            if (d1 > d2)
            {
                this.fromDate = d2;
                this.toDate = d1;
            }
            else
            {
                this.fromDate = d1;
                this.toDate = d2;
            }

            /// 
            /// Day Calculation
            /// 
            increment = 0;

            if (this.fromDate.Day > this.toDate.Day)
            {
                increment = this.monthDay[this.fromDate.Month - 1];

            }
            /// if it is february month
            /// if it's to day is less then from day
            if (increment == -1)
            {
                if (DateTime.IsLeapYear(this.fromDate.Year))
                {
                    // leap year february contain 29 days
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (this.toDate.Day + increment) - this.fromDate.Day;
                increment = 1;
            }
            else
            {
                day = this.toDate.Day - this.fromDate.Day;
            }

            ///
            ///month calculation
            ///
            if ((this.fromDate.Month + increment) > this.toDate.Month)
            {
                this.month = (this.toDate.Month + 12) - (this.fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                this.month = (this.toDate.Month) - (this.fromDate.Month + increment);
                increment = 0;
            }

            ///
            /// year calculation
            ///
            this.year = this.toDate.Year - (this.fromDate.Year + increment);

        }

        public override string ToString()
        {
            //return base.ToString();
            return this.year + " Year(s), " + this.month + " month(s), " + this.day + " day(s)";
        }

        public int Years
        {
            get
            {
                return this.year;
            }
        }

        public int Months
        {
            get
            {
                return this.month;
            }
        }

        public int Days
        {
            get
            {
                return this.day;
            }
        }

    }
}