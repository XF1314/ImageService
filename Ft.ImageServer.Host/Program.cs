using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

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
                .WriteTo.File("Logs/logs.txt")
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

        //Ft.ImageServer.Host.dll --server.urls "https://10.99.59.183:5002;http://10.99.59.183:5000"
        /// <inheritdoc/>
        public static IWebHost BuildWebHostInternal(string[] args) => new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                var hostingEnvironment = hostingContext.HostingEnvironment;
                configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                if (hostingEnvironment.IsDevelopment())
                {
                    var appAssembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                    if (appAssembly != null) configurationBuilder.AddUserSecrets(appAssembly, optional: true);
                }
                configurationBuilder.AddEnvironmentVariables();
                if (args != null) configurationBuilder.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
            })
            .UseIISIntegration()
            .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
            .UseStartup<Startup>()
            .UseSerilog()
            .Build();

        //new WebHostBuilder()
        //    .UseKestrel()
        //    .UseContentRoot(Directory.GetCurrentDirectory())
        //    .UseIISIntegration()
        //    .UseStartup<Startup>()
        //    .UseSerilog()
        //    .Build();
    }
}
