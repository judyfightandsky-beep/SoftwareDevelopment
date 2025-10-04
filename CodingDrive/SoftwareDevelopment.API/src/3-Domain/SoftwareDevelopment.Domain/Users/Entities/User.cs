using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Shared;
using SoftwareDevelopment.Domain.Users.Events;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Entities;

/// <summary>
/// 使用者聚合根
/// 管理使用者的基本資訊、角色和狀態
/// </summary>
public sealed class User : AggregateRoot<UserId>, IAggregateRoot
{
    private readonly List<TeamMembership> _teamMemberships = [];
    private readonly List<ProjectMembership> _projectMemberships = [];

    private User(
        UserId id,
        Username username,
        Email email,
        string firstName,
        string lastName,
        UserRole role,
        UserStatus status,
        DateTime createdAt) : base(id)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Status = status;
        CreatedAt = createdAt;
        LastModifiedAt = createdAt;
    }

    /// <summary>
    /// 為了 EF Core 反序列化需要的無參數建構函數
    /// </summary>
    private User() : base(default!)
    {
        Username = default!;
        Email = default!;
        FirstName = default!;
        LastName = default!;
        Role = default!;
        Status = default!;
    }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public Username Username { get; private set; }

    /// <summary>
    /// 電子信箱
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// 名字
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// 姓氏
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// 使用者角色
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// 使用者狀態
    /// </summary>
    public UserStatus Status { get; private set; }

    /// <summary>
    /// 密碼雜湊
    /// </summary>
    public string? PasswordHash { get; private set; }

    /// <summary>
    /// 最後登入時間
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// 最後修改時間
    /// </summary>
    public DateTime LastModifiedAt { get; private set; }

    /// <summary>
    /// 團隊成員資格
    /// </summary>
    public IReadOnlyCollection<TeamMembership> TeamMemberships => _teamMemberships.AsReadOnly();

    /// <summary>
    /// 專案成員資格
    /// </summary>
    public IReadOnlyCollection<ProjectMembership> ProjectMemberships => _projectMemberships.AsReadOnly();

    /// <summary>
    /// 全名
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// 建立新使用者
    /// </summary>
    /// <param name="username">使用者名稱</param>
    /// <param name="email">電子信箱</param>
    /// <param name="firstName">名字</param>
    /// <param name="lastName">姓氏</param>
    /// <param name="companyDomains">公司網域清單</param>
    /// <returns>新使用者</returns>
    public static User Create(
        Username username,
        Email email,
        string firstName,
        string lastName,
        IEnumerable<string> companyDomains)
    {
        ArgumentNullException.ThrowIfNull(username);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentNullException.ThrowIfNull(companyDomains);

        var userId = UserId.NewId();
        var role = email.IsCompanyEmail(companyDomains) ? UserRole.Employee(userId) : UserRole.Guest();
        var status = role.Type == UserRoleType.Employee ? UserStatus.PendingApproval : UserStatus.PendingVerification;
        var now = DateTime.UtcNow;

        var user = new User(
            userId,
            username,
            email,
            firstName.Trim(),
            lastName.Trim(),
            role,
            status,
            now);

        user.AddDomainEvent(new UserCreatedEvent(userId, email, role.Type, now));

        return user;
    }

    /// <summary>
    /// 設定密碼
    /// </summary>
    /// <param name="passwordHash">密碼雜湊</param>
    public void SetPassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;
        UpdateLastModified();
    }

    /// <summary>
    /// 驗證密碼
    /// </summary>
    /// <param name="passwordHasher">密碼雜湊器</param>
    /// <param name="password">要驗證的密碼</param>
    /// <returns>是否密碼正確</returns>
    public bool VerifyPassword(IPasswordHasher passwordHasher, string password)
    {
        ArgumentNullException.ThrowIfNull(passwordHasher);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (Status != UserStatus.Active)
        {
            throw new InvalidOperationException("只有啟用狀態的使用者可以登入");
        }

        return passwordHasher.VerifyPassword(PasswordHash ?? "", password);
    }

    /// <summary>
    /// 驗證電子信箱
    /// </summary>
    public void VerifyEmail()
    {
        if (Status != UserStatus.PendingVerification)
        {
            throw new InvalidOperationException("只有待驗證狀態的使用者可以驗證電子信箱");
        }

        Status = Role.Type == UserRoleType.Employee ? UserStatus.PendingApproval : UserStatus.Active;
        UpdateLastModified();

        AddDomainEvent(new UserEmailVerifiedEvent(Id, Email, DateTime.UtcNow));
    }

    /// <summary>
    /// 核准使用者
    /// </summary>
    /// <param name="approvedBy">核准者識別碼</param>
    public void Approve(UserId approvedBy)
    {
        ArgumentNullException.ThrowIfNull(approvedBy);

        if (Status != UserStatus.PendingApproval)
        {
            throw new InvalidOperationException("只有待審核狀態的使用者可以被核准");
        }

        Status = UserStatus.Active;
        UpdateLastModified();

        AddDomainEvent(new UserApprovedEvent(Id, approvedBy, DateTime.UtcNow));
    }

    /// <summary>
    /// 拒絕使用者
    /// </summary>
    /// <param name="rejectedBy">拒絕者識別碼</param>
    /// <param name="reason">拒絕原因</param>
    public void Reject(UserId rejectedBy, string reason)
    {
        ArgumentNullException.ThrowIfNull(rejectedBy);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (Status != UserStatus.PendingApproval)
        {
            throw new InvalidOperationException("只有待審核狀態的使用者可以被拒絕");
        }

        Status = UserStatus.Inactive;
        UpdateLastModified();

        AddDomainEvent(new UserRejectedEvent(Id, rejectedBy, reason, DateTime.UtcNow));
    }

    /// <summary>
    /// 變更角色
    /// </summary>
    /// <param name="newRole">新角色</param>
    /// <param name="changedBy">變更者識別碼</param>
    public void ChangeRole(UserRole newRole, UserId changedBy)
    {
        ArgumentNullException.ThrowIfNull(newRole);
        ArgumentNullException.ThrowIfNull(changedBy);

        if (Status != UserStatus.Active)
        {
            throw new InvalidOperationException("只有啟用狀態的使用者可以變更角色");
        }

        var oldRole = Role;
        Role = newRole;
        UpdateLastModified();

        AddDomainEvent(new UserRoleChangedEvent(Id, oldRole.Type, newRole.Type, changedBy, DateTime.UtcNow));
    }

    /// <summary>
    /// 記錄登入
    /// </summary>
    public void RecordLogin()
    {
        if (Status != UserStatus.Active)
        {
            throw new InvalidOperationException("只有啟用狀態的使用者可以登入");
        }

        LastLoginAt = DateTime.UtcNow;
        UpdateLastModified();

        AddDomainEvent(new UserLoggedInEvent(Id, LastLoginAt.Value));
    }

    /// <summary>
    /// 停用使用者
    /// </summary>
    /// <param name="deactivatedBy">停用者識別碼</param>
    public void Deactivate(UserId deactivatedBy)
    {
        ArgumentNullException.ThrowIfNull(deactivatedBy);

        if (Status == UserStatus.Inactive)
        {
            return; // 已經是停用狀態
        }

        Status = UserStatus.Inactive;
        UpdateLastModified();

        AddDomainEvent(new UserDeactivatedEvent(Id, deactivatedBy, DateTime.UtcNow));
    }

    /// <summary>
    /// 啟用使用者
    /// </summary>
    /// <param name="activatedBy">啟用者識別碼</param>
    public void Activate(UserId activatedBy)
    {
        ArgumentNullException.ThrowIfNull(activatedBy);

        if (Status == UserStatus.Active)
        {
            return; // 已經是啟用狀態
        }

        if (Status == UserStatus.Blocked)
        {
            throw new InvalidOperationException("封鎖狀態的使用者無法直接啟用");
        }

        Status = UserStatus.Active;
        UpdateLastModified();

        AddDomainEvent(new UserActivatedEvent(Id, activatedBy, DateTime.UtcNow));
    }

    /// <summary>
    /// 檢查是否具有指定權限
    /// </summary>
    /// <param name="permission">權限</param>
    /// <returns>是否具有權限</returns>
    public bool HasPermission(Permission permission)
    {
        return Status == UserStatus.Active && Role.HasPermission(permission);
    }

    private void UpdateLastModified()
    {
        LastModifiedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// 團隊成員資格值物件
/// </summary>
public sealed class TeamMembership : ValueObject
{
    public TeamMembership(Guid teamId, TeamRole role, DateTime joinedAt)
    {
        TeamId = teamId;
        Role = role;
        JoinedAt = joinedAt;
    }

    public Guid TeamId { get; }
    public TeamRole Role { get; }
    public DateTime JoinedAt { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TeamId;
        yield return Role;
        yield return JoinedAt;
    }
}

/// <summary>
/// 專案成員資格值物件
/// </summary>
public sealed class ProjectMembership : ValueObject
{
    public ProjectMembership(Guid projectId, ProjectRole role, DateTime joinedAt, decimal allocationPercentage = 100)
    {
        ProjectId = projectId;
        Role = role;
        JoinedAt = joinedAt;
        AllocationPercentage = allocationPercentage;
    }

    public Guid ProjectId { get; }
    public ProjectRole Role { get; }
    public DateTime JoinedAt { get; }
    public decimal AllocationPercentage { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProjectId;
        yield return Role;
        yield return JoinedAt;
        yield return AllocationPercentage;
    }
}

/// <summary>
/// 團隊角色列舉
/// </summary>
public enum TeamRole
{
    Member,
    Lead,
    Senior,
    Junior
}

/// <summary>
/// 專案角色列舉
/// </summary>
public enum ProjectRole
{
    Developer,
    TechLead,
    ProjectManager,
    Tester,
    Designer
}
