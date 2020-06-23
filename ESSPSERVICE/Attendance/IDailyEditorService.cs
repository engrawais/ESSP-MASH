using ESSPCORE.Attendance;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Represent methods which are used to perform manual update of attendance
    /// </summary>
    /// <remarks></remarks>
    public interface IDailyEditorService
    {
        List<DailyAttendance> GetIndex();

        /// <summary>
        /// Get Time office attendance progress bars for specific date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="Criteria">Criteria could be Missing, Absent, Late In and Early Out</param>
        /// <param name="att">List of daily attendance data based on user location permission</param>
        /// <returns>Returns object which contains attendance progress bar data</returns>
        /// <remarks></remarks>
        VMEditAttendanceDateWise GetTMDashboardAttendance(DateTime date, string Criteria, List<VAT_DailyAttendance> att);
        /// <summary>
        /// This method gets the list of attendance of the employee days wise.
        /// </summary>
        /// <param name="dailyAttendance">This paramteter gets the Employee attendance list from the database </param>
        /// <param name="dtFrom">This date parameter gets the attendance from selected start date</param>
        /// <param name="dtTo">This date parameter gets the attendance from selected end date</param>
        /// <param name="empid"> This parameter gets the id of the employee whose attendance is to be seen</param>
        /// <returns></returns>
        /// <remarks></remarks>
        AttEditSingleEmployee GetAttendanceAttributes(List<DailyAttendance> dailyAttendance, DateTime dtFrom, DateTime dtTo, int empid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empDate"></param>
        /// <param name="DutyCode"></param>
        /// <param name="DutyTime"></param>
        /// <param name="ShiftTime"></param>
        /// <param name="TimeIn"></param>
        /// <param name="TimeOut"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        EditAttendanceList GetEditAttendanceList(string empDate, string DutyCode, string DutyTime, string ShiftTime, string TimeIn, string TimeOut, string Remarks);
        TimeSpan ConvertTime(string p);
        bool CheckRecordIsEdited(DailyAttendance att, EditAttendanceList editlist);
        List<DailyAttendance> GetEmployeeAttendance(int empID, DateTime dtFrom, DateTime dtTo);
        VMEditAttendanceDateWise GetAttendanceAttributesDateWise(List<DailyAttendance> dailyAttendance, DateTime dtTo, string Criteria, int CriteriaData);
        VMEditAttendanceDateWise EditDateWiseEntries(DateTime _AttDataTo, string Selection, string ShiftList, string LocationList, string GroupList, string DepartmentList, string SectionList);
        /// <summary>
        /// This method manually processes the attendance of employee after changing in the editor.
        /// </summary>
        /// <param name="EmpDate">Date on which attendance is to be processed.</param>
        /// <param name="JobCardName"></param>
        /// <param name="JobCardStatus">If set to <see langword="true" />, then ; otherwise, .</param>
        /// <param name="NewTimeIn"></param>
        /// <param name="NewTimeOut"></param>
        /// <param name="NewDutyCode"></param>
        /// <param name="_UserID"></param>
        /// <param name="_NewDutyTime"></param>
        /// <param name="_Remarks"></param>
        /// <param name="_ShiftMins"></param>
        /// <remarks></remarks>
        void ManualAttendanceProcess(string EmpDate, string JobCardName, bool JobCardStatus, DateTime NewTimeIn, DateTime NewTimeOut, string NewDutyCode, int _UserID, TimeSpan _NewDutyTime, string _Remarks, short _ShiftMins);
    }
}
