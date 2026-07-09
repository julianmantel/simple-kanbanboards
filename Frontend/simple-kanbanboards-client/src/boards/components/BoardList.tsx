import { mockBoards } from "../data/mockBoards";
import BoardCard from "./BoardCard";

export default function BoardList({ projectId }: { projectId: number }) {
  const boards = mockBoards.filter((b) => b.projectId === projectId);

  if (boards.length === 0) {
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
