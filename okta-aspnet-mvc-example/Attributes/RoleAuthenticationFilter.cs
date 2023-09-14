using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace aspnet_mvc_example.Attributes
{
    public class RoleAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public string Roles { get; set; }
        void IAuthenticationFilter.OnAuthentication(AuthenticationContext filterContext)
        {
            var claims =  filterContext.HttpContext.GetOwinContext().Authentication.User.Claims.ToList();
            var userType = claims.FirstOrDefault(x => x.Type == "userType");
            if (userType != null)
            {
                if (!Roles.Contains(userType.Value))
                {
                    //return error page
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!!" };
                    throw new HttpResponseException(msg);
                }
            }
        }

        void IAuthenticationFilter.OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                    { "controller", "Account" },
                    { "action", "Login" } });
            }
        }
    }
}