<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { clienteService } from '@/services/clienteService'
import ConfirmModal from '@/components/ConfirmModal.vue'

const clientes = ref([])
const mensaje = ref('')
const mensajeClase = ref('')

const router = useRouter()

const showConfirm = ref(false)
const idAEliminar = ref(null)

// Cargar clientes
async function cargarClientes() {
  mensaje.value = ''
  try {
    clientes.value = await clienteService.obtenerClientes()
  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

// Modal
function pedirConfirmacion(id) {
  idAEliminar.value = id
  showConfirm.value = true
}

async function confirmarBorrado() {
  if (!idAEliminar.value) return

  try {
    await clienteService.eliminarCliente(idAEliminar.value)
    await cargarClientes()
  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

// Navegación
function verCliente(id) {
  router.push({ name: 'ver-cliente', params: { id } })
}

function editarCliente(id) {
  router.push({ name: 'editar-cliente', params: { id } })
}

onMounted(cargarClientes)
</script>

<template>
  <section class="table-container">

    <div class="table-card">

      <!-- HEADER -->
      <div class="table-header">
        <h2>Clientes</h2>
        <p>Listado completo de clientes registrados</p>
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
      <div v-if="clientes.length" class="table-wrapper">
        <table class="table">

          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Email</th>
              <th>Transacciones</th>
              <th>Acciones</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="c in clientes" :key="c.id">

              <td>{{ c.id }}</td>

              <td class="bold">
                {{ c.name }}
              </td>

              <td>{{ c.email }}</td>

              <td>
                <span class="badge">
                  {{ c.cantidadTransacciones }}
                </span>
              </td>

              <td class="acciones">

                <button class="btn-ver" @click="verCliente(c.id)">
                  Ver
                </button>

                <button class="btn-editar" @click="editarCliente(c.id)">
                  Editar
                </button>

                <button
                  class="btn-borrar"
                  @click="pedirConfirmacion(c.id)"
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
        <p>No hay clientes registrados</p>
      </div>

    </div>

    <!-- 🔥 MODAL GLOBAL -->
    <ConfirmModal
      v-model="showConfirm"
      titulo="Eliminar cliente"
      :mensaje="`¿Seguro que querés eliminar a ${
        clientes.find(c => c.id === idAEliminar)?.name || 'este cliente'
      }?`"
      textoConfirmar="Eliminar"
      @confirm="confirmarBorrado"
    />

  </section>
</template>