using System;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Owin.Security.Providers.NoAuth
{
    public class NoAuthAuthenticationOptions : AuthenticationOptions
    {
        public class NoAuthAuthenticationEndpoints
        {
			/// <summary>
			/// Endpoint which is used to redirect users to request  access
			/// </summary>
			/// <remarks>
			/// Defaults to https://app..com/-/oauth_authorize
			/// </remarks>
			public string AuthorizationEndpoint { get; set; } = "http://localhost:34759/oauth/authorize";

			/// <summary>
			/// Endpoint which is used to exchange code for access token
			/// </summary>
			/// <remarks>
			/// Defaults to https://app..com/-/oauth_token
			/// </remarks>
			public string TokenEndpoint { get; set; } = "http://localhost:34759/oauth/token";

			/// <summary>
			/// Endpoint which is used to obtain user information after authentication
			/// </summary>
			/// <remarks>
			/// Defaults to https://.com/1/OAuthGetRequestToken
			/// </remarks>
			public string UserInfoEndpoint { get; set; } = "http://localhost:34759/account/info";
		}


		/// <summary>
		///     Gets or sets the a pinned certificate validator to use to validate the endpoints used
		///     in back channel communications belong to .
		/// </summary>
		/// <value>
		///     The pinned certificate validator.
		/// </value>
		/// <remarks>
		///     If this property is null then the default certificate checks are performed,
		///     validating the subject name and if the signing chain is a trusted party.
		/// </remarks>
		public ICertificateValidator BackchannelCertificateValidator { get; set; }

        /// <summary>
        ///     The HttpMessageHandler used to communicate with .
        ///     This cannot be set at the same time as BackchannelCertificateValidator unless the value
        ///     can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        ///     Gets or sets timeout value in milliseconds for back channel communications with .
        /// </summary>
        /// <value>
        ///     The back channel timeout in milliseconds.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        ///     The request path within the application's base path where the user-agent will be returned.
        ///     The middleware will process this request when it arrives.
        ///     Default value is "/signin-".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        ///     Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        ///     Gets or sets the  supplied Client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     Gets or sets the  supplied Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets the sets of OAuth endpoints used to authenticate against .  Overriding these endpoints allows you to use  Enterprise for
        /// authentication.
        /// </summary>
        public NoAuthAuthenticationEndpoints Endpoints { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="INoAuthAuthenticationProvider" /> used in the authentication events
        /// </summary>
        public INoAuthAuthenticationProvider Provider { get; set; }


        /// <summary>
        ///     Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user
        ///     <see cref="System.Security.Claims.ClaimsIdentity" />.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        ///     Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        ///     Initializes a new <see cref="NoAuthAuthenticationOptions" />
        /// </summary>
        public NoAuthAuthenticationOptions()
            : base("NoAuth")
        {
            Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-noauth");
            AuthenticationMode = AuthenticationMode.Passive;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
            Endpoints = new NoAuthAuthenticationEndpoints();
        }
    }
}