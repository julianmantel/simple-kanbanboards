import { useState, type ChangeEvent } from "react";
import type { CreateBoardModel } from "../../../boards/types/board";

interface NewBoardModalProps {
  isOpen: boolean;
  projectId: number;
  onClose: () => void;
  onCreate: (board: CreateBoardModel) => void;
}

export default function NewBoardModal({ isOpen, projectId, onClose, onCreate }: NewBoardModalProps) {
  const [formData, setFormData] = useState({ name: "", description: "", isActive: true });
  const [errors, setErrors] = useState<Record<string, string>>({});

  if (!isOpen) return null;

  const resetForm = () => {
    setFormData({ name: "", description: "", isActive: true });
    setErrors({});
  };

  const handleChange = (field: keyof typeof formData) => (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const value = field === "isActive" ? (event.target as HTMLInputElement).checked : event.target.value;
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors((prev) => {
        const next = { ...prev };
        delete next[field];
        return next;
      });
    }
  };

  const handleSubmit = () => {
    const name = formData.name.trim();
    const description = formData.description.trim();

    const nextErrors: Record<string, string> = {};
    if (!name) nextErrors.name = "Board name is required.";
    if (!description) nextErrors.description = "Board description is required.";

    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors);
      return;
    }

    const newBoard: CreateBoardModel = {
      name,
      description,
      isActive: formData.isActive,
      projectId,
    };

    onCreate(newBoard);
    resetForm();
    onClose();
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
            <h2 className="text-xl font-semibold text-text">Create New Board</h2>
            <p className="text-sm text-muted mt-1">Add a board to organize tasks within this project.</p>
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
            <label htmlFor="board-name" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Board name
            </label>
            <input
              id="board-name"
              type="text"
              value={formData.name}
              onChange={handleChange("name")}
              placeholder="Example: Backend Tasks"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.name && <p className="mt-2 text-xs text-danger">{errors.name}</p>}
          </div>

          <div>
            <label htmlFor="board-description" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Description
            </label>
            <textarea
              id="board-description"
              value={formData.description}
              onChange={handleChange("description")}
              placeholder="Brief description of the board's purpose"
              rows={4}
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.description && <p className="mt-2 text-xs text-danger">{errors.description}</p>}
          </div>

          <label className="flex items-center gap-3 cursor-pointer select-none">
            <input
              type="checkbox"
              checked={formData.isActive}
              onChange={handleChange("isActive")}
              className="size-4 rounded border-border bg-surface2 text-teal accent-teal focus:ring-1 focus:ring-teal"
            />
            <span className="text-sm text-text">Active board</span>
          </label>
        </div>

        <div className="flex items-center justify-end gap-3 mt-6 pt-4 border-t border-border">
          <button
            onClick={() => {
              resetForm();
              onClose();
            }}
            className="rounded-lg border border-border bg-surface2 px-5 py-3 text-sm font-medium text-text transition hover:bg-surface3"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            className="rounded-lg bg-teal px-5 py-3 text-sm font-medium text-white transition hover:bg-teal-dim"
          >
            Create board
          </button>
        </div>
      </div>
    </div>
  );
}
