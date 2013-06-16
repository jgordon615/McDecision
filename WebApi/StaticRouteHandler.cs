using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApi
{
    public class StaticRouteHandler : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }
    }
}