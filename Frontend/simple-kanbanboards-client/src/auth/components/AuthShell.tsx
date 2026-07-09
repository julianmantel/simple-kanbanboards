import LeftPanel from './LeftPanel';
import RightPanel from './RightPanel';

export default function AuthShell() {
  return (
    <div className="min-h-screen flex items-center justify-center p-6 bg-[#0f0f11]">
      <h1 className="sr-only">KanbanFlow — Inicio de sesión y registro</h1>
      <div className="w-full max-w-5xl grid lg:grid-cols-2 min-h-[480px] rounded-xl overflow-hidden border border-border bg-surface shadow-[0_0_60px_-15px_rgba(29,158,117,0.15)]">
        <LeftPanel />
        <RightPanel />
      </div>
    </div>
  );
}
