import type { TaskModel } from "../types/task";

export const mockTasks: TaskModel[] = [
  { id: 1, idUser: 1, idBoardColumn: 2, title: "Set up authentication flow", description: "Implement JWT login and registration", createdAt: "2026-05-21T10:00:00Z", completedAt: null, priority: 1, serviceClass: "standard" },
  { id: 2, idUser: 1, idBoardColumn: 3, title: "Design project schema", description: "Create database schema for projects", createdAt: "2026-05-21T11:00:00Z", completedAt: null, priority: 1, serviceClass: "expedite" },
  { id: 3, idUser: 2, idBoardColumn: 4, title: "Review API contracts", description: "Review and approve API contracts", createdAt: "2026-05-22T09:00:00Z", completedAt: null, priority: 2, serviceClass: "standard" },
  { id: 4, idUser: 1, idBoardColumn: 5, title: "Create project repository", description: "Initialize git repo and CI/CD", createdAt: "2026-05-20T08:00:00Z", completedAt: "2026-05-20T16:00:00Z", priority: 1, serviceClass: "standard" },
];
