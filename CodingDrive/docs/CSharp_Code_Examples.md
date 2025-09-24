# C#程式碼範例 - DDD架構實作

## 1. Domain Layer 實作範例

### 1.1 基礎建設類別

#### Entity基礎類別
```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuth.Domain.Common
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        protected Entity() { }

        protected Entity(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            Id = id;
        }

        public TId Id { get; protected set; }

        public override bool Equals(object obj)
        {
            return obj is Entity<TId> entity && Id.Equals(entity.Id);
        }

        public bool Equals(Entity<TId> other)
        {
            return other != null && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !(left == right);
        }
    }
}
```

#### AggregateRoot基礎類別
```csharp
using MediatR;

namespace UserAuth.Domain.Common
{
    public abstract class AggregateRoot<TId> : Entity<TId>
    {
        private readonly List<INotification> _domainEvents = new();

        protected AggregateRoot() { }
        protected AggregateRoot(TId id) : base(id) { }

        [NotMapped]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void RemoveDomainEvent(INotification domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}
```

#### DomainEvent基礎類別
```csharp
using MediatR;

namespace UserAuth.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}
```

### 1.2 值物件實作

#### Email值物件
```csharp
using System.Text.RegularExpressions;

namespace UserAuth.Domain.ValueObjects
{
    public record Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; init; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("電子信箱不能為空或空白", nameof(value));

            if (value.Length > 254) // RFC 5321 限制
                throw new ArgumentException("電子信箱長度不能超過254個字元", nameof(value));

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("無效的電子信箱格式", nameof(value));

            Value = value.ToLowerInvariant().Trim();
        }

        public static implicit operator string(Email email) => email.Value;
        public static explicit operator Email(string value) => new(value);

        public override string ToString() => Value;
    }
}
```

#### Password值物件
```csharp
using System.Security.Cryptography;
using System.Text;

namespace UserAuth.Domain.ValueObjects
{
    public record Password
    {
        public string HashedValue { get; init; }
        public string Salt { get; init; }
        public DateTime CreatedAt { get; init; }
        public int Iterations { get; init; }

        private Password(string hashedValue, string salt, DateTime createdAt, int iterations)
        {
            HashedValue = hashedValue ?? throw new ArgumentNullException(nameof(hashedValue));
            Salt = salt ?? throw new ArgumentNullException(nameof(salt));
            CreatedAt = createdAt;
            Iterations = iterations;
        }

        public static Password Create(string plainPassword, int iterations = 10000)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("密碼不能為空或空白", nameof(plainPassword));

            ValidatePasswordStrength(plainPassword);

            var salt = GenerateSalt();
            var hashedValue = HashPassword(plainPassword, salt, iterations);

            return new Password(hashedValue, salt, DateTime.UtcNow, iterations);
        }

        public bool Verify(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                return false;

            var hashedInput = HashPassword(plainPassword, Salt, Iterations);
            return SlowEquals(hashedValue: HashedValue, hashedInput);
        }

        public bool IsExpired(TimeSpan maxAge)
        {
            return DateTime.UtcNow - CreatedAt > maxAge;
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 8)
                throw new ArgumentException("密碼長度至少8個字元");

            if (!password.Any(char.IsUpper))
                throw new ArgumentException("密碼必須包含至少一個大寫字母");

            if (!password.Any(char.IsLower))
                throw new ArgumentException("密碼必須包含至少一個小寫字母");

            if (!password.Any(char.IsDigit))
                throw new ArgumentException("密碼必須包含至少一個數字");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                throw new ArgumentException("密碼必須包含至少一個特殊字元");
        }

        private static string GenerateSalt()
        {
            var saltBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt, int iterations)
        {
            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(
                password: Encoding.UTF8.GetBytes(password),
                salt: Convert.FromBase64String(salt),
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256);

            var hashBytes = rfc2898DeriveBytes.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }

        private static bool SlowEquals(string hashedValue, string hashedInput)
        {
            var a = Convert.FromBase64String(hashedValue);
            var b = Convert.FromBase64String(hashedInput);

            if (a.Length != b.Length)
                return false;

            var result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}
```

