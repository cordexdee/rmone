using DevExpress.Spreadsheet;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ConfigurationVariableListView : UserControl
    {
        protected string delegateUrl, addNetItem=string.Empty;

        ConfigurationVariableManager objConfigurationVariableHelper=new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        UserProfileManager ObjUserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        UserProfile userProfile = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            addNetItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=configvaribaleedit");
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Configuration Variable - New Item','750px','530px',0,'{1}')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Configuration Variable - New Item','750px','530px',0,'{1}')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            List<ConfigurationVariable> dt = objConfigurationVariableHelper.Load().Where(x => x.CategoryName != null).OrderBy(x=>x.CategoryName).OrderBy(x=>x.KeyName).ToList(); // where condition added to ignore smtp settings & category with null values
            if (dt != null && dt.Count > 0)
            {
                //dt.DefaultView.Sort = "CategoryName, KeyName ASC";
                dxgridConfigVariable.DataSource = dt;
                dxgridConfigVariable.DataBind();

            }
        }

        protected void GvFilteredList_PreRender(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
        }    

        protected void dxgridConfigVariable_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
              
            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ConfigurationVariableManager cVManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            cVManager.RefreshCache();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var list = objConfigurationVariableHelper.Load(x => !x.KeyName.EqualsIgnoreCase("SmtpCredentials")).Select(x => new {  x.CategoryName, x.KeyName, x.KeyValue, x.Title, x.Description, x.Type }).OrderBy(x => x.CategoryName).ThenBy(x => x.KeyName).ToList();

            DataTable table = UGITUtility.ToDataTable(list);
            var worksheet = SpreadSheetConfigVar.Document.Worksheets.ActiveWorksheet;
            worksheet.FreezeRows(0);
            worksheet.Import(table, true, 0, 0);
            MemoryStream st = new MemoryStream();
            SpreadSheetConfigVar.Document.SaveDocument(st, DocumentFormat.OpenXml);
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=ConfigurationVariables.xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();
        }

        protected void dxgridConfigVariable_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {            
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "KeyName")
            {
                string Title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.KeyName));
                int Index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=configvaribaleedit&ID=" + datakeyvalue + " ");
                string editurl = string.Format("javascript:UgitOpenPopupDialog('{0}','','Configuration Variable - Edit Item','750px','530px',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)dxgridConfigVariable.FindRowCellTemplateControl(Index, e.DataColumn, "editlink");
                aHtml.Attributes.Add("href", editurl);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.KeyName)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
            if (e.DataColumn.FieldName == "KeyValue")
            {
                ConfigurationVariable configurationVariable = (sender as uGovernIT.Web.ASPxGridView).GetRow(e.VisibleIndex) as ConfigurationVariable;
                if (configurationVariable != null && configurationVariable.Type == "User")
                {
                    //UserProfileManager ObjUserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                    //UserProfile userProfile = ObjUserManager.GetUserById(configurationVariable.KeyValue);
                    userProfile = ObjUserManager.GetUserById(configurationVariable.KeyValue);
                    if (userProfile != null)                    
                        e.Cell.Text = userProfile.UserName;
                    else
                        e.Cell.Text = string.Empty;
                }
            }
        }
    }
}
