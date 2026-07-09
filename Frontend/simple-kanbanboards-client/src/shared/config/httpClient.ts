import axios, { AxiosError } from 'axios';
import { API_BASE_URL } from './api';

export const httpClient = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

httpClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<{ message?: string }>) => {
    const message = error.response?.data?.message || 'Ocurrió un error inesperado';
    return Promise.reject(new Error(message));
  }
);