export interface PagedResultDto<T> {
    items?: T[] | null;
    totalCount: number;
}