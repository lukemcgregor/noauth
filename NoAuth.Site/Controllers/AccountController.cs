using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NoAuth.Site.OAuth;

namespace NoAuth.Site.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserStore _userStore;

		public AccountController(IUserStore userStore)
		{
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
			_userStore.Users[id] = new List<System.Security.Claims.Claim>
			{
				new Claim(ClaimTypes.Name, model.Name),
				new Claim(ClaimTypes.Email, model.Email)
			};

			FormsAuthentication.SetAuthCookie(id, true);

			return Redirect(returnUrl);
		}

		public ActionResult Info(string accessToken)
		{
			//TODO make this pull from the datas the user typed in
			return Json(new
			{
				Id = "qq",
				Name = "Joe",
				Email = "sffs@dfss.con"
			}, JsonRequestBehavior.AllowGet);
		}
	}

	public class LoginModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
	}
}