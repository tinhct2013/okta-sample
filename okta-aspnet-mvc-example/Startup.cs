using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Okta.AspNet;
using Okta.Idx.Sdk;
using Okta.Sdk.Abstractions;
using Okta.Sdk.Api;
using Okta.Sdk.Client;
using Owin;
using Thinktecture.IdentityModel.Client;
using Configuration = Okta.Sdk.Client.Configuration;

[assembly: OwinStartup(typeof(okta_aspnet_mvc_example.Startup))]

namespace okta_aspnet_mvc_example
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/Account/Login"),
            });

            app.UseOktaMvc(new OktaMvcOptions()
            {
                OktaDomain = ConfigurationManager.AppSettings["okta:OktaDomain"],
                ClientId = ConfigurationManager.AppSettings["okta:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["okta:ClientSecret"],
                AuthorizationServerId = ConfigurationManager.AppSettings["okta:AuthorizationServerId"],
                RedirectUri = ConfigurationManager.AppSettings["okta:RedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["okta:PostLogoutRedirectUri"],
                Scope = new List<string> { "openid", "profile", "optional_profile" },
                LoginMode = LoginMode.OktaHosted,
                OpenIdConnectEvents = new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationNotifications()
                {
                    SecurityTokenValidated = async ctx =>
                    {
                        var userInfoClient = new UserInfoClient(new Uri($"{ConfigurationManager.AppSettings["okta:OktaDomain"]}/oauth2/default/v1/userinfo"), ctx.ProtocolMessage.AccessToken);
                        var userInfo = await userInfoClient.GetAsync();
                        if (userInfo != null)
                        {
                            var claim = userInfo.Claims.FirstOrDefault(x => x.Item1 == "userType");
                            if (claim != null)
                            {
                               //Save session at here
                            }
                        }
                        await Task.CompletedTask;
                    }
                }
            });
        }
    }
}
