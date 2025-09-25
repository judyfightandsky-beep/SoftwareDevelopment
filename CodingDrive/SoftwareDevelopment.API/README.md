# DevAuth API

開發者身份驗證系統 - 基於 .NET 8 和 DDD 架構的 RESTful API

## 專案結構

本專案採用 DDD（Domain-Driven Design）四層架構設計：

```
src/
├── 1-Presentation/           # 展示層
│   └── DevAuth.Api/         # Web API 專案
├── 2-Application/           # 應用層
│   └── DevAuth.Application/ # 應用邏輯和使用案例
├── 3-Domain/               # 領域層
│   └── DevAuth.Domain/     # 領域模型和業務邏輯
└── 4-Infrastructure/       # 基礎設施層
    └── DevAuth.Infrastructure/ # 資料存取和外部服務
```

## 主要特色

- **DDD 架構設計**：清楚的責任分離和依賴關係
- **CQRS 模式**：命令和查詢責任分離
- **RESTful API**：符合 REST 原則的 API 設計
- **全域例外處理**：統一的錯誤處理機制
- **強型別設計**：使用值物件確保資料完整性
- **領域事件**：解耦的業務邏輯處理
- **實體框架整合**：完整的 ORM 配置
- **Swagger 文件**：自動生成的 API 文件
- **結構化日誌**：使用 Serilog 進行日誌記錄

## 技術棧

- **.NET 8**：最新的 .NET 平台
- **ASP.NET Core**：Web API 框架
- **Entity Framework Core**：ORM 資料存取
- **MediatR**：中介者模式實作
- **FluentValidation**：輸入驗證
- **BCrypt.Net**：密碼雜湊
- **Serilog**：結構化日誌
- **Swagger/OpenAPI**：API 文件

## 快速開始

### 前置需求

- .NET 8 SDK
- SQL Server（LocalDB 或完整版本）
- Visual Studio 2022 或 VS Code

### 安裝和執行

1. **複製專案**
   ```bash
   git clone <repository-url>
   cd DevAuth.API
   ```

2. **還原套件**
   ```bash
   dotnet restore
   ```

3. **更新資料庫**
   ```bash
   dotnet ef database update --project src/4-Infrastructure/DevAuth.Infrastructure --startup-project src/1-Presentation/DevAuth.Api
   ```

4. **執行專案**
   ```bash
   dotnet run --project src/1-Presentation/DevAuth.Api
   ```

5. **開啟 API 文件**
   - 瀏覽器開啟：`https://localhost:7001/swagger`

## API 端點

### 使用者管理

- `POST /api/users/register` - 註冊新使用者
- `GET /api/users/{id}` - 根據識別碼取得使用者
- `GET /api/users` - 取得使用者清單

### 範例請求

#### 註冊使用者

```http
POST /api/users/register
Content-Type: application/json

{
    "username": "johndoe",
    "email": "john@company.com",
    "firstName": "John",
    "lastName": "Doe",
    "password": "SecurePassword123!",
    "confirmPassword": "SecurePassword123!"
}
```

#### 回應範例

```json
{
    "userId": "123e4567-e89b-12d3-a456-426614174000",
    "username": "johndoe",
    "email": "john@company.com",
    "role": "Employee",
    "status": "PendingApproval",
    "requiresApproval": true
}
```

## 配置設定

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DevAuthDb;Trusted_Connection=true"
  },
  "CompanySettings": {
    "Domains": ["company.com", "devauth.com"],
    "RequireApprovalForEmployees": true
  }
}
```

## 業務邏輯

### 使用者註冊流程

1. **角色自動判定**：系統根據電子信箱網域自動判定使用者角色
   - 公司網域 → 員工角色（需審核）
   - 外部網域 → 訪客角色（直接啟用）

2. **驗證流程**：
   - 訪客：註冊 → 信箱驗證 → 啟用
   - 員工：註冊 → 信箱驗證 → 管理員審核 → 啟用

3. **密碼安全**：使用 BCrypt 進行密碼雜湊儲存

### 權限模型

- **訪客**：基本閱讀權限
- **員工**：開發相關權限，可加入團隊和專案
- **主管**：團隊和專案管理權限
- **系統管理員**：完整系統管理權限

## 開發指南

### 新增功能

1. **領域層**：定義聚合、實體、值物件和領域服務
2. **應用層**：實作命令、查詢和處理器
3. **基礎設施層**：實作儲存庫和外部服務整合
4. **展示層**：建立控制器和 DTO

### 編碼規範

專案遵循 [多奇數位 C# 程式碼撰寫規範](CSharp-Coding-Guidelines.md)：

- 使用 Pascal Case 命名類別和方法
- 使用 Camel Case 命名參數和區域變數
- 每個類別都有完整的 XML 文件註釋
- 啟用 nullable reference types
- 使用 record 定義不可變資料結構

## 測試

```bash
# 執行所有測試
dotnet test

# 執行特定專案測試
dotnet test tests/DevAuth.UnitTests/
```

## 部署

### Docker 容器化

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DevAuth.Api.dll"]
```

## 貢獻

1. Fork 專案
2. 建立功能分支：`git checkout -b feature/amazing-feature`
3. 提交變更：`git commit -m 'Add amazing feature'`
4. 推送分支：`git push origin feature/amazing-feature`
5. 建立 Pull Request

## 授權

此專案使用 MIT 授權條款。詳見 [LICENSE](LICENSE) 檔案。

## 聯絡資訊

- 專案負責人：DevAuth 開發團隊
- 電子信箱：support@devauth.com
- 文件：[API 文件](https://api.devauth.com/docs)