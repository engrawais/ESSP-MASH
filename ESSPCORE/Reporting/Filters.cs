using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Reporting
{
    public class VMFilterAttribute
    {
        public int FilterID;
        public string FilterIDString;
        public string FilterName;
        public bool IsSlected;
        public int? FieldOneID;
        public string FieldOneName;
        public int? FieldTwoID;
        public string FieldTwoName;
        public int? FieldThreeID;
        public string FieldThreeName;
    }
    public class VMSelectedFilter
    {
        public List<VMFilterAttribute> SelectedCompany = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedCommonOU = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedLocation = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedGrade = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedPosition = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedOrganizationalUnit = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedCrew = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedEmployee = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedShift = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedJobTitle = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedEmployementType = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedObjectiveStatus= new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedFeedbackStatus = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedMidYearStatus = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedAppraisalStatus = new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedCycle= new List<VMFilterAttribute>();
        public List<VMFilterAttribute> SelectedRating = new List<VMFilterAttribute>();
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
