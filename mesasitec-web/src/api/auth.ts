import http from "./http";

export interface LoginRequest {

    email: string;

    password: string;
}

export interface UsuarioResponse {

    id: string;

    nombre: string;

    email: string;

    rol: string;

    tenantId: string;

    tenantNombre: string;
}

export interface LoginResponse {

    accessToken: string;

    expiraEn: number;

    usuario: UsuarioResponse;
}

export async function login(request: LoginRequest) {

    const response = await http.post<LoginResponse>(
        "/auth/login",
        request);

    return response.data;
}