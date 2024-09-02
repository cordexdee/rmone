using System;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class RegistrationSuccessm : System.Web.UI.Page
    {
        private ConfigurationVariableManager _configVariableHelper = null;
        private TenantManager _tenantManager { get; set; }
        private ApplicationContext _applicationContext = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isTenantCreated = true;
            _applicationContext = ApplicationContext.Create();
            _configVariableHelper = new ConfigurationVariableManager(_applicationContext);

            var registrationMessage = _configVariableHelper.GetValue(ConfigConstants.RegistrationMessage);
            var tenantConfirmationMessage = _configVariableHelper.GetValue(ConfigConstants.TenantConfirmationMessage);

            var isVerificationEmail = Request["ve"];
            var isCompanyExists = Request["cm"];
            var isEmailExists = Request["em"];
            var companyName = Request["tn"];
            var errorMessage = Request["msg"];

            if (!string.IsNullOrEmpty(errorMessage))
            {                
                HeaderMsg.InnerText = "Alert!";
                BodyMsg.InnerHtml = Convert.ToString(QueryString.Decode(errorMessage));
            }
            else if (!string.IsNullOrEmpty(isVerificationEmail))
            {
                HeaderMsg.InnerText = "Thank You!";

                if ("True".EqualsIgnoreCase(isCompanyExists))
                {
                    BodyMsg.InnerHtml = "This company name  already registered.";
                    isTenantCreated = false;
                }
                if ("True".EqualsIgnoreCase(isEmailExists))
                {
                    BodyMsg.InnerHtml = BodyMsg.InnerHtml + "<br/>" + "This email ID already registered.";
                    isTenantCreated = false;
                }

                if (!string.IsNullOrEmpty(companyName) && isTenantCreated)
                {
                    // move message to configuration
                    // BodyMsg.InnerText = $"Thank you for confirming your email ID. In a short period of time, you will receive a link to start the trial to test {Convert.ToString(companyName)}";
                    //BodyMsg.InnerText = $"Thank you for confirming your email ID. In a short period of time, you will receive a link to start the trial to test Service Prime";
                    BodyMsg.InnerText = tenantConfirmationMessage.Replace("[$companyname$]", companyName);
                }
            }
            else if (!string.IsNullOrEmpty(registrationMessage))
            {
                BodyMsg.InnerHtml = registrationMessage;
            }
        }
    }
}