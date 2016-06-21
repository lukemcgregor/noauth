// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace Owin.Security.Providers.NoAuth
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class NoAuthAuthenticatedContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="NoAuthAuthenticatedContext"/>
        /// </summary>
        /// <param name="context">The OWIN environment</param>
        /// <param name="user">The JSON-serialized user</param>
        /// <param name="accessToken"> Access token</param>
        public NoAuthAuthenticatedContext(IOwinContext context, dynamic user, string accessToken)
            : base(context)
        {
            User = user;
            AccessToken = accessToken;
			Id = user.Id;
			Name = user.Name;
			Email = user.Email;
        }

        /// <summary>
        /// Gets the JSON-serialized user
        /// </summary>
        /// <remarks>
        /// Contains the  user obtained from the User Info endpoint. By default this is https://.com/1/OAuthGetRequestToken but it can be
        /// overridden in the options
        /// </remarks>
        public JObject User { get; private set; }

        /// <summary>
        /// Gets the  access token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the  user ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the  email
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the <see cref="ClaimsIdentity"/> representing the user
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }

        private static string TryGetValue(dynamic user, string propertyName)
        {
            string value = null;

            try
            {
                value = (string)user[propertyName];
            }
            catch
            {

            }

            return value;
        }
    }
}
