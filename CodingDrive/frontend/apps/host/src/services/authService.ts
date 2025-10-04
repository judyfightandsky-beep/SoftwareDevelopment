import {
  LoginRequest,
  LoginResponse,
  UserRegistrationRequest,
  UserRegistrationResponse,
  PasswordResetRequest,
  PasswordResetConfirmRequest,
  EmailVerificationRequest,
  AuthErrorResponse
} from '../types/auth';
import { apiClient } from './api-client';

class AuthService {

  async register(data: UserRegistrationRequest): Promise<UserRegistrationResponse> {
    try {
      const response = await apiClient.post('/Users/register', data);
      if (response.data.success) {
        return response.data.data;
      }
      throw new Error(response.data.message || '註冊失敗');
    } catch (error) {
      console.error('Registration error:', error);
      throw error;
    }
  }

  async login(data: LoginRequest): Promise<LoginResponse> {
    try {
      const response = await apiClient.post<LoginResponse>('/Users/login', data);
      // The API returns the login response directly, not wrapped in a success/data structure
      this.setAccessToken(response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken || '');
      return response.data;
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  }

  async passwordResetRequest(data: PasswordResetRequest): Promise<void> {
    try {
      const response = await apiClient.post('/Users/password-reset-request', data);
      if (!response.data.success) {
        throw new Error(response.data.message || '密碼重設請求失敗');
      }
    } catch (error) {
      console.error('Password reset request error:', error);
      throw error;
    }
  }

  async passwordResetConfirm(data: PasswordResetConfirmRequest): Promise<void> {
    try {
      const response = await apiClient.post('/Users/password-reset-confirm', data);
      if (!response.data.success) {
        throw new Error(response.data.message || '密碼重設失敗');
      }
    } catch (error) {
      console.error('Password reset confirm error:', error);
      throw error;
    }
  }

  async verifyEmail(data: EmailVerificationRequest): Promise<void> {
    try {
      const response = await apiClient.post('/Users/verify-email', data);
      if (!response.data.success) {
        throw new Error(response.data.message || '信箱驗證失敗');
      }
    } catch (error) {
      console.error('Email verification error:', error);
      throw error;
    }
  }

  async resendVerificationEmail(email: string): Promise<void> {
    try {
      const response = await apiClient.post('/Users/resend-verification-email', { email });
      if (!response.data.success) {
        throw new Error(response.data.message || '重新發送驗證信件失敗');
      }
    } catch (error) {
      console.error('Resend verification email error:', error);
      throw error;
    }
  }

  // Local token management
  setAccessToken(token: string): void {
    localStorage.setItem('accessToken', token);
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  removeAccessToken(): void {
    localStorage.removeItem('accessToken');
  }

  // Refresh token mechanism
  async refreshToken(): Promise<LoginResponse | null> {
    const refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken) return null;

    try {
      const response = await apiClient.post('/Users/refresh-token', { refreshToken });

      if (response.data.success) {
        this.setAccessToken(response.data.data.accessToken);
        return response.data.data;
      }

      this.removeAccessToken();
      return null;
    } catch {
      this.removeAccessToken();
      return null;
    }
  }
}

export const authService = new AuthService();