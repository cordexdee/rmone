using System;
using System.Configuration;

namespace uGovernIT.Manager
{
    public class ConfigHelper
    {
        public static string DefaultTenant
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings["DefaultTenant"]);
            }
        }
    }
}
