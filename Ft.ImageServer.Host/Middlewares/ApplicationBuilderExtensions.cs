using Microsoft.AspNetCore.Builder;

namespace Ft.ImageServer.Host.Middlewares
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>
    /// </summary>
    public  static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// <see cref="RequestUrlNormalizeMiddleware"/>
        /// </summary>
        public static IApplicationBuilder UseRequestUrlNormalizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUrlNormalizeMiddleware>();
        }

        /// <summary>
        /// <see cref="PerformanceCountMiddleware"/>
        /// </summary>
  
        public static IApplicationBuilder UsePerformanceCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceCountMiddleware>();
        }

        /// <summary>
        /// <see cref="ImageServerUnitOfWorkMiddleware"/>
        /// </summary>
        public static IApplicationBuilder UseImageServerUnitOfWork(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageServerUnitOfWorkMiddleware>();
        }
    }
}
