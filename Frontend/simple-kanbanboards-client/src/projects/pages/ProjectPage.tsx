import { useParams, Link } from "react-router-dom";
import { mockProjects } from "../data/mockProjects";
import BoardList from "../../boards/components/BoardList";

export default function ProjectPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const project = mockProjects.find((p) => p.id === Number(projectId));

  if (!project) {
    return (
      <div className="text-center py-16">
        <p className="text-muted text-sm">Project not found.</p>
        <Link to="/home" className="text-teal-light hover:underline text-sm mt-2 inline-block">
          Back to Home
        </Link>
      </div>
    );
  }

  return (
    <div>
      <Link
        to="/home"
        className="inline-flex items-center gap-1.5 text-sm text-muted hover:text-text transition-colors mb-6"
      >
        <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
          <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5 8.25 12l7.5-7.5" />
        </svg>
        Back to Projects
      </Link>

      {/* Project header */}
      <div className="mb-8">
        <h1 className="text-2xl font-heading font-semibold text-text tracking-tight mb-2">
          {project.title}
        </h1>
        <p className="text-sm text-muted max-w-2xl">{project.description}</p>

        <div className="flex items-center gap-6 mt-4 text-xs text-muted">
          <span className="flex items-center gap-1.5">
            <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5" />
            </svg>
            Start: {new Date(project.startDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
          </span>
          {project.endDate && (
            <span className="flex items-center gap-1.5">
              <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M3 12h12m0 0-3-2.25M15 12l-3 2.25" />
              </svg>
              End: {new Date(project.endDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
            </span>
          )}
          {project.maxDevs && (
            <span className="flex items-center gap-1.5">
              <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z" />
              </svg>
              {project.maxDevs} devs max
            </span>
          )}
        </div>
      </div>

      {/* Section header */}
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-lg font-heading font-semibold text-text">Boards</h2>
        <button className="flex items-center gap-1.5 px-3 py-1.5 bg-teal hover:bg-teal-dim transition-colors text-white text-sm font-medium rounded-lg">
          <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
          </svg>
          New Board
        </button>
      </div>

      <BoardList projectId={project.id} />
    </div>
  );
}