#### SessionToken值物件
```csharp
using System.Security.Cryptography;

namespace UserAuth.Domain.ValueObjects
{
    public record SessionToken
    {
        public string Value { get; init; }
        public DateTime CreatedAt { get; init; }

        public SessionToken(string value = null)
        {
            Value = value ?? GenerateSecureToken();
            CreatedAt = DateTime.UtcNow;
        }

        private static string GenerateSecureToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public bool IsExpired(TimeSpan lifetime)
        {
            return DateTime.UtcNow - CreatedAt > lifetime;
        }

        public static implicit operator string(SessionToken token) => token.Value;
        public override string ToString() => Value;
    }
}
```

### 1.3 聚合根實作

#### User聚合根
```csharp
using UserAuth.Domain.Common;
using UserAuth.Domain.ValueObjects;
using UserAuth.Domain.Events;

namespace UserAuth.Domain.Aggregates.UserAggregate
{
    public class User : AggregateRoot<UserId>
    {
        public Username Username { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public UserStatus Status { get; private set; }
        public UserProfile Profile { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockedUntil { get; private set; }

        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        private User() { } // For EF Core

        private User(UserId id, Username username, Email email, Password password, UserProfile profile)
            : base(id)
        {
            Username = username;
            Email = email;
            Password = password;
            Profile = profile;
            Status = UserStatus.PendingVerification();
            CreatedAt = DateTime.UtcNow;
            FailedLoginAttempts = 0;

            AddDomainEvent(new UserRegisteredEvent(Id, Email, Username, CreatedAt));
        }

        public static User Register(Username username, Email email, Password password, UserProfile profile)
        {
            var userId = new UserId(Guid.NewGuid());
            return new User(userId, username, email, password, profile);
        }

        public void ConfirmEmail()
        {
            if (Status.Type == UserStatusType.Active)
                return;

            Status = UserStatus.Active();
            AddDomainEvent(new UserEmailConfirmedEvent(Id, Email, DateTime.UtcNow));
        }

        public void UpdateProfile(string firstName, string lastName, DateTime? dateOfBirth)
        {
            var oldProfile = Profile;
            Profile = new UserProfile(firstName, lastName, dateOfBirth);

            AddDomainEvent(new UserProfileUpdatedEvent(Id, oldProfile, Profile, DateTime.UtcNow));
        }

        public void ChangePassword(Password newPassword, string reason = null)
        {
            if (newPassword == null)
                throw new ArgumentNullException(nameof(newPassword));

            var oldPasswordCreatedAt = Password.CreatedAt;
            Password = newPassword;

            AddDomainEvent(new UserPasswordChangedEvent(Id, oldPasswordCreatedAt, DateTime.UtcNow, reason));
        }

        public bool VerifyPassword(string plainPassword)
        {
            return Password.Verify(plainPassword);
        }

        public void RecordSuccessfulLogin()
        {
            FailedLoginAttempts = 0;
            LastLoginAt = DateTime.UtcNow;
            LockedUntil = null;

            AddDomainEvent(new UserLoggedInEvent(Id, DateTime.UtcNow, LoginMethod.Password));
        }

        public void RecordFailedLogin()
        {
            FailedLoginAttempts++;

            const int maxAttempts = 5;
            if (FailedLoginAttempts >= maxAttempts)
            {
                LockAccount(TimeSpan.FromMinutes(30), "連續登入失敗次數過多");
            }

            AddDomainEvent(new UserLoginFailedEvent(Id, FailedLoginAttempts, DateTime.UtcNow));
        }

        public void LockAccount(TimeSpan duration, string reason)
        {
            LockedUntil = DateTime.UtcNow.Add(duration);
            Status = UserStatus.Suspended(reason);

            AddDomainEvent(new UserAccountLockedEvent(Id, LockedUntil.Value, reason, DateTime.UtcNow));
        }

        public void UnlockAccount()
        {
            LockedUntil = null;
            FailedLoginAttempts = 0;
            Status = UserStatus.Active();

            AddDomainEvent(new UserAccountUnlockedEvent(Id, DateTime.UtcNow));
        }

        public bool IsLocked()
        {
            return LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;
        }

        public bool IsActive()
        {
            return Status.Type == UserStatusType.Active && !IsLocked();
        }

        public void AssignRole(RoleId roleId)
        {
            if (_userRoles.Any(ur => ur.RoleId == roleId))
                return; // 角色已存在

            var userRole = new UserRole(Id, roleId, DateTime.UtcNow);
            _userRoles.Add(userRole);

            AddDomainEvent(new UserRoleAssignedEvent(Id, roleId, DateTime.UtcNow));
        }

        public void RemoveRole(RoleId roleId)
        {
            var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole == null)
                return; // 角色不存在

            _userRoles.Remove(userRole);

            AddDomainEvent(new UserRoleRemovedEvent(Id, roleId, DateTime.UtcNow));
        }

        public bool HasRole(RoleId roleId)
        {
            return _userRoles.Any(ur => ur.RoleId == roleId);
        }

        public string GenerateEmailConfirmationToken()
        {
            // 這裡可以使用更複雜的令牌生成邏輯
            var tokenBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
```

