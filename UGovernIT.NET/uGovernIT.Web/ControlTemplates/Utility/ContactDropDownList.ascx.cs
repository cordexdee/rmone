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
    public partial class ContactDropDownList : UserControl
    {
        public string Value { get; set; }
        public string ControlMode { get; set; }
        public bool IsMandatory { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationGroup { get; set; }
        public bool ShowSearch { get; set; }
        public bool ShowAdd { get; set; }
        ContactManager ObjContactManager = new ContactManager(HttpContext.Current.GetManagerContext());
        public string AssemblyVersion = string.Empty;
        public void SetValue(string value)
        {
            Value = value;
            if (!string.IsNullOrEmpty(Value))
            {
                if (Value.Contains(Constants.Separator))
                {
                    ddlContacts.SelectedIndex = ddlContacts.Items.IndexOf(ddlContacts.Items.FindByValue(UGITUtility.GetLookupID(Value).ToString()));
                }
                else
                {
                    ddlContacts.SelectedIndex = ddlContacts.Items.IndexOf(ddlContacts.Items.FindByValue(Value));
                }
            }
        }

        public string GetValue()
        {

            string val = string.Empty;
            if (ddlContacts.SelectedItem != null)
            {
                val = ddlContacts.SelectedItem.Value.ToString();
            }
            return val;
        }

        protected override void OnInit(EventArgs e)
        {
            ShowAdd = true;
            ShowSearch = true;
            lnkAdd.Visible = ShowAdd;
            lnkSearch.Visible = ShowSearch;
            LoadPopupControl();
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            BindDDL();
        }
        private void BindDDL()
        {
            ddlContacts.Items.Clear();
            List<Contact> dtContacts = ObjContactManager.Load();
            if (dtContacts!=null)
            {
                
                //dtContacts.DefaultView.Sort = DatabaseObjects.Columns.UGITFirstName + " ASC";
                ddlContacts.DataSource = dtContacts;
                ddlContacts.TextField = DatabaseObjects.Columns.UGITFirstName;
                ddlContacts.ValueField = DatabaseObjects.Columns.Id;
                ddlContacts.DataBind();
            }
            SetValue(Value);
            //if (ControlMode == SPControlMode.Display)
            //{
            //    lblContact.Text = ddlContacts.Text;
            //    pnlContact.Visible = false;
            //    lblContact.Visible = true;
            //}
        }
        private void LoadPopupControl()
        {
            //ContactAddEdit Ctrl = (ContactAddEdit)LoadControl("~/_controltemplates/15/ListForm/ContactAddEdit.ascx");
            //Ctrl.ID = "ContactAddEdit";
            //Ctrl.FromCtrl = true;
            //pnlContactPopup.Controls.Add(Ctrl);
            //Ctrl.ClosePupup += Ctrl_ClosePupup;
        }
        void Ctrl_ClosePupup(int contactID)
        {
            SetValue(contactID.ToString());
            ContactAddNewPopup.ShowOnPageLoad = false;
        }
    }
}
