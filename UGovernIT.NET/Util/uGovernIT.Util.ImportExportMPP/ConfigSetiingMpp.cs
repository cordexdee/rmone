using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.ImportExportMPP
{
    public class ConfigSetiingMpp
    {
        public bool EnableExportImport { get; set; }
        public bool UseMSProject { get; set; }

        public bool importDatesOnly { get; set; }
        public bool dontImportAssignee { get; set; }
        //Added 27 jan 2020
        public bool calculateEstimatedHrs { get; set; }
        //
    }
}
