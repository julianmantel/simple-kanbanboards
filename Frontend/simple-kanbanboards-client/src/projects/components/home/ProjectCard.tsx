import { useNavigate } from "react-router-dom";
import type { ProjectModel } from "../../types/project";

export default function ProjectCard({ project }: { project: ProjectModel }) {
  const navigate = useNavigate();

  return (
    <div
      onClick={() => navigate(`/projects/${project.id}`)}
      className="group bg-surface border border-border rounded-default p-5 hover:border-teal/30 hover:shadow-[0_0_30px_-10px_rgba(29,158,117,0.15)] transition-all duration-200 cursor-pointer"
    >
      <div className="w-10 h-1 rounded-full bg-teal mb-3" />

      <h3 className="text-sm font-semibold text-text group-hover:text-teal-light transition-colors leading-snug mb-1.5 overflow-hidden text-ellipsis whitespace-nowrap" title={project.title}>
        {project.title}
      </h3>

      <p className="text-xs text-muted leading-relaxed line-clamp-2 mb-4">
        {project.description}
      </p>

      <div className="flex items-center gap-4 text-xs text-muted pt-3 border-t border-border">
        <span className="flex items-center gap-1.5">
          <svg className="size-3.5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5" />
          </svg>
          {new Date(project.startDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
        </span>
        {project.endDate && (
          <span className="flex items-center gap-1.5">
            <svg className="size-3.5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M3 12h12m0 0-3-2.25M15 12l-3 2.25" />
            </svg>
            {new Date(project.endDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
          </span>
        )}
        {project.maxDevs && (
          <span className="flex items-center gap-1.5 ml-auto">
            <svg className="size-3.5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z" />
            </svg>
            {project.maxDevs} devs
          </span>
        )}
      </div>
    </div>
  );
}
