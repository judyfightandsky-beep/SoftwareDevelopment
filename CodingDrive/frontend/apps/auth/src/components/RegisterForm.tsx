import React from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import toast from 'react-hot-toast';
import { authService, RegisterRequest } from '../services/authApi';

const registerSchema = yup.object({
  username: yup
    .string()
    .required('使用者名稱為必填')
    .min(3, '使用者名稱至少需要3個字元')
    .max(50, '使用者名稱不能超過50個字元'),
  email: yup
    .string()
    .required('電子信箱為必填')
    .email('請輸入有效的電子信箱'),
  firstName: yup
    .string()
    .required('名字為必填')
    .max(50, '名字不能超過50個字元'),
  lastName: yup
    .string()
    .required('姓氏為必填')
    .max(50, '姓氏不能超過50個字元'),
  password: yup
    .string()
    .required('密碼為必填')
    .min(8, '密碼至少需要8個字元')
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
      '密碼需包含大寫字母、小寫字母和數字'
    ),
  confirmPassword: yup
    .string()
    .required('確認密碼為必填')
    .oneOf([yup.ref('password')], '確認密碼必須與密碼相符'),
});

type RegisterFormData = yup.InferType<typeof registerSchema>;

interface RegisterFormProps {
  onSuccess?: (data: any) => void;
  onError?: (error: string) => void;
}

export const RegisterForm: React.FC<RegisterFormProps> = ({
  onSuccess,
  onError,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<RegisterFormData>({
    resolver: yupResolver(registerSchema),
  });

  const onSubmit = async (data: RegisterFormData) => {
    try {
      const registerData: RegisterRequest = {
        username: data.username,
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
        password: data.password,
        confirmPassword: data.confirmPassword,
      };

      const response = await authService.register(registerData);

      toast.success('註冊成功！請檢查您的電子信箱以驗證帳號。');
      reset();

      if (onSuccess) {
        onSuccess(response);
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : '註冊失敗';
      toast.error(errorMessage);

      if (onError) {
        onError(errorMessage);
      }
    }
  };

  return (
    <div className="register-form">
      <div className="form-container">
        <h2 className="form-title">註冊新帳號</h2>
        <form onSubmit={handleSubmit(onSubmit)} className="form">
          <div className="form-group">
            <label htmlFor="username" className="form-label">
              使用者名稱 *
            </label>
            <input
              id="username"
              type="text"
              className={`form-input ${errors.username ? 'error' : ''}`}
              {...register('username')}
              placeholder="請輸入使用者名稱"
            />
            {errors.username && (
              <span className="error-message">{errors.username.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="email" className="form-label">
              電子信箱 *
            </label>
            <input
              id="email"
              type="email"
              className={`form-input ${errors.email ? 'error' : ''}`}
              {...register('email')}
              placeholder="請輸入電子信箱"
            />
            {errors.email && (
              <span className="error-message">{errors.email.message}</span>
            )}
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="firstName" className="form-label">
                名字 *
              </label>
              <input
                id="firstName"
                type="text"
                className={`form-input ${errors.firstName ? 'error' : ''}`}
                {...register('firstName')}
                placeholder="請輸入名字"
              />
              {errors.firstName && (
                <span className="error-message">{errors.firstName.message}</span>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="lastName" className="form-label">
                姓氏 *
              </label>
              <input
                id="lastName"
                type="text"
                className={`form-input ${errors.lastName ? 'error' : ''}`}
                {...register('lastName')}
                placeholder="請輸入姓氏"
              />
              {errors.lastName && (
                <span className="error-message">{errors.lastName.message}</span>
              )}
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="password" className="form-label">
              密碼 *
            </label>
            <input
              id="password"
              type="password"
              className={`form-input ${errors.password ? 'error' : ''}`}
              {...register('password')}
              placeholder="請輸入密碼"
            />
            {errors.password && (
              <span className="error-message">{errors.password.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="confirmPassword" className="form-label">
              確認密碼 *
            </label>
            <input
              id="confirmPassword"
              type="password"
              className={`form-input ${errors.confirmPassword ? 'error' : ''}`}
              {...register('confirmPassword')}
              placeholder="請再次輸入密碼"
            />
            {errors.confirmPassword && (
              <span className="error-message">{errors.confirmPassword.message}</span>
            )}
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className={`submit-button ${isSubmitting ? 'loading' : ''}`}
          >
            {isSubmitting ? '註冊中...' : '註冊'}
          </button>
        </form>
      </div>

      <style jsx>{`
        .register-form {
          max-width: 500px;
          margin: 0 auto;
          padding: 2rem;
        }

        .form-container {
          background: white;
          border-radius: 8px;
          box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
          padding: 2rem;
        }

        .form-title {
          font-size: 1.5rem;
          font-weight: 600;
          text-align: center;
          margin-bottom: 2rem;
          color: #333;
        }

        .form {
          display: flex;
          flex-direction: column;
          gap: 1rem;
        }

        .form-row {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 1rem;
        }

        .form-group {
          display: flex;
          flex-direction: column;
        }

        .form-label {
          font-weight: 500;
          margin-bottom: 0.5rem;
          color: #374151;
        }

        .form-input {
          padding: 0.75rem;
          border: 1px solid #d1d5db;
          border-radius: 6px;
          font-size: 1rem;
          transition: border-color 0.2s;
        }

        .form-input:focus {
          outline: none;
          border-color: #3b82f6;
          box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
        }

        .form-input.error {
          border-color: #ef4444;
        }

        .error-message {
          color: #ef4444;
          font-size: 0.875rem;
          margin-top: 0.25rem;
        }

        .submit-button {
          background: #3b82f6;
          color: white;
          padding: 0.75rem 1.5rem;
          border: none;
          border-radius: 6px;
          font-size: 1rem;
          font-weight: 500;
          cursor: pointer;
          transition: background-color 0.2s;
          margin-top: 1rem;
        }

        .submit-button:hover:not(:disabled) {
          background: #2563eb;
        }

        .submit-button:disabled {
          background: #9ca3af;
          cursor: not-allowed;
        }

        .submit-button.loading {
          position: relative;
        }

        .submit-button.loading::after {
          content: '';
          position: absolute;
          width: 16px;
          height: 16px;
          margin: auto;
          border: 2px solid transparent;
          border-top-color: #ffffff;
          border-radius: 50%;
          animation: spin 1s linear infinite;
        }

        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }

        @media (max-width: 640px) {
          .register-form {
            padding: 1rem;
          }

          .form-row {
            grid-template-columns: 1fr;
          }
        }
      `}</style>
    </div>
  );
};