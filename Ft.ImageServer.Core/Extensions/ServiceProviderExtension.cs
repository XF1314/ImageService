using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.Extensions
{
    public static class ServiceProviderExtension
    {
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            var serviceType = typeof(T);
            var serviceObject = serviceProvider.GetService(serviceType);
            if (serviceObject == null)
                throw new Exception($"未能找到类型为:{serviceType.Name}的实现");
            else
            {
                return (T)serviceProvider.GetService(serviceType);
            }
        }
    }
}
