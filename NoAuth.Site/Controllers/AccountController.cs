using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.OAuth2;
using NoAuth.Site.OAuth;

namespace NoAuth.Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly ResourceServer _authorizationServer;
        private readonly IUserStore _userStore;

        public AccountController(IUserStore userStore, ResourceServer authServer)
        {
            _authorizationServer = authServer;

            _userStore = userStore;
        }

        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string returnUrl, LoginModel model)
        {
            var id = Guid.NewGuid().ToString();
            _userStore.Users[id] = new List<Tuple<string, string>>
            {
                new Tuple<string,string>(ClaimTypes.Name, model.Name),
                new Tuple<string,string>(ClaimTypes.Email, model.Email)
            };

            FormsAuthentication.SetAuthCookie(id, true);

            return Redirect(returnUrl);
        }

        public async Task<ActionResult> Info(string accessToken)
        {
            try
            {
                var at = await _authorizationServer.GetAccessTokenAsync(Request);

                return Json(new
                {
                    Id = at.User,
                    Claims = _userStore.Users.ContainsKey(at.User) ? _userStore.Users[at.User] : new List<Tuple<string, string>>()
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }

    public class LoginModel
    {
        public string ClaimedIdentifier { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}