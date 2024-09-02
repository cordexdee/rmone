using DevExpress.Web;
using DevExpress.XtraReports.Design.ParameterEditor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.DockPanels;

namespace uGovernIT.Web
{
    public partial class FilterTicketsProperties : UserControl
    {
        public TicketDockPanelSetting ticketDockPanelSetting { get; set; }    
        public string DataFilterUrl { get; set; }
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = ticketDockPanelSetting.Title;
            //txtName.Text = ticketDockPanelSetting.Name;
            txtNoOfTickets.Text = Convert.ToString(ticketDockPanelSetting.PageSize);
            if (ticketDockPanelSetting.ModuleName != null)
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(ticketDockPanelSetting.ModuleName));

            chkTitle.Checked = ticketDockPanelSetting.ShowTitle;
            chkShowBandedRows.Checked = ticketDockPanelSetting.ShowBandedRows;
            chkShowCompactRows.Checked = ticketDockPanelSetting.ShowCompactRows;
            chkModuleLogo.Checked = !ticketDockPanelSetting.HideModuleLogo;
            chkModuleDescription.Checked = !ticketDockPanelSetting.HideModuleDescription;
            chkNewbutton.Checked = !ticketDockPanelSetting.HideNewbutton;
            chkFilteredTabs.Checked = !ticketDockPanelSetting.HideFilteredTabs;
            //chkGlobalSearch.Checked = !ticketDockPanelSetting.HideGlobalSearch;
            //chkStatusOverProgressBar.Checked = !ticketDockPanelSetting.HideStatusOverProgressBar;
            
