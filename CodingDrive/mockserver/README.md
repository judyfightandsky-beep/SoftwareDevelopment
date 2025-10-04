# Software Development Mock Server

一個完整的 Mock API 服務器，用於支持前端開發過程中的 API 模擬。基於 JSON Server 構建，提供認證、項目模板、任務管理、代碼品質檢查和工作流程等功能。

## 🚀 快速開始

### 安裝依賴

```bash
npm install
```

### 啟動服務器

```bash
# 使用啟動腳本（推薦）
./start.sh

# 或直接使用 npm
npm start

# 開發模式（自動重啟）
npm run dev
```

服務器將在 `http://localhost:3001` 啟動。

## 📊 API 端點

### 基礎配置
- **基礎 URL**: `http://localhost:3001/api/v2`
- **認證方式**: Bearer Token (JWT)
- **數據格式**: JSON

### 認證端點

#### 登入
```http
POST /api/v2/auth/login
Content-Type: application/json

{
  "email": "ming.zhang@example.com",
  "password": "password123"
}
```

**響應:**
```json
{
  "success": true,
  "data": {
    "token": "jwt-token-here",
    "user": {
      "id": "user-1",
      "name": "張明",
      "email": "ming.zhang@example.com",
      "role": "TechLead"
    }
  }
}
```

#### 註冊
```http
POST /api/v2/auth/register
Content-Type: application/json

{
  "username": "新用戶",
  "email": "newuser@example.com",
  "firstName": "新",
  "lastName": "用戶",
  "password": "password123",
  "confirmPassword": "password123"
}
```

#### 刷新 Token
```http
POST /api/v2/auth/refresh
Authorization: Bearer {token}
```

#### 檢查可用性
```http
GET /api/v2/auth/check-availability?username=testuser
GET /api/v2/auth/check-availability?email=test@example.com
```

### 項目模板端點

#### 獲取所有模板
```http
GET /api/v2/templates
Authorization: Bearer {token}
```

#### 獲取熱門模板
```http
GET /api/v2/templates/popular
Authorization: Bearer {token}
```

#### 獲取特定模板
```http
GET /api/v2/templates/{id}
Authorization: Bearer {token}
```

#### 創建模板
```http
POST /api/v2/templates
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "新模板",
  "description": "模板描述",
  "type": "DDD",
  "techStack": {
    "framework": ".NET Core",
    "version": "8.0",
    "database": "PostgreSQL",
    "runtime": ".NET 8",
    "libraries": ["Entity Framework Core"]
  }
}
```

#### 生成項目
```http
POST /api/v2/templates/{id}/generate
Authorization: Bearer {token}
Content-Type: application/json

{
  "projectName": "我的新項目",
  "namespace": "MyCompany.MyProject",
  "selectedModules": ["mod-1", "mod-2"],
  "additionalSettings": {}
}
```

### 任務管理端點

#### 獲取任務列表
```http
GET /api/v2/tasks?projectId={id}&status={status}&assignedTo={userId}
Authorization: Bearer {token}
```

#### 創建任務
```http
POST /api/v2/tasks
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "新任務",
  "description": "任務描述",
  "type": "Feature",
  "priority": "High",
  "projectId": "project-1",
  "assignedTo": "user-1",
  "dueDate": "2024-12-31T17:00:00Z",
  "estimatedHours": 8
}
```

#### 更新任務
```http
PUT /api/v2/tasks/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "InProgress",
  "progress": 50,
  "actualHours": 4
}
```

### 代碼品質檢查端點

#### 獲取品質檢查列表
```http
GET /api/v2/qualityChecks?projectId={id}
Authorization: Bearer {token}
```

#### 啟動品質檢查
```http
POST /api/v2/quality-checks
Authorization: Bearer {token}
Content-Type: application/json

{
  "projectId": "project-1",
  "commitHash": "abc123def456",
  "branchName": "feature/new-feature",
  "trigger": "Manual",
  "configuration": {
    "enabledRules": ["all"],
    "includeTestFiles": true
  }
}
```

### 工作流程端點

