using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Web.Rendering;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class ServiceCategories : UserControl
    {
        public string editServiceCategoryUrl = string.Empty;
        DataTable categories = new DataTable();
        public string CategoryType { get; set; }
        protected override void OnInit(EventArgs e)
        {
            editServiceCategoryUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernit/DelegateControl.aspx?control=servicecategoryeditor&categorytype=");
            editServiceCategoryUrl = string.Format("{0}{1}", editServiceCategoryUrl,CategoryType);
            BindCategory();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        private void BindCategory()
        {
            DataRow[] collectionservicecatgory = null;
            ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            categories = objServiceCategoryManager.GetDataTable();
            if (categories != null)
            {
                collectionservicecatgory = categories.Select($"{DatabaseObjects.Columns.CategoryName}<>'{Constants.ModuleFeedback}' And {DatabaseObjects.Columns.CategoryName}<>'{Constants.ModuleAgent}' And {DatabaseObjects.Columns.Deleted}<>'True' ");

                if (collectionservicecatgory != null && collectionservicecatgory.Length > 0)
                {
                    categories = collectionservicecatgory.CopyToDataTable();
                }
                GridViewDataComboBoxColumn column = (rCategories.Columns["ItemOrder"] as GridViewDataComboBoxColumn);
                
                DataView view = categories.DefaultView;
                view.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                rCategories.DataSource = view.ToTable();
                column.PropertiesComboBox.DataSource = view.ToTable();
                column.PropertiesComboBox.TextField = "ItemOrder";
                column.PropertiesComboBox.ValueField = "ItemOrder";
                rCategories.DataBind();

                
            }
            if (categories != null && categories.Rows.Count > 0)
            {
                
            }
        }
        protected void rCategories_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            int index = e.VisibleIndex;
           // drop down fill
            if (index >= 0)
            {
               
                
            }

        }
        protected void rCategories_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());

            foreach (var args in e.UpdateValues)
                objServiceCategoryManager.SaveCategoryOrder(args.Keys, args.NewValues);
            e.Handled = true;
            BindCategory();


        }

    }
}
