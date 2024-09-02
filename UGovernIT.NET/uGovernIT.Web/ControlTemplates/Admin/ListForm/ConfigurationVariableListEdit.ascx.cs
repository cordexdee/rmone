using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ConfigurationVariableListEdit : UserControl
    {
        string sScript = "<script>";
        string eScript = "</script>";

        public int clientAdminID { get; set; }

       
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";

        ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        UserProfile user;

        UserProfileManager userManager;

        ConfigurationVariable ObjConfigVariable;


        protected override void OnInit(EventArgs e)
        {        
            string keyValue = string.Empty;
            string TypeValue = string.Empty;
            int cleintadminid;
            int.TryParse(Request["ID"], out cleintadminid);
            clientAdminID = cleintadminid;
            BindCategoryDDL();
            // For edit mode
            if (!IsPostBack && clientAdminID > 0)
            {
                    lnkDelete.Visible = true;
                    ObjConfigVariable = ObjConfigurationVariableManager.LoadByID(clientAdminID);
                    if (ObjConfigVariable != null)
                    {
                        categoryDD.SelectedIndex = categoryDD.Items.IndexOf(categoryDD.Items.FindByValue(ObjConfigVariable.CategoryName));
                        if (!string.IsNullOrEmpty(ObjConfigVariable.Description))
                            txtDescription.Text = ObjConfigVariable.Description;
                        txtkeyName.Text = ObjConfigVariable.KeyName;
                        keyValue = ObjConfigVariable.KeyValue;
                        TypeValue = ObjConfigVariable.Type;
                        //rdbKeyvalue.Items.FindByValue(TypeValue);
                        rdbKeyvalue.SelectedIndex = rdbKeyvalue.Items.IndexOf(rdbKeyvalue.Items.FindByText(TypeValue));
                        if (rdbKeyvalue.SelectedIndex < 0)
                        {
                            // By defaul key value show in Text type
                            rdbKeyvalue.SelectedIndex = 1;
                            txtText.Text = keyValue;
                        }

                        LoadControlByType(TypeValue, keyValue);
                    }
                }
        }

        private void LoadControlByType(string keyType, string keyValue)
        {
            DateTime validDate;
            DateTime.TryParse(keyValue, out validDate);
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            user = HttpContext.Current.CurrentUser();

            if (keyType == Constants.ConfigVariableType.Date)
            {
                dateValueType.Value = validDate;
                trDate.Attributes.Add("style", "display:block");
                truser.Attributes.Add("style", "display:none");
                trBool.Attributes.Add("style", "display:none");
                trText.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:none");
                trPassword.Attributes.Add("style", "display:none");
            }
            else if (keyType == Constants.ConfigVariableType.Bool)
            {

                trDate.Attributes.Add("style", "display:none");
                truser.Attributes.Add("style", "display:none");
                trBool.Attributes.Add("style", "display:block");
                trText.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:none");
                if (keyValue.ToLower().Contains("true"))
                {
                    rdbBool.SelectedIndex = 0;
                }
                else
                {
                    rdbBool.SelectedIndex = 1;
                }
                trPassword.Attributes.Add("style", "display:none");

            }
            // Key value is Type of User
            else if (keyType == Constants.ConfigVariableType.User)
            {

                pEditorUser.SetValues(ObjConfigVariable.KeyValue);
                trDate.Attributes.Add("style", "display:none");
                truser.Attributes.Add("style", "display:block");
                trBool.Attributes.Add("style", "display:none");
                trText.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:none");
                trPassword.Attributes.Add("style", "display:none");

            }
            // Key value is Type of Attach
            else if (keyType == Constants.ConfigVariableType.Attachments)
            {
                //string attach = Convert.ToString(dtConfigVariable.Rows[0][DatabaseObjects.Columns.KeyValue]);
                //txtFile.Text = attach;
                UGITFileUploadManager1.SetImageUrl(ObjConfigVariable.KeyValue);
                trDate.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:block");
                trBool.Attributes.Add("style", "display:none");
                trText.Attributes.Add("style", "display:none");
                truser.Attributes.Add("style", "display:none");
                trPassword.Attributes.Add("style", "display:none");

            }
            // Key value is Type of Password
            else if (keyType == Constants.ConfigVariableType.Password)
            {
                string pwd = Convert.ToString(ObjConfigVariable.KeyValue);
                txtPassword.Text = pwd;
                trDate.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:none");
                trBool.Attributes.Add("style", "display:none");
                trText.Attributes.Add("style", "display:none");
                truser.Attributes.Add("style", "display:none");
                trPassword.Attributes.Add("style", "display:block");

            }
            //By Deafault Key value is Type of Text
            else
            {
                //var isSuperadmin = userManager.IsUGITSuperAdmin(user);
                if (ObjConfigVariable.KeyName.EqualsIgnoreCase(ConfigConstants.NumberOfFreeUserAccounts))
                {

                    if (!userManager.IsUGITSuperAdmin(user))
                    {
                        txtText.ReadOnly = true;
                    }
                }


                 txtText.Text = Convert.ToString(ObjConfigVariable.KeyValue);
                    trDate.Attributes.Add("style", "display:none");
                    truser.Attributes.Add("style", "display:none");
                    trBool.Attributes.Add("style", "display:none");
                    trText.Attributes.Add("style", "display:block");
                    trattach.Attributes.Add("style", "display:none");
                    trPassword.Attributes.Add("style", "display:none");

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // To bind Javascript with RadioButton list
            foreach (ListItem li in rdbKeyvalue.Items)
            {
                li.Attributes.Add("onclick", "getIndex('" + li.Text + "');");
            }
            //string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIK", string.Empty, "WikiHelp", txtFile.ClientID));
           // aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            // For new config variable entry.
            if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "LstPickerSelectedValue")))
            {
               // txtFile.Text = string.Format("/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={0}", UGITUtility.GetCookieValue(Request, "LstPickerSelectedValue"));
                rdbKeyvalue.SelectedIndex = 4;
                UGITUtility.DeleteCookie(Request, Response, "LstPickerSelectedValue");
                trDate.Attributes.Add("style", "display:none");
                trattach.Attributes.Add("style", "display:block");
                trBool.Attributes.Add("style", "display:none");
                trText.Attributes.Add("style", "display:none");
                truser.Attributes.Add("style", "display:none");
            }
        }

        /// <summary>
        /// To Bind Category Dropdown
        /// </summary>
        protected void BindCategoryDDL()
        {
            List<string> listCategory = ObjConfigurationVariableManager.Load().Where(x => x.CategoryName != null).OrderBy(x => x.CategoryName).Select(x => x.CategoryName).Distinct().ToList(); // where condition added to ignore smtp settings & category with null values
            categoryDD.DataSource = listCategory;
            //categoryDD.DataTextField = DatabaseObjects.Columns.CategoryName;
            //categoryDD.DataValueField = DatabaseObjects.Columns.CategoryName;
            categoryDD.DataBind();
            categoryDD.Items.Insert(0, new ListItem("<Choose Later>", "0"));
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btSave_Click(object sender, EventArgs e)
        {

            ObjConfigVariable = ObjConfigurationVariableManager.LoadByID(clientAdminID);
            if (ObjConfigVariable == null)
                ObjConfigVariable = new ConfigurationVariable();
            if (!Page.IsValid)
                return;
            if (categoryDD.SelectedIndex > 0)
            {
               ObjConfigVariable.CategoryName= categoryDD.SelectedItem.Text;
            }
            else
            {
               ObjConfigVariable.CategoryName= txtCategory.Text;
            }

           ObjConfigVariable.Description= txtDescription.Text.Trim();
           ObjConfigVariable.KeyName= txtkeyName.Text.Trim();
           ObjConfigVariable.Title= txtkeyName.Text.Trim();
            
            // if key value is Type of Boolean
            if (rdbKeyvalue.SelectedValue == Constants.ConfigVariableType.Bool)
            {
               ObjConfigVariable.KeyValue= rdbBool.SelectedItem.Text;
               ObjConfigVariable.Type= Constants.ConfigVariableType.Bool;
            }
            // if key value is Type of Date
            else if (rdbKeyvalue.SelectedValue == Constants.ConfigVariableType.Date)
            {
               ObjConfigVariable.KeyValue= Convert.ToString(dateValueType.Value);
               ObjConfigVariable.Type= Constants.ConfigVariableType.Date;
            }
            // if key value is Type of User
            else if (rdbKeyvalue.SelectedValue == Constants.ConfigVariableType.User)
            {
               ObjConfigVariable.KeyValue= pEditorUser.GetValues();
               ObjConfigVariable.Type= Constants.ConfigVariableType.User;
            }
            /// if key value is type of attach
            else if (rdbKeyvalue.SelectedValue == Constants.ConfigVariableType.Attachments)
            {
                ObjConfigVariable.KeyValue= UGITFileUploadManager1.GetImageUrl();
                ObjConfigVariable.Type= Constants.ConfigVariableType.Attachments;
            }
            else if (rdbKeyvalue.SelectedValue == Constants.ConfigVariableType.Password)
            {
                string encryptedCredential = uGovernITCrypto.Encrypt(txtPassword.Text, Constants.UGITAPass);
                if (encryptedCredential != string.Empty)
                {
                    ObjConfigVariable.KeyValue= encryptedCredential;
                    ObjConfigVariable.Type = Constants.ConfigVariableType.Password;

                }
            }
            else
            {
                // To Replace starting and closing Script
                if (txtText.Text.ToLower().Contains(sScript) || txtText.Text.ToLower().Contains(eScript))
                {
                    txtText.Text = txtText.Text.Replace(sScript, " ");
                    txtText.Text = txtText.Text.Replace(eScript, " ");
                }
                ObjConfigVariable.KeyValue= txtText.Text.Trim();
                ObjConfigVariable.Type= Constants.ConfigVariableType.Text;
            }
            if (clientAdminID > 0)
            {
                ObjConfigurationVariableManager.Update(ObjConfigVariable);
                //uHelper.ClosePopUpAndEndResponse(Context, true);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Configuration Variable, {ObjConfigVariable.KeyName} to {ObjConfigVariable.KeyValue}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            }
            else
            {
               ObjConfigurationVariableManager.Insert(ObjConfigVariable);
                //uHelper.ClosePopUpAndEndResponse(Context, true);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Configuration Variable, {ObjConfigVariable.KeyName} to {ObjConfigVariable.KeyValue}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            }
            //Util.Cache.CacheHelper<object>.Clear();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvTxtCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validate category name 
            if (categoryDD.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtCategory.Text))
                {
                    args.IsValid = false;
                    newCategoryContainer.Attributes.Add("style", "display:block");
                }
            }
        }

        protected void cvText_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validate key valye as  Text Type
            if (rdbKeyvalue.SelectedIndex == 1)
            {
                if (string.IsNullOrEmpty(txtText.Text))
                {
                    args.IsValid = false;
                    trDate.Attributes.Add("style", "display:none");
                    truser.Attributes.Add("style", "display:none");
                    trBool.Attributes.Add("style", "display:none");
                    trText.Attributes.Add("style", "display:block");
                    trattach.Attributes.Add("style", "display:none");
                }
            }

        }

        //protected void cvfileUpload_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    /// validate key value as attachment type.
        //    if (rdbKeyvalue.SelectedIndex == 4)
        //    {
        //        //if (!fileUpload.HasFile && string.IsNullOrEmpty(txtFile.Text))
        //        //{
        //        //    args.IsValid = false;
        //        //    trDate.Attributes.Add("style", "display:none");
        //        //    truser.Attributes.Add("style", "display:none");
        //        //    trBool.Attributes.Add("style", "display:none");
        //        //    trText.Attributes.Add("style", "display:none");
        //        //    trattach.Attributes.Add("style", "display:block");
        //        //}
        //    }
        //}

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (clientAdminID > 0)
                {
                    ObjConfigVariable = ObjConfigurationVariableManager.LoadByID(clientAdminID);
                    ObjConfigurationVariableManager.Delete(ObjConfigVariable);
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Configuration Variable, {ObjConfigVariable.KeyName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }
    }
}
