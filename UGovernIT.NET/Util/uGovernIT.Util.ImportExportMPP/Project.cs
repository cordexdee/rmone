using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.ImportExportMPP
{
    public class Project
    {
       public List<TaskList> taskList { get; set; }
       public double HoursPerDay { get; set; }
       public List<DataPropert> datapropert { get; set; }

    }
} 
