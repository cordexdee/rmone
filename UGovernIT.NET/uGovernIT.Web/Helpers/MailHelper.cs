using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public class MailHelper
    {
        public static string GetFromEmailId(bool SendMailFromLoggedInUser = false)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            if (SendMailFromLoggedInUser == true)
            {
                return context.CurrentUser.Email;
            }
            else
            {
                SmtpConfiguration smtpSettings = context.ConfigManager.GetValueAsClassObj(ConfigConstants.SmtpCredentials, typeof(SmtpConfiguration)) as SmtpConfiguration;
                if (smtpSettings != null && !string.IsNullOrEmpty(smtpSettings.SmtpFrom))
                {
                    return smtpSettings.SmtpFrom;
                }
            }

            return string.Empty;
        }
    }
}
