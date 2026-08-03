<template>

<AppLayout>

<div class="card">

    <div v-if="cargando">

        Cargando...

    </div>

    <div
        v-else-if="solicitud">

        <h1>

            Detalle de solicitud

        </h1>

        <p data-testid="detalle-codigo">

            <strong>Código:</strong>

            {{ solicitud.codigo }}

        </p>

        <p data-testid="detalle-titulo">

            <strong>Título:</strong>

            {{ solicitud.titulo }}

        </p>

        <p data-testid="detalle-descripcion">

            <strong>Descripción:</strong>

            {{ solicitud.descripcion }}

        </p>

        <p data-testid="detalle-estado">

            <strong>Estado:</strong>

            {{ solicitud.estado }}

        </p>

        <p data-testid="detalle-prioridad">

            <strong>Prioridad:</strong>

            {{ solicitud.prioridad }}

        </p>

        <p data-testid="detalle-categoria">

            <strong>Categoría:</strong>

            {{ solicitud.categoria.nombre }}

        </p>

        <p data-testid="detalle-agente">

            <strong>Agente:</strong>

            {{ solicitud.agente?.nombre ?? "Sin asignar" }}

        </p>

        <p data-testid="detalle-fecha-creacion">

            {{ solicitud.fechaCreacion }}

        </p>

        <p data-testid="detalle-fecha-limite">

            {{ solicitud.fechaLimiteSla }}

        </p>

        <p
            v-if="solicitud.vencida"
            data-testid="detalle-vencida">

            VENCIDA

        </p>

    </div>

</div>

</AppLayout>

</template>

<script setup lang="ts">

import { ref, onMounted } from "vue";

import { useRoute } from "vue-router";

import AppLayout from "../layouts/AppLayout.vue";

import {

    getSolicitudDetalle,

    type SolicitudDetalleResponse

} from "../api/solicitudes";

const route = useRoute();

const cargando = ref(true);

const solicitud = ref<SolicitudDetalleResponse>();

onMounted(async () => {

    solicitud.value =
        await getSolicitudDetalle(
            route.params.id as string);

    cargando.value = false;

});

</script>