using System;
using SoftwareDevelopment.Domain.Shared.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.ValueObjects
{
    /// <summary>
    /// 任務進度值物件
    /// </summary>
    public class Progress
    {
        public int Percentage { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public UserId UpdatedBy { get; private set; }
        public string Notes { get; private set; }

        private Progress(int percentage, UserId updatedBy, string notes = null)
        {
            Update(percentage, updatedBy, notes);
        }

        /// <summary>
        /// 建立進度
        /// </summary>
        public static Progress Create(int percentage, UserId updatedBy = null, string notes = null)
        {
            return new Progress(percentage, updatedBy ?? UserId.System, notes);
        }

        /// <summary>
        /// 更新進度
        /// </summary>
        public void Update(int percentage, UserId updatedBy, string notes = null)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "進度必須介於 0 到 100 之間");

            Percentage = percentage;
            LastUpdated = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            Notes = notes;
        }

        /// <summary>
        /// 將進度設為完成
        /// </summary>
        public void UpdateToComplete()
        {
            Update(100, UserId.System, "任務已完成");
        }

        /// <summary>
        /// 檢查是否已完成
        /// </summary>
        public bool IsComplete() => Percentage == 100;

        /// <summary>
        /// 驗證進度
        /// </summary>
        public bool Validate() => Percentage >= 0 && Percentage <= 100;
    }
}