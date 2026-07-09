import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function ProtectedRoute() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) return <div>Cargando...</div>; // evita un flash de redirect antes de saber si hay sesión

  return isAuthenticated ? <Outlet /> : <Navigate to="/" replace />;
}