import { createContext, useContext, useEffect, useState, type ReactNode } from 'react';
import { authApi } from '../api/authApi';
import type { User } from '../types/user';
import type { LoginRequest } from '../types/login';

interface AuthContextValue {
  user: User | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  login: (loginRequest: LoginRequest) => Promise<void>;
  logout: () => Promise<void>;
  refetchUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const fetchUser = async () => {
    try {
      const currentUser = await authApi.me();
      setUser(currentUser);
    } catch {
      console.error('Error al obtener el usuario actual');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchUser(); 
  }, []);

  const login = async (loginRequest: LoginRequest) => {
    await authApi.login(loginRequest);
    
    await fetchUser();
  };

  const logout = async () => {
    await authApi.logout();
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isLoading,
        isAuthenticated: !!user,
        login,
        logout,
        refetchUser: fetchUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth debe usarse dentro de un AuthProvider');
  return context;
}