import { useNavigate } from "react-router-dom";
import type { BoardModel } from "../types/board";

export default function BoardCard({ board, projectId }: { board: BoardModel; projectId: number }) {
  const navigate = useNavigate();

  return (
    <div
      onClick={() => navigate(`/projects/${projectId}/boards/${board.id}`)}
      className="group bg-surface border border-border rounded-default p-5 hover:border-teal/30 hover:shadow-[0_0_30px_-10px_rgba(29,158,117,0.15)] transition-all duration-200 cursor-pointer"
    >
      <div className="flex items-start justify-between mb-3">
        <div className={`w-10 h-1 rounded-full ${board.is_Active ? "bg-teal" : "bg-muted"}`} />
        <span
          className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-xs font-medium ${
            board.is_Active
              ? "bg-teal/10 text-teal-light"
              : "bg-surface2 text-muted"
          }`}
        >
          <span className={`size-1.5 rounded-full ${board.is_Active ? "bg-teal" : "bg-muted"}`} />
          {board.is_Active ? "Active" : "Inactive"}
        </span>
      </div>

      <h3 className="text-sm font-semibold text-text group-hover:text-teal-light transition-colors leading-snug mb-1.5">
        {board.name}
      </h3>

      <p className="text-xs text-muted leading-relaxed line-clamp-2 mb-4">
        {board.description}
      </p>

      <div className="flex items-center gap-2 text-xs text-muted pt-3 border-t border-border">
        <svg className="size-3.5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
          <path strokeLinecap="round" strokeLinejoin="round" d="M12 6v6h4.5m4.5 0a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
        </svg>
        Created {new Date(board.created_At).toLocaleDateString("en-US", {
          month: "short",
          day: "numeric",
          year: "numeric",
        })}
      </div>
    </div>
  );
}
