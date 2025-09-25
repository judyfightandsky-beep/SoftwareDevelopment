# DevAuth 開發文檔總覽

## 📚 文檔目錄

### 🎯 核心專案文檔
> 專案專屬的詳細文檔位於 `DevAuth.API/docs/` 目錄

- **[專案分析文檔](../DevAuth.API/docs/project-analysis.md)** - 完整的需求分析與專案規範
- **[資料庫架構文檔](../DevAuth.API/docs/database-architecture.md)** - 資料庫設計與架構說明
- **[專案 README](../DevAuth.API/docs/README.md)** - 專案快速開始指南

### 🧭 開發指南與規範
> 團隊共用的開發標準與流程

#### 📋 流程與協作
- **[代理協作流程指南](Agent-Collaboration-Guide.md)** - 系統分析師、設計師、開發工程師協作流程
- **[系統開發框架](Development-Framework.md)** - 完整的開發生命週期指南

#### 🏗️ 架構與設計
- **[DDD 架構概述](DDD_Architecture_Overview.md)** - Domain-Driven Design 架構說明
- **[領域模型設計](Domain_Model_Design.md)** - User 聚合與領域模型詳細設計
- **[領域服務與事件設計](Domain_Services_And_Events.md)** - 認證服務與領域事件規範

#### 💻 編程規範
- **[C# 編程規範](CSharp-Coding-Guidelines.md)** - 團隊 C# 編碼標準
- **[C# 程式碼範例](CSharp_Code_Examples.md)** - 實際程式碼範例與最佳實踐

#### 🎨 UI 設計
- **[UI 設計系統](UI-Design-System.md)** - 使用者介面設計規範
- **[UI 原型](../ui-prototype/)** - 登入、註冊、管理頁面原型

### 📂 歸檔文檔
> 歷史文檔，供參考使用

- **[Application Layer Design](archived-guides/Application_Layer_Design.md)** - 應用層設計歷史版本
- **[Interface Contracts](archived-guides/Interface_Contracts.md)** - 介面契約設計
- **[UML Diagrams](archived-guides/UML-Diagrams.md)** - UML 圖表設計

## 🚀 快速開始

### 對於新開發者
1. 先閱讀 **[專案分析文檔](../DevAuth.API/docs/project-analysis.md)** 了解專案需求與目標
2. 參考 **[代理協作流程指南](Agent-Collaboration-Guide.md)** 了解開發流程
3. 學習 **[C# 編程規範](CSharp-Coding-Guidelines.md)** 確保代碼品質
4. 查看 **[領域模型設計](Domain_Model_Design.md)** 理解業務邏輯

### 對於系統分析師
- 使用 **[系統開發框架](Development-Framework.md)** 指導需求分析流程
- 參考現有的 **[專案分析文檔](../DevAuth.API/docs/project-analysis.md)** 作為模板

### 對於系統設計師
- 基於 **[DDD 架構概述](DDD_Architecture_Overview.md)** 進行設計
- 參考 **[領域模型設計](Domain_Model_Design.md)** 和 **[領域服務設計](Domain_Services_And_Events.md)**

### 對於開發工程師
- 遵循 **[C# 編程規範](CSharp-Coding-Guidelines.md)**
- 參考 **[程式碼範例](CSharp_Code_Examples.md)** 進行實作

## 📝 文檔維護

- **專案專屬文檔**：更新 `DevAuth.API/docs/` 中的文檔
- **通用開發指南**：更新 `docs/` 中的相關文檔
- **歷史歸檔**：過期或不再使用的文檔移至 `docs/archived-guides/`

## 🔗 相關連結

- [DevAuth.API 專案目錄](../DevAuth.API/)
- [UI 原型目錄](../ui-prototype/)
- [資料夾結構說明](../FOLDER_STRUCTURE.md)