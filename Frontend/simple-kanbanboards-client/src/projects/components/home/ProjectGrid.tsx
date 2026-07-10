import type { ProjectModel } from "../../types/project";
import ProjectCard from "./ProjectCard";
import { useState, useRef } from "react";

interface ProjectGridProps {
  projects: ProjectModel[];
}

export default function ProjectGrid({ projects }: ProjectGridProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);

  const searched = projects.filter((p) => {
    if (!searchTerm) return true;
    return p.title.toLowerCase().includes(searchTerm.toLowerCase());
  });

  return (
    <section>
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6">
        <div className="relative w-full sm:w-64">
          <svg
            className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted pointer-events-none"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth={1.5}
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z"
            />
          </svg>
          <input
            ref={inputRef}
            type="text"
            placeholder="Search projects..."
            className="w-full pl-9 pr-3 py-2 bg-surface2 border border-border rounded-xl text-sm text-text placeholder:text-muted/50 focus:outline-none focus:ring-2 focus:ring-teal/40 focus:border-teal/40 transition-all"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 animate-fadeIn">
        {searched.map((project) => (
          <ProjectCard key={project.id} project={project} />
        ))}
      </div>
    </section>
  );
}
