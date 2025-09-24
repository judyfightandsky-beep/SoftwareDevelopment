# 使用者登入系統 - UI設計規範

## 設計理念

**簡潔有力 | 開發者友善 | 功能清晰**

專為軟體開發團隊設計的身份認證介面，採用現代化的設計語言，強調功能性和使用效率，避免過度複雜的視覺元素。

## 🎨 設計系統

### 色彩系統 (Developer-Focused Palette)

```css
/* 主要色彩 - 專業藍調 */
:root {
  /* Primary Colors - 科技藍 */
  --primary-50: #eff6ff;
  --primary-100: #dbeafe;
  --primary-200: #bfdbfe;
  --primary-300: #93c5fd;
  --primary-400: #60a5fa;
  --primary-500: #3b82f6;  /* 主色 */
  --primary-600: #2563eb;
  --primary-700: #1d4ed8;
  --primary-800: #1e40af;
  --primary-900: #1e3a8a;

  /* Secondary Colors - 程式碼綠 */
  --success-50: #f0fdf4;
  --success-100: #dcfce7;
  --success-200: #bbf7d0;
  --success-300: #86efac;
  --success-400: #4ade80;
  --success-500: #22c55e;  /* 成功色 */
  --success-600: #16a34a;
  --success-700: #15803d;

  /* Warning Colors - 警告橘 */
  --warning-50: #fffbeb;
  --warning-100: #fef3c7;
  --warning-200: #fde68a;
  --warning-300: #fcd34d;
  --warning-400: #fbbf24;
  --warning-500: #f59e0b;  /* 警告色 */
  --warning-600: #d97706;

  /* Error Colors - 錯誤紅 */
  --error-50: #fef2f2;
  --error-100: #fee2e2;
  --error-200: #fecaca;
  --error-300: #fca5a5;
  --error-400: #f87171;
  --error-500: #ef4444;    /* 錯誤色 */
  --error-600: #dc2626;
  --error-700: #b91c1c;

  /* Neutral Colors - 開發者灰階 */
  --gray-50: #f9fafb;
  --gray-100: #f3f4f6;
  --gray-200: #e5e7eb;
  --gray-300: #d1d5db;
  --gray-400: #9ca3af;
  --gray-500: #6b7280;
  --gray-600: #4b5563;
  --gray-700: #374151;
  --gray-800: #1f2937;
  --gray-900: #111827;

  /* Code Colors - 程式碼風格 */
  --code-bg: #0d1117;
  --code-text: #c9d1d9;
  --code-border: #21262d;
  --code-accent: #58a6ff;
}
```

### 字體系統 (Typography)

```css
/* 字體族群 - 開發者友善字體 */
:root {
  /* 主要字體 - 系統字體優先 */
  --font-main: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto,
               'Helvetica Neue', Arial, sans-serif;

  /* 程式碼字體 - 等寬字體 */
  --font-mono: 'JetBrains Mono', 'SF Mono', Monaco, 'Cascadia Code',
               'Roboto Mono', Consolas, 'Courier New', monospace;

  /* 標題字體 - 更有力量感 */
  --font-heading: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;
}

/* 字體大小階層 */
:root {
  --text-xs: 0.75rem;    /* 12px */
  --text-sm: 0.875rem;   /* 14px */
  --text-base: 1rem;     /* 16px */
  --text-lg: 1.125rem;   /* 18px */
  --text-xl: 1.25rem;    /* 20px */
  --text-2xl: 1.5rem;    /* 24px */
  --text-3xl: 1.875rem;  /* 30px */
  --text-4xl: 2.25rem;   /* 36px */
}
```

### 間距系統 (Spacing)

```css
:root {
  --space-0: 0;
  --space-1: 0.25rem;   /* 4px */
  --space-2: 0.5rem;    /* 8px */
  --space-3: 0.75rem;   /* 12px */
  --space-4: 1rem;      /* 16px */
  --space-5: 1.25rem;   /* 20px */
  --space-6: 1.5rem;    /* 24px */
  --space-8: 2rem;      /* 32px */
  --space-10: 2.5rem;   /* 40px */
  --space-12: 3rem;     /* 48px */
  --space-16: 4rem;     /* 64px */
  --space-20: 5rem;     /* 80px */
}
```

### 圓角系統 (Border Radius)

```css
:root {
  --radius-sm: 0.25rem;   /* 4px - 小圓角 */
  --radius-md: 0.375rem;  /* 6px - 中圓角 */
  --radius-lg: 0.5rem;    /* 8px - 大圓角 */
  --radius-xl: 0.75rem;   /* 12px - 超大圓角 */
  --radius-full: 9999px;  /* 完全圓形 */
}
```

### 陰影系統 (Shadows)

```css
:root {
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  --shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.1);

  /* 開發者風格陰影 */
  --shadow-code: 0 0 0 1px var(--code-border), 0 2px 4px rgba(0, 0, 0, 0.1);
  --shadow-focus: 0 0 0 3px rgba(59, 130, 246, 0.15);
}
```

## 🖼️ UI組件設計

### 1. 登入頁面設計

