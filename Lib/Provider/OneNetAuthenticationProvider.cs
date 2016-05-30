using System;
using System.Threading.Tasks;

namespace Archymeta.Web.Security.OneNet.OAuth
{
    /// <summary>
    /// Default <see cref="IOneNetAuthenticationProvider"/> implementation.
    /// </summary>
    public class OneNetAuthenticationProvider : IOneNetAuthenticationProvider
    {
        /// <summary>
        /// Initializes a <see cref="OneNetAuthenticationProvider"/>
        /// </summary>
        public OneNetAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
        }

        /// <summary>
        /// Gets or sets the function that is invoked when the Authenticated method is invoked.
        /// </summary>
        public Func<OneNetAuthenticatedContext, Task> OnAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the function that is invoked when the ReturnEndpoint method is invoked.
        /// </summary>
        public Func<OneNetReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        /// <summary>
        /// Invoked whenever OneNet successfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task Authenticated(OneNetAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        /// <summary>
        /// Invoked prior to the <see cref="System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task ReturnEndpoint(OneNetReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }
    }
}