//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESSPCORE.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class VAT_DailyAttedanceEdit
    {
        public int PDailyAttendanceEditID { get; set; }
        public Nullable<System.DateTime> EditDateTime { get; set; }
        public Nullable<System.DateTime> OldTimeIn { get; set; }
        public Nullable<System.DateTime> OldTimeOut { get; set; }
        public Nullable<System.DateTime> NewTimeIn { get; set; }
        public Nullable<System.DateTime> NewTimeOut { get; set; }
        public Nullable<int> NewOTMin { get; set; }
        public string EmpDate { get; set; }
        public string OldDutyCode { get; set; }
        public Nullable<System.TimeSpan> OldDutyTime { get; set; }
        public string NewDutyCode { get; set; }
        public Nullable<System.TimeSpan> NewDutyTime { get; set; }
        public string UserName { get; set; }
        public string UserEmployeeName { get; set; }
        public Nullable<short> OldShiftMin { get; set; }
        public Nullable<short> NewShiftMin { get; set; }
        public Nullable<int> OldOTMin { get; set; }
        public int PEmployeeID { get; set; }
        public Nullable<int> FPID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> EmploymentTypeID { get; set; }
        public Nullable<int> DesigationID { get; set; }
        public Nullable<int> JobTitleID { get; set; }
        public Nullable<int> GradeID { get; set; }
        public Nullable<int> OUID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<System.DateTime> ResignDate { get; set; }
        public Nullable<short> ShiftID { get; set; }
        public string CardNo { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public Nullable<bool> FaceTemp { get; set; }
        public Nullable<bool> ProcessAttendance { get; set; }
        public Nullable<short> OTPolicyID { get; set; }
        public string LocationName { get; set; }
        public string EmploymentTypeName { get; set; }
        public string GradeName { get; set; }
        public string OGradeID { get; set; }
        public string DesignationName { get; set; }
        public string ODesignationID { get; set; }
        public string OEmployeementTypeID { get; set; }
        public string OLocationID { get; set; }
        public string OTPolicyName { get; set; }
        public string ShiftName { get; set; }
        public string OEmpID { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string CrewName { get; set; }
        public Nullable<int> CrewID { get; set; }
        public Nullable<int> ALPolicyID { get; set; }
        public string ALPolicyName { get; set; }
        public Nullable<int> CLPolicyID { get; set; }
        public string CLPolicyName { get; set; }
        public Nullable<int> SLPolicyID { get; set; }
        public string SLPolicyName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> CPLPolicyID { get; set; }
        public string CPLPolicyName { get; set; }
        public Nullable<bool> FPTemp { get; set; }
        public string OUName { get; set; }
        public string OOUID { get; set; }
        public string OUCommonName { get; set; }
        public Nullable<int> OUCommonID { get; set; }
        public string JobTitleName { get; set; }
        public string OJobTitleID { get; set; }
        public Nullable<int> JTCommonID { get; set; }
        public string JTCommonName { get; set; }
        public Nullable<int> CommonLocationID { get; set; }
        public string CommonLocationName { get; set; }
        public Nullable<System.DateTime> AttDate { get; set; }
        public string NewRemarks { get; set; }
        public string OldRemarks { get; set; }
    }
}