```html
<!-- 登入頁面 HTML 結構 -->
<div class="login-container">
  <div class="login-card">
    <!-- Logo區域 -->
    <div class="logo-section">
      <div class="logo">
        <svg class="logo-icon" viewBox="0 0 24 24">
          <path d="M12 2L2 7V10C2 16 6 20.09 12 22C18 20.09 22 16 22 10V7L12 2Z"/>
        </svg>
        <h1 class="logo-text">DevAuth</h1>
      </div>
      <p class="subtitle">開發者身份驗證系統</p>
    </div>

    <!-- 登入表單 -->
    <form class="login-form">
      <!-- 登入方式切換 -->
      <div class="login-tabs">
        <button type="button" class="tab-button active" data-tab="credentials">
          <svg class="tab-icon" viewBox="0 0 20 20">
            <path d="M10 2L3 7V18H17V7L10 2Z"/>
          </svg>
          帳號登入
        </button>
        <button type="button" class="tab-button" data-tab="qrcode">
          <svg class="tab-icon" viewBox="0 0 20 20">
            <path d="M3 3H9V9H3V3ZM11 3H17V9H11V3ZM3 11H9V17H3V11Z"/>
          </svg>
          QR登入
        </button>
      </div>

      <!-- 帳號登入面板 -->
      <div class="tab-panel active" data-panel="credentials">
        <div class="input-group">
          <label for="identifier" class="input-label">
            帳號 / 電子信箱
          </label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M10 9C11.1 9 12 8.1 12 7S11.1 5 10 5 8 5.9 8 7 8.9 9 10 9Z"/>
            </svg>
            <input
              type="text"
              id="identifier"
              class="input-field"
              placeholder="輸入您的帳號或電子信箱"
              autocomplete="username"
              required
            >
          </div>
        </div>

        <div class="input-group">
          <label for="password" class="input-label">
            密碼
          </label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M5 9V7C5 4.79 6.79 3 9 3H11C13.21 3 15 4.79 15 7V9H16C16.55 9 17 9.45 17 10V18C17 18.55 16.55 19 16 19H4C3.45 19 3 18.55 3 18V10C3 9.45 3.45 9 4 9H5Z"/>
            </svg>
            <input
              type="password"
              id="password"
              class="input-field"
              placeholder="輸入您的密碼"
              autocomplete="current-password"
              required
            >
            <button type="button" class="password-toggle">
              <svg class="toggle-icon" viewBox="0 0 20 20">
                <path d="M10 4C5 4 1.73 7.11 1 10C1.73 12.89 5 16 10 16S18.27 12.89 19 10C18.27 7.11 15 4 10 4Z"/>
              </svg>
            </button>
          </div>
        </div>

        <div class="form-options">
          <label class="checkbox-wrapper">
            <input type="checkbox" class="checkbox">
            <span class="checkmark"></span>
            <span class="checkbox-label">記住我</span>
          </label>
          <a href="/forgot-password" class="forgot-link">忘記密碼？</a>
        </div>

        <button type="submit" class="submit-button">
          <svg class="button-icon" viewBox="0 0 20 20">
            <path d="M3 10L8 15L17 6"/>
          </svg>
          登入
        </button>
      </div>

      <!-- QR Code登入面板 -->
      <div class="tab-panel" data-panel="qrcode">
        <div class="qr-section">
          <div class="qr-code-container">
            <div class="qr-code" id="qrCodeImage">
              <!-- QR Code 會動態生成在這裡 -->
              <div class="qr-loading">
                <svg class="loading-spinner" viewBox="0 0 24 24">
                  <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" fill="none"/>
                </svg>
                <p>生成中...</p>
              </div>
            </div>
          </div>
          <div class="qr-instructions">
            <h3>使用手機應用程式掃描</h3>
            <ol class="qr-steps">
              <li>開啟手機上的認證應用</li>
              <li>掃描上方的QR碼</li>
              <li>確認登入即可完成</li>
            </ol>
            <p class="qr-expire">QR碼將在 <span id="qrTimer">4:59</span> 後過期</p>
          </div>
        </div>
      </div>
    </form>

    <!-- 註冊連結 -->
    <div class="register-section">
      <p class="register-text">
        還沒有帳號？
        <a href="/register" class="register-link">立即註冊</a>
      </p>
    </div>

    <!-- 狀態訊息 -->
    <div class="message-container" id="messageContainer">
      <!-- 動態顯示錯誤或成功訊息 -->
    </div>
  </div>

  <!-- 背景裝飾 -->
  <div class="background-pattern">
    <div class="pattern-grid"></div>
  </div>
</div>
```

### 2. 登入頁面CSS樣式

