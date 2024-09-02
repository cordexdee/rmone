using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using uGovernIT.Core;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ProjectAllStatusSummary : UserControl
    {

        public string ProjectID { get; set; }
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProjectID == null)
                return;

            UGITModule uGITModule = ModuleManager.LoadByName("PMM");
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            DataRow pmmItem = ticketManager.GetByTicketID(uGITModule, ProjectID);
            if (pmmItem != null)
            {
                List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                if (historyList.Count > 0)
                {
                    rSummaryItems.DataSource = historyList;
                    rSummaryItems.DataBind();
                    pSummary.Visible = true;
                    pEmptyMsg.Visible = false;
                }
            }
        }
    }
}
