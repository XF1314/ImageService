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
    /// GridFsReoute约束
    /// </summary>
    public class GridFsReouteConstraint : IRouteConstraint
    {
        /// <inheritdoc/>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[routeKey] as string??string.Empty;
            var index = value.LastIndexOf('.');
            value = index == -1 ? value : value.Substring(0, index);
            if (string.IsNullOrWhiteSpace(value) || value.Length != 24) //GridFs id 长度必须是24
                return false;
            else
            {
                //var regex = new Regex(@"^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$",RegexOptions.IgnoreCase);
                var regex = new Regex(@"([0-9a-fA-F]{24})");
                var match = regex.Match(value);
                    return match.Success ;
            }
        }
    }
}
