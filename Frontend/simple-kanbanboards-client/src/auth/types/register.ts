export interface RegisterFormData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  roles?: number[];
}

export interface RegisterErrors {
  username?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
}