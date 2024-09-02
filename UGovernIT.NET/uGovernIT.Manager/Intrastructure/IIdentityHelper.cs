using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager
{
    public static class IIdentityHelper
    {
        public static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
                return string.Empty;

            var nameObj = (identity as ClaimsIdentity).FindFirst(ClaimTypes.GivenName);
            if (nameObj != null)
            {
                return string.Format("{0}", nameObj.Value);
            }
            return string.Empty;
        }

        public static string GetEmail(this IIdentity identity)
        {

            if (identity == null)
                return string.Empty;

            var emailObj = (identity as ClaimsIdentity).FindFirst(ClaimTypes.Email);
            if (emailObj != null)
            {
                return string.Format("{0}", emailObj.Value);
            }
            return string.Empty;
        }
    }
 }
