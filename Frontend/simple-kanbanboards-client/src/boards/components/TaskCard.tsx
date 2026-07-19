import type { TaskModel } from "../../tasks/types/task";
import { useAuth } from "../../auth/context/AuthContext";

interface TaskCardProps {
  task: TaskModel;
}

const PRIORITY_CONFIG: Record<number, { label: string; className: string }> = {
  1: { label: "Alta", className: "bg-danger/10 text-danger" },
  2: { label: "Media", className: "bg-amber/10 text-amber" },
  3: { label: "Baja", className: "bg-teal/10 text-teal-light" },
};

export default function TaskCard({ task }: TaskCardProps) {
  const { user } = useAuth();
  const priority = PRIORITY_CONFIG[task.priority] ?? { label: "Unknown", className: "bg-surface2 text-muted" };
  const initials = user?.userName?.slice(0, 2).toUpperCase() ?? "??";

  return (
    <div className="bg-surface2 border border-border rounded-lg p-2.5 cursor-grab">
      <p className="text-sm text-text leading-snug">{task.title}</p>
      <div className="flex items-center gap-2 mt-2">
        <span className={`inline-flex px-2 py-0.5 rounded-full text-xs font-medium ${priority.className}`}>
          {priority.label}
        </span>
        <div className="size-5 rounded-full bg-surface0 border border-border flex items-center justify-center text-xs font-medium text-muted ml-auto">
          {initials}
        </div>
      </div>
    </div>
  );
}
