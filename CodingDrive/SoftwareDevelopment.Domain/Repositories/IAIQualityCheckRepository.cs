using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.AIQualityCheck;

namespace SoftwareDevelopment.Domain.Repositories
{
    public interface IAIQualityCheckRepository
    {
        Task<AIQualityCheck> GetByIdAsync(Guid id);
        Task<IEnumerable<AIQualityCheck>> GetByTaskIdAsync(Guid taskId);
        Task<IEnumerable<AIQualityCheck>> GetByStatusAsync(AIQualityCheck.ValueObjects.AIQualityCheckStatus status);
        Task AddAsync(AIQualityCheck aiQualityCheck);
        Task UpdateAsync(AIQualityCheck aiQualityCheck);
        Task DeleteAsync(Guid id);
    }
}