            string moduleName = ticketDockPanelSetting.ModuleName;
            DataFilterUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagerule&moduleName={0}&controlId={1}&callbackfunction=pickFilter(filter)&BindModuleColumns=true", moduleName, lblDataFilterExpression.ClientID));
            if (!string.IsNullOrEmpty(ticketDockPanelSetting.DataFilterExpression))
            {
                lblDataFilterExpression.Text = ticketDockPanelSetting.DataFilterExpression;
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ticketDockPanelSetting.ShowTitle = chkTitle.Checked;
            ticketDockPanelSetting.Title = txtTitle.Text;
            ticketDockPanelSetting.ShowCompactRows = chkShowCompactRows.Checked;
            ticketDockPanelSetting.ShowBandedRows = chkShowBandedRows.Checked;
            //ticketDockPanelSetting.Name = txtName.Text;
            ticketDockPanelSetting.PageSize = Convert.ToInt32(txtNoOfTickets.Text);
            ticketDockPanelSetting.ModuleName=Convert.ToString(ddlModule.Value);
            ticketDockPanelSetting.HideModuleLogo = !chkModuleLogo.Checked;
            ticketDockPanelSetting.HideModuleDescription = !chkModuleDescription.Checked;
            ticketDockPanelSetting.HideNewbutton = !chkNewbutton.Checked;
            ticketDockPanelSetting.HideFilteredTabs = !chkFilteredTabs.Checked;
            //ticketDockPanelSetting.HideGlobalSearch = !chkGlobalSearch.Checked;
            //ticketDockPanelSetting.HideStatusOverProgressBar = !chkStatusOverProgressBar.Checked;           
            if (hdnDataFilterExpression.Contains("FilterExpression"))
            {
                string filterexpression = Convert.ToString(hdnDataFilterExpression.Get("FilterExpression"));
                ticketDockPanelSetting.DataFilterExpression = filterexpression;
            }
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            //List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup == ticketDockPanelSetting.ModuleName);
            string Viewname = string.Empty;
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(ticketDockPanelSetting.Name)))
                Viewname = ticketDockPanelSetting.Name;
            List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup == ticketDockPanelSetting.ModuleName && z.ViewName == Viewname);
            if (tabView.Count == 0)
            {
                tabView = tabViewManager.Load(z => z.ModuleNameLookup == ticketDockPanelSetting.ModuleName);
                foreach (var item in tabView)
                {
                    item.ID = 0;
                }
            }
            if (tabView != null && ddlModule.SelectedIndex > 0)
            {
                repeaterTabView.DataSource = tabView;
                repeaterTabView.DataBind();
            }
        }

        protected void ddlModule_Init(object sender, EventArgs e)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> modules = moduleManager.Load();
            ddlModule.Items.Clear();

            if (modules.Count > 0)
            {
                ddlModule.TextField = "Title";
                ddlModule.ValueField = "ModuleName";
                ddlModule.DataSource = modules;
                ddlModule.DataBind();
                if(ticketDockPanelSetting.ModuleName!=null)
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(ticketDockPanelSetting.ModuleName)));
            }
        }
        public void CopyFromWebpart(TicketDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            webpart.ShowBandedRows = chkShowBandedRows.Checked;
            webpart.ShowCompactRows = chkShowCompactRows.Checked;
            //webpart.Name = txtName.Text;
            webpart.PageSize = Convert.ToInt32(txtNoOfTickets.Text);
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            webpart.HideModuleLogo = !chkModuleLogo.Checked;
            webpart.HideModuleDescription = !chkModuleDescription.Checked;
            webpart.HideNewbutton = !chkNewbutton.Checked;
            webpart.HideFilteredTabs = !chkFilteredTabs.Checked;
            //webpart.HideGlobalSearch = !chkGlobalSearch.Checked;
            //webpart.HideStatusOverProgressBar = !chkStatusOverProgressBar.Checked;
            //Tab related properties configured from Config_TabView table, Add/ Edit and view will be based on this table
            //table contains record for each tab with view name like 'Home','TSR' etc.
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            TabView tabView = null;
            foreach (RepeaterItem i in repeaterTabView.Items)
            {
                HiddenField hiddenID = (HiddenField)i.FindControl("idField");
                if (hiddenID != null)
                {
                    tabView = tabViewManager.LoadByID(Convert.ToInt64(hiddenID.Value));
                }
                if (tabView != null)
                {
                    TextBox txtDisplay = (TextBox)i.FindControl("txtDisplayName");
                    ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                    if (checkBox != null)
                        tabView.ShowTab = checkBox.Checked;
                    if (txtDisplay != null)
                        tabView.TabDisplayName = Convert.ToString(txtDisplay.Text);
                    tabViewManager.Update(tabView);
                    tabView = null;
                }
            }
            if (hdnDataFilterExpression.Contains("FilterExpression"))
            {
                string filterexpression = Convert.ToString(hdnDataFilterExpression.Get("FilterExpression"));
                ticketDockPanelSetting.DataFilterExpression = filterexpression;
            }
            ticketDockPanelSetting = webpart;
            ticketDockPanelSetting = webpart;
        }
        public void CopyFromControl(TicketDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            webpart.ShowCompactRows = chkShowCompactRows.Checked;
            webpart.ShowBandedRows = chkShowBandedRows.Checked;
            //webpart.Name = txtName.Text;
            webpart.PageSize = Convert.ToInt32(txtNoOfTickets.Text);
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            webpart.HideModuleLogo = !chkModuleLogo.Checked;
            webpart.HideModuleDescription = !chkModuleDescription.Checked;
            webpart.HideNewbutton = !chkNewbutton.Checked;
            webpart.HideFilteredTabs = !chkFilteredTabs.Checked;
            //webpart.HideGlobalSearch = !chkGlobalSearch.Checked;
            //webpart.HideStatusOverProgressBar = !chkStatusOverProgressBar.Checked;
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            foreach (RepeaterItem i in repeaterTabView.Items)
            {
                HiddenField hiddenID = (HiddenField)i.FindControl("idField");
                if (hiddenID != null && Convert.ToInt64(hiddenID.Value) > 0)
                {
                    TabView tabView = tabViewManager.LoadByID(Convert.ToInt64(hiddenID.Value));
                    if (tabView != null)
                    {
                        ASPxTextBox txtDisplay = (ASPxTextBox)i.FindControl("txtDisplayName");
                        ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                        if (checkBox != null)
                            tabView.ShowTab = checkBox.Checked;
                        if (txtDisplay != null)
                            tabView.TabDisplayName = Convert.ToString(txtDisplay.Text);
                        tabView.ViewName = webpart.Name;
                        tabViewManager.Update(tabView);
                    }
                }
                else
                {
                    TabView tab = new TabView();
                    ASPxTextBox txtDisplay = (ASPxTextBox)i.FindControl("txtDisplayName");
                    ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                    HiddenField hdnTabOrderId = (HiddenField)i.FindControl("hdnTabOrder");
                    HiddenField hdnTablableName = (HiddenField)i.FindControl("hdnTablabelName");
                    
                    if (checkBox != null)
                        tab.ShowTab = checkBox.Checked;
                    if (txtDisplay != null)
                        tab.TabDisplayName = Convert.ToString(txtDisplay.Text);
                    tab.ViewName = webpart.Name;
                    tab.ModuleNameLookup= webpart.ModuleName;
                    tab.TabOrder = Convert.ToInt32(hdnTabOrderId.Value);
                    tab.TabName = txtDisplay.ClientInstanceName;
                    tab.ColumnViewName = "MyHomeTab";
                    tab.TablabelName = hdnTablableName.Value;
                    tabViewManager.Insert(tab);
                }
            }
            if (hdnDataFilterExpression.Contains("FilterExpression"))
            {
                string filterexpression = Convert.ToString(hdnDataFilterExpression.Get("FilterExpression"));
                ticketDockPanelSetting.DataFilterExpression = filterexpression;
            }
            ticketDockPanelSetting = webpart;
        }

        protected void control_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                string Viewname = string.Empty;
                TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(ticketDockPanelSetting.Name)))
                    Viewname = ticketDockPanelSetting.Name;
                List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup ==e.Parameter && z.ViewName==Viewname);
                if (tabView.Count == 0)
                {
                    tabView = tabViewManager.Load(z => z.ModuleNameLookup == e.Parameter);
                    foreach (var item in tabView)
                    {
                        item.ID = 0;
                    }
                }
                if (tabView != null)
                {
                    repeaterTabView.DataSource = tabView;
                    repeaterTabView.DataBind();
                }
            }
        }
    }
}
