using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.Middlewares
{
    public class RequestUrlNormalizer
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestUrlNormalizer> _logger;

        public RequestUrlNormalizer(RequestDelegate next, ILogger<RequestUrlNormalizer> logger)
        {
            _next = next;
            _logger = logger;
        }

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
