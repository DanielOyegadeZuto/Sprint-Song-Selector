using Microsoft.OpenApi.Models;
using Song_Selector_app.Services;
using Song_Selector_app;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((_, services) =>
{
    // Add HttpClientFactory
    services.AddHttpClient();

    // Add SpotifyService as a service
    services.AddScoped<ISpotifyService, SpotifyService>();

    // Add GetAccessTokens as a service
    services.AddScoped<GetAccessTokens>();

    // Add logging
    services.AddLogging();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Song-Selector", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();