using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class Solarwind
    {
        SolarwindConfig config;
        public Solarwind(SolarwindConfig config)
        {
            this.config = config;
    
        }

        public List<SolarwindAssetModel> GetAssets(int page)
        {
            List<SolarwindAssetModel> swAssets = new List<SolarwindAssetModel>();
            // Create WebClient
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Headers["X-FORMS_BASED_AUTH_ACCEPTED"] = "f";
            client.UseDefaultCredentials = true;

            // Consume Service
             string serviceUrl = string.Format("{0}/helpdesk/WebObjects/Helpdesk.woa/ra/Assets?apiKey={1}&page={2}&limit={3}", config.Url, config.APIKey, page, config.Limit);
           // string serviceUrl = string.Format("{0}helpdesk/WebObjects/Helpdesk.woa/ra/Assets?limit={3}&apiKey={1}", config.Url, config.APIKey, page, config.Limit);
           
            //Log.WriteLog("Calling web method: " + serviceUrl);
            byte[] response = client.DownloadData(new Uri(serviceUrl));
            string result = System.Text.Encoding.UTF8.GetString(response);
            dynamic objs = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            if (objs is JArray)
            {
                JArray objArray = objs as JArray;
                foreach (JObject obj in objArray)
                {
                    int assetID = Convert.ToInt32(obj.GetValue("id"));
                    serviceUrl = string.Format("{0}/helpdesk/WebObjects/Helpdesk.woa/ra/Assets/{2}?apiKey={1}", config.Url, config.APIKey, assetID);

                    response = client.DownloadData(new Uri(serviceUrl));
                    result = System.Text.Encoding.UTF8.GetString(response);
                    SolarwindAssetModel asset = Newtonsoft.Json.JsonConvert.DeserializeObject<SolarwindAssetModel>(result);
                    swAssets.Add(asset);
                }
            }

            return swAssets;
        }
    }
}
