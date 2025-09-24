# DevAuth 專案文件

## 文件概述

歡迎來到 DevAuth 專案文件中心。DevAuth 是一個專為軟體開發環境設計的全方位身份驗證與授權系統，採用 .NET Core 8 DDD 架構和 PostgreSQL 資料庫。

## 文件結構

### 📊 [資料庫架構文件](./database-architecture.md)
**作為系統設計師的完整資料庫設計文件**

包含內容：
- 資料庫架構概述與設計原則
- 詳細資料表結構說明（欄位、型別、約束、索引）
- 關聯性圖解與資料流程說明
- 效能優化策略與監控建議
- 備份維護策略與未來擴展規劃

適合對象：
- 資料庫管理員 (DBA)
- 後端工程師
- 系統架構師
- DevOps 工程師

---

### 📋 [專案分析文件](./project-analysis.md)
**作為系統分析師的全方位專案需求與設計分析**

包含內容：
- 專案定位與核心價值主張
- 完整業務需求分析與角色權限體系
- DDD 架構設計與技術實作方案
- 現有功能分析與未來發展規劃
- 部署運維策略與成功指標定義

適合對象：
- 產品經理
- 專案經理
- 系統分析師
- 技術主管
- 開發團隊成員

## 快速開始

### 🚀 專案設定
```bash
# 克隆專案
git clone <repository-url>

# 進入專案目錄
cd DevAuth.API

# 還原套件
dotnet restore

# 更新資料庫
dotnet ef database update --project src/4-Infrastructure/DevAuth.Infrastructure

# 啟動專案
dotnet run --project src/1-Presentation/DevAuth.Api
```

### 🗄️ 資料庫連線
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=192.168.76.202;port=5432;database=codingManager;user id=postgres;password=yesee@24774948"
  }
}
```

### 📡 API 端點
- **API 文件**: http://localhost:5000/swagger
- **健康檢查**: http://localhost:5000/health
- **使用者註冊**: POST /api/users/register

## 專案現況

### ✅ 已完成功能
- 使用者註冊系統
- 角色權限體系設計
- 資料庫架構與 Migration
- DDD 架構基礎框架

### 🚧 開發中功能
- 使用者登入與 JWT 驗證
- 電子信箱驗證流程
- 管理員審核工作流程

### 📋 計劃功能
- 密碼管理與重置
- 多因子身份驗證
- 第三方身份提供者整合
- 使用者管理介面

## 技術堆疊

### 後端技術
- **.NET Core 8**: 主要開發框架
- **PostgreSQL**: 關聯式資料庫
- **Entity Framework Core**: ORM 框架
- **MediatR**: CQRS 模式實作
- **FluentValidation**: 輸入驗證

### 架構模式
- **Domain-Driven Design (DDD)**: 領域驅動設計
- **CQRS**: 命令查詢責任分離
- **Clean Architecture**: 清潔架構
- **Repository Pattern**: 資料存取抽象化

### 開發工具
- **JetBrains Rider / Visual Studio**: IDE
- **Docker**: 容器化部署
- **Swagger**: API 文件生成
- **xUnit**: 單元測試框架

## 團隊角色與權限

### 🎭 使用者角色體系

| 角色 | 權限範圍 | 典型使用者 |
|-----|---------|-----------|
| **Guest** | 基礎瀏覽權限 | 外部合作夥伴、試用使用者 |
| **Employee** | 開發相關權限 | 開發工程師、QA、設計師 |
| **Manager** | 團隊管理權限 | 專案經理、技術主管 |
| **SystemAdmin** | 完整系統權限 | IT 管理員、系統運維 |

### 🔒 權限細分說明
詳細的權限定義請參閱 [專案分析文件 - 角色權限體系](./project-analysis.md#角色權限體系)

## 開發指南

### 📝 程式碼規範
- 遵循 C# 命名慣例
- 使用領域驅動設計原則
- 實作完整的單元測試
- 編寫清楚的程式碼註解

### 🧪 測試策略
```bash
# 執行所有測試
dotnet test

# 產生測試覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

### 🔄 Git 工作流程
- `main`: 生產環境分支
- `develop`: 開發環境分支
- `feature/*`: 功能開發分支
- `hotfix/*`: 緊急修復分支

## 部署與運維

### 🐳 Docker 部署
```bash
# 建構映像檔
docker build -t devauth:latest .

# 啟動容器
docker run -d -p 5000:80 devauth:latest
```

### 📊 監控與日誌
- 健康檢查端點
- 結構化日誌記錄
- 效能指標監控
- 錯誤追蹤系統

## 貢獻指南

### 🤝 如何貢獻
1. Fork 專案
2. 建立功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. 推送分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

### 🐛 回報問題
請使用 GitHub Issues 回報問題，並提供：
- 詳細的問題描述
- 重現步驟
- 預期行為與實際行為
- 系統環境資訊

## 聯絡資訊

### 👥 開發團隊
- **專案負責人**: [姓名]
- **技術架構師**: [姓名]
- **資料庫管理員**: [姓名]

### 📧 聯絡方式
- **技術支援**: tech-support@company.com
- **功能建議**: feature-request@company.com
- **安全問題**: security@company.com

## 版本歷程

### v1.0.0 (開發中)
- ✅ 基礎使用者註冊功能
- ✅ 角色權限體系
- ✅ 資料庫架構設計
- 🚧 使用者登入功能
- 📋 電子信箱驗證

---

## 授權條款

本專案採用 [MIT License](../LICENSE) 開源授權條款。

## 致謝

感謝所有為 DevAuth 專案貢獻程式碼、文件和想法的開發者們。