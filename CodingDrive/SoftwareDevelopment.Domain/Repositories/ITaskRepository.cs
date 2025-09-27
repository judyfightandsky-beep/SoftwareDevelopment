using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Tasks;

namespace SoftwareDevelopment.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<Task> GetByIdAsync(Guid id);
        Task<IEnumerable<Task>> GetAllAsync();
        Task<IEnumerable<Task>> GetByStatusAsync(Tasks.ValueObjects.TaskStatus status);
        Task AddAsync(Task task);
        Task UpdateAsync(Task task);
        Task DeleteAsync(Guid id);
    }
}