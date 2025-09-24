using Serilog;
using DevAuth.Api.Extensions;
using DevAuth.Api.Middleware;
using DevAuth.Application.Extensions;
using DevAuth.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 配置 Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 配置服務
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊應用程式服務
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

var app = builder.Build();

// 配置 HTTP 管線
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting DevAuth API");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "DevAuth API terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}