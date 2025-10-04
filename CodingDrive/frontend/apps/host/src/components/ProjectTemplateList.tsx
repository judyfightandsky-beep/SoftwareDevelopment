import React, { useState, useEffect } from 'react'
import { ProjectTemplate, TemplateType } from '../types/project-template'
import { ProjectTemplateService } from '../services/project-template.service'
import { useAuth } from '../contexts/AuthContext'

interface ProjectTemplateListProps {
  onTemplateSelect?: (template: ProjectTemplate) => void
}

const ProjectTemplateList: React.FC<ProjectTemplateListProps> = ({ onTemplateSelect }) => {
  const [templates, setTemplates] = useState<ProjectTemplate[]>([])
  const [filteredTemplates, setFilteredTemplates] = useState<ProjectTemplate[]>([])
  const [selectedType, setSelectedType] = useState<TemplateType | 'All'>('All')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const { user } = useAuth()
  const templateService = new ProjectTemplateService()

  useEffect(() => {
    fetchTemplates()
  }, [])

  useEffect(() => {
    filterTemplates()
  }, [selectedType, templates])

  const fetchTemplates = async () => {
    try {
      setLoading(true)
      const fetchedTemplates = await templateService.getTemplates()
      setTemplates(fetchedTemplates)
      setError(null)
    } catch (err) {
      setError('無法載入專案模板，請稍後再試')
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  const filterTemplates = () => {
    if (selectedType === 'All') {
      setFilteredTemplates(templates)
    } else {
      const filtered = templates.filter(template => template.type === selectedType)
      setFilteredTemplates(filtered)
    }
  }

  const handleTemplateSelect = (template: ProjectTemplate) => {
    if (onTemplateSelect) {
      onTemplateSelect(template)
    }
  }

  const renderTemplateCard = (template: ProjectTemplate) => (
    <div
      key={template.id}
      className="bg-white rounded-lg shadow-md p-4 cursor-pointer hover:shadow-lg transition-all"
      onClick={() => handleTemplateSelect(template)}
    >
      <div className="flex justify-between items-center mb-2">
        <h3 className="text-lg font-semibold">{template.name}</h3>
        <span className="badge badge-primary">{template.type}</span>
      </div>
      <p className="text-gray-600 mb-2">{template.description}</p>
      <div className="flex items-center justify-between">
        <div className="flex space-x-2">
          {template.techStack.libraries.slice(0, 3).map(lib => (
            <span key={lib} className="badge badge-secondary">{lib}</span>
          ))}
        </div>
        <div className="text-sm text-gray-500">
          下載次數：{template.metadata.downloadCount}
        </div>
      </div>
    </div>
  )

  if (loading) {
    return <div>載入中...</div>
  }

  if (error) {
    return <div className="text-red-500">{error}</div>
  }

  return (
    <div className="container mx-auto p-4">
      <div className="mb-4 flex justify-between items-center">
        <h2 className="text-2xl font-bold">專案模板庫</h2>
        <div className="space-x-2">
          <select
            value={selectedType}
            onChange={(e) => setSelectedType(e.target.value as TemplateType | 'All')}
            className="select select-bordered"
          >
            <option value="All">全部類型</option>
            {Object.values(TemplateType).map(type => (
              <option key={type} value={type}>{type}</option>
            ))}
          </select>
        </div>
      </div>

      {filteredTemplates.length === 0 ? (
        <div className="text-center text-gray-500">
          沒有符合的專案模板
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {filteredTemplates.map(renderTemplateCard)}
        </div>
      )}
    </div>
  )
}

export default ProjectTemplateList