using Ft.ImageServer.Core;
using Ft.ImageServer.MongoDB;
using Ft.ImageServer.Service;
using Ft.ImageServer.Service.ImageProviders;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace FT.ImageServer.Service
{
    [DependsOn(typeof(AbpDddApplicationModule),
        typeof(ImageServerCoreModule), typeof(ImageServerMongoDBModule), typeof(AbpGuidsModule))]
    public class ImageServerServiceModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Add(new ServiceDescriptor(typeof(IImageService), typeof(ImageService), ServiceLifetime.Transient));
            context.Services.Add(new ServiceDescriptor(typeof(WebImageProvider), typeof(WebImageProvider), ServiceLifetime.Transient));
            context.Services.Add(new ServiceDescriptor(typeof(MongoDBImageProvider), typeof(MongoDBImageProvider), ServiceLifetime.Transient));
            context.Services.Add(new ServiceDescriptor(typeof(FileSystemImageProvider), typeof(FileSystemImageProvider), ServiceLifetime.Transient));
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
