# 軟體開發專案管理平台 - 前端

React 微前端架構，使用 Module Federation 實現模組化開發。

## 🏗️ 架構說明

```
frontend/
├── apps/
│   ├── host/           # 主應用 (Host App) - 端口 3000
│   │   ├── 負責路由管理
│   │   ├── 載入遠端組件
│   │   └── 整體 UI 框架
│   ├── auth/           # 認證模組 (Remote App) - 端口 3001
│   │   ├── 註冊功能
│   │   ├── 登入功能
│   │   └── 身份驗證相關
│   └── dashboard/      # 儀表板模組 (未來實作)
└── shared/             # 共享組件和工具
```

## 🚀 快速開始

### 1. 安裝相依套件

```bash
# 安裝根目錄套件
npm install

# 安裝所有應用套件
npm run install:all
```

### 2. 環境設定

複製環境變數檔案並配置：

```bash
# Auth 模組
cp apps/auth/.env.example apps/auth/.env
```

編輯 `apps/auth/.env`：
```env
VITE_API_BASE_URL=https://localhost:7071
VITE_ENV=development
```

### 3. 開發模式啟動

```bash
# 啟動所有應用
npm run dev

# 或單獨啟動
npm run dev:host      # 主應用 (http://localhost:3000)
npm run dev:auth      # 認證模組 (http://localhost:3001)
```

### 4. 建置專案

```bash
# 建置所有模組
npm run build

# 或單獨建置
npm run build:host
npm run build:auth
```

## 📋 註冊功能 API 串接

### API 端點
- **URL**: `POST /api/users/register`
- **Content-Type**: `application/json`

### 請求格式
```typescript
interface RegisterRequest {
  username: string;      // 使用者名稱 (3-50字元)
  email: string;         // 電子信箱
  firstName: string;     // 名字 (最大50字元)
  lastName: string;      // 姓氏 (最大50字元)
  password: string;      // 密碼 (最少8字元，需包含大小寫字母和數字)
  confirmPassword: string; // 確認密碼
}
```

### 回應格式
```typescript
// 成功 (201 Created)
interface RegisterResponse {
  userId: string;
  username: string;
  email: string;
  role: string;
  status: string;
  requiresApproval: boolean;
}

// 錯誤 (400 Bad Request / 409 Conflict)
interface ApiError {
  error: string;
  message: string;
}
```

### 驗證規則
- **使用者名稱**: 3-50字元，必填
- **電子信箱**: 有效格式，必填
- **名字**: 最大50字元，必填
- **姓氏**: 最大50字元，必填
- **密碼**: 最少8字元，需包含大寫字母、小寫字母和數字
- **確認密碼**: 必須與密碼相符

## 🔧 開發指南

### 新增遠端模組

1. **建立新的 app 目錄**
2. **配置 vite.config.ts**:
   ```typescript
   federation({
     name: 'new-app',
     filename: 'remoteEntry.js',
     exposes: {
       './Component': './src/components/Component'
     },
     shared: ['react', 'react-dom']
   })
   ```
3. **在 host 中註冊遠端**:
   ```typescript
   remotes: {
     newApp: 'http://localhost:3002/assets/remoteEntry.js'
   }
   ```

### 組件開發規範

- 使用 TypeScript
- 實作 Props 介面
- 包含錯誤處理
- 響應式設計
- 無障礙性支援

## 🧪 測試 API 串接

### 測試步驟

1. **啟動後端 API**:
   ```bash
   cd ../SoftwareDevelopment.API
   dotnet run
   ```

2. **啟動前端應用**:
   ```bash
   npm run dev
   ```

3. **開啟瀏覽器**: http://localhost:3000

4. **測試註冊流程**:
   - 填寫註冊表單
   - 檢查驗證錯誤
   - 提交並觀察網路請求
   - 確認 API 回應處理

### 常見問題排解

**CORS 錯誤**:
- 確認後端已設定 CORS
- 檢查 API URL 是否正確

**模組載入失敗**:
- 確認所有服務都已啟動
- 檢查端口是否衝突

**網路請求失敗**:
- 確認 API 服務運行正常
- 檢查環境變數設定

## 📝 下一步開發

1. **登入功能** - 實作 LoginForm 組件
2. **儀表板模組** - 用戶管理介面
3. **共享組件庫** - 統一 UI 組件
4. **狀態管理** - Redux/Zustand 整合
5. **測試覆蓋** - 單元測試和整合測試