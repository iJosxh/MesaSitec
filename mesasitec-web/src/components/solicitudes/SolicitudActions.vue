<template>

    <div>

        <button
            v-if="auth.isAdmin && solicitud.estado === 'Nueva'"
            @click="asignar">

            Asignar

        </button>

        <button
            v-if="auth.isAgente && solicitud.estado === 'Asignada'"
            @click="iniciar">

            Iniciar

        </button>

        <button
            v-if="auth.isAgente && solicitud.estado === 'EnProceso'"
            @click="resolver">

            Resolver

        </button>

    </div>

</template>

<script setup lang="ts">

import { useAuthStore } from "../../stores/auth";
import type { SolicitudDetalleResponse } from "../../api/solicitudes";

const auth = useAuthStore();

const props = defineProps<{

    solicitud: SolicitudDetalleResponse;

}>();

function asignar() {

    console.log("Asignar", props.solicitud.id);

}

function iniciar() {

    console.log("Iniciar", props.solicitud.id);

}

function resolver() {

    console.log("Resolver", props.solicitud.id);

}

</script>