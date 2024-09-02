using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using System.Data;
using Newtonsoft.Json;

namespace uGovernIT.Web
{
    public partial class NewLoginWizard : System.Web.UI.Page
    {
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public string moduletypejson { get; set; }
        public string TenantAccountID { get; set; }
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            TenantAccountID = AppContext.TenantAccountId;
            string configModuleTypes = configManager.GetValue("ModuleTypes");
            Dictionary<string, string> lstmodules = UGITUtility.DeserializeDicObjects(configModuleTypes, ";", ":");
            //moduletypejson = UGITUtility.GetJsonForDictionary(lstmodules);
            moduletypejson = JsonConvert.SerializeObject(lstmodules.Select(x => new { id = x.Key, name=x.Value }));
            MainMaster mainMasterPage = Page.Master as MainMaster;
            if(mainMasterPage != null)
                mainMasterPage.IsHideMenuBar = true;

        }
    }
}