// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace Archymeta.Web.Security.OneNet.OAuth
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class OneNetAuthenticatedContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="OneNetAuthenticatedContext"/>
        /// </summary>
        /// <param name="context">The OWIN environment</param>
        /// <param name="user">The JSON-serialized user</param>
        /// <param name="accessToken">OneNet Access token</param>
        public OneNetAuthenticatedContext(IOwinContext context, JObject user, JObject accessToken)
            : base(context)
        {
            User = user;
            AccessToken = accessToken;
            Id = TryGetValue(user, "userId");
            Name = TryGetValue(user, "displayName");
            JToken _urls;
            if (user.TryGetValue("urls", out _urls))
            {
                JArray urls = (JArray)_urls;
                if (urls.HasValues)
                {
                    var q = from d in urls select d;
                    var first = urls.First.ToObject<JObject>();
                    Link = TryGetValue(first, "url");
                }
            }
            UserName = TryGetValue(user, "username");
            Email = TryGetValue(user, "email");
            Gender = TryGetValue(user, "gender");
            EndpointId = TryGetValue(accessToken, "endpointId");
        }

        /// <summary>
        /// Gets the JSON-serialized user
        /// </summary>
        /// <remarks>
        /// Contains the OneNet user obtained from the User Info endpoint. By default this is https://api.yiwg.net/api/signin/user but it can be
        /// overridden in the options
        /// </remarks>
        public JObject User { get; private set; }

        /// <summary>
        /// Gets the OneNet access token
        /// </summary>
        public JObject AccessToken { get; private set; }

        /// <summary>
        /// Gets the OneNet user ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the OneNet user EndpointID
        /// </summary>
        public string EndpointId { get; private set; }

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public string Name { get; private set; }

        public string Gender { get; private set; }

        public string Link { get; private set; }

        /// <summary>
        /// Gets the OneNet username
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the OneNet email
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

        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }
}
