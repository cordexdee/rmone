using Microsoft.AspNet.FriendlyUrls.Resolvers;
using System.Web;

namespace uGovernIT.Web.Helpers
{
    public class WebMethodFriendlyUrlResolver : WebFormsFriendlyUrlResolver
    {
        public override string ConvertToFriendlyUrl(string path)
        {
            if (HttpContext.Current.Request.PathInfo != string.Empty)
            {
                return path;
            }
            else
            {
                return base.ConvertToFriendlyUrl(path);
            }
        }
    }
}