import { httpClient } from "../../shared/config/httpClient";
import type { ApiResult } from "../../shared/types/apiResult.type";
import type { AddDevToProjectModel, CreateProjectModel, ProjectModel, UpdateProjectModel } from "../types/project";
import { PROJECT_ENDPOINTS } from "./projectsEndpoints";

export const projectsApi = {
    getProjects: async (): Promise<ProjectModel[]> => {
        const { data } = await httpClient.get<ApiResult<ProjectModel[]>>(PROJECT_ENDPOINTS.get_projects);
        return data.result;
    },

    getProjectById: async (projectId: number): Promise<ProjectModel> => {
        const { data } = await httpClient.get<ApiResult<ProjectModel>>(PROJECT_ENDPOINTS.get_project_by_id(projectId));
        return data.result;
    },

    createProject: async (payload: CreateProjectModel): Promise<void> => {
        await httpClient.post<ApiResult<ProjectModel>>(PROJECT_ENDPOINTS.create_project, payload);
    },

    addDevToProject: async (payload: AddDevToProjectModel): Promise<void> => {
        await httpClient.post<ApiResult<void>>(PROJECT_ENDPOINTS.add_dev_to_project, payload);
    },

    deleteProject: async (projectId: number): Promise<void> => {
        await httpClient.delete<ApiResult<void>>(PROJECT_ENDPOINTS.delete_project(projectId));
    },

    updateProject: async (payload: UpdateProjectModel): Promise<void> => {
        await httpClient.put<ApiResult<void>>(PROJECT_ENDPOINTS.update_project, payload);
    }
}