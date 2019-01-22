using Ft.ImageServer.Core;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Ft.ImageServer.MongoDB
{
    [DependsOn(typeof(ImageServerCoreModule),typeof(AbpMongoDbModule))]
    public class ImageServerMongoDBModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
