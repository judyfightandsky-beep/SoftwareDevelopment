// 工作流程引擎相關型別定義
export interface WorkflowNodeType {
  id: string;
  name: string;
  type: 'Start' | 'End' | 'Task' | 'Decision' | 'Parallel' | 'Merge' | 'Timer' | 'Service' | 'Human' | 'Approval';
  configuration: Record<string, any>;
  inputs: string[];
  outputs: string[];
  isRequired: boolean;
  timeLimit?: number;
  assignedRoles?: string[];
}

export interface WorkflowTransitionType {
  id: string;
  fromNodeId: string;
  toNodeId: string;
  condition?: string;
  name: string;
  type: 'Standard' | 'Conditional';
}

export interface WorkflowDefinitionType {
  id: string;
  name: string;
  description: string;
  version: string;
  status: 'Draft' | 'Active' | 'Deprecated' | 'Archived';
  nodes: WorkflowNodeType[];
  transitions: WorkflowTransitionType[];
  trigger: 'Manual' | 'Automatic' | 'Event' | 'Scheduled';
  createdAt: Date;
  createdBy: string;
}

export interface WorkflowInstanceType {
  id: string;
  workflowDefinitionId: string;
  title: string;
  status: 'Created' | 'Running' | 'Suspended' | 'Completed' | 'Cancelled' | 'Failed';
  variables: Record<string, any>;
  nodeExecutions: {
    id: string;
    nodeId: string;
    status: 'Pending' | 'Running' | 'Completed' | 'Failed';
    startedAt: Date;
    completedAt?: Date;
    assignedTo?: string;
    executedBy?: string;
    inputData?: Record<string, any>;
    outputData?: Record<string, any>;
    comments?: string;
  }[];
  startedAt: Date;
  completedAt?: Date;
  startedBy: string;
  projectId?: string;
  taskId?: string;
}

export interface WorkflowCreateParams {
  name: string;
  description: string;
  nodes: Omit<WorkflowNodeType, 'id'>[];
  transitions: Omit<WorkflowTransitionType, 'id'>[];
  trigger: WorkflowDefinitionType['trigger'];
  configuration?: Record<string, any>;
}

export interface WorkflowStartParams {
  workflowDefinitionId: string;
  title: string;
  variables?: Record<string, any>;
  projectId?: string;
  taskId?: string;
}
