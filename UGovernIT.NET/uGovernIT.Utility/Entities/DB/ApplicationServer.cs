using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ApplicationServers)]
    public class ApplicationServer : DBBaseEntity
    {
        public long ID { get; set; }
        public long APPTitleLookup { get; set; }
        public long AssetsTitleLookup { get; set; }
        public long EnvironmentLookup { get; set; }
        public string Title { get; set; } 
        public string ServerFunctionsChoice { get; set; }

       
    }
}
