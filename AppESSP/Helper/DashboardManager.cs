using ESSPCORE.Attendance;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppESSP.Helper
{
    public static class DashboardManager
    {
        internal static DMPieChartParentModel ApplyGraphForItems(DMPieChartParentModel vm, UserGraphType userGraphType, VMAttendanceDashboard vmAttendanceDashboard)
        {
            switch (userGraphType)
            {
                case UserGraphType.HasMultipleCommonOU:
                    vm.HeaderRight = "Common Organizational Units";
                    vm.TBLHeaderID = "OUCommonID";
                    vm.TBLHeaderName = "Common Organization Units";
                    if (vm.TBLHeaderCount == null) vm.TBLHeaderCount = "No. Of Employees";
                    if (vmAttendanceDashboard.ID == 0)
                    {
                        vm = GetDataForPieChartCommonOU(vm.AttData, vm);
                    }
                    else
                    {
                        vm = GetDataForPieChartCommonOU(vm.AttData.Where(aa => aa.OUCommonID == vmAttendanceDashboard.ID).ToList(), vm);
                    }
                    break;
                case UserGraphType.HasMultipleOU:
                    vm.HeaderRight = "Organizational Units";
                    vm.TBLHeaderID = "OUID";
                    vm.TBLHeaderName = "Organization Units";
                    if (vm.TBLHeaderCount == null) vm.TBLHeaderCount = "No. Of Employees";
                    if (vmAttendanceDashboard.ID == 0)
                    {
                        vm = GetDataForPieChartOU(vm.AttData, vm);
                    }
                    else
                    {
                        vm = GetDataForPieChartOU(vm.AttData.Where(aa => aa.OUCommonID == vmAttendanceDashboard.ID).ToList(), vm);
                    }
                    break;
                case UserGraphType.SimpleLM:
                    vm.HeaderRight = "Employee";
                    vm.TBLHeaderID = "EmpID";
                    vm.TBLHeaderName = "Employee Name";
                    vm.TBLHeaderCount = "No. Of Days";
                    if (vmAttendanceDashboard.ID == 0)
                    {
                        vm = GetDataForPieChartEmployee(vm.AttData, vm);
                    }
                    else
                    {
                        vm = GetDataForPieChartEmployee(vm.AttData.Where(aa => aa.OUID == vmAttendanceDashboard.ID).ToList(), vm);
                    }
                    break;
            }
            return vm;
        }
        internal static DMPieChartParentModel ApplyGraphTypeItems(DMPieChartParentModel vm, List<VAT_DailyAttendance> AttList, string graphType)
        {
            switch (graphType)
            {
                case "LateIn":
                    vm.HeaderLeft = "Late In Percentage";
                    vm.TBLHeaderCount = "Late Arrivals";
                    AttList = AttList.Where(aa => aa.LateIn > 0).ToList();
                    break;
                case "LateOut":
                    vm.HeaderLeft = "Late Out Percentage";
                    vm.TBLHeaderCount = "Late Departures";
                    AttList = AttList.Where(aa => aa.LateOut > 0).ToList();
                    break;
                case "EarlyIn":
                    vm.HeaderLeft = "Early In Detail Percentage";
                    vm.TBLHeaderCount = "Early Arrivals";
                    AttList = AttList.Where(aa => aa.EarlyIn > 0).ToList();
                    break;
                case "EarlyOut":
                    vm.HeaderLeft = "Early Out Detail Percentage";
                    vm.TBLHeaderCount = "Early Departures";
                    AttList = AttList.Where(aa => aa.EarlyOut > 0).ToList();
                    break;
                case "Absent":
                    vm.HeaderLeft = "Absent Detail Percentage";
                    vm.TBLHeaderCount = "Total Days";
                    AttList = AttList.Where(aa => aa.AbDays > 0).ToList();
                    break;
                case "Leave":
                    vm.HeaderLeft = "Leaves Detail Percentage";
                    vm.TBLHeaderCount = "Total Days";
                    AttList = AttList.Where(aa => aa.LeaveDays > 0).ToList();
                    break;
                case "OfficialDuty":
                    vm.HeaderLeft = "Official Duty Detail Percentage";
                    vm.TBLHeaderCount = "Total Days";
                    AttList = AttList.Where(aa => aa.TimeIn==null && aa.TimeOut==null&& aa.Remarks!=null && aa.Remarks.Contains("JC:")).ToList();
                    break;
            }
            vm.AttData = AttList;
            return vm;
        }
        internal static DMPieChartParentModel GetDataForPieChartCommonOU(List<VAT_DailyAttendance> AttList, DMPieChartParentModel vm)
        {
            if (AttList.Count > 0)
            {
                vm.HeaderRight = "Common Organizational Units";
                vm.HeaderDescription = "Click on below common organizational unit to view its details";
                List<DMParentModel> dmList = new List<DMParentModel>();
                foreach (var id in AttList.Select(aa => aa.OUCommonID).Distinct().ToList())
                {
                    if (AttList.Where(aa => aa.OUCommonID == id).Count() > 0 && id != null)
                    {
                        DMParentModel dmObj = new DMParentModel();
                        dmObj.ID = (int)id;
                        dmObj.Name = AttList.Where(aa => aa.OUCommonID == id).First().OUCommonName;
                        dmObj.NameWithDetail = AttList.Where(aa => aa.OUCommonID == id).First().OUCommonName;
                        dmObj.Count = AttList.Where(aa => aa.OUCommonID == id).Count();
                        dmList.Add(dmObj);
                    }
                }
                vm.ChildList = dmList.OrderByDescending(aa => aa.Count).ToList();
            }
            return vm;
        }
        internal static DMPieChartParentModel GetDataForPieChartOU(List<VAT_DailyAttendance> AttList, DMPieChartParentModel vm)
        {
            if (AttList.Count > 0)
            {
                vm.HeaderRight = "Organizational Units for " + AttList.FirstOrDefault().OUCommonName;
                vm.HeaderDescription = "Click on below organizational unit to view its details";
                List<DMParentModel> dmList = new List<DMParentModel>();
                foreach (var id in AttList.Select(aa => aa.OUID).Distinct().ToList())
                {
                    if (AttList.Where(aa => aa.OUID == id).Count() > 0)
                    {
                        DMParentModel dmObj = new DMParentModel();
                        dmObj.ID = (int)id;
                        dmObj.NameWithDetail = AttList.Where(aa => aa.OUID == id).First().OUName;
                        dmObj.Name = AttList.Where(aa => aa.OUID == id).First().OUName;
                        dmObj.Count = AttList.Where(aa => aa.OUID == id).Count();
                        dmList.Add(dmObj);
                    }
                }
                vm.ChildList = dmList.OrderByDescending(aa => aa.Count).ToList();
            }
            return vm;
        }
        internal static DMPieChartParentModel GetDataForPieChartEmployee(List<VAT_DailyAttendance> AttList, DMPieChartParentModel vm)
        {
            if (AttList.Count > 0)
            {
                vm.HeaderRight = "Employees for " + AttList.FirstOrDefault().OUName;
                vm.HeaderDescription = "Click on below employee to view its details";
                List<DMParentModel> dmList = new List<DMParentModel>();
                foreach (var id in AttList.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    if (AttList.Where(aa => aa.EmpID == id).Count() > 0)
                    {
                        DMParentModel dmObj = new DMParentModel();
                        dmObj.ID = (int)id;
                        dmObj.NameWithDetail = AttList.Where(aa => aa.EmpID == id).First().EmployeeName+" ("+ AttList.Where(aa => aa.EmpID == id).First().JobTitleName + ")" ;
                        dmObj.Name = AttList.Where(aa => aa.EmpID == id).First().EmployeeName;
                       dmObj.Count = AttList.Where(aa => aa.EmpID == id).Count();
                        dmList.Add(dmObj);
                    }
                }
                vm.ChildList = dmList.OrderByDescending(aa => aa.Count).ToList();
            }
            return vm;
        }
    }
}