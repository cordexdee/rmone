/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.Graph;
using System.Net.Http.Headers;
using System;
using System.Web;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public class SDKHelper
    {

        // Get an authenticated Microsoft Graph Service client.
        [Obsolete]
        public static GraphServiceClient GetAuthenticatedClient()
        {
            string accessToken = string.Empty;
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        try
                        {
                             accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                            accessToken = Convert.ToString(HttpContext.Current.Session["OpenOffice_Token"]);
                           
                        }
                        

                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                        // Get event times in the current time zone.
                        requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");

                        // This header has been added to identify our sample in the Microsoft Graph service. If extracting this code for your project please remove.
                        requestMessage.Headers.Add("SampleID", "aspnet-snippets-sample");
                    }));
            return graphClient;
        }

    }
}