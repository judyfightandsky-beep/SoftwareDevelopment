using SoftwareDevelopment.Domain.Tasks.Entities;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Shared.ValueObjects;
using TaskStatus = SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus;

namespace SoftwareDevelopment.Domain.Tasks.Repositories;

/// <summary>
/// 任務倉儲介面
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// 根據 ID 取得任務
    /// </summary>
    /// <param name="id">任務 ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>任務</returns>
    System.Threading.Tasks.Task<Entities.Task?> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜尋任務
    /// </summary>
    /// <param name="projectId">專案 ID</param>
    /// <param name="assignedTo">指派給</param>
    /// <param name="status">任務狀態</param>
    /// <param name="type">任務類型</param>
    /// <param name="priority">優先級</param>
    /// <param name="searchTerm">搜尋詞</param>
    /// <param name="pageNumber">頁碼</param>
    /// <param name="pageSize">頁面大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>任務列表和總數</returns>
    System.Threading.Tasks.Task<(IReadOnlyList<Entities.Task> Tasks, int TotalCount)> SearchAsync(
        ProjectId? projectId = null,
        UserId? assignedTo = null,
        TaskStatus? status = null,
        TaskType? type = null,
        Priority? priority = null,
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得使用者的任務
    /// </summary>
    /// <param name="userId">使用者 ID</param>
    /// <param name="status">任務狀態</param>
    /// <param name="pageNumber">頁碼</param>
    /// <param name="pageSize">頁面大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>任務列表和總數</returns>
    System.Threading.Tasks.Task<(IReadOnlyList<Entities.Task> Tasks, int TotalCount)> GetUserTasksAsync(
        UserId userId,
        TaskStatus? status = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得逾期任務
    /// </summary>
    /// <param name="userId">使用者 ID（可選）</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>逾期任務列表</returns>
    System.Threading.Tasks.Task<IReadOnlyList<Entities.Task>> GetOverdueTasksAsync(
        UserId? userId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 新增任務
    /// </summary>
    /// <param name="task">任務</param>
    /// <param name="cancellationToken">取消權杖</param>
    System.Threading.Tasks.Task AddAsync(Entities.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新任務
    /// </summary>
    /// <param name="task">任務</param>
    /// <param name="cancellationToken">取消權杖</param>
    System.Threading.Tasks.Task UpdateAsync(Entities.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// 刪除任務
    /// </summary>
    /// <param name="task">任務</param>
    /// <param name="cancellationToken">取消權杖</param>
    System.Threading.Tasks.Task DeleteAsync(Entities.Task task, CancellationToken cancellationToken = default);
}