# SpecDriveDevelop/CodingDrive 資料夾結構

## 📁 整理後的目錄結構

```
SpecDriveDevelop/CodingDrive/
├── 📁 DevAuth.API/              # 🎯 主要專案：身份驗證 API
│   ├── DevAuth.sln              # Visual Studio 解決方案檔案
│   ├── README.md                # 專案說明文檔
│   ├── 📁 docs/                 # 專案專屬文檔
│   │   ├── README.md            # 文檔索引
│   │   ├── database-architecture.md  # 資料庫架構文檔
│   │   └── project-analysis.md  # 專案分析文檔
│   └── 📁 src/                  # 原始碼 (DDD 4層架構)
│       ├── 📁 1-Presentation/   # 呈現層 (API Controllers)
│       ├── 📁 2-Application/    # 應用層 (Business Logic)
│       ├── 📁 3-Domain/         # 領域層 (Core Domain)
│       └── 📁 4-Infrastructure/ # 基礎設施層 (Database, External)
│
├── 📁 docs/                     # 🧭 共用開發文檔
│   ├── CSharp-Coding-Guidelines.md    # C# 編程規範
│   ├── CSharp_Code_Examples.md        # C# 程式碼範例
│   ├── DDD_Architecture_Overview.md   # DDD 架構概述
│   ├── UI-Design-System.md            # UI 設計系統
│   └── 📁 archived-guides/     # 歷史文檔存檔
│       ├── Agent-Collaboration-Guide.md
│       ├── Application_Layer_Design.md
│       ├── Development-Framework.md
│       ├── Domain_Model_Design.md
│       ├── Domain_Services_And_Events.md
│       ├── Interface_Contracts.md
│       └── UML-Diagrams.md
│
└── 📁 ui-prototype/             # 🎨 使用者介面原型
    ├── login.html               # 登入頁面原型
    ├── register.html            # 註冊頁面原型
    ├── admin.html               # 管理頁面原型
    └── 📁 assets/               # 靜態資源
        ├── 📁 css/              # 樣式檔案
        ├── 📁 js/               # JavaScript 檔案
        └── 📁 images/           # 圖片資源
```

## 🧹 清理動作記錄

### ✅ 已刪除的重複/無用檔案
- `UserAuthSystem/` (重複的使用者認證系統目錄)
- `.spec-workflow/` (未使用的 spec workflow 配置)
- `.claude/` (Claude 配置檔案)
- `user-auth-system/` (重複目錄)
- `src/examples/` (空的範例目錄)
- `.workflow-confirmations.json` (workflow 確認檔案)
- 重複的基礎設施目錄嵌套結構

### 📁 重新組織的檔案
- **開發指南** → `docs/` (主要參考文檔)
- **歷史文檔** → `docs/archived-guides/` (存檔備查)
- **專案文檔** → `DevAuth.API/docs/` (專案專屬)

## 🎯 目前專案狀態

### 主要專案：DevAuth.API
- **狀態**: ✅ 可正常編譯與執行
- **架構**: DDD 4層架構
- **資料庫**: PostgreSQL (已建立並遷移)
- **功能**: 使用者註冊、基本 API 端點

### 開發資源
- **編程指南**: `docs/CSharp-Coding-Guidelines.md`
- **架構說明**: `docs/DDD_Architecture_Overview.md`
- **UI 原型**: `ui-prototype/` 目錄

## 📝 建議

1. **專注開發**: 主要開發工作集中在 `DevAuth.API/` 專案
2. **文檔維護**: 更新相關文檔時優先修改 `DevAuth.API/docs/` 中的專案專屬文檔
3. **原型參考**: UI 開發時可參考 `ui-prototype/` 中的前端原型

## 🔄 下一步開發重點

1. 實作使用者登入功能
2. 完善 JWT 身份驗證
3. 實作電子信箱驗證
4. 開發使用者管理介面