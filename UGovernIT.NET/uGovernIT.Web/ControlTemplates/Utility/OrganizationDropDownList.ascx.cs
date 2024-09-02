
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class OrganizationDropDownList : UserControl
    {
        public string Value { get; set; }
        public ControlMode ControlMode { get; set; }
        public bool IsMandatory { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationGroup { get; set; }
        public bool ShowSearch { get; set; }
        public bool ShowAdd { get; set; }

        public string AssemblyVersion = string.Empty;
        public void SetValue(string value)
        {
            Value = value;
            if (!string.IsNullOrEmpty(Value))
            {
                if (Value.Contains(Constants.Separator))
                {
                    ddlOrg.SelectedIndex = ddlOrg.Items.IndexOf(ddlOrg.Items.FindByValue(UGITUtility.GetLookupID(Value).ToString()));
                }
                else
                {
                    ddlOrg.SelectedIndex = ddlOrg.Items.IndexOf(ddlOrg.Items.FindByValue(Value));
                }
            }
        }

        public string GetValue() {

            string val = string.Empty;
            if (ddlOrg.SelectedItem != null)
            {
                val = ddlOrg.SelectedItem.Value.ToString();
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
            ddlOrg.Items.Clear();
            OrganizationManager organizationManager = new OrganizationManager(HttpContext.Current.GetManagerContext());
            List<Organization> dtOrg = organizationManager.Load();
            if (dtOrg.Count > 0)
            {
                ddlOrg.DataSource = dtOrg;
                ddlOrg.TextField = DatabaseObjects.Columns.LegalName;
                ddlOrg.ValueField = DatabaseObjects.Columns.Id;
                ddlOrg.DataBind();
            }
            SetValue(Value);

            if (ControlMode == ControlMode.Display)
            {
                lblOrg.Text = ddlOrg.Text;
                pnlOrg.Visible = false;
                lblOrg.Visible = true;
            }
        }
        private void LoadPopupControl()
        {
            //OrganizationAddEdit Ctrl = (OrganizationAddEdit)LoadControl("~/_controltemplates/15/ListForm/OrganizationAddEdit.ascx");
            //Ctrl.ID = "OrganizationAddEdit";
            //Ctrl.FromCtrl = true;
            //pnlOrgPopup.Controls.Add(Ctrl);
            //Ctrl.ClosePupup += Ctrl_ClosePupup;
        }
        void Ctrl_ClosePupup(int orgID)
        {
            SetValue(orgID.ToString());
            OrgAddNewPopUp.ShowOnPageLoad = false;
        }
    }
}
