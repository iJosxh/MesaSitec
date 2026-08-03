import http from "./http";

export interface CategoriaResponse {
    id: string;
    nombre: string;
    slaHoras: number;
}

export async function getCategorias() {

    const response = await http.get<CategoriaResponse[]>("/categorias");

    return response.data;
}