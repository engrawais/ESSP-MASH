using AppESSP.Areas.Reporting.BusinessLogic;
using AppESSP.Areas.Reporting.BusinessLogic.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppESSP
{
    public partial class ReportContainer : System.Web.UI.Page
    {
        ABESSPEntities db = new ABESSPEntities();
        public string DateTitle { get; set; }
        public string ReportTitle { get; set; }
        public string ReportFooter { get; set; }
        public string ReportPath { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
                VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
                DataTable dataTable = new DataTable();
                ReportPath = "~/Areas/Reporting/RDLC/Attendance/";
                string DateFromS = vmf.DateFrom.Value.ToString("dd-MMM-yyyy");
                string DateToS = vmf.DateTo.Value.ToString("dd-MMM-yyyy");
                string query = QueryBuilder.GetReportQueryForLoggedUser(LoggedInUser, db.VHR_EmployeeProfile.ToList());
                if (query != "")
                    query = " and " + query;

                int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, db.FinancialYears.OrderByDescending(aa => aa.PFinancialYearID).ToList());
                String reportName = Request.QueryString["reportname"];
                switch (reportName)
                {
                    case "leaveBalanceEmp_report":    ///YEARLY LEAVE BALANCE REPORT
                        List<VHR_EmployeeProfile> dbEmps = ReportAssistant.GetEmployeeInfo(LoggedInUser, db.VHR_EmployeeProfile.ToList());
                        List<VHR_EmployeeProfile> dbTempEmps = new List<VHR_EmployeeProfile>();
                        dbEmps = AttendanceFilter.ReportsFilterImplementation(vmf, dbTempEmps, dbEmps);
                        List<LeavePolicy> dbLeavePolicies = db.LeavePolicies.ToList();
                        ReportTitle = "Yearly Leave Ledger Report";
                        ReportPath = ReportPath + "YEmpProRataBalance.rdlc";
                        FinancialYear dbFinancialYear = db.FinancialYears.Where(aa => aa.PFinancialYearID == FinancialYearID).First();
                        DateTitle = dbFinancialYear.FYStartDate.Value.ToString("dd-MM-yyyy") + " TO " + dbFinancialYear.FYEndDate.Value.ToString("dd-MM-yyyy");
                        List<LeaveQuotaYear> dbLeaveQuotaYear = db.LeaveQuotaYears.Where(aa => aa.FinancialYearID == dbFinancialYear.PFinancialYearID).ToList();
                        List<LeaveData> dbLeaveDatas = db.LeaveDatas.Where(aa => aa.AttDate >= dbFinancialYear.FYStartDate && aa.AttDate <= dbFinancialYear.FYEndDate && aa.LeaveTypeID == 5).ToList();
                        DateTitle = "Leave Balance Statement from Year: " + dbFinancialYear.FYName;
                        ReportFooter = "System Generated Report by: " + LoggedInUser.UserName + " at " + DateTime.Now.ToString("hh:mm tt");
                        reportViewer = LoadReport_vMLeaveBalanceWithSR(ReportAssistant.GetYearlyLeaveBalance(dbEmps, dbFinancialYear, dbLeaveQuotaYear, dbLeavePolicies, dbLeaveDatas, db), reportViewer);
                        break;
                }
            }
        }
        private ReportViewer LoadReport_vMLeaveBalanceWithSR(List<VMLeaveBalance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VMLeaveBalance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            //{
            //    string EmpNo = e.Parameters["EmployeeID"].Values.First().ToString();
            //    VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            //    int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, db.FinancialYears.ToList());
            //    FinancialYear dbFinancialYear = db.FinancialYears.Where(aa => aa.PFinancialYearID == FinancialYearID).First();
            //    DataTable dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where OEmpID='" + EmpNo + "' and (AttDate >= " + "'" + dbFinancialYear.FYStartDate.Value.ToString("yyyy-MM-ddd") + "'" + " and AttDate <= " + "'" + dbFinancialYear.FYEndDate.Value.ToString("yyyy-MM-ddd") + "'" + " )");
            //    List<VAT_LeaveData> VATLeaveData = dataTable.ToList<VAT_LeaveData>();
            //    e.DataSources.Add(new ReportDataSource("DataSet1", VATLeaveData));
            //};
            rv.LocalReport.Refresh();
            return rv;
        }
        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            string EmpNo = e.Parameters["PEmployeeID"].Values.First().ToString();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, db.FinancialYears.ToList());
            FinancialYear dbFinancialYear = db.FinancialYears.Where(aa => aa.PFinancialYearID == FinancialYearID).First();
            DataTable dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where EmpID='" + EmpNo + "' and (AttDate >= " + "'" + dbFinancialYear.FYStartDate.Value.ToString("yyyy-MM-dd") + "'" + " and AttDate <= " + "'" + dbFinancialYear.FYEndDate.Value.ToString("yyyy-MM-dd") + "'" + " )");
            List<VAT_LeaveData> VATLeaveData = dataTable.ToList<VAT_LeaveData>();
            e.DataSources.Add(new ReportDataSource("DataSet1", VATLeaveData));

        }
    }
}