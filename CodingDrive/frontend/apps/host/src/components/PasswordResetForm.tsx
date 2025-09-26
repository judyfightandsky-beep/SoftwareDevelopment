import React, { useState, useCallback } from 'react';
import {
  PasswordResetRequest,
  PasswordResetConfirmRequest,
  AuthErrorResponse
} from '../types/auth';

interface PasswordResetFormProps {
  onSuccess?: () => void;
  onError?: (error: string) => void;
  onLoginRequest?: () => void;
}

export const PasswordResetForm: React.FC<PasswordResetFormProps> = ({
  onSuccess,
  onError,
  onLoginRequest
}) => {
  const [email, setEmail] = useState('');
  const [resetToken, setResetToken] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [stage, setStage] = useState<'request' | 'confirm'>('request');
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState<{
    email?: string;
    token?: string;
    newPassword?: string;
    confirmPassword?: string;
  }>({});

  const validateRequestStage = useCallback((): boolean => {
    const newErrors: { email?: string } = {};

    if (!email.trim()) {
      newErrors.email = '請輸入電子信箱';
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      newErrors.email = '請輸入有效的電子信箱';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [email]);

  const validateConfirmStage = useCallback((): boolean => {
    const newErrors: {
      token?: string;
      newPassword?: string;
      confirmPassword?: string;
    } = {};

    if (!resetToken.trim()) {
      newErrors.token = '請輸入重設密碼驗證碼';
    }

    if (!newPassword) {
      newErrors.newPassword = '請輸入新密碼';
    } else if (newPassword.length < 8) {
      newErrors.newPassword = '密碼至少需要8個字元';
    }

    if (newPassword !== confirmPassword) {
      newErrors.confirmPassword = '確認密碼不符合';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [resetToken, newPassword, confirmPassword]);

  const handlePasswordResetRequest = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateRequestStage()) return;

    setIsLoading(true);

    try {
      const resetRequestData: PasswordResetRequest = { email };

      const response = await fetch('http://localhost:5555/api/Users/password-reset-request', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(resetRequestData),
      });

      if (response.ok) {
        setStage('confirm');
      } else {
        const error: AuthErrorResponse = await response.json();
        throw new Error(error.message || '密碼重設請求失敗');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : '密碼重設請求失敗';
      if (onError) onError(errorMessage);
      alert(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handlePasswordResetConfirm = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateConfirmStage()) return;

    setIsLoading(true);

    try {
      const resetConfirmData: PasswordResetConfirmRequest = {
        token: resetToken,
        newPassword,
        confirmPassword
      };

      const response = await fetch('http://localhost:5555/api/Users/password-reset-confirm', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(resetConfirmData),
      });

      if (response.ok) {
        if (onSuccess) onSuccess();
        alert('密碼重設成功');
      } else {
        const error: AuthErrorResponse = await response.json();
        throw new Error(error.message || '密碼重設失敗');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : '密碼重設失敗';
      if (onError) onError(errorMessage);
      alert(errorMessage);
    } finally {
      setIsLoading(false);
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
          {stage === 'request' ? '重設密碼' : '確認密碼重設'}
        </p>
      </div>

      {stage === 'request' && (
        <form onSubmit={handlePasswordResetRequest}>
          {/* 電子信箱 */}
          <div style={{ marginBottom: '1.5rem' }}>
            <label style={{
              display: 'block',
              fontWeight: '500',
              marginBottom: '0.5rem',
              color: '#374151'
            }}>
              電子信箱 *
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
              placeholder="輸入您的電子信箱"
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

          {/* 提交按鈕 */}
          <button
            type="submit"
            disabled={isLoading}
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
            {isLoading ? '發送中...' : '發送重設密碼連結'}
            {!isLoading && <span>→</span>}
          </button>
        </form>
      )}

      {stage === 'confirm' && (
        <form onSubmit={handlePasswordResetConfirm}>
          {/* 驗證碼 */}
          <div style={{ marginBottom: '1rem' }}>
            <label style={{
              display: 'block',
              fontWeight: '500',
              marginBottom: '0.5rem',
              color: '#374151'
            }}>
              驗證碼 *
            </label>
            <input
              type="text"
              value={resetToken}
              onChange={(e) => {
                setResetToken(e.target.value);
                if (errors.token) {
                  setErrors(prev => ({ ...prev, token: undefined }));
                }
              }}
              style={{
                width: '100%',
                padding: '0.75rem',
                border: `1px solid ${errors.token ? '#ef4444' : '#d1d5db'}`,
                borderRadius: '6px',
                fontSize: '1rem',
                outline: 'none',
                transition: 'border-color 0.2s'
              }}
              placeholder="輸入重設密碼驗證碼"
            />
            {errors.token && (
              <div style={{
                color: '#ef4444',
                fontSize: '0.875rem',
                marginTop: '0.25rem'
              }}>
                {errors.token}
              </div>
            )}
          </div>

          {/* 新密碼 */}
          <div style={{ marginBottom: '1rem' }}>
            <label style={{
              display: 'block',
              fontWeight: '500',
              marginBottom: '0.5rem',
              color: '#374151'
            }}>
              新密碼 *
            </label>
            <div style={{ position: 'relative' }}>
              <input
                type={showPassword ? 'text' : 'password'}
                value={newPassword}
                onChange={(e) => {
                  setNewPassword(e.target.value);
                  if (errors.newPassword) {
                    setErrors(prev => ({ ...prev, newPassword: undefined }));
                  }
                }}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  paddingRight: '3rem',
                  border: `1px solid ${errors.newPassword ? '#ef4444' : '#d1d5db'}`,
                  borderRadius: '6px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s'
                }}
                placeholder="輸入新密碼"
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
            {errors.newPassword && (
              <div style={{
                color: '#ef4444',
                fontSize: '0.875rem',
                marginTop: '0.25rem'
              }}>
                {errors.newPassword}
              </div>
            )}
          </div>

          {/* 確認新密碼 */}
          <div style={{ marginBottom: '1.5rem' }}>
            <label style={{
              display: 'block',
              fontWeight: '500',
              marginBottom: '0.5rem',
              color: '#374151'
            }}>
              確認新密碼 *
            </label>
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => {
                setConfirmPassword(e.target.value);
                if (errors.confirmPassword) {
                  setErrors(prev => ({ ...prev, confirmPassword: undefined }));
                }
              }}
              style={{
                width: '100%',
                padding: '0.75rem',
                border: `1px solid ${errors.confirmPassword ? '#ef4444' : '#d1d5db'}`,
                borderRadius: '6px',
                fontSize: '1rem',
                outline: 'none',
                transition: 'border-color 0.2s'
              }}
              placeholder="再次輸入新密碼"
            />
            {errors.confirmPassword && (
              <div style={{
                color: '#ef4444',
                fontSize: '0.875rem',
                marginTop: '0.25rem'
              }}>
                {errors.confirmPassword}
              </div>
            )}
          </div>

          {/* 提交按鈕 */}
          <button
            type="submit"
            disabled={isLoading}
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
            {isLoading ? '重設中...' : '重設密碼'}
            {!isLoading && <span>→</span>}
          </button>
        </form>
      )}

      {/* 返回登入 */}
      <div style={{
        textAlign: 'center',
        marginTop: '1.5rem',
        paddingTop: '1.5rem',
        borderTop: '1px solid #e5e7eb'
      }}>
        <p style={{ color: '#6b7280', fontSize: '0.875rem' }}>
          想起密碼了？
          <a
            href="#"
            onClick={(e) => {
              e.preventDefault();
              if (onLoginRequest) onLoginRequest();
            }}
            style={{
              color: '#3b82f6',
              textDecoration: 'none',
              marginLeft: '0.25rem'
            }}
          >
            立即登入
          </a>
        </p>
      </div>
    </div>
  );
};