```css
/* 登入頁面樣式 */
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, var(--gray-50) 0%, var(--primary-50) 100%);
  padding: var(--space-4);
  position: relative;
  overflow: hidden;
}

.login-card {
  background: white;
  border-radius: var(--radius-xl);
  box-shadow: var(--shadow-xl);
  padding: var(--space-10);
  width: 100%;
  max-width: 420px;
  position: relative;
  z-index: 10;
}

/* Logo區域 */
.logo-section {
  text-align: center;
  margin-bottom: var(--space-10);
}

.logo {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: var(--space-3);
  margin-bottom: var(--space-4);
}

.logo-icon {
  width: 40px;
  height: 40px;
  fill: var(--primary-500);
}

.logo-text {
  font-family: var(--font-heading);
  font-size: var(--text-3xl);
  font-weight: 700;
  color: var(--gray-900);
  margin: 0;
}

.subtitle {
  color: var(--gray-600);
  font-size: var(--text-sm);
  margin: 0;
}

/* 登入方式切換 */
.login-tabs {
  display: flex;
  background: var(--gray-100);
  border-radius: var(--radius-lg);
  padding: var(--space-1);
  margin-bottom: var(--space-8);
}

.tab-button {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: var(--space-2);
  padding: var(--space-3) var(--space-4);
  border: none;
  background: transparent;
  color: var(--gray-600);
  font-size: var(--text-sm);
  font-weight: 500;
  border-radius: var(--radius-md);
  cursor: pointer;
  transition: all 0.2s ease;
}

.tab-button.active {
  background: white;
  color: var(--primary-600);
  box-shadow: var(--shadow-sm);
}

.tab-icon {
  width: 16px;
  height: 16px;
  fill: currentColor;
}

/* 表單面板 */
.tab-panel {
  display: none;
}

.tab-panel.active {
  display: block;
}

/* 輸入欄位 */
.input-group {
  margin-bottom: var(--space-6);
}

.input-label {
  display: block;
  font-size: var(--text-sm);
  font-weight: 500;
  color: var(--gray-700);
  margin-bottom: var(--space-2);
}

.input-wrapper {
  position: relative;
}

.input-field {
  width: 100%;
  padding: var(--space-3) var(--space-4) var(--space-3) var(--space-12);
  border: 2px solid var(--gray-200);
  border-radius: var(--radius-lg);
  font-size: var(--text-base);
  background: white;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.input-field:focus {
  outline: none;
  border-color: var(--primary-500);
  box-shadow: var(--shadow-focus);
}

.input-field::placeholder {
  color: var(--gray-400);
}

.input-icon {
  position: absolute;
  left: var(--space-4);
  top: 50%;
  transform: translateY(-50%);
  width: 20px;
  height: 20px;
  fill: var(--gray-400);
  pointer-events: none;
}

.password-toggle {
  position: absolute;
  right: var(--space-4);
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: var(--gray-400);
  cursor: pointer;
  padding: var(--space-1);
}

.toggle-icon {
  width: 20px;
  height: 20px;
  fill: currentColor;
}

/* 表單選項 */
.form-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: var(--space-8);
}

.checkbox-wrapper {
  display: flex;
  align-items: center;
  gap: var(--space-2);
  cursor: pointer;
}

.checkbox {
  display: none;
}

.checkmark {
  width: 18px;
  height: 18px;
  border: 2px solid var(--gray-300);
  border-radius: var(--radius-sm);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.checkbox:checked + .checkmark {
  background: var(--primary-500);
  border-color: var(--primary-500);
}

.checkbox:checked + .checkmark::after {
  content: '✓';
  color: white;
  font-size: 12px;
  font-weight: bold;
}

.checkbox-label {
  font-size: var(--text-sm);
  color: var(--gray-600);
  user-select: none;
}

.forgot-link {
  color: var(--primary-600);
  font-size: var(--text-sm);
  text-decoration: none;
  font-weight: 500;
}

.forgot-link:hover {
  text-decoration: underline;
}

/* 提交按鈕 */
.submit-button {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: var(--space-2);
  padding: var(--space-4);
  background: var(--primary-500);
  color: white;
  border: none;
  border-radius: var(--radius-lg);
  font-size: var(--text-base);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.submit-button:hover {
  background: var(--primary-600);
  transform: translateY(-1px);
  box-shadow: var(--shadow-lg);
}

.submit-button:active {
  transform: translateY(0);
}

.submit-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.button-icon {
  width: 20px;
  height: 20px;
  fill: currentColor;
}

/* QR Code區域 */
.qr-section {
  text-align: center;
}

.qr-code-container {
  margin-bottom: var(--space-6);
}

.qr-code {
  width: 200px;
  height: 200px;
  margin: 0 auto var(--space-4);
  border: 2px solid var(--gray-200);
  border-radius: var(--radius-lg);
  display: flex;
  align-items: center;
  justify-content: center;
  background: white;
}

.qr-loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-2);
  color: var(--gray-400);
}

.loading-spinner {
  width: 32px;
  height: 32px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

.qr-instructions h3 {
  font-size: var(--text-lg);
  color: var(--gray-800);
  margin-bottom: var(--space-4);
  font-weight: 600;
}

.qr-steps {
  text-align: left;
  margin-bottom: var(--space-4);
  padding-left: var(--space-4);
}

.qr-steps li {
  color: var(--gray-600);
  font-size: var(--text-sm);
  margin-bottom: var(--space-2);
}

.qr-expire {
  color: var(--gray-500);
  font-size: var(--text-xs);
  margin: 0;
}

#qrTimer {
  color: var(--warning-500);
  font-weight: 600;
  font-family: var(--font-mono);
}

/* 註冊區域 */
.register-section {
  text-align: center;
  margin-top: var(--space-8);
  padding-top: var(--space-6);
  border-top: 1px solid var(--gray-200);
}

.register-text {
  color: var(--gray-600);
  font-size: var(--text-sm);
  margin: 0;
}

.register-link {
  color: var(--primary-600);
  font-weight: 600;
  text-decoration: none;
}

.register-link:hover {
  text-decoration: underline;
}

/* 訊息容器 */
.message-container {
  margin-top: var(--space-4);
}

/* 背景圖案 */
.background-pattern {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  opacity: 0.1;
  z-index: 1;
}

.pattern-grid {
  width: 100%;
  height: 100%;
  background-image:
    linear-gradient(90deg, var(--primary-200) 1px, transparent 1px),
    linear-gradient(var(--primary-200) 1px, transparent 1px);
  background-size: 20px 20px;
}

/* 響應式設計 */
@media (max-width: 480px) {
  .login-card {
    padding: var(--space-6);
  }

  .logo-text {
    font-size: var(--text-2xl);
  }

  .qr-code {
    width: 160px;
    height: 160px;
  }
}
```

### 3. 訊息組件設計

```css
/* 訊息組件 */
.message {
  padding: var(--space-4);
  border-radius: var(--radius-lg);
  font-size: var(--text-sm);
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: var(--space-3);
  margin-bottom: var(--space-4);
}

.message-success {
  background: var(--success-50);
  color: var(--success-700);
  border: 1px solid var(--success-200);
}

.message-error {
  background: var(--error-50);
  color: var(--error-700);
  border: 1px solid var(--error-200);
}

.message-warning {
  background: var(--warning-50);
  color: var(--warning-700);
  border: 1px solid var(--warning-200);
}

.message-info {
  background: var(--primary-50);
  color: var(--primary-700);
  border: 1px solid var(--primary-200);
}

.message-icon {
  width: 20px;
  height: 20px;
  fill: currentColor;
  flex-shrink: 0;
}
```

## 📱 響應式設計

### 行動裝置優化

```css
/* 行動裝置適配 */
@media (max-width: 768px) {
  .login-container {
    padding: var(--space-2);
  }

  .login-card {
    padding: var(--space-6);
    max-width: none;
    width: 100%;
  }

  .login-tabs {
    flex-direction: column;
    gap: var(--space-1);
  }

  .tab-button {
    padding: var(--space-4);
  }

  .input-field {
    padding: var(--space-4);
    font-size: 16px; /* 防止iOS縮放 */
  }
}

/* 平板適配 */
@media (min-width: 769px) and (max-width: 1024px) {
  .login-card {
    max-width: 480px;
  }
}
```

## 🎯 互動設計

### JavaScript 功能實作

