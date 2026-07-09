import type { BoardModel } from "../types/board";
import type { BoardColumnModel } from "../types/boardColumn";

export const mockBoards: BoardModel[] = [
  { id: 1, name: "Kanban", description: "Main development board", createdAt: "2026-05-21", isActive: true, projectId: 1 },
  { id: 2, name: "Bug Tracker", description: "Track and resolve bugs", createdAt: "2026-05-22", isActive: true, projectId: 1 },
  { id: 3, name: "Sprint Board", description: "Sprint planning and tracking", createdAt: "2026-06-02", isActive: true, projectId: 2 },
  { id: 4, name: "Feedback", description: "Collect user feedback", createdAt: "2026-06-05", isActive: false, projectId: 2 },
  { id: 5, name: "Integration Status", description: "Track API integrations", createdAt: "2026-05-16", isActive: true, projectId: 3 },
  { id: 6, name: "Sprint Board", description: "Mobile app sprint tracking", createdAt: "2026-06-11", isActive: true, projectId: 4 },
  { id: 7, name: "Analytics", description: "Performance metrics board", createdAt: "2026-01-11", isActive: true, projectId: 6 },
  { id: 8, name: "Components", description: "Design system components", createdAt: "2026-02-21", isActive: true, projectId: 7 },
];

export const mockBoardColumns: BoardColumnModel[] = [
  { id: 1, name: "Backlog", position: 1, wipLimit: 0, isEntry: true, isDone: false, idBoard: 1 },
  { id: 2, name: "To Do", position: 2, wipLimit: 5, isEntry: false, isDone: false, idBoard: 1 },
  { id: 3, name: "In Progress", position: 3, wipLimit: 3, isEntry: false, isDone: false, idBoard: 1 },
  { id: 4, name: "Review", position: 4, wipLimit: 3, isEntry: false, isDone: false, idBoard: 1 },
  { id: 5, name: "Done", position: 5, wipLimit: 0, isEntry: false, isDone: true, idBoard: 1 },
  { id: 6, name: "Reported", position: 1, wipLimit: 0, isEntry: true, isDone: false, idBoard: 2 },
  { id: 7, name: "Confirmed", position: 2, wipLimit: 5, isEntry: false, isDone: false, idBoard: 2 },
  { id: 8, name: "Fixed", position: 3, wipLimit: 0, isEntry: false, isDone: true, idBoard: 2 },
  { id: 9, name: "Backlog", position: 1, wipLimit: 0, isEntry: true, isDone: false, idBoard: 3 },
  { id: 10, name: "In Progress", position: 2, wipLimit: 3, isEntry: false, isDone: false, idBoard: 3 },
  { id: 11, name: "Done", position: 3, wipLimit: 0, isEntry: false, isDone: true, idBoard: 3 },
  { id: 12, name: "Backlog", position: 1, wipLimit: 0, isEntry: true, isDone: false, idBoard: 5 },
  { id: 13, name: "In Progress", position: 2, wipLimit: 4, isEntry: false, isDone: false, idBoard: 5 },
  { id: 14, name: "Completed", position: 3, wipLimit: 0, isEntry: false, isDone: true, idBoard: 5 },
  { id: 15, name: "To Do", position: 1, wipLimit: 6, isEntry: true, isDone: false, idBoard: 6 },
  { id: 16, name: "Doing", position: 2, wipLimit: 3, isEntry: false, isDone: false, idBoard: 6 },
  { id: 17, name: "Done", position: 3, wipLimit: 0, isEntry: false, isDone: true, idBoard: 6 },
  { id: 18, name: "Metrics", position: 1, wipLimit: 0, isEntry: true, isDone: false, idBoard: 7 },
  { id: 19, name: "Audited", position: 2, wipLimit: 0, isEntry: false, isDone: true, idBoard: 7 },
  { id: 20, name: "To Design", position: 1, wipLimit: 3, isEntry: true, isDone: false, idBoard: 8 },
  { id: 21, name: "In Review", position: 2, wipLimit: 2, isEntry: false, isDone: false, idBoard: 8 },
  { id: 22, name: "Approved", position: 3, wipLimit: 0, isEntry: false, isDone: true, idBoard: 8 },
];
