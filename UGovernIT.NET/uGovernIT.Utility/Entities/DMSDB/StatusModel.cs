using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Utility.Entities.DMSDB
{
    public class StatusModel
    {
        public bool IsSuccess { get; set; }
        // ErrorCode : -999 is session is already expired
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}