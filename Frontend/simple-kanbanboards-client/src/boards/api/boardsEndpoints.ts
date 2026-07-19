

export const boardsEndpoints = {
  getBoardsByProject: (projectId: number) => `/boards/project/${projectId}`,
  getBoardById: (boardId: number) => `/boards/${boardId}`,
  createBoard: '/boards/',
  updateBoard: (boardId: number) => `/boards/${boardId}`,
  toggleBoard: (boardId: number) => `/boards/${boardId}`
}