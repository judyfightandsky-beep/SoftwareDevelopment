export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    userId: string;
    username: string;
    email: string;
    accessToken: string;
    refreshToken: string;
}

export interface LoginResponseLegacy {
    accessToken: string;
    refreshToken: string;
    tokenType: string;
    expiresIn: number;
    user: UserResponse;
}

export interface UserResponse {
    id: string;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    isEmailVerified: boolean;
    createdAt: string;
    lastLoginAt?: string;
}

export interface AuthErrorResponse {
    message: string;
}

export interface ApiResponse<T> {
    success: boolean;
    message?: string;
    data?: T;
}

export interface AvailabilityResponse {
    available: boolean;
    message?: string;
}

export interface UserRegistrationRequest {
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
}

export interface UserRegistrationResponse {
    userId: string;
    username: string;
    email: string;
    role: string;
    status: string;
    requiresApproval: boolean;
}

export enum UserRole {
    Guest = 'GUEST',
    Member = 'MEMBER',
    Admin = 'ADMIN'
}

export enum UserStatus {
    Pending = 'PENDING',
    Active = 'ACTIVE',
    Suspended = 'SUSPENDED'
}

export interface PasswordResetRequest {
    email: string;
}

export interface PasswordResetConfirmRequest {
    token: string;
    newPassword: string;
}

export interface EmailVerificationRequest {
    token: string;
}
