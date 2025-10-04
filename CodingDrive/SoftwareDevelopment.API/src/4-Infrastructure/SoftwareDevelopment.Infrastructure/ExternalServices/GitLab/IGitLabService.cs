namespace SoftwareDevelopment.Infrastructure.ExternalServices.GitLab;

/// <summary>
/// GitLab 服務介面
/// </summary>
public interface IGitLabService
{
    /// <summary>
    /// 創建 Issue
    /// </summary>
    Task<GitLabIssueDto> CreateIssueAsync(CreateGitLabIssueRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得 Issue 詳情
    /// </summary>
    Task<GitLabIssueDto?> GetIssueAsync(string projectId, int issueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新 Issue
    /// </summary>
    Task<GitLabIssueDto> UpdateIssueAsync(string projectId, int issueId, UpdateGitLabIssueRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 關閉 Issue
    /// </summary>
    Task<GitLabIssueDto> CloseIssueAsync(string projectId, int issueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 創建 Merge Request
    /// </summary>
    Task<GitLabMergeRequestDto> CreateMergeRequestAsync(CreateGitLabMergeRequestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得 Merge Request 詳情
    /// </summary>
    Task<GitLabMergeRequestDto?> GetMergeRequestAsync(string projectId, int mergeRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 合併 Merge Request
    /// </summary>
    Task<GitLabMergeRequestDto> MergeMergeRequestAsync(string projectId, int mergeRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得專案資訊
    /// </summary>
    Task<GitLabProjectDto?> GetProjectAsync(string projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得分支列表
    /// </summary>
    Task<IEnumerable<GitLabBranchDto>> GetBranchesAsync(string projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 創建分支
    /// </summary>
    Task<GitLabBranchDto> CreateBranchAsync(string projectId, CreateGitLabBranchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 檢查連接狀態
    /// </summary>
    Task<bool> CheckConnectionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// GitLab Issue DTO
/// </summary>
public record GitLabIssueDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public GitLabUserDto? Author { get; init; }
    public GitLabUserDto? Assignee { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public string WebUrl { get; init; } = string.Empty;
    public string[] Labels { get; init; } = Array.Empty<string>();
}

/// <summary>
/// GitLab Merge Request DTO
/// </summary>
public record GitLabMergeRequestDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string SourceBranch { get; init; } = string.Empty;
    public string TargetBranch { get; init; } = string.Empty;
    public GitLabUserDto? Author { get; init; }
    public GitLabUserDto? Assignee { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime? MergedAt { get; init; }
    public string WebUrl { get; init; } = string.Empty;
    public bool HasConflicts { get; init; }
    public string MergeStatus { get; init; } = string.Empty;
}

/// <summary>
/// GitLab 專案 DTO
/// </summary>
public record GitLabProjectDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string WebUrl { get; init; } = string.Empty;
    public string HttpUrlToRepo { get; init; } = string.Empty;
    public string SshUrlToRepo { get; init; } = string.Empty;
    public string DefaultBranch { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime LastActivityAt { get; init; }
}

/// <summary>
/// GitLab 分支 DTO
/// </summary>
public record GitLabBranchDto
{
    public string Name { get; init; } = string.Empty;
    public bool Merged { get; init; }
    public bool Protected { get; init; }
    public bool Default { get; init; }
    public GitLabCommitDto? Commit { get; init; }
}

/// <summary>
/// GitLab 提交 DTO
/// </summary>
public record GitLabCommitDto
{
    public string Id { get; init; } = string.Empty;
    public string ShortId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public GitLabUserDto? Author { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// GitLab 用戶 DTO
/// </summary>
public record GitLabUserDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string AvatarUrl { get; init; } = string.Empty;
}

/// <summary>
/// 創建 GitLab Issue 請求
/// </summary>
public record CreateGitLabIssueRequest
{
    public string ProjectId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int? AssigneeId { get; init; }
    public string[] Labels { get; init; } = Array.Empty<string>();
}

/// <summary>
/// 更新 GitLab Issue 請求
/// </summary>
public record UpdateGitLabIssueRequest
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? State { get; init; }
    public int? AssigneeId { get; init; }
    public string[]? Labels { get; init; }
}

/// <summary>
/// 創建 GitLab Merge Request 請求
/// </summary>
public record CreateGitLabMergeRequestRequest
{
    public string ProjectId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string SourceBranch { get; init; } = string.Empty;
    public string TargetBranch { get; init; } = "main";
    public int? AssigneeId { get; init; }
}

/// <summary>
/// 創建 GitLab 分支請求
/// </summary>
public record CreateGitLabBranchRequest
{
    public string BranchName { get; init; } = string.Empty;
    public string Ref { get; init; } = "main";
}