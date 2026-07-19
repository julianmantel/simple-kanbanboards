import { useEffect, useState } from "react";
import { boardColumnsApi } from "../api/boardsApi";
import { tasksApi } from "../../tasks/api/tasksApi";
import BoardColumn from "./BoardColumn";
import type { BoardColumnModel } from "../types/boardColumn";
import type { TaskModel } from "../../tasks/types/task";

interface BoardViewProps {
  boardId: number;
}

export default function BoardView({ boardId }: BoardViewProps) {
  const [columns, setColumns] = useState<BoardColumnModel[]>([]);
  const [tasksByColumn, setTasksByColumn] = useState<Record<number, TaskModel[]>>({});
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
      setLoading(true);
      try {
        const cols = await boardColumnsApi.getBoardColumnsByBoard(boardId);
        setColumns(cols);

        const tasksMap: Record<number, TaskModel[]> = {};
        const results = await Promise.all(
          cols.map((col) => tasksApi.getTasksByBoardColumn(col.id)),
        );
        cols.forEach((col, i) => {
          tasksMap[col.id] = results[i];
        });
        setTasksByColumn(tasksMap);
      } catch (error) {
        console.error("Error loading board data:", error);
      } finally {
        setLoading(false);
      }
    };

  useEffect(() => {
    fetchData();
  }, [boardId]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-sm text-muted">Loading board...</p>
      </div>
    );
  }

  return (
    <div>
      <div className="overflow-x-auto pb-2">
        <div className="flex gap-3 min-w-min">
          {columns.map((column) => (
            <BoardColumn
              key={column.id}
              column={column}
              tasks={tasksByColumn[column.id] ?? []}
            />
          ))}

          <button className="w-60 shrink-0 border-2 border-dashed border-border rounded-xl flex items-center justify-center gap-2 text-sm text-muted hover:text-teal-light hover:border-teal transition-colors min-h-[8rem]">
            <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
            </svg>
            New column
          </button>
        </div>
      </div>

      {columns.length > 0 && (
        <div className="flex items-center gap-1.5 text-xs text-muted mt-2.5">
          <svg className="size-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M8.25 15 12 18.75 15.75 15m-7.5-6L12 5.25 15.75 9" />
          </svg>
          Scroll horizontally to see more columns
        </div>
      )}
    </div>
  );
}
