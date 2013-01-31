﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Http.WebHost.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    /// <summary>
    /// Route to use for ASP.NET-hosted Web API routes.
    /// </summary>
    public class HttpWebAttributeRoute : HttpWebRoute, IAttributeRoute
    {
        private readonly HttpWebConfiguration _configuration;

        /// <summary>
        /// Route used by the AttributeRouting framework in self-host projects.
        /// </summary>
        public HttpWebAttributeRoute(string url,
                                     RouteValueDictionary defaults,
                                     RouteValueDictionary constraints,
                                     RouteValueDictionary dataTokens,
                                     IRouteHandler routeHandler,
                                     IHttpRoute httpRoute,
                                     HttpWebConfiguration configuration)
            : base(url, defaults, constraints, dataTokens, routeHandler, httpRoute)
        {
            _configuration = configuration;
        }

        public string RouteName { get; set; }

        public string CultureName { get; set; }

        public List<string> MappedSubdomains { get; set; }

        public string Subdomain { get; set; }

        public bool? UseLowercaseRoute { get; set; }

        public bool? PreserveCaseForUrlParameters { get; set; }

        public bool? AppendTrailingSlash { get; set; }

        IDictionary<string, object> IAttributeRoute.DataTokens
        {
            get { return DataTokens; }
            set { throw new NotImplementedException("HttpRoute.DataTokens has no setter."); }
        }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { throw new NotImplementedException("HttpRoute.Constraints has no setter."); }
        }

        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { throw new NotImplementedException("HttpRoute.Defaults has no setter."); }
        }

        public IEnumerable<IAttributeRoute> Translations { get; set; }

        public IAttributeRoute DefaultRouteContainer { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Optimize matching by comparing the static left part of the route url with the requested path.
            var requestedPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
            if (!this.IsLeftPartOfUrlMatched(requestedPath))
            {
                return null;
            }

            // Let the underlying route match, and if it does, then add a few more constraints.
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
            {
                return null;
            }

            // Constrain by subdomain if configured
            var host = httpContext.SafeGet(ctx => ctx.Request.Headers["host"]);
            if (!this.IsSubdomainMatched(host, _configuration))
            {
                return null;
            }

            // Constrain by culture name if configured
            var currentUICultureName = _configuration.CurrentUICultureResolver(httpContext, routeData);
            if (!this.IsCultureNameMatched(currentUICultureName, _configuration))
            {
                return null;
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Let the underlying route do its thing, and if it does, then add some functionality on top.
            var virtualPathData = this.GetVirtualPath(() => base.GetVirtualPath(requestContext, values));
            if (virtualPathData == null)
            {
                return null;
            }

            // Translate this path if a translation is available.
            if (_configuration.TranslationProviders.Any())
            {
                virtualPathData =
                    this.GetTranslatedVirtualPath(t => ((Route)t).GetVirtualPath(requestContext, values))
                    ?? virtualPathData;
            }

            // Lowercase, append trailing slash, etc.
            var virtualPath = this.GetFinalVirtualPath(virtualPathData.VirtualPath, _configuration);
            virtualPathData.VirtualPath = virtualPath;

            return virtualPathData;
        }
    }
}
