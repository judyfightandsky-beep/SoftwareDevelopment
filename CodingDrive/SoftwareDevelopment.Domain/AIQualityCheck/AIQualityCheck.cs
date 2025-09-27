using System;
using SoftwareDevelopment.Domain.AIQualityCheck.ValueObjects;

namespace SoftwareDevelopment.Domain.AIQualityCheck
{
    public class AIQualityCheck
    {
        public Guid Id { get; private set; }
        public Guid TaskId { get; private set; }
        public AIQualityCheckStatus Status { get; private set; }
        public double Score { get; private set; }
        public string Feedback { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private AIQualityCheck() { } // For ORM

        public AIQualityCheck(
            Guid taskId,
            double initialScore = 0)
        {
            Id = Guid.NewGuid();
            TaskId = taskId;
            Status = AIQualityCheckStatus.Pending;
            Score = initialScore;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateScore(double score, string feedback)
        {
            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100.");

            Score = score;
            Feedback = feedback;
            Status = score >= 80 ? AIQualityCheckStatus.Passed : AIQualityCheckStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }

        public void MarkAsInProgress()
        {
            Status = AIQualityCheckStatus.InProgress;
        }
    }
}