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
    public partial class GovernanceCategoryAdd : UserControl
    {
        //SPList splst;

        GovernanceLinkCategory governanceLink { get; set; }
        GovernanceLinkCategoryManager ObjGovernanceLinkCategoryManager = new GovernanceLinkCategoryManager(HttpContext.Current.GetManagerContext());

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            governanceLink = new GovernanceLinkCategory();
            governanceLink.Title = categoryName.Text.Trim();
            governanceLink.CategoryName = categoryName.Text.Trim();
            governanceLink.ItemOrder = UGITUtility.StringToInt(txtOrder.Text);
            governanceLink.ImageUrl = txtImageUrl.Text;
            if (ObjGovernanceLinkCategoryManager.Get(x => x.Title.ToLower().Equals(categoryName.Text.ToLower().Trim())) == null)
                ObjGovernanceLinkCategoryManager.Insert(governanceLink);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
