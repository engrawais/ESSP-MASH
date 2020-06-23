using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.HumanResource
{
    public class VMAppUserRole: AppUserTM
    {
        public string PVMUserRoleID { get; set; }
        public string VMUserRoleName { get; set; }
    }
    public class VMAppUser : AppUserTM
    {
        public int PUserID { get; set; }
        [Required(ErrorMessage = "Package Name is Required")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Package Name is Required")]
        [StringLength(50)]
        public string Password { get; set; }
        public bool? UserStatus { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public string EmpNo { get; set; }
        public int? EmpID { get; set; }
        public int? UserAccessTypeID { get; set; }
        public int? AppUserRoleID { get; set; }
        public bool? HasESSP { get; set; }
        public List<VMUserLocation> UserLocations { get; set; }
        public List<VMUserDepartment> UserDepartment { get; set; }
    }
    public class VMUserLocation:Location
    {
        public bool IsSelected { get; set; }
    }
    public class VMUserDepartment : OUCommon
    {
        public bool IsSelectedDepartment { get; set; }
    }
}
