using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class MoveStageToProduction : UserControl
    {
        #region Stage to Production

        // Destination site credentials, leave empty if using integrated authentication
      //  private static string destinationDomain = "mx2";
        //TransferConfig config;
        //UGITTransfer transf;
        #endregion

        #region Properties
        public string ModuleName { get; set; }
        public string ListName { get; set; }
        public string ServiceId { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Convert.ToString(Request["module"])))
                chkAllModule.Visible = true;

            chkAllServices.Visible = UGITUtility.StringToBoolean(Convert.ToString(Request["isService"]));

            lblDifference.Text = "do you want to migrate to target?";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void btnMoveToProduction_Click(object sender, EventArgs e)
        {
            //config = GetConfig();
            //transf = new UGITTransfer(config);
            //if (!string.IsNullOrEmpty(ModuleName))
            //    transf.ModuleName = ModuleName.Split(',').ToList();
            //transf.All = (chkAllModule.Checked || chkAllServices.Checked) == true ? true : false;
            //if (!string.IsNullOrEmpty(ServiceId))
            //    transf.ServiceId = ServiceId.Split(',').ToList();

            //if (!string.IsNullOrWhiteSpace(ListName))
            //    transf.StartTransfer(ListName);
        }

        //public static TransferConfig GetConfig()
        //{
        //    TransferConfig config = new TransferConfig();

        //    string decryptedCredentials = string.Empty;
        //    MigrateConfiguration iniobj = new MigrateConfiguration();
        //    string credential = ConfigurationVariable.GetValue(SPContext.Current.Web, ConfigConstants.EnableMigrateCredential);

        //    if (!string.IsNullOrWhiteSpace(credential))
        //    {
        //        try
        //        {
        //            XmlDocument xmlDocCtnt = new XmlDocument();
        //            xmlDocCtnt.LoadXml(credential);
        //            iniobj = (MigrateConfiguration)uHelper.DeSerializeAnObject(xmlDocCtnt, iniobj);
        //            decryptedCredentials = uGovernITCrypto.Decrypt(iniobj.Password, Constants.UGITAPass);
        //        }
        //        catch (Exception ex)
        //        {
        //            decryptedCredentials = string.Empty;
        //            Log.WriteException(ex);
        //        }
        //    }

        //    // Destination site credentials
        //    SiteAuthentication toAuth = new SiteAuthentication();
        //    if (iniobj != null)
        //    {
        //        toAuth.SiteUrl = iniobj.Target;

        //        toAuth.UserName = iniobj.UserName;

        //        toAuth.Password = decryptedCredentials;
        //    }

        //    config.DestinationAuth = toAuth;

        //    Log.WriteLog("Transferring data from " + SPContext.Current.Web.Url + " => " + toAuth.SiteUrl);
        //    return config;
        //}

    }
}
