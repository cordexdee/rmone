using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class NewOPMWizard : System.Web.UI.UserControl
    {
        public string ajaxPageURL;
        protected int closeoutperiod = 0;
        public string rmoneControllerUrl = UGITUtility.GetAbsoluteURL("/api/RMOne/");
        public string opmwizardControllerUrl = UGITUtility.GetAbsoluteURL("/api/OPMWizard/");
        public string NewProjectSummaryPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=projectsummary");
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        public string selectionMode;
        public string ticketId;
        public string moduleName;
        public string title;
        public string ticketURL;
        private Ticket moduleRequest;
        public bool HideAllocationTemplate { get; set; }
        public ConfigurationVariableManager ConfigVariableMGR = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public string EnableSimilarityFunction
        {
            get { return UGITUtility.ObjectToString(ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableSimilarityFunction)); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            closeoutperiod = uHelper.getCloseoutperiod(HttpContext.Current.GetManagerContext());
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/module");
            selectionMode = UGITUtility.ObjectToString(Request["SelectionMode"]);
            ticketId = UGITUtility.ObjectToString(Request["ticketId"]);
            moduleName = UGITUtility.ObjectToString(Request["module"]);
            title = UGITUtility.ObjectToString(Request["title"]);
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), moduleName, HttpContext.Current.GetManagerContext().CurrentUser);
                ticketURL = UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath);
            }
            HideAllocationTemplate = uHelper.HideAllocationTemplate(HttpContext.Current.GetManagerContext());
        }
    }
}