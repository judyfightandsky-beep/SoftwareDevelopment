import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import federation from '@originjs/vite-plugin-federation'

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'auth-app',
      filename: 'remoteEntry.js',
      exposes: {
        './RegisterForm': './src/components/RegisterForm',
        './LoginForm': './src/components/LoginForm'
      },
      shared: ['react', 'react-dom']
    })
  ],
  server: {
    port: 3001,
    cors: true
  },
  build: {
    modulePreload: false,
    target: 'esnext',
    minify: false,
    cssCodeSplit: false
  }
})