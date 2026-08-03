<template>

    <div>

        <header
            class="navbar"
            data-testid="app-nav">

            <div>

                <h2>MesaSitec</h2>

            </div>

            <div class="navbar-user">

                <div>

                    <div data-testid="nav-usuario-nombre">

                        {{ auth.nombre }}

                    </div>

                    <small data-testid="nav-usuario-rol">

                        {{ auth.rol }}

                    </small>

                </div>

                <button
                    data-testid="btn-logout"
                    @click="logout">

                    Cerrar sesión

                </button>

            </div>

        </header>

        <main class="container">

            <slot />

        </main>

    </div>

</template>

<script setup lang="ts">

import { useRouter } from "vue-router";

import { useAuthStore } from "../stores/auth";

const auth = useAuthStore();

const router = useRouter();

function logout() {

    auth.logout();

    router.push("/login");

}

</script>