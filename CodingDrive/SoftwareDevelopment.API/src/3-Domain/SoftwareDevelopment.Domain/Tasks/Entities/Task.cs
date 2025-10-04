using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Tasks.Events;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Shared.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.Entities;

/// <summary>
/// 任務聚合根
/// </summary>
public sealed class Task : AggregateRoot<TaskId>, IAggregateRoot
{
    // EF Core 需要的無參數建構函式
    private Task() : base(TaskId.From(Guid.NewGuid()))
    {
        Title = string.Empty;
        Description = string.Empty;
        Type = TaskType.Feature;
        Priority = Priority.Medium;
        ProjectId = ProjectId.From(Guid.Empty);
        CreatedBy = UserId.From(Guid.Empty);
        EstimatedHours = EstimatedHours.Zero;
        ActualHours = ActualHours.Zero;
        Status = ValueObjects.TaskStatus.Todo;
        CreatedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
    }

    private Task(
        TaskId id,
        string title,
        string description,
        TaskType type,
        Priority priority,
        ProjectId projectId,
        UserId createdBy,
        EstimatedHours estimatedHours,
        DateTime createdAt) : base(id)
    {
        Title = title;
        Description = description;
        Type = type;
        Priority = priority;
        ProjectId = projectId;
        CreatedBy = createdBy;
        EstimatedHours = estimatedHours;
        ActualHours = ActualHours.Zero;
        Status = ValueObjects.TaskStatus.Todo;
        CreatedAt = createdAt;
        LastModifiedAt = createdAt;
    }

    /// <summary>
    /// 任務標題
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// 任務描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 任務類型
    /// </summary>
    public TaskType Type { get; private set; }

    /// <summary>
    /// 任務狀態
    /// </summary>
    public ValueObjects.TaskStatus Status { get; private set; }

    /// <summary>
    /// 優先級
    /// </summary>
    public Priority Priority { get; private set; }

    /// <summary>
    /// 所屬專案
    /// </summary>
    public ProjectId ProjectId { get; private set; }

    /// <summary>
    /// 創建者
    /// </summary>
    public UserId CreatedBy { get; }

    /// <summary>
    /// 指派給
    /// </summary>
    public UserId? AssignedTo { get; private set; }

    /// <summary>
    /// 預估工時
    /// </summary>
    public EstimatedHours EstimatedHours { get; private set; }

    /// <summary>
    /// 實際工時
    /// </summary>
    public ActualHours ActualHours { get; private set; }

