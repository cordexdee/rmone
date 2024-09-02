using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class TicketTaskControl : UserControl
    {
        public string landingPageUrl = string.Empty;
        DataTable tofillGridDue = new DataTable();
        private ApplicationContext _context = null;
        DataTable modulewaitingonme = new DataTable();
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        private ModuleViewManager _moduleViewManager = null;
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        public string absoluteurl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx");
        public string ViewMode { get; set; }
        public string type { get; set; }
        //public string cardview { get; set; }
        //public string gantview { get; set; }
        //public string calendar { get; set; }

        protected override void OnInit(EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserProfile user = HttpContext.Current.CurrentUser();
            if (user != null)
            {
                if (ApplicationContext != null && !string.IsNullOrEmpty(user.UserRoleId))
                {
                    landingPageUrl = UGITUtility.GetAbsoluteURL(new LandingPagesManager(ApplicationContext).GetLandingPageById(user.UserRoleId));
                }
            }

            if (Request.QueryString["Viewmode"] != null)
            {
                ViewMode = Convert.ToString(Request.QueryString["Viewmode"]);
            }
            if (Request.QueryString["type"] != null)
            {
                type = Convert.ToString(Request.QueryString["type"]);
            }

            if (!IsPostBack)
            {
                gridPanel.Visible = false;
                CardView.Visible = true;
                CardViewRecentTask.Visible = true;
                headCardView.Visible = true;
                headCardViewRecentTask.Visible = true;
            }
            var HomecardView = Page.Master.Master.FindControlRecursive("UserDashboardCardView") as ASPxCardView;
            HomecardView.CssClass = "cardView-wrapper cardViewTable-wrap";
            BindAspxGridView();

            if (ViewMode == "gridview")
            {
                gridPanel.Visible = true;
                CardView.Visible = false;
                CardViewRecentTask.Visible = false;
                //taskScheduler.Visible = false;
                headCardView.Visible = false;
                headCardViewRecentTask.Visible = false;
            }
            else if (ViewMode == "cardview")
            {
                gridPanel.Visible = false;
                CardView.Visible = true;
                CardViewRecentTask.Visible = true;
                //taskScheduler.Visible = false;
                headCardView.Visible = true;
                headCardViewRecentTask.Visible = true;
            }
            else if (ViewMode == "calendar")
            {
                gridPanel.Visible = false;
                CardView.Visible = false;
                CardViewRecentTask.Visible = false;
                //taskScheduler.Visible = true;
                headCardView.Visible = false;
                headCardViewRecentTask.Visible = false;
            }
                        
            //start
            CustomFilteredTickets dashboardTickets = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");

            string ticketModuleName = Convert.ToString(Request["Module"]);
            string status = Convert.ToString(Request["Status"]);
            //string UserType = Convert.ToString(Request["UserType"]);
            //UserType = UserType.Replace(" ", string.Empty);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //TicketManager ticketMgr = new TicketManager(context);
            ModuleViewManager moduleMgr = new ModuleViewManager(context);
            UGITModule homeModule = moduleMgr.LoadByName(ticketModuleName);

            getDataSource("myclosedtickets");
            tofillGridDue.Merge(getDataSource("myclosedtickets"));
            dashboardTickets.FilteredTable = tofillGridDue;

            dashboardTickets.IsFilteredTableExist = true;
            dashboardTickets.HideAllTicketTab = true;
            dashboardTickets.HideModuleDesciption = true;
            dashboardTickets.HideNewTicketButton = false;
            dashboardTickets.HideModuleDetail = true;
            dashboardTickets.HideReport = true;
            dashboardTickets.ModuleName = ticketModuleName;
            gridPanel.Controls.Add(dashboardTickets);
            //end
        }

        private void BindAspxGridView()
        {
            //grid.DataSource = getDataSource(type);
            //grid.DataBind();

            //taskScheduler.DataSource = getDataSource(type);
            //taskScheduler.DataBind();
            //taskScheduler.AppointmentDataSource = getDataSource(type);
            //taskScheduler.DataBind();


            DataTable cardViewData = getDataSource(type);
            if (cardViewData != null && cardViewData.Rows.Count > 0)
            {
                tofillGridDue = cardViewData;
                cardViewData.Columns.Add("KeyId", typeof(string));
                DateTime maxDate = Convert.ToDateTime(cardViewData.Compute("MAX(DesiredCompletionDate)", null));
                int count = 0;
                foreach (DataRow dr in cardViewData.Rows)
                {
                    dr["KeyId"] = count;
                    if (dr["DesiredCompletionDate"] == DBNull.Value)
                    {
                        dr["DesiredCompletionDate"] = maxDate;
                    }
                    count = count + 1;
                }
                cardViewData.DefaultView.Sort = "DesiredCompletionDate";
                cardViewData = cardViewData.DefaultView.ToTable();
                cardViewData.Columns.Add("AgeText", typeof(string));
                cardViewData.Columns.Add("Age", typeof(int));
                cardViewData.Columns.Add("Color", typeof(string));
               
                cardViewData = getFilteredData(cardViewData);
                CardView.DataSource = cardViewData;
                CardView.DataBind(); 
            }


            DataTable cardViewDataCom = getDataSource("myclosedtickets");
            if (cardViewDataCom != null && cardViewDataCom.Rows.Count > 0)
            {
                int count = 0;
                DateTime maxDateCom = Convert.ToDateTime(cardViewDataCom.Compute("MAX(DesiredCompletionDate)", null));
                foreach (DataRow dr in cardViewData.Rows)
                {
                    dr["KeyId"] = count;
                    if (dr["DesiredCompletionDate"] == DBNull.Value)
                    {
                        dr["DesiredCompletionDate"] = maxDateCom;
                    }
                    count = count + 1;
                }
                cardViewDataCom.Columns.Add("AgeText", typeof(string));
                cardViewDataCom.Columns.Add("Age", typeof(int));
                cardViewDataCom.Columns.Add("Color", typeof(string));
                cardViewDataCom.DefaultView.Sort = "CloseDate";
                cardViewDataCom = cardViewDataCom.DefaultView.ToTable();
                cardViewDataCom = getFilteredDataCom(cardViewDataCom);

                CardViewRecentTask.DataSource = cardViewDataCom;
                CardViewRecentTask.DataBind(); 
            }
        }

        public DataTable getDataSource(string type)
        {

            modulewaitingonme = new DataTable();
            TabViewManager tabViewManager = new TabViewManager(_context);
            List<string> tabs = new List<string>();
            List<TabView> tabViewrows = tabViewManager.GetTabsByViewName("Home");
            foreach (TabView item in tabViewrows)
            {
               // Tab newtab = new Tab(Convert.ToString(item.TabDisplayName), Convert.ToString(item.TabName));
                tabs.Add(item.TabName);
            }

            tabs.Add(FilterTab.myclosedtickets);

            _moduleViewManager = new ModuleViewManager(_context);
            List<UGITModule> listModule = _moduleViewManager.LoadAllModule();
            listModule = listModule.Where(x=>x.EnableModule == true).ToList<UGITModule>();


            List<UGITModule> moduleTable = new List<UGITModule>();
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            ModuleStatisticResponse moduleStatResult;
            ModuleStatistics mod = new ModuleStatistics(_context);
            mRequest.Tabs = tabs;
            mRequest.CurrentTab = FilterTab.waitingonme;
            if (listModule.Count() > 0)
            {
                moduleTable = listModule.Where(x => x.ModuleType == ModuleType.SMS).ToList();
            }
            for(int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    mRequest.CurrentTab = FilterTab.waitingonme;
                }
                else
                {
                    mRequest.CurrentTab = FilterTab.myopentickets;
                }

                if (type == FilterTab.myclosedtickets)
                {
                    mRequest.CurrentTab = FilterTab.myclosedtickets;
                    i = 1;
                }
                foreach (UGITModule module in moduleTable)
                {
 
                    mRequest.ModuleName = module.ModuleName;
                    moduleStatResult = mod.Load(mRequest);
                    
                    if (moduleStatResult.ResultedData != null)
                    {
                        modulewaitingonme.Merge(moduleStatResult.ResultedData);
                    }

                    
                }

            }
            


            return modulewaitingonme;




        }

        public DataTable getFilteredData(DataTable cardViewData)
        {
            foreach (DataRow dr in cardViewData.Rows)
            {
                if (dr["DesiredCompletionDate"] != DBNull.Value)
                {
                    //dr["DesiredCompletionDate"] = DateTime.Today;
                    
                    int Age = UGITUtility.GetDueInValue(Convert.ToDateTime(dr["DesiredCompletionDate"]));

                    if (Age < 0)
                    {
                        dr["AgeText"] = $" {Math.Abs(Age)} days late";
                        dr["Age"] = Age;

                        dr["color"] = "High";
                    }
                    else if (Age > 0)
                    {
                        if (Age == 1)
                        {
                            dr["AgeText"] = $"Due in {Math.Abs(Age)} day";
                        }
                        else
                        {
                            dr["AgeText"] = $"Due in {Math.Abs(Age)} days";
                        }
                        dr["Age"] = Age;
                        if (Age <= 3)
                        {
                            dr["color"] = "Middle";
                        }
                        else
                        {
                            dr["color"] = "Low";
                        }
                    }
                    else if (Age == 0)
                    {
                        dr["AgeText"] = $"Today";
                        dr["color"] = "Middle";
                    }
                }
            }

            return cardViewData;
        }

        public DataTable getFilteredDataCom(DataTable cardViewData)
        {
            foreach (DataRow dr in cardViewData.Rows)
            {
                if (dr["CloseDate"] != DBNull.Value)
                {
                    //dr["DesiredCompletionDate"] = DateTime.Today;
                    
                    int Age = UGITUtility.GetDueInValue(Convert.ToDateTime(dr["CloseDate"]));

                    if (Convert.ToDateTime(dr["CloseDate"]).Date == DateTime.Now.Date)
                    {
                        dr["AgeText"] = $" Today";
                    }
                    else
                    {
                        dr["AgeText"] = Convert.ToDateTime(dr["CloseDate"]).ToString("MM/dd/yyyy");
                    }
                }
            }
            

            return cardViewData;
        }
        protected void CardView_HtmlCardPrepared(object sender, DevExpress.Web.ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            string viewUrl = string.Empty;
            string title = string.Empty;
            string func = string.Empty;
            string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            sourceURL = "/default.aspx";
            string color = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, "Color"));
            string ticketID = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string ticketTitle = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
           // string ItemOrder = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ItemOrder));
            if (!string.IsNullOrEmpty(ticketID))
            {
                if (color == "Middle")
                {
                    e.Card.BorderColor = Color.Orange;
                    e.Card.CssClass = "PendingOrange col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
                }

                else if (color == "Low")
                {
                    e.Card.BorderColor = Color.DarkGray;
                    e.Card.CssClass = "PendingGrey col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
                }
                else if (color == "High")
                {
                    e.Card.BorderColor = Color.Red;
                    e.Card.CssClass = "PendingRed col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
                }
                string module = uHelper.getModuleNameByTicketId(ticketID);
                DataRow moduleDetail = null;
                if (!string.IsNullOrEmpty(module))
                {
                    DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(module));
                    if (moduledt.Rows.Count > 0) 
                        moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                }


                if (moduleDetail != null)
                {
                    viewUrl = string.Empty;
                    if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                    {
                        viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                    }


                    if (!string.IsNullOrEmpty(ticketTitle))
                    {
                        ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                    }

                    if (!string.IsNullOrEmpty(ticketID))
                    {
                        title = string.Format("{0}: ", ticketID);
                    }
                    title = string.Format("{0}{1}", title, ticketTitle);

                }

                func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), title, sourceURL, 90, 90);
                e.Card.Attributes.Add("onclick", func);
                //e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                e.Card.Style.Add("cursor", "pointer");
            }
        }

        protected void CardViewRecentTask_HtmlCardPrepared(object sender, DevExpress.Web.ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            string viewUrl = string.Empty;
            string title = string.Empty;
            string func = string.Empty;
            string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            sourceURL = "/default.aspx";
            string color = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, "Color"));
            string ticketID = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string  ticketTitle = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ID));
            //string ItemOrder = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ItemOrder));
            if (!string.IsNullOrEmpty(ticketID))
            {
                e.Card.BorderColor = Color.SkyBlue;
                e.Card.CssClass = "CompletedBlue col-md-2 col-sm-3 col-xs-12 noPadding colFormd";

                string module = uHelper.getModuleNameByTicketId(ticketID);
                DataRow moduleDetail = null;
                if (!string.IsNullOrEmpty(module))
                {
                    DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(module));
                    if (moduledt.Rows.Count > 0)
                        moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                }


                if (moduleDetail != null)
                {
                    viewUrl = string.Empty;
                    if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                    {
                        viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                    }


                    if (!string.IsNullOrEmpty(ticketTitle))
                    {
                        ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                    }

                    if (!string.IsNullOrEmpty(ticketID))
                    {
                        title = string.Format("{0}: ", ticketID);
                    }
                    title = string.Format("{0}{1}", title, ticketTitle);
                }

                func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), title, sourceURL, 90, 90);
                e.Card.Attributes.Add("onclick", func);
                //e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                e.Card.Style.Add("cursor", "pointer");                
            }
        }

        protected void CardView_CardLayoutCreated(object sender, DevExpress.Web.ASPxCardViewCardLayoutCreatedEventArgs e)
        {
        }

        protected void CardView_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            var data = e.LayoutData;
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = grid.GetDataRow(e.VisibleIndex);
                if (row != null)
                {
                    string ticketID = Convert.ToString(row[DatabaseObjects.Columns.TicketId]);
                    string taskId = Convert.ToString(row[DatabaseObjects.Columns.ID]);

                    string url = "/Layouts/ugovernit/delegatecontrol.aspx";
                    string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName=CPR&control=taskedit&taskType=task", ticketID, taskId);
                    string title = $" Edit task for {ticketID}";
                    e.Row.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
           // grid.Visible = true;
            gridPanel.Visible = true;
            CardView.Visible = false;
            CardViewRecentTask.Visible = false;
            //taskScheduler.Visible = false;
            headCardView.Visible = false;
            headCardViewRecentTask.Visible = false;
            dvViewChangeCardPending.Visible = true;
        }

        protected void viewChange_Click(object sender, ImageClickEventArgs e)
        {
            //grid.Visible = true;
            gridPanel.Visible = true;
            CardView.Visible = false;
            CardViewRecentTask.Visible = false;
            //taskScheduler.Visible = false;
            headCardView.Visible = false;
            headCardViewRecentTask.Visible = false;
            dvViewChangeCardPending.Visible = true;
        }

        protected void ViewChangeCardPending_Click(object sender, ImageClickEventArgs e)
        {
            //grid.Visible = false;
            gridPanel.Visible = false;
            CardView.Visible = true;
            CardViewRecentTask.Visible = true;
            //taskScheduler.Visible = false;
            viewChange.Visible = true;
            headCardView.Visible = true;
            headCardViewRecentTask.Visible = true;
            dvViewChangeCardPending.Visible = false;
        }

        //protected void CardView_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        //{
        //    grid.Visible = true;
        //    CardView.Visible = false;
        //    CardViewRecentTask.Visible = false;
        //    //taskScheduler.Visible = false;
        //    headCardView.Visible = false;
        //    headCardViewRecentTask.Visible = false;
        //    dvViewChangeCardPending.Visible = true;
        //}

        //protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    grid.Visible = false;
        //    CardView.Visible = true;
        //    CardViewRecentTask.Visible = true;
        //    //taskScheduler.Visible = false;
        //    viewChange.Visible = true;
        //    headCardViewRecentTask.Visible = true;
        //    dvViewChangeCardPending.Visible = false;

        //}
    }
}