using ESSPCORE.Attendance;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESSPCORE.Attendance;
using ESSPCORE.Common;

namespace ESSPSERVICE.Attendance
{
    /// <summary>
    /// Interface for editing monthly attendance 
    /// </summary>
    /// <remarks></remarks>
    public interface IMonthlyEditorService
    {
        //List<MonthData> GetIndex();
        VMJobCardCreate GetIndex();
        /// <summary>
        /// Gets the List of all the locations,OU,employee type,all other filters selection.
        /// </summary>
        /// <returns>returns a view containing all the filters to get employee location wise or single.</returns>
        /// <remarks></remarks>
        VMEditMonthlyCreate GetCreate1();
        /// <summary>
        ///  This method gets the information of selected filter
        /// </summary>
        /// <param name="es"> Gets the information of Selected employee for overtime encashment. </param>
        /// <param name="SelectedOUCommonIds">Parameter of Common OU</param>
        /// <param name="SelectedOUIds">parameter of OU </param>
        /// <param name="SelectedEmploymentTypeIds">Selected Employee types</param>
        /// <param name="SelectedLocationIds">Selected Locations</param>
        /// <param name="SelectedGradeIds">Selected Grades </param>
        /// <param name="SelectedJobTitleIds">Selected Job titles</param>
        /// <param name="SelectedDesignationIds">Slected Designations</param>
        /// <param name="SelectedCrewIds">Selected Crews./param>
        /// <param name="SelectedShiftIds">Selected Shifts</param>
        /// <param name="LoggedInUser">Logged in User </param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMEditMonthlyCreate GetCreate2(VMEditMonthlyCreate es, int?[] SelectedCompanyIds, int?[] SelectedOUCommonIds, int?[] SelectedOUIds, int?[] SelectedEmploymentTypeIds,
                   int?[] SelectedLocationIds, int?[] SelectedGradeIds, int?[] SelectedJobTitleIds, int?[] SelectedDesignationIds,
                   int?[] SelectedCrewIds, int?[] SelectedShiftIds, VMLoggedUser LoggedInUser);
        List<VAT_MonthlySummary> AttendanceDetails();
        /// <summary>
        /// Gets the attendance attributes from monthly summary view and show total days, paid days 
        /// </summary>
        /// <param name="MonthlyAttendance">parameter for getting the list of Monthly attendance summary</param>
        /// <param name="PayrollID">Shows the attendance for the specific payroll period.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        VMEditMonthlyAttendance GetMonthlyAttendanceAttributes(List<VAT_MonthlySummary> MonthlyAttendance, int PayrollID);
        /// <summary>
        /// Gets the edit list of monthly 
        /// </summary>
        /// <param name="MonthlyAttendance"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        EditMonthlyAttendanceList GetTotalCount(List<EditMonthlyAttendanceList> MonthlyAttendance);
        EditMonthlyAttendanceList GetEditMonthlyAttendanceList(int EmpID, int prid, string TotalDays, string PaidDays, string AbsentDays, string Remarks);
        /// <summary>
        /// check that whether record is edited if not and 
        /// </summary>
        /// <param name="att"></param>
        /// <param name="editlist"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        string CheckMonthRecordIsEdited(MonthData att, EditMonthlyAttendanceList editlist);
    }
}
