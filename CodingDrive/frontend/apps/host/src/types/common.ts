// 定義通用型別與介面

export enum UserRole {
  ProjectManager = 'ProjectManager',
  Developer = 'Developer',
  TechLead = 'TechLead',
  SystemAnalyst = 'SystemAnalyst'
}

export interface User {
  id: string
  username: string
  email: string
  role?: UserRole
}

export interface BaseEntity {
  id: string
  createdAt: Date
  updatedAt: Date
  createdBy: string
  updatedBy: string
}