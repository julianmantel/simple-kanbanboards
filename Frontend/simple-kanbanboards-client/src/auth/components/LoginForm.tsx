import { useNavigate } from 'react-router-dom';
import { useState, type SubmitEvent } from 'react';
import type { LoginFormData, LoginErrors, LoginRequest } from '../types/login';
import { useAuth } from '../context/AuthContext';

export default function LoginForm() {
  const [formData, setFormData] = useState<LoginFormData>({
    username: '',
    password: '',
    rememberMe: false,
  });
  const [errors, setErrors] = useState<LoginErrors>({});
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const navigate = useNavigate();
  const { login } = useAuth();

  const validate = (): boolean => {
    const e: LoginErrors = {};
    if (!formData.username) e.username = 'Username is required';
    if (!formData.password) e.password = 'Password is required';
    else if (formData.password.length < 8)
      e.password = 'The password must be at least 8 characters long';
    setErrors(e);
    return Object.keys(e).length === 0;
  };

  const handleSubmit = async (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!validate()) return;

    const loginRequest: LoginRequest = {
      username: formData.username,
      password: formData.password,
    };

    setIsLoading(true);
    try {
      await login(loginRequest);

      navigate('/home');
    } catch (error) {
      setErrors({ general: (error as Error).message });
    } finally {
      setIsLoading(false);
    }
  };

  const update = (field: keyof LoginFormData, value: string | boolean) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors[field as keyof LoginErrors])
      setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  return (
    <form onSubmit={handleSubmit}>
      {errors.general && (
        <p className="text-xs text-danger mt-1" role="alert">{errors.general}</p>
      )}

      <div className="space-y-4 ">
        <div>
          <label htmlFor="l-username" className="block text-xs text-muted mb-1 tracking-wide">
            Username
          </label>
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
            </svg>
            <input
              id="l-username"
              type="text"
              placeholder="Julian"
              value={formData.username}
              onChange={(e) => update('username', e.target.value)}
              className="w-full py-2 pl-9 pr-3 bg-surface2 border border-border rounded-lg text-sm text-text placeholder:text-muted outline-none transition-colors focus:border-teal"
            />
          </div>
          {errors.username && (
            <p className="text-xs text-danger mt-1" role="alert">{errors.username}</p>
          )}
        </div>

        <div>
          <label htmlFor="l-pass" className="block text-xs text-muted mb-1 tracking-wide">
            Password
          </label>
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
            <input
              id="l-pass"
              type={showPassword ? 'text' : 'password'}
              placeholder="••••••••"
              value={formData.password}
              onChange={(e) => update('password', e.target.value)}
              className="w-full py-2 pl-9 pr-3 bg-surface2 border border-border rounded-lg text-sm text-text placeholder:text-muted outline-none transition-colors focus:border-teal"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-2.5 top-1/2 -translate-y-1/2 text-muted hover:text-text transition-colors"
              aria-label={showPassword ? 'Ocultar contraseña' : 'Mostrar contraseña'}
            >
              {showPassword ? (
                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                  <path strokeLinecap="round" strokeLinejoin="round" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
              ) : (
                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                  <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path strokeLinecap="round" strokeLinejoin="round" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              )}
            </button>
          </div>
          <a href="#" className="block text-xs text-teal-light text-right -mt-1.5 mt-2 hover:underline">
            Forgot your password?
          </a>
          {errors.password && (
            <p className="text-xs text-danger mt-1" role="alert">{errors.password}</p>
          )}
        </div>

        <label className="flex items-center gap-2 text-xs text-muted cursor-pointer">
          <input
            type="checkbox"
            checked={formData.rememberMe}
            onChange={(e) => update('rememberMe', e.target.checked)}
            className="w-4 h-4 accent-teal cursor-pointer"
          />
          Remember me
        </label>
      </div>

      <div id="l-error" role="alert" className="text-xs text-danger mt-2" hidden>
        Invalid username or password.
      </div>

      <button
        type="submit"
        className="w-full py-2.5 px-4 bg-none text-white text-sm font-medium rounded-lg border border-teal-light transition-colors duration-200 mt-5 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-teal"
        disabled={isLoading}
      >
        {isLoading ? 'Logging in...' : 'Log in →'}
      </button>
    </form>
  );
}
