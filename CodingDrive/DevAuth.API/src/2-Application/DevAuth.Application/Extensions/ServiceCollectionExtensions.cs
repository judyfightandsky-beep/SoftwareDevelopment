using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace DevAuth.Application.Extensions;

/// <summary>
/// 應用層服務註冊擴充方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊應用層服務
    /// </summary>
    /// <param name="services">服務集合</param>
    /// <returns>服務集合</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 註冊 MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        // 註冊 FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}