using ESSPCORE.EF;
using ESSPCORE.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppESSP.Areas.Reporting.BusinessLogic.Attendance
{
    public static class AttendanceFilter
    {
        public static List<VAT_DailyAttendance> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_DailyAttendance> _TempViewList, List<VAT_DailyAttendance> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VAT_DailyAttendanceDetail> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_DailyAttendanceDetail> _TempViewList, List<VAT_DailyAttendanceDetail> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VAT_MonthlySummary> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_MonthlySummary> _TempViewList, List<VAT_MonthlySummary> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }





        internal static List<VAT_MonthlySheet> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_MonthlySheet> _TempViewList, List<VAT_MonthlySheet> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    //_TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.PEmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }








        internal static List<VAT_DeviceData> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_DeviceData> _TempViewList, List<VAT_DeviceData> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VAT_LeaveData> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_LeaveData> _TempViewList, List<VAT_LeaveData> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VMLeaveBalance> ReportsFilterImplementation(VMSelectedFilter fm, List<VMLeaveBalance> _TempViewList, List<VMLeaveBalance> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.PEmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VHR_EmployeeProfile> ReportsFilterImplementation(VMSelectedFilter fm, List<VHR_EmployeeProfile> _TempViewList, List<VHR_EmployeeProfile> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.PEmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VAT_DailyAttedanceEdit> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_DailyAttedanceEdit> _TempViewList, List<VAT_DailyAttedanceEdit> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.PEmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        internal static List<VMMonthlyLeaveBalance> ReportsFilterImplementation(VMSelectedFilter fm, List<VMMonthlyLeaveBalance> _TempViewList, List<VMMonthlyLeaveBalance> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        public static List<VAT_MonthDataEdit> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_MonthDataEdit> _TempViewList, List<VAT_MonthDataEdit> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        public static List<VAT_OvertimeHistory> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_OvertimeHistory> _TempViewList, List<VAT_OvertimeHistory> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
        public static List<VAT_CPLBalance> ReportsFilterImplementation(VMSelectedFilter fm, List<VAT_CPLBalance> _TempViewList, List<VAT_CPLBalance> _ViewList)
        {
            //for company
            if (fm.SelectedCompany.Count > 0)
            {
                foreach (var item in fm.SelectedCompany)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CompanyID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for Common OU
            if (fm.SelectedCommonOU.Count > 0)
            {
                foreach (var item in fm.SelectedCommonOU)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUCommonID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (fm.SelectedLocation.Count > 0)
            {
                foreach (var item in fm.SelectedLocation)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for employment Type
            if (fm.SelectedEmployementType.Count > 0)
            {
                foreach (var item in fm.SelectedEmployementType)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmploymentTypeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for grade
            if (fm.SelectedGrade.Count > 0)
            {
                foreach (var item in fm.SelectedGrade)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.GradeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for jobtitle
            if (fm.SelectedJobTitle.Count > 0)
            {
                foreach (var item in fm.SelectedJobTitle)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.JobTitleID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for designation
            if (fm.SelectedPosition.Count > 0)
            {
                foreach (var item in fm.SelectedPosition)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DesigationID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            //for OU
            if (fm.SelectedOrganizationalUnit.Count > 0)
            {
                foreach (var item in fm.SelectedOrganizationalUnit)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.OUID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for crew
            if (fm.SelectedCrew.Count > 0)
            {
                foreach (var item in fm.SelectedCrew)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shift
            if (fm.SelectedShift.Count > 0)
            {
                foreach (var item in fm.SelectedShift)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for employee
            if (fm.SelectedEmployee.Count > 0)
            {
                foreach (var item in fm.SelectedEmployee)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmployeeID == item.FilterID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            return _ViewList;
        }
    
    }
}