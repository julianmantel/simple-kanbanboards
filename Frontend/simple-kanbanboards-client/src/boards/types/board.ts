export interface BoardModel {
  id: number;
  name: string;
  description: string;
  createdAt: string;
  isActive: boolean;
  projectId: number;
}

export interface CreateBoardModel {
  name: string;
  description: string;
  isActive: boolean;
  projectId: number;
}

export interface UpdateBoardModel {
  id: number;
  name: string;
  description: string;
}
