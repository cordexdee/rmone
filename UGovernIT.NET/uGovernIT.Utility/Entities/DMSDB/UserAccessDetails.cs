using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Utility.Entities.DMSDB
{
    public class UserAccessDetails
    {
        public int? fileId { get; set; }
        public int? directoryId { get; set; }
        public int readAccess { get; set; }
        public int writeAccess { get; set; }
    }
}