export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    userId: string;
    username: string;
    email: string;
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
    firstName: string;
    lastName: string;
    role: UserRole;
    status: UserStatus;
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
