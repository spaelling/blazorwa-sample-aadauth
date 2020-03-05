using System;
using Microsoft.AspNetCore.Blazor.Hosting;
using Serilog;
using Serilog.Core;

namespace blazor.wa.aadauth.sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var levelSwitch = new LoggingLevelSwitch();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
                .WriteTo.BrowserHttp(controlLevelSwitch: levelSwitch)
                .WriteTo.BrowserConsole()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();            
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
