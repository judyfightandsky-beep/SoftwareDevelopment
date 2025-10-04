// 專案模板相關型別定義
export interface TechnologyStackType {
  framework: string;
  version: string;
  database: string;
  runtime: string;
  libraries: string[];
}

export interface TemplateType {
  id: string;
  name: string;
  description: string;
  type: 'DDD' | 'CQRS' | 'CleanArchitecture' | 'Microservices' | 'MVC' | 'Custom';
  techStack: TechnologyStackType;
  createdAt: Date;
  createdBy: string;
}

export interface TemplateConfigurationType {
  modules: string[];
  databaseSettings: {
    type: string;
    connectionString?: string;
  };
  authenticationSettings: {
    type: string;
    strategy: string;
  };
}

export interface ProjectGenerationParams {
  templateId: string;
  projectName: string;
  namespace: string;
  configuration: TemplateConfigurationType;
}