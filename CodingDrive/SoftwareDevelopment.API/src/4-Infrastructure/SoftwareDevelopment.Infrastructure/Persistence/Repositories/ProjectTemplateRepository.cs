using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Domain.Templates.Entities;
using SoftwareDevelopment.Domain.Templates.Repositories;
using SoftwareDevelopment.Domain.Templates.ValueObjects;

namespace SoftwareDevelopment.Infrastructure.Persistence.Repositories;

public sealed class ProjectTemplateRepository : IProjectTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectTemplateRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ProjectTemplate?> GetByIdAsync(TemplateId id, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectTemplates
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ProjectTemplate> Templates, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        TemplateType? type = null,
        TemplateStatus? status = null,
        IEnumerable<string>? tags = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ProjectTemplates.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm));
        }

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var templates = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (templates.AsReadOnly(), totalCount);
    }

    public async Task<IReadOnlyList<ProjectTemplate>> GetPopularTemplatesAsync(
        int count = 10,
        CancellationToken cancellationToken = default)
    {
        return await _context.ProjectTemplates
            .Where(t => t.Status == TemplateStatus.Published)
            .OrderByDescending(t => t.DownloadCount)
            .ThenByDescending(t => t.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProjectTemplate template, CancellationToken cancellationToken = default)
    {
        await _context.ProjectTemplates.AddAsync(template, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProjectTemplate template, CancellationToken cancellationToken = default)
    {
        _context.ProjectTemplates.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProjectTemplate template, CancellationToken cancellationToken = default)
    {
        _context.ProjectTemplates.Remove(template);
        await _context.SaveChangesAsync(cancellationToken);
    }
}