<template>

<div class="login-page">

    <div class="login-card">

        <h1>MesaSitec</h1>

        <form
            class="login-form"
            @submit.prevent="iniciarSesion">

            <label>

                Email

                <input
                    data-testid="login-email"
                    v-model="email"
                    type="email">

            </label>

            <label>

                Contraseña

                <input
                    data-testid="login-password"
                    v-model="password"
                    type="password">

            </label>

            <button
                data-testid="login-submit">

                Ingresar

            </button>

            <p
                v-if="error"
                class="error"
                data-testid="login-error">

                {{ error }}

            </p>

        </form>

    </div>

</div>

</template>

<script setup lang="ts">

import { ref } from "vue";
import { useRouter } from "vue-router";

import { useAuthStore } from "../stores/auth";

const router = useRouter();

const auth = useAuthStore();

const email = ref("");

const password = ref("");

const error = ref("");

async function iniciarSesion() {

    error.value = "";

    try {

        await auth.login(
            email.value,
            password.value);

        router.push("/solicitudes");

    }

    catch {

        error.value = "Credenciales incorrectas.";

    }

}

</script>