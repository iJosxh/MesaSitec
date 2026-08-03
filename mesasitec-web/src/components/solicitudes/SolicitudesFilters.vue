<template>

    <div>

        <input
            v-model="localQuery.q"
            placeholder="Buscar..."
            data-testid="filtro-busqueda">

        <select
            v-model="localQuery.estado"
            data-testid="filtro-estado">

            <option value="">Todos los estados</option>
            <option>Nueva</option>
            <option>Asignada</option>
            <option>EnProceso</option>
            <option>Resuelta</option>
            <option>Cerrada</option>
            <option>Cancelada</option>

        </select>

        <select
            v-model="localQuery.prioridad"
            data-testid="filtro-prioridad">

            <option value="">Todas las prioridades</option>
            <option>Baja</option>
            <option>Media</option>
            <option>Alta</option>
            <option>Critica</option>

        </select>

        <select
            v-model="localQuery.categoriaId"
            data-testid="filtro-categoria">

            <option value="">
                Todas las categorías
            </option>

            <option
                v-for="categoria in categorias"
                :key="categoria.id"
                :value="categoria.id">

                {{ categoria.nombre }}

            </option>

        </select>

        <label>

            <input
                type="checkbox"
                v-model="localQuery.vencidas"
                data-testid="filtro-vencidas">

            Solo vencidas

        </label>

        <button
            @click="emitirBusqueda"
            type="button">

            Buscar

        </button>

        <button
            @click="limpiar"
            data-testid="btn-limpiar-filtros"
            type="button">

            Limpiar

        </button>

    </div>

</template>

<script setup lang="ts">

import { reactive } from "vue";
import { onMounted, ref } from "vue";
import {
    getCategorias,
    type CategoriaResponse
} from "../../api/categorias";
import type { SolicitudesQuery } from "../../types/SolicitudQuery";

const emit = defineEmits<{
    (e: "buscar", query: SolicitudesQuery): void;
}>();

const localQuery = reactive<SolicitudesQuery>({
    page: 1,
    pageSize: 20
});

const categorias = ref<CategoriaResponse[]>([]);

function emitirBusqueda() {

    emit("buscar", { ...localQuery });

}

function limpiar() {

    localQuery.estado = "";
    localQuery.prioridad = "";
    localQuery.categoriaId = "";
    localQuery.q = "";
    localQuery.vencidas = false;
    localQuery.page = 1;
    localQuery.pageSize = 20;

    emitirBusqueda();

}

onMounted(async () => {

    categorias.value = await getCategorias();

});

</script>