import React, { useState } from 'react'
import { ProjectTemplate, ProjectTemplateGenerationConfig } from '../types/project-template'
import { ProjectTemplateService } from '../services/project-template.service'
import { useAuth } from '../contexts/AuthContext'

interface ProjectTemplateDetailsProps {
  template: ProjectTemplate
  onGenerate?: (config: ProjectTemplateGenerationConfig) => void
}

const ProjectTemplateDetails: React.FC<ProjectTemplateDetailsProps> = ({ template, onGenerate }) => {
  const [projectName, setProjectName] = useState('')
  const [namespace, setNamespace] = useState('')
  const [selectedModules, setSelectedModules] = useState<string[]>([])
  const [additionalSettings, setAdditionalSettings] = useState<Record<string, unknown>>({})

  const { user } = useAuth()
  const templateService = new ProjectTemplateService()

  const handleModuleToggle = (moduleId: string) => {
    setSelectedModules(prev =>
      prev.includes(moduleId)
        ? prev.filter(id => id !== moduleId)
        : [...prev, moduleId]
    )
  }

  const handleGenerate = () => {
    if (!projectName || !namespace) {
      alert('請填寫專案名稱和命名空間')
      return
    }

    const config: ProjectTemplateGenerationConfig = {
      projectName,
      namespace,
      selectedModules,
      additionalSettings
    }

    if (onGenerate) {
      onGenerate(config)
    } else {
      templateService.generateProject(template.id, config)
        .then(result => {
          alert(`專案生成成功！專案ID：${result.projectId}`)
        })
        .catch(error => {
          console.error('專案生成失敗', error)
          alert('專案生成失敗，請稍後再試')
        })
    }
  }

  return (
    <div className="bg-white rounded-lg shadow-lg p-6">
      <div className="flex justify-between items-start mb-6">
        <div>
          <h2 className="text-2xl font-bold">{template.name}</h2>
          <p className="text-gray-600">{template.description}</p>
        </div>
        <div className="badge badge-primary">{template.type}</div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div>
          <h3 className="text-lg font-semibold mb-4">技術棧</h3>
          <div className="space-y-2">
            <p>框架：{template.techStack.framework} {template.techStack.version}</p>
            <p>資料庫：{template.techStack.database}</p>
            <p>執行環境：{template.techStack.runtime}</p>
          </div>

          <h3 className="text-lg font-semibold mt-6 mb-4">庫與套件</h3>
          <div className="flex flex-wrap gap-2">
            {template.techStack.libraries.map(lib => (
              <span key={lib} className="badge badge-secondary">{lib}</span>
            ))}
          </div>
        </div>

        <div>
          <h3 className="text-lg font-semibold mb-4">模組選擇</h3>
          <div className="space-y-2">
            {template.modules.map(module => (
              <div key={module.id} className="flex items-center">
                <input
                  type="checkbox"
                  id={module.id}
                  checked={selectedModules.includes(module.id)}
                  onChange={() => handleModuleToggle(module.id)}
                  className="checkbox checkbox-primary mr-2"
                />
                <label htmlFor={module.id} className="flex-1">
                  <div className="font-medium">{module.name}</div>
                  <div className="text-sm text-gray-500">{module.description}</div>
                </label>
                {module.isRequired && (
                  <span className="badge badge-xs badge-info ml-2">必需</span>
                )}
              </div>
            ))}
          </div>

          <div className="mt-6 space-y-4">
            <div>
              <label htmlFor="projectName" className="block mb-2">專案名稱</label>
              <input
                type="text"
                id="projectName"
                value={projectName}
                onChange={(e) => setProjectName(e.target.value)}
                className="input input-bordered w-full"
                placeholder="輸入專案名稱"
              />
            </div>

            <div>
              <label htmlFor="namespace" className="block mb-2">命名空間</label>
              <input
                type="text"
                id="namespace"
                value={namespace}
                onChange={(e) => setNamespace(e.target.value)}
                className="input input-bordered w-full"
                placeholder="輸入命名空間"
              />
            </div>

            <button
              onClick={handleGenerate}
              className="btn btn-primary w-full"
              disabled={!projectName || !namespace}
            >
              生成專案
            </button>
          </div>
        </div>
      </div>

      <div className="mt-6 text-center">
        <p className="text-sm text-gray-500">
          下載次數：{template.metadata.downloadCount} |
          平均評分：{template.metadata.averageRating.toFixed(1)}
        </p>
      </div>
    </div>
  )
}

export default ProjectTemplateDetails