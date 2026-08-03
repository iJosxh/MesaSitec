<template>

    <table data-testid="tabla-solicitudes">

        <thead>

            <tr>

                <th>Código</th>
                <th>Estado</th>
                <th>Prioridad</th>
                <th>SLA</th>

            </tr>

        </thead>

        <tbody>

            <tr
                v-for="solicitud in solicitudes"
                :key="solicitud.id"
                :data-codigo="solicitud.codigo"
                data-testid="fila-solicitud"
                @click="$emit('detalle', solicitud.id)"
                style="cursor:pointer">

                <td data-testid="celda-codigo">

                    {{ solicitud.codigo }}

                </td>

                <td data-testid="celda-estado">

                    {{ solicitud.estado }}

                </td>

                <td data-testid="celda-prioridad">

                    {{ solicitud.prioridad }}

                </td>

                <td data-testid="celda-sla">

                    {{ formatearFecha(solicitud.fechaLimiteSla) }}

                    <span
                        v-if="solicitud.vencida"
                        class="badge-danger"
                        data-testid="badge-vencida">

                        Vencida

                    </span>

                </td>

            </tr>

        </tbody>

    </table>

</template>

<script setup lang="ts">

import type { SolicitudResponse } from "../../api/solicitudes";

defineProps<{

    solicitudes: SolicitudResponse[];

}>();

defineEmits<{

    (e: "detalle", id: string): void;

}>();

function formatearFecha(fecha: string) {

    return new Date(fecha).toLocaleString();

}

</script>