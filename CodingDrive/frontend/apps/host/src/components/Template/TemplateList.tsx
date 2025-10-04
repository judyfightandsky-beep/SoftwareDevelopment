import React, { useState, useEffect } from 'react';
import { TemplateType, ProjectGenerationParams } from '../../types/template';

// 專案模板列表組件
const TemplateList: React.FC = () => {
  const [templates, setTemplates] = useState<TemplateType[]>([]);
  const [selectedTemplate, setSelectedTemplate] = useState<TemplateType | null>(null);

  useEffect(() => {
    // TODO: 實作從後端獲取專案模板的邏輯
    const fetchTemplates = async () => {
      try {
        // 呼叫 API 取得模板列表
        // const response = await templateService.getTemplates();
        // setTemplates(response);
      } catch (error) {
        console.error('無法載入專案模板', error);
      }
    };

    fetchTemplates();
  }, []);

  const handleTemplateSelect = (template: TemplateType) => {
    setSelectedTemplate(template);
  };

  const handleProjectGeneration = async (params: ProjectGenerationParams) => {
    try {
      // TODO: 實作專案生成邏輯
      // const result = await templateService.generateProject(params);
      // 處理生成結果
    } catch (error) {
      console.error('專案生成失敗', error);
    }
  };

  return (
    <div className="template-list-container">
      <h2>專案模板管理</h2>
      <div className="template-grid">
        {templates.map((template) => (
          <div
            key={template.id}
            className={`template-card ${selectedTemplate?.id === template.id ? 'selected' : ''}`}
            onClick={() => handleTemplateSelect(template)}
          >
            <h3>{template.name}</h3>
            <p>{template.description}</p>
            <div className="template-details">
              <span>框架：{template.techStack.framework}</span>
              <span>資料庫：{template.techStack.database}</span>
            </div>
          </div>
        ))}
      </div>

      {selectedTemplate && (
        <div className="template-generation-form">
          <h3>產生專案：{selectedTemplate.name}</h3>
          {/* TODO: 建立專案生成表單 */}
          <form onSubmit={(e) => {
            e.preventDefault();
            // 在這裡實作專案生成的邏輯
          }}>
            {/* 專案名稱、命名空間等輸入欄位 */}
            <button type="submit">生成專案</button>
          </form>
        </div>
      )}
    </div>
  );
};

export default TemplateList;