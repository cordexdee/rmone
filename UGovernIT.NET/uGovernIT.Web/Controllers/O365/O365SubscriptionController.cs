using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using uGovernIT.DMS.Amazon;

using uGovernIT.Manager;
using System.Web.Script.Serialization;

namespace uGovernIT.Web
{
    public class O365SubscriptionController : Controller
    {

        ApplicationContext context = null;
        string userId = string.Empty;
        DMSDocRepPermissionManagerService permissionService = null;

        public O365SubscriptionController()
        {

            context = System.Web.HttpContext.Current.GetManagerContext();
            userId = context.CurrentUser.Id;
            permissionService = new DMSDocRepPermissionManagerService(context);
        }
       
    }
}