```javascript
// 登入頁面互動邏輯
class LoginUI {
  constructor() {
    this.initTabs();
    this.initForm();
    this.initQRCode();
    this.initPasswordToggle();
  }

  initTabs() {
    const tabButtons = document.querySelectorAll('.tab-button');
    const tabPanels = document.querySelectorAll('.tab-panel');

    tabButtons.forEach(button => {
      button.addEventListener('click', (e) => {
        const tabId = e.currentTarget.dataset.tab;

        // 切換按鈕狀態
        tabButtons.forEach(btn => btn.classList.remove('active'));
        e.currentTarget.classList.add('active');

        // 切換面板
        tabPanels.forEach(panel => {
          panel.classList.remove('active');
          if (panel.dataset.panel === tabId) {
            panel.classList.add('active');
          }
        });

        // 如果切換到QR碼，重新生成
        if (tabId === 'qrcode') {
          this.generateQRCode();
        }
      });
    });
  }

  initForm() {
    const form = document.querySelector('.login-form');
    const submitButton = document.querySelector('.submit-button');

    form.addEventListener('submit', async (e) => {
      e.preventDefault();

      const activePanel = document.querySelector('.tab-panel.active');
      const isCredentialLogin = activePanel.dataset.panel === 'credentials';

      if (isCredentialLogin) {
        await this.handleCredentialLogin(form);
      }
    });
  }

  async handleCredentialLogin(form) {
    const formData = new FormData(form);
    const identifier = formData.get('identifier');
    const password = formData.get('password');
    const rememberMe = formData.get('remember') === 'on';

    this.showLoading(true);

    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          identifier,
          password,
          rememberMe,
          ipAddress: await this.getClientIP(),
          userAgent: navigator.userAgent
        })
      });

      const result = await response.json();

      if (response.ok) {
        this.showMessage('登入成功！正在跳轉...', 'success');

        // 儲存token
        if (rememberMe) {
          localStorage.setItem('authToken', result.token);
        } else {
          sessionStorage.setItem('authToken', result.token);
        }

        // 跳轉到儀表板
        setTimeout(() => {
          window.location.href = '/dashboard';
        }, 1000);

      } else {
        this.showMessage(result.message || '登入失敗，請檢查您的帳號密碼', 'error');
      }
    } catch (error) {
      console.error('Login error:', error);
      this.showMessage('網路連線異常，請稍後再試', 'error');
    } finally {
      this.showLoading(false);
    }
  }

  initPasswordToggle() {
    const toggleButton = document.querySelector('.password-toggle');
    const passwordField = document.querySelector('#password');

    toggleButton.addEventListener('click', () => {
      const isPassword = passwordField.type === 'password';
      passwordField.type = isPassword ? 'text' : 'password';

      // 更新圖標
      const icon = toggleButton.querySelector('.toggle-icon');
      if (isPassword) {
        icon.innerHTML = `<path d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.878 9.878L8.464 8.464m1.414 1.414l-2.122 2.122m0 0L6.343 10.586m1.415 1.414L9.88 13.88m-3.415-3.415L4.343 8.343"/>`;
      } else {
        icon.innerHTML = `<path d="M10 4C5 4 1.73 7.11 1 10C1.73 12.89 5 16 10 16S18.27 12.89 19 10C18.27 7.11 15 4 10 4Z"/>`;
      }
    });
  }

  async generateQRCode() {
    const qrContainer = document.querySelector('#qrCodeImage');
    const qrTimer = document.querySelector('#qrTimer');

    // 顯示載入狀態
    qrContainer.innerHTML = `
      <div class="qr-loading">
        <svg class="loading-spinner" viewBox="0 0 24 24">
          <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" fill="none"/>
        </svg>
        <p>生成中...</p>
      </div>
    `;

    try {
      const response = await fetch('/api/auth/qr-code', {
        method: 'GET',
      });

      const result = await response.json();

      if (response.ok) {
        // 顯示QR碼
        qrContainer.innerHTML = `<img src="data:image/png;base64,${result.imageData}" alt="QR Code" style="width: 100%; height: 100%; object-fit: contain;">`;

        // 啟動倒數計時
        this.startQRTimer(result.expiresAt, qrTimer);

        // 監聽QR碼狀態
        this.monitorQRStatus(result.id);

      } else {
        throw new Error(result.message);
      }
    } catch (error) {
      console.error('QR Code generation error:', error);
      qrContainer.innerHTML = `
        <div class="qr-error">
          <p>QR碼生成失敗</p>
          <button onclick="location.reload()" class="retry-button">重試</button>
        </div>
      `;
    }
  }

  startQRTimer(expiresAt, timerElement) {
    const expiryTime = new Date(expiresAt).getTime();

    const updateTimer = () => {
      const now = new Date().getTime();
      const timeLeft = expiryTime - now;

      if (timeLeft <= 0) {
        timerElement.textContent = '已過期';
        timerElement.style.color = 'var(--error-500)';
        this.generateQRCode(); // 自動重新生成
        return;
      }

      const minutes = Math.floor(timeLeft / 60000);
      const seconds = Math.floor((timeLeft % 60000) / 1000);
      timerElement.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
    };

    updateTimer();
    const interval = setInterval(updateTimer, 1000);

    // 5分鐘後清除計時器
    setTimeout(() => clearInterval(interval), 300000);
  }

  monitorQRStatus(qrCodeId) {
    // 使用WebSocket監聽QR碼狀態
    const ws = new WebSocket(`ws://localhost:8080/ws/qr-status/${qrCodeId}`);

    ws.onmessage = (event) => {
      const data = JSON.parse(event.data);

      if (data.status === 'authenticated') {
        this.showMessage('QR碼登入成功！正在跳轉...', 'success');

        // 儲存token
        localStorage.setItem('authToken', data.token);

        // 跳轉到儀表板
        setTimeout(() => {
          window.location.href = '/dashboard';
        }, 1000);
      }
    };

    ws.onerror = () => {
      console.warn('QR WebSocket connection failed, falling back to polling');
      // 降級為輪詢機制
      this.pollQRStatus(qrCodeId);
    };
  }

  async pollQRStatus(qrCodeId) {
    const checkStatus = async () => {
      try {
        const response = await fetch(`/api/auth/qr-code/${qrCodeId}/status`);
        const result = await response.json();

        if (result.status === 'authenticated') {
          this.showMessage('QR碼登入成功！正在跳轉...', 'success');
          localStorage.setItem('authToken', result.token);
          setTimeout(() => {
            window.location.href = '/dashboard';
          }, 1000);
          return;
        }

        if (result.status === 'expired') {
          this.generateQRCode(); // 重新生成
          return;
        }

        // 繼續輪詢
        setTimeout(checkStatus, 2000);

      } catch (error) {
        console.error('QR status polling error:', error);
        setTimeout(checkStatus, 5000); // 錯誤時延長間隔
      }
    };

    checkStatus();
  }

  showMessage(text, type) {
    const container = document.getElementById('messageContainer');

    const iconMap = {
      success: `<path d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/>`,
      error: `<path d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>`,
      warning: `<path d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z"/>`,
      info: `<path d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>`
    };

    container.innerHTML = `
      <div class="message message-${type}">
        <svg class="message-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
          ${iconMap[type]}
        </svg>
        ${text}
      </div>
    `;

    // 自動移除訊息
    setTimeout(() => {
      container.innerHTML = '';
    }, 5000);
  }

  showLoading(isLoading) {
    const submitButton = document.querySelector('.submit-button');
    const buttonIcon = submitButton.querySelector('.button-icon');

    if (isLoading) {
      submitButton.disabled = true;
      buttonIcon.outerHTML = `
        <svg class="button-icon loading-spinner" viewBox="0 0 24 24">
          <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" fill="none"/>
        </svg>
      `;
    } else {
      submitButton.disabled = false;
      submitButton.querySelector('.button-icon').outerHTML = `
        <svg class="button-icon" viewBox="0 0 20 20">
          <path d="M3 10L8 15L17 6"/>
        </svg>
      `;
    }
  }

  async getClientIP() {
    try {
      const response = await fetch('https://api.ipify.org?format=json');
      const data = await response.json();
      return data.ip;
    } catch (error) {
      return 'unknown';
    }
  }
}