#### LoginSession聚合根
```csharp
using UserAuth.Domain.Common;
using UserAuth.Domain.ValueObjects;
using UserAuth.Domain.Events;

namespace UserAuth.Domain.Aggregates.SessionAggregate
{
    public class LoginSession : AggregateRoot<SessionId>
    {
        public UserId UserId { get; private set; }
        public SessionToken Token { get; private set; }
        public LoginMethod Method { get; private set; }
        public SessionStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }
        public DeviceInfo DeviceInfo { get; private set; }
        public string IpAddress { get; private set; }

        private LoginSession() { } // For EF Core

        private LoginSession(
            SessionId id,
            UserId userId,
            SessionToken token,
            LoginMethod method,
            DeviceInfo deviceInfo,
            DateTime expiresAt,
            string ipAddress)
            : base(id)
        {
            UserId = userId;
            Token = token;
            Method = method;
            DeviceInfo = deviceInfo;
            Status = SessionStatus.Active;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            LastAccessedAt = CreatedAt;
            IpAddress = ipAddress;

            AddDomainEvent(new SessionCreatedEvent(Id, UserId, Method, CreatedAt, ExpiresAt));
        }

        public static LoginSession Create(
            UserId userId,
            LoginMethod method,
            DeviceInfo deviceInfo,
            TimeSpan? duration = null,
            string ipAddress = null)
        {
            var sessionId = new SessionId(Guid.NewGuid());
            var token = new SessionToken();
            var expiresAt = DateTime.UtcNow.Add(duration ?? TimeSpan.FromHours(8));

            return new LoginSession(sessionId, userId, token, method, deviceInfo, expiresAt, ipAddress);
        }

        public void UpdateLastAccessed()
        {
            if (IsExpired())
                throw new InvalidOperationException("無法更新已過期的會話");

            LastAccessedAt = DateTime.UtcNow;
        }

        public void Extend(TimeSpan extension)
        {
            if (IsExpired())
                throw new InvalidOperationException("無法延長已過期的會話");

            ExpiresAt = DateTime.UtcNow.Add(extension);

            AddDomainEvent(new SessionExtendedEvent(Id, ExpiresAt, DateTime.UtcNow));
        }

        public void Invalidate(string reason = null)
        {
            Status = SessionStatus.Invalidated;

            AddDomainEvent(new SessionInvalidatedEvent(Id, UserId, reason, DateTime.UtcNow));
        }

        public void MarkExpired()
        {
            Status = SessionStatus.Expired;

            AddDomainEvent(new SessionExpiredEvent(Id, UserId, DateTime.UtcNow));
        }

        public bool IsValid()
        {
            return Status == SessionStatus.Active && !IsExpired();
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }

        public TimeSpan GetRemainingTime()
        {
            if (IsExpired())
                return TimeSpan.Zero;

            return ExpiresAt - DateTime.UtcNow;
        }
    }

    public enum SessionStatus
    {
        Active,
        Expired,
        Invalidated
    }

    public enum LoginMethod
    {
        Password,
        QRCode,
        TwoFactor,
        SocialLogin
    }
}
```

## 2. Application Layer 實作範例

### 2.1 Use Case實作

