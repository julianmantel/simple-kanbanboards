import { useParams, Link } from "react-router-dom";
import { mockBoards } from "../data/mockBoards";

export default function BoardPage() {
  const { projectId, boardId } = useParams<{ projectId: string; boardId: string }>();
  const board = mockBoards.find((b) => b.id === Number(boardId));

  if (!board) {
    return (
      <div className="text-center py-16">
        <p className="text-muted text-sm">Board not found.</p>
        <Link to={`/projects/${projectId}`} className="text-teal-light hover:underline text-sm inline-block">
          Back to Project
        </Link>
      </div>
    );
  }

  return (
    <div>
      <Link
        to={`/projects/${projectId}`}
        className="inline-flex items-center gap-1.5 text-sm text-muted hover:text-text transition-colors mb-6"
      >
        <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
          <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5 8.25 12l7.5-7.5" />
        </svg>
        Back to Project
      </Link>

      <div className="flex items-center gap-3 mb-8">
        <div className={`size-3 rounded-full ${board.isActive ? "bg-teal" : "bg-muted"}`} />
        <h1 className="text-2xl font-heading font-semibold text-text tracking-tight">{board.name}</h1>
      </div>

      <p className="text-sm text-muted mb-8">{board.description}</p>

      <div className="flex items-center justify-center h-64 border-2 border-dashed border-border rounded-default">
        <p className="text-sm text-muted">Board view coming soon...</p>
      </div>
    </div>
  );
}
