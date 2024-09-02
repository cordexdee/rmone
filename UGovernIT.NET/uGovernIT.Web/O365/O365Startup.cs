/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(uGovernIT.Web.Startup))]

namespace uGovernIT.Web
{
    public partial class O365Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //uGovernIT.DMS.Helpers.Startup test = new DMS.Helpers.Startup();
            //test.ConfigureAuth(app);
            ConfigureAuth(app);
        }
    }
}
