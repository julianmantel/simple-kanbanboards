export type ApiResult<T> = {
  succeeded: boolean;
  result: T;
  errors: string[];
};
