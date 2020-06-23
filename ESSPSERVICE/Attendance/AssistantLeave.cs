using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Assitant leave is a generic and static class used for the leave application that will be used in both Leave Application and ESSP Leave Application.
    /// </summary>
    public static class AssistantLeave
    {
        /// <summary>
        /// This method defines the policy for the employee that is applied through  employee edit view in the Human resource Area
        /// the Policies are different for both A1 and BCl employees that is why a generic method is created for overall employees to be called through a 
        /// single method
        /// </summary>
        /// <param name="lvPolicy"> Get the Leave Policy details of the employe in the Leave Policy table</param>
        /// <param name="emp"> Get the employee whose policy is to checked </param>
        /// <returns></returns>
        public static bool EmployeeEligbleForLeave(LeavePolicy lvPolicy, List<VHR_EmployeeProfile> emp)
        {
            try
            {
                bool check = false;
                if (lvPolicy.ActiveAfterJoinDate == true)
                {
                    check = true;
                }
                else if (lvPolicy.ActiveAfterProbation == true)
                {
                    if (emp.FirstOrDefault().Status == "Active")
                        check = true;
                }
                else if (lvPolicy.ActiveAfterCustomDays == true)
                {
                    if (emp.FirstOrDefault().DOJ != null)
                    {
                        double daysFromJoining = (DateTime.Today.Date - emp.FirstOrDefault().DOJ.Value.Date).TotalDays;
                        if (daysFromJoining > lvPolicy.CustomDays)
                        {
                            check = true;
                        }
                    }
                }

                return check;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Get the leave type and their respective Policies.
        /// </summary>
        /// <param name="_emp"></param>
        /// <param name="LeaveType"></param>
        /// <param name="leavePolicies"></param>
        /// <returns></returns>
        public static LeavePolicy GetEmployeeLeavePolicyID(List<VHR_EmployeeProfile> _emp, byte LeaveType, List<LeavePolicy> leavePolicies)
        {
            try
            {
                short empLvPolivyID = 0;
                LeavePolicy lvPolicy = new LeavePolicy();
                //check emp leave type
                switch (LeaveType)
                {
                    case 2:///1	Casual
                        if (_emp.FirstOrDefault().CLPolicyID != null)
                        {
                            empLvPolivyID = (short)_emp.FirstOrDefault().CLPolicyID;
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.PLeavePolicyID == empLvPolivyID);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 3://2	Sick
                        if (_emp.FirstOrDefault().SLPolicyID != null)
                        {
                            empLvPolivyID = (short)_emp.FirstOrDefault().SLPolicyID;
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.PLeavePolicyID == empLvPolivyID);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 1://3	Annual
                        if (_emp.FirstOrDefault().ALPolicyID != null)
                        {
                            empLvPolivyID = (short)_emp.FirstOrDefault().ALPolicyID;
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.PLeavePolicyID == empLvPolivyID);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 4://8	CPL
                        if (_emp.FirstOrDefault().CPLPolicyID != null)
                        {
                            empLvPolivyID = (short)_emp.FirstOrDefault().CPLPolicyID;
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.PLeavePolicyID == empLvPolivyID);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 5://8	LWOP
                        if (leavePolicies.Where(aa => aa.LeaveTypeID == 5).Count() > 0)
                        {
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.LeaveTypeID == 5);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 11://8	ACADEMIC
                        if (leavePolicies.Where(aa => aa.LeaveTypeID == 11).Count() > 0)
                        {
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.LeaveTypeID == 11);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    case 12://8	CME
                        if (leavePolicies.Where(aa => aa.LeaveTypeID == 12).Count() > 0)
                        {
                            lvPolicy = leavePolicies.FirstOrDefault(aa => aa.LeaveTypeID == 12);
                        }
                        else
                        {
                            lvPolicy = GetNoPolicy();
                        }
                        break;
                    default://

                        lvPolicy = GetNoPolicy();
                        break;
                }
                return lvPolicy;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// For those leave types for which no leave policy is required 
        /// </summary>
        /// <returns></returns>
        public static LeavePolicy GetNoPolicy()
        {
            try
            {
                LeavePolicy AttLvPolicy = new LeavePolicy();
                AttLvPolicy.LeaveTypeID = 0;
                AttLvPolicy.UpdateBalance = false;
                return AttLvPolicy;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///General class for the Payroll Period if user is applying the leave for out of the give
        /// </summary>
        /// <param name="prPeriods">List of all the payrolls</param>
        /// <param name="CurrentDay">Current Date time</param>
        /// <returns></returns>
        public static int GetPayRollPeriodID(List<PayrollPeriod> prPeriods, DateTime CurrentDay)
        {
            int val = 0;
            prPeriods = prPeriods.Where(aa => aa.PRStartDate <= CurrentDay && aa.PREndDate >= CurrentDay).ToList();
            if (prPeriods.Count > 0)
                val = prPeriods.First(aa => aa.PRStartDate <= CurrentDay && aa.PREndDate >= CurrentDay).PPayrollPeriodID;
            return val;
        }
        /// <summary>
        /// General class for the Financial year if user is applying the leave for out of the given date of current Finanacial Year
        /// </summary>
        /// <param name="FinYears">List of all the Financial Years </param>
        /// <param name="CurrentDay">Current Date time</param>
        /// <returns></returns>
        public static int GetFinancialYearID(List<FinancialYear> FinYears, DateTime CurrentDay)
        {
            int val = 0;
            FinYears = FinYears.Where(aa => aa.FYStartDate <= CurrentDay && aa.FYEndDate >= CurrentDay).ToList();
            if (FinYears.Count > 0)
                val = FinYears.First(aa => aa.FYStartDate <= CurrentDay && aa.FYEndDate >= CurrentDay).PFinancialYearID;
            return val;
        }
        /// <summary>
        /// Adding Balace in the leave Quota Year Table from the application 
        /// </summary>
        /// <param name="_lvConsumed"></param>
        /// <param name="lvappl"></param>
        /// <param name="atLQP"></param>
        /// <returns></returns>
        public static LeaveQuotaPeriod AddBalancceMonthQuota(List<LeaveQuotaYear> _lvConsumed, LeaveApplication lvappl, LeaveQuotaPeriod atLQP)
        {
            atLQP.ConsumedDays = atLQP.ConsumedDays + lvappl.NoOfDays;
            atLQP.RemainingDays = atLQP.StartNoOfDays - atLQP.ConsumedDays;
            return atLQP;
        }

        /// <summary>
        /// fot those employee of A1 whose leave should be deducted from sturday and sundays BCl employee are not included in it because 
        /// Saturday and sunday are not included in the leave days .
        /// </summary>
        /// <param name="ss">Gets the shift days and changes their value to zero if there is a holiday</param>
        /// <param name="dts"></param>
        /// <returns></returns>
        public static bool CurrentDayNotRest(Shift ss, DateTime dts)
        {
            bool holiday = false;
            switch (dts.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (ss.MonMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Tuesday:
                    if (ss.TueMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Wednesday:
                    if (ss.WedMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Thursday:
                    if (ss.ThuMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Friday:
                    if (ss.FriMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Saturday:
                    if (ss.SatMin == 0)
                        holiday = true;
                    break;
                case DayOfWeek.Sunday:
                    if (ss.SunMin == 0)
                        holiday = true;
                    break;
            }

            return holiday;
        }
    }
}
