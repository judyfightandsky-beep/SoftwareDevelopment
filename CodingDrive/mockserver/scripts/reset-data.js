const fs = require('fs');
const path = require('path');

console.log('🔄 重置模擬數據庫...');

// 讀取備份數據文件
const backupPath = path.join(__dirname, '../db.json');
const targetPath = path.join(__dirname, '../db.json');

// 這裡可以重置為原始數據，或者清空某些集合
const resetData = {
  "users": [
    {
      "id": "user-1",
      "name": "張明",
      "email": "ming.zhang@example.com",
      "role": "TechLead",
      "password": "$2a$10$K8f5QnWg.uFqAGO4dE7C6O4gX7HLhE7.4BxFqgGZ8oCXd1pD3jLTW",
      "status": "Active",
      "createdAt": "2024-01-15T08:00:00Z",
      "updatedAt": "2024-09-20T10:30:00Z"
    },
    {
      "id": "user-2",
      "name": "李小華",
      "email": "xiaohua.li@example.com",
      "role": "Developer",
      "password": "$2a$10$K8f5QnWg.uFqAGO4dE7C6O4gX7HLhE7.4BxFqgGZ8oCXd1pD3jLTW",
      "status": "Active",
      "createdAt": "2024-02-01T09:15:00Z",
      "updatedAt": "2024-09-19T14:20:00Z"
    },
    {
      "id": "user-3",
      "name": "王大明",
      "email": "daming.wang@example.com",
      "role": "ProjectManager",
      "password": "$2a$10$K8f5QnWg.uFqAGO4dE7C6O4gX7HLhE7.4BxFqgGZ8oCXd1pD3jLTW",
      "status": "Active",
      "createdAt": "2024-01-20T11:00:00Z",
      "updatedAt": "2024-09-21T16:45:00Z"
    }
  ],
  "templates": [],
  "projects": [],
  "tasks": [],
  "qualityChecks": [],
  "workflows": [],
  "workflowInstances": []
};

try {
  fs.writeFileSync(targetPath, JSON.stringify(resetData, null, 2), 'utf8');
  console.log('✅ 數據庫已重置為初始狀態');
  console.log('📝 保留了用戶數據，清空了其他所有集合');
  console.log('🔐 測試登入: ming.zhang@example.com / password123');
} catch (error) {
  console.error('❌ 重置失敗:', error.message);
  process.exit(1);
}