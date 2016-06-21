using System;

namespace Owin.Security.Providers.NoAuth
{
    public static class NoAuthAuthenticationExtensions
    {
        public static IAppBuilder UseNoAuthAuthentication(this IAppBuilder app,
            NoAuthAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            app.Use(typeof(NoAuthAuthenticationMiddleware), app, options);

            return app;
        }

        public static IAppBuilder UseNoAuthAuthentication(this IAppBuilder app, string clientId, string clientSecret)
        {
            return app.UseNoAuthAuthentication(new NoAuthAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }
    }
}