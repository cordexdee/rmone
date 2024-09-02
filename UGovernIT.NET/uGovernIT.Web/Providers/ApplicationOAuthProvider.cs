using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using uGovernIT.Manager;
using uGovernIT.Web.Models;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }
        //public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{
        //    await Task.FromResult(context.Validated());          
        //}
        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    await Task.FromResult(context.Validated(new ClaimsIdentity(context.Options.AuthenticationType)));
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<UserProfileManager>();
            string tenantAcc = context.OwinContext.Get<string>("tenant");
            if (!string.IsNullOrWhiteSpace(tenantAcc))
                Utility.UGITUtility.TenantAccount = tenantAcc;
            else
                Utility.UGITUtility.TenantAccount = null;

            Utility.UGITUtility.AuthenticationType = context.Options.AuthenticationType;

            UserProfile user = await userManager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await userManager.GenerateUserIdentityAsync(user,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await userManager.GenerateUserIdentityAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Response.Cookies.Append("Token", context.Options.AccessTokenFormat.Protect(ticket));
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            string[] additionalParam= context.Parameters.Where(x => x.Key == "tenant").Select(x => x.Value).FirstOrDefault();
            if (additionalParam != null)
                context.OwinContext.Set<string>("tenant", additionalParam[0]);
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}