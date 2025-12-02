
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.AspNetCore;
using Scalar.AspNetCore;
using WebApi.Extensions;
using WebApi.Models;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Reload appsettings.json when changed
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Env var for read local secrets for api keys during development, use keyvault if in az

//To handle feature flags
builder.Services.AddFeatureManagement();

//Bind our custom typed Settings 
builder.Services.Configure<Settings>(builder.Configuration);

// Adds default health checks, logging, open telemetry, service discovery, and http client defaults
builder.AddServiceDefaults();

builder.Services.AddOpenApi();

//Custom services
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<GoogleAIService>();
// Register the middleware router as the single IAIService
builder.Services.AddScoped<IAIService, ToggleAIService>();


var app = builder.Build();

// maps healthchecks and telemetry
app.MapDefaultEndpoints();

// our custom api endpoints
app.MapApiEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.Logging.AddConsole();
    app.MapOpenApi();
    app.MapScalarApiReference("api");

    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapStaticAssets();
}
// Feature-gated endpoint 
// Returns 404 if feature is disabled, no need for app restart if toggled
app.MapGet("/feature", () =>
{
    // Some new feature logic

}).WithFeatureGate("EnableNewFeature");




app.Run();