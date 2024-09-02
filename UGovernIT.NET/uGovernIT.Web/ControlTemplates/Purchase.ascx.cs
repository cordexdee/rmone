using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using static uGovernIT.Utility.Enums;

namespace uGovernIT.Web
{
    public partial class Purchase : System.Web.UI.UserControl
    {
        public TenantManager tenantManager { get; set; }
        public ApplicationContext _context = null;
        public ApplicationContext _contextTenant = null;
        public Tenant tenant { get; set; }
        public ConfigurationVariableManager ObjConfigVariable = null;
        public string oncanceldir = UGITUtility.GetAbsoluteURL("~/Admin/NewAdminUI.aspx");

        public Purchase()
        {
            _context = HttpContext.Current.GetManagerContext();
            tenantManager = new TenantManager(_context);
            tenant = new Tenant();
            


        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void btnpurchase_Click(object sender, EventArgs e)
        {

            tenant = tenantManager.GetTenantById(_context.TenantID);
            
            if (tenant != null)
            {
                _contextTenant = ApplicationContext.Create();
                
                tenant.Subscription =(int) SubscriptionType.SimpleUser;
                var result = tenantManager.UpdateItemCommon(tenant);
                if(result == 1)
                {
                    lblmsg.Visible = true;

                    ObjConfigVariable = new ConfigurationVariableManager(_contextTenant);
                    string forwardMailAddress = ObjConfigVariable.GetValue(ConfigConstants.ForwardMailAddress);

                    if (!string.IsNullOrEmpty(forwardMailAddress))
                    {
                        var lstforwardMailAddress = forwardMailAddress.Split(',');
                        foreach (string strMail in lstforwardMailAddress)
                        {
                            var response = new EmailHelper(_context).SendEmailForPurchaseNotification(strMail, HttpContext.Current.CurrentUser().AccountId);
                        }
                    }
                    else
                    {
                        //var response = new EmailHelper(_context).SendEmailToTenantAdminAccount(HttpContext.Current.User, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                    }
                }
                else
                {

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/NewAdminUI.aspx");
        }
    }
}