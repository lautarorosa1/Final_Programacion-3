<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { clienteService } from '@/services/clienteService'

const route = useRoute()

const cliente = ref(null)
const mensaje = ref('')
const mensajeClase = ref('')

// Cargar cliente
async function cargar() {
  mensaje.value = ''
  mensajeClase.value = ''

  try {
    cliente.value = await clienteService.obtenerCliente(route.params.id)
  } catch (error) {
    mensaje.value = error.message || 'No se pudo cargar el cliente.'
    mensajeClase.value = 'error'
  }
}

onMounted(cargar)
</script>

<template>
  <section class="detalle-container">

    <div class="detalle-header">
      <h2>Detalle del Cliente</h2>
      <p>Información completa del cliente</p>
    </div>

    <p
      v-if="mensaje"
      class="form-message"
      :class="mensajeClase"
    >
      {{ mensaje }}
    </p>

    <!-- INFO -->
    <div v-else-if="cliente" class="info-card">
      <div class="info-grid">

        <div>
          <span>ID</span>
          <strong>{{ cliente.id }}</strong>
        </div>

        <div>
          <span>Nombre</span>
          <strong>{{ cliente.name }}</strong>
        </div>

        <div>
          <span>Email</span>
          <strong>{{ cliente.email }}</strong>
        </div>

        <div>
          <span>Transacciones</span>
          <strong>{{ cliente.cantidadTransacciones }}</strong>
        </div>

      </div>
    </div>

    <!-- LOADING -->
    <div v-else class="empty-state">
      <p>Cargando cliente...</p>
    </div>

  </section>
</template>