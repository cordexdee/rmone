using System;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class AutomateUserInfo : UserControl
    {
        public bool IsUserLimitExceed = false;

        private ApplicationContext _context = null;
        private TenantValidation _tenantValidation = null;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected TenantValidation TenantValidation
        {
            get
            {
                if (_tenantValidation == null)
                {
                    _tenantValidation = new TenantValidation(ApplicationContext);
                }
                return _tenantValidation;
            }
        }
        protected string ajaxPageURL;

        protected override void OnInit(EventArgs e)
        {
            IsUserLimitExceed = TenantValidation.IsUserLimitExceed();


        }


        protected void Page_Load(object sender, EventArgs e)
        {

            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/userinfo/");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

    }
}