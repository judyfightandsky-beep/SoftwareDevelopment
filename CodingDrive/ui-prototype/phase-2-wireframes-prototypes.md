# 軟體開發專案管理平台 - 第2階段UI/UX原型規格

## 版本資訊
- **文檔版本**：1.0
- **建立日期**：2025-09-27
- **負責人**：系統分析師
- **審核狀態**：待審核
- **相關專案**：SoftwareDevelopment.API - Phase 2

---

## 1. 原型設計概覽

### 1.1 設計理念
- **簡潔高效**：減少認知負荷，專注核心功能
- **角色導向**：針對不同角色優化介面布局
- **情境感知**：根據使用情境動態調整介面
- **一致性**：與第1階段設計語言保持一致
- **可擴展性**：為未來功能擴展預留空間

### 1.2 設計系統規範

**色彩系統**：
- **主色調**：#2563EB（專業藍）
- **輔助色**：#10B981（成功綠）、#F59E0B（警告橙）、#EF4444（錯誤紅）
- **中性色**：#1F2937（深灰）、#6B7280（中灰）、#F9FAFB（淺灰）
- **背景色**：#FFFFFF（白色）、#F8FAFC（淺藍灰）

**字體系統**：
- **主要字體**：Inter, -apple-system, BlinkMacSystemFont
- **標題字重**：600 (SemiBold)
- **內文字重**：400 (Regular)
- **強調字重**：500 (Medium)

**間距系統**：
- **基礎單位**：4px
- **組件間距**：8px, 12px, 16px, 24px, 32px
- **版面間距**：48px, 64px, 80px

**圓角規範**：
- **按鈕/表單**：6px
- **卡片元件**：8px
- **模態框**：12px

### 1.3 響應式斷點
- **XS (Extra Small)**：< 576px (手機直向)
- **SM (Small)**：576px - 768px (手機橫向)
- **MD (Medium)**：768px - 992px (平板)
- **LG (Large)**：992px - 1200px (桌面)
- **XL (Extra Large)**：≥ 1200px (大桌面)

---

## 2. 主要頁面原型設計

### 2.1 專案儀表板 (Project Dashboard)

#### 2.1.1 整體布局

```
┌─────────────────────────────────────────────────────────────────┐
│ Header Navigation [Logo] [Search] [Notifications] [User Menu]   │
├─────────────────────────────────────────────────────────────────┤
│ ┌───────────┐ ┌─────────────────────────────────────────────┐ │
│ │  Sidebar  │ │              Main Content Area              │ │
│ │           │ │                                             │ │
│ │ - 專案總覽 │ │  ┌─────────────┐ ┌─────────────────────────┐ │ │
│ │ - 任務管理 │ │  │ 專案統計卡片 │ │     專案進度概覽         │ │ │
│ │ - 模板中心 │ │  └─────────────┘ └─────────────────────────┘ │ │
│ │ - 品質中心 │ │                                             │ │
│ │ - 報表     │ │  ┌─────────────────────────────────────────┐ │ │
│ │ - 設定     │ │  │          最近活動 & 任務列表            │ │ │
│ └───────────┘ │  └─────────────────────────────────────────┘ │ │
│               └─────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

#### 2.1.2 專案統計卡片組件

```html
<!-- 專案統計卡片 -->
<div class=\"grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8\">
  <!-- 進行中專案 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <div class=\"flex items-center justify-between\">
      <div>
        <p class=\"text-sm font-medium text-gray-600\">進行中專案</p>
        <p class=\"text-3xl font-semibold text-gray-900\">12</p>
      </div>
      <div class=\"w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center\">
        <svg class=\"w-6 h-6 text-blue-600\"><!-- 專案圖示 --></svg>
      </div>
    </div>
    <div class=\"mt-4 flex items-center text-sm\">
      <span class=\"text-green-600 font-medium\">+2</span>
      <span class=\"text-gray-600 ml-1\">本週新增</span>
    </div>
  </div>

  <!-- 待辦任務 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <div class=\"flex items-center justify-between\">
      <div>
        <p class=\"text-sm font-medium text-gray-600\">待辦任務</p>
        <p class=\"text-3xl font-semibold text-gray-900\">47</p>
      </div>
      <div class=\"w-12 h-12 bg-orange-100 rounded-lg flex items-center justify-center\">
        <svg class=\"w-6 h-6 text-orange-600\"><!-- 任務圖示 --></svg>
      </div>
    </div>
    <div class=\"mt-4 flex items-center text-sm\">
      <span class=\"text-red-600 font-medium\">3</span>
      <span class=\"text-gray-600 ml-1\">即將逾期</span>
    </div>
  </div>

  <!-- 代碼品質 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <div class=\"flex items-center justify-between\">
      <div>
        <p class=\"text-sm font-medium text-gray-600\">代碼品質</p>
        <p class=\"text-3xl font-semibold text-gray-900\">8.5</p>
      </div>
      <div class=\"w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center\">
        <svg class=\"w-6 h-6 text-green-600\"><!-- 品質圖示 --></svg>
      </div>
    </div>
    <div class=\"mt-4 flex items-center text-sm\">
      <span class=\"text-green-600 font-medium\">+0.3</span>
      <span class=\"text-gray-600 ml-1\">較上週提升</span>
    </div>
  </div>

  <!-- 團隊效率 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <div class=\"flex items-center justify-between\">
      <div>
        <p class=\"text-sm font-medium text-gray-600\">團隊效率</p>
        <p class=\"text-3xl font-semibold text-gray-900\">92%</p>
      </div>
      <div class=\"w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center\">
        <svg class=\"w-6 h-6 text-purple-600\"><!-- 效率圖示 --></svg>
      </div>
    </div>
    <div class=\"mt-4 flex items-center text-sm\">
      <span class=\"text-green-600 font-medium\">+5%</span>
      <span class=\"text-gray-600 ml-1\">較上月提升</span>
    </div>
  </div>
</div>
```

#### 2.1.3 專案進度圖表組件

```html
<!-- 專案進度概覽 -->
<div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-8\">
  <div class=\"flex items-center justify-between mb-6\">
    <h3 class=\"text-lg font-semibold text-gray-900\">專案進度概覽</h3>
    <div class=\"flex space-x-2\">
      <button class=\"px-3 py-1 text-sm font-medium text-blue-600 bg-blue-50 rounded-md\">
        本週
      </button>
      <button class=\"px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md\">
        本月
      </button>
      <button class=\"px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md\">
        本季
      </button>
    </div>
  </div>

  <!-- 進度條列表 -->
  <div class=\"space-y-4\">
    <div class=\"flex items-center justify-between\">
      <div class=\"flex-1\">
        <div class=\"flex items-center justify-between mb-2\">
          <span class=\"text-sm font-medium text-gray-900\">電商平台重構專案</span>
          <span class=\"text-sm text-gray-600\">85%</span>
        </div>
        <div class=\"w-full bg-gray-200 rounded-full h-2\">
          <div class=\"bg-blue-600 h-2 rounded-full\" style=\"width: 85%\"></div>
        </div>
      </div>
      <div class=\"ml-4 text-sm text-gray-600\">
        <span class=\"px-2 py-1 bg-green-100 text-green-800 rounded-full\">進行中</span>
      </div>
    </div>

    <div class=\"flex items-center justify-between\">
      <div class=\"flex-1\">
        <div class=\"flex items-center justify-between mb-2\">
          <span class=\"text-sm font-medium text-gray-900\">用戶管理系統</span>
          <span class=\"text-sm text-gray-600\">65%</span>
        </div>
        <div class=\"w-full bg-gray-200 rounded-full h-2\">
          <div class=\"bg-blue-600 h-2 rounded-full\" style=\"width: 65%\"></div>
        </div>
      </div>
      <div class=\"ml-4 text-sm text-gray-600\">
        <span class=\"px-2 py-1 bg-yellow-100 text-yellow-800 rounded-full\">需關注</span>
      </div>
    </div>

    <div class=\"flex items-center justify-between\">
      <div class=\"flex-1\">
        <div class=\"flex items-center justify-between mb-2\">
          <span class=\"text-sm font-medium text-gray-900\">API 文件系統</span>
          <span class=\"text-sm text-gray-600\">100%</span>
        </div>
        <div class=\"w-full bg-gray-200 rounded-full h-2\">
          <div class=\"bg-green-600 h-2 rounded-full\" style=\"width: 100%\"></div>
        </div>
      </div>
      <div class=\"ml-4 text-sm text-gray-600\">
        <span class=\"px-2 py-1 bg-gray-100 text-gray-800 rounded-full\">已完成</span>
      </div>
    </div>
  </div>
