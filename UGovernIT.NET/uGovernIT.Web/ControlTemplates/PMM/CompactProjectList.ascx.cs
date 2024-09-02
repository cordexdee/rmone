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

namespace uGovernIT.Web.ControlTemplates.PMM
{
    public partial class CompactProjectList : System.Web.UI.UserControl
    {
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        protected string sourceURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            string title = "Project Import Wizard";
            btNewButton.ToolTip = "Import PMM";
            //btNewbutton.ToolTip = "Import PMM";
            int width = 60;
            int height = 90;
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl?ctrl=PMM.AddNewProject");
            //btNewbutton.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', 'TicketId=0', '{1}', '{3}', '{4}', 0,'{2}');", url, title, sourceURL, width, height));
            btNewButton.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', 'TicketId=0', '{1}', '{3}', '{4}', 0,'{2}');", url, title, sourceURL, width, height));
            ModuleViewManager moduleManagerObj = new ModuleViewManager(AppContext);
            UGITModule module = moduleManagerObj.LoadByName(ModuleNames.PMM);

            TicketManager ticketManagerObj = new TicketManager(AppContext);
            DataTable dtProjects = ticketManagerObj.GetAllOpenTicketsBasedOnModuleName(ModuleNames.PMM);
            if(dtProjects != null)
            {
                grdProjects.DataSource = dtProjects;
                grdProjects.DataBind();
            }
        }
    }
}