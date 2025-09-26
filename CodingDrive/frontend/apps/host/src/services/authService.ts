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

class AuthService {
  private baseUrl = 'http://localhost:5555/api/Users';

  async register(data: UserRegistrationRequest): Promise<UserRegistrationResponse> {
    const response = await fetch(`${this.baseUrl}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '註冊失敗');
    }

    return response.json();
  }

  async login(data: LoginRequest): Promise<LoginResponse> {
    const response = await fetch(`${this.baseUrl}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '登入失敗');
    }

    return response.json();
  }

  async passwordResetRequest(data: PasswordResetRequest): Promise<void> {
    const response = await fetch(`${this.baseUrl}/password-reset-request`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '密碼重設請求失敗');
    }
  }

  async passwordResetConfirm(data: PasswordResetConfirmRequest): Promise<void> {
    const response = await fetch(`${this.baseUrl}/password-reset-confirm`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '密碼重設失敗');
    }
  }

  async verifyEmail(data: EmailVerificationRequest): Promise<void> {
    const response = await fetch(`${this.baseUrl}/verify-email`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '信箱驗證失敗');
    }
  }

  async resendVerificationEmail(email: string): Promise<void> {
    const response = await fetch(`${this.baseUrl}/resend-verification-email`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email })
    });

    if (!response.ok) {
      const error: AuthErrorResponse = await response.json();
      throw new Error(error.message || '重新發送驗證信件失敗');
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
      const response = await fetch(`${this.baseUrl}/refresh-token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ refreshToken })
      });

      if (!response.ok) {
        this.removeAccessToken();
        return null;
      }

      return response.json();
    } catch {
      this.removeAccessToken();
      return null;
    }
  }
}

export const authService = new AuthService();