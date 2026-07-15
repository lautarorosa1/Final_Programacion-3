<script setup>
import { ref, onMounted, watch, nextTick, onBeforeUnmount } from 'vue'
import { useRoute } from 'vue-router'
import { clienteService } from '@/services/clienteService'
import { Chart, PieController, ArcElement, Tooltip, Legend } from 'chart.js'

Chart.register(PieController, ArcElement, Tooltip, Legend)

const chartRef = ref(null)
let chartInstance = null

const route = useRoute()

const cliente = ref(null)
const estado = ref(null)
const mensaje = ref('')
const mensajeClase = ref('')

async function cargar() {
  mensaje.value = ''

  try {
    const id = route.params.id

    cliente.value = await clienteService.obtenerCliente(id)
    estado.value = await clienteService.obtenerEstadoCliente(id)

  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

function crearGrafico() {
  if (!estado.value?.criptos?.length || !chartRef.value) return

  const labels = estado.value.criptos.map(c => c.codigo.toUpperCase())
  const data = estado.value.criptos.map(c => c.dineroARS)

  if (chartInstance) chartInstance.destroy()

  chartInstance = new Chart(chartRef.value, {
    type: 'pie',
    data: {
      labels,
      datasets: [
        {
          data,
          backgroundColor: ['#F7931A', '#627EEA', '#10B981']        
        }
      ]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false, // 👈 CLAVE
      plugins: {
        legend: { position: 'bottom' }
      }
    }
  })
}

watch(estado, async () => {
  await nextTick()
  crearGrafico()
})

onBeforeUnmount(() => {
  if (chartInstance) chartInstance.destroy()
})

onMounted(cargar)
</script>

<template>
  <section class="detalle-container">

    <div class="detalle-header">
      <h2>Detalle del Cliente</h2>
      <p>Información general y estado financiero</p>
    </div>

    <p
      v-if="mensaje"
      class="form-message"
      :class="mensajeClase"
    >
      {{ mensaje }}
    </p>

    <!-- INFO CLIENTE -->
    <div v-if="cliente" class="info-card">
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

    <!-- ESTADO -->
    <div v-if="estado" class="estado-card">

      <h3>Estado Financiero</h3>

      <div class="table-wrapper">
        <table class="table">
          <thead>
            <tr>
              <th>Cripto</th>
              <th>Cantidad</th>
              <th>ARS</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="c in estado.criptos" :key="c.codigo">
              <td class="bold">{{ c.codigo }}</td>
              <td>{{ c.cantidad }}</td>
              <td>${{ $formatoARS(c.dineroARS) }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="total">
        Total: ${{ $formatoARS(estado.totalARS) }}
      </div>

      <div class="chart-section">
        <h3>Composición de la cartera</h3>

        <div v-if="estado.criptos.length" class="chart-container">
          <canvas ref="chartRef"></canvas>
        </div>

        <p v-else class="sin-datos">
          El cliente no posee criptomonedas para graficar.
        </p>
      </div>

    </div>

  </section>
</template>