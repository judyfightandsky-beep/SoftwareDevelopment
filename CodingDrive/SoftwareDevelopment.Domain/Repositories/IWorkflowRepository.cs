using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Workflow;

namespace SoftwareDevelopment.Domain.Repositories
{
    public interface IWorkflowRepository
    {
        Task<Workflow> GetByIdAsync(Guid id);
        Task<IEnumerable<Workflow>> GetAllAsync();
        Task<IEnumerable<Workflow>> GetByStatusAsync(Workflow.ValueObjects.WorkflowStatus status);
        Task AddAsync(Workflow workflow);
        Task UpdateAsync(Workflow workflow);
        Task DeleteAsync(Guid id);
    }
}