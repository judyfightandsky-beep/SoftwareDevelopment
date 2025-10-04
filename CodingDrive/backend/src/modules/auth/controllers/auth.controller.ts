import { Controller, Get, Post, Body, Query, Req, Res } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { ApiResponse } from '../../../common/interfaces/api-response.interface';
import { Request, Response } from 'express';

@Controller('api/auth')
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Get('verify-email')
  async verifyEmail(
    @Query('token') token: string,
    @Res() res: Response
  ): Promise<void> {
    const result: ApiResponse<boolean> = await this.authService.verifyEmail(token);

    if (result.success) {
      res.redirect('/login?verified=true');
    } else {
      res.redirect('/login?verified=false&error=' + encodeURIComponent(result.error?.message || 'Unknown error'));
    }
  }

  @Post('forgot-password')
  async forgotPassword(
    @Body() body: { email: string }
  ): Promise<ApiResponse<boolean>> {
    return this.authService.requestPasswordReset(body.email);
  }

  @Post('reset-password')
  async resetPassword(
    @Body() body: { token: string; newPassword: string }
  ): Promise<ApiResponse<boolean>> {
    return this.authService.resetPassword(body.token, body.newPassword);
  }

  @Post('logout')
  async logout(
    @Req() req: Request,
    @Res() res: Response
  ): Promise<void> {
    const token = req.headers.authorization?.split(' ')[1];
    const result = await this.authService.logout(token);

    if (result.success) {
      res.clearCookie('jwt');
      res.status(200).json(result);
    } else {
      res.status(400).json(result);
    }
  }

  @Post('refresh-token')
  async refreshToken(
    @Body() body: { refreshToken: string }
  ): Promise<ApiResponse<{ accessToken: string; refreshToken: string }>> {
    return this.authService.refreshToken(body.refreshToken);
  }
}