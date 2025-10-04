import React, { useState, useEffect } from 'react';
import { TaskType, TaskCreateParams, TaskUpdateParams } from '../../types/task';

// 任務看板組件
const TaskBoard: React.FC = () => {
  const [tasks, setTasks] = useState<TaskType[]>([]);
  const [selectedTask, setSelectedTask] = useState<TaskType | null>(null);

  const taskStatuses: TaskType['status'][] = [
    'Todo', 'InProgress', 'Review', 'Testing', 'Done'
  ];

  useEffect(() => {
    // TODO: 實作從後端獲取任務的邏輯
    const fetchTasks = async () => {
      try {
        // const response = await taskService.getTasks();
        // setTasks(response);
      } catch (error) {
        console.error('無法載入任務', error);
      }
    };

    fetchTasks();
  }, []);

  const handleTaskCreate = async (taskParams: TaskCreateParams) => {
    try {
      // TODO: 實作任務創建邏輯
      // const newTask = await taskService.createTask(taskParams);
      // setTasks([...tasks, newTask]);
    } catch (error) {
      console.error('任務創建失敗', error);
    }
  };

  const handleTaskUpdate = async (taskParams: TaskUpdateParams) => {
    try {
      // TODO: 實作任務更新邏輯
      // const updatedTask = await taskService.updateTask(taskParams);
      // setTasks(tasks.map(task => task.id === updatedTask.id ? updatedTask : task));
    } catch (error) {
      console.error('任務更新失敗', error);
    }
  };

  const handleTaskDragEnd = (result: any) => {
    // 處理任務在看板中拖曳變更狀態的邏輯
    if (!result.destination) return;

    const { source, destination } = result;
    const newStatus = taskStatuses[destination.droppableId];
    const draggedTask = tasks[source.index];

    handleTaskUpdate({
      id: draggedTask.id,
      status: newStatus
    });
  };

  return (
    <div className="task-board-container">
      <h2>任務管理看板</h2>
      <div className="task-board-columns">
        {taskStatuses.map((status, index) => (
          <div key={status} className="task-column">
            <h3>{status}</h3>
            <div className="tasks-container">
              {tasks
                .filter(task => task.status === status)
                .map(task => (
                  <div
                    key={task.id}
                    className="task-card"
                    onClick={() => setSelectedTask(task)}
                  >
                    <h4>{task.title}</h4>
                    <p>{task.description}</p>
                    <div className="task-details">
                      <span>優先級：{task.priority}</span>
                      <span>負責人：{task.assignedTo || '未指派'}</span>
                    </div>
                  </div>
                ))}
            </div>
          </div>
        ))}
      </div>

      {/* 任務詳細資訊和編輯模態框 */}
      {selectedTask && (
        <div className="task-detail-modal">
          {/* TODO: 實作任務詳細資訊顯示和編輯表單 */}
        </div>
      )}

      {/* 新增任務按鈕 */}
      <button onClick={() => {/* 打開新增任務表單 */}}>
        新增任務
      </button>
    </div>
  );
};

export default TaskBoard;