#### 獲取工作流程定義
```http
GET /api/v2/workflows
Authorization: Bearer {token}
```

#### 啟動工作流程實例
```http
POST /api/v2/workflows/{id}/start
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "代碼審查流程",
  "variables": {
    "pullRequestId": "123",
    "reviewer": "user-1"
  },
  "projectId": "project-1",
  "taskId": "task-1"
}
```

#### 獲取工作流程實例
```http
GET /api/v2/workflowInstances?workflowDefinitionId={id}&status={status}
Authorization: Bearer {token}
```

## 🔐 測試帳號

系統預設了以下測試帳號：

| 郵箱 | 密碼 | 角色 | 名稱 |
|------|------|------|------|
| ming.zhang@example.com | password123 | TechLead | 張明 |
| xiaohua.li@example.com | password123 | Developer | 李小華 |
| daming.wang@example.com | password123 | ProjectManager | 王大明 |

## 📋 數據結構

### 用戶 (User)
```typescript
interface User {
  id: string;
  name: string;
  email: string;
  role: 'ProjectManager' | 'Developer' | 'TechLead' | 'SystemAnalyst';
  status: 'Active' | 'Suspended';
  createdAt: string;
  updatedAt: string;
}
```

### 項目模板 (ProjectTemplate)
```typescript
interface ProjectTemplate {
  id: string;
  name: string;
  description: string;
  type: 'DDD' | 'CQRS' | 'CleanArchitecture' | 'Microservices' | 'MVC' | 'Custom';
  status: 'Draft' | 'Active' | 'Deprecated' | 'Archived';
  techStack: TechnologyStack;
  modules: TemplateModule[];
  metadata: {
    downloadCount: number;
    averageRating: number;
    lastUsed: Date;
    tags: string[];
  };
  createdAt: string;
  updatedAt: string;
  createdBy: string;
  updatedBy: string;
}
```

### 任務 (Task)
```typescript
interface Task {
  id: string;
  taskNumber: string;
  title: string;
  description: string;
  type: 'Feature' | 'Bug' | 'Refactor' | 'Documentation' | 'Other';
  priority: 'Critical' | 'High' | 'Medium' | 'Low';
  status: 'Todo' | 'InProgress' | 'Review' | 'Testing' | 'Done' | 'Closed';
  projectId: string;
  assignedTo?: string;
  createdBy: string;
  createdAt: string;
  dueDate?: string;
  estimatedHours?: number;
  actualHours?: number;
  progress?: number;
}
```

## 🛠 開發指令

```bash
# 安裝依賴
npm install

# 啟動服務器
npm start

# 開發模式（自動重啟）
npm run dev

# 重置數據庫
npm run reset-data
```

## 🔧 配置

### 環境變數
- `PORT`: 服務器端口（默認: 3001）
- `JWT_SECRET`: JWT 簽名密鑰（生產環境請修改）

### CORS 配置
默認允許的來源：
- `http://localhost:3000`
- `http://localhost:5173`
- `http://127.0.0.1:3000`

## 📁 文件結構

```
mockserver/
├── package.json          # 依賴配置
├── server.js             # 主服務器文件
├── db.json              # 數據庫文件
├── start.sh             # 啟動腳本
├── README.md            # 說明文檔
└── scripts/
    └── reset-data.js    # 數據重置腳本
```

## 🐛 常見問題

### Q: 服務器無法啟動？
A: 請檢查：
1. Node.js 版本是否 >= 14
2. 端口 3001 是否被佔用
3. 依賴是否正確安裝

### Q: 前端無法連接到 API？
A: 請檢查：
1. 前端的 API 基礎 URL 是否正確設置為 `http://localhost:3001/api/v2`
2. CORS 設置是否包含前端域名
3. 認證 token 是否正確設置

### Q: 如何添加新的模擬數據？
A: 編輯 `db.json` 文件，或使用 API 端點動態添加數據。

### Q: 如何重置數據？
A: 執行 `npm run reset-data` 命令。

## 📞 支援

如有問題或建議，請聯繫開發團隊。