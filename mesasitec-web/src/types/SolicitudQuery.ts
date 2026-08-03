export interface SolicitudesQuery {

    estado?: string;

    prioridad?: string;

    categoriaId?: string;

    agenteId?: string;

    q?: string;

    vencidas?: boolean;

    page?: number;

    pageSize?: number;

    sort?: string;
}