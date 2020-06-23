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
    
    public partial class LeaveApplication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LeaveApplication()
        {
            this.LeaveDatas = new HashSet<LeaveData>();
        }
    
        public int PLeaveAppID { get; set; }
        public System.DateTime LeaveDate { get; set; }
        public byte LeaveTypeID { get; set; }
        public int EmpID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public float NoOfDays { get; set; }
        public Nullable<double> CalenderDays { get; set; }
        public Nullable<bool> IsHalf { get; set; }
        public Nullable<bool> FirstHalf { get; set; }
        public Nullable<bool> HalfAbsent { get; set; }
        public string LeaveReason { get; set; }
        public string LeaveAddress { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string LeaveStageID { get; set; }
        public int LineManagerID { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<bool> Active { get; set; }
        public string RejectRemarks { get; set; }
        public Nullable<bool> IsDeducted { get; set; }
        public Nullable<bool> TimeOffice { get; set; }
        public Nullable<int> ReplamentEmpID { get; set; }
        public Nullable<bool> HasMedicalCertificate { get; set; }
        public Nullable<bool> IsAccum { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> FinancialYearID { get; set; }
        public string PathName { get; set; }
        public Nullable<int> SubmittedByUserID { get; set; }
    
        public virtual LeaveStage LeaveStage { get; set; }
        public virtual LeaveType LeaveType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveData> LeaveDatas { get; set; }
    }
}