</div>
```

### 2.2 專案模板中心 (Project Template Center)

#### 2.2.1 模板瀏覽頁面布局

```
┌─────────────────────────────────────────────────────────────────┐
│ ┌─────────────────────────────────────────────────────────────┐ │
│ │ 頁面標題: 專案模板中心                                        │ │
│ │ 副標題: 選擇適合的模板快速啟動你的專案                          │ │
│ └─────────────────────────────────────────────────────────────┘ │
│                                                                 │
│ ┌─────────────┐ ┌─────────────────────────────────────────────┐ │
│ │  篩選面板   │ │              模板卡片網格                    │ │
│ │             │ │                                             │ │
│ │ 技術棧篩選   │ │  ┌─────────┐ ┌─────────┐ ┌─────────┐       │ │
│ │ □ .NET Core │ │  │ DDD模板 │ │ CQRS模板│ │ Clean模板│       │ │
│ │ □ React     │ │  └─────────┘ └─────────┘ └─────────┘       │ │
│ │ □ Angular   │ │                                             │ │
│ │             │ │  ┌─────────┐ ┌─────────┐ ┌─────────┐       │ │
│ │ 架構類型     │ │  │微服務模板│ │MVC模板  │ │自訂模板 │       │ │
│ │ □ 分層架構   │ │  └─────────┘ └─────────┘ └─────────┘       │ │
│ │ □ 六角架構   │ │                                             │ │
│ │ □ 微服務     │ │              [載入更多模板]                  │ │
│ └─────────────┘ └─────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

#### 2.2.2 模板卡片組件

```html
<!-- 模板卡片 -->
<div class=\"bg-white rounded-lg border border-gray-200 hover:border-blue-300 hover:shadow-md transition-all duration-200 cursor-pointer\">
  <!-- 模板預覽圖 -->
  <div class=\"aspect-w-16 aspect-h-9 bg-gradient-to-br from-blue-50 to-indigo-100 rounded-t-lg p-6\">
    <div class=\"flex items-center justify-center\">
      <div class=\"text-center\">
        <div class=\"w-16 h-16 bg-blue-600 rounded-lg flex items-center justify-center mb-3\">
          <svg class=\"w-8 h-8 text-white\"><!-- DDD圖示 --></svg>
        </div>
        <h3 class=\"text-lg font-semibold text-gray-900\">DDD架構模板</h3>
      </div>
    </div>
  </div>

  <!-- 模板資訊 -->
  <div class=\"p-6\">
    <div class=\"mb-4\">
      <p class=\"text-sm text-gray-600 line-clamp-2\">
        基於領域驅動設計的四層架構模板，包含完整的聚合根、實體、值物件和領域服務實現。
      </p>
    </div>

    <!-- 技術標籤 -->
    <div class=\"flex flex-wrap gap-2 mb-4\">
      <span class=\"px-2 py-1 text-xs font-medium bg-blue-100 text-blue-800 rounded\">
        .NET Core 8
      </span>
      <span class=\"px-2 py-1 text-xs font-medium bg-green-100 text-green-800 rounded\">
        PostgreSQL
      </span>
      <span class=\"px-2 py-1 text-xs font-medium bg-purple-100 text-purple-800 rounded\">
        EF Core
      </span>
    </div>

    <!-- 統計資訊 -->
    <div class=\"flex items-center justify-between text-sm text-gray-600 mb-4\">
      <div class=\"flex items-center space-x-4\">
        <span class=\"flex items-center\">
          <svg class=\"w-4 h-4 mr-1\"><!-- 下載圖示 --></svg>
          1.2k 次使用
        </span>
        <span class=\"flex items-center\">
          <svg class=\"w-4 h-4 mr-1\"><!-- 星星圖示 --></svg>
          4.8 評分
        </span>
      </div>
      <span class=\"text-xs text-gray-500\">更新於 2天前</span>
    </div>

    <!-- 操作按鈕 -->
    <div class=\"flex space-x-3\">
      <button class=\"flex-1 bg-blue-600 text-white text-sm font-medium py-2 px-4 rounded-md hover:bg-blue-700 transition-colors\">
        使用模板
      </button>
      <button class=\"px-4 py-2 border border-gray-300 text-gray-700 text-sm font-medium rounded-md hover:bg-gray-50 transition-colors\">
        預覽
      </button>
    </div>
  </div>
</div>
```

#### 2.2.3 模板配置對話框

```html
<!-- 模板配置對話框 -->
<div class=\"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50\">
  <div class=\"bg-white rounded-lg shadow-xl max-w-2xl w-full mx-4 max-h-[90vh] overflow-hidden\">
    <!-- 對話框標頭 -->
    <div class=\"flex items-center justify-between p-6 border-b border-gray-200\">
      <h3 class=\"text-lg font-semibold text-gray-900\">配置 DDD架構模板</h3>
      <button class=\"text-gray-400 hover:text-gray-600\">
        <svg class=\"w-6 h-6\"><!-- 關閉圖示 --></svg>
      </button>
    </div>

    <!-- 對話框內容 -->
    <div class=\"p-6 overflow-y-auto\">
      <div class=\"space-y-6\">
        <!-- 基本設定 -->
        <div>
          <h4 class=\"text-md font-medium text-gray-900 mb-4\">基本設定</h4>
          <div class=\"grid grid-cols-1 md:grid-cols-2 gap-4\">
            <div>
              <label class=\"block text-sm font-medium text-gray-700 mb-2\">
                專案名稱
              </label>
              <input type=\"text\"
                     class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\"
                     placeholder=\"MyAwesomeProject\">
            </div>
            <div>
              <label class=\"block text-sm font-medium text-gray-700 mb-2\">
                命名空間
              </label>
              <input type=\"text\"
                     class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\"
                     placeholder=\"Company.Product\">
            </div>
          </div>
        </div>

        <!-- 技術選項 -->
        <div>
          <h4 class=\"text-md font-medium text-gray-900 mb-4\">技術選項</h4>
          <div class=\"space-y-3\">
            <label class=\"flex items-center\">
              <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\" checked>
              <span class=\"ml-2 text-sm text-gray-700\">包含身份驗證模組</span>
            </label>
            <label class=\"flex items-center\">
              <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\" checked>
              <span class=\"ml-2 text-sm text-gray-700\">包含審計日誌功能</span>
            </label>
            <label class=\"flex items-center\">
              <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
              <span class=\"ml-2 text-sm text-gray-700\">包含快取層 (Redis)</span>
            </label>
            <label class=\"flex items-center\">
              <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
              <span class=\"ml-2 text-sm text-gray-700\">包含訊息佇列 (RabbitMQ)</span>
            </label>
          </div>
        </div>

        <!-- 資料庫設定 -->
        <div>
          <h4 class=\"text-md font-medium text-gray-900 mb-4\">資料庫設定</h4>
          <div class=\"space-y-4\">
            <div>
              <label class=\"block text-sm font-medium text-gray-700 mb-2\">
                資料庫類型
              </label>
              <select class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\">
                <option>PostgreSQL</option>
                <option>SQL Server</option>
                <option>MySQL</option>
                <option>SQLite</option>
              </select>
            </div>
            <div>
              <label class=\"block text-sm font-medium text-gray-700 mb-2\">
                連接字串名稱
              </label>
              <input type=\"text\"
                     class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\"
                     value=\"DefaultConnection\">
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 對話框底部 -->
    <div class=\"flex items-center justify-between p-6 border-t border-gray-200 bg-gray-50\">
      <button class=\"px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors\">
        取消
      </button>
      <button class=\"px-6 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 transition-colors\">
        生成專案
      </button>
    </div>
  </div>
</div>
```

