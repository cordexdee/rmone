
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class FormLayoutAndPermission : UserControl
    {
        protected string addNewItemUrl;

        List<ModuleFormLayout> formLayout ;
        //private int currentTab = 0;
        /// <summary>
        /// Configuration variable for stage to production
        /// </summary>
        /// <param name="e"></param>    
        #region Stage to Production
        // Destination site credentials, leave empty if using integrated authentication
        protected string moveToProductionUrl = "";        
        //ModuleFormLayout moduleFormLayout;
        ModuleFormTab moduleFormTab;
        #endregion
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager ObjConfigurationVariableHelper = null;
        ModuleFormLayoutManager ObjModuleFormLayoutManager = null;
        FormLayoutManager ObjFormLayoutManager = null;
        ModuleViewManager ObjModuleViewManager = null;
        RequestRoleWriteAccessManager objRequestRoleWriteAccessManager = null;
        ModuleFormTabManager ObjModuleFormTabManager = null;
        
        protected override void OnInit(EventArgs e)
        {
            ObjConfigurationVariableHelper = new ConfigurationVariableManager(context);
            ObjModuleFormLayoutManager = new ModuleFormLayoutManager(context);
            ObjFormLayoutManager = new FormLayoutManager(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            objRequestRoleWriteAccessManager = new RequestRoleWriteAccessManager(context);
            ObjModuleFormTabManager = new ModuleFormTabManager(context);

            BindModule();
            EnableMigrate();
            grid.Styles.AlternatingRow.CssClass = "";
            grid.Styles.AlternatingRow.BackColor = Color.White;
            base.OnInit(e);
        }     
        
        private void EnableMigrate()
        {
            if (ObjConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                btnMoveStageToProduction.Visible = true;
            }
        }

        private int GetTabIndexById(string tabID)
        {
            int tabIndex = 0;

            foreach (Tab t in flTabControl.Tabs)
            {
                if (t.Name.Trim() == tabID)
                {
                    tabIndex = t.Index;
                }
            }

            return tabIndex;
        }

        void FillTabs()
        {
            if (flTabControl.Tabs.Count > 0)
                flTabControl.Tabs.Clear();   
            
            List<ModuleFormTab> listTabs = ObjFormLayoutManager.Load(x=>x.ModuleNameLookup== ddlModule.SelectedValue).OrderBy(y=>y.TabSequence).ToList();
            if (listTabs != null && listTabs.Count > 0)
            {
                foreach (ModuleFormTab dr in listTabs)
                {
                    DevExpress.Web.Tab tb = new DevExpress.Web.Tab();
                    tb.Name = Convert.ToString(dr.TabId);
                    tb.Text = Convert.ToString(dr.TabName);
                    flTabControl.Tabs.Add(tb);
                }
            }

            DevExpress.Web.Tab tbNew = new DevExpress.Web.Tab();
            tbNew.Name = "";
            tbNew.Text = "";
            tbNew.ClientEnabled = false;
            flTabControl.Tabs.Add(tbNew);

            DevExpress.Web.Tab tbNewForm = new DevExpress.Web.Tab();
            tbNewForm.Name = "0";
            tbNewForm.Text = "New";
            flTabControl.Tabs.Insert(0, tbNewForm);
        }
        
        protected override void OnLoad(EventArgs e)
        {
            grid.Styles.AlternatingRow.CssClass = "";
            if (!IsPostBack)
            {
                if (Request["module"] != null)
                {
                    string module = Convert.ToString(Request["module"]);
                    ddlModule.SelectedValue = module;
                }
            }

             FillTabs();
             if (!IsPostBack)
             {
                 hndCurrentTab.Value = flTabControl.Tabs[0].Name;
                 flTabControl.ActiveTabIndex = GetTabIndexById(hndCurrentTab.Value);
             }
            object cacheVal = Context.Cache.Get(string.Format("CURRENTTABINTEX-{0}", context.CurrentUser.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("CURRENTTABINTEX-{0}", context.CurrentUser.Id));
                flTabControl.ActiveTabIndex = GetTabIndexById(Convert.ToString(cacheVal));
                hndCurrentTab.Value = cacheVal.ToString();
            }


            if (Request.Form["__CALLBACKPARAM"] != null)
             {
                 string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                 if (Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                 {

                     if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGROW"))
                     {

                         if (val.Length > 1)
                         {
                            ObjModuleFormLayoutManager.UpdateSequence(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty), ddlModule.SelectedValue, hndCurrentTab.Value);
                         }
                     }
                     else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DROPONTAB"))
                     {
                         UpdateSequenceOnDifferentTab(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty));
                     }

                 }
                 else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGTAB"))
                 {
                     UpdateTabs(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty));
                     FillTabs();
                     flTabControl.ActiveTabIndex = GetTabIndexById(hndCurrentTab.Value);
                 }
                 else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("RENAMETAB"))
                 {
                     RenameTab(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty));
                     FillTabs();
                     flTabControl.ActiveTabIndex = GetTabIndexById(hndCurrentTab.Value);
                 }
                 else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DELETETAB"))
                 {
                     DeleteTab(val[val.Length - 1].Replace(";", string.Empty));
                     FillTabs();
                     flTabControl.ActiveTabIndex = GetTabIndexById(hndCurrentTab.Value);
                 }
                 else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("ADDTAB"))
                 {
                     AddTab(val[val.Length - 1].Replace(";", string.Empty));
                     FillTabs();
                     flTabControl.ActiveTabIndex = GetTabIndexById(hndCurrentTab.Value);
                 }
             }

            //string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=movestagetoproduction&module={0}&list={1}");

            //moveToProductionUrl = string.Format(url, ddlModule.SelectedValue, DatabaseObjects.Tables.FormLayout);

            addNewItemUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermissionaddedit&module=" + ddlModule.SelectedValue); // CreateNewItemUrl()          
            BindGridView();
            base.OnLoad(e);
        }
       
        void DeleteTab(string ID)
        {
            ModuleFormTab moduleFormTab = ObjModuleFormTabManager.Load().FirstOrDefault(x=>x.TabId== UGITUtility.StringToInt(ID) && x.ModuleNameLookup==ddlModule.SelectedValue);
            try
            {
                if (moduleFormTab != null)
                {                  
                    ObjFormLayoutManager.DeleteTab(moduleFormTab);
                }                
                // Delete fields
                formLayout = ObjModuleFormLayoutManager.Load(x=>x.ModuleNameLookup==ddlModule.SelectedValue && x.TabId==Convert.ToInt64(ID));        
                // Delete permission
                string fieldNames = formLayout.Count == 0 ? string.Empty : string.Join(",", formLayout.Select(x => string.Format("'{0}'", x.FieldName)).ToArray());
                var result = objRequestRoleWriteAccessManager.Load(x => x.FieldName.Contains(fieldNames) && x.ModuleNameLookup== ddlModule.SelectedValue);
                List<ModuleRoleWriteAccess> moduleRequestWriteAccessList = result;               
                if (moduleRequestWriteAccessList!=null && moduleRequestWriteAccessList.Count > 0)
                {
                    ObjFormLayoutManager.DeleteRequestWriteAccess(moduleRequestWriteAccessList);
                }
                if(formLayout != null && formLayout.Count > 0)
                {
                    ObjFormLayoutManager.DeleteFieldFromTab(formLayout);
                }
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Delete Formlayout", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        void AddTab(string name)
        {
            int seq = 1;
            List<ModuleFormTab> dtTab = ObjModuleFormTabManager.Load(x=>x.ModuleNameLookup==ddlModule.SelectedValue).OrderByDescending(x=>x.TabId).ToList();//  GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleFormTab);
             if (dtTab != null && dtTab.Count > 0)
            {
                seq = UGITUtility.StringToInt(dtTab[0].TabId) + 1;
            }
            ModuleFormTab tab = new ModuleFormTab();
            tab.TabName = name;
            tab.TabId = seq;
            tab.TabSequence = seq;
            tab.ModuleNameLookup = ddlModule.SelectedValue;
            tab.Title = $"{ddlModule.SelectedValue} - {name}";
            tab.ShowInMobile = chkShowinMobile.Checked;
            tab.AuthorizedToView = Convert.ToString(hndAuthorizedToView.Value); //authorizedToView.GetValues();
            ObjFormLayoutManager.AddTab(tab);
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Add tab: {tab.TabName}; Module: {tab.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
        }

        void RenameTab(string ID, string Text)
        {
            int Id = Convert.ToInt32(ID);
            ModuleFormTab dr = ObjModuleFormTabManager.Load().FirstOrDefault(x=>x.TabId==Convert.ToInt64(ID) && x.ModuleNameLookup==ddlModule.SelectedValue);
            try
            {
                if (dr != null)
                {
                    ModuleFormTab tab = new ModuleFormTab();
                    tab.TabName = Text;
                    tab.ID = UGITUtility.StringToLong(dr.ID);
                    tab.TabId = UGITUtility.StringToInt(dr.TabId);
                    //tab.TabSequence = UGITUtility.StringToInt(dr.TabSequence);
                    tab.TabSequence = UGITUtility.StringToInt(txtOrder.Text);
                    tab.Title = dr.Title;
                    tab.ModuleNameLookup = ddlModule.SelectedValue;
                    tab.ShowInMobile = chkShowinMobile.Checked;
                    tab.Title = $"{ddlModule.SelectedValue} - {Text}";
                    tab.AuthorizedToView = Convert.ToString(hndAuthorizedToView.Value); //authorizedToView.GetValues();
                    ObjFormLayoutManager.UpdateTab(tab);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Update tab: {tab.TabName}; Module: {tab.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                    
                    List<ModuleFormTab> moduleFormTab = ObjModuleFormTabManager.Load(x => x.ModuleNameLookup == ddlModule.SelectedValue);
                    if (moduleFormTab != null && moduleFormTab.Count > 0)
                    {
                        moduleFormTab = moduleFormTab.OrderBy(x => x.TabSequence).ToList();
                        int sequence = 1;
                        foreach (ModuleFormTab mCol in moduleFormTab)
                        {
                            mCol.TabSequence = sequence;
                            ObjModuleFormTabManager.Update(mCol);
                            sequence++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        void UpdateSequenceOnDifferentTab(string firstID, string tabID)
        {
            List<ModuleFormLayout> dtFormLayout = ObjModuleFormLayoutManager.Load();
            List<ModuleFormLayout> drFormLayout = dtFormLayout.Where(x=>x.ModuleNameLookup==ddlModule.SelectedValue && x.TabId==x.TabId).OrderBy(x=>x.FieldSequence).ToList();

            ModuleFormLayout drFirstItem = drFormLayout.Where(m => m.ID == UGITUtility.StringToLong(firstID.Trim())).FirstOrDefault();
            int firstSeq = UGITUtility.StringToInt(drFirstItem.FieldSequence);
            List<ModuleFormLayout> drItems = dtFormLayout.Where(x => x.ModuleNameLookup ==ddlModule.SelectedValue && x.TabId == Convert.ToInt32(hndCurrentTab.Value)).ToList(); ;
            var maxVal = 0.0;
            if (drItems != null && drItems.Count > 0)
            {
                maxVal = drItems
                       .AsEnumerable()
                       .Max(r => r.FieldSequence);
            }

            int groupEnd = firstSeq;

            // if group or table field dropped.
            if (Convert.ToString(drFirstItem.FieldName) == "#GroupStart#" || Convert.ToString(drFirstItem.FieldName) == "#TableStart#")
            {
                foreach (ModuleFormLayout dr in dtFormLayout)
                {
                    if (UGITUtility.StringToInt(dr.FieldSequence) >= firstSeq
                                && Convert.ToString(dr.FieldName) == Convert.ToString(drFirstItem.FieldName).Replace("Start", "End"))
                    {
                        groupEnd = UGITUtility.StringToInt(dr.FieldSequence);
                        break;
                    }
                }
            }         
        }

        void UpdateTabs(string firstID, string secondID)
        {
            List<ModuleFormTab> drTabs = ObjModuleFormTabManager.Load().Where(x=>x.ModuleNameLookup== ddlModule.SelectedValue).ToList();
            ModuleFormTab drFirstItem = drTabs.Where(m => m.TabId == UGITUtility.StringToInt(firstID.Trim()) && m.ModuleNameLookup == ddlModule.SelectedValue).FirstOrDefault();
            ModuleFormTab drSecondItem = drTabs.Where(m => m.TabId == UGITUtility.StringToInt(secondID.Trim()) && m.ModuleNameLookup == ddlModule.SelectedValue).FirstOrDefault();
            int firstSeq = UGITUtility.StringToInt(drFirstItem.TabSequence);
            int secondSeq = UGITUtility.StringToInt(drSecondItem.TabSequence);


            foreach (ModuleFormTab dr in drTabs)
            {
                if (firstSeq > secondSeq)
                {

                    if (firstSeq > UGITUtility.StringToInt(dr.TabSequence) && UGITUtility.StringToInt(dr.TabSequence) >= secondSeq)
                    {
                        dr.TabSequence = UGITUtility.StringToInt(dr.TabSequence) + 1;
                    }
                }
                else
                {
                    if (secondSeq >= UGITUtility.StringToInt(dr.TabSequence) && UGITUtility.StringToInt(dr.TabSequence) > firstSeq)
                    {

                        dr.TabSequence = UGITUtility.StringToInt(dr.TabSequence) - 1;

                    }
                }
            }

            drFirstItem.TabSequence = secondSeq;
            List<ModuleFormTab> dataView = drTabs.OrderBy(x=>x.TabSequence).ToList(); 
            foreach (ModuleFormTab dr in dataView)
            {
                    moduleFormTab = new ModuleFormTab();
                    moduleFormTab.ID = Convert.ToInt64(dr.ID);
                    moduleFormTab.TabId = UGITUtility.StringToInt(dr.TabId);
                    moduleFormTab.TabSequence = UGITUtility.StringToInt(dr.TabSequence);
                    moduleFormTab.Title = Convert.ToString(dr.Title);
                    moduleFormTab.ModuleNameLookup = ddlModule.SelectedValue;
                    moduleFormTab.TabName = Convert.ToString(dr.TabName);
                    ObjFormLayoutManager.UpdateTab(moduleFormTab);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
        }

        private void BindGridView()
        {
            //Fetch all roles of selected module
            List<ModuleFormLayout> formLayout = ObjModuleFormLayoutManager.Load(x => x.ModuleNameLookup == ddlModule.SelectedValue && x.TabId == Convert.ToInt32(hndCurrentTab.Value)).OrderBy(y => y.FieldSequence).ToList() ; 
            if (formLayout == null)
                formLayout = new List<ModuleFormLayout>();
            grid.DataSource = formLayout;           
            grid.DataBind();
        }

        private void BindModule()
        {
            ddlModule.Items.Clear();
            List<UGITModule> moduleList=ObjModuleViewManager.Load(x=>x.EnableLayout && x.EnableModule).OrderBy(y=>y.ModuleName).ToList();
            if (moduleList != null && moduleList.Count > 0)
            {
                foreach (UGITModule moduleRow in moduleList)
                {
                    string moduleName = Convert.ToString(moduleRow.ModuleName);
                    ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow.Title), Value = moduleName });
                }
                ddlModule.DataBind();
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission&pageTitle=Form Layout&isdlg=1&isudlg=1&module=" + moduleName + "&currentTabIndex=" + hndCurrentTab.Value);
            Response.Redirect(url);
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                int sum = 0;
                for (int i = 0; i <= e.VisibleIndex; i++)
                {
                   
                    if (UGITUtility.StringToInt(grid.GetRowValues(i, DatabaseObjects.Columns.FieldDisplayWidth)) == 3 && sum % 3 > 0)
                    {
                        sum += 3 - (sum % 3);
                    }
                    sum += UGITUtility.StringToInt(grid.GetRowValues(i, DatabaseObjects.Columns.FieldDisplayWidth));
                }

                if (Math.Ceiling(((decimal)sum / 3)) % 2 == 0)
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                }

                if (e.Row.Cells.Count > 4)
                {
                    if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.FieldName)) == "#GroupStart#"
                        || Convert.ToString(e.GetValue(DatabaseObjects.Columns.FieldName)) == "#GroupEnd#"
                        || Convert.ToString(e.GetValue(DatabaseObjects.Columns.FieldName)) == "#TableStart#"
                        || Convert.ToString(e.GetValue(DatabaseObjects.Columns.FieldName)) == "#TableEnd#")
                    {
                        e.Row.Cells[4].Text = string.Empty;
                        e.Row.Cells[2].Style.Add("font-weight", "bold");
                        e.Row.Cells[3].Style.Add("font-weight", "bold");

                    }
                    else
                    {
                        for (int i = e.VisibleIndex; i >= 0; i--)
                        {
                            if (Convert.ToString(grid.GetRowValues(i, DatabaseObjects.Columns.FieldName)) == "#GroupStart#"
                                || Convert.ToString(grid.GetRowValues(i, DatabaseObjects.Columns.FieldName)) == "#TableStart#")
                            {
                                HtmlAnchor aDisplayName = grid.FindRowCellTemplateControl(e.VisibleIndex, null, "aDisplayName") as HtmlAnchor;
                                aDisplayName.Attributes.Add("style", "padding-left:20px");
                                e.Row.Cells[3].Text = string.Format("<span style='padding-left:20px'>{0}</span> ", e.GetValue(DatabaseObjects.Columns.FieldName));
                            
                                break;
                            }
                            else if (Convert.ToString(grid.GetRowValues(i, DatabaseObjects.Columns.FieldName)) == "#GroupEnd#"
                                || Convert.ToString(grid.GetRowValues(i, DatabaseObjects.Columns.FieldName)) == "#TableEnd#")
                            {
                                break;
                            }
                        }
                    }
                }
            }

        }

        protected void grid_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

        }

        protected void aEdit_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermissionaddedit&ID=" + lsDataKeyValue + " &module=" + ddlModule.SelectedValue + "&currentTabIndex=" + hndCurrentTab.Value);
            string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','800','700',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), "Edit Item");
            aHtml.Attributes.Add("href", Url);
        }

        protected void flTabControl_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }

        protected void aDisplayName_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermissionaddedit&ID=" + lsDataKeyValue + " &module=" + ddlModule.SelectedValue);
            string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','800','830',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), "Edit Item");
            aHtml.Attributes.Add("href", Url);
            aHtml.InnerText = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text.ToString();
        }

        protected void aPermission_Load(object sender, EventArgs e)
        {

            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            // || Convert.ToString(dr.FieldName]) == "#Control#"
            List<ModuleFormLayout> formLayouts = grid.DataSource as List<ModuleFormLayout>;
            ModuleFormLayout dr = formLayouts.Where(x => x.ID == Convert.ToInt64(lsDataKeyValue)).FirstOrDefault();
            if (Convert.ToString(dr.FieldName) == "#GroupStart#"
                || Convert.ToString(dr.FieldName) == "#GroupEnd#"
                || Convert.ToString(dr.FieldName) == "#TableStart#"
                || Convert.ToString(dr.FieldName) == "#TableEnd#"
                || Convert.ToString(dr.FieldName) == "#Label#"
                || Convert.ToString(dr.FieldName) == "#PlaceHolder#"
                )
            {
                aHtml.Visible = false;
            }
            else
            {
                string title = string.Format("{0} ({1})", dr.FieldDisplayName, dr.FieldName);
                title = UGITUtility.ReplaceInvalidCharsInURL(title);
                string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=requestrolewriteaccess&ID=" + lsDataKeyValue + " &module=" + ddlModule.SelectedValue);
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','1200','600',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Uri.EscapeDataString(title));
                aHtml.Attributes.Add("href", Url);
            }

        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(ddlModule.SelectedValue, false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }

        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGridView();
        }

        protected void aspxPopupFormTab_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (Convert.ToInt32(e.Parameter) > 0)
            {
                ModuleFormTab moduleFormTab = ObjModuleFormTabManager.Load(x => x.TabId == Convert.ToInt32(e.Parameter) && x.ModuleNameLookup == ddlModule.SelectedValue).FirstOrDefault();

                if (moduleFormTab != null)
                {
                    txtTabName.Text = Convert.ToString(moduleFormTab.TabName);
                    txtOrder.Text = Convert.ToString(moduleFormTab.TabSequence);
                    chkShowinMobile.Checked = moduleFormTab.ShowInMobile.HasValue ? moduleFormTab.ShowInMobile.Value : false;
					authorizedToView.SetValues(moduleFormTab.AuthorizedToView);
                }
            }
            else
            {
                txtTabName.Text = "";
                txtOrder.Text = "";
                chkShowinMobile.Checked = false;
                authorizedToView.SetValues("");
            }            
        }
    }
}
