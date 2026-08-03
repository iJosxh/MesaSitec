import http from "./http";
import type { SolicitudesQuery } from "../types/SolicitudQuery";

export interface CategoriaSimpleResponse {
    id: string;
    nombre: string;
}

export interface AgenteSimpleResponse {
    id: string;
    nombre: string;
}

export interface SolicitudResponse {
    id: string;
    codigo: string;
    titulo: string;
    estado: string;
    prioridad: string;
    categoria: CategoriaSimpleResponse;
    agente: AgenteSimpleResponse | null;
    fechaCreacion: string;
    fechaLimiteSla: string;
    vencida: boolean;
}

export interface SolicitudesResponse {
    items: SolicitudResponse[];
    page: number;
    pageSize: number;
    total: number;
    totalPaginas: number;
}

export interface UsuarioSimpleResponse {

    id: string;

    nombre: string;

}

export interface CategoriaResponse {

    id: string;

    nombre: string;

    slaHoras: number;

}

export interface SolicitudDetalleResponse {

    id: string;

    codigo: string;

    titulo: string;

    descripcion: string;

    estado: string;

    prioridad: string;

    categoria: CategoriaResponse;

    solicitante: UsuarioSimpleResponse;

    agente: UsuarioSimpleResponse | null;

    fechaCreacion: string;

    fechaLimiteSla: string;

    fechaResolucion: string | null;

    motivoResolucion: string | null;

    motivoCancelacion: string | null;

    vencida: boolean;

}

export interface EjecutarTransicionRequest {

    accion: string;

    agenteId?: string;

    motivoResolucion?: string;

    motivoCancelacion?: string;

}

export async function ejecutarTransicion(
    id: string,
    request: EjecutarTransicionRequest) {

    const response = await http.post(
        `/solicitudes/${id}/transiciones`,
        request);

    return response.data;

}

export async function getSolicitudes(
    query: SolicitudesQuery) {

    const response = await http.get<SolicitudesResponse>(
        "/solicitudes",
        {
            params: query
        });

    return response.data;
}

export async function getSolicitudDetalle(id: string) {

    const response =
        await http.get<SolicitudDetalleResponse>(
            `/solicitudes/${id}`);

    return response.data;

}