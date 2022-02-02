using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace WWSI.PK105_Notifications
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.AddSerilog();
                    webBuilder.UseStartup<Startup>();
                });
    }
    public static class SerilogExtensions
    {
        public static IWebHostBuilder AddSerilog(this IWebHostBuilder builder)
        {
            return builder.UseSerilog((context, loggerConfig) =>
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.InstrumentationKey =
                    context.Configuration["ApplicationInsights:InstrumentationKey"];
                loggerConfig.ReadFrom.Configuration(context.Configuration).WriteTo
                    .ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);

                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfig.WriteTo.Console();
                }

                if (context.Configuration["Serilog:LogDirectory"] != null)
                {
                    loggerConfig.WriteTo.File(
                    Path.Combine(context.Configuration["Serilog:LogDirectory"], "log_.txt"),
                    rollingInterval: RollingInterval.Day);
                }
            });
        }
    }
}
