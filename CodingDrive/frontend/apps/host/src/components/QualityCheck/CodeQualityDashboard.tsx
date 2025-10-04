import React, { useState, useEffect } from 'react';
import {
  CodeQualityCheckType,
  QualityIssueType,
  QualityCheckStartParams
} from '../../types/quality-check';

// AI程式碼品質檢查儀表板
const CodeQualityDashboard: React.FC = () => {
  const [qualityChecks, setQualityChecks] = useState<CodeQualityCheckType[]>([]);
  const [selectedCheck, setSelectedCheck] = useState<CodeQualityCheckType | null>(null);

  useEffect(() => {
    // TODO: 實作從後端獲取程式碼品質檢查的邏輯
    const fetchQualityChecks = async () => {
      try {
        // const response = await qualityCheckService.getQualityChecks();
        // setQualityChecks(response);
      } catch (error) {
        console.error('無法載入程式碼品質檢查', error);
      }
    };

    fetchQualityChecks();
  }, []);

  const handleStartQualityCheck = async (params: QualityCheckStartParams) => {
    try {
      // TODO: 實作啟動程式碼品質檢查的邏輯
      // const newCheck = await qualityCheckService.startCheck(params);
      // setQualityChecks([...qualityChecks, newCheck]);
    } catch (error) {
      console.error('程式碼品質檢查啟動失敗', error);
    }
  };

  const renderIssuesBySeverity = (issues: QualityIssueType[]) => {
    const severityOrder = ['Critical', 'High', 'Medium', 'Low', 'Info'];

    return severityOrder.map(severity => {
      const filteredIssues = issues.filter(issue => issue.severity === severity);

      return filteredIssues.length > 0 ? (
        <div key={severity} className={`issues-${severity.toLowerCase()}`}>
          <h4>{severity}級問題 ({filteredIssues.length})</h4>
          {filteredIssues.map(issue => (
            <div key={issue.id} className="issue-card">
              <h5>{issue.title}</h5>
              <p>{issue.description}</p>
              <div className="issue-details">
                <span>檔案：{issue.fileName}</span>
                <span>行號：{issue.lineNumber}</span>
                <pre>{issue.codeSnippet}</pre>
                <div className="issue-suggestion">
                  <strong>建議：</strong>
                  <p>{issue.suggestion}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : null;
    });
  };

  return (
    <div className="code-quality-dashboard">
      <h2>AI程式碼品質檢查</h2>

      <div className="quality-checks-list">
        {qualityChecks.map(check => (
          <div
            key={check.id}
            className={`quality-check-summary ${check.status.toLowerCase()}`}
            onClick={() => setSelectedCheck(check)}
          >
            <h3>檢查：{check.commitHash}</h3>
            <div className="check-details">
              <span>狀態：{check.status}</span>
              <span>分數：{check.overallScore.score.toFixed(2)}</span>
              <span>等級：{check.overallScore.grade}</span>
            </div>
          </div>
        ))}
      </div>

      {selectedCheck && (
        <div className="quality-check-details">
          <h3>檢查詳情：{selectedCheck.commitHash}</h3>
          <div className="quality-metrics">
            <h4>品質指標</h4>
            {selectedCheck.metrics.map(metric => (
              <div key={metric.name} className="metric-card">
                <span>{metric.name}</span>
                <span>{metric.value} {metric.unit}</span>
              </div>
            ))}
          </div>

          <div className="quality-issues">
            <h4>程式碼問題</h4>
            {renderIssuesBySeverity(selectedCheck.issues)}
          </div>
        </div>
      )}

      <div className="quality-check-actions">
        <button
          onClick={() => {
            // 打開檢查設定和觸發對話框
          }}
        >
          啟動新的品質檢查
        </button>
      </div>
    </div>
  );
};

export default CodeQualityDashboard;