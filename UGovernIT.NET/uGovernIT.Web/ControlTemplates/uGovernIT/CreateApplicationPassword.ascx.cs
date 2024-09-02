using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Web
{
    public partial class CreateApplicationPassword : UserControl
    {
        public int ApplicationId { get; set; }
        public string ticketID { get; set; }
        public int Id { get; set; }
        ApplicationPasswordManager appPwdManager;
        ApplicationContext context;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            appPwdManager = new ApplicationPasswordManager(context);
            if (!IsPostBack)
            {
                bool isExists = false;
                if (ApplicationId > 0)
                {
                    DataRow spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, ApplicationId))[0];

                   // DataRow spItem =Get SPListHelper.GetSPListItem(DatabaseObjects.Table.Applications, ApplicationId);
                    isExists = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(),spItem,HttpContext.Current.CurrentUser());
                    if (isExists == false)
                    {
                        //SPUtility.TransferToErrorPage("Access denied.");
                        return;
                    }
                }

                if (Id > 0)
                {
                    chkShowPassword.Visible = true;
                    EditPassword();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string key = string.Empty;
            lblErrorMesage.Style.Add("display", "none");
            string userName = string.Empty;
            key = ticketID + "-" + txtUserName.Text;
            userName = txtUserName.Text;
            DataTable spList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationPassword, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            ApplicationPasswordEntity spitem = null;            
         
            string historyDescription = string.Empty;
            DataRow spItemApplication = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, ApplicationId))[0];
            if (Id > 0)
            {
                spitem = appPwdManager.LoadByID(Id);
                historyDescription = @" Password updated for user " + userName + " application id " + ticketID;
            }
            else if (!CheckUserPassword(userName, spList))
            {
                spitem = new ApplicationPasswordEntity();
                historyDescription = @" Password created for user " + userName + " application id " + ticketID;
            }

            if (spitem != null)
            {
                spitem.Description= txtDescription.Text.Trim();
                spitem.APPTitleLookup = ApplicationId;
                spitem.Password = txtPassword.Text;
                spitem.APPUserName = userName;
                spitem.APPPasswordTitle= txtTitle.Text.Trim(); ;
                string encryptedPassword = uGovernITCrypto.Encrypt(txtPassword.Text.Trim(), key);
                spitem.EncryptedPassword = encryptedPassword;
                if (spitem.ID > 0)
                    appPwdManager.Update(spitem);
                else
                    appPwdManager.Insert(spitem);
                if (!string.IsNullOrEmpty(historyDescription))
                {
                    uHelper.CreateHistory(context.CurrentUser, historyDescription, spItemApplication,context);
                }
                uHelper.ClosePopUpAndEndResponse(Context);
            }
            else
            {
                lblErrorMesage.Text = "User already exists";
                lblErrorMesage.Style.Add("display", "block");
            }

          
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void ChkShowPassword_Click(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked == true)
            {
                txtPassword.TextMode = TextBoxMode.SingleLine;
            }
            else
            {
                txtPassword.TextMode = TextBoxMode.Password;
            }
        }

        public bool CheckUserPassword(string UserName, DataTable spList)
        {
            bool isExists = false;
           string spQuery = string.Format("{0}={1} and {2}='{3}'", 
                                            DatabaseObjects.Columns.APPTitleLookup, ApplicationId, DatabaseObjects.Columns.APPUserName, UserName.Trim());
            DataRow[] spListItemColl = spList.Select(spQuery); // Don't use viewfields here - crashes!
            if (spListItemColl != null && spListItemColl.Count() > 0)
                isExists = true;

            return isExists;
        }

        private void EditPassword()
        {
            ApplicationPasswordEntity spitem = appPwdManager.LoadByID(Id);
            if (spitem != null)
            {
             
                txtDescription.Text = Convert.ToString(spitem.Description);
                txtTitle.Text = Convert.ToString(spitem.APPPasswordTitle);
                txtPassword.Attributes["value"] = Convert.ToString(spitem.Password);
                txtUserName.Text = Convert.ToString(spitem.APPUserName);
            }
        }
    }
}
