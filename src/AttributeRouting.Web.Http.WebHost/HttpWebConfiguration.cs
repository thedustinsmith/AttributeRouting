﻿using System;
using System.Threading;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebConfiguration : HttpConfigurationBase
    {
        public HttpWebConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            ParameterFactory = new RouteParameterFactory();
            RouteConstraintFactory = new RouteConstraintFactory(this);

            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;
            RouteHandlerFactory = () => null;
            RegisterDefaultInlineRouteConstraints<IRouteConstraint>(typeof(Web.Constraints.RegexRouteConstraint).Assembly);
        }

        public Func<IRouteHandler> RouteHandlerFactory { get; set; }

        /// <summary>
        /// This delegate returns the current UI culture name,
        /// which is used when constraining inbound routes by culture.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<HttpContextBase, RouteData, string> CurrentUICultureResolver { get; set; }

        /// <summary>
        /// Specifies a function that returns an alternate route handler.
        /// By default, the route handler is the default HttpControllerRouteHandler.
        /// </summary>
        /// <param name="routeHandlerFactory">The route handler to use.</param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory)
        {
            RouteHandlerFactory = routeHandlerFactory;
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}
