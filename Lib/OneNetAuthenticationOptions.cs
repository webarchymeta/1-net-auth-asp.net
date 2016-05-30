using System;
using System.Collections.Generic;
using System.Net.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Archymeta.Web.Security.OneNet.OAuth
{

    public class OneNetAuthenticationOptions : AuthenticationOptions
    {
        public class OneNetAuthenticationEndpoints
        {
            /// <summary>
            /// Endpoint which is used to redirect users to request OneNet access
            /// </summary>
            public string AuthorizationEndpoint { get; set; }

            /// <summary>
            /// Endpoint which is used to exchange code for access token
            /// </summary>
            public string TokenEndpoint { get; set; }

            /// <summary>
            /// Endpoint which is used to obtain user information after authentication
            /// </summary>
            public string UserInfoEndpoint { get; set; }
        }

        protected Config.Section Opts
        {
            get
            {
                if (_opts == null)
                {
                    _opts = System.Configuration.ConfigurationManager.GetSection("oneNetSignIn") as Config.Section;
                }
                return _opts;
            }
        }
        private Config.Section _opts = null;

        /// <summary>
        ///     Gets or sets the a pinned certificate validator to use to validate the endpoints used
        ///     in back channel communications belong to OneNet.
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
        ///     The HttpMessageHandler used to communicate with OneNet.
        ///     This cannot be set at the same time as BackchannelCertificateValidator unless the value
        ///     can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        ///     Gets or sets timeout value in milliseconds for back channel communications with OneNet.
        /// </summary>
        /// <value>
        ///     The back channel timeout in milliseconds.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        ///     The request path within the application's base path where the user-agent will be returned.
        ///     The middleware will process this request when it arrives.
        ///     Default value is "/signin-1-net".
        /// </summary>
        public PathString CallbackPath 
        { 
            get
            {
                if (_callbackPath == PathString.Empty)
                {
                    if (Opts.ReturnPath != null)
                        return new PathString(Opts.ReturnPath);
                    else
                        return new PathString("/signin-1-net");
                }
                else
                    return _callbackPath;
            }
            set
            {
                _callbackPath = value;
            }
        }
        private PathString _callbackPath = PathString.Empty;

        /// <summary>
        ///     Gets or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        ///     Gets the OneNet supplied Client ID
        /// </summary>
        public string ClientId
        {
            get
            {
                return Opts.ClientId;
            }
        }

        /// <summary>
        ///     Gets the OneNet supplied Client Secret
        /// </summary>
        public string ClientSecret 
        { 
            get
            {
                return Opts.ClientSecret;
            }
        }

        /// <summary>
        /// Gets the sets of OAuth endpoints used to authenticate against OneNet.  Overriding these endpoints allows you to use OneNet Enterprise for
        /// authentication.
        /// </summary>
        public OneNetAuthenticationEndpoints Endpoints
        {
            get
            {
                if (_endpoints == null)
                {
                    _endpoints = new OneNetAuthenticationEndpoints
                    {
                        AuthorizationEndpoint = Opts.BaseServiceUrl + string.Format(Opts.AuthPathFmt, Opts.ViewType.ToString().ToLower()),
                        TokenEndpoint = Opts.BaseServiceUrl + Opts.TokenPath,
                        UserInfoEndpoint = Opts.BaseServiceUrl + Opts.UserPath
                    };
                }
                return _endpoints;
            }
        }
        private OneNetAuthenticationEndpoints _endpoints = null;

        /// <summary>
        ///     Gets or sets the <see cref="IOneNetAuthenticationProvider" /> used in the authentication events
        /// </summary>
        public IOneNetAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// A list of permissions to request.
        /// </summary>
        public IList<string> Scope
        {
            get
            {
                return Opts.Scope;
            }
        }

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
        ///     Initializes a new <see cref="OneNetAuthenticationOptions" />
        /// </summary>
        public OneNetAuthenticationOptions()
            : base("1-Net")
        {
            Caption = Constants.DefaultAuthenticationType;
            AuthenticationMode = AuthenticationMode.Passive;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }
    }
}
