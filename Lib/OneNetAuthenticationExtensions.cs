using System;
using Owin;

namespace Archymeta.Web.Security.OneNet.OAuth
{
    public static class OneNetAuthenticationExtensions
    {
        public static IAppBuilder UseOneNetAuthentication(this IAppBuilder app, OneNetAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (options == null)
                throw new ArgumentNullException("options");

            app.Use(typeof(OneNetAuthenticationMiddleware), app, options);

            return app;
        }
    }
}
