using Serilog;
using SoftwareDevelopment.Api.Extensions;
using SoftwareDevelopment.Api.Middleware;
using SoftwareDevelopment.Application.Extensions;
using SoftwareDevelopment.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 配置 Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 註冊應用程式服務
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);

var app = builder.Build();

// 配置 HTTP 管線
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SoftwareDevelopment API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();

// 只在有靜態檔案目錄時才使用 fallback
var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (Directory.Exists(webRootPath))
{
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
}

try
{
    Log.Information("Starting SoftwareDevelopment API");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "SoftwareDevelopment API terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
