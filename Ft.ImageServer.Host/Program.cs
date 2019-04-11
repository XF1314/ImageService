using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Builder;

namespace Ft.ImageServer.Host
{
    /// <inheritdoc/>
    public class Program
    {
        /// <inheritdoc/>
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("App_Data/logs.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                BuildWebHostInternal(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        //dotnet  Ft.ImageServer.Host.dll --server.urls "https://10.99.59.183:5002;http://10.99.59.183:5000" --environment Production
        /// <inheritdoc/>
        public static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

        #region 废弃
        //public static IWebHost BuildWebHostInternal(string[] args) => new WebHostBuilder()
        //    .UseKestrel((builderContext, options) => options.Configure(builderContext.Configuration.GetSection("Kestrel")))
        //    .UseContentRoot(Directory.GetCurrentDirectory())
        //    .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
        //    {
        //        var hostingEnvironment = hostingContext.HostingEnvironment;
        //        Log.Information($"当前环境是:{hostingEnvironment.EnvironmentName}");
        //        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        //        if (hostingEnvironment.IsDevelopment())
        //        {
        //            var appAssembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
        //            if (appAssembly != null) configurationBuilder.AddUserSecrets(appAssembly, optional: true);
        //        }

        //        configurationBuilder.AddEnvironmentVariables();
        //        if (args != null) configurationBuilder.AddCommandLine(args);

        //    })
        //    .ConfigureLogging((hostingContext, logging) =>
        //    {
        //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //        logging.AddConsole();
        //        logging.AddDebug();
        //    })
        //    .ConfigureServices((hostingContext, services) =>
        //    {
        //        services.PostConfigure<HostFilteringOptions>(options =>// Fallback
        //        {
        //            Log.Information($"AllowedHosts:{string.Join(';', options.AllowedHosts)}");
        //            if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
        //            {
        //                // "AllowedHosts": "localhost;127.0.0.1;[::1]"
        //                var hosts = hostingContext.Configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //                // Fall back to "*" to disable.
        //                options.AllowedHosts = (hosts?.Length > 0 ? hosts : new[] { "*" });
        //                Log.Information($"AllowedHosts:{string.Join(';', options.AllowedHosts)}");
        //            }
        //        });
        //        // Change notification
        //        services.AddSingleton<IOptionsChangeTokenSource<HostFilteringOptions>>(
        //            new ConfigurationChangeTokenSource<HostFilteringOptions>(hostingContext.Configuration));
        //        services.AddTransient<IStartupFilter, HostFilteringStartupFilter>();
        //    })
        //    .UseIISIntegration()
        //    .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
        //    .UseStartup<Startup>()
        //    .UseSerilog()
        //    .UseUrls("http://*:*")
        //    .Build();

        //public static IWebHost BuildWebHostInternal(string[] args) => new WebHostBuilder()
        //    .UseKestrel()
        //    .UseContentRoot(Directory.GetCurrentDirectory())
        //    .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
        //    {
        //        var hostingEnvironment = hostingContext.HostingEnvironment;
        //        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        //        if (hostingEnvironment.IsDevelopment())
        //        {
        //            var appAssembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
        //            if (appAssembly != null) configurationBuilder.AddUserSecrets(appAssembly, optional: true);
        //        }
        //        configurationBuilder.AddEnvironmentVariables();
        //        if (args != null) configurationBuilder.AddCommandLine(args);
        //    })
        //    .ConfigureLogging((hostingContext, logging) =>
        //    {
        //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //        logging.AddConsole();
        //        logging.AddDebug();
        //    })
        //    .UseIISIntegration()
        //    //.UseDefaultServiceProvider((context, options) =>
        //    //{
        //    //    var ss = context.HostingEnvironment.IsDevelopment();
        //    //    options.ValidateScopes = false;//context.HostingEnvironment.IsDevelopment();
        //    //})
        //    .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
        //    .UseStartup<Startup>()
        //    .UseSerilog()
        //    .Build();

        //internal class HostFilteringStartupFilter : IStartupFilter
        //{
        //    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        //    {
        //        return app =>
        //        {
        //            app.UseHostFiltering();
        //            next(app);
        //        };
        //    }
        //}

        #endregion
    }
}
