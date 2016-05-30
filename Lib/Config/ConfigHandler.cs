using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Archymeta.Web.Security.OneNet.OAuth.Config
{
    public enum ViewTypes
    {
        Browser,
        Desktop,
        Mobile
    }

    public class SectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return new Section(section);
        }
    }

    public class Section
    {
        public ViewTypes ViewType = ViewTypes.Browser;
        public string ReturnPath;
        public string ClientId;
        public string ClientSecret;
        public string BaseServiceUrl;
        public string AuthPathFmt;
        public string TokenPath;
        public string UserPath;
        public string[] Scope;

        public Section(XmlNode section)
        {
            var settings = section.SelectSingleNode("settings");
            ViewType = (ViewTypes)Enum.Parse(typeof(ViewTypes), settings.Attributes["viewType"].Value, true);
            ReturnPath = settings.Attributes["returnPath"].Value;
            var service = section.SelectSingleNode("service");
            ClientId = service.Attributes["clientId"].Value;
            ClientSecret = service.Attributes["clientSecret"].Value;
            var endpoints = service.SelectSingleNode("endpoints");
            BaseServiceUrl = endpoints.Attributes["baseUrl"].Value;
            AuthPathFmt = endpoints.SelectSingleNode("authorize").Attributes["pathFormat"].Value;
            TokenPath = endpoints.SelectSingleNode("token").Attributes["path"].Value;
            UserPath = endpoints.SelectSingleNode("user").Attributes["path"].Value;
            var scopes = new List<string>();
            foreach (XmlNode sn in endpoints.SelectSingleNode("token").SelectNodes("scope"))
            {
                scopes.Add(sn.Attributes["value"].Value);
            }
            Scope = scopes.ToArray();
        }
    }
}
