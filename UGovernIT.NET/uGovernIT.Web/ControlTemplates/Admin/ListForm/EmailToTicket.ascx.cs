
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
namespace uGovernIT.Web
{
    public partial class EmailToTicket : UserControl
    {
        string servername = string.Empty;
        string ss = string.Empty;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected bool hidecontrol = true;
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_context.ConfigManager.GetValueAsBool(ConfigConstants.EnableEmailToTicket))
                {
                    chkEnableEmailToTicket.Checked = true;
                    hideserverconfiguration.Attributes.Add("style", "display:block");
                    btnSave.Visible = true;
                    btnEnableEmailToTicketSave.Visible = false;
                }
                else
                {
                    hideserverconfiguration.Attributes.Add("style", "display:none");
                    btnSave.Visible = false;
                    btnEnableEmailToTicketSave.Visible = true;
                }
            }
            string decryptedCredentials = string.Empty;
            EmailToTicketConfiguration iniobj = new EmailToTicketConfiguration();
            string credential = _context.ConfigManager.GetValue(ConfigConstants.EmailToTicketCredentials);

            if (!string.IsNullOrWhiteSpace(credential))
            {
                try
                {
                    XmlDocument xmlDocCtnt = new XmlDocument();
                    xmlDocCtnt.LoadXml(credential);
                    iniobj = (EmailToTicketConfiguration)uHelper.DeSerializeAnObject(xmlDocCtnt, iniobj);
                    decryptedCredentials = uGovernITCrypto.Decrypt(iniobj.Password, Constants.UGITAPass);
                }
                catch (Exception ex)
                {
                    decryptedCredentials = string.Empty;
                    Util.Log.ULog.WriteException(ex);
                }
            }
            else
                chkIsDelete.Checked = true;

            if (iniobj != null)
            {
                if (string.IsNullOrEmpty(iniobj.ServerName))
                {
                    servername = Constants.MailServerName;
                    txtServerName.Text = servername;

                }
                else
                    txtServerName.Text = iniobj.ServerName;

                txtUserName.Text = iniobj.UserName;
                if (_context.ConfigManager.GetValueAsBool(ConfigConstants.EmailToTicketUsesOAuth2))
                {
                    hidecontrol = false;
                    txtPassword.Visible = false;
                    dvclientid.Visible = true;
                    dvtenantid.Visible = true;
                    dvsecretid.Visible = true;
                    txtClientId.Text = iniobj.ClientId;
                    txtTenantId.Text = iniobj.TenantId;
                    txtSecretId.Text = iniobj.SecretId;
                }
                if (txtPassword.TextMode == TextBoxMode.Password)
                {
                    txtPassword.Attributes.Add("value", decryptedCredentials);
                }
                txtPassword.Text = decryptedCredentials;
                chkIsDelete.Checked = iniobj.IsDelete;
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmailToTicketConfiguration eobj = new EmailToTicketConfiguration();

            eobj.PortNo = 993;
            eobj.SSL = true;
            eobj.ServerName = txtServerName.Text.Trim();
            eobj.UserName = txtUserName.Text.Trim();
            eobj.ClientId = txtClientId.Text.Trim();
            eobj.TenantId = txtTenantId.Text.Trim();
            eobj.SecretId = txtSecretId.Text.Trim();
            string encriptedData = uGovernITCrypto.Encrypt(txtPassword.Text.Trim(), Constants.UGITAPass);
            eobj.Password = encriptedData;
            eobj.IsDelete = chkIsDelete.Checked;

            //Serialize object and excript it using code
            XmlDocument xmlDoc = uHelper.SerializeObject(eobj);
            // string encriptedData = uGovernITCrypto.Encrypt(xmlDoc.OuterXml, Constants.UGITAPass);

            //Get or create configuration variable object to store data
            ConfigurationVariable cvData = _context.ConfigManager.LoadVaribale(ConfigConstants.EmailToTicketCredentials);
            if (cvData == null)
            {
                cvData = _context.ConfigManager.Save(ConfigConstants.EmailToTicketCredentials, xmlDoc.OuterXml);

            }
            else
            {
                cvData.KeyValue = xmlDoc.OuterXml;
                _context.ConfigManager.Update(cvData);
            }
            //  cvData.Save();

            //for Enable To Ticket config variable
            SaveEnableEnableToTicket();
            //Refresh cache for configuration variable list
            //uGITCache.RefreshList(DatabaseObjects.Lists.ConfigurationVariable);

            //lbMessage.Text = "Saved Successfully";
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnEnableEmailToTicketSave_Click(object sender, EventArgs e)
        {
            try
            {
                //for Enable To Ticket config variable
                SaveEnableEnableToTicket();
                //Refresh cache for configuration variable list
                //  uGITCache.RefreshList(DatabaseObjects.Lists.ConfigurationVariable);

                //lbMessage.Text = "Saved Successfully";
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            catch { }
        }

        protected void chkEnableEmailToTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableEmailToTicket.Checked)
            {
                hideserverconfiguration.Attributes.Add("style", "display:block");
                btnSave.Visible = true;
                btnEnableEmailToTicketSave.Visible = false;
            }
            else
            {
                hideserverconfiguration.Attributes.Add("style", "display:none");
                btnSave.Visible = false;
                btnEnableEmailToTicketSave.Visible = true;
            }
        }

        private void SaveEnableEnableToTicket()
        {
            try
            {
                // ConfigurationVariable cvData = _context.ConfigManager.LoadVaribale(ConfigConstants.EnableEmailToTicket);
                //  cvData.KeyValue = Convert.ToString(chkEnableEmailToTicket.Checked);
                //cvData.Save();
                _context.ConfigManager.Save(ConfigConstants.EnableEmailToTicket, Convert.ToString(chkEnableEmailToTicket.Checked));
                Util.Log.ULog.WriteUGITLog(_context.CurrentUser.Id, $"Update Email to Ticket", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _context.TenantID);
            }
            catch { }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
