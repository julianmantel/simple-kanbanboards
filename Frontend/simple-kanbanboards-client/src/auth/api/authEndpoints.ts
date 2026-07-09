export const AUTH_ENDPOINTS = {
  login: '/users/login/',
  logout: '/users/logout/',
  register: '/users/',
  resetPassword: '/users/reset-password/',
  changePassword: '/users/change-password/',
  get_user_by_id: (id: number) => `/users/${id}/`,
  deleteUser: (id: number) => `/users/${id}/`,
  me: '/users/me/',
  get_roles: '/roles/',
  change_roles: '/users/change-roles/',
} as const;