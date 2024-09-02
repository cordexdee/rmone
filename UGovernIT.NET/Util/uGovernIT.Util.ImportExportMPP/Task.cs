using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.Util.ImportExportMPP
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public DateTime ActualStart { get; set; }
        public string Notes { get; set; }
        public double PercentComplete { get; set; }
        public double PercentageWorkComplete {get;set;}
        public bool Milestone { get; set; }
        public string ResourceNames { get; set; }
        public double Duration { get; set; }
        public double HoursPerDay { get; set; }
        public double PercentageComplete { get; set; }

        public int ResourceAssignments { get; set; }
        public double workingMinutePerDay { get; set; }
        public string ParentTask { get; set; }
        public List<TaskDependency> taskDependencyList { get; set; }
        public List<ProjectTaskDependency> taskDeps { get; set; }
        public List<string> taskIds { get; set; }
        public string strMainAssignToPct { get; set; }
        public string OutlineLevel { get; set; }
        public List<UGITAssignTo> listAssignTo { get; set; }
        public string[] predecessorsArray { get; set; }
        public double Work { get; set; }
        public double ActualWork { get; set; }
        public double RemainingWork { get; set; }
        public string Status { get; set; }
        //Added 27 jan 2020
        public double strmainCalculateEstHrsToPct { get; set; }
        //
    }
    public class DataPropert
    {
        public int PMMID { get; set; }
        public int TaskIndex { get; set; }
        public string PredecessorsID { get; set; }
    }
}
