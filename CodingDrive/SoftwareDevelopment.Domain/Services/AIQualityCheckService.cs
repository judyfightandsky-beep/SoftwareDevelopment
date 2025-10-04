using System;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Repositories;
using SoftwareDevelopment.Domain.AIQualityCheck;
using SoftwareDevelopment.Domain.AIQualityCheck.ValueObjects;

namespace SoftwareDevelopment.Domain.Services
{
    public class AIQualityCheckService
    {
        private readonly IAIQualityCheckRepository _aiQualityCheckRepository;

        public AIQualityCheckService(IAIQualityCheckRepository aiQualityCheckRepository)
        {
            _aiQualityCheckRepository = aiQualityCheckRepository
                ?? throw new ArgumentNullException(nameof(aiQualityCheckRepository));
        }

        public async Task<AIQualityCheck> CreateAIQualityCheckAsync(Guid taskId)
        {
            var aiQualityCheck = new AIQualityCheck(taskId);
            await _aiQualityCheckRepository.AddAsync(aiQualityCheck);
            return aiQualityCheck;
        }

        public async Task UpdateAIQualityCheckScoreAsync(Guid aiQualityCheckId, double score, string feedback)
        {
            var aiQualityCheck = await _aiQualityCheckRepository.GetByIdAsync(aiQualityCheckId);
            if (aiQualityCheck == null)
                throw new ArgumentException("AI Quality Check not found", nameof(aiQualityCheckId));

            aiQualityCheck.UpdateScore(score, feedback);
            await _aiQualityCheckRepository.UpdateAsync(aiQualityCheck);
        }
    }
}