// 初始化登入介面
document.addEventListener('DOMContentLoaded', () => {
  new LoginUI();
});
```

這個UI設計具有以下特色：

## ✨ 設計特色

### 🎯 簡潔有力
- **最小化視覺干擾**：清晰的層次結構，重點突出
- **功能導向**：每個元素都有明確目的，無多餘裝飾
- **直覺操作**：符合用戶預期的互動模式

### 🛠️ 開發者風格
- **程式碼美學**：使用等寬字體、程式碼色彩系統
- **技術感**：適度的幾何圖形和網格背景
- **專業色調**：藍色主調，傳達可靠和專業感

### 🔧 功能清晰
- **標籤式切換**：傳統登入vs QR碼登入清楚分離
- **視覺回饋**：載入狀態、錯誤提示、成功確認
- **漸進增強**：基礎功能優先，進階功能可選

### 📱 響應式設計
- **行動優先**：從小螢幕開始設計，向上擴展
- **觸控友善**：適當的點擊區域大小
- **跨裝置一致**：保持品牌和功能一致性

## 📝 註冊頁面設計

### 註冊頁面HTML結構

```html
<!-- 註冊頁面 -->
<div class="register-container">
  <div class="register-card">
    <!-- 進度指示器 -->
    <div class="progress-indicator">
      <div class="progress-step active" data-step="1">
        <div class="step-number">1</div>
        <div class="step-label">基本資料</div>
      </div>
      <div class="progress-line"></div>
      <div class="progress-step" data-step="2">
        <div class="step-number">2</div>
        <div class="step-label">驗證信箱</div>
      </div>
      <div class="progress-line"></div>
      <div class="progress-step" data-step="3">
        <div class="step-number">3</div>
        <div class="step-label">完成註冊</div>
      </div>
    </div>

    <!-- Logo區域 -->
    <div class="logo-section">
      <div class="logo">
        <svg class="logo-icon" viewBox="0 0 24 24">
          <path d="M12 2L2 7V10C2 16 6 20.09 12 22C18 20.09 22 16 22 10V7L12 2Z"/>
        </svg>
        <h1 class="logo-text">DevAuth</h1>
      </div>
      <p class="subtitle">建立您的開發者帳號</p>
    </div>

    <!-- 步驟1: 基本資料 -->
    <div class="register-step active" data-step="1">
      <form class="register-form" id="basicInfoForm">
        <div class="form-row">
          <div class="input-group">
            <label for="firstName" class="input-label">名字 *</label>
            <div class="input-wrapper">
              <input
                type="text"
                id="firstName"
                name="firstName"
                class="input-field"
                placeholder="請輸入您的名字"
                required
              >
            </div>
          </div>
          <div class="input-group">
            <label for="lastName" class="input-label">姓氏 *</label>
            <div class="input-wrapper">
              <input
                type="text"
                id="lastName"
                name="lastName"
                class="input-field"
                placeholder="請輸入您的姓氏"
                required
              >
            </div>
          </div>
        </div>

        <div class="input-group">
          <label for="username" class="input-label">使用者名稱 *</label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M10 9C11.1 9 12 8.1 12 7S11.1 5 10 5 8 5.9 8 7 8.9 9 10 9Z"/>
            </svg>
            <input
              type="text"
              id="username"
              name="username"
              class="input-field"
              placeholder="選擇一個唯一的使用者名稱"
              required
              minlength="3"
              maxlength="20"
            >
            <div class="field-status" id="usernameStatus"></div>
          </div>
          <div class="field-hint">3-20個字元，只能包含字母、數字和下底線</div>
        </div>

        <div class="input-group">
          <label for="email" class="input-label">電子信箱 *</label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M3 4C3 3.45 3.45 3 4 3H16C16.55 3 17 3.45 17 4V16C17 16.55 16.55 17 16 17H4C3.45 17 3 16.55 3 16V4Z"/>
            </svg>
            <input
              type="email"
              id="email"
              name="email"
              class="input-field"
              placeholder="example@company.com"
              required
            >
            <div class="field-status" id="emailStatus"></div>
          </div>
          <div class="field-hint">我們將發送驗證信件到此信箱</div>
        </div>

        <div class="input-group">
          <label for="password" class="input-label">密碼 *</label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M5 9V7C5 4.79 6.79 3 9 3H11C13.21 3 15 4.79 15 7V9H16C16.55 9 17 9.45 17 10V18C17 18.55 16.55 19 16 19H4C3.45 19 3 18.55 3 18V10C3 9.45 3.45 9 4 9H5Z"/>
            </svg>
            <input
              type="password"
              id="password"
              name="password"
              class="input-field"
              placeholder="建立一個強密碼"
              required
              minlength="8"
            >
            <button type="button" class="password-toggle">
              <svg class="toggle-icon" viewBox="0 0 20 20">
                <path d="M10 4C5 4 1.73 7.11 1 10C1.73 12.89 5 16 10 16S18.27 12.89 19 10C18.27 7.11 15 4 10 4Z"/>
              </svg>
            </button>
          </div>
          <div class="password-strength">
            <div class="strength-bar">
              <div class="strength-fill" id="strengthFill"></div>
            </div>
            <div class="strength-text" id="strengthText">請輸入密碼</div>
          </div>
        </div>

        <div class="input-group">
          <label for="confirmPassword" class="input-label">確認密碼 *</label>
          <div class="input-wrapper">
            <svg class="input-icon" viewBox="0 0 20 20">
              <path d="M5 9V7C5 4.79 6.79 3 9 3H11C13.21 3 15 4.79 15 7V9H16C16.55 9 17 9.45 17 10V18C17 18.55 16.55 19 16 19H4C3.45 19 3 18.55 3 18V10C3 9.45 3.45 9 4 9H5Z"/>
            </svg>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              class="input-field"
              placeholder="再次輸入密碼"
              required
            >
            <div class="field-status" id="confirmPasswordStatus"></div>
          </div>
        </div>

        <div class="terms-section">
          <label class="checkbox-wrapper">
            <input type="checkbox" class="checkbox" id="agreeTerms" required>
            <span class="checkmark"></span>
            <span class="checkbox-label">
              我同意 <a href="/terms" target="_blank" class="link">服務條款</a>
              和 <a href="/privacy" target="_blank" class="link">隱私政策</a>
            </span>
          </label>
        </div>

        <button type="submit" class="submit-button">
          繼續
          <svg class="button-icon" viewBox="0 0 20 20">
            <path d="M12.95 10.707L8.414 15.242L9.828 16.656L16.485 9.999L9.828 3.343L8.414 4.757L12.95 10.707Z"/>
          </svg>
        </button>
      </form>
    </div>

    <!-- 步驟2: 信箱驗證 -->
    <div class="register-step" data-step="2">
      <div class="verification-content">
        <div class="verification-icon">
          <svg viewBox="0 0 64 64">
            <path d="M32 2L6 12V20C6 38 16 52 32 58C48 52 58 38 58 20V12L32 2Z" fill="var(--primary-100)"/>
            <path d="M22 30L30 38L44 24" stroke="var(--primary-500)" stroke-width="3" fill="none"/>
          </svg>
        </div>
        <h3>驗證您的信箱</h3>
        <p class="verification-text">
          我們已發送驗證信件到<br>
          <strong id="verificationEmail">example@company.com</strong>
        </p>
        <p class="verification-hint">
          請檢查您的收件匣（包括垃圾信件匣），並點擊信件中的驗證連結。
        </p>

        <div class="verification-actions">
          <button type="button" class="secondary-button" id="resendEmail">
            <svg class="button-icon" viewBox="0 0 20 20">
              <path d="M4 2V8H10M10 2V8H16M20 12A8 8 0 11-4 12 8 8 0 0120 12Z"/>
            </svg>
            重新發送
          </button>
          <button type="button" class="secondary-button" id="changeEmail">
            <svg class="button-icon" viewBox="0 0 20 20">
              <path d="M11 5H6C5.45 5 5 5.45 5 6V14C5 14.55 5.45 15 6 15H11"/>
            </svg>
            更改信箱
          </button>
        </div>

        <div class="countdown-text">
          <span id="resendCountdown">30秒後可重新發送</span>
        </div>
      </div>
    </div>

    <!-- 步驟3: 完成註冊 -->
    <div class="register-step" data-step="3">
      <div class="success-content">
        <div class="success-icon">
          <svg viewBox="0 0 64 64">
            <circle cx="32" cy="32" r="28" fill="var(--success-100)"/>
            <path d="M20 32L28 40L44 24" stroke="var(--success-500)" stroke-width="4" fill="none"/>
          </svg>
        </div>
        <h3>註冊成功！</h3>
        <p class="success-text">
          歡迎加入 DevAuth 開發者社群！<br>
          您的帳號已成功建立並通過驗證。
        </p>

        <div class="user-info">
          <div class="info-item">
            <span class="info-label">使用者名稱：</span>
            <span class="info-value" id="finalUsername">johndoe</span>
          </div>
          <div class="info-item">
            <span class="info-label">電子信箱：</span>
            <span class="info-value" id="finalEmail">john@company.com</span>
          </div>
          <div class="info-item">
            <span class="info-label">角色權限：</span>
            <span class="info-value">訪客 (Guest)</span>
          </div>
        </div>

        <div class="next-steps">
          <h4>接下來您可以：</h4>
          <ul>
            <li>設定您的個人資料</li>
            <li>探索開發者工具</li>
            <li>加入專案團隊</li>
            <li>開始您的第一個專案</li>
          </ul>
        </div>

        <button type="button" class="submit-button" onclick="window.location.href='/login'">
          前往登入
          <svg class="button-icon" viewBox="0 0 20 20">
            <path d="M12.95 10.707L8.414 15.242L9.828 16.656L16.485 9.999L9.828 3.343L8.414 4.757L12.95 10.707Z"/>
          </svg>
        </button>
      </div>
    </div>

    <!-- 返回登入 -->
    <div class="back-to-login">
      <p>已經有帳號了？<a href="/login" class="link">立即登入</a></p>
    </div>

    <!-- 訊息容器 -->
    <div class="message-container" id="messageContainer"></div>
  </div>