### 2.3 任務管理介面 (Task Management)

#### 2.3.1 任務列表視圖

```
┌─────────────────────────────────────────────────────────────────┐
│ ┌─────────────────────────────────────────────────────────────┐ │
│ │ 任務管理 [新增任務] [批次操作] [檢視切換: 表格|看板|時程]      │ │
│ └─────────────────────────────────────────────────────────────┘ │
│                                                                 │
│ ┌─────────────┐ ┌─────────────────────────────────────────────┐ │
│ │  篩選器     │ │              任務表格                        │ │
│ │             │ │                                             │ │
│ │ 狀態篩選     │ │ [✓] | 任務名稱 | 指派人 | 優先級 | 狀態 | 截止日 │ │
│ │ □ 待辦       │ │ [ ] | API重構  | 張三   | 高    |進行中| 12/31 │ │
│ │ □ 進行中     │ │ [ ] | UI設計   | 李四   | 中    | 待辦 | 01/15 │ │
│ │ □ 已完成     │ │ [ ] | 測試     | 王五   | 低    |審查中| 01/10 │ │
│ │             │ │                                             │ │
│ │ 指派人篩選   │ │              [載入更多任務]                  │ │
│ │ □ 張三       │ │                                             │ │
│ │ □ 李四       │ │                                             │ │
│ │ □ 王五       │ │                                             │ │
│ └─────────────┘ └─────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

#### 2.3.2 看板視圖

```html
<!-- 看板視圖 -->
<div class=\"flex space-x-6 overflow-x-auto pb-6\">
  <!-- 待辦欄 -->
  <div class=\"flex-shrink-0 w-80\">
    <div class=\"bg-gray-50 rounded-lg p-4\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"font-medium text-gray-900\">待辦事項</h3>
        <span class=\"px-2 py-1 text-xs font-medium bg-gray-200 text-gray-700 rounded-full\">
          8
        </span>
      </div>

      <!-- 任務卡片 -->
      <div class=\"space-y-3\">
        <div class=\"bg-white rounded-lg border border-gray-200 p-4 cursor-pointer hover:shadow-md transition-shadow\">
          <div class=\"flex items-start justify-between mb-2\">
            <h4 class=\"text-sm font-medium text-gray-900 line-clamp-2\">
              用戶註冊API端點實作
            </h4>
            <span class=\"px-2 py-1 text-xs font-medium bg-red-100 text-red-800 rounded\">
              高
            </span>
          </div>
          <p class=\"text-xs text-gray-600 mb-3 line-clamp-2\">
            實作用戶註冊相關的API端點，包含輸入驗證和錯誤處理機制
          </p>
          <div class=\"flex items-center justify-between\">
            <div class=\"flex items-center space-x-2\">
              <img class=\"w-6 h-6 rounded-full\" src=\"/api/avatar/zhang-san\" alt=\"張三\">
              <span class=\"text-xs text-gray-600\">張三</span>
            </div>
            <span class=\"text-xs text-gray-500\">12/31</span>
          </div>
        </div>

        <div class=\"bg-white rounded-lg border border-gray-200 p-4 cursor-pointer hover:shadow-md transition-shadow\">
          <div class=\"flex items-start justify-between mb-2\">
            <h4 class=\"text-sm font-medium text-gray-900 line-clamp-2\">
              前端登入頁面設計
            </h4>
            <span class=\"px-2 py-1 text-xs font-medium bg-yellow-100 text-yellow-800 rounded\">
              中
            </span>
          </div>
          <p class=\"text-xs text-gray-600 mb-3 line-clamp-2\">
            設計並實作用戶登入頁面，需符合設計系統規範
          </p>
          <div class=\"flex items-center justify-between\">
            <div class=\"flex items-center space-x-2\">
              <img class=\"w-6 h-6 rounded-full\" src=\"/api/avatar/li-si\" alt=\"李四\">
              <span class=\"text-xs text-gray-600\">李四</span>
            </div>
            <span class=\"text-xs text-gray-500\">01/15</span>
          </div>
        </div>
      </div>

      <!-- 新增任務按鈕 -->
      <button class=\"w-full mt-3 py-2 text-sm text-gray-600 bg-white border-2 border-dashed border-gray-300 rounded-lg hover:border-gray-400 hover:text-gray-700 transition-colors\">
        + 新增任務
      </button>
    </div>
  </div>

  <!-- 進行中欄 -->
  <div class=\"flex-shrink-0 w-80\">
    <div class=\"bg-blue-50 rounded-lg p-4\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"font-medium text-gray-900\">進行中</h3>
        <span class=\"px-2 py-1 text-xs font-medium bg-blue-200 text-blue-700 rounded-full\">
          5
        </span>
      </div>
      <!-- 任務卡片... -->
    </div>
  </div>

  <!-- 審查中欄 -->
  <div class=\"flex-shrink-0 w-80\">
    <div class=\"bg-orange-50 rounded-lg p-4\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"font-medium text-gray-900\">審查中</h3>
        <span class=\"px-2 py-1 text-xs font-medium bg-orange-200 text-orange-700 rounded-full\">
          3
        </span>
      </div>
      <!-- 任務卡片... -->
    </div>
  </div>

  <!-- 已完成欄 -->
  <div class=\"flex-shrink-0 w-80\">
    <div class=\"bg-green-50 rounded-lg p-4\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"font-medium text-gray-900\">已完成</h3>
        <span class=\"px-2 py-1 text-xs font-medium bg-green-200 text-green-700 rounded-full\">
          12
        </span>
      </div>
      <!-- 任務卡片... -->
    </div>
  </div>
