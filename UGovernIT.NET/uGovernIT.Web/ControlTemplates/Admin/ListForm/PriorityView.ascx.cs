using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class PriorityView : UserControl
    {
        /// <summary>
        /// Set mode to view page (impact, severity, priority)
        /// </summary>
        private string ListName = string.Empty;
        private string ColumnName = string.Empty;
        string moduleName = string.Empty;
        string addNewItem = string.Empty;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&ID={1}&moduleName={2}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}";
        private string formTitle = string.Empty;
        private string viewParam = "ticketpriority";
        private string newParam = "ticketprioritynew";
        private string editParam = "ticketpriorityedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            formTitle = string.Format("Ticket Priority");

            if (string.IsNullOrEmpty(moduleName))
            {
                moduleName = ModuleNames.ACR;
                dxddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), ModuleNames.ACR)));
            }
            if (Session["ModuleName"] != null)
            {
                moduleName = Convert.ToString(Session["ModuleName"]);
                dxddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
            }


            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, 0, moduleName));
            string addNewItemURL = (string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, "/Layouts/uGovernIT/uGovernITConfiguration/", formTitle));
            aAddItem.ClientSideEvents.Click = "function(){ " + addNewItemURL + " }";
            string aAddItem_TopURL = (string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, "/Layouts/uGovernIT/uGovernITConfiguration/", formTitle));
            aAddItem_Top.ClientSideEvents.Click = "function(){ " + aAddItem_TopURL + " }";
            dxddlModule.Paging = false;
            FillModuleDropDown(dxddlModule);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request["module"] != null)
                {
                    moduleName = Request["module"].ToString();
                    dxddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }

                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    dxshowdeleted.Checked = showdelete == "0" ? false : true;
                }
            }
            BindGridView(Convert.ToString(dxddlModule.GetValues()));
            base.OnLoad(e);
        }

        private void BindGridView(string SelectedModule)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(context);
            List<ModulePrioirty> list;
            if (!string.IsNullOrEmpty(SelectedModule))
            {
                SelectedModule = ObjModuleViewManager.GetByID(Convert.ToInt64(SelectedModule)).ModuleName;
                list = prioirtyViewManager.LoadByModule(SelectedModule).OrderBy(x => x.ItemOrder).ToList();
                if (!dxshowdeleted.Checked)
                {
                    list = list.Where(x => !x.Deleted).ToList();
                }

                dxgridPriority.DataSource = list;
                dxgridPriority.DataBind();
            }
            else
            {
                list = new List<ModulePrioirty>();
                dxgridPriority.DataSource = list;
                dxgridPriority.DataBind();
            }
        }

        private void FillModuleDropDown(LookUpValueBox dropDown)
        {
            dropDown.devexListBox.AutoPostBack = false;
            dropDown.devexListBox.ClientSideEvents.ValueChanged = "function(s, e){ changeRequestType(); }";
        }

        protected void dxcombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string moduleName = Convert.ToString(dxddlModule.Value);
            string showdelete = dxshowdeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, moduleName, showdelete));
            Response.Redirect(url);
        }

        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.GetValues());
            string showdelete = dxshowdeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, ObjModuleViewManager.GetByID(Convert.ToInt64(moduleName)).ModuleName, showdelete));
            Response.Redirect(url);

        }
        protected void dxgridPriority_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue("Title"));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, dataKeyValue, moduleName));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','600',0,'{2}','true')", editItem, title, "/Layouts/uGovernIT/uGovernITConfiguration/", this.formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxgridPriority.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void dxgridPriority_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (dxddlModule.devexListBox.Value != null)
            {
                int selectedModule = Convert.ToInt32(dxddlModule.devexListBox.Value);
                Session["ModuleName"] = ObjModuleViewManager.GetByID(selectedModule).ModuleName;
                BindGridView(Convert.ToString(selectedModule));
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

            string cacheName = "Lookup_" + DatabaseObjects.Tables.TicketPriority + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketPriority, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
