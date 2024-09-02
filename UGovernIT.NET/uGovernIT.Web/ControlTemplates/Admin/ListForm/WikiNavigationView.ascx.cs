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
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class WikiNavigationView : UserControl
    {
        protected string viewUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=wikinavigationview");
        WikiCategoryManager objWikiCategoryManager = new WikiCategoryManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            //DataTable dtWikiNavigation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.WikiLeftNavigation);
            var listWikiNavigation = objWikiCategoryManager.Load();

            listWikiNavigation = listWikiNavigation.OrderBy(x=>x.ItemOrder).ToList();
            aAddItem.Attributes.Add("href", string.Format("javascript:NewWikiNavigationDialog()"));
            LinkButton1.Attributes.Add("href", string.Format("javascript:NewWikiNavigationDialog()"));
            //dtWikiNavigation.DefaultView.Sort = DatabaseObjects.Columns.ItemOrder;
            ASPxGridViewWikiNavigation.DataSource = listWikiNavigation;
             ASPxGridViewWikiNavigation.DataBind();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ASPxGridViewWikiNavigation_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            //DataRow currentRow = ASPxGridViewWikiNavigation.GetDataRow(e.VisibleIndex);
            var currentRow = ASPxGridViewWikiNavigation.GetRow(e.VisibleIndex);
            if (currentRow == null) return;

            string func = string.Empty;
            string WikiId = string.Empty;
            
            if (Convert.ToString(((WikiCategory)currentRow).ID) != string.Empty)
            {
                WikiId = Convert.ToString(((WikiCategory)currentRow).ID).Trim();
            }
            func = string.Format("openWikinavigationDialog('{0}','{1}','{2}','{3}','{4}', 0)", viewUrl, string.Format("WikiId={0}", WikiId), "Edit Wiki Category", 90, 90);
            e.Row.Attributes.Add("onClick", func);
            //if (e.Row.Cells.Count == 4)
            //{
            //    e.Row.Cells[0].Attributes.Add("onClick", func);
            //    e.Row.Cells[1].Attributes.Add("onClick", func);
            //}
        }
    }
}