</div>
```

#### 2.3.3 任務詳情側邊面板

```html
<!-- 任務詳情側邊面板 -->
<div class=\"fixed inset-y-0 right-0 w-96 bg-white shadow-xl border-l border-gray-200 transform transition-transform duration-300 ease-in-out z-50\">
  <!-- 面板標頭 -->
  <div class=\"flex items-center justify-between p-6 border-b border-gray-200\">
    <h3 class=\"text-lg font-semibold text-gray-900\">任務詳情</h3>
    <button class=\"text-gray-400 hover:text-gray-600\">
      <svg class=\"w-6 h-6\"><!-- 關閉圖示 --></svg>
    </button>
  </div>

  <!-- 面板內容 -->
  <div class=\"flex-1 overflow-y-auto p-6\">
    <!-- 任務標題和狀態 -->
    <div class=\"mb-6\">
      <div class=\"flex items-start justify-between mb-2\">
        <h4 class=\"text-xl font-medium text-gray-900 flex-1 mr-4\">
          用戶註冊API端點實作
        </h4>
        <span class=\"px-3 py-1 text-sm font-medium bg-blue-100 text-blue-800 rounded-full\">
          進行中
        </span>
      </div>
      <p class=\"text-sm text-gray-600\">
        TASK-2024-0156
      </p>
    </div>

    <!-- 任務資訊 -->
    <div class=\"space-y-4 mb-6\">
      <div class=\"flex items-center justify-between\">
        <span class=\"text-sm font-medium text-gray-700\">指派人</span>
        <div class=\"flex items-center space-x-2\">
          <img class=\"w-6 h-6 rounded-full\" src=\"/api/avatar/zhang-san\" alt=\"張三\">
          <span class=\"text-sm text-gray-900\">張三</span>
        </div>
      </div>

      <div class=\"flex items-center justify-between\">
        <span class=\"text-sm font-medium text-gray-700\">優先級</span>
        <span class=\"px-2 py-1 text-xs font-medium bg-red-100 text-red-800 rounded\">
          高
        </span>
      </div>

      <div class=\"flex items-center justify-between\">
        <span class=\"text-sm font-medium text-gray-700\">截止日期</span>
        <span class=\"text-sm text-gray-900\">2024-12-31</span>
      </div>

      <div class=\"flex items-center justify-between\">
        <span class=\"text-sm font-medium text-gray-700\">預估工時</span>
        <span class=\"text-sm text-gray-900\">16 小時</span>
      </div>

      <div class=\"flex items-center justify-between\">
        <span class=\"text-sm font-medium text-gray-700\">進度</span>
        <div class=\"flex items-center space-x-2 flex-1 ml-4\">
          <div class=\"flex-1 bg-gray-200 rounded-full h-2\">
            <div class=\"bg-blue-600 h-2 rounded-full\" style=\"width: 65%\"></div>
          </div>
          <span class=\"text-sm text-gray-600\">65%</span>
        </div>
      </div>
    </div>

    <!-- 任務描述 -->
    <div class=\"mb-6\">
      <h5 class=\"text-sm font-medium text-gray-700 mb-2\">任務描述</h5>
      <div class=\"text-sm text-gray-600 bg-gray-50 rounded-lg p-3\">
        實作用戶註冊相關的API端點，需要包含：
        <ul class=\"list-disc list-inside mt-2 space-y-1\">
          <li>輸入參數驗證</li>
          <li>電子信箱格式檢查</li>
          <li>密碼強度驗證</li>
          <li>重複帳號檢查</li>
          <li>錯誤處理機制</li>
        </ul>
      </div>
    </div>

    <!-- 驗收標準 -->
    <div class=\"mb-6\">
      <h5 class=\"text-sm font-medium text-gray-700 mb-2\">驗收標準</h5>
      <div class=\"space-y-2\">
        <label class=\"flex items-start space-x-2\">
          <input type=\"checkbox\" class=\"mt-1 rounded border-gray-300 text-blue-600 focus:ring-blue-500\" checked>
          <span class=\"text-sm text-gray-600\">API端點正確回應POST請求</span>
        </label>
        <label class=\"flex items-start space-x-2\">
          <input type=\"checkbox\" class=\"mt-1 rounded border-gray-300 text-blue-600 focus:ring-blue-500\" checked>
          <span class=\"text-sm text-gray-600\">輸入驗證正確運作</span>
        </label>
        <label class=\"flex items-start space-x-2\">
          <input type=\"checkbox\" class=\"mt-1 rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
          <span class=\"text-sm text-gray-600\">單元測試覆蓋率達80%</span>
        </label>
        <label class=\"flex items-start space-x-2\">
          <input type=\"checkbox\" class=\"mt-1 rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
          <span class=\"text-sm text-gray-600\">API文件更新完成</span>
        </label>
      </div>
    </div>

    <!-- 活動歷史 -->
    <div class=\"mb-6\">
      <h5 class=\"text-sm font-medium text-gray-700 mb-3\">活動歷史</h5>
      <div class=\"space-y-3\">
        <div class=\"flex space-x-3\">
          <div class=\"flex-shrink-0 w-2 h-2 bg-blue-600 rounded-full mt-2\"></div>
          <div class=\"flex-1\">
            <p class=\"text-sm text-gray-900\">張三 更新了任務進度至 65%</p>
            <p class=\"text-xs text-gray-500\">2 小時前</p>
          </div>
        </div>
        <div class=\"flex space-x-3\">
          <div class=\"flex-shrink-0 w-2 h-2 bg-green-600 rounded-full mt-2\"></div>
          <div class=\"flex-1\">
            <p class=\"text-sm text-gray-900\">張三 完成了驗收項目：輸入驗證正確運作</p>
            <p class=\"text-xs text-gray-500\">4 小時前</p>
          </div>
        </div>
        <div class=\"flex space-x-3\">
          <div class=\"flex-shrink-0 w-2 h-2 bg-yellow-600 rounded-full mt-2\"></div>
          <div class=\"flex-1\">
            <p class=\"text-sm text-gray-900\">李經理 將任務指派給 張三</p>
            <p class=\"text-xs text-gray-500\">昨天</p>
          </div>
        </div>
      </div>
    </div>

    <!-- 評論區 -->
    <div>
      <h5 class=\"text-sm font-medium text-gray-700 mb-3\">評論</h5>
      <div class=\"space-y-3 mb-4\">
        <div class=\"flex space-x-3\">
          <img class=\"w-8 h-8 rounded-full\" src=\"/api/avatar/zhang-san\" alt=\"張三\">
          <div class=\"flex-1\">
            <div class=\"bg-gray-50 rounded-lg p-3\">
              <p class=\"text-sm text-gray-900 font-medium mb-1\">張三</p>
              <p class=\"text-sm text-gray-700\">
                API基本結構已完成，正在進行輸入驗證的實作。預計明天可以完成第一版。
              </p>
            </div>
            <p class=\"text-xs text-gray-500 mt-1\">2 小時前</p>
          </div>
        </div>
      </div>

      <!-- 新增評論 -->
      <div class=\"flex space-x-3\">
        <img class=\"w-8 h-8 rounded-full\" src=\"/api/avatar/current-user\" alt=\"目前使用者\">
        <div class=\"flex-1\">
          <textarea class=\"w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none\"
                    rows=\"3\"
                    placeholder=\"新增評論...\"></textarea>
          <div class=\"flex justify-end mt-2\">
            <button class=\"px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 transition-colors\">
              發布評論
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### 2.4 AI程式碼品質中心 (AI Code Quality Center)

#### 2.4.1 品質儀表板

