using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class ProjectTaskDependency
    {
        public int Index { get; set; }
        public double Lag { get; set; }
        public ProjectFormatUnit LagType { get; set; }
        public ProjectTaskLinkType Type { get; set; }
        public int FromID { get; set; }
        public int ToID { get; set; }
        public int ParenID { get; set; }

    }
}
