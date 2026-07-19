export const BOARD_ENDPOINTS = {
  getBoardsByProject: (projectId: number) => `/boards/project/${projectId}`,
  getBoardById: (boardId: number) => `/boards/${boardId}`,
  createBoard: '/boards/',
  updateBoard: '/boards/',
  toggleBoard: (boardId: number) => `/boards/${boardId}`,
  getBoardColumns: (boardId: number) => `/boardcolumns/board/${boardId}`,
}

export const BOARD_COLUMN_ENDPOINTS = {
  getBoardColumnsByBoard: (boardId: number) => `/boardcolumns/board/${boardId}`,
  getBoardColumnById: (columnId: number) => `/boardcolumns/${columnId}`,
  createBoardColumn: '/boardcolumns/',
  updateBoardColumn: '/boardcolumns/',
  deleteBoardColumn: (columnId: number) => `/boardcolumns/${columnId}`,
}