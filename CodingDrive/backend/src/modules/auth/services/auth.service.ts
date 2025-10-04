import { Injectable } from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { ApiResponse } from '../../../common/interfaces/api-response.interface';
import { UserRepository } from '../../../infrastructure/repositories/user.repository';

@Injectable()
export class AuthService {
  constructor(
    private readonly jwtService: JwtService,
    private readonly userRepository: UserRepository
  ) {}

  async verifyEmail(token: string): Promise<ApiResponse<boolean>> {
    try {
      const user = await this.userRepository.findByVerificationToken(token);

      if (!user) {
        return {
          success: false,
          error: {
            code: 'INVALID_TOKEN',
            message: 'Invalid verification token'
          }
        };
      }

      user.isEmailVerified = true;
      user.verificationToken = null;

      await this.userRepository.save(user);

      return { success: true, data: true };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'EMAIL_VERIFICATION_FAILED',
          message: error.message
        }
      };
    }
  }

  async requestPasswordReset(email: string): Promise<ApiResponse<boolean>> {
    try {
      const user = await this.userRepository.findByEmail(email);

      if (!user) {
        return {
          success: false,
          error: {
            code: 'USER_NOT_FOUND',
            message: 'User not found'
          }
        };
      }

      // Generate password reset token
      const resetToken = this.generateResetToken();
      user.passwordResetToken = resetToken;
      user.passwordResetTokenExpires = new Date(Date.now() + 3600000); // 1 hour from now

      await this.userRepository.save(user);

      // TODO: Send email with reset link

      return { success: true, data: true };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'PASSWORD_RESET_FAILED',
          message: error.message
        }
      };
    }
  }

  async resetPassword(token: string, newPassword: string): Promise<ApiResponse<boolean>> {
    try {
      const user = await this.userRepository.findByPasswordResetToken(token);

      if (!user || user.passwordResetTokenExpires < new Date()) {
        return {
          success: false,
          error: {
            code: 'INVALID_RESET_TOKEN',
            message: 'Invalid or expired reset token'
          }
        };
      }

      // Hash the new password
      user.password = await this.hashPassword(newPassword);
      user.passwordResetToken = null;
      user.passwordResetTokenExpires = null;

      await this.userRepository.save(user);

      return { success: true, data: true };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'PASSWORD_RESET_FAILED',
          message: error.message
        }
      };
    }
  }

  async logout(token: string): Promise<ApiResponse<boolean>> {
    try {
      // Blacklist the token
      await this.blacklistToken(token);

      return { success: true, data: true };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'LOGOUT_FAILED',
          message: error.message
        }
      };
    }
  }

  async refreshToken(refreshToken: string): Promise<ApiResponse<{ accessToken: string; refreshToken: string }>> {
    try {
      const decoded = this.jwtService.verify(refreshToken);
      const user = await this.userRepository.findByEmail(decoded.email);

      if (!user) {
        return {
          success: false,
          error: {
            code: 'INVALID_USER',
            message: 'User not found'
          }
        };
      }

      const newAccessToken = this.generateAccessToken(user);
      const newRefreshToken = this.generateRefreshToken(user);

      return {
        success: true,
        data: {
          accessToken: newAccessToken,
          refreshToken: newRefreshToken
        }
      };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'TOKEN_REFRESH_FAILED',
          message: 'Invalid or expired refresh token'
        }
      };
    }
  }

  private async hashPassword(password: string): Promise<string> {
    // TODO: Implement secure password hashing
    return password;
  }

  private async blacklistToken(token: string): Promise<void> {
    // TODO: Implement token blacklisting mechanism
  }

  private generateResetToken(): string {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  private generateAccessToken(user: any): string {
    return this.jwtService.sign({
      email: user.email,
      sub: user.id,
      type: 'access'
    }, { expiresIn: '1h' });
  }

  private generateRefreshToken(user: any): string {
    return this.jwtService.sign({
      email: user.email,
      sub: user.id,
      type: 'refresh'
    }, { expiresIn: '7d' });
  }
}