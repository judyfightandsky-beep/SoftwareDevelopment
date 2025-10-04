import React, { useState, useEffect } from 'react';
import {
  WorkflowDefinitionType,
  WorkflowInstanceType,
  WorkflowCreateParams,
  WorkflowStartParams,
  WorkflowNodeType,
  WorkflowTransitionType
} from '../../types/workflow';

// 工作流程設計器和管理組件
const WorkflowDesigner: React.FC = () => {
  const [workflowDefinitions, setWorkflowDefinitions] = useState<WorkflowDefinitionType[]>([]);
  const [workflowInstances, setWorkflowInstances] = useState<WorkflowInstanceType[]>([]);
  const [selectedDefinition, setSelectedDefinition] = useState<WorkflowDefinitionType | null>(null);
  const [selectedInstance, setSelectedInstance] = useState<WorkflowInstanceType | null>(null);

  useEffect(() => {
    // TODO: 實作從後端獲取工作流程定義和實例的邏輯
    const fetchWorkflows = async () => {
      try {
        // const definitionsResponse = await workflowService.getDefinitions();
        // const instancesResponse = await workflowService.getInstances();
        // setWorkflowDefinitions(definitionsResponse);
        // setWorkflowInstances(instancesResponse);
      } catch (error) {
        console.error('無法載入工作流程', error);
      }
    };

    fetchWorkflows();
  }, []);

  const handleWorkflowDefinitionCreate = async (params: WorkflowCreateParams) => {
    try {
      // TODO: 實作建立工作流程定義的邏輯
      // const newDefinition = await workflowService.createDefinition(params);
      // setWorkflowDefinitions([...workflowDefinitions, newDefinition]);
    } catch (error) {
      console.error('工作流程定義創建失敗', error);
    }
  };

  const handleWorkflowStart = async (params: WorkflowStartParams) => {
    try {
      // TODO: 實作啟動工作流程實例的邏輯
      // const newInstance = await workflowService.startWorkflow(params);
      // setWorkflowInstances([...workflowInstances, newInstance]);
    } catch (error) {
      console.error('工作流程啟動失敗', error);
    }
  };

  const renderWorkflowGraph = (definition: WorkflowDefinitionType) => {
    // TODO: 使用 D3.js 或類似庫來視覺化工作流程圖
    return (
      <div className="workflow-graph">
        {definition.nodes.map(node => (
          <div key={node.id} className={`workflow-node ${node.type.toLowerCase()}`}>
            {node.name}
          </div>
        ))}
        {definition.transitions.map(transition => (
          <div key={transition.id} className="workflow-transition">
            {transition.fromNodeId} → {transition.toNodeId}
          </div>
        ))}
      </div>
    );
  };

  const renderWorkflowInstanceTimeline = (instance: WorkflowInstanceType) => {
    return (
      <div className="workflow-instance-timeline">
        {instance.nodeExecutions.map(execution => (
          <div
            key={execution.id}
            className={`node-execution ${execution.status.toLowerCase()}`}
          >
            <span>{execution.nodeId}</span>
            <span>{execution.status}</span>
            <span>{new Date(execution.startedAt).toLocaleString()}</span>
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="workflow-designer-container">
      <h2>工作流程管理</h2>

      <div className="workflow-definitions">
        <h3>工作流程定義</h3>
        {workflowDefinitions.map(definition => (
          <div
            key={definition.id}
            className={`workflow-definition ${definition.status.toLowerCase()}`}
            onClick={() => setSelectedDefinition(definition)}
          >
            <h4>{definition.name}</h4>
            <p>{definition.description}</p>
            <span>觸發器：{definition.trigger}</span>
          </div>
        ))}

        <button onClick={() => {/* 打開新增工作流程對話框 */}}>
          建立新工作流程
        </button>
      </div>

      {selectedDefinition && (
        <div className="workflow-definition-details">
          <h3>{selectedDefinition.name} - 工作流程圖</h3>
          {renderWorkflowGraph(selectedDefinition)}

          <button
            onClick={() => {
              // 啟動工作流程實例
              handleWorkflowStart({
                workflowDefinitionId: selectedDefinition.id,
                title: `${selectedDefinition.name} - 實例`,
                variables: {}
              });
            }}
          >
            啟動工作流程
          </button>
        </div>
      )}

      <div className="workflow-instances">
        <h3>工作流程實例</h3>
        {workflowInstances.map(instance => (
          <div
            key={instance.id}
            className={`workflow-instance ${instance.status.toLowerCase()}`}
            onClick={() => setSelectedInstance(instance)}
          >
            <h4>{instance.title}</h4>
            <span>狀態：{instance.status}</span>
            <span>開始時間：{new Date(instance.startedAt).toLocaleString()}</span>
          </div>
        ))}
      </div>

      {selectedInstance && (
        <div className="workflow-instance-details">
          <h3>{selectedInstance.title} - 執行時間線</h3>
          {renderWorkflowInstanceTimeline(selectedInstance)}
        </div>
      )}
    </div>
  );
};

export default WorkflowDesigner;