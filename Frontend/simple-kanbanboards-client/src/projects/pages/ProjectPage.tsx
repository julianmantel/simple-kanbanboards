import { useEffect, useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { projectsApi } from "../api/projectsApi";
import EditProjectModal from "../components/project/EditProjectModal";
import ConfirmDeleteModal from "../components/project/ConfirmDeleteModal";
import ProjectToolbar from "../components/project/ProjectToolbar";
import ProjectDetails from "../components/project/ProjectDetails";
import type { ProjectModel, UpdateProjectModel } from "../types/project";
import ProjectBoardsSection from "../components/project/ProjectBoardsSection";

export default function ProjectPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const navigate = useNavigate();
  const [project, setProject] = useState<ProjectModel | undefined>(undefined);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  
  useEffect(() => {
    if (projectId) {
      projectsApi.getProjectById(parseInt(projectId))
        .then((loadedProject) => {
          setProject(loadedProject);
        })
        .catch((error) => {
          console.error("Error fetching project:", error);
        });
    }
  }, [projectId]);

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

  const handleSave = async (payload: UpdateProjectModel) => {
    await projectsApi.updateProject(payload);
    setProject((prev) =>
      prev ? { ...prev, title: payload.title, description: payload.description, maxDevs: payload.maxDevs } : prev,
    );
    setShowEditModal(false);
  };

  const handleDelete = async () => {
    await projectsApi.deleteProject(project.id);
    navigate("/home");
  };

  return (
    <div>
      <ProjectToolbar
        onEdit={() => setShowEditModal(true)}
        onDelete={() => setShowDeleteModal(true)}
      />

      <ProjectDetails project={project} />

      <ProjectBoardsSection
        projectId={project.id}
        onCreateBoard={(board) => {
          console.log("Create board", board);
        }}
      />

      <EditProjectModal
        isOpen={showEditModal}
        project={project}
        onSave={handleSave}
        onClose={() => setShowEditModal(false)}
      />

      <ConfirmDeleteModal
        isOpen={showDeleteModal}
        projectTitle={project.title}
        onConfirm={handleDelete}
        onCancel={() => setShowDeleteModal(false)}
      />
    </div>
  );
}
