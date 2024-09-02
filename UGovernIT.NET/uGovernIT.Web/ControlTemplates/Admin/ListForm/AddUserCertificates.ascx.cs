using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class AddUserCertificates : UserControl
    {
        public int certificateID { get; set; }
        List<UserCertificates> spUserCertificateList;
        UserCertificates spitem;
        UserCertificateManager userCertificateManager = new UserCertificateManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            spUserCertificateList = userCertificateManager.Load();
            BindUserCertificateCategory();
            if (certificateID == 0)
            {
                spitem = new UserCertificates();
                txtUserCertificate.Text = "";
                txtDescription.Text = "";
                lnkDelete.Visible = false;
            }
            else
            {
                spitem = userCertificateManager.LoadByID(certificateID);
                if (spitem != null)
                {
                    txtUserCertificate.Text = spitem.Title;
                    txtDescription.Text = spitem.Description;
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(Convert.ToString(spitem.CategoryName)));
                    lnkDelete.Visible = true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (ValidateCertificate())
            {
                spitem.Title = txtUserCertificate.Text;
                spitem.Description = txtDescription.Text;
                if (ddlCategory.SelectedIndex > 0)
                {
                    spitem.CategoryName = ddlCategory.SelectedItem.Text;
                }
                else
                {
                    spitem.CategoryName = txtCategory.Text;
                }
                userCertificateManager.Save(spitem);

                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated user Certificate: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private Boolean ValidateCertificate()
        {
            List<UserCertificates> collection = userCertificateManager.Load(x => x.ID != certificateID && x.Title == txtUserCertificate.Text);
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Certificate is already in the list";
                return false;
            }
            else
            { return true; }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            spitem = userCertificateManager.LoadByID(certificateID);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted user Certificate: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            userCertificateManager.Delete(spitem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void csvdivCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool argsval = false;
            if (ddlCategory.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtCategory.Text) && hdnCategory.Value == "1")
                {
                    divddlCategory.Attributes.Add("style", "display:none");
                    divCategory.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(txtCategory.Text))
                {
                    argsval = true;
                }
                args.IsValid = argsval;
            }
            else
            {
                divddlCategory.Attributes.Add("style", "display:block");
                divCategory.Attributes.Add("style", "display:none");
            }
        }

        private void BindUserCertificateCategory()
        {
            if (spUserCertificateList.Count > 0 && spUserCertificateList != null)
            {
                IEnumerable<object> itemsDistinct = spUserCertificateList
                                .Select(item => item.CategoryName)
                                .Distinct();
                using (IEnumerator<object> enumerator = itemsDistinct.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current != null)
                            ddlCategory.Items.Add(new ListItem(Convert.ToString(enumerator.Current)));
                    }
                }

                ddlCategory.Items.Insert(0, new ListItem("None", "0"));
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}