#### RegisterUserUseCase
```csharp
using MediatR;
using Microsoft.Extensions.Logging;
using UserAuth.Application.Common.Interfaces;
using UserAuth.Application.Common.Models;

namespace UserAuth.Application.UseCases.Users.Register
{
    public class RegisterUserCommand : IRequest<RegisterUserResult>
    {
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string IpAddress { get; init; }
        public string UserAgent { get; init; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUniquenessService _uniquenessService;
        private readonly IPasswordPolicyService _passwordPolicyService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUserUniquenessService uniquenessService,
            IPasswordPolicyService passwordPolicyService,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _uniquenessService = uniquenessService;
            _passwordPolicyService = passwordPolicyService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RegisterUserResult> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1. 基本驗證
                if (request.Password != request.ConfirmPassword)
                {
                    return RegisterUserResult.Failure("密碼確認不一致");
                }

                // 2. 創建值物件
                var email = new Email(request.Email);
                var username = new Username(request.Username);

                // 3. 唯一性檢查
                var uniquenessResult = await _uniquenessService
                    .ValidateUserUniquenessAsync(email, username, cancellationToken);

                if (!uniquenessResult.IsValid)
                {
                    return RegisterUserResult.Failure(uniquenessResult.Errors.ToArray());
                }

                // 4. 密碼政策驗證
                var passwordResult = await _passwordPolicyService
                    .ValidatePasswordAsync(request.Password, cancellationToken);

                if (!passwordResult.IsValid)
                {
                    return RegisterUserResult.Failure(passwordResult.Errors.ToArray());
                }

                // 5. 創建使用者
                var password = Password.Create(request.Password);
                var profile = new UserProfile(request.FirstName, request.LastName, request.DateOfBirth);
                var user = User.Register(username, email, password, profile);

                // 6. 儲存使用者
                await _userRepository.SaveAsync(user, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                // 7. 發送確認郵件
                var confirmationToken = user.GenerateEmailConfirmationToken();
                await _emailService.SendConfirmationEmailAsync(
                    email.Value,
                    username.Value,
                    confirmationToken);

                _logger.LogInformation("使用者註冊成功: {UserId}, {Email}", user.Id, email.Value);

                return RegisterUserResult.Success(user.Id.Value, confirmationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "使用者註冊失敗: {Email}", request.Email);
                await _unitOfWork.RollbackAsync(cancellationToken);
                return RegisterUserResult.Failure("註冊過程中發生錯誤");
            }
        }
    }

    public class RegisterUserResult
    {
        public bool IsSuccess { get; init; }
        public Guid? UserId { get; init; }
        public string ConfirmationToken { get; init; }
        public List<string> Errors { get; init; } = new();

        public static RegisterUserResult Success(Guid userId, string confirmationToken) =>
            new() { IsSuccess = true, UserId = userId, ConfirmationToken = confirmationToken };

        public static RegisterUserResult Failure(params string[] errors) =>
            new() { IsSuccess = false, Errors = errors.ToList() };
    }
}
```

### 2.2 Application Service實作

#### UserApplicationService
```csharp
using UserAuth.Application.Common.Interfaces;
using UserAuth.Application.DTOs;
using UserAuth.Application.UseCases.Users.Register;
using UserAuth.Application.UseCases.Users.Login;

namespace UserAuth.Application.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserApplicationService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<RegisterUserResponseDto> RegisterAsync(
            RegisterUserRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return _mapper.Map<RegisterUserResponseDto>(result);
        }

        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<LoginCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return _mapper.Map<LoginResponseDto>(result);
        }

        public async Task<UserProfileDto> GetUserProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetUserProfileQuery { UserId = userId };
            var result = await _mediator.Send(query, cancellationToken);

            return _mapper.Map<UserProfileDto>(result);
        }

        public async Task<bool> ChangePasswordAsync(
            Guid userId,
            ChangePasswordRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess;
        }
    }
}
```

## 3. Infrastructure Layer 實作範例

### 3.1 Repository實作

