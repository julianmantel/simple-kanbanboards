export interface BoardModel {
  id: number;
  name: string;
  description: string;
  created_At: string;
  is_Active: boolean;
  projectId: number;
}

export interface CreateBoardModel {
  name: string;
  description: string;
  is_Active: boolean;
  projectId: number;
}

export interface UpdateBoardModel {
  id: number;
  name: string;
  description: string;
}
