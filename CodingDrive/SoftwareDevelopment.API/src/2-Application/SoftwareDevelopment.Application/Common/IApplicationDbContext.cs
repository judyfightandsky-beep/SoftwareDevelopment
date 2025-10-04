using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Templates.Entities;
using SoftwareDevelopment.Domain.Tasks.Entities;

namespace SoftwareDevelopment.Application.Common;

/// <summary>
/// 應用程式資料庫內容介面
/// 定義應用層對資料庫的訪問契約
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// 使用者實體集
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// 驗證權杖實體集
    /// </summary>
    DbSet<VerificationToken> VerificationTokens { get; }

    /// <summary>
    /// 專案模板實體集
    /// </summary>
    DbSet<ProjectTemplate> ProjectTemplates { get; }

    /// <summary>
    /// 任務實體集
    /// </summary>
    DbSet<SoftwareDevelopment.Domain.Tasks.Entities.Task> Tasks { get; }

    /// <summary>
    /// 儲存變更
    /// </summary>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>受影響的資料列數</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
