using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web
{
    public partial class SLADashboardProperties : UserControl
    {
        public DashboardSLADockPanelSetting dashboardSlaDockPanelSetting { get; set; }
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        public string Module { get; set; }
        public string SLAEnableModules { get; set; }
        ASPxListBox list;
        Dictionary<string, string> dicSelectedModule;

        protected override void OnInit(EventArgs e)
        {
            dicSelectedModule = new Dictionary<string, string>();
            chkTitle.Checked = dashboardSlaDockPanelSetting.ShowTitle;
            txtTitle.Text = dashboardSlaDockPanelSetting.Title;
            ddlViewFilter.Value = dashboardSlaDockPanelSetting.FilterView;
            chkIncludeOpenTickets.Checked = dashboardSlaDockPanelSetting.IncludeOpen;
            chkShowSLAName.Checked = dashboardSlaDockPanelSetting.ShowSLAName;
            txtSlaTolerance.Text = dashboardSlaDockPanelSetting.LegendSetting;

            if (!string.IsNullOrEmpty(dashboardSlaDockPanelSetting.StringOfSelectedModule))
            {
                ddeModules_Init(ddeModules, null);
            }

            if (!IsPostBack)
            {
                ddeModules_Init(ddeModules, null);
                BindPeriodFilter(ddlViewFilter);
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dashboardSlaDockPanelSetting.ShowTitle = chkTitle.Checked;
            dashboardSlaDockPanelSetting.Title = dashboardSlaDockPanelSetting.ContentTitle = txtTitle.Text.Trim();
            dashboardSlaDockPanelSetting.IncludeOpen = chkIncludeOpenTickets.Checked;
            dashboardSlaDockPanelSetting.ShowSLAName = chkShowSLAName.Checked;
            dashboardSlaDockPanelSetting.Module = string.Empty;
            dashboardSlaDockPanelSetting.LegendSetting = txtSlaTolerance.Text.Trim();
            dashboardSlaDockPanelSetting.FilterView = string.Empty;
            dashboardSlaDockPanelSetting.StringOfSelectedModule = string.Empty;

            ListEditItem viewFilterItem = ddlViewFilter.SelectedItem;
            if (viewFilterItem != null)
                dashboardSlaDockPanelSetting.FilterView = viewFilterItem.Text;   //Convert.ToString(ddlViewFilter.Value);

            if (list != null && list.SelectedValues.Count > 0)
            {
                List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                List<string> lstSelectedModule = new List<string>();
                foreach (ListEditItem item in moduleList)
                {
                    string key = Convert.ToString(item.Value);
                    
                    if (!dicSelectedModule.ContainsKey(key))
                    {
                        dicSelectedModule.Add(key, item.Text);
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }
                }

                dashboardSlaDockPanelSetting.StringOfSelectedModule = string.Join(";", lstSelectedModule);
            }
                
        }

        public void CopyFromWebpart(DashboardSLADockPanelSetting webpart)
        {
            if (dashboardSlaDockPanelSetting != null)
            {
                webpart.ShowTitle = dashboardSlaDockPanelSetting.ShowTitle;
                webpart.Title = dashboardSlaDockPanelSetting.Title;
                webpart.ContentTitle = dashboardSlaDockPanelSetting.ContentTitle;
                webpart.FilterView = dashboardSlaDockPanelSetting.FilterView;
                webpart.IncludeOpen = dashboardSlaDockPanelSetting.IncludeOpen;
                webpart.ShowSLAName = dashboardSlaDockPanelSetting.ShowSLAName;
                webpart.StringOfSelectedModule = dashboardSlaDockPanelSetting.StringOfSelectedModule;
                webpart.SLAEnableModules = dashboardSlaDockPanelSetting.SLAEnableModules;
                webpart.Module = dashboardSlaDockPanelSetting.Module;
                webpart.LegendSetting = dashboardSlaDockPanelSetting.LegendSetting;
            }
            else
            {
                webpart.ShowTitle = chkTitle.Checked;
                webpart.Title = webpart.ContentTitle = webpart.Name = txtTitle.Text.Trim();
                webpart.FilterView = dashboardSlaDockPanelSetting.FilterView;
                webpart.IncludeOpen = chkIncludeOpenTickets.Checked;
                webpart.ShowSLAName = chkShowSLAName.Checked;
                webpart.StringOfSelectedModule = string.Empty;
                webpart.SLAEnableModules = SLAEnableModules;
                webpart.Module = Module;
                webpart.LegendSetting = txtSlaTolerance.Text.Trim();

                ListEditItem viewFilterItem = ddlViewFilter.SelectedItem;
                if (viewFilterItem != null)
                    webpart.FilterView = viewFilterItem.Text;

                if (list != null && list.SelectedValues.Count > 0)
                {
                    List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                    List<string> lstSelectedModule = new List<string>();
                    foreach (ListEditItem item in moduleList)
                    {
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }

                    webpart.StringOfSelectedModule = string.Join(";", lstSelectedModule);
                }

                dashboardSlaDockPanelSetting = webpart;
            }
        }

        public void CopyFromControl(DashboardSLADockPanelSetting webpart)
        {
            if (dashboardSlaDockPanelSetting != null)
            {
                webpart.ShowTitle = dashboardSlaDockPanelSetting.ShowTitle;
                webpart.Title = dashboardSlaDockPanelSetting.Title;
                webpart.ContentTitle = dashboardSlaDockPanelSetting.ContentTitle;
                webpart.FilterView = dashboardSlaDockPanelSetting.FilterView;
                webpart.IncludeOpen = dashboardSlaDockPanelSetting.IncludeOpen;
                webpart.ShowSLAName = dashboardSlaDockPanelSetting.ShowSLAName;
                webpart.StringOfSelectedModule = dashboardSlaDockPanelSetting.StringOfSelectedModule;
                webpart.SLAEnableModules = dashboardSlaDockPanelSetting.SLAEnableModules;
                webpart.Module = dashboardSlaDockPanelSetting.Module;
                webpart.LegendSetting = dashboardSlaDockPanelSetting.LegendSetting;
            }
            else
            {
                webpart.ShowTitle = chkTitle.Checked;
                webpart.Title = webpart.ContentTitle = webpart.Name = txtTitle.Text.Trim();
                webpart.FilterView = dashboardSlaDockPanelSetting.FilterView;
                webpart.IncludeOpen = chkIncludeOpenTickets.Checked;
                webpart.ShowSLAName = chkShowSLAName.Checked;
                webpart.StringOfSelectedModule = string.Empty;
                webpart.SLAEnableModules = SLAEnableModules;
                webpart.Module = Module;
                webpart.LegendSetting = txtSlaTolerance.Text.Trim();

                ListEditItem viewFilterItem = ddlViewFilter.SelectedItem;
                if (viewFilterItem != null)
                    webpart.FilterView = viewFilterItem.Text;

                if (list != null && list.SelectedValues.Count > 0)
                {
                    List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                    List<string> lstSelectedModule = new List<string>();
                    foreach (ListEditItem item in moduleList)
                    {
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }

                    webpart.StringOfSelectedModule = string.Join(";", lstSelectedModule);
                }

                dashboardSlaDockPanelSetting = webpart;
            }
        }

        private void BindModule(object ctr)
        {
            if (ctr != null && ctr is ASPxListBox)
            {
                list = ctr as ASPxListBox;
                if (list != null && list.Items.Count <= 0)
                {
                    ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                    SlaRulesManager slaRulesManager = new SlaRulesManager(context);

                    DataTable dtModule = moduleViewManager.LoadAllModules();
                    DataTable dtSLARule = slaRulesManager.GetDataTable();
                    
                    if (dtSLARule != null && dtSLARule.Rows.Count > 0)
                    {
                        dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                        dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule });
                        DataRow[] moduleRows = dtModule.Select(string.Format("{0}='True'", DatabaseObjects.Columns.EnableModule));
                        DataRow[] dr = null;
                        foreach (DataRow moduleRow in moduleRows)
                        {
                            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
                            dr = dtSLARule.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
                            
                            if (dr != null && dr.Length > 0)
                            {
                                list.Items.Add(new ListEditItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleName });
                            }

                            if (list.Items.IndexOf(list.Items.FindByValue(ModuleNames.SVC)) == -1 && moduleName == ModuleNames.SVC)
                                list.Items.Add(new ListEditItem(Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), ModuleNames.SVC));
                        }

                        if (list.Items.Count > 0)
                        {
                            List<string> lstOfModules = list.Items.Cast<ListEditItem>().Select(x => x.Text).ToList();
                            SLAEnableModules = string.Join(Constants.Separator5, lstOfModules);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dashboardSlaDockPanelSetting.StringOfSelectedModule))
                {
                    string[] arrOfSplitModules = dashboardSlaDockPanelSetting.StringOfSelectedModule.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrOfSplitModules)
                    {
                        string[] arrKeyVal = str.Split('(');
                        string keyVal = arrKeyVal[0];
                        string val = arrKeyVal[1].Replace(')', ' ').Trim();
                        ListEditItem editItem = list.Items.FindByValue(val);
                        
                        if (editItem != null)
                            editItem.Selected = true;
                    }
                }
            }
        }

        private void BindPeriodFilter(ASPxComboBox ddl)
        {
            if (ddl.Items.Count <= 0)
            {
                List<string> dateViews = DashboardCache.GetDateViewList();
                foreach (string item in dateViews)
                {
                    ListEditItem itm = new ListEditItem(item, item);
                    ddl.Items.Add(new ListEditItem(item, item));
                }

                ddl.Items.Insert(0, new ListEditItem("All Requests", string.Empty));
            }
        }

        protected void ddlViewFilter_Init(object sender, EventArgs e)
        {
            BindPeriodFilter(ddlViewFilter);            
        }

        protected void ddeModules_Init(object sender, EventArgs e)
        {
            ASPxDropDownEdit dropdownedit = sender as ASPxDropDownEdit;
            list = dropdownedit.FindControl("listBox") as ASPxListBox;
            BindModule(list);
        }
    }
}