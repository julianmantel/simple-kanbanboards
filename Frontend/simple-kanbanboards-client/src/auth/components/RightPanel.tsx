import { useState } from 'react';
import LoginForm from './LoginForm';
import RegisterForm from './RegisterForm';
import SocialButtons from './SocialButtons';

type AuthMode = 'login' | 'register';

export default function RightPanel() {
  const [mode, setMode] = useState<AuthMode>('login');

  const handleTabClick = (newMode: AuthMode) => {
    if (newMode === mode) return;
    setMode(newMode);
  };

  return (
    <div className="p-10 flex flex-col justify-center bg-surface">
      {/* Tabs con píldora deslizante */}
      <div className="relative flex bg-surface2 rounded-lg p-0.5 mb-7 gap-0.5" role="tablist">
        <div
          className="absolute top-0.5 bottom-0.5 rounded-md bg-teal transition-all duration-200 ease-in-out"
          style={{
            left: mode === 'login' ? '2px' : 'calc(50% + 1px)',
            width: 'calc(50% - 3px)',
          }}
        />
        <button
          className={`relative z-10 flex-1 py-1.5 text-sm font-medium rounded-md transition-colors duration-200 ${
            mode === 'login' ? 'text-white' : 'text-muted hover:text-text'
          }`}
          role="tab"
          aria-selected={mode === 'login'}
          onClick={() => handleTabClick('login')}
        >
          Ingresar
        </button>
        <button
          className={`relative z-10 flex-1 py-1.5 text-sm font-medium rounded-md transition-colors duration-200 ${
            mode === 'register' ? 'text-white' : 'text-muted hover:text-text'
          }`}
          role="tab"
          aria-selected={mode === 'register'}
          onClick={() => handleTabClick('register')}
        >
          Registrarse
        </button>
      </div>

      {/* Transición fade + slide entre formularios */}
      <div className="relative min-h-[420px]">
        <div
          className={`absolute inset-0 transition-all duration-200 ease-out ${
            mode === 'login'
              ? 'opacity-100 translate-x-0 z-10'
              : 'opacity-0 -translate-x-4 pointer-events-none z-0'
          }`}
        >
          <LoginForm />
        </div>
        <div
          className={`absolute inset-0 transition-all duration-200 ease-out ${
            mode === 'register'
              ? 'opacity-100 translate-x-0 z-10'
              : 'opacity-0 translate-x-4 pointer-events-none z-0'
          }`}
        >
          <RegisterForm />
        </div>
      </div>

      <div className="flex items-center gap-2.5 my-5">
        <span className="flex-1 h-px bg-border" />
      </div>

      <SocialButtons />
    </div>
  );
}
