using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Domain.Tasks.Entities;
using SoftwareDevelopment.Domain.Tasks.Repositories;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Shared.ValueObjects;
using TaskStatus = SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus;

namespace SoftwareDevelopment.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async System.Threading.Tasks.Task<SoftwareDevelopment.Domain.Tasks.Entities.Task?> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async System.Threading.Tasks.Task<(IReadOnlyList<SoftwareDevelopment.Domain.Tasks.Entities.Task> Tasks, int TotalCount)> SearchAsync(
        ProjectId? projectId = null,
        UserId? assignedTo = null,
        TaskStatus? status = null,
        TaskType? type = null,
        Priority? priority = null,
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tasks.AsQueryable();

        if (projectId != null)
        {
            query = query.Where(t => t.ProjectId == projectId);
        }

        if (assignedTo != null)
        {
            query = query.Where(t => t.AssignedTo == assignedTo);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (tasks.AsReadOnly(), totalCount);
    }

    public async System.Threading.Tasks.Task<(IReadOnlyList<SoftwareDevelopment.Domain.Tasks.Entities.Task> Tasks, int TotalCount)> GetUserTasksAsync(
        UserId userId,
        TaskStatus? status = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tasks
            .Where(t => t.AssignedTo == userId);

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (tasks.AsReadOnly(), totalCount);
    }

    public async System.Threading.Tasks.Task<IReadOnlyList<SoftwareDevelopment.Domain.Tasks.Entities.Task>> GetOverdueTasksAsync(
        UserId? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tasks
            .Where(t => t.DueDate.HasValue &&
                       t.DueDate.Value.Date < DateTime.UtcNow.Date &&
                       t.Status != TaskStatus.Done &&
                       t.Status != TaskStatus.Cancelled &&
                       t.Status != TaskStatus.Archived);

        if (userId != null)
        {
            query = query.Where(t => t.AssignedTo == userId);
        }

        return await query
            .OrderBy(t => t.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task AddAsync(SoftwareDevelopment.Domain.Tasks.Entities.Task task, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task UpdateAsync(SoftwareDevelopment.Domain.Tasks.Entities.Task task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task DeleteAsync(SoftwareDevelopment.Domain.Tasks.Entities.Task task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}