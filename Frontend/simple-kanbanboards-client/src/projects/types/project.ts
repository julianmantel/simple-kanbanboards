export interface ProjectModel {
  id: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string | null;
  maxDevs: number | null;
}

export interface CreateProjectModel {
  title: string;
  description: string;
  maxDevs: number | null;
  authorId: number;
}

export interface UpdateProjectModel {
  id: number;
  title: string;
  maxDevs: number | null;
  description: string;
}

export interface AddDevToProjectModel {
  projectId: number;
  devId: number;
}