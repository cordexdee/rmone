using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ServiceCategoryEditor : UserControl
    {
        private ServiceCategory spItem;
        private List<ServiceCategory> cList;
        private string absPath = string.Empty;
        
        protected override void OnInit(EventArgs e)
        {
            int categoryID = 0;
            int.TryParse(Request["categoryID"], out categoryID);

            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            cList = serviceCategoryManager.Load();
            spItem = serviceCategoryManager.LoadByID(categoryID);
            if (spItem != null)
            {
                tdDeleteAction.Visible = true;
                txtTitle.Text = Convert.ToString(spItem.CategoryName);
                int orderNo = 0;
                int.TryParse(Convert.ToString(spItem.ItemOrder), out orderNo);
                txtItemOrder.Text = orderNo.ToString();
                fileUploadIcon.SetImageUrl(Convert.ToString(spItem.ImageUrl));
            }
            else
            {
                txtItemOrder.Text = (cList.Count+1).ToString();
            }

            if (spItem == null && categoryID > 0)
            {
                return;
            }

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            //lnkbtnPickAssets.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));
          
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           //

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
           // if (Validate())
            {
                ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
                if (spItem == null)
                {
                    spItem = new ServiceCategory();
                }

                // = txtTitle.Text.Trim();
                spItem.CategoryName = txtTitle.Text.Trim();
                spItem.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
                spItem.Title = txtTitle.Text.Trim();
                spItem.ImageUrl = fileUploadIcon.GetImageUrl();

                if (spItem.ID > 0)
                {
                    serviceCategoryManager.Update(spItem);
                }
                else
                {
                    serviceCategoryManager.Insert(spItem);
                }
                List<ServiceCategory> lstCategories = serviceCategoryManager.LoadAllCategories();
                if (lstCategories != null && lstCategories.Count > 0)
                    lstCategories = lstCategories.OrderBy(x => x.ItemOrder).ToList();
                int counter = 0;
                foreach (ServiceCategory item in lstCategories)
                {
                    item.ItemOrder = ++counter;
                }
                serviceCategoryManager.UpdateItems(lstCategories);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void RFVCustomImpact_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            List<ServiceCategory> sCategoryList = serviceCategoryManager.Load();

            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CategoryName, txtTitle.Text.Trim());
            List<ServiceCategory> filteredsCategoryList = sCategoryList.Where(x => x.CategoryName == txtTitle.Text.Trim()).ToList();
            if (filteredsCategoryList != null && filteredsCategoryList.Count > 1)
            {
                args.IsValid = false;
            }
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (spItem != null)
            {
                ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
                spItem.Deleted = true;
                serviceCategoryManager.Update(spItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        private Boolean Validate()
        {
            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            List<ServiceCategory> sCategoryList = serviceCategoryManager.Load();
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CategoryName, txtTitle.Text.Trim());
            List<ServiceCategory> filteredsCategoryList = sCategoryList.Where(x => Convert.ToString(x.CategoryName) == txtTitle.Text.Trim()).ToList();
            if (filteredsCategoryList != null && filteredsCategoryList.Count > 0)
            {
                lblErrorMessage.Text = "Category is already in the list";
                return false;
            }
            else
            { return true; }
        }

    }
}
