import { httpClient } from '../../shared/config/httpClient';
import { AUTH_ENDPOINTS } from './authEndpoints';
import type { LoginRequest, LoginResponse } from '../types/login';
import type { ResetPasswordRequest, ChangePasswordRequest } from '../types/passwordReset';
import type { RegisterRequest } from '../types/register';
import type { Role, User } from '../types/user';
import type { ApiResult } from '../../shared/types/apiResult.type';

export const authApi = {
  login: async (payload: LoginRequest): Promise<LoginResponse> => {
    const { data } = await httpClient.post<LoginResponse>(AUTH_ENDPOINTS.login, payload);
    return data;
  },

  logout: async (): Promise<void> => {
    await httpClient.post(AUTH_ENDPOINTS.logout);
  },

  register: async (payload: RegisterRequest): Promise<void> => {
    await httpClient.post(AUTH_ENDPOINTS.register, payload);
  },

  requestPasswordReset: async (payload: ResetPasswordRequest): Promise<void> => {
    await httpClient.post(AUTH_ENDPOINTS.resetPassword, payload);
  },

  changePassword: async (payload: ChangePasswordRequest): Promise<void> => {
    await httpClient.post(AUTH_ENDPOINTS.changePassword, payload);
  },

  getUser: async (userId: number): Promise<User> => {
    const { data } = await httpClient.get<ApiResult<User>>(AUTH_ENDPOINTS.get_user_by_id(userId));
    return data.result;
  },

  deleteUser: async (userId: number): Promise<void> => {
    await httpClient.delete(AUTH_ENDPOINTS.deleteUser(userId));
  },

  me: async (): Promise<User> => {
    const { data } = await httpClient.get<ApiResult<User>>(AUTH_ENDPOINTS.me);
    return data.result;
  },

  getRoles: async (): Promise<Role[]> => {
    const { data } = await httpClient.get<ApiResult<Role[]>>(AUTH_ENDPOINTS.get_roles);
    return data.result;
  },

  changeRoles: async (payload: User): Promise<void> => {
    await httpClient.post(AUTH_ENDPOINTS.change_roles, payload);
  }
};