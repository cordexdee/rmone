using System.Collections.Generic;

namespace uGovernIT.Web.Models
{
    public class Parameter
    {
        public string NameParameter { get; set; }
        public string ValueParameter { get; set; }
    }

    public class Workflow
    {
        public Workflow()
        {
            AgentParameters = new List<Parameter>();
        }
        public string Title { get; set; }
        public string TicketId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskId { get; set; }
        public string  ChildModuleName { get; set; }

        public List<Parameter> AgentParameters { get; set; }
    }

    public class ServiceAgent
    {
        public ServiceAgent()
        {
            Workflow = new Workflow();
        }
        public Workflow Workflow { get; set; }
    }

}