</div>
```

### 註冊頁面CSS樣式

```css
/* 註冊頁面樣式 */
.register-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, var(--gray-50) 0%, var(--primary-50) 100%);
  padding: var(--space-4);
}

.register-card {
  background: white;
  border-radius: var(--radius-xl);
  box-shadow: var(--shadow-xl);
  padding: var(--space-10);
  width: 100%;
  max-width: 520px;
  position: relative;
}

/* 進度指示器 */
.progress-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: var(--space-10);
  padding: 0 var(--space-4);
}

.progress-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-2);
  position: relative;
  z-index: 2;
}

.step-number {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: var(--text-sm);
  background: var(--gray-200);
  color: var(--gray-600);
  transition: all 0.3s ease;
}

.progress-step.active .step-number {
  background: var(--primary-500);
  color: white;
}

.progress-step.completed .step-number {
  background: var(--success-500);
  color: white;
}

.step-label {
  font-size: var(--text-xs);
  color: var(--gray-600);
  font-weight: 500;
  text-align: center;
  white-space: nowrap;
}

.progress-step.active .step-label {
  color: var(--primary-600);
}

.progress-line {
  flex: 1;
  height: 2px;
  background: var(--gray-200);
  margin: 0 var(--space-2);
  position: relative;
  top: -19px;
}

.progress-line.completed {
  background: var(--success-500);
}

/* 註冊步驟 */
.register-step {
  display: none;
}

.register-step.active {
  display: block;
}

/* 表單行 */
.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--space-4);
  margin-bottom: var(--space-6);
}

/* 欄位提示 */
.field-hint {
  font-size: var(--text-xs);
  color: var(--gray-500);
  margin-top: var(--space-2);
}

/* 欄位狀態指示器 */
.field-status {
  position: absolute;
  right: var(--space-4);
  top: 50%;
  transform: translateY(-50%);
  width: 20px;
  height: 20px;
}

.field-status.checking {
  animation: spin 1s linear infinite;
}

.field-status.valid::after {
  content: '✓';
  color: var(--success-500);
  font-weight: bold;
}

.field-status.invalid::after {
  content: '✕';
  color: var(--error-500);
  font-weight: bold;
}

/* 密碼強度指示器 */
.password-strength {
  margin-top: var(--space-2);
}

.strength-bar {
  width: 100%;
  height: 4px;
  background: var(--gray-200);
  border-radius: var(--radius-full);
  overflow: hidden;
  margin-bottom: var(--space-2);
}

.strength-fill {
  height: 100%;
  border-radius: var(--radius-full);
  transition: all 0.3s ease;
  width: 0%;
}

