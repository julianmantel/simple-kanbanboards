import BoardCard from "./BoardCard";
import type { BoardModel } from "../types/board";

interface BoardListProps {
  projectId: number;
  boards?: BoardModel[];
};

export default function BoardList({ projectId, boards }: BoardListProps) {

  if (!boards || boards.length === 0) {
    return (
      <p className="text-sm text-muted py-8 text-center">
        No boards yet for this project.
      </p>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {boards.map((board) => (
        <BoardCard key={board.id} board={board} projectId={projectId} />
      ))}
    </div>
  );
}
