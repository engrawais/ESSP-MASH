using ESSPCORE.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppESSP.Areas.Reporting.BusinessLogic
{

    public static class ReportFilterManager
    {
        public static VMSelectedFilter AddValuesInSession(int ID, string Name, string FilterType, VMSelectedFilter vm)
        {
            switch (FilterType)
            {
                case "CommonOU":
                    if (vm.SelectedCommonOU.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedCommonOU.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "EmployementType":
                    if (vm.SelectedEmployementType.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedEmployementType.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Company":
                    if (vm.SelectedCompany.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedCompany.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Location":
                    if (vm.SelectedLocation.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedLocation.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Grade":
                    if (vm.SelectedGrade.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedGrade.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "JobTitle":
                    if (vm.SelectedJobTitle.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedJobTitle.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Position":
                    if (vm.SelectedPosition.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedPosition.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "OrganizationalUnit":
                    if (vm.SelectedOrganizationalUnit.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedOrganizationalUnit.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Crew":
                    if (vm.SelectedCrew.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedCrew.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Shift":
                    if (vm.SelectedShift.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedShift.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Employee":
                    if (vm.SelectedEmployee.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedEmployee.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Cycle":
                    if (vm.SelectedCycle.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedCycle.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
                case "Rating":
                    if (vm.SelectedRating.Where(aa => aa.FilterID == ID).Count() == 0)
                        vm.SelectedRating.Add(new VMFilterAttribute() { FilterID = ID, FilterName = Name, IsSlected = true });
                    break;
            }
            return vm;
        }
        public static VMSelectedFilter RemoveValuesFromSession(int ID, string FilterType, VMSelectedFilter vm)
        {
            switch (FilterType)
            {
                case "CommonOU":
                    for (int k = 0; k < vm.SelectedCommonOU.Count; k++)
                    {
                        if (vm.SelectedCommonOU[k].FilterID == ID)
                            vm.SelectedCommonOU.RemoveAt(k);
                    }
                    break;
                case "EmployementType":
                    for (int k = 0; k < vm.SelectedEmployementType.Count; k++)
                    {
                        if (vm.SelectedEmployementType[k].FilterID == ID)
                            vm.SelectedEmployementType.RemoveAt(k);
                    }
                    break;
                case "Company":
                    for (int k = 0; k < vm.SelectedCompany.Count; k++)
                    {
                        if (vm.SelectedCompany[k].FilterID == ID)
                            vm.SelectedCompany.RemoveAt(k);
                    }
                    break;
                case "Location":
                    for (int k = 0; k < vm.SelectedLocation.Count; k++)
                    {
                        if (vm.SelectedLocation[k].FilterID == ID)
                            vm.SelectedLocation.RemoveAt(k);
                    }
                    break;
                case "Grade":
                    for (int k = 0; k < vm.SelectedGrade.Count; k++)
                    {
                        if (vm.SelectedGrade[k].FilterID == ID)
                            vm.SelectedGrade.RemoveAt(k);
                    }
                    break;
                case "JobTitle":
                    for (int k = 0; k < vm.SelectedJobTitle.Count; k++)
                    {
                        if (vm.SelectedJobTitle[k].FilterID == ID)
                            vm.SelectedJobTitle.RemoveAt(k);
                    }
                    break;
                case "Position":
                    for (int k = 0; k < vm.SelectedPosition.Count; k++)
                    {
                        if (vm.SelectedPosition[k].FilterID == ID)
                            vm.SelectedPosition.RemoveAt(k);
                    }
                    break;
                case "OrganizationalUnit":
                    for (int k = 0; k < vm.SelectedOrganizationalUnit.Count; k++)
                    {
                        if (vm.SelectedOrganizationalUnit[k].FilterID == ID)
                            vm.SelectedOrganizationalUnit.RemoveAt(k);
                    }
                    break;
                case "Crew":
                    for (int k = 0; k < vm.SelectedCrew.Count; k++)
                    {
                        if (vm.SelectedCrew[k].FilterID == ID)
                            vm.SelectedCrew.RemoveAt(k);
                    }
                    break;
                case "Shift":
                    for (int k = 0; k < vm.SelectedShift.Count; k++)
                    {
                        if (vm.SelectedShift[k].FilterID == ID)
                            vm.SelectedShift.RemoveAt(k);
                    }
                    break;
                case "Employee":
                    for (int k = 0; k < vm.SelectedEmployee.Count; k++)
                    {
                        if (vm.SelectedEmployee[k].FilterID == ID)
                            vm.SelectedEmployee.RemoveAt(k);
                    }
                    break;
                case "Cycle":
                    for (int k = 0; k < vm.SelectedCycle.Count; k++)
                    {
                        if (vm.SelectedCycle[k].FilterID == ID)
                            vm.SelectedCycle.RemoveAt(k);
                    }
                    break;
                case "Rating":
                    for (int k = 0; k < vm.SelectedRating.Count; k++)
                    {
                        if (vm.SelectedRating[k].FilterID == ID)
                            vm.SelectedRating.RemoveAt(k);
                    }
                    break;
            }
            return vm;
        }
        public static VMSelectedFilter AddValuesInSessionString(string ID, string Name, string FilterType, VMSelectedFilter vm)
        {
            switch (FilterType)
            {
                case "ObjectiveSetting":
                    if (vm.SelectedObjectiveStatus.Where(aa => aa.FilterIDString == ID).Count() == 0)
                        vm.SelectedObjectiveStatus.Add(new VMFilterAttribute() { FilterIDString = ID, FilterName = Name, IsSlected = true });
                    break;
                case "FeedbackMeeting":
                    if (vm.SelectedFeedbackStatus.Where(aa => aa.FilterIDString == ID).Count() == 0)
                        vm.SelectedFeedbackStatus.Add(new VMFilterAttribute() { FilterIDString = ID, FilterName = Name, IsSlected = true });
                    break;
                case "MidYearReview":
                    if (vm.SelectedMidYearStatus.Where(aa => aa.FilterIDString == ID).Count() == 0)
                        vm.SelectedMidYearStatus.Add(new VMFilterAttribute() { FilterIDString = ID, FilterName = Name, IsSlected = true });
                    break;
                case "AnnualAppraisal":
                    if (vm.SelectedAppraisalStatus.Where(aa => aa.FilterIDString == ID).Count() == 0)
                        vm.SelectedAppraisalStatus.Add(new VMFilterAttribute() { FilterIDString = ID, FilterName = Name, IsSlected = true });
                    break;
            }
            return vm;
        }
        public static VMSelectedFilter RemoveValuesFromSessionString(string ID, string FilterType, VMSelectedFilter vm)
        {
            switch (FilterType)
            {
                
                case "ObjectiveSetting":
                    for (int k = 0; k < vm.SelectedObjectiveStatus.Count; k++)
                    {
                        if (vm.SelectedObjectiveStatus[k].FilterIDString == ID)
                            vm.SelectedObjectiveStatus.RemoveAt(k);
                    }
                    break;
                case "FeedbackMeeting":
                    for (int k = 0; k < vm.SelectedFeedbackStatus.Count; k++)
                    {
                        if (vm.SelectedFeedbackStatus[k].FilterIDString == ID)
                            vm.SelectedFeedbackStatus.RemoveAt(k);
                    }
                    break;
                case "MidYearReview":
                    for (int k = 0; k < vm.SelectedMidYearStatus.Count; k++)
                    {
                        if (vm.SelectedMidYearStatus[k].FilterIDString == ID)
                            vm.SelectedMidYearStatus.RemoveAt(k);
                    }
                    break;
                case "AnnualAppraisal":
                    for (int k = 0; k < vm.SelectedAppraisalStatus.Count; k++)
                    {
                        if (vm.SelectedAppraisalStatus[k].FilterIDString == ID)
                            vm.SelectedAppraisalStatus.RemoveAt(k);
                    }
                    break;
            }
            return vm;
        }
        public static VMSelectedFilter RemoveAllValuesFromSession(string FilterType, VMSelectedFilter vm)
        {
            switch (FilterType)
            {
                case "CommonOU":
                    vm.SelectedCommonOU = new List<VMFilterAttribute>();
                    break;
                case "Company":
                    vm.SelectedCompany = new List<VMFilterAttribute>();
                    break;
                case "EmployementType":
                    vm.SelectedEmployementType = new List<VMFilterAttribute>();
                    break;
                case "Location":
                    vm.SelectedLocation = new List<VMFilterAttribute>();
                    break;
                case "Grade":
                    vm.SelectedGrade = new List<VMFilterAttribute>();
                    break;
                case "JobTitle":
                    vm.SelectedJobTitle = new List<VMFilterAttribute>();
                    break;
                case "Position":
                    vm.SelectedPosition = new List<VMFilterAttribute>();
                    break;
                case "OrganizationalUnit":
                    vm.SelectedOrganizationalUnit = new List<VMFilterAttribute>();
                    break;
                case "Crew":
                    vm.SelectedCrew = new List<VMFilterAttribute>();
                    break;
                case "Shift":
                    vm.SelectedShift = new List<VMFilterAttribute>();
                    break;
                case "Employee":
                    vm.SelectedEmployee = new List<VMFilterAttribute>();
                    break;
                case "ObjectiveSetting":
                    vm.SelectedObjectiveStatus = new List<VMFilterAttribute>();
                    break;
                case "FeedbackMeeting":
                    vm.SelectedFeedbackStatus = new List<VMFilterAttribute>();
                    break;
                case "MidYearReview":
                    vm.SelectedMidYearStatus = new List<VMFilterAttribute>();
                    break;
                case "AnnualAppraisal":
                    vm.SelectedAppraisalStatus = new List<VMFilterAttribute>();
                    break;
                case "Cycle":
                    vm.SelectedCycle = new List<VMFilterAttribute>();
                    break;
                case "Rating":
                    vm.SelectedRating = new List<VMFilterAttribute>();
                    break;
            }
            return vm;
        }
        
    }
}