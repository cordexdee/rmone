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
using DevExpress.Web.Rendering;
using System.Web;

namespace uGovernIT.Web
{
    public partial class EventCategoriesView : UserControl
    {
       // private SPList _SPList;
        //private DataTable _DataTable;
       
        private string addNewItem = string.Empty;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string formTitle = "Event Category";
        private string newParam = "eventcategoryedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','350',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','350',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            BindGriview();
            base.OnLoad(e);
        }

        private void BindGriview()
        {
            //SPQuery qry = new SPQuery();
            //qry.Query = "<Where></Where>";
            //qry.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.UGITItemColor),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ItemOrder),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.IsDeleted));
            //qry.ViewFieldsOnly = true;

            EventCategoryViewManager eventCategoryViewManager= new EventCategoryViewManager(context);
            DataTable dt=eventCategoryViewManager.GetDataTable();


            //DataTable dt = //SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, qry);
                           
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        row[DatabaseObjects.Columns.UGITItemColor] = Convert.ToString(row[DatabaseObjects.Columns.UGITItemColor]);
                    }
                }
           
            dxgridview.DataSource = dt;
            dxgridview.DataBind();
        }

        protected void dxgridview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if(e.DataColumn.Name=="aEdit" ||e.DataColumn.FieldName=="Title")
            {
                string Title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                int Index = e.VisibleIndex;
                string datakeyvale = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, datakeyvale));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','170',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor ahtml = (HtmlAnchor)dxgridview.FindRowCellTemplateControl(Index, e.DataColumn, "editlink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    ahtml.InnerText = e.CellValue.ToString();

                }
            }
        }
    }
}
