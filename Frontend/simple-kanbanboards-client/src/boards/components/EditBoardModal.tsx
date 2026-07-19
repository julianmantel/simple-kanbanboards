import { useState, type ChangeEvent } from "react";
import type { BoardModel, UpdateBoardModel } from "../types/board";

interface EditBoardModalProps {
  isOpen: boolean;
  board: BoardModel;
  onSave: (payload: UpdateBoardModel) => Promise<void>;
  onClose: () => void;
}

export default function EditBoardModal({ isOpen, board, onSave, onClose }: EditBoardModalProps) {
  const [name, setName] = useState(board.name);
  const [description, setDescription] = useState(board.description);
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  if (!isOpen) return null;

  const validate = (): boolean => {
    const next: Record<string, string> = {};
    if (!name.trim()) next.name = "Board name is required.";
    if (!description.trim()) next.description = "Board description is required.";
    setErrors(next);
    return Object.keys(next).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;

    setIsSaving(true);
    try {
      await onSave({
        id: board.id,
        name: name.trim(),
        description: description.trim(),
      });
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center px-4 py-6">
      <div className="fixed inset-0 bg-black/40 backdrop-blur-sm" onClick={onClose} />
      <div
        className="relative w-full max-w-xl rounded-default border border-border bg-surface p-6 shadow-[0_0_45px_-14px_rgba(0,0,0,0.45)]"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-start justify-between gap-4">
          <div>
            <h2 className="text-xl font-semibold text-text">Edit Board</h2>
            <p className="text-sm text-muted mt-1">Update the board name and description.</p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-muted hover:text-text shrink-0"
            aria-label="Close modal"
          >
            <svg className="size-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M6 18 18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <div className="mt-6 space-y-5">
          <div>
            <label htmlFor="edit-board-name" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Board name
            </label>
            <input
              id="edit-board-name"
              type="text"
              value={name}
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setName(e.target.value);
                if (errors.name) setErrors((prev) => ({ ...prev, name: "" }));
              }}
              placeholder="Example: Backend Tasks"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.name && <p className="mt-2 text-xs text-danger">{errors.name}</p>}
          </div>

          <div>
            <label htmlFor="edit-board-description" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Description
            </label>
            <textarea
              id="edit-board-description"
              value={description}
              onChange={(e: ChangeEvent<HTMLTextAreaElement>) => {
                setDescription(e.target.value);
                if (errors.description) setErrors((prev) => ({ ...prev, description: "" }));
              }}
              placeholder="Brief description of the board's purpose"
              rows={4}
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.description && <p className="mt-2 text-xs text-danger">{errors.description}</p>}
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <button
              onClick={onClose}
              className="rounded-lg border border-border bg-surface2 px-5 py-3 text-sm font-medium text-text transition hover:bg-surface3"
            >
              Cancel
            </button>
            <button
              onClick={handleSubmit}
              disabled={isSaving}
              className="rounded-lg bg-teal px-5 py-3 text-sm font-medium text-white transition hover:bg-teal-dim disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isSaving ? "Saving..." : "Save changes"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
