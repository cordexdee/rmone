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
    public partial class SmtpSetting :UserControl
    {
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
           
            if (!IsPostBack)
            {
               

            }
            string decryptedCredentials = string.Empty;
           // SmtpConfiguration iniobj = new SmtpConfiguration();
            SmtpConfiguration smtpSettings = _context.ConfigManager.GetValueAsClassObj(ConfigConstants.SmtpCredentials,typeof(SmtpConfiguration)) as SmtpConfiguration;
            if (smtpSettings != null && smtpSettings.Host != null)
            {

                decryptedCredentials = uGovernITCrypto.Decrypt(smtpSettings.Password, Constants.UGITAPass);
                txtSmtpServerName.Text = smtpSettings.SmtpFrom;
                txtUserName.Text = smtpSettings.UserName;
                if (string.IsNullOrEmpty(smtpSettings.UserName))
                {
                    chkdefaultCredentials.Checked = true;
                    trusername.Visible = false;
                    trpassword.Visible = false;
                    txtUserName.Visible = false;
                    txtPassword.Visible = false;

                }
                if (txtPassword.TextMode == TextBoxMode.Password)
                {
                    txtPassword.Attributes.Add("value", decryptedCredentials);
                }
                txtPassword.Text = decryptedCredentials;
                txtPortNumber.Text = Convert.ToString(smtpSettings.PortNo);
                chkEnableSSl.Checked = smtpSettings.SSL;
                txtHostName.Text = smtpSettings.Host;

            }
            base.OnInit(e);
        }
            protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void chkEnableSmtpSetting_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SmtpConfiguration smtpConfiguration = new SmtpConfiguration();
            smtpConfiguration.SmtpFrom = txtSmtpServerName.Text.Trim();
            smtpConfiguration.Host = txtHostName.Text.Trim();
            smtpConfiguration.PortNo = UGITUtility.StringToInt(txtPortNumber.Text.Trim());
            smtpConfiguration.SSL = UGITUtility.StringToBoolean(chkEnableSSl.Checked);
            smtpConfiguration.UserName = txtUserName.Text.Trim();
            string encriptedData = uGovernITCrypto.Encrypt(txtPassword.Text.Trim(), Constants.UGITAPass);
            smtpConfiguration.Password = encriptedData;
            if (chkdefaultCredentials.Checked)
            {
                smtpConfiguration.UserName = string.Empty;
                smtpConfiguration.Password = string.Empty;
            }
            //Serialize object and excript it using code
            XmlDocument xmlDoc = uHelper.SerializeObject(smtpConfiguration);
            //Get or create configuration variable object to store data
            ConfigurationVariable cvData = _context.ConfigManager.LoadVaribale(ConfigConstants.SmtpCredentials);
            if (cvData == null)
            {
                cvData = _context.ConfigManager.Save(ConfigConstants.SmtpCredentials, xmlDoc.OuterXml);
            }
            else
            {
                cvData.KeyValue = xmlDoc.OuterXml;
                _context.ConfigManager.Update(cvData);
            }

            Util.Cache.CacheHelper<object>.Clear();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void chkdefaultCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdefaultCredentials.Checked)
            {
                trusername.Visible = false;
                trpassword.Visible = false;
                txtUserName.Visible = false;
                txtPassword.Visible = false;
            }
            else
            {
                trusername.Visible = true;
                trpassword.Visible = true;
                txtUserName.Visible = true;
                txtPassword.Visible = true;
            }

        }
    }
}