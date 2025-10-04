import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5555';

const authApi = axios.create({
  baseURL: `${API_BASE_URL}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export interface RegisterRequest {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
}

export interface RegisterResponse {
  userId: string;
  username: string;
  email: string;
  role: string;
  status: string;
  requiresApproval: boolean;
}

export interface ApiError {
  error: string;
  message: string;
}

export const authService = {
  register: async (data: RegisterRequest): Promise<RegisterResponse> => {
    try {
      const response = await authApi.post<RegisterResponse>('/Users/register', data);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        const apiError = error.response.data as ApiError;
        throw new Error(apiError.message || '註冊失敗');
      }
      throw new Error('網路連線錯誤，請稍後再試');
    }
  },
};