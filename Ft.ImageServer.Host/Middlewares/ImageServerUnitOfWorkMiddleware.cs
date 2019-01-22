using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Uow;
using Ft.ImageServer.Core.Extensions;
namespace Ft.ImageServer.Host.Middlewares
{
    public class ImageServerUnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ImageServerUnitOfWorkMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var unitOfWorkManager = _serviceProvider.GetService<IUnitOfWorkManager>();
            using (var uow = unitOfWorkManager.Begin())
            {
                await _next(httpContext);
                await uow.CompleteAsync(httpContext.RequestAborted);
            }
        }
    }
}
