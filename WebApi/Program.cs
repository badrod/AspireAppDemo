
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using WebApi.Extensions;
using WebApi.Models;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
// Configure settings from appsettings.json as typed settings object
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<Settings>(builder.Configuration);
 

// Adds default health checks, logging, open telemetry, service discovery, and http client defaults
builder.AddServiceDefaults();


builder.Services.AddSingleton<IAIService, OpenAIService>();
//builder.Services.AddSingleton<IAIService, AzureOpenAIService>();

builder.Services.AddOpenApi();
var app = builder.Build();

//maps healthchecks and telemetry
app.MapDefaultEndpoints();

//our custom api endpints
app.MapApiEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api");
 
    // Serve files from wwwroot and use default files (e.g. index.html)
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapStaticAssets();
    
}


app.Run();

// Partial Program class to enable integration tests, Is now default and not needed to be added manually
//public partial class Program { }
