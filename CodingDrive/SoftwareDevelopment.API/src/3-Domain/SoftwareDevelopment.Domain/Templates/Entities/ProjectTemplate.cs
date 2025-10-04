using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Templates.Events;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Entities;

/// <summary>
/// 專案模板聚合根
/// </summary>
public sealed class ProjectTemplate : AggregateRoot<TemplateId>, IAggregateRoot
{
    // EF Core 需要的無參數建構函式
    private ProjectTemplate() : base(TemplateId.From(Guid.NewGuid()))
    {
        Name = string.Empty;
        Description = string.Empty;
        Type = TemplateType.WebApplication;
        CreatedBy = UserId.From(Guid.Empty);
        Configuration = TemplateConfiguration.Create("{}");
        Metadata = TemplateMetadata.Create("1.0.0", new List<string>());
        Status = TemplateStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
        DownloadCount = 0;
    }

    private ProjectTemplate(
        TemplateId id,
        string name,
        string description,
        TemplateType type,
        UserId createdBy,
        TemplateConfiguration configuration,
        TemplateMetadata metadata,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Description = description;
        Type = type;
        CreatedBy = createdBy;
        Configuration = configuration;
        Metadata = metadata;
        Status = TemplateStatus.Draft;
        CreatedAt = createdAt;
        LastModifiedAt = createdAt;
        DownloadCount = 0;
    }

    /// <summary>
    /// 模板名稱
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 模板描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 模板類型
    /// </summary>
    public TemplateType Type { get; private set; }

    /// <summary>
    /// 模板狀態
    /// </summary>
    public TemplateStatus Status { get; private set; }

    /// <summary>
    /// 創建者
    /// </summary>
    public UserId CreatedBy { get; }

    /// <summary>
    /// 模板配置
    /// </summary>
    public TemplateConfiguration Configuration { get; private set; }

    /// <summary>
    /// 模板元數據
    /// </summary>
    public TemplateMetadata Metadata { get; private set; }

    /// <summary>
    /// 下載次數
    /// </summary>
    public int DownloadCount { get; private set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// 最後修改時間
    /// </summary>
    public DateTime LastModifiedAt { get; private set; }

    /// <summary>
    /// 創建新的專案模板
    /// </summary>
    /// <param name="name">模板名稱</param>
    /// <param name="description">模板描述</param>
    /// <param name="type">模板類型</param>
    /// <param name="createdBy">創建者</param>
    /// <param name="configuration">模板配置</param>
    /// <param name="metadata">模板元數據</param>
    /// <returns>專案模板</returns>
    public static ProjectTemplate Create(
        string name,
        string description,
        TemplateType type,
        UserId createdBy,
        TemplateConfiguration? configuration = null,
        TemplateMetadata? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("模板名稱不得為空", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("模板描述不得為空", nameof(description));
        }

        var template = new ProjectTemplate(
            TemplateId.NewId(),
            name,
            description,
            type,
            createdBy,
            configuration ?? TemplateConfiguration.Empty,
            metadata ?? TemplateMetadata.Default,
            DateTime.UtcNow);

        // 觸發模板建立事件
        template.AddDomainEvent(new TemplateCreatedEvent(
            template.Id,
            template.Name,
            template.Type,
            template.CreatedBy,
            DateTime.UtcNow));

        return template;
    }

    /// <summary>
    /// 更新模板資訊
    /// </summary>
    /// <param name="name">模板名稱</param>
    /// <param name="description">模板描述</param>
    /// <param name="type">模板類型</param>
    /// <param name="configuration">模板配置</param>
    /// <param name="metadata">模板元數據</param>
    public void Update(
        string name,
        string description,
        TemplateType type,
        TemplateConfiguration? configuration = null,
        TemplateMetadata? metadata = null)
    {
        if (Status == TemplateStatus.Deleted)
        {
            throw new InvalidOperationException("無法更新已刪除的模板");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("模板名稱不得為空", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("模板描述不得為空", nameof(description));
        }

        Name = name;
        Description = description;
        Type = type;
        Configuration = configuration ?? Configuration;
        Metadata = metadata ?? Metadata;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 發布模板
    /// </summary>
    public void Publish()
    {
        if (Status == TemplateStatus.Deleted)
        {
            throw new InvalidOperationException("無法發布已刪除的模板");
        }

        Status = TemplateStatus.Published;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發模板發布事件
        AddDomainEvent(new TemplatePublishedEvent(
            Id,
            Name,
            Metadata.Version,
            CreatedBy, // TODO: 應該從上下文取得當前使用者
            DateTime.UtcNow,
            DateTime.UtcNow));
    }

    /// <summary>
    /// 封存模板
    /// </summary>
    /// <param name="archivedBy">封存者 ID</param>
    public void Archive(UserId archivedBy)
    {
        if (Status == TemplateStatus.Deleted)
        {
            throw new InvalidOperationException("無法封存已刪除的模板");
        }

        var oldStatus = Status;
        Status = TemplateStatus.Archived;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發模板狀態變更事件
        AddDomainEvent(new TemplateStatusChangedEvent(
            Id,
            oldStatus,
            Status,
            archivedBy,
            DateTime.UtcNow));
    }

    /// <summary>
    /// 刪除模板
    /// </summary>
    /// <param name="deletedBy">刪除者 ID</param>
    public void Delete(UserId deletedBy)
    {
        var oldStatus = Status;
        Status = TemplateStatus.Deleted;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發模板狀態變更事件
        AddDomainEvent(new TemplateStatusChangedEvent(
            Id,
            oldStatus,
            Status,
            deletedBy,
            DateTime.UtcNow));
    }

    /// <summary>
    /// 增加下載次數
    /// </summary>
    /// <param name="downloadedBy">下載者 ID</param>
    public void IncrementDownloadCount(UserId downloadedBy)
    {
        if (Status != TemplateStatus.Published)
        {
            throw new InvalidOperationException("只有已發布的模板才能下載");
        }

        DownloadCount++;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發模板下載事件
        AddDomainEvent(new TemplateDownloadedEvent(
            Id,
            Name,
            downloadedBy,
            DownloadCount,
            DateTime.UtcNow));
    }

    /// <summary>
    /// 檢查是否可以下載
    /// </summary>
    /// <returns>是否可以下載</returns>
    public bool CanDownload() => Status == TemplateStatus.Published;
}