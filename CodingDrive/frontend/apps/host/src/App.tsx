import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { RegisterForm } from './components/RegisterForm';
import LandingPage from './components/HomePage';
import UserDashboard from './components/UserDashboard';
import { LoginForm } from './components/LoginForm';
import { AuthProvider } from './contexts/AuthContext';
import PrivateRoute from './components/PrivateRoute';


const App: React.FC = () => {
  const [currentPath, setCurrentPath] = useState(window.location.pathname);

  React.useEffect(() => {
    const handleLocationChange = () => {
      setCurrentPath(window.location.pathname);
    };

    window.addEventListener('popstate', handleLocationChange);
    return () => window.removeEventListener('popstate', handleLocationChange);
  }, []);

  const isHomePage = currentPath === '/';

  return (
    <Router>
      <AuthProvider>
        <div style={{ minHeight: '100vh', background: isHomePage ? 'transparent' : '#f9fafb' }}>
          {!isHomePage && (
            <header style={{
              background: 'white',
              boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
              padding: '1rem 0'
            }}>
              <div style={{
                maxWidth: '1200px',
                margin: '0 auto',
                padding: '0 1rem',
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
              }}>
                <h1 style={{
                  fontSize: '1.25rem',
                  fontWeight: '600',
                  color: '#374151',
                  margin: '0'
                }}>
                  軟體開發專案管理平台
                </h1>
                <nav style={{ display: 'flex', gap: '1rem' }}>
                  <a
                    href="/"
                    style={{
                      color: '#6b7280',
                      textDecoration: 'none',
                      padding: '0.5rem 1rem',
                      borderRadius: '6px'
                    }}
                  >
                    首頁
                  </a>
                  <a
                    href="/register"
                    style={{
                      color: '#6b7280',
                      textDecoration: 'none',
                      padding: '0.5rem 1rem',
                      borderRadius: '6px'
                    }}
                  >
                    註冊
                  </a>
                  <a
                    href="/login"
                    style={{
                      color: '#6b7280',
                      textDecoration: 'none',
                      padding: '0.5rem 1rem',
                      borderRadius: '6px'
                    }}
                  >
                    登入
                  </a>
                </nav>
              </div>
            </header>
          )}

          <main style={{ padding: isHomePage ? '0' : '2rem 0' }}>
            <div style={{ maxWidth: isHomePage ? '100%' : '600px', margin: '0 auto' }}>
              <Routes>
                <Route path="/" element={<LandingPage />} />
                <Route
                  path="/dashboard"
                  element={<PrivateRoute><UserDashboard /></PrivateRoute>}
                />
                <Route
                  path="/register"
                  element={<RegisterForm />}
                />
                <Route
                  path="/login"
                  element={<LoginForm />}
                />
              </Routes>
            </div>
          </main>
        </div>
      </AuthProvider>
    </Router>
  );
};

export default App;