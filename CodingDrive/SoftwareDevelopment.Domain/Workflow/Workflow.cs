using System;
using System.Collections.Generic;
using SoftwareDevelopment.Domain.Workflow.ValueObjects;

namespace SoftwareDevelopment.Domain.Workflow
{
    public class Workflow
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public WorkflowStatus Status { get; private set; }
        public List<WorkflowStep> Steps { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private Workflow()
        {
            Steps = new List<WorkflowStep>();
        } // For ORM

        public Workflow(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Status = WorkflowStatus.Created;
            Steps = new List<WorkflowStep>();
            CreatedAt = DateTime.UtcNow;
        }

        public void AddStep(string stepName, WorkflowStepType stepType)
        {
            var step = new WorkflowStep(stepName, stepType, Steps.Count + 1);
            Steps.Add(step);
        }

        public void Start()
        {
            if (Status != WorkflowStatus.Created)
                throw new InvalidOperationException("Workflow can only be started from Created state.");

            Status = WorkflowStatus.InProgress;
        }

        public void CompleteWorkflow()
        {
            if (Status != WorkflowStatus.InProgress)
                throw new InvalidOperationException("Workflow can only be completed from InProgress state.");

            Status = WorkflowStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string reason)
        {
            Status = WorkflowStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public class WorkflowStep
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public WorkflowStepType StepType { get; private set; }
        public int Order { get; private set; }
        public WorkflowStepStatus Status { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        internal WorkflowStep(string name, WorkflowStepType stepType, int order)
        {
            Id = Guid.NewGuid();
            Name = name;
            StepType = stepType;
            Order = order;
            Status = WorkflowStepStatus.Pending;
        }

        public void Start()
        {
            Status = WorkflowStepStatus.InProgress;
            StartedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            Status = WorkflowStepStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail(string reason)
        {
            Status = WorkflowStepStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }
    }
}