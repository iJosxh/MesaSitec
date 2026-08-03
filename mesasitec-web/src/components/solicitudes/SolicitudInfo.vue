<template>

    <div>

        <h2>{{ solicitud.codigo }}</h2>

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

        <p data-testid="detalle-solicitante">

            <strong>Solicitante:</strong>

            {{ solicitud.solicitante.nombre }}

        </p>

        <p data-testid="detalle-agente">

            <strong>Agente:</strong>

            {{ solicitud.agente?.nombre ?? "Sin asignar" }}

        </p>

        <p data-testid="detalle-fecha-creacion">

            <strong>Creación:</strong>

            {{ formatear(solicitud.fechaCreacion) }}

        </p>

        <p data-testid="detalle-fecha-limite">

            <strong>SLA:</strong>

            {{ formatear(solicitud.fechaLimiteSla) }}

        </p>

        <p
            v-if="solicitud.vencida"
            data-testid="detalle-vencida">

            🔴 Vencida

        </p>

    </div>

</template>

<script setup lang="ts">

import type { SolicitudDetalleResponse } from "../../api/solicitudes";

defineProps<{

    solicitud: SolicitudDetalleResponse;

}>();

function formatear(fecha: string) {

    return new Date(fecha).toLocaleString();

}

</script>