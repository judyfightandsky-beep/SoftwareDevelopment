import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { GlobalStateProvider } from './contexts/GlobalStateContext';
import { LoginForm } from './components/LoginForm';
import { RegisterForm } from './components/RegisterForm';
import SimpleDashboard from './components/SimpleDashboard';
import HomePage from './components/HomePage';

// Login page component
const LoginPage: React.FC = () => {
  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: '#f3f4f6'
    }}>
      <LoginForm />
    </div>
  );
};

// Register page component
const RegisterPage: React.FC = () => {
  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: '#f3f4f6'
    }}>
      <RegisterForm />
    </div>
  );
};

// Protected route wrapper
const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />;
};

const App: React.FC = () => {
  return (
    <Router>
      <GlobalStateProvider>
        <AuthProvider>
          <Routes>
            {/* 美麗的首頁 - 主要入口 */}
            <Route path="/" element={<HomePage />} />

            {/* 驗證相關頁面 */}
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* 受保護的儀表板 */}
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <SimpleDashboard />
                </ProtectedRoute>
              }
            />

            {/* 兼容性路由 - 未登入時重導向到首頁 */}
            <Route path="/home" element={<Navigate to="/" replace />} />
          </Routes>
        </AuthProvider>
      </GlobalStateProvider>
    </Router>
  );
};

export default App;