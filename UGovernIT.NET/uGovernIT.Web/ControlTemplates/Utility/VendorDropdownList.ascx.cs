using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class VendorDropdownList : UserControl
    {
        public string Value { get; set; }
        public ControlMode ControlMode { get; set; }
        public bool IsMandatory { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationGroup { get; set; }
        public bool ShowSearch { get; set; }
        public bool ShowAdd { get; set; }
        public string AssemblyVersion = string.Empty;
        //AssetVendorsEdit AssetVendorsEditControl;

        public int SelectedVendorId { get; set; }

        public void SetValue(string value)
        {
            Value = value;
            if (!string.IsNullOrEmpty(Value))
            {
                if (Value.Contains(Constants.Separator))
                {
                    ddlVendor.SelectedIndex = ddlVendor.Items.IndexOf(ddlVendor.Items.FindByValue(UGITUtility.GetLookupID(Value).ToString()));
                    hdnVendorid.Value = UGITUtility.GetLookupID(Value).ToString();
                }
                else
                {
                    ddlVendor.SelectedIndex = ddlVendor.Items.IndexOf(ddlVendor.Items.FindByValue(Value));
                    hdnVendorid.Value = Value;
                }
            }
        }

        public string GetValue()
        {

            string val = string.Empty;
            if (ddlVendor.SelectedItem != null)
            {
                val = ddlVendor.SelectedItem.Value.ToString();
            }
            return val;
        }


        protected override void OnInit(EventArgs e)
        {
            ShowAdd = true;
            ShowSearch = true;
            lnkAdd.Visible = ShowAdd;
            lnkSearch.Visible = ShowSearch;
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadPopupControl();
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            BindDDL();
        }
        private void BindDDL()
        {
            ddlVendor.Items.Clear();
            AssetVendorViewManager assetVendorViewManager = new AssetVendorViewManager(HttpContext.Current.GetManagerContext());
            List<AssetVendor> list = assetVendorViewManager.Load().OrderBy(x=>x.Title).ToList();
            if (list.Count > 0)
            {                
                ddlVendor.DataSource = list;
                ddlVendor.TextField = DatabaseObjects.Columns.Title;
                ddlVendor.ValueField = DatabaseObjects.Columns.Id;
                ddlVendor.DataBind();
            }
            SetValue(Value);

            if (ControlMode == ControlMode.Display)
            {
                lblOrg.Text = ddlVendor.Text;
                pnlOrg.Visible = false;
                lblOrg.Visible = true;
            }
        }
        private void LoadPopupControl()
        {
            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (Request.Form["__CALLBACKPARAM"].ToString().Contains("VENDORID"))
                {
                    if (val.Length > 1)
                    {
                        SelectedVendorId = Convert.ToInt32(val[val.Length - 1].Replace(";", string.Empty));
                        hdnVendorid.Value = val[val.Length - 1].Replace(";", string.Empty);
                    }
                }
            }
            if (!string.IsNullOrEmpty(Request[hdnVendorid.UniqueID]))
            {
                SelectedVendorId = Convert.ToInt32(Request[hdnVendorid.UniqueID]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(ddlVendor.Value)))
            {
                SelectedVendorId = Convert.ToInt32(ddlVendor.Value);
            }

            //AssetVendorsEditControl = (AssetVendorsEdit)LoadControl("~/_controltemplates/15/ListForm/AssetVendorsEdit.ascx");
            //AssetVendorsEditControl.ID = "AssetVendorsEdit";
            //AssetVendorsEditControl.FromCtrl = true;
            //AssetVendorsEditControl.Id = SelectedVendorId;
            //pnlOrgPopup.Controls.Add(AssetVendorsEditControl);
            //AssetVendorsEditControl.ClosePupup += Ctrl_ClosePupup;
            
        }
        void Ctrl_ClosePupup(int orgID)
        {
            SetValue(orgID.ToString());
            VendorAddNewPopUp.ShowOnPageLoad = false;
        }

        protected void VendorAddNewPopUp_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            //if (e.Parameter == "ADDVENDOR")
            //{
            //    AssetVendorsEditControl.ClearForm();
            //}
            //else
            //{
            //    var vendorid = Convert.ToInt32(ddlVendor.Value);
            //    AssetVendorsEditControl.Fill(vendorid);
            //}
        }
    }
}
