using System.Collections.Generic;
using System.Web.Http.WebHost;
using System.Web.Http.WebHost.Routing;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    public class HostedHttpAttributeRoute : HostedHttpRoute
    {
        public HostedHttpAttributeRoute(string uriTemplate,
                                        IDictionary<string, object> defaults,
                                        IDictionary<string, object> constraints,
                                        IDictionary<string, object> dataTokens,
                                        HttpWebConfiguration configuration)
            : base(uriTemplate, defaults, constraints, dataTokens, null)
        {
            var routeDefaults = defaults != null ? new RouteValueDictionary(defaults) : null;
            var routeConstraints = constraints != null ? new RouteValueDictionary(constraints) : null;
            var routeDataTokens = dataTokens != null ? new RouteValueDictionary(dataTokens) : null;

            OriginalRoute = new HttpWebAttributeRoute(uriTemplate,
                                                      routeDefaults,
                                                      routeConstraints,
                                                      routeDataTokens,
                                                      HttpControllerRouteHandler.Instance,
                                                      this,
                                                      configuration);
        }
    }
}