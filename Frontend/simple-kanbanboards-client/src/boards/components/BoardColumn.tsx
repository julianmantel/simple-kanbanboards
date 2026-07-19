import type { BoardColumnModel } from "../types/boardColumn";
import type { TaskModel } from "../../tasks/types/task";
import TaskCard from "./TaskCard";

interface BoardColumnProps {
  column: BoardColumnModel;
  tasks: TaskModel[];
}

const COLUMN_DOTS = [
  "bg-muted",
  "bg-teal",
  "bg-teal-light",
  "bg-teal-dim",
];

export default function BoardColumn({ column, tasks }: BoardColumnProps) {
  const dotColor = COLUMN_DOTS[column.position % COLUMN_DOTS.length];

  return (
    <div className="w-60 shrink-0 bg-surface border border-border rounded-xl flex flex-col overflow-hidden">
      <div className="flex items-center justify-between px-3.5 py-3 border-b border-border">
        <div className="flex items-center gap-2 min-w-0">
          <div className={`size-2 rounded-full shrink-0 ${dotColor}`} />
          <span className="text-sm font-medium text-text truncate">{column.name}</span>
          <span className="text-xs text-muted bg-surface0 rounded-full px-1.5 py-0.5 border border-border shrink-0">
            {tasks.length}
          </span>
        </div>
        <button className="text-muted hover:text-text shrink-0 ml-1" aria-label="Column menu">
          <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M12 6.75a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5ZM12 12.75a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5ZM12 18.75a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z" />
          </svg>
        </button>
      </div>

      <div className="flex flex-col gap-2 p-2.5 flex-1">
        {tasks.map((task) => (
          <TaskCard key={task.id} task={task} />
        ))}
      </div>

      <button className="flex items-center gap-1.5 mx-2.5 mb-2.5 px-2 py-1.5 text-xs text-muted hover:text-text hover:bg-surface0 rounded-lg transition-colors">
        <svg className="size-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
          <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
        </svg>
        Add card
      </button>
    </div>
  );
}
