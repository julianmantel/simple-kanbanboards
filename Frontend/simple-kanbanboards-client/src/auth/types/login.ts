export interface LoginFormData {
  username: string;
  password: string;
  rememberMe: boolean;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  succeeded: boolean;
  result: string;
  errors: string[];
}

export interface LoginErrors {
  general?: string;
  username?: string;
  password?: string;
}