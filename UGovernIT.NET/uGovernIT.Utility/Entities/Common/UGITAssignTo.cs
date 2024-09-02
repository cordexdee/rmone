using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class UGITAssignTo
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string Percentage { get; set; }
        public UGITAssignTo()
        { }
        public UGITAssignTo(string valUserName, string valLoginName, string valPercentage)
        {
            UserName = valUserName;
            LoginName = valLoginName;
            Percentage = valPercentage;
        }
    }
}
