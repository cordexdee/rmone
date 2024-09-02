
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using System.Text;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Web;
using uGovernIT.DAL;
using System.Web;
using uGovernIT.Util.Cache;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ModuleColumnsView : UserControl
    {
        string addNewItem;
        //DataRow[] moduleColumns = new DataRow[0];
        //ModuleColumn moduleColumnsSequence;
        FormLayoutManager formLayoutManager;
        ModuleColumnManager moduleColumnManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        List<ModuleColumn> lstModuleColumns;
        ModuleViewManager ObjModuleManager;
        ConfigurationVariableManager ConfigManager = null;
        protected override void OnInit(EventArgs e)
        {
            ObjModuleManager = new ModuleViewManager(context);
            formLayoutManager = new FormLayoutManager(context);
            moduleColumnManager = new ModuleColumnManager(context);
            ConfigManager = new ConfigurationVariableManager(context);

            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";

            BindModule();

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["module"] != null)
                {
                    string module = Convert.ToString(Request["module"]);
                    ddlModule.SelectedValue = module;
                }
            }
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=modulecolumnsaddedit&module={0}&moduleType={1}", ddlModule.SelectedValue, rdnModuleClassification.SelectedIndex));
            // addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsaddedit&module={0}&moduleType={1}", ddlModule.SelectedValue, rdnModuleClassification.SelectedIndex);
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','700','700',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','700','700',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));

            if (Request.Form["__CALLBACKPARAM"] != null && Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
            {
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGROW"))
                {
                    if (val.Length > 1)
                        UpdateSequence(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty));
                }
            }

            ShowHideGridColumns();
            BindGridView();
            base.OnLoad(e); 
        }

        void UpdateSequence(string firstID, string secondID)
        {

            #region Convert sharepoint code to .Net version
            lstModuleColumns = moduleColumnManager.Load(x => x.CategoryName.Equals(ddlModule.SelectedValue));
            if (lstModuleColumns != null && lstModuleColumns.Count > 0)
                lstModuleColumns = lstModuleColumns.OrderBy(x => x.FieldSequence).ToList();

            
            if (lstModuleColumns == null || lstModuleColumns.Count == 0)
                return;

          
            ModuleColumn drFirstItem = lstModuleColumns.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(firstID.Trim()));
            
            ModuleColumn drSecondItem = null;
            if (UGITUtility.StringToInt(secondID.Trim()) == 0)
            {
                double lastSequence = lstModuleColumns.Max(x => x.FieldSequence);
                drSecondItem = lstModuleColumns.FirstOrDefault(x => x.FieldSequence == lastSequence); 
            }
            else
                drSecondItem = lstModuleColumns.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(secondID.Trim()));

            if (drFirstItem == null || drSecondItem == null)
                return;
            int firstSeq = drFirstItem.FieldSequence;
            int secondSeq = drSecondItem.FieldSequence;

            foreach (ModuleColumn dr in lstModuleColumns)
            {
                if (firstSeq > secondSeq)
                {

                    if (firstSeq > UGITUtility.StringToInt(dr.FieldSequence) && UGITUtility.StringToInt(dr.FieldSequence) >= secondSeq)
                    {
                        dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) + 1;
                    }
                }
                else
                {
                    if (secondSeq >= UGITUtility.StringToInt(dr.FieldSequence) && UGITUtility.StringToInt(dr.FieldSequence) > firstSeq)
                    {
                        dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) - 1;
                    }
                }
            }

            drFirstItem.FieldSequence = secondSeq;
            moduleColumnManager.UpdateItems(lstModuleColumns);

            lstModuleColumns = moduleColumnManager.Load(x => x.CategoryName.Equals(ddlModule.SelectedValue)).OrderBy(x=>x.FieldSequence).ToList();
            int counter = 0;
            foreach (ModuleColumn dr in lstModuleColumns)
            {
                dr.FieldSequence = ++counter;
                moduleColumnManager.Update(dr);
            }
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Request list items Reordered : {drFirstItem.FieldName} - {drSecondItem.FieldName} ({drFirstItem.CategoryName})", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            #endregion

            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void BindGridView()
        {
            List<string> tabs = new List<string>();

            // Get 'Waiting On Me' Tab name from configuration variable
            //string waitingOnMeTabName = ConfigManager.GetValue(ConfigConstants.WaitingOnMeTabName);

            //if (string.IsNullOrEmpty(waitingOnMeTabName))
            //    waitingOnMeTabName = "Waiting on Me";

            //Dictionary<string, string> dictTabs = UGITUtility.GetTabNames();

            //dictTabs["waitingonme"] = waitingOnMeTabName;
            //dictTabs.Add("all", "All");
            Dictionary<string, string> dictTabs = new Dictionary<string, string>();
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup == ddlModule.SelectedItem.Value).GroupBy(t => t.TabName).Select(g => g.First()).ToList();
            foreach (var item in tabView)
            {
                dictTabs.Add(item.TabName, item.TabDisplayName);
            }
            dictTabs.Add("all", "All");
            lstModuleColumns = moduleColumnManager.Load($"{DatabaseObjects.Columns.CategoryName}='{ddlModule.SelectedValue}'").OrderBy(x => x.FieldSequence).ToList();
            var val = new[] { "", "None",null};
            lstModuleColumns.Where(w => val.Contains(w.TextAlignment)).ToList().Select(w => w.TextAlignment = "Center").ToList();
            lstModuleColumns.ForEach( x => { 
                if (!string.IsNullOrEmpty(x.SelectedTabs))
                {
                    tabs.Clear();
                    tabs = x.SelectedTabs.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < tabs.Count; i++)
                    {
                        tabs[i] = dictTabs[tabs[i]];
                    }
                    x.SelectedTabs = string.Join(", ", tabs);
                }
            });

            if (lstModuleColumns != null && lstModuleColumns.Count > 0)
            {
                grid.DataSource = lstModuleColumns;
                grid.DataBind();
            }
        }

        private void BindModule()
        {           
            ddlModule.Items.Clear();
            string[] defaultView = new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule };
            bool enableFilter = false;

            if (rdnModuleClassification.SelectedIndex == 1 || (Request["moduleType"] != null && UGITUtility.StringToInt(Request["moduleType"]) == 1))
            {
                enableFilter = true;
                rdnModuleClassification.SelectedIndex = 1;
                defaultView = new string[] { DatabaseObjects.Columns.CategoryName };
            }
            List<UGITModule> spModuleList = ObjModuleManager.Load().OrderBy(x => x.ModuleName).ToList();

            if (spModuleList != null && spModuleList.Count > 0)
            {
                var spNonModuleList = moduleColumnManager.Load().Select(x => x.CategoryName);
                if (!enableFilter)
                {
                    ddlModule.DataSource = spModuleList.Where(x => x.EnableModule == true).ToList();
                    ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                    ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                }
                else
                {
                    List<string> listModule = spModuleList.Select(x => x.ModuleName).ToList();
                    List<string> listAllModule = spNonModuleList.Select(x => Convert.ToString(x)).Distinct().ToList();
                    List<string> listNonModule = listAllModule.Except(listModule).ToList();
                    listNonModule.ForEach(x =>
                    {
                        ddlModule.Items.Add(new ListItem { Text = x, Value = x });
                    });
                }
                ddlModule.DataBind();
            }

        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ModuleColumnsView&pageTitle=Module Columns&isdlg=1&isudlg=1&module=" + moduleName);
            if (rdnModuleClassification.SelectedIndex == 1)
                url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ModuleColumnsView&pageTitle=Module Columns&isdlg=1&isudlg=1&module={0}&moduleType={1}", moduleName, rdnModuleClassification.SelectedIndex));
            Response.Redirect(url);

        }

        protected void aEdit_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            // string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsaddedit&ID=" + lsDataKeyValue + " &module=" + ddlModule.SelectedValue);
            string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=modulecolumnsaddedit&ItemID={0}&module={1}&moduleType={2}", lsDataKeyValue, ddlModule.SelectedValue, rdnModuleClassification.SelectedIndex));
            string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','700','700',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), "Edit Item");
            aHtml.Attributes.Add("href", Url);
        }

        protected void flTabControl_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }

        protected void aDisplayName_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            //string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsaddedit&ID=" + lsDataKeyValue + " &module=" + ddlModule.SelectedValue);
            string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=modulecolumnsaddedit&ItemID={0}&module={1}&moduleType={2}", lsDataKeyValue, ddlModule.SelectedValue, rdnModuleClassification.SelectedIndex));
            string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','700','700',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), "Edit Item");
            aHtml.Attributes.Add("href", Url);
            aHtml.InnerText = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text.ToString();
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    GridViewTableDataCell editCell = (GridViewTableDataCell)cell;

                    if (((GridViewDataColumn)editCell.Column).FieldName == DatabaseObjects.Columns.IsAscending)
                    {
                        if (editCell.Controls[0] is LiteralControl)
                        {
                            var control = editCell.Controls[0] as LiteralControl;
                            if (control.Text == "True")
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).VisibleIndex].Text = "&#x25B2;"; // "&uarr;";
                            }
                            else if (control.Text == "False")
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).VisibleIndex].Text = "&#x25BC;"; // "&darr;";
                            }
                        }
                    }
                    else if (((GridViewDataColumn)editCell.Column).PropertiesEdit.DisplayFormatString == "Yes;No")
                    {
                        if (editCell.Controls[0] is LiteralControl)
                        {
                            var control = editCell.Controls[0] as LiteralControl;
                            if (control.Text == "True")
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).VisibleIndex].Text = "Yes";
                            }
                            else
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).VisibleIndex].Text = "No";
                            }
                        }
                    }
                }
            }
        }
        protected void rdnModuleClassification_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ModuleColumnsView&pageTitle=Module Columns&isdlg=1&isudlg=1&moduleType={0}", rdnModuleClassification.SelectedIndex));
            Response.Redirect(url);
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            UGITModule module = ObjModuleManager.LoadByName(ddlModule.SelectedValue, false);
            if (module != null)
            {
                //module.List_ModuleColumns = moduleColumnManager.Load().Where(x => x.CategoryName == ddlModule.SelectedValue).OrderBy(x => x.FieldSequence).ToList();
                CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
                TicketManager ticketManager = new TicketManager(context);
                ticketManager.RefreshCacheModuleWise(module);
                CacheStatisticHelper.UpdateStat(CacheStatisticConstants.TICKETCACHEUPDATEON, context.TenantID, DateTime.UtcNow);
            }
            DataTable modulecolumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            CacheHelper<object>.AddOrUpdate($"ModuleColumns{context.TenantID}", context.TenantID, modulecolumns);
            CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{context.TenantID}", moduleColumnManager.Load());
            BindGridView();
        }
        /// <summary>
        /// This Method is used to show/hide columns for Module and Non-Module data
        /// </summary>
        protected void ShowHideGridColumns()
        {
            bool isModuleData = rdnModuleClassification.SelectedIndex == 0;

            grid.Columns[DatabaseObjects.Columns.IsDisplay].Visible = grid.Columns[DatabaseObjects.Columns.DisplayForClosed].Visible = !isModuleData;
            grid.Columns[DatabaseObjects.Columns.SelectedTabs].Visible = grid.Columns[DatabaseObjects.Columns.IsUseInWildCard].Visible = isModuleData;
        }
       
    }
}
