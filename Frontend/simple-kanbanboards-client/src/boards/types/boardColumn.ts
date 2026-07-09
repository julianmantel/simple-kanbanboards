export interface BoardColumnModel {
  id: number;
  name: string;
  position: number;
  wipLimit: number;
  isEntry: boolean;
  isDone: boolean;
  idBoard: number;
}

export interface CreateBoardColumnModel {
  name: string;
  position: number;
  wipLimit: number;
  isEntry: boolean;
  isDone: boolean;
  idBoard: number;
}

export interface UpdateBoardColumnModel {
  id: number;
  name: string;
  position: number;
  wipLimit: number;
  isEntry: boolean;
  isDone: boolean;
}
