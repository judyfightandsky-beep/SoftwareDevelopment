using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareDevelopment.Domain.Tasks.Entities;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Shared.ValueObjects;
using TaskEntity = SoftwareDevelopment.Domain.Tasks.Entities.Task;

namespace SoftwareDevelopment.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TaskId.From(value))
            .ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Priority)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.From(value))
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.Property(t => t.AssignedTo)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? UserId.From(value.Value) : null);

        builder.Property(t => t.EstimatedHours)
            .HasConversion(
                hours => hours.Value,
                value => EstimatedHours.Create(value))
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(t => t.ActualHours)
            .HasConversion(
                hours => hours.Value,
                value => ActualHours.Create(value))
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(t => t.DueDate);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.LastModifiedAt)
            .IsRequired();

        builder.Property(t => t.StartedAt);

        builder.Property(t => t.CompletedAt);

        builder.HasIndex(t => t.Title);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Type);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.AssignedTo);
        builder.HasIndex(t => t.DueDate);
        builder.HasIndex(t => t.CreatedAt);
    }
}