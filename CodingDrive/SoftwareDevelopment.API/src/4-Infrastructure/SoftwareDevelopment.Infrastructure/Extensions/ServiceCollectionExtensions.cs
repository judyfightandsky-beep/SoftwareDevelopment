using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Services;
using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.Repositories;
using SoftwareDevelopment.Domain.Tasks.Repositories;
using SoftwareDevelopment.Domain.Users.Repositories;
using SoftwareDevelopment.Domain.Users.Events;
using SoftwareDevelopment.Infrastructure.Persistence;
using SoftwareDevelopment.Infrastructure.Persistence.Repositories;
using SoftwareDevelopment.Infrastructure.Services;
using SoftwareDevelopment.Infrastructure.EventHandlers;

namespace SoftwareDevelopment.Infrastructure.Extensions;

/// <summary>
/// 基礎設施層服務註冊擴充方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊基礎設施層服務
    /// </summary>
    /// <param name="services">服務集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服務集合</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 註冊資料庫內容
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);

            // 開發環境啟用敏感資料記錄
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // 註冊資料庫內容介面
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // 註冊儲存庫
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IProjectTemplateRepository, ProjectTemplateRepository>();

        // TODO: 等 GitLab 整合服務實作完成後再啟用
        // services.Configure<GitLabConfiguration>(configuration.GetSection(GitLabConfiguration.ConfigurationKey));
        // services.AddHttpClient<IGitLabService, GitLabService>(client =>
        // {
        //     client.Timeout = TimeSpan.FromSeconds(30);
        // });

        // 註冊應用服務
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        // 註冊基礎設施層的事件處理器
        services.AddScoped<INotificationHandler<DomainEventNotification<UserCreatedEvent>>, UserCreatedEmailNotificationHandler>();

        return services;
    }
}


/// <summary>
/// BCrypt 密碼雜湊實作
/// </summary>
public sealed class BCryptPasswordHasher : IPasswordHasher
{
    /// <summary>
    /// 雜湊密碼
    /// </summary>
    /// <param name="password">明文密碼</param>
    /// <returns>雜湊後的密碼</returns>
    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// 驗證密碼
    /// </summary>
    /// <param name="hashedPassword">雜湊密碼</param>
    /// <param name="providedPassword">提供的密碼</param>
    /// <returns>是否匹配</returns>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);
        ArgumentException.ThrowIfNullOrWhiteSpace(providedPassword);

        try
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
        catch
        {
            return false;
        }
    }
}
