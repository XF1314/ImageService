using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.RouteConstraints
{
    /// <summary>
    /// MetaHashRoute约束
    /// </summary>
    public class MetaHashRouteConstraint : IRouteConstraint
    {
        /// <inheritdoc/>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[routeKey] as string ?? string.Empty;
            if (value.Length != 32)//Hash 长度必须为32
                return false;
            else
            {
                var regex = new Regex(@"[a-z0-9]{32}$", RegexOptions.IgnoreCase);
                var match = regex.Match(value);
                return match.Success;
            }
        }
    }
}
