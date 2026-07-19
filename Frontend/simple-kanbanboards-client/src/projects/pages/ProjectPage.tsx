import { useEffect, useRef, useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { projectsApi } from "../api/projectsApi";
import EditProjectModal from "../components/project/EditProjectModal";
import ConfirmDeleteModal from "../components/project/ConfirmDeleteModal";
import ProjectToolbar from "../components/project/ProjectToolbar";
import ProjectDetails from "../components/project/ProjectDetails";
import type { ProjectModel, UpdateProjectModel } from "../types/project";
import ProjectBoardsSection from "../components/project/ProjectBoardsSection";
import type { BoardModel, CreateBoardModel } from "../../boards/types/board";
import { boardsApi } from "../../boards/api/boardsApi";

export default function ProjectPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const navigate = useNavigate();
  const [project, setProject] = useState<ProjectModel | undefined>(undefined);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [boards, setBoards] = useState<BoardModel[]>([]);
  const nextBoardId = useRef(1);
  
  useEffect(() => {
    if (projectId) {
      projectsApi.getProjectById(parseInt(projectId))
        .then((loadedProject) => {
          setProject(loadedProject);
        })
        .catch((error) => {
          console.error("Error fetching project:", error);
        });

      boardsApi.getBoardsByProject(parseInt(projectId))
        .then((loadedBoards) => {
          setBoards(loadedBoards);
          const maxId = loadedBoards.length ? Math.max(...loadedBoards.map((board) => board.id)) : 0;
          nextBoardId.current = maxId + 1;
        })
        .catch((error) => {
          console.error("Error fetching boards:", error);
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

  const handleCreateBoard = (board: CreateBoardModel) => {
    boardsApi.createBoard(board)
      .then(() => {
        const createdBoard: BoardModel = {
          id: nextBoardId.current,
          name: board.name,
          description: board.description,
          is_Active: board.is_Active,
          projectId: board.projectId,
          created_At: new Date().toISOString(),
        };

        setBoards((prev) => ([...prev, createdBoard]));
        nextBoardId.current++;
      })
      .catch((error) => {
        console.error("Error creating board:", error);
      });
  };

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
        onCreateBoard={handleCreateBoard}
        boards={boards}
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
