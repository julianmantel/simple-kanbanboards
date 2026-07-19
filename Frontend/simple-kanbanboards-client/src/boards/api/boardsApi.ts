import { httpClient } from "../../shared/config/httpClient";
import type { ApiResult } from "../../shared/types/apiResult.type";
import type { BoardModel, CreateBoardModel, UpdateBoardModel } from "../types/board";
import type { BoardColumnModel, CreateBoardColumnModel, UpdateBoardColumnModel } from "../types/boardColumn";
import { BOARD_COLUMN_ENDPOINTS, BOARD_ENDPOINTS } from "./boardsEndpoints";

export const boardsApi = {
    getBoardsByProject: async (projectId: number): Promise<BoardModel[]> => {
        const { data } = await httpClient.get<ApiResult<BoardModel[]>>(BOARD_ENDPOINTS.getBoardsByProject(projectId));
        return data.result;
    },
    getBoardById: async (boardId: number): Promise<BoardModel> => {
        const { data } = await httpClient.get<ApiResult<BoardModel>>(BOARD_ENDPOINTS.getBoardById(boardId));
        return data.result;
    },
    createBoard: async (payload: CreateBoardModel): Promise<void> => {
        await httpClient.post<ApiResult<BoardModel>>(BOARD_ENDPOINTS.createBoard, payload);
    },
    updateBoard: async (payload: UpdateBoardModel): Promise<void> => {
        await httpClient.put<ApiResult<BoardModel>>(BOARD_ENDPOINTS.updateBoard, payload);
    },
    toggleBoard: async (boardId: number): Promise<void> => {
        await httpClient.delete<ApiResult<BoardModel>>(BOARD_ENDPOINTS.toggleBoard(boardId));
    },
    getBoardColumns: async (boardId: number): Promise<BoardColumnModel[]> => {
        const { data } = await httpClient.get<ApiResult<BoardColumnModel[]>>(BOARD_ENDPOINTS.getBoardColumns(boardId));
        return data.result;
    },
};

export const boardColumnsApi = {
    getBoardColumnsByBoard: async (boardId: number): Promise<BoardColumnModel[]> => {
        const { data } = await httpClient.get<ApiResult<BoardColumnModel[]>>(BOARD_COLUMN_ENDPOINTS.getBoardColumnsByBoard(boardId));
        return data.result;
    },
    getBoardColumnById: async (columnId: number): Promise<BoardColumnModel> => {
        const { data } = await httpClient.get<ApiResult<BoardColumnModel>>(BOARD_COLUMN_ENDPOINTS.getBoardColumnById(columnId));
        return data.result;
    },
    createBoardColumn: async (payload: CreateBoardColumnModel): Promise<void> => {
        await httpClient.post<ApiResult<BoardColumnModel>>(BOARD_COLUMN_ENDPOINTS.createBoardColumn, payload);
    },
    updateBoardColumn: async (payload: UpdateBoardColumnModel): Promise<void> => {
        await httpClient.put<ApiResult<BoardColumnModel>>(BOARD_COLUMN_ENDPOINTS.updateBoardColumn, payload);
    },
    deleteBoardColumn: async (columnId: number): Promise<void> => {
        await httpClient.delete<ApiResult<BoardColumnModel>>(BOARD_COLUMN_ENDPOINTS.deleteBoardColumn(columnId));
    }
}