import { BaseEntity, User } from './common'

export enum TaskType {
  Feature = 'Feature',
  Bug = 'Bug',
  Refactor = 'Refactor',
  Documentation = 'Documentation',
  Testing = 'Testing'
}

export enum TaskStatus {
  Todo = 'Todo',
  InProgress = 'InProgress',
  Review = 'Review',
  Testing = 'Testing',
  Done = 'Done',
  Closed = 'Closed'
}

export enum TaskPriority {
  Critical = 'Critical',
  High = 'High',
  Medium = 'Medium',
  Low = 'Low'
}

export interface TaskProgress {
  percentage: number
  notes: string
  updatedAt: Date
}

export interface TaskComment {
  id: string
  content: string
  authorId: string
  createdAt: Date
  updatedAt?: Date
}

export interface TaskAttachment {
  id: string
  fileName: string
  fileType: string
  fileSize: number
  storagePath: string
  uploadedBy: string
  uploadedAt: Date
}

export interface Task extends BaseEntity {
  projectId: string
  title: string
  description: string
  type: TaskType
  status: TaskStatus
  priority: TaskPriority
  assignedTo?: string
  estimatedHours?: number
  actualHours?: number
  dueDate?: Date
  progress: TaskProgress
  comments: TaskComment[]
  attachments: TaskAttachment[]
  gitLabInfo?: {
    issueId?: number
    issueUrl?: string
    mergeRequestId?: number
    branchName?: string
  }
}