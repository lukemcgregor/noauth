using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;

namespace Owin.Security.Providers.NoAuth
{
    public class NoAuthAuthenticationMiddleware : AuthenticationMiddleware<NoAuthAuthenticationOptions>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public NoAuthAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app,
            NoAuthAuthenticationOptions options)
            : base(next, options)
        {
            //if (string.IsNullOrWhiteSpace(Options.ClientId))
            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
            //        Resources.Exception_OptionMustBeProvided, "ClientId"));
            //if (string.IsNullOrWhiteSpace(Options.ClientSecret))
            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
            //        Resources.Exception_OptionMustBeProvided, "ClientSecret"));

            _logger = app.CreateLogger<NoAuthAuthenticationMiddleware>();

            if (Options.Provider == null)
                Options.Provider = new NoAuthAuthenticationProvider();

            if (Options.StateDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(
                    typeof (NoAuthAuthenticationMiddleware).FullName,
                    Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();

            _httpClient = new HttpClient(ResolveHttpMessageHandler(Options))
            {
                Timeout = Options.BackchannelTimeout,
                MaxResponseContentBufferSize = 1024*1024*10,
            };
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft Owin Oauth NoAuth middleware");
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
        }

        /// <summary>
        ///     Provides the <see cref="T:Microsoft.Owin.Security.Infrastructure.AuthenticationHandler" /> object for processing
        ///     authentication-related requests.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:Microsoft.Owin.Security.Infrastructure.AuthenticationHandler" /> configured with the
        ///     <see cref="T:Owin.Security.Providers.NoAuth.NoAuthAuthenticationOptions" /> supplied to the constructor.
        /// </returns>
        protected override AuthenticationHandler<NoAuthAuthenticationOptions> CreateHandler()
        {
            return new NoAuthAuthenticationHandler(_httpClient, _logger);
        }

        private static HttpMessageHandler ResolveHttpMessageHandler(NoAuthAuthenticationOptions options)
        {
            var handler = options.BackchannelHttpHandler ?? new WebRequestHandler();

            // If they provided a validator, apply it or fail.
            if (options.BackchannelCertificateValidator == null) return handler;
            // Set the cert validate callback
            var webRequestHandler = handler as WebRequestHandler;
            if (webRequestHandler == null)
            {
                throw new InvalidOperationException(Resources.Exception_ValidatorHandlerMismatch);
            }
            webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;

            return handler;
        }
    }
}