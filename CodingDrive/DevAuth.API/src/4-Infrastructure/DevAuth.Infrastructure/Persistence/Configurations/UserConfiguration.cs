using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevAuth.Domain.Users;
using DevAuth.Domain.Shared;

namespace DevAuth.Infrastructure.Persistence.Configurations;

/// <summary>
/// 使用者實體配置
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// 配置使用者實體
    /// </summary>
    /// <param name="builder">實體類型建構器</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureValueObjects(builder);
        ConfigureCollections(builder);
    }

    private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        // 使用者識別碼配置
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .ValueGeneratedNever();

        // 基本屬性配置
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(500);

        builder.Property(u => u.LastLoginAt);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastModifiedAt)
            .IsRequired();
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<User> builder)
    {
        // 使用者名稱配置
        builder.Property(u => u.Username)
            .HasConversion(
                username => username.Value,
                value => Username.Create(value))
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(u => u.Username)
            .IsUnique();

        // 電子信箱配置
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        // 使用者角色配置
        builder.OwnsOne(u => u.Role, roleBuilder =>
        {
            roleBuilder.Property(r => r.Type)
                .HasConversion<string>()
                .HasColumnName("RoleType")
                .IsRequired();

            roleBuilder.Property(r => r.AssignedAt)
                .HasColumnName("RoleAssignedAt")
                .IsRequired();

            roleBuilder.Property(r => r.AssignedBy)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? UserId.From(value.Value) : null)
                .HasColumnName("RoleAssignedBy");
        });

        // 使用者狀態配置
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();
    }

    private static void ConfigureCollections(EntityTypeBuilder<User> builder)
    {
        // 團隊成員資格配置
        builder.OwnsMany(u => u.TeamMemberships, teamBuilder =>
        {
            teamBuilder.ToTable("UserTeamMemberships");

            teamBuilder.WithOwner().HasForeignKey("UserId");

            teamBuilder.Property<int>("Id")
                .ValueGeneratedOnAdd();

            teamBuilder.HasKey("Id");

            teamBuilder.Property(tm => tm.TeamId)
                .IsRequired();

            teamBuilder.Property(tm => tm.Role)
                .HasConversion<string>()
                .IsRequired();

            teamBuilder.Property(tm => tm.JoinedAt)
                .IsRequired();

            teamBuilder.HasIndex("UserId", nameof(TeamMembership.TeamId))
                .IsUnique();
        });

        // 專案成員資格配置
        builder.OwnsMany(u => u.ProjectMemberships, projectBuilder =>
        {
            projectBuilder.ToTable("UserProjectMemberships");

            projectBuilder.WithOwner().HasForeignKey("UserId");

            projectBuilder.Property<int>("Id")
                .ValueGeneratedOnAdd();

            projectBuilder.HasKey("Id");

            projectBuilder.Property(pm => pm.ProjectId)
                .IsRequired();

            projectBuilder.Property(pm => pm.Role)
                .HasConversion<string>()
                .IsRequired();

            projectBuilder.Property(pm => pm.JoinedAt)
                .IsRequired();

            projectBuilder.Property(pm => pm.AllocationPercentage)
                .HasPrecision(5, 2)
                .IsRequired();

            projectBuilder.HasIndex("UserId", nameof(ProjectMembership.ProjectId))
                .IsUnique();
        });

        // 忽略領域事件（運行時使用）
        builder.Ignore(u => u.DomainEvents);
    }
}