.strength-fill.weak {
  width: 25%;
  background: var(--error-400);
}

.strength-fill.fair {
  width: 50%;
  background: var(--warning-400);
}

.strength-fill.good {
  width: 75%;
  background: var(--success-400);
}

.strength-fill.strong {
  width: 100%;
  background: var(--success-500);
}

.strength-text {
  font-size: var(--text-xs);
  color: var(--gray-500);
}

/* 條款區域 */
.terms-section {
  margin-bottom: var(--space-8);
}

.link {
  color: var(--primary-600);
  text-decoration: none;
  font-weight: 500;
}

.link:hover {
  text-decoration: underline;
}

/* 驗證頁面 */
.verification-content {
  text-align: center;
}

.verification-icon {
  width: 80px;
  height: 80px;
  margin: 0 auto var(--space-6);
}

.verification-content h3 {
  font-size: var(--text-2xl);
  font-weight: 700;
  color: var(--gray-900);
  margin-bottom: var(--space-4);
}

.verification-text {
  color: var(--gray-600);
  margin-bottom: var(--space-4);
  line-height: 1.5;
}

.verification-hint {
  color: var(--gray-500);
  font-size: var(--text-sm);
  margin-bottom: var(--space-8);
  line-height: 1.5;
}

.verification-actions {
  display: flex;
  gap: var(--space-4);
  justify-content: center;
  margin-bottom: var(--space-6);
}

