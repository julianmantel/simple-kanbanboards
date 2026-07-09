import { useEffect, useRef, useState } from "react";
import HomeHeader from "../components/HomeHeader";
import ProjectGrid from "../components/ProjectGrid";
import NewProjectModal from "../components/NewProjectModal";
import { projectsApi } from "../api/projectsApi";
import type { CreateProjectModel, ProjectModel } from "../types/project";

export default function Home() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [projects, setProjects] = useState<ProjectModel[]>([]);
  const nextProjectId = useRef(1);

  useEffect(() => {
    projectsApi.getProjects()
      .then((loadedProjects) => {
        setProjects(loadedProjects);
        const maxId = loadedProjects.length ? Math.max(...loadedProjects.map((project) => project.id)) : 0;
        nextProjectId.current = maxId + 1;
      })
      .catch((error) => {
        console.error("Error fetching projects:", error);
      });
  }, []);

  const handleCreateProject = (newProject: CreateProjectModel) => {
    projectsApi.createProject(newProject)
    .then(() => {
        const createdProject: ProjectModel = {
          id: nextProjectId.current,
          title: newProject.title,
          description: newProject.description,
          startDate: new Date().toISOString(),
          endDate: null,
          maxDevs: newProject.maxDevs,
        };
        setProjects((prevProjects) => [...prevProjects, createdProject]);
        nextProjectId.current += 1;
      })
      .catch((error) => {
        console.error("Error creating project:", error);
      });
  }

  return (
    <>
      <HomeHeader onNewProject={() => setIsModalOpen(true)} />
      <ProjectGrid projects={projects} />
      <NewProjectModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCreate={handleCreateProject}
      />
    </>
  );
}
