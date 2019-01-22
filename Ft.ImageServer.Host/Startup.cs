using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Host.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;

namespace Ft.ImageServer.Host
{
    public class Startup
    {
        public IConfigurationRoot ConfigurationRoot { get; }

        public Startup(IHostingEnvironment env)
        {
            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<List<MongoDBHostConfig>>(ConfigurationRoot.GetSection("MongoDBHosts"));
            services.Configure<List<WebHostConfig>>(ConfigurationRoot.GetSection("WebHosts"));
            services.AddMvc(options => options.Filters.Add<ModelValidationFilter>());
            services.AddApplication<ImageServerHostModule>(options => options.UseAutofac());

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.InitializeApplication();
        }
    }
}
