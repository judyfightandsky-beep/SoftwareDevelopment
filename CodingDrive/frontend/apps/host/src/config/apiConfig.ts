// API Configuration
export const API_CONFIG = {
  BASE_URL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5555',
  TIMEOUT: 10000, // 10 seconds timeout
  ENDPOINTS: {
    REGISTER: '/api/Users/register',
    CHECK_EMAIL: '/api/Users/check-email',
    RESEND_VERIFICATION: '/api/Users/resend-verification'
  }
};

// Network Error Handling
export const handleNetworkError = (error: Error) => {
  console.error('Network Error:', error);
  return {
    success: false,
    message: 'Network error. Please check your connection and try again.'
  };
};

// API Request Wrapper
export const apiRequest = async (
  url: string,
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' = 'POST',
  body?: any
) => {
  try {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), API_CONFIG.TIMEOUT);

    const response = await fetch(`${API_CONFIG.BASE_URL}${url}`, {
      method,
      headers: {
        'Content-Type': 'application/json',
        // Add any additional headers like authorization tokens
      },
      body: body ? JSON.stringify(body) : undefined,
      signal: controller.signal
    });

    clearTimeout(timeoutId);

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'An error occurred');
    }

    return {
      success: true,
      data: await response.json()
    };
  } catch (error) {
    return handleNetworkError(error as Error);
  }
};