```html
<!-- AI品質儀表板 -->
<div class=\"space-y-6\">
  <!-- 品質概覽卡片 -->
  <div class=\"grid grid-cols-1 md:grid-cols-3 gap-6\">
    <!-- 整體品質評分 -->
    <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"text-lg font-semibold text-gray-900\">整體品質評分</h3>
        <div class=\"w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-6 h-6 text-green-600\"><!-- 品質圖示 --></svg>
        </div>
      </div>

      <!-- 圓形進度條 -->
      <div class=\"flex items-center justify-center mb-4\">
        <div class=\"relative w-24 h-24\">
          <svg class=\"w-24 h-24 transform -rotate-90\" viewBox=\"0 0 100 100\">
            <circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"#E5E7EB\" stroke-width=\"8\" fill=\"none\"/>
            <circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"#10B981\" stroke-width=\"8\" fill=\"none\"
                    stroke-dasharray=\"251.2\" stroke-dashoffset=\"62.8\" stroke-linecap=\"round\"/>
          </svg>
          <div class=\"absolute inset-0 flex items-center justify-center\">
            <span class=\"text-2xl font-bold text-gray-900\">8.5</span>
          </div>
        </div>
      </div>

      <div class=\"text-center\">
        <p class=\"text-sm text-gray-600 mb-2\">較上週提升</p>
        <div class=\"flex items-center justify-center text-green-600\">
          <svg class=\"w-4 h-4 mr-1\"><!-- 向上箭頭 --></svg>
          <span class=\"text-sm font-medium\">+0.3</span>
        </div>
      </div>
    </div>

    <!-- 程式碼覆蓋率 -->
    <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"text-lg font-semibold text-gray-900\">測試覆蓋率</h3>
        <div class=\"w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-6 h-6 text-blue-600\"><!-- 測試圖示 --></svg>
        </div>
      </div>

      <div class=\"mb-4\">
        <div class=\"flex items-center justify-between mb-2\">
          <span class=\"text-3xl font-bold text-gray-900\">87%</span>
          <span class=\"px-2 py-1 text-xs font-medium bg-blue-100 text-blue-800 rounded\">
            良好
          </span>
        </div>
        <div class=\"w-full bg-gray-200 rounded-full h-2\">
          <div class=\"bg-blue-600 h-2 rounded-full\" style=\"width: 87%\"></div>
        </div>
      </div>

      <div class=\"text-sm text-gray-600\">
        <p>目標: 80% | 當前: 87%</p>
      </div>
    </div>

    <!-- 技術債務 -->
    <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
      <div class=\"flex items-center justify-between mb-4\">
        <h3 class=\"text-lg font-semibold text-gray-900\">技術債務</h3>
        <div class=\"w-12 h-12 bg-orange-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-6 h-6 text-orange-600\"><!-- 債務圖示 --></svg>
        </div>
      </div>

      <div class=\"mb-4\">
        <span class=\"text-3xl font-bold text-gray-900\">2.3</span>
        <span class=\"text-lg text-gray-600 ml-1\">天</span>
      </div>

      <div class=\"space-y-2 text-sm\">
        <div class=\"flex justify-between\">
          <span class=\"text-gray-600\">程式碼重複</span>
          <span class=\"text-gray-900\">0.8天</span>
        </div>
        <div class=\"flex justify-between\">
          <span class=\"text-gray-600\">複雜度過高</span>
          <span class=\"text-gray-900\">1.2天</span>
        </div>
        <div class=\"flex justify-between\">
          <span class=\"text-gray-600\">安全性問題</span>
          <span class=\"text-gray-900\">0.3天</span>
        </div>
      </div>
    </div>
  </div>

  <!-- 品質趨勢圖表 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <div class=\"flex items-center justify-between mb-6\">
      <h3 class=\"text-lg font-semibold text-gray-900\">品質趨勢</h3>
      <div class=\"flex space-x-2\">
        <button class=\"px-3 py-1 text-sm font-medium text-blue-600 bg-blue-50 rounded-md\">
          本週
        </button>
        <button class=\"px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md\">
          本月
        </button>
        <button class=\"px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md\">
          本季
        </button>
      </div>
    </div>

    <!-- 圖表佔位符 -->
    <div class=\"h-64 bg-gradient-to-r from-blue-50 to-green-50 rounded-lg flex items-center justify-center\">
      <p class=\"text-gray-500\">品質趨勢圖表 (Chart.js 或 D3.js 實作)</p>
    </div>
  </div>

  <!-- 最近檢查結果 -->
  <div class=\"bg-white rounded-lg shadow-sm border border-gray-200 p-6\">
    <h3 class=\"text-lg font-semibold text-gray-900 mb-6\">最近檢查結果</h3>

    <div class=\"space-y-4\">
      <!-- 檢查結果項目 -->
      <div class=\"flex items-start space-x-4 p-4 border border-gray-200 rounded-lg\">
        <div class=\"flex-shrink-0 w-10 h-10 bg-green-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-5 h-5 text-green-600\"><!-- 成功圖示 --></svg>
        </div>
        <div class=\"flex-1 min-w-0\">
          <div class=\"flex items-center justify-between mb-2\">
            <h4 class=\"text-sm font-medium text-gray-900\">用戶認證模組</h4>
            <span class=\"text-xs text-gray-500\">2 小時前</span>
          </div>
          <p class=\"text-sm text-gray-600 mb-2\">
            檢查通過 - 無發現重大問題，程式碼品質良好
          </p>
          <div class=\"flex items-center space-x-4 text-xs text-gray-500\">
            <span>評分: 9.2/10</span>
            <span>檔案: 12</span>
            <span>問題: 0</span>
          </div>
        </div>
      </div>

      <div class=\"flex items-start space-x-4 p-4 border border-gray-200 rounded-lg\">
        <div class=\"flex-shrink-0 w-10 h-10 bg-yellow-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-5 h-5 text-yellow-600\"><!-- 警告圖示 --></svg>
        </div>
        <div class=\"flex-1 min-w-0\">
          <div class=\"flex items-center justify-between mb-2\">
            <h4 class=\"text-sm font-medium text-gray-900\">資料存取層</h4>
            <span class=\"text-xs text-gray-500\">4 小時前</span>
          </div>
          <p class=\"text-sm text-gray-600 mb-2\">
            發現 3 個輕微問題，建議進行程式碼重構
          </p>
          <div class=\"flex items-center space-x-4 text-xs text-gray-500 mb-2\">
            <span>評分: 7.8/10</span>
            <span>檔案: 8</span>
            <span>問題: 3</span>
          </div>
          <div class=\"space-y-1\">
            <div class=\"text-xs text-yellow-700 bg-yellow-50 px-2 py-1 rounded\">
              ⚠️ Repository.cs:45 - 複雜度過高，建議拆分方法
            </div>
            <div class=\"text-xs text-yellow-700 bg-yellow-50 px-2 py-1 rounded\">
              ⚠️ DataContext.cs:23 - 硬編碼連接字串
            </div>
          </div>
        </div>
      </div>

      <div class=\"flex items-start space-x-4 p-4 border border-gray-200 rounded-lg\">
        <div class=\"flex-shrink-0 w-10 h-10 bg-red-100 rounded-lg flex items-center justify-center\">
          <svg class=\"w-5 h-5 text-red-600\"><!-- 錯誤圖示 --></svg>
        </div>
        <div class=\"flex-1 min-w-0\">
          <div class=\"flex items-center justify-between mb-2\">
            <h4 class=\"text-sm font-medium text-gray-900\">支付模組</h4>
            <span class=\"text-xs text-gray-500\">6 小時前</span>
          </div>
          <p class=\"text-sm text-gray-600 mb-2\">
            發現 1 個嚴重安全性問題，需立即修復
          </p>
          <div class=\"flex items-center space-x-4 text-xs text-gray-500 mb-2\">
            <span>評分: 5.2/10</span>
            <span>檔案: 5</span>
            <span>問題: 1</span>
          </div>
          <div class=\"text-xs text-red-700 bg-red-50 px-2 py-1 rounded\">
            🚨 PaymentService.cs:67 - 敏感資訊未加密存儲
          </div>
        </div>
      </div>
    </div>

    <div class=\"mt-6 text-center\">
      <button class=\"px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors\">
        檢視所有結果
      </button>
    </div>
  </div>
</div>
```

### 2.5 工作流程設計器 (Workflow Designer)

#### 2.5.1 流程設計介面

