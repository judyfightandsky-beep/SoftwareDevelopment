# 後端 API 整合檢查清單

## API 端點驗證狀態
- [ ] `/check-username` - 檢查用戶名可用性
  - 預期請求：`{ username: string }`
  - 預期回應：`{ success: boolean, data: { available: boolean } }`

- [ ] `/check-email` - 檢查信箱可用性
  - 預期請求：`{ email: string }`
  - 預期回應：`{ success: boolean, data: { available: boolean } }`

- [ ] `/register` - 使用者註冊
  - 預期請求：
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
  - 預期回應：
    ```typescript
    {
      success: boolean;
      data: {
        userId: string;
        username: string;
        email: string;
        verificationStatus: 'pending' | 'verified';
      }
    }
    ```

- [ ] `/resend-verification` - 重新發送驗證信
  - 預期請求：`{ email: string }`
  - 預期回應：`{ success: boolean, data: { message: string } }`

## 整合測試要點
1. 檢查各 API 端點的錯誤處理
2. 驗證 API 回應格式是否一致
3. 模擬異常情境（如重複註冊、無效輸入）
4. 確認後端驗證邏輯與前端一致

## 待解決問題
- [ ] 確認後端密碼複雜度要求
- [ ] 驗證信箱發送與重試機制
- [ ] 帳號初始角色設定