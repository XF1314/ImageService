using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ft.ImageServer.Host.Middlewares
{
    public class PerformanceCounter
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceCounter> _logger;

        public PerformanceCounter(RequestDelegate next, ILogger<PerformanceCounter> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _next.Invoke(context);

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 10000)
                _logger.LogError($"处理耗时: {stopwatch.ElapsedMilliseconds}ms");
            else if (stopwatch.ElapsedMilliseconds > 5000)
                _logger.LogWarning($"处理耗时: {stopwatch.ElapsedMilliseconds}ms");
            else if (stopwatch.ElapsedMilliseconds > 1000)
                _logger.LogTrace($"处理耗时: {stopwatch.ElapsedMilliseconds}ms");
        }

    }
}
