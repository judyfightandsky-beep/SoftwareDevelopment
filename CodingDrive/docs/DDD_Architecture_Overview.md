# DDD架構概覽 - 使用者登入及註冊系統

## 1. 四層架構設計

### Presentation Layer (展示層)
- **職責**: 處理HTTP請求、響應格式化、輸入驗證
- **元件**: Controllers, DTOs, View Models
- **依賴**: 僅依賴Application Layer

### Application Layer (應用層)
- **職責**: 協調領域物件、執行用例、處理事務邊界
- **元件**: Use Cases, Application Services, Command/Query Handlers
- **依賴**: 依賴Domain Layer介面

### Domain Layer (領域層)
- **職責**: 核心業務邏輯、領域規則、業務不變量
- **元件**: Aggregates, Entities, Value Objects, Domain Services, Domain Events
- **依賴**: 無外部依賴，定義基礎設施介面

### Infrastructure Layer (基礎設施層)
- **職責**: 資料存取、外部服務整合、技術實作
- **元件**: Repositories實作、External Services、Data Mappers
- **依賴**: 實作Domain Layer定義的介面

## 2. 依賴關係圖

```
┌─────────────────────────────────────────┐
│           Presentation Layer            │
│     (WebAPI Controllers, DTOs)          │
└─────────────┬───────────────────────────┘
              │ depends on
              ▼
┌─────────────────────────────────────────┐
│           Application Layer             │
│    (Use Cases, Application Services)    │
└─────────────┬───────────────────────────┘
              │ depends on
              ▼
┌─────────────────────────────────────────┐
│             Domain Layer                │
│  (Aggregates, Entities, Value Objects,  │
│   Domain Services, Repository Interfaces)│
└─────────────▲───────────────────────────┘
              │ implements
              │
┌─────────────────────────────────────────┐
│          Infrastructure Layer           │
│   (Repository Implementations,          │
│    External Service Adapters)           │
└─────────────────────────────────────────┘
```

## 3. 職責邊界定義

### 層級間通訊規則
1. **單向依賴**: 上層可以依賴下層，下層不可依賴上層
2. **介面隔離**: Domain層定義介面，Infrastructure層實作
3. **依賴注入**: 透過DI容器管理依賴關係

### 各層核心職責

#### Presentation Layer
```
✅ HTTP請求/響應處理
✅ 輸入驗證和資料轉換
✅ 認證和授權檢查
❌ 業務邏輯處理
❌ 資料存取操作
```

#### Application Layer
```
✅ 用例協調和編排
✅ 事務邊界管理
✅ 領域事件發佈
❌ 具體業務規則
❌ 資料存取實作
```

#### Domain Layer
```
✅ 業務邏輯和規則
✅ 領域模型定義
✅ 不變量維護
❌ 資料存取操作
❌ 外部服務呼叫
```

#### Infrastructure Layer
```
✅ 資料存取實作
✅ 外部服務整合
✅ 技術基礎設施
❌ 業務邏輯決策
❌ 領域規則執行
```

## 4. 關鍵設計原則

### 依賴反轉原則 (DIP)
- Domain Layer定義Repository和Service介面
- Infrastructure Layer實作這些介面
- 透過DI容器注入具體實作

### 單一職責原則 (SRP)
- 每個聚合負責一個業務概念
- 每個用例處理一個特定場景
- 每個服務專注特定領域

### 開放封閉原則 (OCP)
- 透過介面擴展功能
- 新增實作而非修改現有程式碼
- 支援多種認證方式擴展

### 介面隔離原則 (ISP)
- 細化的Repository介面
- 專用的Domain Service介面
- 避免臃腫介面設計

## 5. 核心架構優勢

### 可測試性
- 純淨的領域模型易於單元測試
- 介面隔離支援模擬測試
- 層級分離降低測試複雜度

### 可維護性
- 清晰的職責劃分
- 低耦合高內聚設計
- 領域知識集中管理

### 可擴展性
- 新功能通過新增聚合實現
- 多種資料存取方式支援
- 外部服務整合彈性

### 業務對齊
- 領域模型反映真實業務
- 統一語言貫穿各層
- 業務專家參與設計

## 6. 技術實作策略

### 依賴注入配置
```csharp
// 在Program.cs或Startup.cs中配置
services.AddScoped<IUserRepository, SqlUserRepository>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<LoginUseCase>();
```

### 領域事件處理
```csharp
// 使用MediatR或類似框架處理領域事件
services.AddMediatR(typeof(UserLoggedInEvent));
```

### 資料庫抽象
```csharp
// 透過Repository模式隔離資料存取
public interface IUserRepository
{
    Task<User> GetByIdAsync(UserId id);
    Task SaveAsync(User user);
}
```

這個架構設計確保了系統的可維護性、可測試性和可擴展性，同時保持了領域邏輯的純淨和業務規則的集中管理。