using Microsoft.AspNetCore.Builder;

namespace Ft.ImageServer.Host.Middlewares
{
    public  static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestUrlNormalizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUrlNormalizer>();
        }

        public static IApplicationBuilder UsePerformanceCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceCounter>();
        }

        public static IApplicationBuilder UseImageServerUnitOfWork(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageServerUnitOfWorkMiddleware>();
        }
    }
}
