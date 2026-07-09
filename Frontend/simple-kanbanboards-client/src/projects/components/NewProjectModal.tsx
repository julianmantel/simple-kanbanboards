import { useState, type ChangeEvent, type SubmitEvent } from "react";
import { useAuth } from "../../auth/context/AuthContext";
import type { CreateProjectModel } from "../types/project";

interface NewProjectModalProps {
  isOpen: boolean;
  onClose: () => void;
  onCreate: (project: CreateProjectModel) => void;
}

export default function NewProjectModal({ isOpen, onClose, onCreate }: NewProjectModalProps) {
  const { user } = useAuth();
  const [formData, setFormData] = useState({ title: "", description: "", maxDevs: "" });
  const [errors, setErrors] = useState<Record<string, string>>({});

  if (!isOpen) return null;

  const resetForm = () => {
    setFormData({ title: "", description: "", maxDevs: "" });
    setErrors({});
  };

  const handleChange = (field: keyof typeof formData) => (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData((prev) => ({ ...prev, [field]: event.target.value }));
    if (errors[field]) {
      setErrors((prev) => {
        const next = { ...prev };
        delete next[field];
        return next;
      });
    }
  };

  const handleSubmit = (event: SubmitEvent) => {
    event.preventDefault();

    if (!user) {
      setErrors({ form: "Please sign in before creating a project." });
      return;
    }

    const title = formData.title.trim();
    const description = formData.description.trim();
    const maxDevs = formData.maxDevs.trim() ? Number(formData.maxDevs) : null;

    const nextErrors: Record<string, string> = {};
    if (!title) nextErrors.title = "Project title is required.";
    if (!description) nextErrors.description = "Project description is required.";
    if (formData.maxDevs.trim()) {
      if (maxDevs === null || Number.isNaN(maxDevs) || maxDevs < 1) {
        nextErrors.maxDevs = "Max developers must be a positive number.";
      }
    }

    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors);
      return;
    }

    const newProject: CreateProjectModel = {
      title,
      description,
      maxDevs,
      authorId: user.id,
    };

    onCreate(newProject);
    resetForm();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center px-4 py-6">
      <div className="fixed inset-0 bg-black/40 backdrop-blur-sm" onClick={onClose} />
      <div
        className="relative w-full max-w-xl rounded-default border border-border bg-surface p-6 shadow-[0_0_45px_-14px_rgba(0,0,0,0.45)]"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="flex items-start justify-between gap-4">
          <div>
            <h2 className="text-xl font-semibold text-text">Create New Project</h2>
            <p className="text-sm text-muted mt-1">Add a project name, description, and team size limit.</p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-muted hover:text-text"
            aria-label="Close modal"
          >
            ✕
          </button>
        </div>

        <form className="mt-6 space-y-5" onSubmit={handleSubmit}>
          <div>
            <label htmlFor="project-title" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Project title
            </label>
            <input
              id="project-title"
              type="text"
              value={formData.title}
              onChange={handleChange("title")}
              placeholder="Example: Sprint 24 — Core Features"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.title && <p className="mt-2 text-xs text-danger">{errors.title}</p>}
          </div>

          <div>
            <label htmlFor="project-description" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Description
            </label>
            <textarea
              id="project-description"
              value={formData.description}
              onChange={handleChange("description")}
              placeholder="Brief summary of the project goals and scope"
              rows={4}
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.description && <p className="mt-2 text-xs text-danger">{errors.description}</p>}
          </div>

          <div>
            <label htmlFor="project-maxDevs" className="block text-xs text-muted mb-2 uppercase tracking-wide">
              Max developers
            </label>
            <input
              id="project-maxDevs"
              type="number"
              min={1}
              value={formData.maxDevs}
              onChange={handleChange("maxDevs")}
              placeholder="Optional"
              className="w-full rounded-lg border border-border bg-surface2 px-4 py-3 text-sm text-text outline-none transition focus:border-teal"
            />
            {errors.maxDevs && <p className="mt-2 text-xs text-danger">{errors.maxDevs}</p>}
          </div>

          {errors.form && <p className="text-sm text-danger">{errors.form}</p>}

          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-end gap-3 pt-2 border-t border-border">
            <button
              type="button"
              onClick={() => {
                resetForm();
                onClose();
              }}
              className="w-full sm:w-auto rounded-lg border border-border bg-surface2 px-5 py-3 text-sm font-medium text-text transition hover:bg-surface3"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="w-full sm:w-auto rounded-lg bg-teal px-5 py-3 text-sm font-medium text-white transition hover:bg-teal-dim"
            >
              Create project
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
