using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Generic
{
    public static class AppAssistant
    {
        public static List<int?> GetSeperateIDsFromMultiSelect(string array)
        {
            var numbers = array.Split(',').Select(Int32.Parse).ToList();
            List<int?> numlist = new List<int?>();

            foreach (var item in numbers.ToList())
            {
                numlist.Add(Convert.ToInt32(item));
            }
            return numlist;
        }
        public static List<AppUser> GetLineManagers(List<AppUser> list)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            {
                AppUser vmlist = new AppUser();
                vmlist.PUserID = 0;
                vmlist.UserName = "";
                AppUsers.Add(vmlist);
            }
            foreach (var item in list)
            {
                AppUser au = new ESSPCORE.EF.AppUser();
                au.PUserID = item.PUserID;
                au.UserName = item.Employee.EmployeeName;
                au.UserName = item.Employee.OEmpID + "-" + item.Employee.EmployeeName + "(" + item.Employee.JobTitle.JobTitleName + ")";
                AppUsers.Add(au);
            }
            return AppUsers.OrderBy(aa => aa.UserName).ToList();
        }
        public static List<VHR_AppUser> GetLineManagerss(List<VHR_AppUser> list)
        {
            List<VHR_AppUser> AppUsers = new List<VHR_AppUser>();
            {
                VHR_AppUser vmlist = new VHR_AppUser();
                vmlist.PUserID = 0;
                vmlist.UserName = "";
                AppUsers.Add(vmlist);
            }
            foreach (var item in list)
            {
                VHR_AppUser au = new ESSPCORE.EF.VHR_AppUser();
                au.PUserID = item.PUserID;
                au.UserName = item.UserEmployeeName;
                au.UserName = item.OEmpID + "-" + item.UserEmployeeName + "(" + item.UserJobTitleName + ")";
                AppUsers.Add(au);
            }
            return AppUsers.OrderBy(aa => aa.UserName).ToList();
        }
        private static string ExceptionLogFile = "D:\\" + "EL" + DateTime.Today.ToString("ddMMyy");
        public static void WriteToExceptionLogFile(string strMessage)
        {
            try
            {

                string line = DateTime.Now.ToString() + " | ";
                line += strMessage;
                FileStream fs = new FileStream(ExceptionLogFile, FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter swFromFileStream = new StreamWriter(fs);
                swFromFileStream.WriteLine(line);
                swFromFileStream.Flush();
                swFromFileStream.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public static DateTime MaxDate { get { return DateTime.Today.AddDays(60); } set { } }
        public static DateTime MinDate { get { return DateTime.Today.AddDays(-60); } set { } }

        public static int GetEmployeeWithNoLeaveQuota(List<LeaveQuotaYear> dbLeaveQuotaYear, List<VHR_EmployeeProfile> emps)
        {
            int count = 0;
            foreach (var emp in emps)
            {
                if (dbLeaveQuotaYear.Where(aa => aa.EmployeeID == emp.PEmployeeID).Count() <= 1)
                    count++;
            }
            return count;
        }
    }
}
