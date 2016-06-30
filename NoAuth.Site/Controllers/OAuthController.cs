using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.Messages;
using NoAuth.Site.OAuth;

namespace NoAuth.Site.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AuthorizationServer _authorizationServer;
        public OAuthController(AuthorizationServer authServer)
        {
            _authorizationServer = authServer;
        }

        public async Task<ActionResult> Token()
        {
            var request = await _authorizationServer.HandleTokenRequestAsync(Request, Response.ClientDisconnectedToken);
            Response.ContentType = request.Content.Headers.ContentType.ToString();
            return request.AsActionResult();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Authorize]
        public async Task<ActionResult> Authorize()
        {
            var pendingRequest = await _authorizationServer.ReadAuthorizationRequestAsync(Request, Response.ClientDisconnectedToken);
            if (pendingRequest == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Missing authorization request.");
            }

            var approval = _authorizationServer.PrepareApproveAuthorizationRequest(pendingRequest, HttpContext.User.Identity.Name);
            var response = await _authorizationServer.Channel.PrepareResponseAsync(approval, Response.ClientDisconnectedToken);
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            FormsAuthentication.SignOut();
            return response.AsActionResult();
        }
    }
}