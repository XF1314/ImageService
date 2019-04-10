using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.Extensions
{
    /// <summary>
    /// <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 基于类型获取service 实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
        /// <returns></returns>
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