#### UserRepository
```csharp
using Microsoft.EntityFrameworkCore;
using UserAuth.Domain.Aggregates.UserAggregate;
using UserAuth.Infrastructure.Persistence;

namespace UserAuth.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(UserId id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(Email email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email.Value == email.Value);
        }

        public async Task<User> GetByUsernameAsync(Username username)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Username.Value == username.Value);
        }

        public async Task<User> GetByIdentifierAsync(string identifier)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u =>
                    u.Email.Value == identifier ||
                    u.Username.Value == identifier);
        }

        public async Task<bool> IsEmailUniqueAsync(Email email, UserId excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Email.Value == email.Value);

            if (excludeUserId != null)
                query = query.Where(u => u.Id != excludeUserId);

            return !await query.AnyAsync();
        }

        public async Task<bool> IsUsernameUniqueAsync(Username username, UserId excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Username.Value == username.Value);

            if (excludeUserId != null)
                query = query.Where(u => u.Id != excludeUserId);

            return !await query.AnyAsync();
        }

        public async Task SaveAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                _context.Users.Add(user);
            }
            else
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                _context.Entry(existingUser).State = EntityState.Modified;
            }
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> ExistsAsync(UserId id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<PagedResult<User>> GetUsersPagedAsync(
            int page,
            int pageSize,
            UserSearchCriteria criteria = null)
        {
            var query = _context.Users.AsQueryable();

            // 應用搜尋條件
            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
                {
                    query = query.Where(u =>
                        u.Email.Value.Contains(criteria.SearchTerm) ||
                        u.Username.Value.Contains(criteria.SearchTerm) ||
                        u.Profile.FirstName.Contains(criteria.SearchTerm) ||
                        u.Profile.LastName.Contains(criteria.SearchTerm));
                }

                if (criteria.Status.HasValue)
                {
                    query = query.Where(u => u.Status.Type == criteria.Status.Value);
                }

                if (criteria.CreatedAfter.HasValue)
                {
                    query = query.Where(u => u.CreatedAt >= criteria.CreatedAfter.Value);
                }

                if (criteria.CreatedBefore.HasValue)
                {
                    query = query.Where(u => u.CreatedAt <= criteria.CreatedBefore.Value);
                }
            }

            // 計算總數
            var totalCount = await query.CountAsync();

            // 應用分頁和排序
            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
```

### 3.2 Domain Service實作

#### AuthenticationService
```csharp
using UserAuth.Domain.Services;
using UserAuth.Domain.Aggregates.UserAggregate;
using UserAuth.Domain.Aggregates.SessionAggregate;

namespace UserAuth.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPasswordPolicyService _passwordPolicyService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserRepository userRepository,
            ILoginSessionRepository sessionRepository,
            IAuditLogRepository auditLogRepository,
            IPasswordPolicyService passwordPolicyService,
            ILogger<AuthenticationService> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _auditLogRepository = auditLogRepository;
            _passwordPolicyService = passwordPolicyService;
            _logger = logger;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(
            string identifier,
            string password,
            AuthenticationContext context)
        {
            try
            {
                // 1. 查找使用者
                var user = await _userRepository.GetByIdentifierAsync(identifier);
                if (user == null)
                {
                    await RecordFailedAttempt(identifier, "使用者不存在", context);
                    return AuthenticationResult.Failure("無效的憑證");
                }

                // 2. 檢查帳戶狀態
                if (!user.IsActive())
                {
                    var reason = user.IsLocked() ? "帳戶已鎖定" : "帳戶未啟用";
                    await RecordFailedAttempt(user.Id.ToString(), reason, context);
                    return AuthenticationResult.Failure(reason);
                }

                // 3. 驗證密碼
                if (!user.VerifyPassword(password))
                {
                    user.RecordFailedLogin();
                    await _userRepository.SaveAsync(user);
                    await RecordFailedAttempt(user.Id.ToString(), "密碼錯誤", context);
                    return AuthenticationResult.Failure("無效的憑證");
                }

                // 4. 成功認證
                user.RecordSuccessfulLogin();
                await _userRepository.SaveAsync(user);

                // 5. 創建會話
                var session = LoginSession.Create(
                    user.Id,
                    LoginMethod.Password,
                    context.DeviceInfo,
                    TimeSpan.FromHours(8),
                    context.IpAddress);

                await _sessionRepository.SaveAsync(session);

                // 6. 記錄成功認證
                await RecordSuccessfulAttempt(user.Id.ToString(), context);

                _logger.LogInformation("使用者認證成功: {UserId}", user.Id);

                return AuthenticationResult.Success(user.Id, session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "認證過程發生錯誤: {Identifier}", identifier);
                return AuthenticationResult.Failure("認證過程發生錯誤");
            }
        }

        public async Task<bool> IsAccountLockedAsync(UserId userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.IsLocked() ?? false;
        }

        public async Task<PasswordValidationResult> ValidatePasswordStrengthAsync(
            string password,
            UserId userId = null)
        {
            return await _passwordPolicyService.ValidatePasswordAsync(password, userId);
        }

        private async Task RecordFailedAttempt(
            string identifier,
            string reason,
            AuthenticationContext context)
        {
            var auditLog = AuditLog.CreateAuthenticationEvent(
                identifier,
                AuthenticationEventType.Failed,
                reason,
                context.IpAddress,
                context.UserAgent);

            await _auditLogRepository.SaveAsync(auditLog);
        }

        private async Task RecordSuccessfulAttempt(
            string identifier,
            AuthenticationContext context)
        {
            var auditLog = AuditLog.CreateAuthenticationEvent(
                identifier,
                AuthenticationEventType.Success,
                "認證成功",
                context.IpAddress,
                context.UserAgent);

            await _auditLogRepository.SaveAsync(auditLog);
        }
    }
}
```

