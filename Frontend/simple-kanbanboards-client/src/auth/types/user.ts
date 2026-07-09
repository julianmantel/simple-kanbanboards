export type Role = {
  id: number;
  name: string;
};

export type User = {
  id: number;
  userName: string;
  roles: Role[];
};
