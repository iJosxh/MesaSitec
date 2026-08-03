<template>

    <AppLayout>

        <div class="card">

            <div class="page-title">

                <h1>Solicitudes</h1>

                <button
                    data-testid="btn-nueva-solicitud"
                    @click="router.push('/solicitudes/nueva')">

                    Nueva solicitud

                </button>

            </div>

            <SolicitudesFilters
                @buscar="buscar" />

            <p
                v-if="cargando"
                data-testid="listado-cargando">

                Cargando...

            </p>

            <p
                v-else-if="solicitudes.length === 0"
                data-testid="listado-vacio">

                No existen solicitudes.

            </p>

            <SolicitudesTable
                v-else
                :solicitudes="solicitudes"
                @detalle="verDetalle" />

            <div class="pagination">

                <button
                    data-testid="paginacion-anterior"
                    @click="paginaAnterior"
                    :disabled="pagina <= 1">

                    Anterior

                </button>

                <span data-testid="paginacion-info">

                    Página {{ pagina }} de {{ totalPaginas }} - {{ totalResultados }} resultados

                </span>

                <button
                    data-testid="paginacion-siguiente"
                    @click="paginaSiguiente"
                    :disabled="pagina >= totalPaginas">

                    Siguiente

                </button>

            </div>

        </div>

    </AppLayout>

</template>

<script setup lang="ts">

import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

import {
    getSolicitudes,
    type SolicitudResponse
} from "../api/solicitudes";

import SolicitudesFilters from "../components/solicitudes/SolicitudesFilters.vue";
import SolicitudesTable from "../components/solicitudes/SolicitudesTable.vue";
import AppLayout from "../layouts/AppLayout.vue";

import type { SolicitudesQuery } from "../types/SolicitudQuery";

const router = useRouter();

const solicitudes = ref<SolicitudResponse[]>([]);

const pagina = ref(1);

const totalPaginas = ref(1);

const totalResultados = ref(0);

const cargando = ref(true);

const filtros = ref<SolicitudesQuery>({
    page: 1,
    pageSize: 20
});

async function cargarSolicitudes(query: SolicitudesQuery = {}) {

    cargando.value = true;

    filtros.value = {

        ...filtros.value,

        ...query

    };

    try {

        const response = await getSolicitudes(filtros.value);

        solicitudes.value = response.items;

        pagina.value = response.page;

        totalPaginas.value = response.totalPaginas;

        totalResultados.value = response.total;

    }

    finally {

        cargando.value = false;

    }

}

async function paginaAnterior() {

    if (pagina.value <= 1)
        return;

    await cargarSolicitudes({

        page: pagina.value - 1

    });

}

async function paginaSiguiente() {

    if (pagina.value >= totalPaginas.value)
        return;

    await cargarSolicitudes({

        page: pagina.value + 1

    });

}

function buscar(query: SolicitudesQuery) {

    cargarSolicitudes({

        ...query,

        page: 1

    });

}

function verDetalle(id: string) {

    router.push(`/solicitudes/${id}`);

}

onMounted(() => {

    cargarSolicitudes(filtros.value);

});

</script>