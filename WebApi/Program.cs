using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
 
builder.Services.AddOpenApi();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    //only for testing upload in dev mode
    app.MapUpload();
    app.MapOpenApi();
    // Serve files from wwwroot and use default files (e.g. index.html)
    app.UseDefaultFiles();
    app.UseStaticFiles();

}
 


app.MapStaticAssets();
app.Run();
 
// Partial Program class to enable integration tests
public partial class Program { }