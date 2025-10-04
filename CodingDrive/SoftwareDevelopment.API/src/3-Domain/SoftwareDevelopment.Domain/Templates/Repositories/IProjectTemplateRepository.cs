using SoftwareDevelopment.Domain.Templates.Entities;
using SoftwareDevelopment.Domain.Templates.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Repositories;

/// <summary>
/// 專案模板倉儲介面
/// </summary>
public interface IProjectTemplateRepository
{
    /// <summary>
    /// 根據 ID 取得專案模板
    /// </summary>
    /// <param name="id">模板 ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>專案模板</returns>
    Task<ProjectTemplate?> GetByIdAsync(TemplateId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜尋專案模板
    /// </summary>
    /// <param name="searchTerm">搜尋詞</param>
    /// <param name="type">模板類型</param>
    /// <param name="status">模板狀態</param>
    /// <param name="tags">標籤</param>
    /// <param name="pageNumber">頁碼</param>
    /// <param name="pageSize">頁面大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>專案模板列表和總數</returns>
    Task<(IReadOnlyList<ProjectTemplate> Templates, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        TemplateType? type = null,
        TemplateStatus? status = null,
        IEnumerable<string>? tags = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得熱門專案模板
    /// </summary>
    /// <param name="count">數量</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>熱門專案模板列表</returns>
    Task<IReadOnlyList<ProjectTemplate>> GetPopularTemplatesAsync(
        int count = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 新增專案模板
    /// </summary>
    /// <param name="template">專案模板</param>
    /// <param name="cancellationToken">取消權杖</param>
    Task AddAsync(ProjectTemplate template, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新專案模板
    /// </summary>
    /// <param name="template">專案模板</param>
    /// <param name="cancellationToken">取消權杖</param>
    Task UpdateAsync(ProjectTemplate template, CancellationToken cancellationToken = default);

    /// <summary>
    /// 刪除專案模板
    /// </summary>
    /// <param name="template">專案模板</param>
    /// <param name="cancellationToken">取消權杖</param>
    Task DeleteAsync(ProjectTemplate template, CancellationToken cancellationToken = default);
}