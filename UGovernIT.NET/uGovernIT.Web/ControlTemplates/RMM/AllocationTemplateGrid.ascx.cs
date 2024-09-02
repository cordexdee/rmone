using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class AllocationTemplateGrid : System.Web.UI.UserControl
    {
        public string TemplateID { get; set; }
        public string ProjectID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Option { get; set; }
        public bool ShowTotalAllocationsInSearch { get; set; }
        public bool IsGroupAdmin { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            List<Role> userRoles = context.UserManager.GetUserRoles(context.CurrentUser.Id);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            ConfigurationVariable adminGroup = configManager.LoadVaribale(ConfigConstants.AdminGroup);
            ShowTotalAllocationsInSearch = configManager.GetValueAsBool(ConfigConstants.ShowTotalAllocationsInSearch);
            if (adminGroup != null)
            {
                string[] adminGroups = UGITUtility.SplitString(adminGroup.KeyValue, Constants.Separator5);
                foreach (string s in adminGroups)
                {
                    if (userRoles.Exists(x => x.Title == s))
                    {
                        IsGroupAdmin = true;
                        break;
                    }
                }
            }
        }
    }
}