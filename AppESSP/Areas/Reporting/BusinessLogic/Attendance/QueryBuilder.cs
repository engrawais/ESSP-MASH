using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AppESSP.Areas.Reporting.BusinessLogic.Attendance
{
    public static class QueryBuilder
    {
        public static DataTable GetValuesfromDB(string query)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ABESSPConnectionString"].ConnectionString);
            using (SqlCommand cmdd = Conn.CreateCommand())
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmdd))
                {
                    cmdd.CommandText = query;
                    cmdd.CommandType = CommandType.Text;
                    Conn.Open();
                    sda.Fill(dt);
                    Conn.Close();
                }
            }
            return dt;
        }
        public static string GetReportQueryForLoggedUser(VMLoggedUser loggedUser,List<VHR_EmployeeProfile> emps)
        {
            string query = " (";
            List<int?> ids = new List<int?>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                query = "";
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        ids.Add(userLocaion.LocationID);
                    }
                    if (ids.Count == 1)
                    {
                        query = query +" LocationID = "+ ids[0]+" )";
                    }
                    else if (ids.Count > 1)
                    {
                        query = query + " LocationID in (";
                        for (int i = 0; i < ids.Count - 1; i++)
                        {
                            query = query + ids[i] + " ,";
                        }
                        query = query + ids[ids.Count - 1]+"))";
                    }
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {
                
               foreach(var empid in EmployeeLM.GetReportingEmployees(emps.ToList(), loggedUser).Select(aa=>aa.PEmployeeID).Distinct())
                {
                    ids.Add(empid);
                }
                if (ids.Count == 1)
                {
                    query = query + " EmpID = " + ids[0] + " )";
                }
                else if (ids.Count > 1)
                {
                    query = query + " EmpID in (";
                    for (int i = 0; i < ids.Count - 1; i++)
                    {
                        query = query + ids[i] + " ,";
                    }
                    query = query + ids[ids.Count - 1] + "))";
                }
            }
            return query;
        }
        public static string GetReportQueryForLoggedUser2(VMLoggedUser loggedUser, List<VHR_EmployeeProfile> emps)
        {
            string query = "and  (";
            List<int?> ids = new List<int?>();
            if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.AllEmployees))
            {
                query = "";
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.LocationBased))
            {
                if (loggedUser.UserLoctions != null)
                {
                    foreach (var userLocaion in loggedUser.UserLoctions)
                    {
                        ids.Add(userLocaion.LocationID);
                    }
                    if (ids.Count == 1)
                    {
                        query = query + " LocationID = " + ids[0] + " )";
                    }
                    else if (ids.Count > 1)
                    {
                        query = query + " LocationID in (";
                        for (int i = 0; i < ids.Count - 1; i++)
                        {
                            query = query + ids[i] + " ,";
                        }
                        query = query + ids[ids.Count - 1] + "))";
                    }
                }
            }
            else if (loggedUser.UserAccessTypeID == Convert.ToInt32(UserAccessType.Normal))
            {

                foreach (var empid in EmployeeLM.GetReportingEmployees(emps.ToList(), loggedUser).Select(aa => aa.PEmployeeID).Distinct())
                {
                    ids.Add(empid);
                }
                if (ids.Count == 1)
                {
                    query = query + " PEmployeeID = " + ids[0] + " )";
                }
                else if (ids.Count > 1)
                {
                    query = query + " PEmployeeID in (";
                    for (int i = 0; i < ids.Count - 1; i++)
                    {
                        query = query + ids[i] + " ,";
                    }
                    query = query + ids[ids.Count - 1] + "))";
                }
            }
            return query;
        }

        internal static string GetRatingQuery(VMSelectedFilter vmf)
        {
            string query = "";
            if (vmf.SelectedRating.Count == 1)
            {
                query = query + " = " + vmf.SelectedRating[0].FilterID + "";
            }
            else if (vmf.SelectedRating.Count > 1)
            {
                query = query + "  in (";
                for (int i = 0; i < vmf.SelectedRating.Count - 1; i++)
                {
                    query = query + vmf.SelectedRating[i].FilterID + " ,";
                }
                query = query + vmf.SelectedRating[vmf.SelectedRating.Count - 1].FilterID + ")";
            }
            return query;
        }
    }
}