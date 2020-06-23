using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.HumanRecource
{
    //public class EmployeeOtherService : IEmployeeOtherService
    //{
    //    #region -- Service Variables --
    //    IRepository<VHR_EmployeeOtherProfile> VHREmployeeOtherRepository;
    //    IRepository<EmployeeOther> EmployeeOtherRepository;
    //    public EmployeeOtherService(IRepository<VHR_EmployeeOtherProfile> vhrEmployeeOtherRepository, IRepository<EmployeeOther> employeeOtherRepository)
    //    {
    //        VHREmployeeOtherRepository = vhrEmployeeOtherRepository;
    //        EmployeeOtherRepository = employeeOtherRepository;
    //    }
    //    #endregion
    //    #region -- Service Interface Implementation --
    //    List<VHR_EmployeeOtherProfile> IEmployeeOtherService.GetIndex(VMLoggedUser LoggedInUser)
    //    {
    //        return VHREmployeeOtherRepository.GetAll();
    //    }
    //    public EmployeeOther GetDelete(int? id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public EmployeeOther GetEdit(int id)
    //    {
    //        EmployeeOther dbEmployeeOther = EmployeeOtherRepository.GetSingle(id);
    //        if (dbEmployeeOther.Status == "Active")
    //        {
    //            dbEmployeeOther.Status = "true";
    //        }
    //        else
    //        {
    //            dbEmployeeOther.Status = "false";
    //        }
    //        return dbEmployeeOther;
    //    }
    //    public void PostCreate(EmployeeOther obj)
    //    {
    //    }
    //    public void PostDelete(EmployeeOther obj)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void PostEdit(EmployeeOther obj)
    //    {
    //        EmployeeOther dbEmployeeOther = EmployeeOtherRepository.GetSingle(obj.PEmployeeID);
    //        dbEmployeeOther.EmployeeName = obj.EmployeeName;
    //        dbEmployeeOther.FatherName = obj.FatherName;
    //        dbEmployeeOther.OEmpID = obj.OEmpID;
    //        dbEmployeeOther.DOJ = obj.DOJ;
    //        dbEmployeeOther.TelephoneNo = obj.TelephoneNo;
    //        dbEmployeeOther.FPID = obj.FPID;
    //        dbEmployeeOther.OfficialEmailID = obj.OfficialEmailID;
    //        dbEmployeeOther.ProcessAttendance = obj.ProcessAttendance;
    //        if (obj.Status == "true")
    //        {
    //            dbEmployeeOther.Status = "Active";
    //        }
    //        else
    //        {
    //            dbEmployeeOther.Status = "Resigned";
    //        }
    //        EmployeeOtherRepository.Edit(dbEmployeeOther);
    //        EmployeeOtherRepository.Save();
    //    }
    //    #endregion
    //    #region -- Service Private Methods --
    //    #endregion
    //}
}
