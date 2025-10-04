using System;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Repositories;
using SoftwareDevelopment.Domain.Tasks;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;

namespace SoftwareDevelopment.Domain.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<Task> CreateTaskAsync(string title, string description, TaskPriority priority, DateTime? dueDate = null)
        {
            var task = new Task(title, description, priority, dueDate);
            await _taskRepository.AddAsync(task);
            return task;
        }

        public async Task UpdateTaskStatusAsync(Guid taskId, TaskStatus newStatus)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task not found", nameof(taskId));

            task.UpdateStatus(newStatus);
            await _taskRepository.UpdateAsync(task);
        }
    }
}