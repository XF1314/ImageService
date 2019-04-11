using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    /// <inheritdoc/>
    public class Startup
    {
        private readonly IConfigurationRoot _appSettings;

        /// <inheritdoc/>
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            //Console.WriteLine(hostingEnvironment.EnvironmentName);
            _appSettings = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <inheritdoc/>
        public IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<List<MongoDBHostConfig>>(_appSettings.GetSection("MongoDBHosts"));
            serviceCollection.Configure<List<WebHostConfig>>(_appSettings.GetSection("WebHosts"));
            serviceCollection.AddMvc(options => options.Filters.Add<ModelValidationFilter>());
            serviceCollection.AddApplication<ImageServerHostModule>(options => options.UseAutofac());

            return serviceCollection.BuildServiceProviderFromFactory();
        }

        /// <inheritdoc/>
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment,IConfiguration configuration)
        {
            applicationBuilder.InitializeApplication();
        }
    }
}
