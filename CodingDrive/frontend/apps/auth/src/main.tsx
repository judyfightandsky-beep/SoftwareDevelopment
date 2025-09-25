import React from 'react'
import ReactDOM from 'react-dom/client'
import { RegisterForm } from './components/RegisterForm'
import './index.css'

// 開發模式下可以獨立運行認證模組
if (process.env.NODE_ENV === 'development') {
  const rootElement = document.getElementById('root')
  if (rootElement) {
    ReactDOM.createRoot(rootElement).render(
      <React.StrictMode>
        <div style={{ padding: '2rem' }}>
          <h1 style={{ textAlign: 'center', marginBottom: '2rem' }}>
            認證模組 - 獨立開發模式
          </h1>
          <RegisterForm
            onSuccess={(data) => console.log('註冊成功:', data)}
            onError={(error) => console.error('註冊失敗:', error)}
          />
        </div>
      </React.StrictMode>,
    )
  }
}