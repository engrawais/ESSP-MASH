using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Common
{
    public class VMEmpSelection
    {
        public List<Company> Company { get; set; }
        public List<VHR_Location> Location { get; set; }
        public List<VHR_EmployeeProfile> Employee { get; set; }
        public List<VHR_OrganizationalUnit> OrganizationalUnit { get; set; }
        public List<VAT_Shift> Shift { get; set; }
        public List<Crew> Crew { get; set; }
        public List<VHR_Grade> Grade { get; set; }
        public List<VHR_Designation> Designation { get; set; }
        public List<VHR_EmploymentType>  EmploymentType{ get; set; }
        public List<VHR_JobTitle> JobTitle { get; set; }
        public List<OUCommon> OUCommon { get; set; }
        public string Criteria { get; set; }
        public string CriteriaName { get; set; }
        public string EmpNo { get; set; }
        public string PayrollPeriodID { get; set; }
        

    }
    public class VMDropDown
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
