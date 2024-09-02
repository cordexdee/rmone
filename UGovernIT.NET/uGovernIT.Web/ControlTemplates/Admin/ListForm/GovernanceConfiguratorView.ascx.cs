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
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class GovernanceConfiguratorView : UserControl
    {
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&itemID={2}&categoryType={3}";
        private string absoluteUrlNew = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&itemID={2}&categoryType={3}";
        //private string absoluteUrlView = "_layouts/15/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}&mode={4}";
        private string formTitle = string.Empty;
        //private string viewParam = "governanceconfig";
        private string newParam = "governanceconfigadd";
        private string editParam = "governanceconfigedit";
        List<GovernanceLinkItem> splstCategoryItems;
        List<GovernanceLinkCategory> splstCategory;
        string addNewItem = string.Empty;
        public string categoryTypeParam = string.Empty;
        GovernanceLinkItemManager ObjGovernanceLinkItemManager = new GovernanceLinkItemManager(HttpContext.Current.GetManagerContext());
        GovernanceLinkCategoryManager ObjGovernanceLinkCategoryManager = new GovernanceLinkCategoryManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, 0, 0, this.categoryTypeParam));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            splstCategoryItems = ObjGovernanceLinkItemManager.Load(x=>!x.Deleted); 
            splstCategory = ObjGovernanceLinkCategoryManager.Load(x => !x.Deleted);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            { }
            BindGridView();
            base.OnLoad(e);
        }
        private void BindGridView()
        {
            List<GovernanceLinkItem> finalTable = new List<GovernanceLinkItem>();
            //dtItems = splstCategoryItems.Items.GetDataTable();
            if (splstCategoryItems != null)
            {
                finalTable = splstCategoryItems;
                finalTable.ForEach(x =>
                {
                    GovernanceLinkCategory item = ObjGovernanceLinkCategoryManager.LoadByID(x.GovernanceLinkCategoryLookup);
                    if (item != null)
                        x.CategoryName = item.CategoryName;
                    else
                        x.CategoryName = "";
                });
                List<GovernanceLinkItem> foundRows;
                string categoryName = String.Empty;
                categoryName = "Governance Dashboard Buttons";
                if (this.categoryTypeParam.Equals("config"))
                {
                    foundRows = finalTable.OrderBy(x=>x.TabSequence).ToList();
                    dx_gridView.Settings.ShowGroupPanel = true;
                    if (dx_gridView.Columns["Category"] == null)
                    {
                        GridViewDataColumn gridViewDataColumn = new GridViewDataColumn();
                        gridViewDataColumn.FieldName = "CategoryName";
                        gridViewDataColumn.Caption = "Category";
                        gridViewDataColumn.GroupIndex = 0;
                        dx_gridView.Columns.Add(gridViewDataColumn);
                    }
                    DataView view = UGITUtility.ToDataTable<GovernanceLinkItem>(foundRows).DefaultView;
                    view.Sort = "TabSequence ASC";
                    DataTable sortedDate = view.ToTable();

                    dx_gridView.DataSource = sortedDate;
                    dx_gridView.DataBind();
                }
                else
                {
                    foundRows = finalTable.Where(y => y.CategoryName.ToLower().Equals(categoryName.ToLower())).ToList();
                    dx_gridView.DataSource = UGITUtility.ToDataTable<GovernanceLinkItem>(foundRows);
                    dx_gridView.DataBind();
                }
            }
        }
      
        protected void dx_gridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int Index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string Title = "";
                if (e.GetValue(DatabaseObjects.Columns.Title) != null)
                    Title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string ID = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ID));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, datakeyvalue, ID, this.categoryTypeParam));
                string url;
                HtmlAnchor ahtml = (HtmlAnchor)dx_gridView.FindRowCellTemplateControl(Index, e.DataColumn, "editLink");
                if (this.categoryTypeParam.Equals("config"))
                {
                    url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','400',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), ID);
                }
                else
                {
                    url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','400',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath));
                }
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    ahtml.InnerText = e.CellValue.ToString();
                }
                if (e.DataColumn.FieldName == "GovernanceLinkCategoryLookup")
                {
                    e.Cell.Text = ObjGovernanceLinkCategoryManager.LoadByID(Convert.ToInt32(e.CellValue)).CategoryName;
                }

            }
        }
    }
}
