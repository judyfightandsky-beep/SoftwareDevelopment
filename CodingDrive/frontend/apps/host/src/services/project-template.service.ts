import { ProjectTemplate, ProjectTemplateGenerationConfig } from '../types/project-template'
import { User } from '../types/common'
import { apiClient } from './api-client'

export class ProjectTemplateService {
  // 獲取所有可用的專案模板
  async getTemplates(): Promise<ProjectTemplate[]> {
    try {
      const response = await apiClient.get<ProjectTemplate[]>('/api/Templates')
      return response.data
    } catch (error) {
      console.error('Failed to fetch templates', error)
      throw error
    }
  }

  // 根據ID取得特定模板
  async getTemplateById(id: string): Promise<ProjectTemplate> {
    try {
      const response = await apiClient.get<ProjectTemplate>(`/api/Templates/${id}`)
      return response.data
    } catch (error) {
      console.error(`Failed to fetch template ${id}`, error)
      throw error
    }
  }

  // 建立新的專案模板
  async createTemplate(template: ProjectTemplate): Promise<ProjectTemplate> {
    try {
      const response = await apiClient.post<ProjectTemplate>('/api/Templates', template)
      return response.data
    } catch (error) {
      console.error('Failed to create template', error)
      throw error
    }
  }

  // 更新專案模板 (後端尚未實作此端點)
  async updateTemplate(template: ProjectTemplate): Promise<ProjectTemplate> {
    // TODO: 等待後端實作 PUT /api/Templates/{id} 端點
    throw new Error('Update template functionality not yet implemented in backend')
    // try {
    //   const response = await apiClient.put<ProjectTemplate>(`/api/Templates/${template.id}`, template)
    //   return response.data
    // } catch (error) {
    //   console.error(`Failed to update template ${template.id}`, error)
    //   throw error
    // }
  }

  // 使用特定模板生成專案 (後端尚未實作此端點)
  async generateProject(
    templateId: string,
    config: ProjectTemplateGenerationConfig
  ): Promise<{ projectId: string, repositoryUrl: string }> {
    // TODO: 等待後端實作 POST /api/Templates/{id}/generate 端點
    throw new Error('Generate project functionality not yet implemented in backend')
    // try {
    //   const response = await apiClient.post<{ projectId: string, repositoryUrl: string }>(
    //     `/api/Templates/${templateId}/generate`,
    //     config
    //   )
    //   return response.data
    // } catch (error) {
    //   console.error(`Failed to generate project from template ${templateId}`, error)
    //   throw error
    // }
  }

  // 檢索熱門模板
  async getPopularTemplates(): Promise<ProjectTemplate[]> {
    try {
      const response = await apiClient.get<ProjectTemplate[]>('/api/Templates/popular')
      return response.data
    } catch (error) {
      console.error('Failed to fetch popular templates', error)
      throw error
    }
  }
}