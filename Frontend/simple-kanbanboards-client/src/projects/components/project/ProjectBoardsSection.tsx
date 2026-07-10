import { useState } from "react";
import BoardList from "../../../boards/components/BoardList";
import NewBoardModal from "./NewBoardModal";
import type { CreateBoardModel } from "../../../boards/types/board";

interface ProjectBoardsSectionProps {
  projectId: number;
  onCreateBoard: (board: CreateBoardModel) => void;
}

export default function ProjectBoardsSection({ projectId, onCreateBoard }: ProjectBoardsSectionProps) {
  const [showNewBoardModal, setShowNewBoardModal] = useState(false);

  return (
    <div>
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-lg font-heading font-semibold text-text">Boards</h2>
        <button
          onClick={() => setShowNewBoardModal(true)}
          className="flex items-center gap-1.5 px-3 py-1.5 bg-teal hover:bg-teal-dim transition-colors text-white text-sm font-medium rounded-lg"
        >
          <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
          </svg>
          New Board
        </button>
      </div>

      <BoardList projectId={projectId} />

      <NewBoardModal
        isOpen={showNewBoardModal}
        projectId={projectId}
        onClose={() => setShowNewBoardModal(false)}
        onCreate={(board) => {
          onCreateBoard(board);
          setShowNewBoardModal(false);
        }}
      />
    </div>
  );
}
