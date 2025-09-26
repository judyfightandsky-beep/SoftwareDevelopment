import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';

export const EmailVerificationPage: React.FC = () => {
  const [verificationStatus, setVerificationStatus] = useState<'idle' | 'verifying' | 'success' | 'error'>('idle');
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const location = useLocation();

  useEffect(() => {
    const verifyEmail = async () => {
      const searchParams = new URLSearchParams(location.search);
      const token = searchParams.get('token');
      const email = searchParams.get('email');

      if (!token || !email) {
        setVerificationStatus('error');
        setErrorMessage('無效的驗證連結');
        return;
      }

      setVerificationStatus('verifying');

      try {
        const response = await fetch('http://localhost:5555/api/Users/verify-email', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ token, email })
        });

        if (response.ok) {
          setVerificationStatus('success');
        } else {
          const errorData = await response.json();
          setVerificationStatus('error');
          setErrorMessage(errorData.message || '信箱驗證失敗');
        }
      } catch (error) {
        console.error('Email verification error:', error);
        setVerificationStatus('error');
        setErrorMessage('網路錯誤，請稍後再試');
      }
    };

    verifyEmail();
  }, [location]);

  const renderVerificationContent = () => {
    switch (verificationStatus) {
      case 'idle':
      case 'verifying':
        return (
          <div style={{ textAlign: 'center', padding: '2rem' }}>
            <div style={{
              width: '80px',
              height: '80px',
              backgroundColor: '#e5e7eb',
              borderRadius: '50%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              margin: '0 auto 2rem',
              fontSize: '2rem'
            }}>
              🕐
            </div>
            <h3 style={{ fontSize: '1.5rem', color: '#1f2937', marginBottom: '1rem' }}>
              驗證中...
            </h3>
            <p style={{ color: '#6b7280' }}>
              正在驗證您的電子信箱，請稍候
            </p>
          </div>
        );
      case 'success':
        return (
          <div style={{ textAlign: 'center', padding: '2rem' }}>
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
            <h3 style={{ fontSize: '1.5rem', color: '#1f2937', marginBottom: '1rem' }}>
              信箱驗證成功
            </h3>
            <p style={{ color: '#6b7280', marginBottom: '2rem' }}>
              您的電子信箱已成功驗證。現在可以登入系統了！
            </p>
            <button
              onClick={() => window.location.href = '/login'}
              style={{
                backgroundColor: '#3b82f6',
                color: 'white',
                padding: '0.75rem 1.5rem',
                border: 'none',
                borderRadius: '6px',
                cursor: 'pointer'
              }}
            >
              前往登入
            </button>
          </div>
        );
      case 'error':
        return (
          <div style={{ textAlign: 'center', padding: '2rem' }}>
            <div style={{
              width: '80px',
              height: '80px',
              backgroundColor: '#fee2e2',
              borderRadius: '50%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              margin: '0 auto 2rem',
              fontSize: '2rem'
            }}>
              ❌
            </div>
            <h3 style={{ fontSize: '1.5rem', color: '#1f2937', marginBottom: '1rem' }}>
              驗證失敗
            </h3>
            <p style={{ color: '#ef4444', marginBottom: '2rem' }}>
              {errorMessage || '驗證過程中發生錯誤'}
            </p>
            <button
              onClick={() => window.location.href = '/register'}
              style={{
                backgroundColor: '#f3f4f6',
                color: '#374151',
                padding: '0.75rem 1.5rem',
                border: 'none',
                borderRadius: '6px',
                cursor: 'pointer',
                marginRight: '1rem'
              }}
            >
              重新註冊
            </button>
            <button
              onClick={() => window.location.href = '/'}
              style={{
                backgroundColor: '#3b82f6',
                color: 'white',
                padding: '0.75rem 1.5rem',
                border: 'none',
                borderRadius: '6px',
                cursor: 'pointer'
              }}
            >
              返回首頁
            </button>
          </div>
        );
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
      {renderVerificationContent()}
    </div>
  );
};