interface ConfirmDeleteModalProps {
  isOpen: boolean;
  projectTitle: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export default function ConfirmDeleteModal({ isOpen, projectTitle, onConfirm, onCancel }: ConfirmDeleteModalProps) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center px-4 py-6">
      <div className="fixed inset-0 bg-black/40 backdrop-blur-sm" onClick={onCancel} />
      <div
        className="relative w-full max-w-md rounded-default border border-border bg-surface p-6 shadow-[0_0_45px_-14px_rgba(0,0,0,0.45)]"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-start gap-4">
          <div className="flex size-10 shrink-0 items-center justify-center rounded-full bg-danger/10">
            <svg className="size-5 text-danger" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z" />
            </svg>
          </div>
          <div className="flex-1 min-w-0">
            <h2 className="text-lg font-semibold text-text">Delete project</h2>
            <p className="text-sm text-muted mt-1 leading-relaxed">
              Are you sure you want to delete <span className="text-text font-medium">{projectTitle}</span>? This action cannot be undone.
            </p>
          </div>
          <button
            type="button"
            onClick={onCancel}
            className="text-muted hover:text-text shrink-0"
            aria-label="Close modal"
          >
            <svg className="size-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M6 18 18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <div className="flex items-center justify-end gap-3 mt-6 pt-4 border-t border-border">
          <button
            onClick={onCancel}
            className="rounded-lg border border-border bg-surface2 px-4 py-2 text-sm font-medium text-text transition hover:bg-surface3"
          >
            Cancel
          </button>
          <button
            onClick={onConfirm}
            className="rounded-lg bg-danger px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}
