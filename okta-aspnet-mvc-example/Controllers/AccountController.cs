using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Okta.AspNet;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace okta_aspnet_mvc_example.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(OktaDefaults.MvcAuthenticationType);

                return new HttpUnauthorizedResult();
            }

            var claims = HttpContext.GetOwinContext().Authentication.User.Claims.ToList();
            var claim = claims.FirstOrDefault(x => x.Type == "userType");
            if (claim != null)
            {
                switch (claim.Value)
                {
                    case "EE":
                        {
                            return RedirectToAction("Index", "Employee");
                        }
                    case "ER":
                        {
                            return RedirectToAction("Index", "Employer");
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            //Error
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(
                    CookieAuthenticationDefaults.AuthenticationType,
                    OktaDefaults.MvcAuthenticationType);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}