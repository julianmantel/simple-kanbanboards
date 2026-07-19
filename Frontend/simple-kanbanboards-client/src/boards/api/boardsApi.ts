import { httpClient } from "../../shared/config/httpClient";
import type { ApiResult } from "../../shared/types/apiResult.type";
import type { BoardModel, CreateBoardModel, UpdateBoardModel } from "../types/board";
import { boardsEndpoints } from "./boardsEndpoints";

export const boardsApi = {
    getBoardsByProject: async (projectId: number): Promise<BoardModel[]> => {
        const { data } = await httpClient.get<ApiResult<BoardModel[]>>(boardsEndpoints.getBoardsByProject(projectId));
        return data.result;
    },
    getBoardById: async (boardId: number): Promise<BoardModel> => {
        const { data } = await httpClient.get<ApiResult<BoardModel>>(boardsEndpoints.getBoardById(boardId));
        return data.result;
    },
    createBoard: async (payload: CreateBoardModel): Promise<void> => {
        await httpClient.post<ApiResult<BoardModel>>(boardsEndpoints.createBoard, payload);
    },
    updateBoard: async (payload: UpdateBoardModel): Promise<void> => {
        await httpClient.put<ApiResult<BoardModel>>(boardsEndpoints.updateBoard(payload.id), payload);
    },
    toggleBoard: async (boardId: number): Promise<void> => {
        await httpClient.delete<ApiResult<BoardModel>>(boardsEndpoints.toggleBoard(boardId));
    }
};