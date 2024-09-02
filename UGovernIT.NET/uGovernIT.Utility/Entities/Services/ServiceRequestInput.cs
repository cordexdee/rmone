
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
   public class TaskTempInfo
    {
        public long TaskID { get; set; }
        public int ModuleID { get; set; }
        public string TicketID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string AssignTo { get; set; }
        public DateTime DueDate { get; set; }
        public double EstimatedHours { get; set; }
        public string ServiceApplicationAccessXml { get; set; }
        public string UGITNewUserName { get; set; }
        public int StageStep { get; set; }
        public string Approver { get; set; }
        public bool SLADisabled { get; set; }
    }

   //[Serializable]
   //public class ServiceQuestionInput
   //{
   //    public bool IsSkiped { get; set; }
   //    public string Token { get; set; }
   //    public string Value { get; set; }
   //    public ServiceQuestionInput()
   //    {
   //        Token = string.Empty;
   //        Value = string.Empty;
   //    }
   //}

   //[Serializable]
   //public class ServiceSectionInput
   //{
   //    public bool IsSkiped { get; set; }
   //    public int SectionID { get; set; }
   //    public List<ServiceQuestionInput> Questions { get; set; }
   //    public ServiceSectionInput()
   //    {
   //        Questions = new List<ServiceQuestionInput>();
   //    }

   //}
}
