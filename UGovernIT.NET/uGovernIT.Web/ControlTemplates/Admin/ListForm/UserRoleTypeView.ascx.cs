using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class UserRoleTypeView : UserControl
    {
        //  SPList spRoleType;
        string addNewItem;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}";
        private string formTitle = "User Role Type";
        private string viewParam = "userroletypes";
        private string newParam = "userrolenew";
        private string editParam = "userroleedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleUserTypeManager ObjModuleUserTypeManager;
        ModuleViewManager ObjModuleViewManager;
        FieldConfigurationManager fmanger;

        protected override void OnInit(EventArgs e)
        {           
            ObjModuleUserTypeManager = new ModuleUserTypeManager(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            fmanger = new FieldConfigurationManager(context);
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem1.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            BindModule();
            if (Request["module"] != null)
            {
                dxcombox.Value = Convert.ToString(Request["module"]);
            }
            else
            {
                dxcombox.SelectedIndex = 0;
            }
            BindGridView(Convert.ToString(dxcombox.Value));

            aAddItem1.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','module={3}','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle, dxcombox.Value));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','module={3}','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle, dxcombox.Value));
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        void BindModule()
        {
            dxcombox.Items.Clear();           
            List<UGITModule> lstModules = ObjModuleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            if (lstModules != null && lstModules.Count > 0)
            {
                dxcombox.DataSource = lstModules;
                dxcombox.TextField = DatabaseObjects.Columns.Title;
                dxcombox.ValueField = DatabaseObjects.Columns.ModuleName;
                dxcombox.DataBind();
            }
        }
        void BindGridView(string Module)
        {
            ModuleUserTypeManager userTypemgr = new ModuleUserTypeManager(context);
            List<ModuleUserType> lstModuleUsertype = userTypemgr.Load(x=>x.ModuleNameLookup == Module);
            if (lstModuleUsertype != null)
            {
                dxgridview.DataSource = lstModuleUsertype;
                dxgridview.DataBind();
            }
        }
        protected void combox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = Convert.ToString(dxcombox.Value);
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, module));
            Response.Redirect(url);
            // BindGridView(Convert.ToString(dxcombox.Value));
        }

        protected void dxgridview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string val = string.Empty;
            if (e.DataColumn.FieldName == "ITOnly" || e.DataColumn.FieldName == "ManagerOnly")
            {
                val = e.CellValue.ToString();
                e.Cell.Text = "No";
                if (UGITUtility.StringToBoolean(val))
                    e.Cell.Text = "Yes";
            }
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "ColumnName")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string dxComboValue = Convert.ToString(dxcombox.Value);
                string columnNameValue = Convert.ToString(e.GetValue("ColumnName"));
                string title = dxComboValue + " - " + columnNameValue;
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, dataKeyValue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {2}','600','450',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), title, formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxgridview.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "ColumnName")
                {
                    aHtml.InnerText = e.CellValue?.ToString();
                }
            }
            if (e.DataColumn.FieldName == "DefaultUser")
            {
                if (e.CellValue != null)
                {
                    val = Convert.ToString(e.CellValue);
                    e.Cell.Text = fmanger.GetFieldConfigurationData(DatabaseObjects.Columns.CreatedByUser, val).Replace("#", " ");                     
                }                
            }
            if (e.DataColumn.FieldName == "Groups")
            {
                if (e.CellValue != null)
                {
                    val = Convert.ToString(e.CellValue);
                    e.Cell.Text = fmanger.GetFieldConfigurationData("UserGroup", val).Replace("#", " ");
                }
            }
        }
        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            string moduleName = UGITUtility.ObjectToString(dxcombox.Value);
            UGITModule module = ObjModuleViewManager.GetByName(moduleName);
            if (module != null)
            {
                module.List_ModuleUserTypes = ObjModuleUserTypeManager.Load(x => x.ModuleNameLookup == moduleName);
                CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }
            BindGridView(moduleName);

        }
    }
}
