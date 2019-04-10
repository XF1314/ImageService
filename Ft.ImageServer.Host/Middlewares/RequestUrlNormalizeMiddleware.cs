using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.Middlewares
{
    /// <summary>
    /// 请求Url准化Middleware
    /// </summary>
    public class RequestUrlNormalizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestUrlNormalizeMiddleware> _logger;

        /// <inheritdoc/>
        public RequestUrlNormalizeMiddleware(RequestDelegate next, ILogger<RequestUrlNormalizeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            string url = context.Request.Path.Value;

            if (!string.IsNullOrWhiteSpace(url) && url.Contains("//"))
            {
                _logger.LogWarning($"不符合规范的request url, url={url}");
                while (url.Contains("//"))
                {
                    url = url.Replace("//", "/");
                }

                context.Request.Path = url;
            }

            await _next.Invoke(context);
        }
    }
}