```html
<!-- 工作流程設計器 -->
<div class=\"h-screen flex flex-col bg-gray-50\">
  <!-- 工具列 -->
  <div class=\"bg-white border-b border-gray-200 p-4\">
    <div class=\"flex items-center justify-between\">
      <div class=\"flex items-center space-x-4\">
        <h2 class=\"text-lg font-semibold text-gray-900\">工作流程設計器</h2>
        <div class=\"flex items-center space-x-2\">
          <button class=\"px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors\">
            <svg class=\"w-4 h-4 mr-2\"><!-- 儲存圖示 --></svg>
            儲存
          </button>
          <button class=\"px-3 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 transition-colors\">
            <svg class=\"w-4 h-4 mr-2\"><!-- 發布圖示 --></svg>
            發布流程
          </button>
        </div>
      </div>

      <div class=\"flex items-center space-x-2\">
        <button class=\"px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors\">
          預覽
        </button>
        <button class=\"px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors\">
          測試
        </button>
      </div>
    </div>
  </div>

  <div class=\"flex-1 flex\">
    <!-- 組件面板 -->
    <div class=\"w-64 bg-white border-r border-gray-200 p-4\">
      <h3 class=\"text-sm font-semibold text-gray-900 mb-4\">流程組件</h3>

      <div class=\"space-y-3\">
        <!-- 開始節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-green-100 rounded-full flex items-center justify-center mr-3\">
            <svg class=\"w-4 h-4 text-green-600\"><!-- 開始圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">開始</span>
        </div>

        <!-- 任務節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-blue-100 rounded-lg flex items-center justify-center mr-3\">
            <svg class=\"w-4 h-4 text-blue-600\"><!-- 任務圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">任務</span>
        </div>

        <!-- 決策節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-yellow-100 rounded-lg flex items-center justify-center mr-3 transform rotate-45\">
            <svg class=\"w-4 h-4 text-yellow-600 transform -rotate-45\"><!-- 決策圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">決策</span>
        </div>

        <!-- 並行節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-purple-100 rounded-lg flex items-center justify-center mr-3\">
            <svg class=\"w-4 h-4 text-purple-600\"><!-- 並行圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">並行</span>
        </div>

        <!-- 審核節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-orange-100 rounded-lg flex items-center justify-center mr-3\">
            <svg class=\"w-4 h-4 text-orange-600\"><!-- 審核圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">審核</span>
        </div>

        <!-- 結束節點 -->
        <div class=\"flex items-center p-3 border border-gray-200 rounded-lg cursor-pointer hover:bg-gray-50\" draggable=\"true\">
          <div class=\"w-8 h-8 bg-red-100 rounded-full flex items-center justify-center mr-3\">
            <svg class=\"w-4 h-4 text-red-600\"><!-- 結束圖示 --></svg>
          </div>
          <span class=\"text-sm font-medium text-gray-900\">結束</span>
        </div>
      </div>
    </div>

    <!-- 設計畫布 -->
    <div class=\"flex-1 relative overflow-hidden\">
      <!-- 網格背景 -->
      <div class=\"absolute inset-0 opacity-20\">
        <svg width=\"100%\" height=\"100%\" xmlns=\"http://www.w3.org/2000/svg\">
          <defs>
            <pattern id=\"grid\" width=\"20\" height=\"20\" patternUnits=\"userSpaceOnUse\">
              <path d=\"M 20 0 L 0 0 0 20\" fill=\"none\" stroke=\"#E5E7EB\" stroke-width=\"1\"/>
            </pattern>
          </defs>
          <rect width=\"100%\" height=\"100%\" fill=\"url(#grid)\"/>
        </svg>
      </div>

      <!-- 流程節點 -->
      <div class=\"relative z-10 p-8\">
        <!-- 開始節點 -->
        <div class=\"absolute top-20 left-20 flex flex-col items-center cursor-pointer group\">
          <div class=\"w-12 h-12 bg-green-500 rounded-full flex items-center justify-center shadow-lg group-hover:shadow-xl transition-shadow\">
            <svg class=\"w-6 h-6 text-white\"><!-- 開始圖示 --></svg>
          </div>
          <span class=\"mt-2 text-xs font-medium text-gray-700\">開始</span>
        </div>

        <!-- 任務節點 -->
        <div class=\"absolute top-20 left-60 flex flex-col items-center cursor-pointer group\">
          <div class=\"w-16 h-12 bg-blue-500 rounded-lg flex items-center justify-center shadow-lg group-hover:shadow-xl transition-shadow\">
            <svg class=\"w-6 h-6 text-white\"><!-- 任務圖示 --></svg>
          </div>
          <span class=\"mt-2 text-xs font-medium text-gray-700\">需求分析</span>
        </div>

        <!-- 決策節點 -->
        <div class=\"absolute top-20 left-96 flex flex-col items-center cursor-pointer group\">
          <div class=\"w-12 h-12 bg-yellow-500 rounded-lg transform rotate-45 flex items-center justify-center shadow-lg group-hover:shadow-xl transition-shadow\">
            <svg class=\"w-6 h-6 text-white transform -rotate-45\"><!-- 決策圖示 --></svg>
          </div>
          <span class=\"mt-4 text-xs font-medium text-gray-700\">需求審核</span>
        </div>

        <!-- 連接線 -->
        <svg class=\"absolute top-0 left-0 w-full h-full pointer-events-none\">
          <!-- 開始到任務 -->
          <line x1=\"76\" y1=\"46\" x2=\"124\" y2=\"46\" stroke=\"#6B7280\" stroke-width=\"2\" marker-end=\"url(#arrowhead)\"/>
          <!-- 任務到決策 -->
          <line x1=\"156\" y1=\"46\" x2=\"204\" y2=\"46\" stroke=\"#6B7280\" stroke-width=\"2\" marker-end=\"url(#arrowhead)\"/>

          <defs>
            <marker id=\"arrowhead\" markerWidth=\"10\" markerHeight=\"7\" refX=\"9\" refY=\"3.5\" orient=\"auto\">
              <polygon points=\"0 0, 10 3.5, 0 7\" fill=\"#6B7280\"/>
            </marker>
          </defs>
        </svg>
      </div>
    </div>

    <!-- 屬性面板 -->
    <div class=\"w-80 bg-white border-l border-gray-200 p-4\">
      <h3 class=\"text-sm font-semibold text-gray-900 mb-4\">節點屬性</h3>

      <!-- 當前選中節點的屬性 -->
      <div class=\"space-y-4\">
        <div>
          <label class=\"block text-sm font-medium text-gray-700 mb-2\">
            節點名稱
          </label>
          <input type=\"text\"
                 class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\"
                 value=\"需求分析\">
        </div>

        <div>
          <label class=\"block text-sm font-medium text-gray-700 mb-2\">
            負責角色
          </label>
          <select class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\">
            <option>系統分析師</option>
            <option>專案經理</option>
            <option>技術主管</option>
            <option>開發人員</option>
          </select>
        </div>

        <div>
          <label class=\"block text-sm font-medium text-gray-700 mb-2\">
            預估時間 (小時)
          </label>
          <input type=\"number\"
                 class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent\"
                 value=\"8\">
        </div>

        <div>
          <label class=\"block text-sm font-medium text-gray-700 mb-2\">
            說明
          </label>
          <textarea class=\"w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none\"
                    rows=\"3\"
                    placeholder=\"節點說明...\"></textarea>
        </div>

        <div>
          <label class=\"flex items-center\">
            <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
            <span class=\"ml-2 text-sm text-gray-700\">必要審核點</span>
          </label>
        </div>

        <div>
          <label class=\"flex items-center\">
            <input type=\"checkbox\" class=\"rounded border-gray-300 text-blue-600 focus:ring-blue-500\">
            <span class=\"ml-2 text-sm text-gray-700\">自動執行</span>
          </label>
        </div>
      </div>
    </div>
  </div>
</div>
```

---

## 3. 行動端原型設計

### 3.1 行動端導航設計

```html
<!-- 行動端底部導航 -->
<div class=\"fixed bottom-0 left-0 right-0 bg-white border-t border-gray-200 px-4 py-2 safe-area-inset-bottom\">
  <div class=\"flex justify-around\">
    <button class=\"flex flex-col items-center justify-center py-2 px-3 rounded-lg text-blue-600 bg-blue-50\">
      <svg class=\"w-6 h-6 mb-1\"><!-- 首頁圖示 --></svg>
      <span class=\"text-xs font-medium\">首頁</span>
    </button>

    <button class=\"flex flex-col items-center justify-center py-2 px-3 rounded-lg text-gray-600\">
      <svg class=\"w-6 h-6 mb-1\"><!-- 任務圖示 --></svg>
      <span class=\"text-xs font-medium\">任務</span>
    </button>

    <button class=\"flex flex-col items-center justify-center py-2 px-3 rounded-lg text-gray-600\">
      <svg class=\"w-6 h-6 mb-1\"><!-- 專案圖示 --></svg>
      <span class=\"text-xs font-medium\">專案</span>
    </button>

    <button class=\"flex flex-col items-center justify-center py-2 px-3 rounded-lg text-gray-600\">
      <svg class=\"w-6 h-6 mb-1\"><!-- 通知圖示 --></svg>
      <span class=\"text-xs font-medium\">通知</span>
      <div class=\"absolute -top-1 -right-1 w-5 h-5 bg-red-500 rounded-full flex items-center justify-center\">
        <span class=\"text-xs font-medium text-white\">3</span>
      </div>
    </button>

    <button class=\"flex flex-col items-center justify-center py-2 px-3 rounded-lg text-gray-600\">
      <svg class=\"w-6 h-6 mb-1\"><!-- 個人圖示 --></svg>
      <span class=\"text-xs font-medium\">我的</span>
    </button>
  </div>
</div>
```

