using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SolarwindConfig : IntegrationConfig
    {
        public int Limit { get; set; }
        public SolarwindConfig(string url, string apiKey)
        {
            Url = url;
            APIKey = apiKey;
            Limit = 10;
        }
    }
}
