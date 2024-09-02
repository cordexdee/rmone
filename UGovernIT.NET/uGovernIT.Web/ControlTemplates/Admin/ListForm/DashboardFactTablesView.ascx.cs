using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Manager.Managers;
using DevExpress.Web;
using System.Web.UI.HtmlControls;

namespace uGovernIT.Web
{
    public partial class DashboardFactTablesView : UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        private bool isBindingDone;
        public string importExcelPagePath;
        public string exportListPagePath;
        public string openAddPath;
        public string editDashboradTable;
        DashboardFactTableManager dashboardFactTableManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            importExcelPagePath = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/delegatecontrol.aspx?control=importexcel&sourcePage=DashBoardFactTables");
            exportListPagePath = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/delegatecontrol.aspx?control=exportList&sourcePage=DashBoardFactTables");
            openAddPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configadddashboardfacttable&sourcePage=DashBoardFactTables");
            editDashboradTable = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configeditdashboardfacttable&sourcePage=DashBoardFactTables");
            Show_ImportExport();
        }
       
        protected void Show_ImportExport()
        {
            bool enableImportExport = applicationContext.ConfigManager.GetValueAsBool("EnableFactTableImportExport");
            btImport.Visible = enableImportExport;
            btExport.Visible = enableImportExport;
        }
        protected override void OnInit(EventArgs e)
        {
            BindDashboardTable();
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (!isBindingDone)
            {
                BindDashboardTable();
            }
            base.OnPreRender(e);
        }
        private void BindDashboardTable()
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            List<DashboardFactTables> dashboardFactTable = dashboardFactTableManager.Load().OrderBy(x => x.Title).ToList();
            if (dashboardFactTable == null || dashboardFactTable.Count <= 0)
            {
                return;
            }
            grdDashbaords.DataSource = dashboardFactTable;
            grdDashbaords.DataBind();
            isBindingDone = true;
        }
        protected void BtRefreshItem_Click(object sender, EventArgs e)
        {
            //dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            //ImageButton bt = (ImageButton)sender;
            //int itemID = UGITUtility.StringToInt(bt.CommandArgument);
            //DashboardFactTables dashboardFactTables = dashboardFactTableManager.LoadByID(Convert.ToInt64(itemID));
            //bool isCacheTable = false;
            //bool.TryParse(Convert.ToString(dashboardFactTables.CacheTable), out isCacheTable);
            //if (dashboardFactTables != null && isCacheTable)
            //{
            //    //DashboardCache.GetCachedDashboardData(this.applicationContext, dashboardFactTables.Title);
            //    DashboardCache.RefreshDashboardCache(dashboardFactTables.Title, this.applicationContext);
            //    BindDashboardTable();
            //}
        }
        protected void BtDeleteItem_Click(object sender, EventArgs e)
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            ImageButton bt = (ImageButton)sender;
            int itemID = UGITUtility.StringToInt(bt.CommandArgument);
            DashboardFactTables dashboardFactTables = dashboardFactTableManager.LoadByID(Convert.ToInt64(itemID));
            if (dashboardFactTables != null)
            {
                dashboardFactTableManager.Delete(dashboardFactTables);
                // item.Delete();
            }
        }
        protected void BtRefreshAll_Click(object sender, EventArgs e)
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            List<DashboardFactTables> tables = dashboardFactTableManager.Load();
            foreach (var row in tables)
            {
                //DashboardCache.GetCachedDashboardData(this.applicationContext, row.Title);
                if (row.CacheTable)
                {
                    DashboardCache.RefreshAllDashboardCache(this.applicationContext, Convert.ToInt32(row.ID), row.CacheAfter);
                    BindDashboardTable();
                }
            }
        }
        protected void grdDashbaords_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configeditdashboardfacttable&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','DashBoardFact Table - {1}','700','500',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)grdDashbaords.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
                
            }
            if (e.DataColumn.FieldName == "CacheMode")
            {
                string Mode = Convert.ToString(e.GetValue(DatabaseObjects.Columns.CacheMode));
                if (Mode == "Scheduled")
                {
                    e.Cell.Text = string.Format("(Refresh {0} every {1} min)", Convert.ToString(e.GetValue(DatabaseObjects.Columns.RefreshMode)), Convert.ToString(e.GetValue(DatabaseObjects.Columns.CacheAfter)));
                }
            }
        }

        protected void grdDashbaords_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            int itemID = UGITUtility.StringToInt(e.Parameters);
            DashboardFactTables dashboardFactTables = dashboardFactTableManager.LoadByID(Convert.ToInt64(itemID));
            bool isCacheTable = false;
            bool.TryParse(Convert.ToString(dashboardFactTables.CacheTable), out isCacheTable);
            if (dashboardFactTables != null && isCacheTable)
            {
                //DashboardCache.GetCachedDashboardData(this.applicationContext, dashboardFactTables.Title);
                DashboardCache.RefreshDashboardCache(dashboardFactTables.Title, this.applicationContext);
                BindDashboardTable();
            }
        }
    }
}