## 4. Presentation Layer 實作範例

### 4.1 Controller實作

#### AuthController
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Application.Common.Interfaces;
using UserAuth.Application.DTOs;

namespace UserAuth.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserApplicationService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserApplicationService userService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterUserResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterUserResponseDto>> Register(
            [FromBody] RegisterUserRequestDto request)
        {
            try
            {
                // 添加IP地址和User Agent
                request.IpAddress = GetClientIpAddress();
                request.UserAgent = Request.Headers["User-Agent"].ToString();

                var result = await _userService.RegisterAsync(request);

                if (!result.IsSuccess)
                {
                    return BadRequest(new ValidationProblemDetails
                    {
                        Title = "註冊失敗",
                        Detail = string.Join(", ", result.Errors),
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("使用者註冊成功: {Email}", request.Email);

                return CreatedAtAction(
                    nameof(GetProfile),
                    new { userId = result.UserId },
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊請求處理失敗: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "註冊過程中發生錯誤");
            }
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto request)
        {
            try
            {
                // 添加上下文資訊
                request.IpAddress = GetClientIpAddress();
                request.UserAgent = Request.Headers["User-Agent"].ToString();

                var result = await _userService.LoginAsync(request);

                if (!result.IsSuccess)
                {
                    if (result.RequiresTwoFactorAuth)
                    {
                        return Ok(new { RequiresTwoFactorAuth = true, UserId = result.UserId });
                    }

                    return Unauthorized(new ProblemDetails
                    {
                        Title = "登入失敗",
                        Detail = string.Join(", ", result.Errors),
                        Status = StatusCodes.Status401Unauthorized
                    });
                }

                // 設置Cookie (可選)
                if (request.RememberMe)
                {
                    SetAuthenticationCookie(result.RefreshToken);
                }

                _logger.LogInformation("使用者登入成功: {UserId}", result.UserId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登入請求處理失敗: {Identifier}", request.Identifier);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "登入過程中發生錯誤");
            }
        }

        /// <summary>
        /// 使用者登出
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _userService.LogoutAsync(userId);

                // 清除Cookie
                ClearAuthenticationCookie();

                _logger.LogInformation("使用者登出成功: {UserId}", userId);

                return Ok(new { Message = "登出成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登出處理失敗");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "登出過程中發生錯誤");
            }
        }

        /// <summary>
        /// 獲取使用者個人資料
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var profile = await _userService.GetUserProfileAsync(userId);

                if (profile == null)
                {
                    return NotFound("使用者不存在");
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取個人資料失敗");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "獲取個人資料過程中發生錯誤");
            }
        }

        private string GetClientIpAddress()
        {
            var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var realIp = Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            return Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
            return Guid.Parse(userIdClaim);
        }

        private void SetAuthenticationCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private void ClearAuthenticationCookie()
        {
            Response.Cookies.Delete("refreshToken");
        }
    }
}
```

## 5. 配置和依賴注入

### 5.1 Program.cs配置
```csharp
using Microsoft.EntityFrameworkCore;
using UserAuth.Infrastructure.Persistence;
using UserAuth.Application;
using UserAuth.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 添加資料庫上下文
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 添加應用層服務
builder.Services.AddApplication();

// 添加基礎設施層服務
builder.Services.AddInfrastructure(builder.Configuration);

// 添加JWT認證
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

這個完整的C#程式碼範例展示了DDD架構在實際專案中的實作方式，包含了領域模型、應用服務、基礎設施層和展示層的具體實作，遵循了DDD的核心原則和最佳實踐。