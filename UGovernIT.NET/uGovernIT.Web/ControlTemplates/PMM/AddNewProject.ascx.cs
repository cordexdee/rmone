using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.PMM
{
    public partial class AddNewProject : System.Web.UI.UserControl
    {
        protected string PMMURL = string.Empty;
        protected string NPRURL = string.Empty;
        protected string TemplateURL = string.Empty;
        protected string lifecyleText = string.Empty;
        protected string lifecyleid = string.Empty;
        protected string ProjectSummaryLink = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule pmmModule = moduleManager.GetByName(ModuleNames.PMM);
            PMMURL = UGITUtility.GetAbsoluteURL(pmmModule.StaticModulePagePath);

            UGITModule nprModule = moduleManager.GetByName(ModuleNames.NPR);
            NPRURL = UGITUtility.GetAbsoluteURL(nprModule.StaticModulePagePath);

            ProjectSummaryLink = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ctrl=PMM.ProjectCompactView"));
            ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(context);
            lifecyleText = configVariableManager.GetValue(ConfigConstants.DefaultLifeCycle);
            List<LifeCycle> objLifeCycle = new List<LifeCycle>();
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(context);
            objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
            if (objLifeCycle == null || objLifeCycle.Count <= 0)
                return;
            if (!string.IsNullOrEmpty(lifecyleText))
            {              
                var lifecycle = objLifeCycle.FirstOrDefault(x => x.Name == lifecyleText);
                if(lifecycle.ID > 0)
                {
                    lifecyleid = Convert.ToString(lifecycle.ID);
                }
            }
            else
            {
                
                var lifecycle = objLifeCycle.FirstOrDefault();
                if (lifecycle.ID > 0)
                {
                    lifecyleid = Convert.ToString(lifecycle.ID);
                }
            }
            

        }
    }
}