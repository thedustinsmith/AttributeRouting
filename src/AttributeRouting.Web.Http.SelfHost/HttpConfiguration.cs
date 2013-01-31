using System;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpConfiguration : HttpConfigurationBase
    {
        public HttpConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            RouteConstraintFactory = new RouteConstraintFactory(this);
            ParameterFactory = new RouteParameterFactory();

            RegisterDefaultInlineRouteConstraints<IHttpRouteConstraint>(typeof(RegexRouteConstraint).Assembly);

            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;

            // Must turn on AutoGenerateRouteNames and use the Unique RouteNameBuilder for this to work out-of-the-box.
            AutoGenerateRouteNames = true;
            RouteNameBuilder = RouteNameBuilders.Unique;
        }

        /// <summary>
        /// This delegate returns the current UI culture name,
        /// which is used when constraining inbound routes by culture.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<HttpRequestMessage, IHttpRouteData, string> CurrentUICultureResolver { get; set; }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IHttpRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}