### 3.2 行動端任務卡片

```html
<!-- 行動端任務卡片 -->
<div class=\"bg-white rounded-lg border border-gray-200 p-4 mb-3 shadow-sm\">
  <div class=\"flex items-start justify-between mb-3\">
    <h3 class=\"text-base font-medium text-gray-900 flex-1 mr-2 line-clamp-2\">
      用戶註冊API端點實作
    </h3>
    <span class=\"px-2 py-1 text-xs font-medium bg-red-100 text-red-800 rounded flex-shrink-0\">
      高
    </span>
  </div>

  <p class=\"text-sm text-gray-600 mb-3 line-clamp-2\">
    實作用戶註冊相關的API端點，包含輸入驗證和錯誤處理機制
  </p>

  <div class=\"flex items-center justify-between mb-3\">
    <div class=\"flex items-center space-x-2\">
      <img class=\"w-6 h-6 rounded-full\" src=\"/api/avatar/zhang-san\" alt=\"張三\">
      <span class=\"text-sm text-gray-700\">張三</span>
    </div>
    <span class=\"text-sm text-gray-500\">12/31</span>
  </div>

  <div class=\"mb-3\">
    <div class=\"flex items-center justify-between mb-1\">
      <span class=\"text-xs text-gray-600\">進度</span>
      <span class=\"text-xs text-gray-700\">65%</span>
    </div>
    <div class=\"w-full bg-gray-200 rounded-full h-1.5\">
      <div class=\"bg-blue-600 h-1.5 rounded-full\" style=\"width: 65%\"></div>
    </div>
  </div>

  <div class=\"flex items-center justify-between\">
    <span class=\"px-2 py-1 text-xs font-medium bg-blue-100 text-blue-800 rounded\">
      進行中
    </span>
    <button class=\"p-2 text-gray-400 hover:text-gray-600\">
      <svg class=\"w-4 h-4\"><!-- 更多選項圖示 --></svg>
    </button>
  </div>
</div>
```

### 3.3 行動端模態對話框

```html
<!-- 行動端全屏模態對話框 -->
<div class=\"fixed inset-0 bg-white z-50 safe-area-inset\">
  <!-- 標頭 -->
  <div class=\"flex items-center justify-between p-4 border-b border-gray-200\">
    <button class=\"p-2 text-gray-600\">
      <svg class=\"w-6 h-6\"><!-- 返回箭頭 --></svg>
    </button>
    <h2 class=\"text-lg font-semibold text-gray-900\">任務詳情</h2>
    <button class=\"p-2 text-gray-600\">
      <svg class=\"w-6 h-6\"><!-- 更多選項 --></svg>
    </button>
  </div>

  <!-- 內容區域 -->
  <div class=\"flex-1 overflow-y-auto p-4\">
    <!-- 任務內容 -->
  </div>

  <!-- 底部操作區 -->
  <div class=\"p-4 border-t border-gray-200 safe-area-inset-bottom\">
    <div class=\"flex space-x-3\">
      <button class=\"flex-1 py-3 px-4 text-sm font-medium text-gray-700 bg-gray-100 rounded-lg\">
        編輯
      </button>
      <button class=\"flex-1 py-3 px-4 text-sm font-medium text-white bg-blue-600 rounded-lg\">
        更新進度
      </button>
    </div>
  </div>
</div>
```

---

## 4. 互動設計規範

### 4.1 按鈕互動狀態

```css
/* 主要按鈕樣式 */
.btn-primary {
  @apply px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-md;
  transition: all 0.2s ease-in-out;
}

.btn-primary:hover {
  @apply bg-blue-700 shadow-md;
  transform: translateY(-1px);
}

.btn-primary:active {
  @apply bg-blue-800;
  transform: translateY(0);
}

.btn-primary:focus {
  @apply ring-2 ring-blue-500 ring-offset-2;
  outline: none;
}

.btn-primary:disabled {
  @apply bg-gray-400 cursor-not-allowed;
  transform: none;
}
```

### 4.2 表單互動設計

```css
/* 輸入框樣式 */
.form-input {
  @apply w-full px-3 py-2 border border-gray-300 rounded-md;
  transition: all 0.2s ease-in-out;
}

.form-input:focus {
  @apply ring-2 ring-blue-500 ring-offset-0 border-blue-500;
  outline: none;
}

.form-input:invalid {
  @apply border-red-500 ring-2 ring-red-500 ring-offset-0;
}

.form-input.success {
  @apply border-green-500 ring-2 ring-green-500 ring-offset-0;
}
```

### 4.3 載入狀態設計

```html
<!-- 按鈕載入狀態 -->
<button class=\"btn-primary flex items-center\" disabled>
  <svg class=\"animate-spin -ml-1 mr-3 h-4 w-4 text-white\" viewBox=\"0 0 24 24\">
    <circle class=\"opacity-25\" cx=\"12\" cy=\"12\" r=\"10\" stroke=\"currentColor\" stroke-width=\"4\" fill=\"none\"></circle>
    <path class=\"opacity-75\" fill=\"currentColor\" d=\"M4 12a8 8 0 0 1 8-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 0 1 4 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z\"></path>
  </svg>
  處理中...
</button>

<!-- 頁面載入骨架 -->
<div class=\"animate-pulse\">
  <div class=\"h-4 bg-gray-300 rounded w-3/4 mb-4\"></div>
  <div class=\"h-4 bg-gray-300 rounded w-1/2 mb-4\"></div>
  <div class=\"h-4 bg-gray-300 rounded w-2/3\"></div>
</div>
```

### 4.4 通知與回饋設計

```html
<!-- 成功通知 -->
<div class=\"fixed top-4 right-4 bg-green-500 text-white px-6 py-4 rounded-lg shadow-lg z-50 animate-slide-in-right\">
  <div class=\"flex items-center\">
    <svg class=\"w-5 h-5 mr-3\"><!-- 成功圖示 --></svg>
    <span class=\"font-medium\">操作成功完成！</span>
  </div>
</div>

<!-- 錯誤通知 -->
<div class=\"fixed top-4 right-4 bg-red-500 text-white px-6 py-4 rounded-lg shadow-lg z-50 animate-slide-in-right\">
  <div class=\"flex items-center\">
    <svg class=\"w-5 h-5 mr-3\"><!-- 錯誤圖示 --></svg>
    <span class=\"font-medium\">操作失敗，請稍後再試</span>
  </div>
</div>
```

---

## 5. 可用性與無障礙設計

### 5.1 鍵盤導航支援

```html
<!-- 跳過連結 -->
<a href=\"#main-content\" class=\"sr-only focus:not-sr-only focus:absolute focus:top-4 focus:left-4 bg-blue-600 text-white px-4 py-2 rounded-md z-50\">
  跳到主要內容
</a>

<!-- 焦點指示器 -->
<style>
.focus-visible {
  @apply ring-2 ring-blue-500 ring-offset-2;
}

/* 自訂焦點樣式 */
.btn:focus-visible {
  @apply ring-2 ring-blue-500 ring-offset-2;
  outline: none;
}
</style>
```

### 5.2 ARIA 標籤應用

