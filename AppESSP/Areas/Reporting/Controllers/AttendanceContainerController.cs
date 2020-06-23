using AppESSP.Areas.Reporting.BusinessLogic;
using AppESSP.Areas.Reporting.BusinessLogic.Attendance;
using AppESSP.Controllers;
using ESSPCORE.Attendance;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPCORE.Reporting;
using ESSPSERVICE.Attendance;
using ESSPSERVICE.Generic;
using ESSPSERVICE.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AppESSP.Areas.Reporting.Controllers
{
    public class AttendanceContainerController : BaseController
    {
        public string DateTitle { get; set; }
        public string ReportTitle { get; set; }
        public string CompanyHeader { get; set; }
        public string ReportFooter { get; set; }
        public string ReportPath { get; set; }
        public string PerformanceReportPath { get; set; }
        IDDService DDService;
        IAttReportingService AttReportingService;
        // IEntityService<VAT_DailyAttendanceOther> VATDailAttendanceOtherEntityService;
        public AttendanceContainerController(IDDService dDService, IAttReportingService attReportingService)
        {
            DDService = dDService;
            AttReportingService = attReportingService;

        }
        // GET: Reporting/AttendanceContainer
        public ActionResult Index()
        {
            String reportName = Request.QueryString["reportName"];
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            DataTable dataTable = new DataTable();
            ReportPath = "~/Areas/Reporting/RDLC/Attendance/";
            PerformanceReportPath = "~/Areas/Reporting/RDLC/Performance/";
            string DateFromS = vmf.DateFrom.Value.ToString("dd-MMM-yyyy");
            string DateToS = vmf.DateTo.Value.ToString("dd-MMM-yyyy");
            string query = QueryBuilder.GetReportQueryForLoggedUser(LoggedInUser, DDService.GetEmployeeInfo());
            if (query != "")
                query = " and " + query;
            DateTitle = DateFromS + " TO " + DateToS;
            if (vmf.SelectedCompany.Count() == 1)
            {
                if (vmf.SelectedCompany.First().FilterID == 1)
                    CompanyHeader = "BCL - ";
                else if (vmf.SelectedCompany.First().FilterID == 2)
                    CompanyHeader = "A1 - ";
                else
                    CompanyHeader = "Other - ";
            }
            if (vmf.SelectedCompany.Count() == 0 || vmf.SelectedCompany == null)
            {
                CompanyHeader = "";
            }
            // Initilize Report
            ReportViewer reportViewer = new ReportViewer()
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Pixel(1200),
                Height = Unit.Pixel(1300)
            };

            int PayrollPeriodIDStart = ATAssistant.GetPayrollPeriodIDStart((DateTime)vmf.DateFrom, DDService.GetAllPayrollPeriod());
            int PayrollPeriodIDEnd = ATAssistant.GetPayrollPeriodIDEnd((DateTime)vmf.DateTo, DDService.GetAllPayrollPeriod());
            int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, DDService.GetFinancialYear());

            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.DisplayName = CompanyHeader + ReportTitle;
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            reportViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.EnableExternalImages = true;
            ReportFooter = "System Generated Report by: " + LoggedInUser.UserName + " at " + DateTime.Now.ToString("hh:mm tt");
            switch (reportName)
            {
                #region ------Monthly Attendance Reports-------
                case "detailed_att":     //EMPLOYEE DETAILED ATTENDANCE REPORT  CAN BE GENERATED DAILY
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " ) " + query);
                    List<VAT_DailyAttendance> VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    List<VAT_DailyAttendance> TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Employee Detailed Attendence Report";

                    ReportPath = ReportPath + "EmpAttSummary.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "monthly_attendancesheet":   //MONTHLY ATTENDANCE SUMMARY REPORT 26=25 
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_MonthlySheet where (PayrollPeriodID >=" + PayrollPeriodIDStart + " and PayrollPeriodID <=" + PayrollPeriodIDEnd + " )" + query);
                    List<VAT_MonthlySheet> VATMonthlySheet = dataTable.ToList<VAT_MonthlySheet>();
                    List<VAT_MonthlySheet> TMP_VAT_MonthlySheet = new List<VAT_MonthlySheet>();
                    ReportTitle = "Employee Monthly Attendance Report 25-24";
                    ReportPath = ReportPath + "MonthlySheet26-25.rdlc";
                    reportViewer = LoadReport_VATMonthlySheet(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VAT_MonthlySheet, VATMonthlySheet), reportViewer);
                    break;
                case "monthly_attendance":   //MONTHLY ATTENDANCE SUMMARY REPORT 
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_MonthlySummary where (PayrollPeriodID >=" + PayrollPeriodIDStart + " and PayrollPeriodID <=" + PayrollPeriodIDEnd + " )" + query);
                    List<VAT_MonthlySummary> VATMonthlySummary = dataTable.ToList<VAT_MonthlySummary>();
                    List<VAT_MonthlySummary> TMP_VATMonthlySummary = new List<VAT_MonthlySummary>();
                    ReportTitle = "Employee Monthly Attendance Report";
                    ReportPath = ReportPath + "MRSummary.rdlc";
                    reportViewer = LoadReport_VATMonthlySummary(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATMonthlySummary, VATMonthlySummary), reportViewer);
                    break;
                case "overtime_summaryreport": ///MONTHLY OVERTIME SUMMARY REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_MonthlySummary  where ((EncashbaleSingleOT>0 or EncashbaleDoubleOT>0 or CPLConversionOT>0) and PayrollPeriodID=" + PayrollPeriodIDStart + " )" + query);
                    List<VAT_MonthlySummary> VATMonthlySummary1 = dataTable.ToList<VAT_MonthlySummary>();
                    List<VAT_MonthlySummary> TMP_VATMonthlySummary1 = new List<VAT_MonthlySummary>();
                    ReportTitle = "Monthly Overtime Summary Report";
                    ReportPath = ReportPath + "MonthlyOvertimeSummary.rdlc";
                    reportViewer = LoadReport_VATMonthlySummary(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATMonthlySummary1, VATMonthlySummary1), reportViewer);
                    break;
                case "monthly_attendanceedit": ///MONTHLY ATTENDANCE EDIT REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_MonthDataEdit where (PayrollPeriodID >=" + PayrollPeriodIDStart + " and PayrollPeriodID <=" + PayrollPeriodIDEnd + " )" + query);
                    List<VAT_MonthDataEdit> VATMonthDataEdit = dataTable.ToList<VAT_MonthDataEdit>();
                    List<VAT_MonthDataEdit> TMP_VATMonthDataEdit = new List<VAT_MonthDataEdit>();
                    ReportTitle = "Monthly Attendance Edit Report";
                    ReportPath = ReportPath + "MonthlyAttendanceEdit.rdlc";
                    reportViewer = LoadReport_VATMonthDataEdit(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATMonthDataEdit, VATMonthDataEdit), reportViewer);
                    break;
                case "monthly_overtimeapproval": ///MONTHLY OVERTIME APPROVAL REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_OvertimeHistory where (PayrollPeriodID >=" + PayrollPeriodIDStart + " and PayrollPeriodID <=" + PayrollPeriodIDEnd + " )" + query);
                    List<VAT_OvertimeHistory> VATOvertimeHistory = dataTable.ToList<VAT_OvertimeHistory>();
                    List<VAT_OvertimeHistory> TMP_VATOvertimeHistory = new List<VAT_OvertimeHistory>();
                    ReportTitle = "Monthly Overtime Approval History Report";
                    ReportPath = ReportPath + "OvertimeApprovalHistory.rdlc";
                    reportViewer = LoadReport_VATOvertimeHistory(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATOvertimeHistory, VATOvertimeHistory), reportViewer);
                    break;
                #endregion
                #region --------Daily Attendance Reports----------------
                case "consolidated_attendance": //CONSOLIDATED ATTENDANCE REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Employee Consolidated Report";
                    ReportPath = ReportPath + "DRConsolidated.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "daily_attendance_summary":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Daily Attendance Summary Report";
                    ReportPath = ReportPath + "SMDailyAtt.rdlc";
                    reportViewer = LoadReport_DailyAttendanceSummary(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "consolidated_attendance_download":   //CONSOLIDATED ATTENDANCE DOWNLOADER
                    List<VHR_EmployeeProfile> VHREmployeeProfile2 = DDService.GetEmployeeInfo(LoggedInUser);
                    List<VHR_EmployeeProfile> TMP_vHREmployeeProfile2 = new List<VHR_EmployeeProfile>();
                    VHREmployeeProfile2 = AttendanceFilter.ReportsFilterImplementation(vmf, TMP_vHREmployeeProfile2, VHREmployeeProfile2);
                    string val = "";
                    val = AttReportingService.GetDailyReportInExcel(VHREmployeeProfile2, LoggedInUser, Convert.ToDateTime(DateFromS), Convert.ToDateTime(DateToS));
                    if (val != "")
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(val);
                        string fileName = "ConsolidatedReport.xls";
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                    break;
                case "missing_attendance": /// EMPLOYEE'S MISSING ATTENDANCE REPORTS
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where ((TimeIn is null and TimeOut is not null) or (TimeIn is not null and TimeOut is null)) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Employee Missing Attendence Report";
                    ReportPath = ReportPath + "DRMissingAtt.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "multipleinout_attendance": /// EMPLOYEE'S MULTIPLE IN/OUT REPORTS
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendanceDetail  where (Tin1 is not null or Tout1 is not null) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    List<VAT_DailyAttendanceDetail> VATDailyAttendanceDetail = dataTable.ToList<VAT_DailyAttendanceDetail>();
                    List<VAT_DailyAttendanceDetail> TMP_VATDailyAttendanceDetail = new List<VAT_DailyAttendanceDetail>();
                    ReportTitle = "Employee Multiple In/Out Report";
                    ReportPath = ReportPath + "DRMultipleInOut.rdlc";
                    reportViewer = LoadReport_VATDailyAttendanceDetail(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendanceDetail, VATDailyAttendanceDetail), reportViewer);
                    break;
                case "present_attendance": ///PRESENT EMPLOYEE REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (PDays>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Present Employees Report";
                    ReportPath = ReportPath + "DRPresent.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "absent_attendance":  //ABSENT EMPLOYEES REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AbDays>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Absent Employees Report";
                    ReportPath = ReportPath + "DRAbsent.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "latein_attendance":  ///LATE IN EMPLOYEES REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (LateIn>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "LateIn Employees Report";
                    ReportPath = ReportPath + "DRLateIn.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "lateout_attendance":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (LateOut>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "LateOut Employees Report";
                    ReportPath = ReportPath + "DRLateOut.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;

                case "earlyin_attendance":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (EarlyIn>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "EarlyIn Employees Report";
                    ReportPath = ReportPath + "DREarlyIn.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "earlyout_attendance":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (EarlyOut>0) and (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "EarlyOut Employees Report";
                    ReportPath = ReportPath + "DREarlyOut.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;

                case "overtime_attendance":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where ((ApprovedOT>0 or ApprovedDoubleOT>0 or ApprovedCPL>0 ) and AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Employees Overtime Report";
                    ReportPath = ReportPath + "DROverTime.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "edit_attendance":  ////WHO EDITED ATTENDANCE AND WHEN REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttedanceEdit  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " ) " + QueryBuilder.GetReportQueryForLoggedUser2(LoggedInUser, DDService.GetEmployeeInfo()));
                    List<VAT_DailyAttedanceEdit> VATDailyAttedanceEdit = dataTable.ToList<VAT_DailyAttedanceEdit>();
                    List<VAT_DailyAttedanceEdit> TMP_VATDailyAttendanceEdit = new List<VAT_DailyAttedanceEdit>();
                    ReportTitle = "Manual Attendance User Log Report";
                    ReportPath = ReportPath + "EditAttendanceLog.rdlc";
                    reportViewer = LoadReport_VATDailyAttedanceEdit(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendanceEdit, VATDailyAttedanceEdit), reportViewer);
                    break;
                #endregion
                #region -------LEAVE REPORTS------------
                case "leave_report":   /// EMPLOYEEWISE LEAVE REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where  (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    List<VAT_LeaveData> VATLeaveData = dataTable.ToList<VAT_LeaveData>();
                    List<VAT_LeaveData> TMP_VATLeaveData = new List<VAT_LeaveData>();
                    ReportTitle = "Employee wise consolidated Leave Report";
                    ReportPath = ReportPath + "DRLeave.rdlc";
                    reportViewer = LoadReport_VATLeaveData(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATLeaveData, VATLeaveData), reportViewer);
                    break;
                case "leavedatewise_report":      //DATEWISE LEAVE REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " )" + query);
                    VATLeaveData = dataTable.ToList<VAT_LeaveData>();
                    TMP_VATLeaveData = new List<VAT_LeaveData>();
                    ReportTitle = "Date wise Leave Report";
                    ReportPath = ReportPath + "DRLeaveDate.rdlc";
                    reportViewer = LoadReport_VATLeaveData(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATLeaveData, VATLeaveData), reportViewer);
                    break;
                case "leavebalance_report":    ///YEARLY LEAVE BALANCE REPORT
                    List<VHR_EmployeeProfile> VHREmployeeProfile = DDService.GetEmployeeInfo(LoggedInUser);
                    List<VHR_EmployeeProfile> TMP_vHREmployeeProfile = new List<VHR_EmployeeProfile>();
                    VHREmployeeProfile = AttendanceFilter.ReportsFilterImplementation(vmf, TMP_vHREmployeeProfile, VHREmployeeProfile);
                    ReportTitle = "Yearly Leave Ledger Report";
                    ReportPath = ReportPath + "DRLeaveBalance.rdlc";
                    DateTitle = "Yearly Leaves Balance Statement";
                    reportViewer = LoadReport_vMLeaveBalance(AttReportingService.GetYearlyLeaveBalance(VHREmployeeProfile, FinancialYearID), reportViewer);
                    break;
                case "monthlyleavebalance_report": //MONTHLY LEAVE BALANCE REPORT
                    List<VHR_EmployeeProfile> VHREmployeeProfile1 = DDService.GetEmployeeInfo(LoggedInUser);
                    List<VHR_EmployeeProfile> TMP_vHREmployeeProfile1 = new List<VHR_EmployeeProfile>();
                    VHREmployeeProfile = AttendanceFilter.ReportsFilterImplementation(vmf, TMP_vHREmployeeProfile1, VHREmployeeProfile1.Where(aa => aa.Status == "Active").ToList());
                    ReportTitle = "Monthly Leave Ledger Report";
                    ReportPath = ReportPath + "DRMonthlyLeaveData.rdlc";
                    reportViewer = LoadReport_vMMonthlyLeaveBalance(AttReportingService.GetMonthlyLeaveBalance(VHREmployeeProfile, FinancialYearID, Convert.ToDateTime(DateFromS), Convert.ToDateTime(DateToS)), reportViewer);
                    break;
                case "leaveBalanceEmp_report":    ///YEARLY LEAVE BALANCE REPORT
                    VHREmployeeProfile = DDService.GetEmployeeInfo(LoggedInUser);
                    TMP_vHREmployeeProfile = new List<VHR_EmployeeProfile>();
                    VHREmployeeProfile = AttendanceFilter.ReportsFilterImplementation(vmf, TMP_vHREmployeeProfile, VHREmployeeProfile);
                    ReportTitle = "Yearly Leave Ledger Report";
                    ReportPath = ReportPath + "YEmpLeaveBalance.rdlc";
                    FinancialYear dbFinancialYear = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == FinancialYearID).First();
                    DateTitle = "Leave Balance Statement from Year: " + dbFinancialYear.FYName;
                    reportViewer = LoadReport_vMLeaveBalanceWithSR(AttReportingService.GetYearlyLeaveBalance(VHREmployeeProfile, FinancialYearID), reportViewer);
                    break;
                case "cplexpire_report":    ///CPL Expire report REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_CPLBalance  where  (ExpireDate >= " + "'" + DateFromS + "'" + " and ExpireDate <= " + "'" + DateToS + "'" + " and IsExpire = 1)" + query);
                    List<VAT_CPLBalance> VATCPLBalance = dataTable.ToList<VAT_CPLBalance>();
                    List<VAT_CPLBalance> TMP_VATCPLBalance = new List<VAT_CPLBalance>();
                    ReportTitle = "CPL Balance report";
                    ReportPath = ReportPath + "CPLExpire.rdlc";
                    reportViewer = LoadReport_VATCPLBalance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATCPLBalance, VATCPLBalance), reportViewer);
                    break;
                #endregion
                #region ---------MISCELLANEOUS REPORTS-----------------
                case "devicedata_report":  //DEVICE DATA REPORT
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DeviceData  where (EntDate >= " + "'" + DateFromS + "'" + " and EntDate <= " + "'" + DateToS + "'" + " )" + query);
                    List<VAT_DeviceData> VATDeviceData = dataTable.ToList<VAT_DeviceData>();
                    List<VAT_DeviceData> TMP_VATDeviceData = new List<VAT_DeviceData>();
                    ReportTitle = "Device Data Report";
                    ReportPath = ReportPath + "DRDeviceData.rdlc";
                    reportViewer = LoadReport_VATDeviceData(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDeviceData, VATDeviceData), reportViewer);
                    break;
                case "unregatt_report":// Un registered employees
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_DailyAttendance  where (AttDate >= " + "'" + DateFromS + "'" + " and AttDate <= " + "'" + DateToS + "'" + " and LocationID = " + "'" + 144 + "'" + " )");
                    VATDailyAttendance = dataTable.ToList<VAT_DailyAttendance>();
                    TMP_VATDailyAttendance = new List<VAT_DailyAttendance>();
                    ReportTitle = "Un-registered Employee Consolidated Report";
                    ReportPath = ReportPath + "DRUnRegEmpAtt.rdlc";
                    reportViewer = LoadReport_VATDailyAttendance(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_VATDailyAttendance, VATDailyAttendance), reportViewer);
                    break;
                case "employee_record":
                    dataTable = QueryBuilder.GetValuesfromDB("select * from VHR_EmployeeProfile");
                    VHREmployeeProfile = dataTable.ToList<VHR_EmployeeProfile>();
                    TMP_vHREmployeeProfile = new List<VHR_EmployeeProfile>();
                    ReportTitle = "Employee Records";
                    ReportPath = ReportPath + "HREmployeeDetail.rdlc";
                    reportViewer = LoadReport_VHREmployeeProfile(AttendanceFilter.ReportsFilterImplementation(vmf, TMP_vHREmployeeProfile, VHREmployeeProfile), reportViewer);
                    break;
                #endregion
             

            }
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        private ReportViewer LoadReport_DailyAttendanceSummary(List<VAT_DailyAttendance> attdata, ReportViewer rv)
        {
            List<VMDailyAttSummary> attDailySummayList = AttReportingService.GetConvertedDailyAttSummary(attdata);
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VMDailyAttSummary> ie;
            ie = attDailySummayList.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }

        private ReportViewer LoadReport_vMMonthlyLeaveBalance(List<VMMonthlyLeaveBalance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VMMonthlyLeaveBalance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_vMLeaveBalance(List<VMLeaveBalance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VMLeaveBalance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_vMLeaveBalanceWithSR(List<VMLeaveBalance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VMLeaveBalance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.SubreportProcessing += (object sender, SubreportProcessingEventArgs e) =>
            {
                string EmpNo = e.Parameters["EmployeeID"].Values.First().ToString();
                VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
                int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, DDService.GetFinancialYear());
                FinancialYear dbFinancialYear = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == FinancialYearID).First();
                DataTable dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where OEmpID='" + EmpNo + "' and (AttDate >= " + "'" + dbFinancialYear.FYStartDate.Value.ToString("yyyy-MM-ddd") + "'" + " and AttDate <= " + "'" + dbFinancialYear.FYEndDate.Value.ToString("yyyy-MM-ddd") + "'" + " )");
                List<VAT_LeaveData> VATLeaveData = dataTable.ToList<VAT_LeaveData>();
                e.DataSources.Add(new ReportDataSource("DataSet1", VATLeaveData));
            };
            rv.LocalReport.Refresh();
            return rv;
        }
        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            string EmpNo = e.Parameters["EmployeeID"].Values.First().ToString();
            VMSelectedFilter vmf = Session["FiltersModel"] as VMSelectedFilter;
            int FinancialYearID = ATAssistant.GetFinancialYearID((DateTime)vmf.DateTo, DDService.GetFinancialYear());
            FinancialYear dbFinancialYear = DDService.GetFinancialYear().Where(aa => aa.PFinancialYearID == FinancialYearID).First();
            DataTable dataTable = QueryBuilder.GetValuesfromDB("select * from VAT_LeaveData  where OEmpID='" + EmpNo + "' and (AttDate >= " + "'" + dbFinancialYear.FYStartDate.Value.ToString("yyyy-MM-ddd") + "'" + " and AttDate <= " + "'" + dbFinancialYear.FYEndDate.Value.ToString("yyyy-MM-ddd") + "'" + " )");
            List<VAT_LeaveData> VATLeaveData = dataTable.ToList<VAT_LeaveData>();
            e.DataSources.Add(new ReportDataSource("DataSet1", VATLeaveData));

        }

        private ReportViewer LoadReport_VATDailyAttendance(List<VAT_DailyAttendance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_DailyAttendance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        //private ReportViewer LoadReport_VATDailyAttendanceOther(List<VMEmployeeAttendanceOther> list, ReportViewer rv)
        //{
        //    rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
        //    IEnumerable<VMEmployeeAttendanceOther> ie;
        //    ie = list.AsQueryable();
        //    ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
        //    rv.LocalReport.DataSources.Add(datasource1);
        //    ReportParameter rp = new ReportParameter("Date", DateTitle, false);
        //    ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
        //    ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
        //    rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
        //    rv.LocalReport.Refresh();
        //    return rv;
        //}
        private ReportViewer LoadReport_VATDailyAttendanceDetail(List<VAT_DailyAttendanceDetail> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_DailyAttendanceDetail> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATMonthlySummary(List<VAT_MonthlySummary> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_MonthlySummary> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }







        private ReportViewer LoadReport_VATMonthlySheet(List<VAT_MonthlySheet> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_MonthlySheet> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }


















        private ReportViewer LoadReport_VATDeviceData(List<VAT_DeviceData> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_DeviceData> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATLeaveData(List<VAT_LeaveData> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_LeaveData> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VHREmployeeProfile(List<VHR_EmployeeProfile> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VHR_EmployeeProfile> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATDailyAttedanceEdit(List<VAT_DailyAttedanceEdit> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_DailyAttedanceEdit> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATMonthDataEdit(List<VAT_MonthDataEdit> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_MonthDataEdit> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATOvertimeHistory(List<VAT_OvertimeHistory> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_OvertimeHistory> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
        private ReportViewer LoadReport_VATCPLBalance(List<VAT_CPLBalance> list, ReportViewer rv)
        {
            rv.LocalReport.ReportPath = Server.MapPath(ReportPath);
            IEnumerable<VAT_CPLBalance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            rv.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateTitle, false);
            ReportParameter rp1 = new ReportParameter("Header", CompanyHeader + ReportTitle, false);
            ReportParameter rp2 = new ReportParameter("Footer", ReportFooter, false);
            rv.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2 });
            rv.LocalReport.Refresh();
            return rv;
        }
    }
}