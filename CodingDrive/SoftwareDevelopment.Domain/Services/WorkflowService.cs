using System;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Repositories;
using SoftwareDevelopment.Domain.Workflow;
using SoftwareDevelopment.Domain.Workflow.ValueObjects;

namespace SoftwareDevelopment.Domain.Services
{
    public class WorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository ?? throw new ArgumentNullException(nameof(workflowRepository));
        }

        public async Task<Workflow> CreateWorkflowAsync(string name)
        {
            var workflow = new Workflow(name);
            await _workflowRepository.AddAsync(workflow);
            return workflow;
        }

        public async Task AddWorkflowStepAsync(Guid workflowId, string stepName, WorkflowStepType stepType)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId);
            if (workflow == null)
                throw new ArgumentException("Workflow not found", nameof(workflowId));

            workflow.AddStep(stepName, stepType);
            await _workflowRepository.UpdateAsync(workflow);
        }

        public async Task StartWorkflowAsync(Guid workflowId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId);
            if (workflow == null)
                throw new ArgumentException("Workflow not found", nameof(workflowId));

            workflow.Start();
            await _workflowRepository.UpdateAsync(workflow);
        }

        public async Task CompleteWorkflowAsync(Guid workflowId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId);
            if (workflow == null)
                throw new ArgumentException("Workflow not found", nameof(workflowId));

            workflow.CompleteWorkflow();
            await _workflowRepository.UpdateAsync(workflow);
        }
    }
}