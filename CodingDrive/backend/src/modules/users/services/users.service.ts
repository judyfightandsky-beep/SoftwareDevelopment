import { Injectable } from '@nestjs/common';
import { ApiResponse } from '../../../common/interfaces/api-response.interface';
import { UserRepository } from '../../../infrastructure/repositories/user.repository';

@Injectable()
export class UsersService {
  constructor(
    private readonly userRepository: UserRepository
  ) {}

  async getUserProfile(userId: string): Promise<ApiResponse<any>> {
    try {
      const user = await this.userRepository.findById(userId);

      if (!user) {
        return {
          success: false,
          error: {
            code: 'USER_NOT_FOUND',
            message: 'User not found'
          }
        };
      }

      // Exclude sensitive information
      const { password, ...userProfile } = user;

      return {
        success: true,
        data: userProfile
      };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'PROFILE_FETCH_FAILED',
          message: error.message
        }
      };
    }
  }

  async updateUserProfile(userId: string, updateData: any): Promise<ApiResponse<any>> {
    try {
      const user = await this.userRepository.findById(userId);

      if (!user) {
        return {
          success: false,
          error: {
            code: 'USER_NOT_FOUND',
            message: 'User not found'
          }
        };
      }

      // Update user profile fields
      Object.keys(updateData).forEach(key => {
        if (['email', 'name', 'profilePicture'].includes(key)) {
          user[key] = updateData[key];
        }
      });

      await this.userRepository.save(user);

      const { password, ...updatedProfile } = user;

      return {
        success: true,
        data: updatedProfile
      };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 'PROFILE_UPDATE_FAILED',
          message: error.message
        }
      };
    }
  }
}