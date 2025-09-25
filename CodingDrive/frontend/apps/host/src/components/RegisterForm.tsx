import React, { useState, useEffect } from 'react';

interface RegisterFormProps {
  onSuccess?: (data: any) => void;
  onError?: (error: string) => void;
}

interface FormData {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  agreeTerms: boolean;
}

interface PasswordStrength {
  score: number;
  text: string;
  color: string;
}

export const RegisterForm: React.FC<RegisterFormProps> = ({ onSuccess, onError }) => {
  const [currentStep, setCurrentStep] = useState(1);
  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    agreeTerms: false
  });

  const [passwordStrength, setPasswordStrength] = useState<PasswordStrength>({
    score: 0,
    text: '請輸入密碼',
    color: '#d1d5db'
  });

  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [usernameStatus, setUsernameStatus] = useState<'checking' | 'available' | 'taken' | 'idle'>('idle');
  const [emailStatus, setEmailStatus] = useState<'checking' | 'available' | 'taken' | 'idle'>('idle');
  const [errors, setErrors] = useState<Partial<FormData>>({});

  // 計算密碼強度
  const calculatePasswordStrength = (password: string): PasswordStrength => {
    if (!password) return { score: 0, text: '請輸入密碼', color: '#d1d5db' };

    let score = 0;
    const checks = {
      length: password.length >= 8,
      lowercase: /[a-z]/.test(password),
      uppercase: /[A-Z]/.test(password),
      number: /\d/.test(password),
      special: /[!@#$%^&*(),.?":{}|<>]/.test(password)
    };

    Object.values(checks).forEach(check => check && score++);

    if (score < 2) return { score: 1, text: '密碼強度：弱', color: '#ef4444' };
    if (score < 4) return { score: 2, text: '密碼強度：中等', color: '#f59e0b' };
    return { score: 3, text: '密碼強度：強', color: '#10b981' };
  };

  // 檢查用戶名稱可用性（模擬）
  const checkUsernameAvailability = async (username: string) => {
    if (username.length < 3) {
      setUsernameStatus('idle');
      return;
    }

    setUsernameStatus('checking');

    // 模擬API檢查
    setTimeout(() => {
      const taken = ['admin', 'user', 'test', 'demo'].includes(username.toLowerCase());
      setUsernameStatus(taken ? 'taken' : 'available');
    }, 500);
  };

  // 檢查信箱可用性（模擬）
  const checkEmailAvailability = async (email: string) => {
    if (!email || !email.includes('@')) {
      setEmailStatus('idle');
      return;
    }

    setEmailStatus('checking');

    // 模擬API檢查
    setTimeout(() => {
      const taken = ['test@example.com', 'admin@example.com'].includes(email.toLowerCase());
      setEmailStatus(taken ? 'taken' : 'available');
    }, 500);
  };

  // 表單輸入處理
  const handleInputChange = (field: keyof FormData, value: string | boolean) => {
    setFormData(prev => ({ ...prev, [field]: value }));

    // 清除該欄位的錯誤
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: undefined }));
    }

    // 特殊處理
    if (field === 'password') {
      setPasswordStrength(calculatePasswordStrength(value as string));
    }
    if (field === 'username') {
      checkUsernameAvailability(value as string);
    }
    if (field === 'email') {
      checkEmailAvailability(value as string);
    }
  };

  // 表單驗證
  const validateStep1 = (): boolean => {
    const newErrors: Partial<FormData> = {};

    if (!formData.firstName.trim()) newErrors.firstName = '請輸入名字';
    if (!formData.lastName.trim()) newErrors.lastName = '請輸入姓氏';

    if (!formData.username.trim()) {
      newErrors.username = '請輸入使用者名稱';
    } else if (formData.username.length < 3) {
      newErrors.username = '使用者名稱至少需要3個字元';
    } else if (usernameStatus === 'taken') {
      newErrors.username = '此使用者名稱已被使用';
    }

    if (!formData.email.trim()) {
      newErrors.email = '請輸入電子信箱';
    } else if (emailStatus === 'taken') {
      newErrors.email = '此信箱已被註冊';
    }

    if (!formData.password) {
      newErrors.password = '請輸入密碼';
    } else if (formData.password.length < 8) {
      newErrors.password = '密碼至少需要8個字元';
    }

    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = '確認密碼不符合';
    }

    if (!formData.agreeTerms) {
      newErrors.agreeTerms = true;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // 提交步驟1
  const handleStep1Submit = async () => {
    if (!validateStep1()) return;

    setIsLoading(true);

    try {
      const response = await fetch('http://localhost:5555/api/Users/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: formData.username,
          email: formData.email,
          firstName: formData.firstName,
          lastName: formData.lastName,
          password: formData.password,
          confirmPassword: formData.confirmPassword,
        }),
      });

      if (response.ok) {
        const result = await response.json();
        console.log('註冊API回應:', result);
        setCurrentStep(2); // 進入驗證步驟
        if (onSuccess) onSuccess(result);
      } else {
        const error = await response.json();
        throw new Error(error.message || '註冊失敗');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : '註冊失敗';
      if (onError) onError(errorMessage);
      alert(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  // 進度指示器
  const renderProgressIndicator = () => (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      marginBottom: '2rem'
    }}>
      {[1, 2, 3].map((step, index) => (
        <React.Fragment key={step}>
          <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            opacity: step <= currentStep ? 1 : 0.5
          }}>
            <div style={{
              width: '40px',
              height: '40px',
              borderRadius: '50%',
              backgroundColor: step <= currentStep ? '#3b82f6' : '#e5e7eb',
              color: 'white',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontWeight: '600',
              marginBottom: '0.5rem'
            }}>
              {step}
            </div>
            <div style={{
              fontSize: '0.875rem',
              color: step <= currentStep ? '#374151' : '#9ca3af'
            }}>
              {['基本資料', '驗證信箱', '完成註冊'][step - 1]}
            </div>
          </div>
          {index < 2 && (
            <div style={{
              width: '60px',
              height: '2px',
              backgroundColor: step < currentStep ? '#3b82f6' : '#e5e7eb',
              margin: '0 1rem',
              marginBottom: '1.5rem'
            }} />
          )}
        </React.Fragment>
      ))}
    </div>
  );

  // 密碼強度指示器
  const renderPasswordStrength = () => (
    <div style={{ marginTop: '0.5rem' }}>
      <div style={{
        height: '4px',
        backgroundColor: '#e5e7eb',
        borderRadius: '2px',
        overflow: 'hidden'
      }}>
        <div style={{
          height: '100%',
          width: `${(passwordStrength.score / 3) * 100}%`,
          backgroundColor: passwordStrength.color,
          transition: 'all 0.3s ease'
        }} />
      </div>
      <div style={{
        fontSize: '0.875rem',
        color: passwordStrength.color,
        marginTop: '0.25rem'
      }}>
        {passwordStrength.text}
      </div>
    </div>
  );

  // 欄位狀態指示器
  const getFieldStatusIcon = (status: typeof usernameStatus) => {
    switch (status) {
      case 'checking':
        return <div style={{ color: '#6b7280' }}>檢查中...</div>;
      case 'available':
        return <div style={{ color: '#10b981' }}>✓ 可使用</div>;
      case 'taken':
        return <div style={{ color: '#ef4444' }}>✗ 已被使用</div>;
      default:
        return null;
    }
  };

  return (
    <div style={{
      maxWidth: '500px',
      margin: '0 auto',
      padding: '2rem',
      background: 'white',
      borderRadius: '12px',
      boxShadow: '0 10px 25px rgba(0, 0, 0, 0.1)'
    }}>
      {/* Logo區域 */}
      <div style={{ textAlign: 'center', marginBottom: '2rem' }}>
        <div style={{
          display: 'inline-flex',
          alignItems: 'center',
          gap: '0.75rem',
          marginBottom: '1rem'
        }}>
          <div style={{
            width: '40px',
            height: '40px',
            backgroundColor: '#3b82f6',
            borderRadius: '8px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: 'white',
            fontWeight: 'bold',
            fontSize: '1.5rem'
          }}>
            D
          </div>
          <h1 style={{
            fontSize: '1.875rem',
            fontWeight: 'bold',
            color: '#1f2937',
            margin: 0
          }}>
            DevAuth
          </h1>
        </div>
        <p style={{
          color: '#6b7280',
          fontSize: '1rem',
          margin: 0
        }}>
          建立您的開發者帳號
        </p>
      </div>

      {/* 進度指示器 */}
      {renderProgressIndicator()}

      {/* 步驟1: 基本資料 */}
      {currentStep === 1 && (
        <form onSubmit={(e) => { e.preventDefault(); handleStep1Submit(); }}>
          {/* 姓名 */}
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
            <div>
              <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
                名字 *
              </label>
              <input
                type="text"
                value={formData.firstName}
                onChange={(e) => handleInputChange('firstName', e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: `1px solid ${errors.firstName ? '#ef4444' : '#d1d5db'}`,
                  borderRadius: '6px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s'
                }}
                placeholder="請輸入您的名字"
              />
              {errors.firstName && (
                <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                  {errors.firstName}
                </div>
              )}
            </div>

            <div>
              <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
                姓氏 *
              </label>
              <input
                type="text"
                value={formData.lastName}
                onChange={(e) => handleInputChange('lastName', e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: `1px solid ${errors.lastName ? '#ef4444' : '#d1d5db'}`,
                  borderRadius: '6px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s'
                }}
                placeholder="請輸入您的姓氏"
              />
              {errors.lastName && (
                <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                  {errors.lastName}
                </div>
              )}
            </div>
          </div>

          {/* 使用者名稱 */}
          <div style={{ marginBottom: '1rem' }}>
            <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
              使用者名稱 *
            </label>
            <input
              type="text"
              value={formData.username}
              onChange={(e) => handleInputChange('username', e.target.value)}
              style={{
                width: '100%',
                padding: '0.75rem',
                border: `1px solid ${errors.username ? '#ef4444' : '#d1d5db'}`,
                borderRadius: '6px',
                fontSize: '1rem',
                outline: 'none',
                transition: 'border-color 0.2s'
              }}
              placeholder="選擇一個唯一的使用者名稱"
            />
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '0.25rem' }}>
              <div style={{ fontSize: '0.875rem', color: '#6b7280' }}>
                3-20個字元，只能包含字母、數字和下底線
              </div>
              {getFieldStatusIcon(usernameStatus)}
            </div>
            {errors.username && (
              <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                {errors.username}
              </div>
            )}
          </div>

          {/* 電子信箱 */}
          <div style={{ marginBottom: '1rem' }}>
            <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
              電子信箱 *
            </label>
            <input
              type="email"
              value={formData.email}
              onChange={(e) => handleInputChange('email', e.target.value)}
              style={{
                width: '100%',
                padding: '0.75rem',
                border: `1px solid ${errors.email ? '#ef4444' : '#d1d5db'}`,
                borderRadius: '6px',
                fontSize: '1rem',
                outline: 'none',
                transition: 'border-color 0.2s'
              }}
              placeholder="example@company.com"
            />
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '0.25rem' }}>
              <div style={{ fontSize: '0.875rem', color: '#6b7280' }}>
                我們將發送驗證信件到此信箱
              </div>
              {getFieldStatusIcon(emailStatus)}
            </div>
            {errors.email && (
              <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                {errors.email}
              </div>
            )}
          </div>

          {/* 密碼 */}
          <div style={{ marginBottom: '1rem' }}>
            <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
              密碼 *
            </label>
            <div style={{ position: 'relative' }}>
              <input
                type={showPassword ? 'text' : 'password'}
                value={formData.password}
                onChange={(e) => handleInputChange('password', e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  paddingRight: '3rem',
                  border: `1px solid ${errors.password ? '#ef4444' : '#d1d5db'}`,
                  borderRadius: '6px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s'
                }}
                placeholder="建立一個強密碼"
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                style={{
                  position: 'absolute',
                  right: '0.75rem',
                  top: '50%',
                  transform: 'translateY(-50%)',
                  background: 'none',
                  border: 'none',
                  color: '#6b7280',
                  cursor: 'pointer'
                }}
              >
                {showPassword ? '🙈' : '👁️'}
              </button>
            </div>
            {renderPasswordStrength()}
            {errors.password && (
              <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                {errors.password}
              </div>
            )}
          </div>

          {/* 確認密碼 */}
          <div style={{ marginBottom: '1.5rem' }}>
            <label style={{ display: 'block', fontWeight: '500', marginBottom: '0.5rem', color: '#374151' }}>
              確認密碼 *
            </label>
            <input
              type="password"
              value={formData.confirmPassword}
              onChange={(e) => handleInputChange('confirmPassword', e.target.value)}
              style={{
                width: '100%',
                padding: '0.75rem',
                border: `1px solid ${errors.confirmPassword ? '#ef4444' : '#d1d5db'}`,
                borderRadius: '6px',
                fontSize: '1rem',
                outline: 'none',
                transition: 'border-color 0.2s'
              }}
              placeholder="再次輸入密碼"
            />
            {errors.confirmPassword && (
              <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.25rem' }}>
                {errors.confirmPassword}
              </div>
            )}
          </div>

          {/* 服務條款 */}
          <div style={{ marginBottom: '2rem' }}>
            <label style={{ display: 'flex', alignItems: 'flex-start', gap: '0.75rem', cursor: 'pointer' }}>
              <input
                type="checkbox"
                checked={formData.agreeTerms}
                onChange={(e) => handleInputChange('agreeTerms', e.target.checked)}
                style={{ marginTop: '0.125rem' }}
              />
              <span style={{ fontSize: '0.875rem', color: '#374151', lineHeight: '1.5' }}>
                我同意 <a href="#" style={{ color: '#3b82f6' }}>服務條款</a> 和 <a href="#" style={{ color: '#3b82f6' }}>隱私政策</a>
              </span>
            </label>
            {errors.agreeTerms && (
              <div style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.5rem' }}>
                請同意服務條款和隱私政策
              </div>
            )}
          </div>

          {/* 提交按鈕 */}
          <button
            type="submit"
            disabled={isLoading || usernameStatus === 'taken' || emailStatus === 'taken'}
            style={{
              width: '100%',
              padding: '0.875rem 1.5rem',
              backgroundColor: isLoading ? '#9ca3af' : '#3b82f6',
              color: 'white',
              border: 'none',
              borderRadius: '6px',
              fontSize: '1rem',
              fontWeight: '600',
              cursor: isLoading ? 'not-allowed' : 'pointer',
              transition: 'background-color 0.2s',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '0.5rem'
            }}
          >
            {isLoading ? '註冊中...' : '繼續'}
            {!isLoading && <span>→</span>}
          </button>
        </form>
      )}

      {/* 步驟2: 信箱驗證 */}
      {currentStep === 2 && (
        <div style={{ textAlign: 'center', padding: '2rem 0' }}>
          <div style={{
            width: '80px',
            height: '80px',
            backgroundColor: '#dbeafe',
            borderRadius: '50%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            margin: '0 auto 2rem',
            fontSize: '2rem'
          }}>
            📧
          </div>
          <h3 style={{ fontSize: '1.5rem', fontWeight: '600', color: '#1f2937', marginBottom: '1rem' }}>
            驗證您的信箱
          </h3>
          <p style={{ color: '#6b7280', marginBottom: '0.5rem' }}>
            我們已發送驗證信件到
          </p>
          <p style={{ fontWeight: '600', color: '#1f2937', marginBottom: '1rem' }}>
            {formData.email}
          </p>
          <p style={{ color: '#6b7280', fontSize: '0.875rem', lineHeight: '1.5', marginBottom: '2rem' }}>
            請檢查您的收件匣（包括垃圾信件匣），並點擊信件中的驗證連結。
          </p>

          <div style={{ display: 'flex', gap: '1rem', justifyContent: 'center', marginBottom: '1rem' }}>
            <button
              type="button"
              style={{
                padding: '0.75rem 1.5rem',
                backgroundColor: '#f3f4f6',
                color: '#374151',
                border: 'none',
                borderRadius: '6px',
                fontSize: '0.875rem',
                cursor: 'pointer'
              }}
            >
              重新發送
            </button>
            <button
              type="button"
              onClick={() => setCurrentStep(1)}
              style={{
                padding: '0.75rem 1.5rem',
                backgroundColor: '#f3f4f6',
                color: '#374151',
                border: 'none',
                borderRadius: '6px',
                fontSize: '0.875rem',
                cursor: 'pointer'
              }}
            >
              更改信箱
            </button>
          </div>

          <button
            type="button"
            onClick={() => setCurrentStep(3)} // 模擬驗證成功
            style={{
              backgroundColor: '#10b981',
              color: 'white',
              border: 'none',
              padding: '0.5rem 1rem',
              borderRadius: '4px',
              fontSize: '0.875rem',
              cursor: 'pointer'
            }}
          >
            模擬驗證成功 (測試用)
          </button>
        </div>
      )}

      {/* 步驟3: 完成註冊 */}
      {currentStep === 3 && (
        <div style={{ textAlign: 'center', padding: '2rem 0' }}>
          <div style={{
            width: '80px',
            height: '80px',
            backgroundColor: '#dcfce7',
            borderRadius: '50%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            margin: '0 auto 2rem',
            fontSize: '2rem'
          }}>
            ✅
          </div>
          <h3 style={{ fontSize: '1.5rem', fontWeight: '600', color: '#1f2937', marginBottom: '1rem' }}>
            註冊成功！
          </h3>
          <p style={{ color: '#6b7280', marginBottom: '2rem', lineHeight: '1.5' }}>
            歡迎加入 DevAuth 開發者社群！<br />
            您的帳號已成功建立並通過驗證。
          </p>

          <div style={{
            backgroundColor: '#f9fafb',
            border: '1px solid #e5e7eb',
            borderRadius: '8px',
            padding: '1.5rem',
            marginBottom: '2rem',
            textAlign: 'left'
          }}>
            <div style={{ marginBottom: '0.5rem' }}>
              <span style={{ color: '#6b7280', fontSize: '0.875rem' }}>使用者名稱：</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>{formData.username}</span>
            </div>
            <div style={{ marginBottom: '0.5rem' }}>
              <span style={{ color: '#6b7280', fontSize: '0.875rem' }}>電子信箱：</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>{formData.email}</span>
            </div>
            <div>
              <span style={{ color: '#6b7280', fontSize: '0.875rem' }}>角色權限：</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>訪客 (Guest)</span>
            </div>
          </div>

          <button
            type="button"
            onClick={() => alert('跳轉到登入頁面')}
            style={{
              width: '100%',
              padding: '0.875rem 1.5rem',
              backgroundColor: '#3b82f6',
              color: 'white',
              border: 'none',
              borderRadius: '6px',
              fontSize: '1rem',
              fontWeight: '600',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '0.5rem'
            }}
          >
            前往登入 →
          </button>
        </div>
      )}

      {/* 返回登入 */}
      <div style={{ textAlign: 'center', marginTop: '1.5rem', paddingTop: '1.5rem', borderTop: '1px solid #e5e7eb' }}>
        <p style={{ color: '#6b7280', fontSize: '0.875rem' }}>
          已經有帳號了？
          <a href="#" style={{ color: '#3b82f6', textDecoration: 'none', marginLeft: '0.25rem' }}>
            立即登入
          </a>
        </p>
      </div>
    </div>
  );
};