import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import { User } from '../types/common'
import { LoginResponse } from '../types/auth'
import { apiClient } from '../services/api-client'

interface AuthState {
  token: string | null
  user: User | null
  isAuthenticated: boolean
  error: string | null
  isLoading: boolean
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  refreshToken: () => Promise<void>
  clearError: () => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      user: null,
      isAuthenticated: false,
      error: null,
      isLoading: false,

      login: async (email: string, password: string) => {
        set({ isLoading: true, error: null })
        try {
          const response = await apiClient.post<LoginResponse>('/Users/login', {
            email,
            password
          })

          // Map the API response to our User model
          const user: User = {
            id: response.data.userId,
            username: response.data.username,
            email: response.data.email
          }

          set({
            token: response.data.accessToken,
            user,
            isAuthenticated: true,
            error: null,
            isLoading: false
          })

          // Store refresh token
          localStorage.setItem('refreshToken', response.data.refreshToken)

          // Redirect will be handled by the component
          window.location.href = '/dashboard'
        } catch (error: any) {
          const errorMessage = error.response?.data?.message || error.message || '登入失敗'
          set({
            error: errorMessage,
            isLoading: false
          })
          throw new Error(errorMessage)
        }
      },

      logout: () => {
        set({
          token: null,
          user: null,
          isAuthenticated: false,
          error: null,
          isLoading: false
        })
        window.location.href = '/login'
      },

      clearError: () => {
        set({ error: null })
      },

      refreshToken: async () => {
        const refreshToken = localStorage.getItem('refreshToken')
        if (!refreshToken) {
          get().logout()
          return
        }

        try {
          const response = await apiClient.post<{ success: boolean; data: { accessToken: string; user: User } }>('/Users/refresh-token', {
            refreshToken
          })
          if (response.data.success) {
            set({
              token: response.data.data.accessToken,
              user: response.data.data.user
            })
          } else {
            get().logout()
          }
        } catch (error) {
          console.error('Token refresh failed', error)
          get().logout()
        }
      }
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({
        token: state.token,
        user: state.user,
        isAuthenticated: state.isAuthenticated
      })
    }
  )
)