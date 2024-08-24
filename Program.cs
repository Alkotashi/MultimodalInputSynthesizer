using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace MyWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildAndRunHost(args);
        }

        public static IHostBuilder BuildAndRunHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                    string environmentVariablesPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
                    if (File.Exists(environmentVariablesPath))
                    {
                        DotNetEnv.Env.Load(environmentVariablesPath);
                    }
                    webHostBuilder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
                    {
                        configBuilder.AddEnvironmentVariables();
                    });
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {
        }
    }
}