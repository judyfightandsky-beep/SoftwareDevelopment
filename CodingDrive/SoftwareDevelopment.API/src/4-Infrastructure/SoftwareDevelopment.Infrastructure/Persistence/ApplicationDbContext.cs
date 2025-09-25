using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users;
using SoftwareDevelopment.Infrastructure.Persistence.Configurations;

namespace SoftwareDevelopment.Infrastructure.Persistence;

/// <summary>
/// 應用程式資料庫內容
/// </summary>
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// 初始化資料庫內容
    /// </summary>
    /// <param name="options">資料庫內容選項</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 使用者實體集
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// 配置模型
    /// </summary>
    /// <param name="modelBuilder">模型建構器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 套用所有配置
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// 儲存變更前的處理
    /// </summary>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>受影響的資料列數</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 在此處可以添加領域事件分派邏輯
        return await base.SaveChangesAsync(cancellationToken);
    }
}
