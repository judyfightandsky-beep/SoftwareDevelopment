document.addEventListener('DOMContentLoaded', function() {
    // 註冊步驟管理
    let currentStep = 1;
    const maxSteps = 3;

    // DOM 元素
    const progressSteps = document.querySelectorAll('.progress-step');
    const registerSteps = document.querySelectorAll('.register-step');
    const basicInfoForm = document.getElementById('basicInfoForm');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const usernameInput = document.getElementById('username');
    const emailInput = document.getElementById('email');
    const messageContainer = document.getElementById('messageContainer');

    // 密碼強度檢查
    const strengthFill = document.getElementById('strengthFill');
    const strengthText = document.getElementById('strengthText');

    // 驗證相關元素
    const verificationEmail = document.getElementById('verificationEmail');
    const resendButton = document.getElementById('resendEmail');
    const changeEmailButton = document.getElementById('changeEmail');
    const resendCountdown = document.getElementById('resendCountdown');

    // 完成註冊相關元素
    const finalUsername = document.getElementById('finalUsername');
    const finalEmail = document.getElementById('finalEmail');

    // 密碼顯示/隱藏切換
    const passwordToggle = document.querySelector('.password-toggle');
    if (passwordToggle) {
        passwordToggle.addEventListener('click', function() {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);

            const icon = this.querySelector('.toggle-icon path');
            if (type === 'text') {
                icon.setAttribute('d', 'M2.458 10.458A.5.5 0 0 1 3 10L10 3L17 10a.5.5 0 0 1-.708.708L10 4.414L3.708 10.708a.5.5 0 0 1-.25.25z');
            } else {
                icon.setAttribute('d', 'M10 4C5 4 1.73 7.11 1 10C1.73 12.89 5 16 10 16S18.27 12.89 19 10C18.27 7.11 15 4 10 4Z');
            }
        });
    }

    // 使用者名稱驗證
    usernameInput.addEventListener('input', debounce(function() {
        validateUsername(this.value);
    }, 500));

    // 電子信箱驗證
    emailInput.addEventListener('input', debounce(function() {
        validateEmail(this.value);
    }, 500));

    // 密碼強度檢查
    passwordInput.addEventListener('input', function() {
        checkPasswordStrength(this.value);
    });

    // 確認密碼檢查
    confirmPasswordInput.addEventListener('input', function() {
        validatePasswordMatch();
    });

    // 基本資料表單提交
    basicInfoForm.addEventListener('submit', function(e) {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        // 模擬註冊請求
        showMessage('正在建立帳號...', 'info');

        setTimeout(() => {
            // 設定驗證信箱
            verificationEmail.textContent = emailInput.value;

            // 進入下一步
            nextStep();

            showMessage('驗證信件已發送', 'success');

            // 啟動重發倒計時
            startResendCountdown();
        }, 2000);
    });

    // 重發驗證信件
    resendButton.addEventListener('click', function() {
        if (this.disabled) return;

        showMessage('正在重新發送驗證信件...', 'info');
        setTimeout(() => {
            showMessage('驗證信件已重新發送', 'success');
            startResendCountdown();
        }, 1000);
    });

    // 更改信箱
    changeEmailButton.addEventListener('click', function() {
        previousStep();
    });


    // 使用者名稱驗證
    function validateUsername(username) {
        const statusElement = document.getElementById('usernameStatus');

        if (!username) {
            statusElement.textContent = '';
            statusElement.className = 'field-status';
            return;
        }

        if (username.length < 3) {
            showFieldStatus(statusElement, '使用者名稱至少需要3個字元', 'error');
            return;
        }

        if (!/^[a-zA-Z0-9_]+$/.test(username)) {
            showFieldStatus(statusElement, '只能包含字母、數字和下底線', 'error');
            return;
        }

        // 模擬檢查使用者名稱是否已存在
        setTimeout(() => {
            if (username === 'admin' || username === 'test') {
                showFieldStatus(statusElement, '此使用者名稱已被使用', 'error');
            } else {
                showFieldStatus(statusElement, '使用者名稱可以使用', 'success');
            }
        }, 500);
    }

    // 電子信箱驗證
    function validateEmail(email) {
        const statusElement = document.getElementById('emailStatus');

        if (!email) {
            statusElement.textContent = '';
            statusElement.className = 'field-status';
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            showFieldStatus(statusElement, '請輸入有效的電子信箱格式', 'error');
            return;
        }

        // 模擬檢查信箱是否已被註冊
        setTimeout(() => {
            if (email === 'admin@test.com' || email === 'test@test.com') {
                showFieldStatus(statusElement, '此信箱已被註冊', 'error');
            } else {
                showFieldStatus(statusElement, '信箱可以使用', 'success');
            }
        }, 500);
    }

    // 密碼強度檢查
    function checkPasswordStrength(password) {
        let strength = 0;
        let strengthText = '';

        if (password.length >= 8) strength++;
        if (/[a-z]/.test(password)) strength++;
        if (/[A-Z]/.test(password)) strength++;
        if (/[0-9]/.test(password)) strength++;
        if (/[^a-zA-Z0-9]/.test(password)) strength++;

        const strengthPercentage = (strength / 5) * 100;
        strengthFill.style.width = strengthPercentage + '%';

        switch (strength) {
            case 0:
            case 1:
                strengthFill.className = 'strength-fill weak';
                strengthText = '密碼強度：弱';
                break;
            case 2:
            case 3:
                strengthFill.className = 'strength-fill medium';
                strengthText = '密碼強度：中等';
                break;
            case 4:
            case 5:
                strengthFill.className = 'strength-fill strong';
                strengthText = '密碼強度：強';
                break;
        }

        document.getElementById('strengthText').textContent = strengthText;
    }

    // 確認密碼驗證
    function validatePasswordMatch() {
        const statusElement = document.getElementById('confirmPasswordStatus');
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        if (!confirmPassword) {
            statusElement.textContent = '';
            statusElement.className = 'field-status';
            return;
        }

        if (password !== confirmPassword) {
            showFieldStatus(statusElement, '密碼不一致', 'error');
        } else {
            showFieldStatus(statusElement, '密碼一致', 'success');
        }
    }

    // 表單驗證
    function validateForm() {
        const formData = new FormData(basicInfoForm);
        const firstName = formData.get('firstName');
        const lastName = formData.get('lastName');
        const username = formData.get('username');
        const email = formData.get('email');
        const password = formData.get('password');
        const confirmPassword = formData.get('confirmPassword');
        const agreeTerms = formData.get('agreeTerms');

        if (!firstName || !lastName) {
            showMessage('請填寫完整的姓名', 'error');
            return false;
        }

        if (!username || username.length < 3) {
            showMessage('請輸入有效的使用者名稱', 'error');
            return false;
        }

        if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            showMessage('請輸入有效的電子信箱', 'error');
            return false;
        }

        if (!password || password.length < 8) {
            showMessage('密碼至少需要8個字元', 'error');
            return false;
        }

        if (password !== confirmPassword) {
            showMessage('密碼確認不一致', 'error');
            return false;
        }

        if (!agreeTerms) {
            showMessage('請同意服務條款和隱私政策', 'error');
            return false;
        }

        return true;
    }

    // 進入下一步
    function nextStep() {
        if (currentStep < maxSteps) {
            // 隱藏當前步驟
            document.querySelector(`.register-step[data-step="${currentStep}"]`).classList.remove('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.remove('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.add('completed');

            currentStep++;

            // 顯示下一步驟
            document.querySelector(`.register-step[data-step="${currentStep}"]`).classList.add('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.add('active');


            // 如果是最後一步，設定用戶資訊
            if (currentStep === 3) {
                finalUsername.textContent = usernameInput.value;
                finalEmail.textContent = emailInput.value;

                // 模擬驗證完成
                setTimeout(() => {
                    showMessage('帳號驗證成功！', 'success');
                }, 1000);
            }
        }
    }

    // 回到上一步
    function previousStep() {
        if (currentStep > 1) {
            // 隱藏當前步驟
            document.querySelector(`.register-step[data-step="${currentStep}"]`).classList.remove('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.remove('active');

            currentStep--;

            // 顯示上一步驟
            document.querySelector(`.register-step[data-step="${currentStep}"]`).classList.add('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.add('active');
            document.querySelector(`.progress-step[data-step="${currentStep}"]`).classList.remove('completed');

        }
    }


    // 重發倒計時
    function startResendCountdown() {
        let countdown = 30;
        resendButton.disabled = true;
        resendButton.style.opacity = '0.5';

        const timer = setInterval(() => {
            resendCountdown.textContent = `${countdown}秒後可重新發送`;
            countdown--;

            if (countdown < 0) {
                clearInterval(timer);
                resendCountdown.textContent = '';
                resendButton.disabled = false;
                resendButton.style.opacity = '1';
            }
        }, 1000);
    }

    // 顯示欄位狀態
    function showFieldStatus(element, message, type) {
        element.textContent = message;
        element.className = `field-status ${type}`;
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
            if (messageElement.parentNode) {
                messageElement.remove();
            }
        }, 5000);

        // 手動關閉
        messageElement.querySelector('.message-close').addEventListener('click', () => {
            messageElement.remove();
        });
    }

    // 防抖函數
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // 模擬信箱驗證完成（用於測試）
    setTimeout(() => {
        if (currentStep === 2) {
            // 可以添加測試按鈕來觸發驗證完成
            console.log('Email verification simulation ready');
        }
    }, 5000);
});