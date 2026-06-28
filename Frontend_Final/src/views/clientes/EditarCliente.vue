<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { clienteService } from '@/services/clienteService'

const route = useRoute()
const router = useRouter()

const cliente = ref(null)
const mensaje = ref('')
const mensajeClase = ref('')

async function cargar() {
  mensaje.value = ''

  try {
    cliente.value = await clienteService.obtenerCliente(route.params.id)
  } catch (error) {
    mensaje.value = error.mensajes?.join('\n') || 'Error inesperado'
    mensajeClase.value = 'error'
  }
}

async function guardar() {
  try {
    await clienteService.editarCliente(route.params.id, cliente.value)
    router.push({ name: 'listado-clientes' })
  } catch (error) {
    mensaje.value = error.mensajes?.join('\n') || 'Error inesperado'
    mensajeClase.value = 'error'
  }
}

onMounted(cargar)
</script>

<template>
  <section class="form-container">

    <div class="form-card">

      <h2 class="form-title">Editar Cliente</h2>
      <p class="form-subtitle">
        Modificá los datos del cliente
      </p>

      <form v-if="cliente" @submit.prevent="guardar" novalidate>

        <div class="form-group">
          <label for="nombre">Nombre</label>
          <input
            id="nombre"
            v-model="cliente.name"
            placeholder="Nombre del cliente"
          />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input
            id="email"
            type="email"
            v-model="cliente.email"
            placeholder="email@ejemplo.com"
          />
        </div>

        <button class="button_guardar" type="submit">
          Guardar Cambios
        </button>

      </form>

      <p
        v-if="mensaje"
        class="form-message"
        :class="mensajeClase"
      >
        {{ mensaje }}
      </p>

    </div>

  </section>
</template>