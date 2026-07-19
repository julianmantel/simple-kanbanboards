import { useParams, Link } from "react-router-dom";
import { boardsApi } from "../api/boardsApi";
import { useEffect, useState } from "react";
import type { BoardModel, UpdateBoardModel } from "../types/board";
import BoardToolbar from "../components/BoardToolbar";
import BoardDetails from "../components/BoardDetails";
import BoardView from "../components/BoardView";
import EditBoardModal from "../components/EditBoardModal";

export default function BoardPage() {
  const { projectId, boardId } = useParams<{ projectId: string; boardId: string }>();
  const [board, setBoard] = useState<BoardModel | undefined>(undefined);
  const [showEditModal, setShowEditModal] = useState(false);

  const fetchBoard = async () => {
    if (!projectId || !boardId) return;

    try {
      const boardData = await boardsApi.getBoardById(parseInt(boardId));

      if (boardData?.projectId !== parseInt(projectId)) {
        setBoard(undefined);
        console.error("Board does not belong to the specified project.");
      } else {
        setBoard(boardData);
      }
    } catch (error) {
      console.error("Error fetching board:", error);
      setBoard(undefined);
    }
  };

  useEffect(() => {
    fetchBoard();
  }, [projectId, boardId]);

  const handleSave = async (payload: UpdateBoardModel) => {
    if (!board) return;
    await boardsApi.updateBoard({
      id: board.id,
      name: payload.name,
      description: payload.description
    });
    setBoard((prev) =>
      prev ? { ...prev, name: payload.name, description: payload.description } : prev,
    );
    setShowEditModal(false);
  };

  const handleToggle = async () => {
    if (!board) return;
    await boardsApi.toggleBoard(board.id);
    setBoard((prev) => (prev ? { ...prev, is_Active: !prev.is_Active } : prev));
  };

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
      <BoardToolbar
        projectId={parseInt(projectId!)}
        onEdit={() => setShowEditModal(true)}
        onToggle={handleToggle}
        isActive={board.is_Active}
      />

      <BoardDetails board={board} />

      <BoardView boardId={board.id} />

      <EditBoardModal
        isOpen={showEditModal}
        board={board}
        onSave={handleSave}
        onClose={() => setShowEditModal(false)}
      />
    </div>
  );
}
