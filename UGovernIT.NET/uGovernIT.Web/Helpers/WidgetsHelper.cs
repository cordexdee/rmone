using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web.Helpers
{
    public class WidgetsHelper
    {
        private ApplicationContext _applicationContext { get; set; }
        private WidgetRunResponse widgetRunResponse { get; set; }
        private AgentsManager _agentsManager { get; set; }

        public WidgetsHelper()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _agentsManager = new AgentsManager(_applicationContext);
        }

        public WidgetRunResponse getWidgetResponse( long Id)
        {
            widgetRunResponse = new WidgetRunResponse();
            Agents agents = _agentsManager.LoadByID(Id);
            widgetRunResponse.Url = UGITUtility.GetAbsoluteURL(getWidgetUrl(agents));
           // string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&module={2}";
           // widgetRunResponse.Url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "requestedit", "0", "ACR"));
            widgetRunResponse.Title = agents.Title;
            widgetRunResponse.Width = string.IsNullOrEmpty( agents.Width ) ? "700" : agents.Width;
            widgetRunResponse.Height = string.IsNullOrEmpty(agents.Height) ? "800" : agents.Height;
            return widgetRunResponse;
        }

        public string getWidgetUrl(Agents agents)
        {
            string url = string.Empty;
            string baseUrl = string.Empty;
            string control = string.Empty;
            if(!string.IsNullOrEmpty(agents.Url))
            {
                baseUrl = agents.Url;
                url = agents.Url;
            }
            if (!string.IsNullOrEmpty(agents.Control))
            {
                control = agents.Control;
                url = url + "?control=" + agents.Control;
            }
            if(!string.IsNullOrEmpty(agents.Control) && !string.IsNullOrEmpty(agents.Parameters))
            {
                url = url +'&'+ agents.Parameters;
            }
            if (string.IsNullOrEmpty(agents.Control) && !string.IsNullOrEmpty(agents.Parameters))
            {
                url = url + '?' + agents.Parameters;
            }
            if (agents.ServiceLookUp > 0)
            {
                url = "/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID=" + agents.ServiceLookUp;
            }
            return url;
        }
    }
}