using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models; 
using Song_Selector_app.Controllers;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a host builder
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Add controllers as services
                services.AddControllers();

                // Add HttpClientFactory
                services.AddHttpClient();

                // Add SongSelectorController as a service
                services.AddScoped<SongSelectorController>();

                // Add HealthChecker as a service
                services.AddScoped<IHealthChecker, BasicHealthChecker>();

                // Add Swagger services
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Song-Selector", Version = "v1" });
                });
            });

        // Build and run the host
        var host = hostBuilder.Build();
         
    }
}
