<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { transaccionService } from '@/services/transaccionService'

const route = useRoute()

const transaccion = ref(null)
const mensaje = ref('')
const mensajeClase = ref('')

// Cargar transacción
async function cargar() {
  mensaje.value = ''

  try {
    transaccion.value = await transaccionService.obtenerTransaccion(
      route.params.id
    )
  } catch (error) {
    mensaje.value = error.message || 'No se pudo cargar la transacción.'
    mensajeClase.value = 'error'
  }
}

onMounted(cargar)
</script>

<template>
  <section class="detalle-container">

    <div class="detalle-header">
      <h2>Detalle de Transacción</h2>
      <p>Información completa de la operación</p>
    </div>

    <p
      v-if="mensaje"
      class="form-message"
      :class="mensajeClase"
    >
      {{ mensaje }}
    </p>

    <!-- INFO -->
    <div v-else-if="transaccion" class="info-card">

      <div class="info-grid">

        <div>
          <span>ID</span>
          <strong>{{ transaccion.id }}</strong>
        </div>

        <div>
          <span>Cliente</span>
          <strong>{{ transaccion.clienteId }}</strong>
        </div>

        <div>
          <span>Cripto</span>
          <strong>{{ transaccion.codigoCripto }}</strong>
        </div>

        <div>
          <span>Tipo</span>
          <strong>
            {{
              transaccion.tipoTransaccion === 'purchase' ||
              transaccion.tipoTransaccion === 0
                ? 'Compra'
                : 'Venta'
            }}
          </strong>
        </div>

        <div>
          <span>Cantidad</span>
          <strong>{{ transaccion.cantidadCripto }}</strong>
        </div>

        <div>
          <span>Monto ARS</span>
          <strong>${{ $formatoARS(transaccion.montoARS) }}</strong>
        </div>

        <div>
          <span>Fecha</span>
          <strong>
            {{ new Date(transaccion.fechaHora).toLocaleString() }}
          </strong>
        </div>

      </div>

    </div>

    <!-- LOADING -->
    <div v-else class="empty-state">
      <p>Cargando transacción...</p>
    </div>

  </section>
</template>