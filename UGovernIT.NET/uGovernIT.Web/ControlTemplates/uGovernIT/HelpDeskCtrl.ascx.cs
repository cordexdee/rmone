using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class HelpDeskCtrl : UserControl
    {
        ApplicationContext context;

        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            UGITUtility.CreateCookie(Response, "TSRClicked", "True");
            Response.Redirect("~/pages/HomeHelpDeskTSR");
        }

        protected void TicketCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (string.IsNullOrEmpty(e.Parameter))
                BindTickets("TSR");
            else
                BindTickets(Convert.ToString(e.Parameter));
        }

        private void BindTickets(string SelectedItem)
        {
            //dvTicket.Controls.Clear();

            // UserControl customFilteredTickets = (UserControl)Page.LoadControl("~/TestCustomFilteredTickets.ascx");

            CustomFilteredTickets customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
            // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpneChangeTicketTypeDialog", "javascript:OpneChangeTicketTypeDialog( );", false);

            //ApplicationContext context = HttpContext.Current.GetManagerContext();
            //TicketManager ticketMgr = new TicketManager(context);
            //ModuleViewManager moduleMgr = new ModuleViewManager(context);
            //UGITModule homeModule = moduleMgr.LoadByName(SelectedItem);

            //DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
            //customFilteredTickets.FilteredTable = dthometickets;
            //customFilteredTickets.ModuleName = SelectedItem;
            //customFilteredTickets.HideModuleDetail = false;
            //customFilteredTickets.MTicketStatus = TicketStatus.WaitingOnMe;
            //customFilteredTickets.MyHomeTab = Constants.MyHomeTicketTab;
            //customFilteredTickets.HideAllTicketTab = false;
            dvTicket.Controls.Add(customFilteredTickets);
        }

        protected void TSR_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/HomeHelpDeskTSR");
        }

        protected void SVC_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/HomeHelpDeskSVC");
        }

        protected void DRQ_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/HomeHelpDeskDRQ");
        }

        protected void ACR_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/HomeHelpDeskACR");
        }



        //protected void TSR_Click(object sender, EventArgs e)
        //{
        //   UGITUtility.CreateCookie(Response, "TSRClicked", "True");

        //    //CustomFilteredTickets customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
        //    // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpneChangeTicketTypeDialog", "javascript:OpneChangeTicketTypeDialog( );", false);

        //    ApplicationContext context = HttpContext.Current.GetManagerContext();
        //    Response.Redirect("~/pages/TSRTickets");
        //    //TicketManager ticketMgr = new TicketManager(context);
        //    //ModuleViewManager moduleMgr = new ModuleViewManager(context);
        //    //UGITModule homeModule = moduleMgr.LoadByName("TSR");

        //    //DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
        //    //customFilteredTickets.FilteredTable = dthometickets;
        //    //customFilteredTickets.ModuleName = homeModule.ModuleName;
        //    //customFilteredTickets.HideModuleDetail = false;
        //    //customFilteredTickets.MTicketStatus = TicketStatus.WaitingOnMe;
        //    //customFilteredTickets.MyHomeTab = Constants.MyHomeTicketTab;
        //    //customFilteredTickets.HideAllTicketTab = false;
        //    //dvTicket.Controls.Add(customFilteredTickets);

        //    //Do somthing
        //}

        //protected void SVC_Click(object sender, EventArgs e)
        //{
        //    UGITUtility.CreateCookie(Response, "SVCClicked", "True");
        //    UGITUtility.CreateCookie(Response, "TSRClicked", "False");

        //    CustomFilteredTickets customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
        //    // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpneChangeTicketTypeDialog", "javascript:OpneChangeTicketTypeDialog( );", false);

        //    ApplicationContext context = HttpContext.Current.GetManagerContext();
        //    TicketManager ticketMgr = new TicketManager(context);
        //    ModuleViewManager moduleMgr = new ModuleViewManager(context);
        //    UGITModule homeModule = moduleMgr.LoadByName("SVC");

        //    //DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
        //    //customFilteredTickets.FilteredTable = dthometickets;
        //    customFilteredTickets.ModuleName = homeModule.ModuleName;
        //    customFilteredTickets.HideModuleDetail = false;
        //    customFilteredTickets.UserType = "my";
        //    customFilteredTickets.MTicketStatus = TicketStatus.Open;
        //    customFilteredTickets.MyHomeTab = Constants.MyHomeTicketTab;
        //    customFilteredTickets.HideAllTicketTab = true;
        //    //customFilteredTickets.Controls.Contains("")
        //    //customFilteredTickets.hide
        //    dvTicket.Controls.Add(customFilteredTickets);

        //    //Do somthing
        //}

        //protected void DRQ_Click(object sender, EventArgs e)
        //{
        //    UGITUtility.CreateCookie(Response, "DRQClicked", "True");
        //    UGITUtility.CreateCookie(Response, "TSRClicked", "False");
        //    CustomFilteredTickets customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
        //    // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpneChangeTicketTypeDialog", "javascript:OpneChangeTicketTypeDialog( );", false);

        //    ApplicationContext context = HttpContext.Current.GetManagerContext();
        //    TicketManager ticketMgr = new TicketManager(context);
        //    ModuleViewManager moduleMgr = new ModuleViewManager(context);
        //    UGITModule homeModule = moduleMgr.LoadByName("DRQ");

        //    DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
        //    customFilteredTickets.FilteredTable = dthometickets;
        //    customFilteredTickets.ModuleName = homeModule.ModuleName;
        //    customFilteredTickets.HideModuleDetail = false;
        //    customFilteredTickets.MTicketStatus = TicketStatus.WaitingOnMe;
        //    customFilteredTickets.MyHomeTab = Constants.MyHomeTicketTab;
        //    customFilteredTickets.HideAllTicketTab = false;
        //    dvTicket.Controls.Add(customFilteredTickets);

        //    //Do somthing
        //}

        //protected void ACR_Click(object sender, EventArgs e)
        //{
        //    CustomFilteredTickets customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
        //    // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpneChangeTicketTypeDialog", "javascript:OpneChangeTicketTypeDialog( );", false);

        //    UGITUtility.CreateCookie(Response, "ACRClicked", "True");
        //    UGITUtility.CreateCookie(Response, "TSRClicked", "False");
        //    ApplicationContext context = HttpContext.Current.GetManagerContext();
        //    TicketManager ticketMgr = new TicketManager(context);
        //    ModuleViewManager moduleMgr = new ModuleViewManager(context);
        //    UGITModule homeModule = moduleMgr.LoadByName("ACR");

        //    DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
        //    customFilteredTickets.FilteredTable = dthometickets;
        //    //customFilteredTickets.ModuleName = homeModule.ModuleName;
        //    //customFilteredTickets.HideModuleDetail = false;
        //    //customFilteredTickets.MTicketStatus = TicketStatus.WaitingOnMe;
        //    //customFilteredTickets.MyHomeTab = Constants.MyHomeTicketTab;
        //    //customFilteredTickets.HideAllTicketTab = false;
        //    dvTicket.Controls.Add(customFilteredTickets);

        //    //Do somthing
        //}


    }
}