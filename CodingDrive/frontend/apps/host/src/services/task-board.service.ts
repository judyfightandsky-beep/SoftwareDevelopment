import { apiClient } from './api-client';
import { Task, TaskStatus, TaskColumn } from '../types/task';
import { useGlobalState } from '../contexts/GlobalStateContext';

export class TaskBoardService {
  // Fetch tasks for a specific project (後端尚未實作此端點)
  async fetchTasks(projectId: string): Promise<TaskColumn[]> {
    // TODO: 等待後端實作 GET /api/Projects/{projectId}/tasks 端點
    throw new Error('Fetch tasks by project functionality not yet implemented in backend')
    // try {
    //   const globalState = useGlobalState();
    //   globalState.setLoading(true);
    //   const response = await apiClient.get<TaskColumn[]>(`/api/Projects/${projectId}/tasks`);
    //   globalState.setLoading(false);
    //   return response.data;
    // } catch (error) {
    //   console.error('Failed to fetch tasks', error);
    //   throw error;
    // }
  }

  // Create a new task
  async createTask(projectId: string, task: Omit<Task, 'id'>): Promise<Task> {
    try {
      const globalState = useGlobalState();
      globalState.setLoading(true);
      // 將projectId加入task對象中發送到後端
      const taskWithProject = { ...task, projectId };
      const response = await apiClient.post<Task>('/api/Tasks', taskWithProject);
      globalState.setLoading(false);
      return response.data;
    } catch (error) {
      console.error('Failed to create task', error);
      throw error;
    }
  }

  // Update task status (使用後端的PUT端點)
  async updateTaskStatus(taskId: string, newStatus: TaskStatus): Promise<Task> {
    try {
      const globalState = useGlobalState();
      globalState.setLoading(true);
      // 使用PUT /api/Tasks/{id}端點，只更新status字段
      const response = await apiClient.put<Task>(`/api/Tasks/${taskId}`, { status: newStatus });
      globalState.setLoading(false);
      return response.data;
    } catch (error) {
      console.error('Failed to update task status', error);
      throw error;
    }
  }

  // Delete a task (後端尚未實作此端點)
  async deleteTask(taskId: string): Promise<void> {
    // TODO: 等待後端實作 DELETE /api/Tasks/{id} 端點
    throw new Error('Delete task functionality not yet implemented in backend')
    // try {
    //   const globalState = useGlobalState();
    //   globalState.setLoading(true);
    //   await apiClient.delete(`/api/Tasks/${taskId}`);
    //   globalState.setLoading(false);
    // } catch (error) {
    //   console.error('Failed to delete task', error);
    //   throw error;
    // }
  }

  // Assign task to a user (使用後端的PUT端點)
  async assignTask(taskId: string, userId: string): Promise<Task> {
    try {
      const globalState = useGlobalState();
      globalState.setLoading(true);
      // 使用PUT /api/Tasks/{id}端點，只更新assignedTo字段
      const response = await apiClient.put<Task>(`/api/Tasks/${taskId}`, { assignedTo: userId });
      globalState.setLoading(false);
      return response.data;
    } catch (error) {
      console.error('Failed to assign task', error);
      throw error;
    }
  }
}

export const taskBoardService = new TaskBoardService();