import { httpClient } from "../../shared/config/httpClient";
import type { ApiResult } from "../../shared/types/apiResult.type";
import type { CreateTaskModel, TaskModel, UpdateTaskModel, UserTaskModel } from "../types/task";
import { TASK_ENDPOINTS } from "./tasksEndpoints";

export const tasksApi = {
  getTasksByBoardColumn: async (columnId: number): Promise<TaskModel[]> => {
    const { data } = await httpClient.get<ApiResult<TaskModel[]>>(TASK_ENDPOINTS.getTasksByBoardColumn(columnId));
    return data.result;
  },
  getTaskById: async (taskId: number): Promise<TaskModel> => {
    const { data } = await httpClient.get<ApiResult<TaskModel>>(TASK_ENDPOINTS.getTaskById(taskId));
    return data.result;
  },
  createTask: async (payload: CreateTaskModel): Promise<void> => {
    await httpClient.post<ApiResult<TaskModel>>(TASK_ENDPOINTS.createTask, payload);
  },
  updateTask: async (payload: UpdateTaskModel): Promise<void> => {
    await httpClient.put<ApiResult<TaskModel>>(TASK_ENDPOINTS.updateTask, payload);
  },
  deleteTask: async (taskId: number): Promise<void> => {
    await httpClient.delete<ApiResult<void>>(TASK_ENDPOINTS.deleteTask(taskId));
  },
  assignTaskToUser: async (payload: UserTaskModel): Promise<void> => {
    await httpClient.post<ApiResult<void>>(TASK_ENDPOINTS.assignTaskToUser, payload);
  }
}
