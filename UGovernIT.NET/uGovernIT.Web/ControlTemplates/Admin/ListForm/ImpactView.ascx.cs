
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ImpactView : UserControl
    {
        /// <summary>
        /// Set mode to view page (impact, severity, priority)
        /// </summary>
        public ViewMode Mode { private get; set; }
        private string ListName = string.Empty;
        private string ColumnName = string.Empty;
        DataTable splTickets;
        DataTable dtTickets;
        string moduleName = string.Empty;
        string addNewItem = string.Empty;

        public string ModuleName
        {
            get
            {
                if (Session["ModuleName"] != null)
                    return Convert.ToString(Session["ModuleName"]);
                else
                {
                    return "";
                }
            }
            set
            {
                Session["ModuleName"] = value;
            }
        }

        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&mode={1}&ItemID={2}&moduleName={3}";
        private string absoluteUrlView = "/layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}&mode={4}";
        private string formTitle = string.Empty;
        private string viewParam = "ticketimpact";
        private string newParam = "ticketimpactnew";
        private string editParam = "ticketimpact";
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            formTitle = string.Format("Ticket {0}", Mode.ToString());
            if (string.IsNullOrEmpty(moduleName))
            {
                moduleName = ModuleNames.ACR;
                ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), ModuleNames.ACR)));
            }
            if(Session["ModuleName"]!=null)
            {
                moduleName = Convert.ToString(Session["ModuleName"]);
                ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
            }
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, Mode, 0, moduleName));
            string addNewItemURL = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','390',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
            aAddItem.ClientSideEvents.Click = "function(){ " + addNewItemURL + " }";                // aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','200',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string aAddItem_TopURL = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','390',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
            // aAddItem_Top.Attributes.Add(string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','200',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.ClientSideEvents.Click = "function(){ " + aAddItem_TopURL + " }";
            switch (Mode)
            {
                case ViewMode.Impact:
                    ListName = DatabaseObjects.Tables.TicketImpact;
                    ColumnName = DatabaseObjects.Columns.Impact;
                    ImpactManager impactMGR = new ImpactManager(HttpContext.Current.GetManagerContext());
                    splTickets = UGITUtility.ToDataTable<ModuleImpact>(impactMGR.Load());
                    break;
                case ViewMode.Severity:
                    ListName = DatabaseObjects.Tables.TicketSeverity;
                    ColumnName = DatabaseObjects.Columns.Severity;
                    SeverityManager severityMGR = new SeverityManager(HttpContext.Current.GetManagerContext());
                    splTickets = UGITUtility.ToDataTable<ModuleSeverity>(severityMGR.Load());
                    break;
                case ViewMode.Priority:
                    ListName = DatabaseObjects.Tables.TicketPriority;
                    ColumnName = DatabaseObjects.Columns.Priority;
                    PrioirtyViewManager priorityMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
                    splTickets = UGITUtility.ToDataTable<ModulePrioirty>(priorityMGR.Load());
                    break;
                default:
                    break;
            }
            hdnMode.Value = Mode.ToString();
            FillModuleDropDown(ddlModule);
            //splTickets = SPListHelper.GetSPList(ListName);
            base.OnInit(e);
        }
       
        protected override void OnLoad(EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                if (Request["module"] != null)
                {
                    moduleName = Request["module"].ToString();
                    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }
                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    chkShowDeleted.Checked = showdelete == "0" ? false : true;
                }
               
            }

            BindGridView(ddlModule.GetValues());

            base.OnLoad(e);
        }

        private void BindGridView(string SelectedModule)
        {
            dtTickets = splTickets;
            DataRow[] dr;
            if (!string.IsNullOrEmpty(SelectedModule))
            {
                string selectedModuleName = ObjModuleViewManager.LoadByID(Convert.ToInt64(SelectedModule)).ModuleName;
                if (!chkShowDeleted.Checked)
                {
                    dr = dtTickets.Select(string.Format("{0}='{1}' and ({2}='False' or {2} = '' or {2} IS NULL)", DatabaseObjects.Columns.ModuleNameLookup, selectedModuleName,
                        DatabaseObjects.Columns.Deleted));
                }
                else
                {
                    dr = dtTickets.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, selectedModuleName)); 
                }


                if (dr.Length > 0)
                {
                    DataTable temp = dr.CopyToDataTable();
                    temp.Columns[ColumnName].ColumnName = "MyTitle";
                    if (temp.Columns.Contains(DatabaseObjects.Columns.ItemOrder))
                    {
                        temp.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                    }
                    _gridImpact.DataSource = temp;                
                }
                else
                {
                    _gridImpact.DataSource = null;
                }
            }
            else
            {
                _gridImpact.DataSource = null;                
            }
            _gridImpact.DataBind();
            //_gridImpact.HeaderRow.Cells[1].Text = Mode.ToString();
        }

        private void FillModuleDropDown(LookUpValueBox dropDown)
        {
            dropDown.devexListBox.AutoPostBack = false;
            dropDown.devexListBox.ClientSideEvents.ValueChanged = "function(s, e){ changeRequestType(); }";
        }
        protected void _gridImpact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            //{
            //    string lsDataKeyValue = _gridImpact.DataKeys[e.Row.RowIndex].Value.ToString();
            //    bool IsDeleted = Convert.ToBoolean(SPListHelper.GetSPListItem(splTickets, Convert.ToInt32(lsDataKeyValue))[DatabaseObjects.Columns.IsDeleted]);
            //    if (IsDeleted)
            //    {
            //        e.Row.BackColor = System.Drawing.Color.FromArgb(165, 52, 33);
            //        foreach (TableCell item in e.Row.Cells)
            //        {
            //            item.Style.Add("color", "#FFF");
            //        }
            //    }
            //    string editItem;
            //    editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit,this.editParam, Mode.ToString(), lsDataKeyValue));
            //    HtmlAnchor anchorEdit = (HtmlAnchor)e.Row.FindControl("aEdit");
            //    HtmlAnchor aImpact = (HtmlAnchor)e.Row.FindControl("aImpact");
            //    HiddenField hiddenImpact = (HiddenField)e.Row.FindControl("hiddenImpact");
            //    HtmlAnchor lnkShowdetail = (HtmlAnchor)e.Row.FindControl("aKeyname");
            //    string Title = hiddenImpact.Value;
            //    string jsFunc =string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','200',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle);
            //    anchorEdit.Attributes.Add("href", jsFunc);
            //    aImpact.Attributes.Add("href", jsFunc);
            //    aImpact.InnerText = Title;
            //    if (IsDeleted)
            //        aImpact.Style.Add("color", "#FFF");
            //}
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            moduleName = ddlModule.GetValues();
            BindGridView(moduleName);
            string showdelete = chkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, ModuleName/* uGITCache.ModuleConfigCache.getModuleName(Convert.ToInt64(moduleName))*/, showdelete, Mode.ToString()));
            Response.Redirect(url);  
        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            moduleName = ddlModule.GetValues();
            string showdelete = chkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, ObjModuleViewManager.LoadByID(Convert.ToInt64(moduleName)).ModuleName, showdelete, Mode.ToString()));
            Response.Redirect(url);  
        }

        protected void _gridImpact_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (ddlModule.devexListBox.Value != null)
            {
                int selectedModule = Convert.ToInt32(ddlModule.devexListBox.Value);
                Session["ModuleName"] = ObjModuleViewManager.LoadByID(selectedModule).ModuleName;
                BindGridView(Convert.ToString(selectedModule));
            }
        }

        protected void _gridImpact_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);

            }
        }

        //protected void _gridImpact_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    //{
        //    //    string lsDataKeyValue = Convert.ToString(e.KeyValue);
        //    //    string editItem;
        //    //    editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, Mode.ToString(), lsDataKeyValue, moduleName));
        //    //    HtmlAnchor anchorEdit = _gridImpact.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as HtmlAnchor;
        //    //    string Title = Convert.ToString(e.GetValue("Title"));
        //    //    string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','200',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle);
        //    //    if (anchorEdit != null)
        //    //    {
        //    //        anchorEdit.Attributes.Add("href", jsFunc);
        //    //    }
        //    //    //LinkButton lnkDelete = grdApplModules.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkDelete") as LinkButton;
        //    //    //if (lnkDelete != null)
        //    //    //{
        //    //    //    lnkDelete.CommandArgument = lsDataKeyValue;
        //    //    //    lnkDelete.Visible = isUserExists;
        //    //    //}
        //    //}
        //}

        
        protected void _gridImpact_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue("Title"));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, Mode.ToString(), dataKeyValue, moduleName));
                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','390',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)_gridImpact.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", jsFunc);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }

        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(moduleName, false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }

            string cacheName = "Lookup_" + ListName + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(ListName, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
