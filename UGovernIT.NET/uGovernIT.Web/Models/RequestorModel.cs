using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.Models
{
    public class RequestorModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public bool IsVIP { get; set; }
        public string UserDeskLoaction { get; set; }
        public string Location { get; set; }
        public string ManagerID { get; set; }
        public string Manager { get; set; }
        public string Department { get; set; }
        public string UserLocationID { get; set; }
        public string FunctionalAreaID { get; set; }
        public string FunctionalAreaName { get; set; }
        public string Name { get; set; }

        public RequestorModel()
        {
          
        }
    }
    public class ProjectScoreModel {
        public long monitorId { get; set; }
        public string monitorOptionName { get; set; }
        public int monitorWeight { get; set; }
        public long pmmid { get; set; }
        public string ProjectMonitorNotes { get; set; }
    }
    public class ProjectScoreModelAutoCalculate
    {
        public int PmmId { get; set; }
        public string MonitorName { get; set; }
        public string ModuleNameLookup { get; set; }
        public bool IsChecked { get; set; }
        public int MonitorStateId { get; set; }
        public string TicketId { get; set; }
    }
    public class TaskListAction
    {
        public string[] TaskKeys { get; set; }
        public string TicketPublicId { get; set; }
        public string TaskType { get; set; }
    }
    public class DuplicateTaskData
    {
        public string[] TaskKeys { get; set; }
        public string ticketId { get; set; }
        public bool copyChild { get; set; }
        public string moduleName { get; set; }
    }
}