import { useAuth } from "../../auth/context/AuthContext";

interface HomeHeaderProps {
  onNewProject: () => void;
}

export default function HomeHeader({ onNewProject }: HomeHeaderProps) {
  const { user } = useAuth();
  const displayName = user?.userName ?? "User";

  return (
    <div className="flex items-center justify-between mt-12 mb-8">
      <div>
        <h1 className="text-2xl font-heading font-semibold text-text tracking-tight">
          Welcome back, <span className="text-teal-light">{displayName}</span>
        </h1>
      </div>
      <button
        type="button"
        onClick={onNewProject}
        className="flex items-center gap-2 px-4 py-2.5 bg-teal hover:bg-teal-dim transition-colors text-white text-sm font-medium rounded-default shadow-[0_0_20px_-5px_rgba(29,158,117,0.3)]"
      >
        <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
          <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
        </svg>
        New Project
      </button>
    </div>
  );
}