.secondary-button {
  display: flex;
  align-items: center;
  gap: var(--space-2);
  padding: var(--space-3) var(--space-5);
  background: var(--gray-100);
  color: var(--gray-700);
  border: 2px solid var(--gray-200);
  border-radius: var(--radius-lg);
  font-size: var(--text-sm);
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.secondary-button:hover {
  background: var(--gray-200);
  border-color: var(--gray-300);
}

.countdown-text {
  font-size: var(--text-xs);
  color: var(--gray-400);
}

/* 成功頁面 */
.success-content {
  text-align: center;
}

.success-icon {
  width: 80px;
  height: 80px;
  margin: 0 auto var(--space-6);
}

.success-content h3 {
  font-size: var(--text-2xl);
  font-weight: 700;
  color: var(--gray-900);
  margin-bottom: var(--space-4);
}

.success-text {
  color: var(--gray-600);
  margin-bottom: var(--space-8);
  line-height: 1.5;
}

.user-info {
  background: var(--gray-50);
  border-radius: var(--radius-lg);
  padding: var(--space-6);
  margin-bottom: var(--space-8);
  text-align: left;
}

.info-item {
  display: flex;
  justify-content: space-between;
  padding: var(--space-2) 0;
  border-bottom: 1px solid var(--gray-200);
}

.info-item:last-child {
  border-bottom: none;
}

.info-label {
  color: var(--gray-600);
  font-size: var(--text-sm);
}

.info-value {
  color: var(--gray-900);
  font-weight: 500;
  font-size: var(--text-sm);
}

.next-steps {
  background: var(--primary-50);
  border-radius: var(--radius-lg);
  padding: var(--space-6);
  margin-bottom: var(--space-8);
  text-align: left;
}

.next-steps h4 {
  color: var(--primary-700);
  font-size: var(--text-base);
  font-weight: 600;
  margin-bottom: var(--space-3);
}

.next-steps ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.next-steps li {
  color: var(--primary-600);
  font-size: var(--text-sm);
  padding: var(--space-1) 0;
  position: relative;
  padding-left: var(--space-5);
}

.next-steps li::before {
  content: '→';
  position: absolute;
  left: 0;
  color: var(--primary-500);
  font-weight: bold;
}

/* 返回登入 */
.back-to-login {
  text-align: center;
  margin-top: var(--space-8);
  padding-top: var(--space-6);
  border-top: 1px solid var(--gray-200);
}

.back-to-login p {
  color: var(--gray-600);
  font-size: var(--text-sm);
  margin: 0;
}

/* 響應式設計 */
@media (max-width: 640px) {
  .register-card {
    padding: var(--space-6);
    max-width: none;
  }

  .form-row {
    grid-template-columns: 1fr;
    gap: var(--space-6);
  }

  .progress-indicator {
    padding: 0;
  }

  .step-label {
    font-size: 10px;
  }

  .verification-actions {
    flex-direction: column;
    align-items: stretch;
  }

  .progress-line {
    margin: 0 var(--space-1);
  }
}
```

## 🎯 使用者管理介面設計

### 管理後台主要介面

```html
<!-- 使用者管理介面 -->
<div class="admin-container">
  <!-- 側邊選單 -->
  <aside class="sidebar">
    <div class="sidebar-header">
      <div class="logo">
        <svg class="logo-icon" viewBox="0 0 24 24">
          <path d="M12 2L2 7V10C2 16 6 20.09 12 22C18 20.09 22 16 22 10V7L12 2Z"/>
        </svg>
        <span class="logo-text">DevAuth</span>
      </div>
    </div>

    <nav class="sidebar-nav">
      <div class="nav-section">
        <div class="nav-title">使用者管理</div>
        <ul class="nav-list">
          <li><a href="/admin/users" class="nav-link active">
            <svg class="nav-icon" viewBox="0 0 20 20">
              <path d="M9 12L11 14L15 10M21 12C21 16.418 16.418 21 12 21S3 16.418 3 12 7.582 3 12 3 21 7.582 21 12Z"/>
            </svg>
            使用者列表
          </a></li>
          <li><a href="/admin/roles" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 20 20">
              <path d="M9 12L11 14L15 10M21 12C21 16.418 16.418 21 12 21S3 16.418 3 12 7.582 3 12 3 21 7.582 21 12Z"/>
            </svg>
            角色權限
          </a></li>
        </ul>
      </div>

      <div class="nav-section">
        <div class="nav-title">安全稽核</div>
        <ul class="nav-list">
          <li><a href="/admin/audit-logs" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 20 20">
              <path d="M9 12L11 14L15 10M21 12C21 16.418 16.418 21 12 21S3 16.418 3 12 7.582 3 12 3 21 7.582 21 12Z"/>
            </svg>
            稽核日誌
          </a></li>
          <li><a href="/admin/security" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 20 20">
              <path d="M9 12L11 14L15 10M21 12C21 16.418 16.418 21 12 21S3 16.418 3 12 7.582 3 12 3 21 7.582 21 12Z"/>
            </svg>
            安全監控
          </a></li>
        </ul>
      </div>
    </nav>
  </aside>

  <!-- 主要內容區域 -->
  <main class="main-content">
    <!-- 頂部欄 -->
    <header class="top-bar">
      <div class="page-title">
        <h1>使用者管理</h1>
        <p class="page-subtitle">管理系統使用者和權限設定</p>
      </div>

      <div class="top-actions">
        <button class="icon-button" title="通知">
          <svg viewBox="0 0 20 20">
            <path d="M10 2C10.5 2 11 2.4 11 3V4.1C13.84 4.56 16 7.03 16 10V14L18 16H2L4 14V10C4 7.03 6.16 4.56 9 4.1V3C9 2.4 9.5 2 10 2Z"/>
          </svg>
        </button>

        <div class="user-menu">
          <button class="user-avatar">
            <img src="/api/users/avatar/current" alt="User Avatar">
            <span class="user-name">管理員</span>
          </button>
        </div>
      </div>
    </header>

    <!-- 內容區域 -->
    <div class="content-area">
      <!-- 操作工具列 -->
      <div class="toolbar">
        <div class="toolbar-left">
          <button class="primary-button">
            <svg class="button-icon" viewBox="0 0 20 20">
              <path d="M10 3V17M3 10H17"/>
            </svg>
            新增使用者
          </button>

          <div class="button-group">
            <button class="secondary-button">匯入</button>
            <button class="secondary-button">匯出</button>
          </div>
        </div>

        <div class="toolbar-right">
          <div class="search-box">
            <svg class="search-icon" viewBox="0 0 20 20">
              <path d="M9 3C5.7 3 3 5.7 3 9S5.7 15 9 15C10.4 15 11.7 14.4 12.7 13.4L16.3 17L17 16.3L13.4 12.7C14.4 11.7 15 10.4 15 9C15 5.7 12.3 3 9 3Z"/>
            </svg>
            <input type="text" placeholder="搜尋使用者..." class="search-input">
          </div>

          <div class="filter-dropdown">
            <button class="filter-button">
              <svg class="button-icon" viewBox="0 0 20 20">
                <path d="M3 4C3 3.45 3.45 3 4 3H16C16.55 3 17 3.45 17 4C17 4.55 16.55 5 16 5H4C3.45 5 3 4.55 3 4ZM6 10C6 9.45 6.45 9 7 9H13C13.55 9 14 9.45 14 10C14 10.55 13.55 11 13 11H7C6.45 11 6 10.55 6 10ZM9 16C9 15.45 9.45 15 10 15H10C10.55 15 11 15.45 11 16C11 16.55 10.55 17 10 17H10C9.45 17 9 16.55 9 16Z"/>
              </svg>
              篩選
            </button>
          </div>
        </div>
      </div>

      <!-- 使用者表格 -->
      <div class="data-table-container">
        <table class="data-table">
          <thead>
            <tr>
              <th class="checkbox-col">
                <input type="checkbox" class="checkbox">
              </th>
              <th class="sortable" data-sort="name">
                使用者
                <svg class="sort-icon" viewBox="0 0 20 20">
                  <path d="M5.5 7.5L10 3L14.5 7.5H5.5ZM14.5 12.5L10 17L5.5 12.5H14.5Z"/>
                </svg>
              </th>
              <th class="sortable" data-sort="email">電子信箱</th>
              <th class="sortable" data-sort="role">角色</th>
              <th class="sortable" data-sort="status">狀態</th>
              <th class="sortable" data-sort="lastLogin">最後登入</th>
              <th class="actions-col">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td><input type="checkbox" class="checkbox"></td>
              <td>
                <div class="user-cell">
                  <img src="/api/users/avatar/1" class="user-avatar-small">
                  <div class="user-info">
                    <div class="user-name">John Doe</div>
                    <div class="user-username">@johndoe</div>
                  </div>
                </div>
              </td>
              <td>john.doe@company.com</td>
              <td><span class="role-badge role-manager">主管</span></td>
              <td><span class="status-badge status-active">啟用</span></td>
              <td>
                <div class="time-info">
                  <div class="time-primary">2小時前</div>
                  <div class="time-secondary">2024-01-15 14:30</div>
                </div>
              </td>
              <td>
                <div class="action-buttons">
                  <button class="icon-button" title="編輯">
                    <svg viewBox="0 0 20 20">
                      <path d="M13.586 3.586C14.367 2.805 15.633 2.805 16.414 3.586C17.195 4.367 17.195 5.633 16.414 6.414L15.621 7.207L12.793 4.379L13.586 3.586ZM11.379 5.793L3 14.172V17H5.828L14.207 8.621L11.379 5.793Z"/>
                    </svg>
                  </button>
                  <button class="icon-button" title="重設密碼">
                    <svg viewBox="0 0 20 20">
                      <path d="M4 8V6C4 3.79 5.79 2 8 2H12C14.21 2 16 3.79 16 6V8H17C17.55 8 18 8.45 18 9V17C18 17.55 17.55 18 17 18H3C2.45 18 2 17.55 2 17V9C2 8.45 2.45 8 3 8H4Z"/>
                    </svg>
                  </button>
                  <button class="icon-button danger" title="停用">
                    <svg viewBox="0 0 20 20">
                      <path d="M10 2C14.42 2 18 5.58 18 10C18 14.42 14.42 18 10 18C5.58 18 2 14.42 2 10C2 5.58 5.58 2 10 2ZM13 7L10 10L7 7L6 8L9 11L6 14L7 15L10 12L13 15L14 14L11 11L14 8L13 7Z"/>
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
            <!-- 更多使用者行... -->
          </tbody>
        </table>
      </div>

      <!-- 分頁 -->
      <div class="pagination-container">
        <div class="pagination-info">
          顯示 1-10 筆，共 156 筆資料
        </div>
        <div class="pagination">
          <button class="page-button" disabled>
            <svg viewBox="0 0 20 20">
              <path d="M12.707 5.293L8 10L12.707 14.707L11.293 16.121L5.172 10L11.293 3.879L12.707 5.293Z"/>
            </svg>
          </button>
          <button class="page-button active">1</button>
          <button class="page-button">2</button>
          <button class="page-button">3</button>
          <span class="page-dots">...</span>
          <button class="page-button">16</button>
          <button class="page-button">
            <svg viewBox="0 0 20 20">
              <path d="M7.293 14.707L12 10L7.293 5.293L8.707 3.879L14.828 10L8.707 16.121L7.293 14.707Z"/>
            </svg>
          </button>
        </div>
      </div>
    </div>
  </main>
</div>
```

這套UI設計系統為軟體開發團隊提供了專業、高效且用戶友善的身份認證體驗！