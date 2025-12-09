using BHS.Infrastructure.IoC;
using BHS.Web;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureAppConfiguration();

builder.Services.AddBhsAuth();

builder.Services.AddControllers()
        .AddBhs400Logging();

builder.Services.AddBhsHealthChecks();
builder.Services.AddBhsSwagger();

builder.Services.AddBhsServices();


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error-local-development");
}
else
{
    app.UseExceptionHandler("/api/error");
    app.UseHsts();
}

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseHttpsRedirection();

app.UseBhsSwagger();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

await app.RunAsync();
