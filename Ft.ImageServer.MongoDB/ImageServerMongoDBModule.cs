using Ft.ImageServer.Core;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Ft.ImageServer.MongoDB
{
    /// <summary>
    /// 图片服务MongoDB模块
    /// </summary>
    [DependsOn(typeof(ImageServerCoreModule),typeof(AbpMongoDbModule))]
    public class ImageServerMongoDBModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        /// <inheritdoc/>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        /// <inheritdoc/>
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
