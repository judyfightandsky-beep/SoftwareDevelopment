import { BaseEntity, User } from './common'

export enum TemplateType {
  DDD = 'DDD',
  CQRS = 'CQRS',
  CleanArchitecture = 'CleanArchitecture',
  Microservices = 'Microservices',
  MVC = 'MVC',
  Custom = 'Custom'
}

export enum TemplateStatus {
  Draft = 'Draft',
  Active = 'Active',
  Deprecated = 'Deprecated',
  Archived = 'Archived'
}

export interface TechnologyStack {
  framework: string
  version: string
  database: string
  runtime: string
  libraries: string[]
}

export interface TemplateModule {
  id: string
  name: string
  description: string
  isRequired: boolean
  dependencies: string[]
  settings: Record<string, unknown>
}

export interface ProjectTemplate extends BaseEntity {
  name: string
  description: string
  type: TemplateType
  status: TemplateStatus
  techStack: TechnologyStack
  modules: TemplateModule[]
  metadata: {
    downloadCount: number
    averageRating: number
    lastUsed: Date
    tags: string[]
  }
}

export interface ProjectTemplateGenerationConfig {
  projectName: string
  namespace: string
  selectedModules: string[]
  additionalSettings: Record<string, unknown>
}