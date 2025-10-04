using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareDevelopment.Domain.Templates.Entities;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Infrastructure.Persistence.Configurations;

public class ProjectTemplateConfiguration : IEntityTypeConfiguration<ProjectTemplate>
{
    public void Configure(EntityTypeBuilder<ProjectTemplate> builder)
    {
        builder.ToTable("ProjectTemplates");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TemplateId.From(value))
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.Property(t => t.Configuration)
            .HasConversion(
                config => config.ConfigurationJson,
                json => TemplateConfiguration.Create(json))
            .HasColumnType("text")
            .IsRequired();

        builder.OwnsOne(t => t.Metadata, metadata =>
        {
            metadata.Property(m => m.Version)
                .HasMaxLength(20)
                .IsRequired();

            metadata.Property(m => m.Tags)
                .HasConversion(
                    tags => string.Join(",", tags),
                    str => str.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().AsReadOnly())
                .HasMaxLength(500);
        });

        builder.Property(t => t.DownloadCount)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.LastModifiedAt)
            .IsRequired();

        builder.HasIndex(t => t.Name);
        builder.HasIndex(t => t.Type);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.CreatedAt);
    }
}