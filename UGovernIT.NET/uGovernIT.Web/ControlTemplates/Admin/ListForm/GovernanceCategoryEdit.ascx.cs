using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using Utils;
using System.Data.SqlClient;
using System.Web;

namespace uGovernIT.Web
{
    public partial class GovernanceCategoryEdit : UserControl
    {
        public string categoryID;
        public int Id { get; set; }
        public string categoryType;
        GovernanceLinkCategory governanceLink { get; set; }
        public DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        GovernanceLinkCategoryManager ObjGovernanceLinkCategoryManager = new GovernanceLinkCategoryManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            governanceLink = ObjGovernanceLinkCategoryManager.Get(x => x.ID == UGITUtility.StringToLong(categoryID));
            if (governanceLink != null)
            {
                categoryName.Text = governanceLink.Title;
                categoryName.Text = governanceLink.CategoryName;
                txtOrder.Text = Convert.ToString(governanceLink.ItemOrder);
                txtImageUrl.Text = governanceLink.ImageUrl;

            }
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            governanceLink = new GovernanceLinkCategory();
            if (!string.IsNullOrEmpty(Request["categoryID"].ToString()))
            {
                governanceLink.ID = Convert.ToInt64(Request["categoryID"]);
                governanceLink.Title = categoryName.Text.Trim();
                governanceLink.CategoryName = categoryName.Text.Trim();
                governanceLink.ItemOrder = Convert.ToInt32(txtOrder.Text);
                governanceLink.ImageUrl = txtImageUrl.Text;
                ObjGovernanceLinkCategoryManager.Update(governanceLink);
            }
            else
            {
                governanceLink.Title = categoryName.Text.Trim();
                governanceLink.CategoryName = categoryName.Text.Trim();
                governanceLink.ItemOrder = Convert.ToInt32(txtOrder.Text);
                governanceLink.ImageUrl = txtImageUrl.Text;
                ObjGovernanceLinkCategoryManager.Update(governanceLink);
            }
            
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
