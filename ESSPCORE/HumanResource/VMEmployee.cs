using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.HumanResource
{
    public class VMEmployee
    {
        public int PEmployeeID { get; set; }
        public string OEmpID { get; set; }
        public Nullable<int> FPID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> CrewID { get; set; }
        public Nullable<System.DateTime> ResignDate { get; set; }
        public Nullable<int> LineManagerID { get; set; }
        public Nullable<System.DateTime> LineManagerActiveDate { get; set; }
        public Nullable<short> ShiftID { get; set; }
        public string CardNo { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public Nullable<bool> FaceTemp { get; set; }
        public Nullable<bool> FPTemp { get; set; }
        public Nullable<bool> ProcessAttendance { get; set; }
        public Nullable<bool> HasOneStep { get; set; }
        public Nullable<short> OTPolicyID { get; set; }
        public Nullable<bool> HasESSPLeave { get; set; }
        public int? ALPolicyID { get; set; }
        public int? CLPolicyID { get; set; }
        public int? SLPolicyID { get; set; }
        public int? EALPolicyID { get; set; }
        public int? CMEPolicyID { get; set; }
        public int? CPLPolicyID { get; set; }
        public DateTime? CrewStartDate { get; set; }
        public DateTime? CrewEndDate { get; set; }
        public bool IsCrewChanged { get; set; }
        public string OldCrewName { get; set; }
        public string OfficialEmail{ get; set; }
        public int? DepartmentID { get; set; }
        public int? SectionID { get; set; }
        public int? GradeID { get; set; }
        public int? JobTitleID { get; set; }
        public string TelephoneNo { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DOJ { get; set; }
        public string CNIC { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int? EmploymentTypeID { get; set; }
        public int? DesigationID { get; set; }
    }
}
