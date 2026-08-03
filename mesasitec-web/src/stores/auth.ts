import { defineStore } from "pinia";
import { login, type UsuarioResponse } from "../api/auth";

export const useAuthStore = defineStore("auth", {

    state: () => ({

        token: localStorage.getItem("accessToken"),

        usuario: JSON.parse(
            localStorage.getItem("usuario") ?? "null"
        ) as UsuarioResponse | null

    }),

    getters: {

        isAuthenticated: (state) => !!state.token,

        rol: (state) => state.usuario?.rol,

        nombre: (state) => state.usuario?.nombre,

        tenantId: (state) => state.usuario?.tenantId,

        isAdmin: (state) => state.usuario?.rol === "Admin",

        isAgente: (state) => state.usuario?.rol === "Agente",

        isSolicitante: (state) => state.usuario?.rol === "Solicitante"

    },

    actions: {

        async login(email: string, password: string) {

            const response = await login({

                email,
                password

            });

            this.token = response.accessToken;

            this.usuario = response.usuario;

            localStorage.setItem(
                "accessToken",
                response.accessToken);

            localStorage.setItem(
                "usuario",
                JSON.stringify(response.usuario));

        },

        logout() {

            this.token = null;

            this.usuario = null;

            localStorage.removeItem("accessToken");
            localStorage.removeItem("usuario");

        }

    }

});