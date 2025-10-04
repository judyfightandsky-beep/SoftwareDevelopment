# 前後端 API 整合溝通備忘錄 (v1)

## 目前狀態
- 前端註冊流程已完成 ✓
- API 呼叫邏輯已建立 ✓
- 需要後端 API 端點確認 🔍

## API 介面規格確認

### 1. 用戶名稱可用性檢查 `/check-username`
- **請求**：`{ username: string }`
- **回應**：
  ```json
  {
    "success": true,
    "data": {
      "available": boolean
    }
  }
  ```
- **驗證邏輯**：
  - 使用者名稱長度：3-20 字元
  - 允許字母、數字、下底線
  - 檢查是否已被使用

### 2. 信箱可用性檢查 `/check-email`
- **請求**：`{ email: string }`
- **回應**：
  ```json
  {
    "success": true,
    "data": {
      "available": boolean
    }
  }
  ```
- **驗證邏輯**：
  - 信箱格式驗證
  - 檢查是否已被使用
  - 單一信箱僅能註冊一個帳號

### 3. 使用者註冊 `/register`
- **請求**：
  ```typescript
  {
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
  }
  ```
- **回應**：
  ```json
  {
    "success": true,
    "data": {
      "userId": string,
      "username": string,
      "email": string,
      "verificationStatus": "pending" | "verified"
    }
  }
  ```
- **驗證邏輯**：
  - 密碼複雜度檢查
  - 密碼與確認密碼比對
  - 產生驗證信
  - 設定預設角色（訪客）

### 4. 重新發送驗證信 `/resend-verification`
- **請求**：`{ email: string }`
- **回應**：
  ```json
  {
    "success": true,
    "data": {
      "message": string
    }
  }
  ```
- **驗證邏輯**：
  - 檢查信箱是否已註冊
  - 限制重試次數
  - 15 分鐘內有效

## 待確認事項
1. 密碼複雜度具體要求
2. 驗證信發送機制
3. 錯誤訊息詳細程度

## 下一步
- 後端實作上述 API 端點
- 測試各種註冊場景
- 確保安全性和使用者體驗