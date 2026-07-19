export const TASK_ENDPOINTS = {
  getTasksByBoardColumn: (columnId: number) => `/tasks/boardcolumn/${columnId}`,
  getTaskById: (taskId: number) => `/tasks/${taskId}`,
  createTask: '/tasks/',
  updateTask: '/tasks/',
  deleteTask: (taskId: number) => `/tasks/${taskId}`,
  assignTaskToUser: '/tasks/assign/'
}
