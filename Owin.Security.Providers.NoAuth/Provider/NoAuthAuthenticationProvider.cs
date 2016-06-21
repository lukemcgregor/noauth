using System;
using System.Threading.Tasks;

namespace Owin.Security.Providers.NoAuth
{
    /// <summary>
    /// Default <see cref="INoAuthAuthenticationProvider"/> implementation.
    /// </summary>
    public class NoAuthAuthenticationProvider : INoAuthAuthenticationProvider
    {
        /// <summary>
        /// Initializes a <see cref="NoAuthAuthenticationProvider"/>
        /// </summary>
        public NoAuthAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
        }

        /// <summary>
        /// Gets or sets the function that is invoked when the Authenticated method is invoked.
        /// </summary>
        public Func<NoAuthAuthenticatedContext, Task> OnAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the function that is invoked when the ReturnEndpoint method is invoked.
        /// </summary>
        public Func<NoAuthReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        /// <summary>
        /// Invoked whenever  successfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task Authenticated(NoAuthAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        /// <summary>
        /// Invoked prior to the <see cref="System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task ReturnEndpoint(NoAuthReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }
    }
}