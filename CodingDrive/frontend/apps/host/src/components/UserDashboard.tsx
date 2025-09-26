import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const UserDashboard: React.FC = () => {
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const renderSidebar = () => (
    <div style={{
      backgroundColor: 'white',
      borderRadius: '12px',
      padding: '1.5rem',
      boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
    }}>
      <div style={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        marginBottom: '2rem'
      }}>
        <div style={{
          width: '80px',
          height: '80px',
          backgroundColor: '#f3f4f6',
          borderRadius: '50%',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          marginBottom: '1rem',
          fontSize: '2rem',
          color: '#374151'
        }}>
          {user?.firstName?.[0] || '👤'}
        </div>
        <h2 style={{
          fontSize: '1.25rem',
          fontWeight: '600',
          color: '#1f2937',
          marginBottom: '0.5rem'
        }}>
          {user?.firstName} {user?.lastName}
        </h2>
        <p style={{
          color: '#6b7280',
          fontSize: '0.875rem'
        }}>
          @{user?.username}
        </p>
      </div>
      <nav>
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {[
            { label: '儀表板', icon: '📊', active: true },
            { label: '個人資料', icon: '👤' },
            { label: '專案', icon: '🚀' },
            { label: '設定', icon: '⚙️' }
          ].map((item, index) => (
            <li key={index} style={{ marginBottom: '0.5rem' }}>
              <a href="#" style={{
                display: 'flex',
                alignItems: 'center',
                gap: '0.75rem',
                padding: '0.75rem 1rem',
                borderRadius: '8px',
                color: item.active ? '#3b82f6' : '#374151',
                textDecoration: 'none',
                backgroundColor: item.active ? '#f0f9ff' : 'transparent',
                transition: 'background-color 0.2s'
              }}>
                <span style={{ fontSize: '1.25rem' }}>{item.icon}</span>
                {item.label}
              </a>
            </li>
          ))}
          <li style={{
            marginTop: '1rem',
            borderTop: '1px solid #e5e7eb',
            paddingTop: '1rem'
          }}>
            <button
              onClick={logout}
              style={{
                width: '100%',
                display: 'flex',
                alignItems: 'center',
                gap: '0.75rem',
                padding: '0.75rem 1rem',
                backgroundColor: 'transparent',
                color: '#ef4444',
                border: 'none',
                borderRadius: '8px',
                cursor: 'pointer',
                transition: 'background-color 0.2s'
              }}
            >
              🚪 登出
            </button>
          </li>
        </ul>
      </nav>
    </div>
  );

  const renderMainContent = () => (
    <div>
      <h1 style={{
        fontSize: '1.75rem',
        fontWeight: '600',
        color: '#1f2937',
        marginBottom: '1.5rem'
      }}>
        歡迎回來, {user?.firstName}!
      </h1>

      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(3, 1fr)',
        gap: '1.5rem'
      }}>
        {[
          { label: '專案數量', value: '0', icon: '🚀' },
          { label: '待辦事項', value: '0', icon: '✅' },
          { label: '貢獻天數', value: '0', icon: '📅' }
        ].map((card, index) => (
          <div key={index} style={{
            backgroundColor: 'white',
            borderRadius: '12px',
            padding: '1.5rem',
            boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)',
            display: 'flex',
            alignItems: 'center',
            gap: '1rem'
          }}>
            <div style={{
              fontSize: '2.5rem',
              backgroundColor: '#f3f4f6',
              borderRadius: '8px',
              padding: '0.5rem',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }}>
              {card.icon}
            </div>
            <div>
              <p style={{
                color: '#6b7280',
                fontSize: '0.875rem',
                marginBottom: '0.25rem'
              }}>
                {card.label}
              </p>
              <h3 style={{
                fontSize: '1.5rem',
                fontWeight: '600',
                color: '#1f2937'
              }}>
                {card.value}
              </h3>
            </div>
          </div>
        ))}
      </div>

      {/* 最近活動 */}
      <div style={{
        backgroundColor: 'white',
        borderRadius: '12px',
        padding: '1.5rem',
        marginTop: '2rem',
        boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
      }}>
        <h2 style={{
          fontSize: '1.25rem',
          fontWeight: '600',
          color: '#1f2937',
          marginBottom: '1rem'
        }}>
          最近活動
        </h2>
        <div style={{
          color: '#6b7280',
          fontSize: '0.875rem'
        }}>
          尚無最近活動
        </div>
      </div>
    </div>
  );

  return (
    <div style={{
      maxWidth: '1200px',
      margin: '0 auto',
      padding: '2rem',
      display: 'grid',
      gap: '2rem',
      gridTemplateColumns: '250px 1fr',
      backgroundColor: '#f9fafb'
    }}>
      {renderSidebar()}
      {renderMainContent()}
    </div>
  );
};

export default UserDashboard;