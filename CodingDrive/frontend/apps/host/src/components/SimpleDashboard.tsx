import React from 'react';
import { useAuth } from '../contexts/AuthContext';

const SimpleDashboard: React.FC = () => {
  const { user, logout } = useAuth();

  const containerStyle: React.CSSProperties = {
    minHeight: '100vh',
    backgroundColor: '#f8fafc',
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
    fontSize: '14px',
    lineHeight: '1.5',
    color: '#374151'
  };

  const headerStyle: React.CSSProperties = {
    backgroundColor: '#ffffff',
    boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
    borderBottom: '1px solid #e5e7eb',
    padding: '0 1rem',
    height: '64px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between'
  };

  const titleStyle: React.CSSProperties = {
    fontSize: '20px',
    fontWeight: '600',
    color: '#1f2937',
    textDecoration: 'none'
  };

  const mainLayoutStyle: React.CSSProperties = {
    display: 'flex'
  };

  const sidebarStyle: React.CSSProperties = {
    width: '256px',
    backgroundColor: '#ffffff',
    borderRight: '1px solid #e5e7eb',
    minHeight: 'calc(100vh - 64px)',
    padding: '1rem'
  };

  const contentStyle: React.CSSProperties = {
    flex: '1',
    padding: '2rem'
  };

  const cardStyle: React.CSSProperties = {
    backgroundColor: '#ffffff',
    borderRadius: '8px',
    boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
    border: '1px solid #e5e7eb',
    padding: '1.5rem',
    marginBottom: '1.5rem'
  };

  const gridStyle: React.CSSProperties = {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
    gap: '1.5rem',
    marginBottom: '2rem'
  };

  const statCardStyle: React.CSSProperties = {
    ...cardStyle,
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between'
  };

  const statNumberStyle: React.CSSProperties = {
    fontSize: '28px',
    fontWeight: '600',
    color: '#1f2937'
  };

  const statLabelStyle: React.CSSProperties = {
    fontSize: '14px',
    fontWeight: '500',
    color: '#6b7280',
    marginBottom: '0.5rem'
  };

  const buttonStyle: React.CSSProperties = {
    padding: '0.5rem 1rem',
    fontSize: '14px',
    fontWeight: '500',
    color: '#374151',
    backgroundColor: '#ffffff',
    border: '1px solid #d1d5db',
    borderRadius: '6px',
    cursor: 'pointer',
    textDecoration: 'none'
  };

  const navItemStyle: React.CSSProperties = {
    display: 'flex',
    alignItems: 'center',
    padding: '0.75rem 1rem',
    fontSize: '14px',
    fontWeight: '500',
    color: '#374151',
    textDecoration: 'none',
    borderRadius: '6px',
    marginBottom: '0.5rem',
    cursor: 'pointer'
  };

  const activeNavItemStyle: React.CSSProperties = {
    ...navItemStyle,
    backgroundColor: '#eff6ff',
    color: '#2563eb'
  };

  return (
    <div style={containerStyle}>
      {/* Header */}
      <header style={headerStyle}>
        <a href="/" style={titleStyle}>
          軟體開發專案管理平台
        </a>
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <span style={{ fontSize: '14px', color: '#6b7280' }}>
            歡迎, {user?.name || user?.firstName || '使用者'}
          </span>
          <button onClick={logout} style={buttonStyle}>
            登出
          </button>
        </div>
      </header>

      <div style={mainLayoutStyle}>
        {/* Sidebar */}
        <nav style={sidebarStyle}>
          <div style={activeNavItemStyle}>📊 專案總覽</div>
          <div style={navItemStyle}>📋 任務管理</div>
          <div style={navItemStyle}>🎯 模板中心</div>
          <div style={navItemStyle}>✅ 品質中心</div>
          <div style={navItemStyle}>📈 報表</div>
          <div style={navItemStyle}>⚙️ 設定</div>
        </nav>

        {/* Main Content */}
        <main style={contentStyle}>
          {/* Statistics Cards */}
          <div style={gridStyle}>
            <div style={statCardStyle}>
              <div>
                <div style={statLabelStyle}>進行中專案</div>
                <div style={statNumberStyle}>12</div>
                <div style={{ fontSize: '12px', color: '#059669' }}>+2 本週新增</div>
              </div>
              <div style={{
                width: '48px',
                height: '48px',
                backgroundColor: '#dbeafe',
                borderRadius: '8px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '20px'
              }}>
                📊
              </div>
            </div>

            <div style={statCardStyle}>
              <div>
                <div style={statLabelStyle}>待辦任務</div>
                <div style={statNumberStyle}>47</div>
                <div style={{ fontSize: '12px', color: '#dc2626' }}>3 即將逾期</div>
              </div>
              <div style={{
                width: '48px',
                height: '48px',
                backgroundColor: '#fed7aa',
                borderRadius: '8px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '20px'
              }}>
                📋
              </div>
            </div>

            <div style={statCardStyle}>
              <div>
                <div style={statLabelStyle}>代碼品質</div>
                <div style={statNumberStyle}>8.5</div>
                <div style={{ fontSize: '12px', color: '#059669' }}>+0.3 較上週提升</div>
              </div>
              <div style={{
                width: '48px',
                height: '48px',
                backgroundColor: '#dcfce7',
                borderRadius: '8px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '20px'
              }}>
                ✅
              </div>
            </div>

            <div style={statCardStyle}>
              <div>
                <div style={statLabelStyle}>團隊效率</div>
                <div style={statNumberStyle}>92%</div>
                <div style={{ fontSize: '12px', color: '#059669' }}>+5% 較上月提升</div>
              </div>
              <div style={{
                width: '48px',
                height: '48px',
                backgroundColor: '#e0e7ff',
                borderRadius: '8px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '20px'
              }}>
                📈
              </div>
            </div>
          </div>

          {/* Project Progress */}
          <div style={cardStyle}>
            <h3 style={{ fontSize: '18px', fontWeight: '600', color: '#1f2937', marginBottom: '1.5rem' }}>
              專案進度概覽
            </h3>
            <div style={{ marginBottom: '1rem' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '0.5rem' }}>
                <span style={{ fontSize: '14px', fontWeight: '500', color: '#1f2937' }}>電商平台重構專案</span>
                <span style={{ fontSize: '14px', color: '#6b7280' }}>85%</span>
              </div>
              <div style={{ width: '100%', height: '8px', backgroundColor: '#e5e7eb', borderRadius: '4px' }}>
                <div style={{ width: '85%', height: '100%', backgroundColor: '#2563eb', borderRadius: '4px' }}></div>
              </div>
            </div>

            <div style={{ marginBottom: '1rem' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '0.5rem' }}>
                <span style={{ fontSize: '14px', fontWeight: '500', color: '#1f2937' }}>用戶管理系統</span>
                <span style={{ fontSize: '14px', color: '#6b7280' }}>65%</span>
              </div>
              <div style={{ width: '100%', height: '8px', backgroundColor: '#e5e7eb', borderRadius: '4px' }}>
                <div style={{ width: '65%', height: '100%', backgroundColor: '#2563eb', borderRadius: '4px' }}></div>
              </div>
            </div>

            <div>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '0.5rem' }}>
                <span style={{ fontSize: '14px', fontWeight: '500', color: '#1f2937' }}>API 文件系統</span>
                <span style={{ fontSize: '14px', color: '#6b7280' }}>100%</span>
              </div>
              <div style={{ width: '100%', height: '8px', backgroundColor: '#e5e7eb', borderRadius: '4px' }}>
                <div style={{ width: '100%', height: '100%', backgroundColor: '#059669', borderRadius: '4px' }}></div>
              </div>
            </div>
          </div>

          {/* Recent Activity */}
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(400px, 1fr))', gap: '1.5rem' }}>
            <div style={cardStyle}>
              <h3 style={{ fontSize: '18px', fontWeight: '600', color: '#1f2937', marginBottom: '1.5rem' }}>
                最近活動
              </h3>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                  <div style={{ width: '8px', height: '8px', backgroundColor: '#059669', borderRadius: '50%' }}></div>
                  <div>
                    <div style={{ fontSize: '14px', color: '#1f2937' }}>張三 完成了任務：用戶認證API實作</div>
                    <div style={{ fontSize: '12px', color: '#6b7280' }}>2 小時前</div>
                  </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                  <div style={{ width: '8px', height: '8px', backgroundColor: '#2563eb', borderRadius: '50%' }}></div>
                  <div>
                    <div style={{ fontSize: '14px', color: '#1f2937' }}>李四 提交了代碼審查請求</div>
                    <div style={{ fontSize: '12px', color: '#6b7280' }}>4 小時前</div>
                  </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                  <div style={{ width: '8px', height: '8px', backgroundColor: '#f59e0b', borderRadius: '50%' }}></div>
                  <div>
                    <div style={{ fontSize: '14px', color: '#1f2937' }}>王五 發現了程式碼品質問題</div>
                    <div style={{ fontSize: '12px', color: '#6b7280' }}>6 小時前</div>
                  </div>
                </div>
              </div>
            </div>

            <div style={cardStyle}>
              <h3 style={{ fontSize: '18px', fontWeight: '600', color: '#1f2937', marginBottom: '1.5rem' }}>
                我的任務
              </h3>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                <div style={{
                  padding: '0.75rem',
                  border: '1px solid #e5e7eb',
                  borderRadius: '6px',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center'
                }}>
                  <div>
                    <div style={{ fontSize: '14px', fontWeight: '500', color: '#1f2937' }}>前端登入頁面優化</div>
                    <div style={{ fontSize: '12px', color: '#6b7280' }}>截止日期: 明天</div>
                  </div>
                  <span style={{
                    padding: '0.25rem 0.5rem',
                    fontSize: '12px',
                    fontWeight: '500',
                    backgroundColor: '#fee2e2',
                    color: '#991b1b',
                    borderRadius: '4px'
                  }}>
                    高
                  </span>
                </div>
                <div style={{
                  padding: '0.75rem',
                  border: '1px solid #e5e7eb',
                  borderRadius: '6px',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center'
                }}>
                  <div>
                    <div style={{ fontSize: '14px', fontWeight: '500', color: '#1f2937' }}>API 文件更新</div>
                    <div style={{ fontSize: '12px', color: '#6b7280' }}>截止日期: 本週五</div>
                  </div>
                  <span style={{
                    padding: '0.25rem 0.5rem',
                    fontSize: '12px',
                    fontWeight: '500',
                    backgroundColor: '#fef3c7',
                    color: '#92400e',
                    borderRadius: '4px'
                  }}>
                    中
                  </span>
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default SimpleDashboard;