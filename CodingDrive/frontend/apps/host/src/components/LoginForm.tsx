import React, { useState, useCallback } from 'react';
import {
  LoginRequest,
  LoginResponse,
  AuthErrorResponse
} from '../types/auth';
import { useAuth } from '../contexts/AuthContext';

interface LoginFormProps {
  onPasswordResetRequest?: () => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({
  onPasswordResetRequest
}) => {
  const { login, authError, isLoading: contextLoading } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<{ email?: string; password?: string }>({});

  const validateForm = useCallback((): boolean => {
    const newErrors: { email?: string; password?: string } = {};

    if (!email.trim()) {
      newErrors.email = '請輸入電子郵件';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = '請輸入有效的電子郵件格式';
    }

    if (!password) {
      newErrors.password = '請輸入密碼';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [email, password]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;

    setIsLoading(true);

    try {
      await login(email, password);
    } catch (error) {
      console.error('登入失敗:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const loginInProgress = isLoading || contextLoading;

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
          登入您的開發者帳號
        </p>
      </div>

      <form onSubmit={handleSubmit}>
          {authError && (
            <div style={{
              backgroundColor: '#fecaca',
              color: '#7f1d1d',
              padding: '0.75rem',
              borderRadius: '6px',
              marginBottom: '1rem',
              display: 'flex',
              alignItems: 'center',
              gap: '0.75rem'
            }}>
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style={{ width: '1.25rem', height: '1.25rem' }}>
                <path fillRule="evenodd" d="M2.25 12c0-5.385 4.365-9.75 9.75-9.75s9.75 4.365 9.75 9.75-4.365 9.75-9.75 9.75S2.25 17.385 2.25 12zM12 8.25a.75.75 0 01.75.75v3.75a.75.75 0 01-1.5 0V9a.75.75 0 01.75-.75zm0 8.25a.75.75 0 100-1.5.75.75 0 000 1.5z" clipRule="evenodd" />
              </svg>
              <span>{authError}</span>
            </div>
          )}
        {/* 電子郵件 */}
        <div style={{ marginBottom: '1rem' }}>
          <label style={{
            display: 'block',
            fontWeight: '500',
            marginBottom: '0.5rem',
            color: '#374151'
          }}>
            電子郵件 *
          </label>
          <input
            type="email"
            value={email}
            onChange={(e) => {
              setEmail(e.target.value);
              if (errors.email) {
                setErrors(prev => ({ ...prev, email: undefined }));
              }
            }}
            style={{
              width: '100%',
              padding: '0.75rem',
              border: `1px solid ${errors.email ? '#ef4444' : '#d1d5db'}`,
              borderRadius: '6px',
              fontSize: '1rem',
              outline: 'none',
              transition: 'border-color 0.2s'
            }}
            placeholder="輸入您的電子郵件"
          />
          {errors.email && (
            <div style={{
              color: '#ef4444',
              fontSize: '0.875rem',
              marginTop: '0.25rem'
            }}>
              {errors.email}
            </div>
          )}
        </div>

        {/* 密碼 */}
        <div style={{ marginBottom: '1rem' }}>
          <label style={{
            display: 'flex',
            fontWeight: '500',
            marginBottom: '0.5rem',
            color: '#374151',
            justifyContent: 'space-between',
            alignItems: 'center'
          }}>
            密碼 *
            <a
              href="#"
              onClick={(e) => {
                e.preventDefault();
                if (onPasswordResetRequest) onPasswordResetRequest();
              }}
              style={{
                color: '#3b82f6',
                fontSize: '0.875rem',
                textDecoration: 'none'
              }}
            >
              忘記密碼？
            </a>
          </label>
          <div style={{ position: 'relative' }}>
            <input
              type={showPassword ? 'text' : 'password'}
              value={password}
              onChange={(e) => {
                setPassword(e.target.value);
                if (errors.password) {
                  setErrors(prev => ({ ...prev, password: undefined }));
                }
              }}
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
              placeholder="輸入您的密碼"
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
          {errors.password && (
            <div style={{
              color: '#ef4444',
              fontSize: '0.875rem',
              marginTop: '0.25rem'
            }}>
              {errors.password}
            </div>
          )}
        </div>

        {/* 提交按鈕 */}
        <button
          type="submit"
          disabled={loginInProgress}
          style={{
            width: '100%',
            padding: '0.875rem 1.5rem',
            backgroundColor: loginInProgress ? '#9ca3af' : '#3b82f6',
            color: 'white',
            border: 'none',
            borderRadius: '6px',
            fontSize: '1rem',
            fontWeight: '600',
            cursor: loginInProgress ? 'not-allowed' : 'pointer',
            transition: 'background-color 0.2s',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            gap: '0.5rem'
          }}
        >
          {loginInProgress ? '登入中...' : '登入'}
          {!loginInProgress && <span>→</span>}
        </button>
      </form>

      {/* 返回註冊 */}
      <div style={{
        textAlign: 'center',
        marginTop: '1.5rem',
        paddingTop: '1.5rem',
        borderTop: '1px solid #e5e7eb'
      }}>
        <p style={{ color: '#6b7280', fontSize: '0.875rem' }}>
          還沒有帳號？
          <a href="#" style={{
            color: '#3b82f6',
            textDecoration: 'none',
            marginLeft: '0.25rem'
          }}>
            立即註冊
          </a>
        </p>
      </div>
    </div>
  );
};