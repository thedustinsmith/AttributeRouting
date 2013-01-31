using System.Collections.Generic;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    internal class AttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpWebConfiguration _configuration;

        public AttributeRouteFactory(HttpWebConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IAttributeRoute> CreateAttributeRoutes(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens)
        {
            var hostedHttpRoute = new HostedHttpAttributeRoute(url,
                                                               defaults,
                                                               constraints,
                                                               dataTokens,
                                                               _configuration);
                
            yield return (HttpWebAttributeRoute)hostedHttpRoute.OriginalRoute;
        }
    }
}
