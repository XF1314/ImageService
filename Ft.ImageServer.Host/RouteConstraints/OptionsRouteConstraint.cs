using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.RouteConstraints
{
    public class OptionsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(value))
                return false;
            else
            {
                var regex = new Regex(@"([tgf]{1,3})");
                var match = regex.Match(value);
                return match.Success && match.Value.Length == value.Length && StringHasUniqueChars(match.Value);
            }
        }

        private bool StringHasUniqueChars(string key)
        {
            var charTable = string.Empty;
            foreach (var character in key)
            {
                if (charTable.IndexOf(character) == -1)
                {
                    charTable += character;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
