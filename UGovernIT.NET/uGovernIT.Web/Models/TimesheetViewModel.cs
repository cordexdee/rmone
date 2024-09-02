using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class EmployeesResponse
    {
        public bool Status { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Employees { get; set; }
    }

    public class UserResponse
    {
        public bool Status { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object User { get; set; }
    }

    public class ActionResponse
    {
        public bool Status { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Message { get; set; }
    }

    public class ErrorResponse
    {
        public string error_code { get; set; }
        public string error_category { get; set; }
        public string error_message { get; set; }
        public List<string> errors { get; set; }
    }

    public class Entry
    {
        public string type { get; set; }
        public string workItem { get; set; }
        public string role { get; set; }
        public string jobCode { get; set; }
        public string hours { get; set; }
        public string status { get; set; }
        public string note { get; set; }
    }

    public class ResourceTimesheetModel
    {
        public string userName { get; set; }
        public string SignOffStatus { get; set; }
        public List<Timesheet> timesheet { get; set; }
    }

    //public class Timesheet
    //{
    //    public string date { get; set; }
    //    public string day { get; set; }
    //    public List<Entry> entries { get; set; }
    //}

    public class Timesheet
    {
        public string date { get; set; }
        public string day { get; set; }
        public string type { get; set; }
        public string workItem { get; set; }
        public string Title { get; set; }
        public string role { get; set; }
        public string jobCode { get; set; }
        public string hours { get; set; }
        public string status { get; set; }
        public string note { get; set; }
        //public List<Entry> entries { get; set; }
    }

    //////////////////////////
    public class Item
    {
        public string projectType { get; set; }
        public string workItem { get; set; }
        public string title { get; set; }
        public string EnableStdWorkItems { get; set; }
        public List<SubWorkItem> subWorkItem { get; set; }
    }

    public class Projects
    {
        public List<Item> items { get; set; }
    }

    public class Root
    {
        public Projects projects { get; set; }
    }

    public class SubSubWorkItem
    {
        public string jobCode { get; set; }
    }

    public class SubWorkItem
    {
        public string role { get; set; }
        public List<SubSubWorkItem> subSubWorkItem { get; set; }
    }
    /////////////////////////

    public class WorkItems
    {
        public string userName { get; set; }
        public List<Workitem> workitems { get; set; }
    }

    public class Workitem
    {        
        public string workItemId { get; set; }
        public string EnableStdWorkItems { get; set; }
        public string projectType { get; set; }
        public string projectTitle { get; set; }
        public string workItem { get; set; }
        public string role { get; set; }
        public string subWorkItem { get; set; }
        public string jobCode { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }


    public class SubWorkItemModel
    {
        public string isModule { get; set; }
        public string type { get; set; }
        public string workItem { get; set; }
    }

    public class CredentialsModel
    {
        public string AccountID { get; set; }
        public string UserName { get; set; }
    }

    public class ChangePasswordModel
    {
        public string AccountID { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }

    public class TimeSheetSignOff
    {
        public string Resource { get; set; }
        public string SignOffStatus { get; set; }
        public string StartDate { get; set; }
        public string Comments { get; set; }

    }
}