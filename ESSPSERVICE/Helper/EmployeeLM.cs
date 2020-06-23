using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Helper
{
    public static class EmployeeLM
    {
        public static List<VHR_EmployeeProfile> GetReportingEmployees(List<VHR_EmployeeProfile> emps, VMLoggedUser LoggedInUser)
        {
            List<VHR_EmployeeProfile> nEmps = emps.Where(aa => aa.LineManagerID == LoggedInUser.PUserID).ToList();

            List<VHR_EmployeeProfile> rEmps = GetReportingToEmps(emps, nEmps);
            nEmps.AddRange(rEmps);
            if (rEmps.Count > 0)
            {
                while (true)
                {
                    rEmps = GetReportingToEmps(emps, rEmps).ToList();
                    nEmps.AddRange(rEmps);
                    if (rEmps.Count == 0)
                        break;
                        //return nEmps;
                    if (nEmps.Count() >= emps.Count())
                    {
                        break;

                    }
                }
            }
            // Add LoggedInUser Employee object in list
            if (nEmps.Where(aa => aa.PEmployeeID == LoggedInUser.UserEmpID).Count() == 0)
                nEmps.AddRange(emps.Where(aa => aa.PEmployeeID == LoggedInUser.UserEmpID).ToList());
            return nEmps;
        }
        private static List<VHR_EmployeeProfile> GetReportingToEmps(List<VHR_EmployeeProfile> emps, List<VHR_EmployeeProfile> checkemps)
        {
            List<VHR_EmployeeProfile> rEmps = new List<VHR_EmployeeProfile>();
            foreach (var emp in checkemps)
            {
                if(emp.PUserID!=null)
                    rEmps.AddRange(emps.Where(aa => aa.LineManagerID == emp.PUserID).ToList());

            }
            return rEmps;

        }
    }
}
