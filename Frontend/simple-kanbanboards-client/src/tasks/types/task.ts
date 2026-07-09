export interface TaskModel {
  id: number;
  idUser: number;
  idBoardColumn: number;
  title: string;
  description: string;
  createdAt: string;
  completedAt: string | null;
  priority: number;
  serviceClass: string | null;
}

export interface CreateTaskModel {
  title: string;
  description: string;
  createdAt: string;
  priority: number;
  serviceClass: string | null;
  idBoardColumn: number;
}

export interface UpdateTaskModel {
  id: number;
  title: string;
  description: string;
  completedAt: string | null;
  priority: number;
  serviceClass: string | null;
  idBoardColumn: number;
}
