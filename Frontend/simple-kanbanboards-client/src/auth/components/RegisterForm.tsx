import { useState, type SubmitEvent } from 'react';
import StrengthBar from './StrengthBar';
import type { RegisterFormData, RegisterErrors, RegisterRequest } from '../types/register';
import { authApi } from '../api/authApi';

export default function RegisterForm() {
  const [formData, setFormData] = useState<RegisterFormData>({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const [errors, setErrors] = useState<RegisterErrors>({});
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const validate = (): boolean => {
    const e: RegisterErrors = {};
    if (!formData.username) e.username = 'Username is required';
    if (!formData.email) e.email = 'Email is required';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email))
      e.email = 'Please enter a valid email address';
    if (!formData.password) e.password = 'Password is required';
    else if (formData.password.length < 8)
      e.password = 'Password must be at least 8 characters long';
    if (formData.password !== formData.confirmPassword)
      e.confirmPassword = 'Passwords do not match';
    setErrors(e);
    return Object.keys(e).length === 0;
  };

  const handleSubmit = async (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!validate()) return;

    const registerRequest: RegisterRequest = {
      username: formData.username,
      email: formData.email,
      password: formData.password,
      roles: [1]
    };

    setIsLoading(true);
    try {
      await authApi.register(registerRequest);

    } catch (err) {
      throw new Error(`Error registering user: ${(err as Error).message}`);
    } finally {
      setIsLoading(false);
    }

  };

  const update = (field: keyof RegisterFormData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors[field as keyof RegisterErrors])
      setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="space-y-4">
        <div>
            <label htmlFor="r-name" className="block text-xs text-muted mb-1 tracking-wide">
              Username
            </label>
            <div className="relative">
              <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
              <input
                id="r-name"
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
          <label htmlFor="r-email" className="block text-xs text-muted mb-1 tracking-wide">
            Email
          </label>
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
            </svg>
            <input
              id="r-email"
              type="email"
              placeholder="you@email.com"
              value={formData.email}
              onChange={(e) => update('email', e.target.value)}
              className="w-full py-2 pl-9 pr-3 bg-surface2 border border-border rounded-lg text-sm text-text placeholder:text-muted outline-none transition-colors focus:border-teal"
            />
          </div>
          {errors.email && (
            <p className="text-xs text-danger mt-1" role="alert">{errors.email}</p>
          )}
        </div>

        <div>
          <label htmlFor="r-pass" className="block text-xs text-muted mb-1 tracking-wide">
            Password
          </label>
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
            <input
              id="r-pass"
              type={showPassword ? 'text' : 'password'}
              placeholder="Mínimo 8 caracteres"
              value={formData.password}
              onChange={(e) => update('password', e.target.value)}
              className="w-full py-2 pl-9 pr-3 bg-surface2 border border-border rounded-lg text-sm text-text placeholder:text-muted outline-none transition-colors focus:border-teal"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-2.5 top-1/2 -translate-y-1/2 text-muted hover:text-text transition-colors"
              aria-label={showPassword ? 'Hide password' : 'Show password'}
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
          <StrengthBar value={formData.password} />
          {errors.password && (
            <p className="text-xs text-danger mt-1" role="alert">{errors.password}</p>
          )}
        </div>

        <div>
          <label htmlFor="r-confirm" className="block text-xs text-muted mb-1 tracking-wide">
            Confirm Password
          </label>
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted pointer-events-none" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
            <input
              id="r-confirm"
              type={showPassword ? 'text' : 'password'}
              placeholder="••••••••"
              value={formData.confirmPassword}
              onChange={(e) => update('confirmPassword', e.target.value)}
              className="w-full py-2 pl-9 pr-3 bg-surface2 border border-border rounded-lg text-sm text-text placeholder:text-muted outline-none transition-colors focus:border-teal"
            />
          </div>
          {errors.confirmPassword && (
            <p className="text-xs text-danger mt-1" role="alert">{errors.confirmPassword}</p>
          )}
        </div>
      </div>

      <div id="r-error" role="alert" className="text-xs text-danger mt-2" hidden>
        Please fill in all fields.
      </div>

      <button
        type="submit"
        className="w-full py-2.5 px-4 bg-none text-white text-sm font-medium rounded-lg border border-teal-light transition-colors duration-200 mt-5 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-teal"
        disabled={isLoading}
      >
        {isLoading ? 'Registering...' : 'Create account →'}
      </button>
    </form>
  );
}
