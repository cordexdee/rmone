using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using DevExpress.Web;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    //Changes By Munna Singh
    public partial class AssetVendorsEdit : UserControl
    {
        public int Id { get; set; }
        //private SPListItem _SPListItem;
        //private DataTable dt;
        public bool FromCtrl { get; set; }
        public delegate void ClosePopupHandler(int orgID);
        public event ClosePopupHandler ClosePupup = delegate { };
        public string dialogUrl = "Layout/ugovernit/DelegateControl.aspx?control=vendortypeaddedit";
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        AssetVendorViewManager assetVendorViewManager;
        AssetVendor assetVendor;
        VendorTypeManager vendorTypeManager;
        VendorType vendorType;
        List<VendorType> spList;
        protected override void OnInit(EventArgs e)
        {
            vendorTypeManager = new VendorTypeManager(_context);
            assetVendorViewManager = new AssetVendorViewManager(_context);
            spList = new List<VendorType>();
            spList = vendorTypeManager.Load();
            BindVendorType();
            BindTimeZone();
            if (Id > 0)
            {
                assetVendor = new AssetVendor();
                assetVendor = assetVendorViewManager.LoadByID(Convert.ToInt64(Id));
                Fill();
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //comment by M
            //if (UserProfile.IsSuperAdmin(SPContext.Current.Web.CurrentUser))
            //    btnrecycle.Visible = true;

        }

        public void Fill()
        {
            txtVendorName.Text = assetVendor.VendorName;
            txtVendorLocation.Text = assetVendor.VendorLocation;
            txtPhone.Text = assetVendor.VendorPhone;
            txtEmail.Text = assetVendor.VendorEmail;
            txtAddress.Text = assetVendor.VendorAddress;
            chkDeleted.Checked = assetVendor.Deleted;
            txtWebsiteUrl.Text = assetVendor.WebsiteUrl;
            ddlTimeZone.SelectedIndex = ddlTimeZone.Items.IndexOf(ddlTimeZone.Items.FindByValue(assetVendor.VendorTimeZoneChoice));
            txtAccountRepEmail.Text = assetVendor.AccountRepEmail;
            txtAccountRepMobile.Text = assetVendor.AccountRepMobile;
            txtAccountRepName.Text = assetVendor.AccountRepName;
            txtAccountRepPhone.Text = assetVendor.AccountRepPhone;
            txtSupportCredentials.Text = assetVendor.SupportCredentials;
            if (assetVendor.VendorTypeLookup != 0)
            {    // SPFieldLookupValue vendorLookup = new SPFieldLookupValue(Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.VendorTypeLookup]));
                //if (vendorLookup != null)
                //{
                ddlVendorType.SelectedValue = assetVendor.VendorTypeLookup.ToString();
                //ddlVendorType.SelectedIndex = ddlVendorType.Items.IndexOf(ddlVendorType.Items.FindByValue(Convert.ToString(vendorLookup.LookupId)));
                // }
            }

            txtDescription.Text = assetVendor.ProductServiceDescription;
            txtsupporthours.Text = UGITUtility.ObjectToString(assetVendor.SupportHours);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // assetVendor = new AssetVendor();
            assetVendor.Title = txtVendorName.Text.Trim();
            assetVendor.VendorName = txtVendorName.Text.Trim();
            if (ddlVendorType.SelectedIndex > 0)
                assetVendor.VendorTypeLookup = UGITUtility.StringToLong(ddlVendorType.SelectedValue.ToString());
            assetVendor.ProductServiceDescription = txtDescription.Text;
            assetVendor.VendorLocation = txtVendorLocation.Text.Trim();
            assetVendor.VendorPhone = txtPhone.Text.Trim();
            assetVendor.VendorEmail = txtEmail.Text.Trim();
            assetVendor.VendorAddress = txtAddress.Text.Trim();
            assetVendor.WebsiteUrl = txtWebsiteUrl.Text.Trim();
            if (ddlTimeZone.SelectedIndex > 0)
                assetVendor.VendorTimeZoneChoice = ddlTimeZone.SelectedValue.ToString();
            assetVendor.SupportHours = UGITUtility.StringToDouble(txtsupporthours.Text.Trim());
            assetVendor.AccountRepName = txtAccountRepName.Text.Trim();
            assetVendor.AccountRepPhone = txtAccountRepPhone.Text.Trim();
            assetVendor.AccountRepEmail = txtAccountRepEmail.Text.Trim();
            assetVendor.AccountRepMobile = txtAccountRepMobile.Text.Trim();
            assetVendor.Deleted = chkDeleted.Checked;
            assetVendorViewManager.Update(assetVendor);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        internal void ClearForm()
        {
            txtVendorName.Text = "";
            txtVendorLocation.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            //txtContactName.Text = "";
            chkDeleted.Checked = false;
            txtWebsiteUrl.Text = "";
            txtSupportCredentials.Text = "";
            txtAccountRepEmail.Text = "";
            txtAccountRepMobile.Text = "";
            txtAccountRepName.Text = "";
            txtAccountRepPhone.Text = "";
            ddlTimeZone.ClearSelection();
            ddlVendorType.ClearSelection();
            txtDescription.Text = "";
            txtsupporthours.Text = string.Empty;
        }

        protected void BindTimeZone()
        {
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_context);
            ddlTimeZone.Items.Clear();
            DataTable list = fieldConfigurationManager.GetFieldDataByFieldName(DatabaseObjects.Columns.VendorTimeZone,DatabaseObjects.Tables.AssetVendors);
            if (list != null && list.Rows.Count > 0)
            {
                ddlTimeZone.DataTextField = DatabaseObjects.Columns.ID;
                ddlTimeZone.DataValueField = DatabaseObjects.Columns.Title;
                ddlTimeZone.DataSource = list;
                ddlTimeZone.DataBind();
                ddlTimeZone.Items.Insert(0, new ListItem("Select"));
            }
        }

        private void BindVendorType()
        {

            ddlVendorType.Items.Clear();
            List<VendorType> listVendorType = vendorTypeManager.Load().OrderBy(x => x.Title).ToList();
            if (listVendorType != null && listVendorType.Count > 0)
            {
                ddlVendorType.DataTextField = DatabaseObjects.Columns.Title;
                ddlVendorType.DataValueField = DatabaseObjects.Columns.Id;
                ddlVendorType.DataSource = listVendorType;
                ddlVendorType.SelectedValue = null;
                ddlVendorType.DataBind();
            }
            ddlVendorType.Items.Insert(0, new ListItem("", ""));

        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            string mode = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(e.Parameter)))
                mode = Convert.ToString(e.Parameter);
            else
                mode = hdnKeepkeyValue.Contains("mode") ? Convert.ToString(hdnKeepkeyValue.Get("mode")) : string.Empty;

            ASPxCallbackPanel1.JSProperties.Add("cpAllowCallback", false);
            ASPxCallbackPanel1.JSProperties.Add("cpPassDropDownValue", "");
            if (!string.IsNullOrEmpty(mode) && mode == "new")
            {
                EditOrUpdate(mode);
                popupnew.JSProperties.Add("cpCloseNew", true);
            }
            else if (mode == "edit" || mode == "update")
            {

                string key = hdnKeepkeyValue.Contains("KeyValue") ? Convert.ToString(hdnKeepkeyValue.Get("KeyValue")) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    EditOrUpdate(mode, key);
                if (mode == "update")
                    popupedit.JSProperties.Add("cpCloseEdit", true);

            }
        }
        protected void EditOrUpdate(string mode, string key = "")
        {
            //SPListItem item = null;
            //DataTable dt;
            if (mode == "new")
            {
                if (string.IsNullOrEmpty(txttitle.Text.Trim()))
                    return;

                vendorType = new VendorType();

                vendorType.Title = txttitle.Text.Trim();
                vendorType.VTDescription = txtdexcription.Text.Trim();
                vendorType.Deleted = chkdelete.Checked;
                vendorTypeManager.Insert(vendorType);
                ASPxCallbackPanel1.JSProperties["cpAllowCallback"] = true;
                ASPxCallbackPanel1.JSProperties["cpPassDropDownValue"] = Convert.ToString(key);
            }
            else if (mode == "edit")
            {
                vendorType = vendorTypeManager.LoadByID(Convert.ToInt64(key));
                if (vendorType != null)
                {
                    txttitleedit.Text = vendorType.Title;
                    txtdesedit.Text = vendorType.VTDescription;
                    chkdeleteedit.Checked = vendorType.Deleted;
                }
            }
            else if (mode == "update")
            {
                if (string.IsNullOrEmpty(txttitleedit.Text.Trim()))
                    return;

                vendorType = vendorTypeManager.LoadByID(Convert.ToInt64(key));
                if (vendorType != null)
                {

                    vendorType.Title = txttitleedit.Text;
                    vendorType.VTDescription = txtdesedit.Text;
                    vendorType.Deleted = chkdeleteedit.Checked;
                    vendorTypeManager.Update(vendorType);

                }
                ASPxCallbackPanel1.JSProperties["cpAllowCallback"] = true;
                ASPxCallbackPanel1.JSProperties["cpPassDropDownValue"] = Convert.ToString(key);
            }
        }

        protected void refreshdropdown_Callback(object sender, CallbackEventArgsBase e)
        {
            BindVendorType();
            refreshdropdown.JSProperties.Add("cpSelectedValue", "");
            if (!string.IsNullOrEmpty(Convert.ToString(e.Parameter)))
                refreshdropdown.JSProperties["cpSelectedValue"] = Convert.ToString(e.Parameter);
        }

        protected void popupdelete_Click(object sender, EventArgs e)
        {
            //SPListItem item = null;
            //string key = hdnKeepkeyValue.Contains("KeyValue") ? Convert.ToString(hdnKeepkeyValue.Get("KeyValue")) : string.Empty;

            //if (!string.IsNullOrEmpty(key) && uHelper.StringToInt(key) > 0)
            //{
            //    item = SPListHelper.GetSPListItem(spList, uHelper.StringToInt(key));
            //    if (item != null)
            //    {
            //        item.Recycle();
            //    }
            //    //BindVendorType();
            //}
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context,true);
        }
    }
}
