import { createRouter, createWebHistory } from "vue-router";

import LoginView from "../views/LoginView.vue";
import SolicitudesView from "../views/SolicitudesView.vue";
import SolicitudDetalleView from "../views/SolicitudDetalleView.vue";
import SolicitudFormView from "../views/SolicitudFormView.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: "/",
            redirect: "/login"
        },
        {
            path: "/login",
            component: LoginView
        },
        {
            path: "/solicitudes",
            component: SolicitudesView
        },
        {
            path: "/solicitudes/nueva",
            component: SolicitudFormView
        },
        {
            path: "/solicitudes/:id",
            component: SolicitudDetalleView
        },
        {
            path: "/solicitudes/:id/editar",
            component: SolicitudFormView
        }
    ]
});

export default router;