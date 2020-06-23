using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ESSPCORE.Common
{
    public enum AuditFormAttendance
    {
        Employee = 1,
        Crew = 2,
        CommonOU = 3,
        CommonJobTitle = 4,
        JobTitle = 5,
        Positions = 6,
        Grades = 7,
        EmploymentType = 8,
        OrganizationalUnits = 9,
        Locations = 10,
        Users = 11,
        UserRoles = 12,
        Devices = 13,
        Holidays = 14,
        DownloadTimes = 15,
        FinancialYear = 16,
        PayrollPeriod = 17,
        LeaveApplication = 18,
        LeaveQuota = 19,
        LeaveCarryForward = 20,
        LeavePolicy = 21,
        DailyEditor = 22,
        MonthlyEditor = 23,
        JobCards = 24,
        Rosters = 25,
        Shifts = 26,
        ShiftChange = 27,
        ShiftChangeEmployee = 28,
        OvertimePolicy = 29,
        OvertimeApproval = 30,
        Home = 31
    }
    public enum AuditTypeCommon
    {
        LogIN = 1,
        LogOut = 2,
        Add = 3,
        Edit = 4,
        Delete = 5
    }
}
