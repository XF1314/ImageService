using Ft.ImageServer.Core;
using Ft.ImageServer.Core.Entities;
using Ft.ImageServer.Host.Middlewares;
using Ft.ImageServer.Host.RouteConstraints;
using Ft.ImageServer.MongoDB;
using Ft.ImageServer.MongoDB.Repositories;
using FT.ImageServer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Ft.ImageServer.Host
{
    [DependsOn(typeof(ImageServerCoreModule), typeof(ImageServerServiceModule),
        typeof(AbpAspNetCoreModule), typeof(AbpGuidsModule), typeof(AbpUnitOfWorkModule), typeof(AbpAutofacModule))]
    public class ImageServerHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configurationRoot = context.Services.BuildConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            context.Services.AddHttpClient();
            context.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("gridfs", typeof(GridFsReouteConstraint)));
            context.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("options", typeof(OptionsRouteConstraint)));
            context.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("filepath", typeof(FilePathRouteConstraint)));
            context.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("metahash", typeof(MetaHashRouteConstraint)));

            Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configurationRoot.GetConnectionString("ThemeParkImageServer");
            });
            context.Services.AddMongoDbContext<ImageServerMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ImageServerMongoDbContext>();
                options.AddRepository<ImageMetadata, ImageMetadataRepository>();
            });
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ImageServer", new Info { Title = "票务图片服务", Version = "V1" });
                options.DocInclusionPredicate((docName, description) =>
                {
                    return description.ActionDescriptor.DisplayName.StartsWith("Ft.ImageServer");
                });
                foreach (var file in Directory.EnumerateFiles(AppContext.BaseDirectory, "Ft.ImageServer.*.xml"))
                {
                    options.IncludeXmlComments(file);
                }
            });
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCors();
            app.UseSwagger();
            app.UseStaticFiles();
            app.UsePerformanceCounter();
            app.UseRequestUrlNormalizer();
            app.UseImageServerUnitOfWork();
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}.json"; });
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/ImageServer.json", "ImageServer API V1"));
            app.UseAbpRequestLocalization();
            app.UseMvcWithDefaultRoute();
        }
    }
}
