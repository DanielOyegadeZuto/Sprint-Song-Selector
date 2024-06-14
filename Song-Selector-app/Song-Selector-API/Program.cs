using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Song_Selector_app;
using Song_Selector_app.Controllers;
using Song_Selector_app.Services;


// Create a host builder
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((_, services) =>
    {
        // Add HttpClientFactory
        services.AddHttpClient();

        //// Add SongSelectorController as a service
        services.AddScoped<ISpotifyService, SpotifyService>();
        //services.AddScoped<SongSelectorController>();

        //// Add HealthChecker as a service
        //services.AddScoped<IHealthChecker, BasicHealthChecker>();

    });

// Build and run the host
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Song-Selector", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


