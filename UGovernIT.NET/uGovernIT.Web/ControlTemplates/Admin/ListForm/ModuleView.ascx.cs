using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ModuleView : UserControl
    {       
        private string addNewItem = string.Empty;
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdisable={2}";
        private string formTitle = "Module";
        private string viewParam = "moduleview";
        private string newParam = "modulenew";
        private string editParam = "moduleedit";
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','New Item','700','700',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','New Item','700','700',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            base.OnInit(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdisable"] != null)
                {
                    dxShowDisabled.Checked = Convert.ToString(Request["showdisable"]) == "0" ? false : true;

                }

            }
            BindGriview();
            base.OnLoad(e);
        }

        private void BindGriview()
        {
            
            List<UGITModule> moduleList = ModuleManager.Load().OrderBy(x => x.ModuleName).ToList();
            if (moduleList.Count > 0 && !dxShowDisabled.Checked)
            {
                moduleList = moduleList.Where(x => x.EnableModule == true).ToList();
            }
            dxModuleView.DataSource = moduleList;
            dxModuleView.DataBind();
        }


        protected void dxShowDisabled_CheckedChanged(object sender, EventArgs e)
        {
            string showdisable = dxShowDisabled.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdisable));
            Response.Redirect(url);
        }

        protected void dxModuleView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, dataKeyValue));
                string url = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{3} - {1}','700','700',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxModuleView.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.EnableEventReceivers )
            {
                bool CheckEnableEventReceivers = Convert.ToBoolean(e.CellValue);
                if (CheckEnableEventReceivers)
                    e.Cell.Text = "Yes";
                else
                    e.Cell.Text = "No";

            }
        }
    }
}