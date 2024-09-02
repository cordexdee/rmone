using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ProjectStandardWorkItemView : UserControl
    {
        private DataTable _SPList;
        private DataTable _DataTable;
        private string addNewItem = string.Empty;

        #region Constants
        private const string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ItemID={1}";
        private const string absoluteUrlView = "/layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private const string formTitle = "Project Standard Work Items";
        private const string viewParam = "projectstandardworkitemview";
        private const string newParam = "projectstandardworkitemnew";
        private const string editParam = "projectstandardworkitemedit";
        #endregion

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectStandardWorkItemManager ProjStdWorkItemManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;

        protected override void OnInit(EventArgs e)
        {
            ProjStdWorkItemManager = new ProjectStandardWorkItemManager(context);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(context);

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','550','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','550','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //_SPList = ProjStdWorkItemManager.GetDataTable();   // GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectStandardWorkItems);
            //_SPList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectStandardWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            _SPList = GetTableDataManager.GetData(DatabaseObjects.Tables.ProjectStandardWorkItems, values);

            bool inCludeDeleted = false;
            if (!IsPostBack && Request["showdelete"] != null)
            {
                string showdelete = Convert.ToString(Request["showdelete"]);
                inCludeDeleted = chkShowDeleted.Checked = showdelete == "0" ? false : true;
            }
            // chkEnableProjectStandardWorkItems.Checked = ConfigurationVariable.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);
            chkEnableProjectStandardWorkItems.Checked = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);
            BindGrid(inCludeDeleted);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
                
        public void BindGrid(bool inCludeDeleted)
        {
            if (_SPList != null && _SPList.Rows.Count > 0)
            {
                string query = string.Empty;

                if (inCludeDeleted)
                    query = string.Format(" {0} in {1}", DatabaseObjects.Columns.Deleted, ("False", "True"));
                else
                    query = string.Format(" {0} = '{1}'", DatabaseObjects.Columns.Deleted, "False");

                DataRow[] prjWorkitems = _SPList.Select(query);

                if (prjWorkitems.Count() > 0)
                    _DataTable = prjWorkitems.CopyToDataTable();
            }
            else
                _DataTable = null;
            if (_DataTable != null)
            {
                _DataTable.DefaultView.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.ItemOrder, DatabaseObjects.Columns.Title);
                dx_gridView.DataSource = _DataTable;
                dx_gridView.DataBind();
            }
        }

        protected void dx_gridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {
                int index = e.VisibleIndex;
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, lsDataKeyValue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','550','400',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dx_gridView.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = chkShowDeleted.Checked ? "1" : "0";

            string url = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProjectStandardWorkItemView&Width=1043.2&Height=465.6&isudlg=1&pageTitle=Project Standard Work Item&IsDlg=1&showdelete=" + showdelete);
            Response.Redirect(url);
        }


        protected void dx_gridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = dx_gridView.GetDataRow(e.VisibleIndex);
                if (UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.Deleted]))
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(250, 219, 216);
                    foreach (TableCell item in e.Row.Cells)
                    {
                        //item.Style.Add("color", "#FFF");
                    }
                }
            }
        }

        protected void chkEnableProjectStandardWorkItems_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            string query = string.Empty;
            //ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            ConfigurationVariable enableStdProjectItems = ObjConfigurationVariableManager.LoadVaribale(ConfigConstants.EnableProjStdWorkItems);
            if (enableStdProjectItems != null)
            {
                if (!chk.Checked)
                {
                    enableStdProjectItems.KeyValue = "False";
                }
                else
                {
                    enableStdProjectItems.KeyValue = "True";
                }
                ObjConfigurationVariableManager.Update(enableStdProjectItems);
            }
            else
            {
                if (chk.Checked)
                {
                    enableStdProjectItems = new ConfigurationVariable();
                    enableStdProjectItems.CategoryName = "General";
                    enableStdProjectItems.KeyName = ConfigConstants.EnableProjStdWorkItems.ToString();
                    enableStdProjectItems.Title = ConfigConstants.EnableProjStdWorkItems.ToString();
                    // spListitem.Properties["Type"] = "Bool";
                    enableStdProjectItems.KeyValue = "True";
                    ObjConfigurationVariableManager.Insert(enableStdProjectItems);
                }
            }
        }
    }
}