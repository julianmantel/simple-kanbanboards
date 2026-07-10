import { useState, type ChangeEvent } from "react";
import type { ProjectModel, UpdateProjectModel } from "../../types/project";

interface EditProjectModalProps {
  isOpen: boolean;
  project: ProjectModel;
  onSave: (payload: UpdateProjectModel) => Promise<void>;
  onClose: () => void;
}

export default function EditProjectModal({ isOpen, project, onSave, onClose }: EditProjectModalProps) {
  const [title, setTitle] = useState(project.title);
  const [description, setDescription] = useState(project.description);
  const [maxDevs, setMaxDevs] = useState(project.maxDevs?.toString() ?? "");
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  if (!isOpen) return null;

  const validate = (): boolean => {
    const next: Record<string, string> = {};
    if (!title.trim()) next.title = "Project title is required.";
    if (!description.trim()) next.description = "Project description is required.";
    if (maxDevs.trim()) {
      const n = Number(maxDevs);
      if (Number.isNaN(n) || n < 1) next.maxDevs = "Must be a positive number.";
    }
    setErrors(next);
    return Object.keys(next).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;

    setIsSaving(true);
    try {
      await onSave({
        id: project.id,
        title: title.trim(),
        description: description.trim(),
        maxDevs: maxDevs.trim() ? Number(maxDevs) : null,
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
            <h2 className="text-xl font-semibold text-text">Edit Project</h2>
            <p className="text-sm text-muted mt-1">Update the project name, description, or team size limit.</p>
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
            <label htmlFor="edit-title" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Project title
            </label>
            <input
              id="edit-title"
              type="text"
              value={title}
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setTitle(e.target.value);
                if (errors.title) setErrors((prev) => ({ ...prev, title: "" }));
              }}
              placeholder="Example: Sprint 24 — Core Features"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.title && <p className="mt-2 text-xs text-danger">{errors.title}</p>}
          </div>

          <div>
            <label htmlFor="edit-description" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Description
            </label>
            <textarea
              id="edit-description"
              value={description}
              onChange={(e: ChangeEvent<HTMLTextAreaElement>) => {
                setDescription(e.target.value);
                if (errors.description) setErrors((prev) => ({ ...prev, description: "" }));
              }}
              placeholder="Brief summary of the project goals and scope"
              rows={4}
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.description && <p className="mt-2 text-xs text-danger">{errors.description}</p>}
          </div>

          <div>
            <label htmlFor="edit-maxDevs" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Max developers
            </label>
            <input
              id="edit-maxDevs"
              type="number"
              min={1}
              value={maxDevs}
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setMaxDevs(e.target.value);
                if (errors.maxDevs) setErrors((prev) => ({ ...prev, maxDevs: "" }));
              }}
              placeholder="Optional"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.maxDevs && <p className="mt-2 text-xs text-danger">{errors.maxDevs}</p>}
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
