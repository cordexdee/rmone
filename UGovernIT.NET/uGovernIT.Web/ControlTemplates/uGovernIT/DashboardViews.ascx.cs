using System;
using System.Web.UI;
using System.Data;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class DashboardViews : UserControl
    {
        protected string delegateUrl = string.Empty;
        DataTable resultTable = null;
        private string AddDashboardUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=dashboarddesigner&viewid=");
        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/uGovernitConfiguration.aspx?control=configdashboardeditviews&viewid=");



            LoadData();
            //gridDashbordViews.AddNewRow();
            //gridDashbordViews.SettingsPager.PageSize = 18;
            
            gridDashbordViews.DataSource = resultTable;

            //string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','','1300px','625px',0,'{1}')", AddDashboardUrl, Uri.EscapeUriString(Request.Url.AbsolutePath));

            //aAddItem.Attributes.Add("href", jsFunc);

            base.OnInit(e);
        }

        private void LoadData()
        {
            DataTable dashboardViewList = objDashboardPanelViewManager.GetDataTable();
          // string query = string.Format("<Where></Where><OrderBy ><FieldRef Name='{0}'/><FieldRef Name='{1}'/></OrderBy>", DatabaseObjects.Columns.UGITViewType, DatabaseObjects.Columns.Title); ;
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.UGITViewType, DatabaseObjects.Columns.Title);
            DataView view = new DataView(dashboardViewList);
            view.Sort = DatabaseObjects.Columns.UGITViewType + " asc, " + DatabaseObjects.Columns.ViewName + " asc";           
            //view.Sort = "Age desc, Name asc";
            resultTable = view.ToTable(false, DatabaseObjects.Columns.ID, DatabaseObjects.Columns.UGITViewType, DatabaseObjects.Columns.ViewName); ;

            if (resultTable != null)
            {
                resultTable.Columns.Add("serviceUrl");
                resultTable.Columns["serviceUrl"].Expression = string.Format("'{0}'+ {1}", delegateUrl, DatabaseObjects.Columns.ID);
                resultTable.DefaultView.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.UGITViewType, DatabaseObjects.Columns.ViewName);
            }            
        }

        protected override void OnPreRender(EventArgs e)
        {
            gridDashbordViews.DataBind();
            base.OnPreRender(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void gridDashbordViews_DataBinding(object sender, EventArgs e)
        {
            if (gridDashbordViews.DataSource == null)
            {
                LoadData();
                 gridDashbordViews.DataSource = resultTable;
                //gridDashbordViews.DataBind();
            }
        }

        protected void gridDashbordViews_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            DataRow currentRow = gridDashbordViews.GetDataRow(e.VisibleIndex);
            string url = string.Format("{0}{1}", (Convert.ToString(currentRow[DatabaseObjects.Columns.UGITViewType]) == "Common Dashboards" ? AddDashboardUrl : delegateUrl), currentRow[DatabaseObjects.Columns.ID]); 
            string title = string.Format("Dashboard View: {0}", currentRow[DatabaseObjects.Columns.ViewName]);
            string sourceUrl = Uri.EscapeDataString(Request.Url.AbsolutePath);
            string clickFunction = string.Format("window.parent.UgitOpenPopupDialog('{0}','','{1}','95','90', 0, '{2}')", url, title, sourceUrl); //'1000px','600px'
            e.Row.Attributes.Add("onClick", clickFunction);
            e.Row.Style.Add("cursor", "pointer");
        }

        protected void gridDashbordViews_CommandButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.Cancel)
            {
                e.Visible = false;
                e.Enabled = false;
            }
        }

        protected void gridDashbordViews_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string title = Convert.ToString(e.NewValues["Title"]);
            DashboardPanelView objDashboardPanelView = new DashboardPanelView();
            // DataTable spDashboardPanelView = objDashboardPanelViewManager.GetDataTable();
            //DataRow item = spDashboardPanelView.NewRow();
            // item[DatabaseObjects.Columns.Title] = title.Trim();
            objDashboardPanelView.Title = title.Trim();
            objDashboardPanelView.ViewName = title.Trim();
            //  item[DatabaseObjects.Columns.UGITViewType] = "Common Dashboards";
            objDashboardPanelView.ViewType = "Common Dashboards";
            //item.Update();
            objDashboardPanelViewManager.Insert(objDashboardPanelView);
            e.Cancel = true;
            gridDashbordViews.CancelEdit();
            //LoadData();
            //gridDashbordViews.DataBind();
            gridDashbordViews.AddNewRow();
            //glSkills.GridView.Selection.SelectRowByKey(item.ID);
        }

        protected void gridDashbordViews_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string title = Convert.ToString(e.NewValues["Title"]);
            if (string.IsNullOrEmpty(title))
            {
                e.RowError = "Please enter skill";
                return;
            }

           // DataTable spDashboardPanelView = objDashboardPanelViewManager.GetDataTable();//(DatabaseObjects.Lists.DashboardPanelView);
            //SPQuery query = new SPQuery();
           // string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, title);
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            // query.ViewFieldsOnly = true;
            //DataRow[] collection = spDashboardPanelView.Select(query);
            List<DashboardPanelView> collection = objDashboardPanelViewManager.Load(x => x.ViewName == title).ToList();
            if (collection.Count() > 0)
            {
                e.RowError = "View is already in the list";
            }
        }

        protected void cbCheckText_Callback(object source, CallbackEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Parameter))
            {
                return;
            }
            
            List<DashboardPanelView> collection = objDashboardPanelViewManager.Load(x => x.ViewName == e.Parameter).ToList();
            //string query= string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, e.Parameter);
           
            if (collection.Count() == 0)
            {
                e.Result = "Valid";
                //DataRow item = spDashboardPanelView.AddItem();
                DashboardPanelView objDashboardPanelView = new DashboardPanelView();
                objDashboardPanelView.ViewName = e.Parameter;
                objDashboardPanelView.Title = e.Parameter;
                objDashboardPanelView.ViewType = hndViewType.Value;// "Common Dashboards";
                objDashboardPanelViewManager.Insert(objDashboardPanelView);
            }
        }

        protected void gridDashbordViews_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }
        
    }
}
