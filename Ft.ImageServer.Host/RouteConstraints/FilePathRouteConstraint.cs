using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.RouteConstraints
{
    public class FilePathRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(value))
                return false;
            else
            {
                var regex = new Regex(@"(.+)");
                var match = regex.Match(value);
                return match.Success;
            }
        }
    }
}
