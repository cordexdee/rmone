using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web.Modules
{
    public partial class Request :UPage
    {
        public string TicketId = string.Empty;
        public string GlobalSerach = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext().TenantAccountId}|{HttpContext.Current.CurrentUser().Name}: Visited Page: Request.aspx");
            Control ctrl = null;
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("module"))
                {
                    string module = Convert.ToString(Request.QueryString["module"]);
                    TicketId = Convert.ToString(Request.QueryString["TicketId"]);

                    if (module == "PMM")
                    {
                        bool checkPMM = TicketId.Length > 1;
                        if (checkPMM && TicketId.Substring(0, 3).ToString() == "PMM")
                        {
                            uGovernITModuleWebpartUserControl ticketCtrl = (uGovernITModuleWebpartUserControl)Page.LoadControl("~/ControlTemplates/Shared/uGovernITModuleWebpartUserControl.ascx");
                            ctrl = ticketCtrl;
                        }
                        else
                        {
                            uGovernITProjectWizardUserControl pmmCtrl = (uGovernITProjectWizardUserControl)Page.LoadControl("~/ControlTemplates/Shared/uGovernITProjectWizardUserControl.ascx");
                            ctrl = pmmCtrl;
                        }
                    }
                    else
                    {
                        uGovernITModuleWebpartUserControl ticketCtrl = (uGovernITModuleWebpartUserControl)Page.LoadControl("~/ControlTemplates/Shared/uGovernITModuleWebpartUserControl.ascx");
                        ctrl = ticketCtrl;
                    }
                }
                //else {
                //    Page.MasterPageFile = null;
                //    CustomFilteredTickets ticketCtrl = (CustomFilteredTickets)Page.LoadControl("/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                //    ctrl = ticketCtrl;
                //}
                managementControlsRequest.Controls.Add(ctrl);
            }
            else
            {

            }

            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
          
        }
    }
}