    /// <summary>
    /// 到期日
    /// </summary>
    public DateTime? DueDate { get; private set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// 最後修改時間
    /// </summary>
    public DateTime LastModifiedAt { get; private set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// 完成時間
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    /// 創建新任務
    /// </summary>
    /// <param name="title">任務標題</param>
    /// <param name="description">任務描述</param>
    /// <param name="type">任務類型</param>
    /// <param name="priority">優先級</param>
    /// <param name="projectId">專案 ID</param>
    /// <param name="createdBy">創建者</param>
    /// <param name="estimatedHours">預估工時</param>
    /// <param name="assignedTo">指派給</param>
    /// <param name="dueDate">到期日</param>
    /// <returns>新任務</returns>
    public static Task Create(
        string title,
        string description,
        TaskType type,
        Priority priority,
        ProjectId projectId,
        UserId createdBy,
        EstimatedHours? estimatedHours = null,
        UserId? assignedTo = null,
        DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("任務標題不得為空", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("任務描述不得為空", nameof(description));
        }

        var task = new Task(
            TaskId.NewId(),
            title,
            description,
            type,
            priority,
            projectId,
            createdBy,
            estimatedHours ?? EstimatedHours.Zero,
            DateTime.UtcNow);

        if (assignedTo != null)
        {
            task.AssignTo(assignedTo);
        }

        if (dueDate.HasValue)
        {
            task.SetDueDate(dueDate.Value);
        }

        // 觸發任務建立事件
        task.AddDomainEvent(new TaskCreatedEvent(
            task.Id,
            task.Title,
            task.Type,
            task.Priority,
            task.ProjectId,
            task.CreatedBy,
            DateTime.UtcNow));

        return task;
    }

    /// <summary>
    /// 更新任務資訊
    /// </summary>
    /// <param name="title">任務標題</param>
    /// <param name="description">任務描述</param>
    /// <param name="type">任務類型</param>
    /// <param name="priority">優先級</param>
    /// <param name="estimatedHours">預估工時</param>
    /// <param name="dueDate">到期日</param>
    public void Update(
        string title,
        string description,
        TaskType type,
        Priority priority,
        EstimatedHours? estimatedHours = null,
        DateTime? dueDate = null)
    {
        if (Status == ValueObjects.TaskStatus.Done || Status == ValueObjects.TaskStatus.Cancelled || Status == ValueObjects.TaskStatus.Archived)
        {
            throw new InvalidOperationException("無法更新已完成、已取消或已封存的任務");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("任務標題不得為空", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("任務描述不得為空", nameof(description));
        }

        Title = title;
        Description = description;
        Type = type;
        Priority = priority;
        EstimatedHours = estimatedHours ?? EstimatedHours;
        DueDate = dueDate;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 指派任務
    /// </summary>
    /// <param name="userId">使用者 ID</param>
    public void AssignTo(UserId userId)
    {
        var previousAssignee = AssignedTo;
        AssignedTo = userId;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發任務指派事件
        AddDomainEvent(new TaskAssignedEvent(
            Id,
            previousAssignee,
            userId,
            userId, // TODO: 應該從上下文取得當前使用者
            DateTime.UtcNow));
    }

    /// <summary>
    /// 取消指派
    /// </summary>
    public void Unassign()
    {
        AssignedTo = null;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 設定到期日
    /// </summary>
    /// <param name="dueDate">到期日</param>
    public void SetDueDate(DateTime dueDate)
    {
        if (dueDate < DateTime.UtcNow.Date)
        {
            throw new ArgumentException("到期日不得為過去時間", nameof(dueDate));
        }

        DueDate = dueDate;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 開始任務
    /// </summary>
    public void Start()
    {
        if (Status != ValueObjects.TaskStatus.Todo)
        {
            throw new InvalidOperationException("只有待辦狀態的任務才能開始");
        }

        Status = ValueObjects.TaskStatus.InProgress;
        StartedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 完成任務
    /// </summary>
    public void Complete()
    {
        if (Status == ValueObjects.TaskStatus.Done)
        {
            throw new InvalidOperationException("任務已經完成");
        }

        if (Status == ValueObjects.TaskStatus.Cancelled || Status == ValueObjects.TaskStatus.Archived)
        {
            throw new InvalidOperationException("無法完成已取消或已封存的任務");
        }

        Status = ValueObjects.TaskStatus.Done;
        CompletedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發任務完成事件
        AddDomainEvent(new TaskCompletedEvent(
            Id,
            Title,
            AssignedTo ?? CreatedBy, // 如果沒有指派人，則使用創建者
            ActualHours,
            EstimatedHours,
            CompletedAt.Value,
            DateTime.UtcNow));
    }

    /// <summary>
    /// 取消任務
    /// </summary>
    public void Cancel()
    {
        if (Status == ValueObjects.TaskStatus.Done)
        {
            throw new InvalidOperationException("無法取消已完成的任務");
        }

        if (Status == ValueObjects.TaskStatus.Cancelled)
        {
            throw new InvalidOperationException("任務已經取消");
        }

        Status = ValueObjects.TaskStatus.Cancelled;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 更新狀態
    /// </summary>
    /// <param name="status">新狀態</param>
    public void UpdateStatus(ValueObjects.TaskStatus status)
    {
        if (Status == ValueObjects.TaskStatus.Archived)
        {
            throw new InvalidOperationException("無法更改已封存任務的狀態");
        }

        if (status == ValueObjects.TaskStatus.InProgress && Status == ValueObjects.TaskStatus.Todo && StartedAt == null)
        {
            StartedAt = DateTime.UtcNow;
        }

        if (status == ValueObjects.TaskStatus.Done && Status != ValueObjects.TaskStatus.Done)
        {
            CompletedAt = DateTime.UtcNow;
        }

        var oldStatus = Status;
        Status = status;
        LastModifiedAt = DateTime.UtcNow;

        // 觸發任務狀態變更事件
        if (oldStatus != status)
        {
            AddDomainEvent(new TaskStatusChangedEvent(
                Id,
                oldStatus,
                status,
                AssignedTo ?? CreatedBy, // TODO: 應該從上下文取得當前使用者
                DateTime.UtcNow));
        }
    }

    /// <summary>
    /// 記錄工時
    /// </summary>
    /// <param name="hours">工時</param>
    public void LogHours(decimal hours)
    {
        if (hours <= 0)
        {
            throw new ArgumentException("工時必須大於零", nameof(hours));
        }

        ActualHours = ActualHours.Create(ActualHours.Value + hours);
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 封存任務
    /// </summary>
    public void Archive()
    {
        Status = ValueObjects.TaskStatus.Archived;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 檢查是否逾期
    /// </summary>
    /// <returns>是否逾期</returns>
    public bool IsOverdue()
    {
        return DueDate.HasValue &&
               DateTime.UtcNow.Date > DueDate.Value.Date &&
               Status != ValueObjects.TaskStatus.Done &&
               Status != ValueObjects.TaskStatus.Cancelled &&
               Status != ValueObjects.TaskStatus.Archived;
    }
}