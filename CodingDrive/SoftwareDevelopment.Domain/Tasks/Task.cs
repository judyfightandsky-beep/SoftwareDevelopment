using System;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DueDate { get; private set; }

        private Task() { } // For ORM

        public Task(
            string title,
            string description,
            TaskPriority priority,
            DateTime? dueDate = null)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Status = TaskStatus.Pending;
            Priority = priority;
            CreatedAt = DateTime.UtcNow;
            DueDate = dueDate;
        }

        public void UpdateStatus(TaskStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(TaskPriority newPriority)
        {
            Priority = newPriority;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDetails(string title, string description, DateTime? dueDate)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            DueDate = dueDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}