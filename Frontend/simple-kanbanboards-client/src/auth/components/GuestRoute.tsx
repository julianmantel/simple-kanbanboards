import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function GuestRoute() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) return <div className="min-h-screen bg-bg-dark" />;

  return isAuthenticated ? <Navigate to="/home" replace /> : <Outlet />;
}