```html
<!-- 語義化導航 -->
<nav aria-label=\"主要導航\" role=\"navigation\">
  <ul class=\"flex space-x-4\">
    <li>
      <a href=\"/dashboard\" aria-current=\"page\" class=\"text-blue-600\">
        儀表板
      </a>
    </li>
    <li>
      <a href=\"/projects\" class=\"text-gray-600\">
        專案
      </a>
    </li>
  </ul>
</nav>

<!-- 表單標籤 -->
<div class=\"form-group\">
  <label for=\"project-name\" class=\"form-label\">
    專案名稱 <span aria-label=\"必填欄位\" class=\"text-red-500\">*</span>
  </label>
  <input
    type=\"text\"
    id=\"project-name\"
    name=\"projectName\"
    class=\"form-input\"
    aria-required=\"true\"
    aria-describedby=\"project-name-help project-name-error\"
  >
  <div id=\"project-name-help\" class=\"text-sm text-gray-600 mt-1\">
    請輸入專案的完整名稱
  </div>
  <div id=\"project-name-error\" class=\"text-sm text-red-600 mt-1\" role=\"alert\" aria-live=\"polite\">
    <!-- 錯誤訊息 -->
  </div>
</div>
```

### 5.3 色彩對比度設計

```css
/* 高對比度模式 */
@media (prefers-contrast: high) {
  .btn-primary {
    @apply bg-blue-800 border-2 border-blue-900;
  }

  .text-gray-600 {
    @apply text-gray-900;
  }

  .border-gray-200 {
    @apply border-gray-400;
  }
}

/* 深色模式支援 */
@media (prefers-color-scheme: dark) {
  .bg-white {
    @apply bg-gray-900;
  }

  .text-gray-900 {
    @apply text-gray-100;
  }

  .border-gray-200 {
    @apply border-gray-700;
  }
}
```

---

## 6. 效能優化設計

### 6.1 圖片優化

```html
<!-- 響應式圖片 -->
<picture>
  <source media=\"(min-width: 768px)\" srcset=\"/images/dashboard-large.webp\" type=\"image/webp\">
  <source media=\"(min-width: 768px)\" srcset=\"/images/dashboard-large.jpg\" type=\"image/jpeg\">
  <source srcset=\"/images/dashboard-small.webp\" type=\"image/webp\">
  <img src=\"/images/dashboard-small.jpg\" alt=\"專案儀表板\" class=\"w-full h-auto\" loading=\"lazy\">
</picture>

<!-- 頭像懶載入 -->
<img
  class=\"w-8 h-8 rounded-full\"
  src=\"/images/placeholder-avatar.jpg\"
  data-src=\"/api/avatar/user-123\"
  alt=\"使用者頭像\"
  loading=\"lazy\"
  onload=\"this.classList.add('loaded')\"
>
```

### 6.2 程式碼分割

```javascript
// 路由層級的程式碼分割
const Dashboard = lazy(() => import('./pages/Dashboard'));
const Projects = lazy(() => import('./pages/Projects'));
const Tasks = lazy(() => import('./pages/Tasks'));

// 組件層級的程式碼分割
const Chart = lazy(() => import('./components/Chart'));
const DataTable = lazy(() => import('./components/DataTable'));
```

### 6.3 虛擬滾動

```html
<!-- 大列表虛擬滾動 -->
<div class=\"virtual-list-container h-96 overflow-auto\">
  <div class=\"virtual-list-spacer\" style=\"height: 0px;\"></div>
  <div class=\"virtual-list-items\">
    <!-- 只渲染可見範圍內的項目 -->
  </div>
  <div class=\"virtual-list-spacer\" style=\"height: 0px;\"></div>
</div>
```

---

## 7. 測試與驗證

### 7.1 可用性測試檢查清單

**導航測試**：
- [ ] 使用者能在3次點擊內找到任何功能
- [ ] 麵包屑導航正確顯示當前位置
- [ ] 返回按鈕功能正常
- [ ] 搜尋功能能快速定位內容

**表單測試**：
- [ ] 所有必填欄位都有明確標示
- [ ] 錯誤訊息清楚且有建設性
- [ ] 表單提交後有明確的成功/失敗回饋
- [ ] 支援鍵盤導航和操作

**響應式測試**：
- [ ] 在不同裝置上布局正確
- [ ] 觸控操作區域大小適當
- [ ] 文字在小螢幕上仍然可讀
- [ ] 橫向/直向切換正常

### 7.2 效能測試基準

**載入效能**：
- [ ] 首次內容繪製 (FCP) < 1.5秒
- [ ] 最大內容繪製 (LCP) < 2.5秒
- [ ] 首次輸入延遲 (FID) < 100毫秒
- [ ] 累積版面偏移 (CLS) < 0.1

**互動效能**：
- [ ] 按鈕點擊回應時間 < 100毫秒
- [ ] 頁面切換動畫流暢 (60fps)
- [ ] 滾動效能流暢
- [ ] 搜尋結果顯示 < 500毫秒

---

## 8. 實作建議

### 8.1 技術堆疊建議

**前端框架**：
- **React 18** + TypeScript
- **Tailwind CSS** 用於樣式系統
- **Headless UI** 用於無障礙組件
- **React Query** 用於狀態管理和API快取

**UI元件庫**：
- **Radix UI** 用於基礎組件
- **Chart.js** 或 **D3.js** 用於資料視覺化
- **React Hook Form** 用於表單管理
- **React Virtual** 用於大列表優化

**開發工具**：
- **Vite** 用於快速建置
- **ESLint** + **Prettier** 用於程式碼品質
- **Storybook** 用於組件文件
- **Cypress** 用於E2E測試

### 8.2 開發流程建議

**設計系統建立**：
1. 建立設計token檔案
2. 開發基礎組件庫
3. 建立組件文件和範例
4. 設定自動化測試

**頁面開發順序**：
1. 專案儀表板 (最高優先級)
2. 任務管理介面
3. 專案模板中心
4. AI品質中心
5. 工作流程設計器

**品質保證**：
1. 每個組件都要有Storybook文件
2. 關鍵頁面要有E2E測試
3. 定期進行可用性測試
4. 效能監控和優化

---

## 9. 結論

### 9.1 原型設計總結

第2階段的UI/UX原型設計完成了以下關鍵成果：

**設計系統建立**：
- 統一的色彩、字體、間距規範
- 響應式設計斷點定義
- 可重用的組件庫設計

**核心頁面原型**：
- 專案儀表板的完整設計
- 模板中心的瀏覽和配置流程
- 任務管理的多視圖設計
- AI品質中心的監控介面
- 工作流程設計器的互動設計

**使用者體驗優化**：
- 角色導向的介面設計
- 直觀的操作流程
- 完善的回饋機制
- 無障礙設計支援

### 9.2 設計特色

**智能化體驗**：
- AI驅動的智能推薦
- 自動化的狀態更新
- 預測性的使用者輔助

**協作性設計**：
- 即時的多人協作
- 清晰的通知機制
- 透明的進度追蹤

**響應式適應**：
- 跨裝置一致體驗
- 觸控友善的互動設計
- 漸進式功能載入

### 9.3 下一步工作

**立即執行**：
1. 建立高保真互動原型
2. 進行使用者測試驗證
3. 優化設計細節

**開發準備**：
1. 建立設計系統組件庫
2. 準備設計規格交付
3. 與開發團隊協作對接

**持續改善**：
1. 收集使用者回饋
2. 迭代優化設計
3. 追蹤使用數據分析

這個UI/UX原型設計為第2階段的開發提供了完整的視覺和互動指引，確保最終產品能夠提供優秀的使用者體驗，並滿足各類使用者的實際工作需求。

---

*此UI/UX原型規格文件將作為前端開發的詳細設計指引，並隨使用者測試結果和開發進展持續優化完善。*