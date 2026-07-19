import type { BoardModel } from "../types/board";

interface BoardDetailsProps {
  board: BoardModel;
}

export default function BoardDetails({ board }: BoardDetailsProps) {
  return (
    <div className="mb-8">
      <div className="flex items-center gap-3">
        <div className={`size-3 rounded-full ${board.is_Active ? "bg-teal" : "bg-muted"}`} />
        <h1 className="text-2xl font-heading font-semibold text-text tracking-tight">{board.name}</h1>
      </div>
      <p className="text-sm text-muted mt-3">{board.description}</p>
    </div>
  );
}
