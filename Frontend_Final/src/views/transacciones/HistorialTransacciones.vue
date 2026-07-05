<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'

import { clienteService } from '@/services/clienteService'
import { transaccionService } from '@/services/transaccionService'

import ConfirmModal from '@/components/ConfirmModal.vue'

const router = useRouter()

const clientes = ref([])
const transacciones = ref([])
const clienteFilter = ref('')

const mensaje = ref('')
const mensajeClase = ref('')

// MODAL
const showConfirm = ref(false)
const idAEliminar = ref(null)

// ERROR HANDLER
const setError = (msg, err) => {
  console.error(err)
  mensaje.value = msg
  mensajeClase.value = 'error'
}

// CARGA DATOS
const cargarClientes = async () => {
  try {
    const res = await clienteService.obtenerClientes()
    clientes.value = Array.isArray(res) ? res : []
  } catch (err) {
    setError('Error al cargar clientes.', err)
  }
}

const cargarTransacciones = async () => {
  try {
    const res = await transaccionService.obtenerTransacciones(clienteFilter.value)
    transacciones.value = Array.isArray(res) ? res : []
    mensaje.value = ''
    mensajeClase.value = ''
  } catch (err) {
    transacciones.value = []
    setError('Error al cargar transacciones.', err)
  }
}

// FILTRO
const filtrar = () => {
  cargarTransacciones()
}

// HELPERS
const formatoFecha = f => {
  const d = new Date(f)
  return f && !isNaN(d) ? d.toLocaleString() : '-'
}

const nombreCliente = id =>
  clientes.value.find(c => c.id === id)?.name || id

// COMPUTED
const transaccionesFiltradas = computed(() =>
  (transacciones.value || [])
    .slice()
    .sort((a, b) => new Date(b.fechaHora || 0) - new Date(a.fechaHora || 0))
)

// NAV
const verTransaccion = id => router.push(`/transaccion/${id}`)
const editarTransaccion = id => router.push(`/editar-transaccion/${id}`)

// MODAL FLOW
const pedirConfirmacion = id => {
  idAEliminar.value = id
  showConfirm.value = true
}

const confirmarBorrado = async () => {
  if (!idAEliminar.value) return

  try {
    await transaccionService.eliminarTransaccion(idAEliminar.value)
    showConfirm.value = false
    idAEliminar.value = null
    await cargarTransacciones()
  } catch (err) {
    setError('No se pudo borrar la transacción.', err)
  }
}

// INIT
onMounted(async () => {
  await cargarClientes()
  await cargarTransacciones()
})
</script>

<template>
  <section class="table-container">
    <div class="table-card">

      <!-- HEADER -->
      <div class="table-header">
        <h2>Historial de Movimientos</h2>
        <p>Listado completo de transacciones realizadas</p>
      </div>

      <!-- FILTRO -->
      <div class="form-group">
        <label for="clienteFiltro">Filtrar por cliente</label>
        <select
          id="clienteFiltro"
          v-model="clienteFilter"
          @change="filtrar"
        >
          <option value="">Todos</option>
          <option
            v-for="c in clientes"
            :key="c.id"
            :value="c.id"
          >
            {{ c.id }} - {{ c.name }}
          </option>
        </select>
      </div>

      <!-- MENSAJE -->
      <p
        v-if="mensaje"
        class="form-message"
        :class="mensajeClase"
      >
        {{ mensaje }}
      </p>

      <!-- TABLA -->
      <div v-if="transaccionesFiltradas.length" class="table-wrapper">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Criptomoneda</th>
              <th>Exchange</th>
              <th>Tipo</th>
              <th>Cantidad</th>
              <th>Monto ARS</th>
              <th>Fecha</th>
              <th>Acciones</th>
            </tr>
          </thead>

          <tbody>
            <tr
              v-for="t in transaccionesFiltradas"
              :key="t.id"
            >
              <td>{{ t.id }}</td>

              <td>{{ nombreCliente(t.clienteId) }}</td>

              <td class="bold">{{ t.codigoCripto }}</td>

              <td>{{ t.exchange }}</td>

              <td>
                <span class="badge">
                  {{
                    t.tipoTransaccion === 'purchase' || t.tipoTransaccion === 0
                      ? 'Compra'
                      : 'Venta'
                  }}
                </span>
              </td>

              <td>
                {{
                  Number(t.cantidadCripto || 0).toLocaleString(undefined, {
                    maximumFractionDigits: 8
                  })
                }}
              </td>

              <td>
                ${{ Number(t.montoARS || 0).toLocaleString() }}
              </td>

              <td>{{ formatoFecha(t.fechaHora) }}</td>

              <td class="acciones">
                <button class="btn-ver" @click="verTransaccion(t.id)">
                  Ver
                </button>

                <button class="btn-editar" @click="editarTransaccion(t.id)">
                  Editar
                </button>

                <button
                  class="btn-borrar"
                  @click="pedirConfirmacion(t.id)"
                >
                  Borrar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- EMPTY -->
      <div v-else class="empty-state">
        <p>No hay transacciones registradas</p>
      </div>

    </div>

    <ConfirmModal
      v-model="showConfirm"
      titulo="Eliminar transacción"
      :mensaje="`¿Seguro que querés eliminar la transacción #${idAEliminar || ''}?`"
      textoConfirmar="Eliminar"
      @confirm="confirmarBorrado"
    />
  </section>
</template>