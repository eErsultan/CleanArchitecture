using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;
using WebAPI.Extensions;

namespace WebAPI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args).Build();
                await host.MigrateContexts();
                await host.InitializeBaseUser();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Environment", environment)
                .CreateLogger();
        }
    }
}
