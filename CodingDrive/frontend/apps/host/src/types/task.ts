// 任務管理相關型別定義
export interface TaskType {
  id: string;
  taskNumber: string;
  title: string;
  description: string;
  type: 'Feature' | 'Bug' | 'Refactor' | 'Documentation' | 'Other';
  priority: 'Critical' | 'High' | 'Medium' | 'Low';
  status: 'Todo' | 'InProgress' | 'Review' | 'Testing' | 'Done' | 'Closed';
  projectId: string;
  assignedTo?: string;
  createdBy: string;
  createdAt: Date;
  dueDate?: Date;
  estimatedHours?: number;
  actualHours?: number;
  progress?: number;
  gitLabInfo?: {
    issueId?: number;
    issueUrl?: string;
    mergeRequestId?: number;
    branchName?: string;
  };
}

export interface TaskCreateParams {
  title: string;
  description: string;
  type: TaskType['type'];
  priority: TaskType['priority'];
  projectId: string;
  assignedTo?: string;
  dueDate?: Date;
  estimatedHours?: number;
}

export interface TaskUpdateParams {
  id: string;
  title?: string;
  description?: string;
  status?: TaskType['status'];
  priority?: TaskType['priority'];
  assignedTo?: string;
  dueDate?: Date;
  estimatedHours?: number;
  actualHours?: number;
  progress?: number;
}