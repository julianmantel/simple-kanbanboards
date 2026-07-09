export interface ResetPasswordRequest {
  email: string;
}

export interface ChangePasswordRequest {
  token: string;
  newPassword: string;
}

export interface ResetPasswordErrors {
  general?: string;
  email?: string;
}

export interface ChangePasswordErrors {
  general?: string;
  newPassword?: string;
  confirmPassword?: string;
}