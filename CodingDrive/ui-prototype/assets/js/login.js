document.addEventListener('DOMContentLoaded', function() {
    // DOM 元素
    const tabButtons = document.querySelectorAll('.tab-button');
    const tabPanels = document.querySelectorAll('.tab-panel');
    const loginForm = document.getElementById('loginForm');
    const passwordToggle = document.querySelector('.password-toggle');
    const passwordInput = document.getElementById('password');
    const qrTimer = document.getElementById('qrTimer');
    const messageContainer = document.getElementById('messageContainer');
    const verifyCodeBtn = document.getElementById('verifyCodeBtn');
    const qrVerificationCode = document.getElementById('qrVerificationCode');

    // Tab 切換功能
    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            const tabType = this.dataset.tab;

            // 移除所有active狀態
            tabButtons.forEach(btn => btn.classList.remove('active'));
            tabPanels.forEach(panel => panel.classList.remove('active'));

            // 添加active狀態到當前選中項
            this.classList.add('active');
            document.querySelector(`[data-panel="${tabType}"]`).classList.add('active');

            // 如果是QR碼登入，生成QR碼
            if (tabType === 'qrcode') {
                generateQRCode();
            }
        });
    });

    // 密碼顯示/隱藏
    if (passwordToggle) {
        passwordToggle.addEventListener('click', function() {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);

            // 切換圖示
            const icon = this.querySelector('.toggle-icon path');
            if (type === 'text') {
                icon.setAttribute('d', 'M2.458 10.458A.5.5 0 0 1 3 10L10 3L17 10a.5.5 0 0 1-.708.708L10 4.414L3.708 10.708a.5.5 0 0 1-.25.25z');
            } else {
                icon.setAttribute('d', 'M10 4C5 4 1.73 7.11 1 10C1.73 12.89 5 16 10 16S18.27 12.89 19 10C18.27 7.11 15 4 10 4Z');
            }
        });
    }

    // 登入表單提交
    loginForm.addEventListener('submit', function(e) {
        e.preventDefault();

        const formData = new FormData(this);
        const identifier = formData.get('identifier');
        const password = formData.get('password');

        if (!identifier || !password) {
            showMessage('請填寫完整的登入資訊', 'error');
            return;
        }

        // 模擬登入請求
        showMessage('正在登入...', 'info');

        setTimeout(() => {
            // 模擬登入成功
            if (identifier === 'admin' && password === 'admin123') {
                showMessage('登入成功！即將跳轉...', 'success');
                setTimeout(() => {
                    window.location.href = 'admin.html';
                }, 1500);
            } else {
                showMessage('帳號或密碼錯誤', 'error');
            }
        }, 1500);
    });

    // QR碼驗證功能
    if (verifyCodeBtn) {
        verifyCodeBtn.addEventListener('click', function() {
            const inputCode = qrVerificationCode.value.trim();

            if (!inputCode) {
                showMessage('請輸入驗證碼', 'error');
                return;
            }

            if (inputCode.length !== 6) {
                showMessage('驗證碼必須為6位數', 'error');
                return;
            }

            if (inputCode === window.currentVerificationCode) {
                showMessage('QR碼驗證成功！即將跳轉...', 'success');
                setTimeout(() => {
                    window.location.href = 'admin.html';
                }, 1500);
            } else {
                showMessage('驗證碼錯誤，請重新確認', 'error');
                qrVerificationCode.value = '';
                qrVerificationCode.focus();
            }
        });
    }

    // 驗證碼輸入框格式化
    if (qrVerificationCode) {
        qrVerificationCode.addEventListener('input', function(e) {
            // 只允許數字
            this.value = this.value.replace(/[^0-9]/g, '');

            // 限制長度
            if (this.value.length > 6) {
                this.value = this.value.slice(0, 6);
            }

            // 如果輸入6位數字，自動觸發驗證
            if (this.value.length === 6) {
                setTimeout(() => {
                    verifyCodeBtn.click();
                }, 300);
            }
        });

        // Enter鍵觸發驗證
        qrVerificationCode.addEventListener('keypress', function(e) {
            if (e.key === 'Enter' && this.value.length === 6) {
                verifyCodeBtn.click();
            }
        });
    }

    // QR碼生成功能
    function generateQRCode() {
        const qrContainer = document.getElementById('qrCodeImage');
        const verificationSection = document.getElementById('verificationSection');

        // 生成驗證碼
        const verificationCode = generateVerificationCode();
        const verificationUrl = `https://devauth.example.com/verify?code=${verificationCode}`;

        // 模擬QR碼生成
        setTimeout(() => {
            qrContainer.innerHTML = `
                <div class="qr-code-image">
                    <svg viewBox="0 0 100 100" class="qr-svg">
                        <rect x="10" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="10" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="20" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="20" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="20" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="20" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="20" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="40" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="60" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="30" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="40" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="40" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="40" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="40" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="40" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="60" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="50" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="40" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="60" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="60" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="70" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="30" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="40" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="60" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="80" width="8" height="8" fill="#243b53"/>
                        <rect x="10" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="20" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="40" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="50" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="70" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="80" y="90" width="8" height="8" fill="#243b53"/>
                        <rect x="90" y="90" width="8" height="8" fill="#243b53"/>
                        <!-- 添加文字標示 -->
                        <text x="50" y="95" text-anchor="middle" font-size="3" fill="#243b53">掃我登入</text>
                    </svg>
                    <div class="qr-url-info">
                        <p>掃碼將顯示網址：</p>
                        <code>${verificationUrl}</code>
                        <p>驗證碼：<strong>${verificationCode}</strong></p>
                    </div>
                </div>
            `;

            // 顯示驗證碼輸入區域
            verificationSection.style.display = 'block';

            // 設定驗證碼到全域變量供驗證使用
            window.currentVerificationCode = verificationCode;

            // 啟動倒計時
            startQRTimer();
        }, 1000);
    }

    // 生成6位數驗證碼
    function generateVerificationCode() {
        return Math.floor(100000 + Math.random() * 900000).toString();
    }

    // QR碼倒計時
    function startQRTimer() {
        let timeLeft = 300; // 5分鐘

        const timer = setInterval(() => {
            const minutes = Math.floor(timeLeft / 60);
            const seconds = timeLeft % 60;

            if (qrTimer) {
                qrTimer.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
            }

            timeLeft--;

            if (timeLeft < 0) {
                clearInterval(timer);
                // QR碼過期，重新生成
                generateQRCode();
            }
        }, 1000);
    }

    // 顯示訊息
    function showMessage(message, type = 'info') {
        if (!messageContainer) return;

        const messageElement = document.createElement('div');
        messageElement.className = `message message-${type}`;

        let icon = '';
        switch(type) {
            case 'success':
                icon = '<path d="M9 12L11 14L15 10"/>';
                break;
            case 'error':
                icon = '<path d="M6 18L18 6M6 6L18 18"/>';
                break;
            case 'info':
                icon = '<path d="M12 8V16M12 4H12.01"/>';
                break;
        }

        messageElement.innerHTML = `
            <div class="message-icon">
                <svg viewBox="0 0 24 24">
                    ${icon}
                </svg>
            </div>
            <span class="message-text">${message}</span>
            <button class="message-close">
                <svg viewBox="0 0 24 24">
                    <path d="M6 18L18 6M6 6L18 18"/>
                </svg>
            </button>
        `;

        messageContainer.appendChild(messageElement);

        // 自動關閉
        setTimeout(() => {
            messageElement.remove();
        }, 5000);

        // 手動關閉
        messageElement.querySelector('.message-close').addEventListener('click', () => {
            messageElement.remove();
        });
    }
});