using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace uGovernIT.Util.ImportExportMPP
{
    public class TaskDependency
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
