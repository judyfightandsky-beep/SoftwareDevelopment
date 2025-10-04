import { Controller, Get, Put, Body, Req, UseGuards } from '@nestjs/common';
import { UsersService } from '../services/users.service';
import { ApiResponse } from '../../../common/interfaces/api-response.interface';
import { JwtAuthGuard } from '../../../common/guards/jwt-auth.guard';
import { Request } from 'express';

@Controller('api/users')
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  @Get('profile')
  @UseGuards(JwtAuthGuard)
  async getUserProfile(
    @Req() req: Request
  ): Promise<ApiResponse<any>> {
    const userId = req.user['sub'];
    return this.usersService.getUserProfile(userId);
  }

  @Put('profile')
  @UseGuards(JwtAuthGuard)
  async updateUserProfile(
    @Req() req: Request,
    @Body() updateData: any
  ): Promise<ApiResponse<any>> {
    const userId = req.user['sub'];
    return this.usersService.updateUserProfile(userId, updateData);
  }
}