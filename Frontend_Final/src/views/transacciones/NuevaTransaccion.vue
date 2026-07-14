<script setup>
import { ref, onMounted, watch } from 'vue'

import { clienteService } from '@/services/clienteService'
import { transaccionService } from '@/services/transaccionService'


// =====================
// STATE
// =====================
const clienteId = ref('')
const codigoCripto = ref('')
const cantidadCripto = ref('')
const tipoTransaccion = ref('')
const exchangeSeleccionado = ref('')
const usarMejorExchange = ref(true)

const clientes = ref([])
const ranking = ref([])

const mensaje = ref('')
const mensajeClase = ref('')

// =====================
// HELPERS
// =====================
const setError = (msg, err) => {
  console.error(err)
  mensaje.value = msg
  mensajeClase.value = 'error'
}

// =====================
// CARGAS
// =====================
const cargarClientes = async () => {
  try {
    const res = await clienteService.obtenerClientes()
    clientes.value = Array.isArray(res) ? res : []
  } catch (err) {
    setError('Error al cargar clientes.', err)
  }
}

const cargarRanking = async () => {
  try {
    // 🔴 IMPORTANTE: no llamar si no hay datos
    if (!codigoCripto.value || !tipoTransaccion.value) {
      ranking.value = []
      exchangeSeleccionado.value = ''
      return
    }

    const res = await transaccionService.obtenerRanking(
      codigoCripto.value,
      tipoTransaccion.value
    )

    ranking.value = Array.isArray(res) ? res : []

    // ✅ solo autoselecciona si corresponde
    if (usarMejorExchange.value && ranking.value.length) {
      exchangeSeleccionado.value = ranking.value[0]?.exchange || ''
    } else {
      exchangeSeleccionado.value = ''
    }

  } catch (err) {
    ranking.value = []
    exchangeSeleccionado.value = ''
    console.error(err)
  }
}

// =====================
// WATCH
// =====================
watch([codigoCripto, tipoTransaccion, usarMejorExchange], cargarRanking)

// =====================
// VALIDACIONES
// =====================
const validarFormulario = () => {
  const errores = []

  if (!clienteId.value) {
    errores.push('Debe seleccionar un cliente.')
  }

  if (!codigoCripto.value) {
    errores.push('Debe seleccionar una criptomoneda.')
  }

  if (!tipoTransaccion.value) {
    errores.push('Debe seleccionar el tipo de transacción.')
  }

  const cantidad = Number(cantidadCripto.value)
  if (!cantidadCripto.value || isNaN(cantidad) || cantidad <= 0) {
    errores.push('Cantidad inválida.')
  }

  if (!usarMejorExchange.value && !exchangeSeleccionado.value) {
    errores.push('Debe seleccionar un exchange.')
  }

  return errores
}

// =====================
// RESET
// =====================
const resetForm = () => {
  clienteId.value = ''
  cantidadCripto.value = ''
  codigoCripto.value = ''   // 👈 antes BTC (mal)
  tipoTransaccion.value = '' // 👈 antes purchase (mal)
  exchangeSeleccionado.value = ''
  usarMejorExchange.value = true
  ranking.value = []
}

// =====================
// ENVIAR
// =====================
const enviarTransaccion = async () => {
  mensaje.value = ''

  const errores = validarFormulario()
  if (errores.length) {
    mensaje.value = errores.join('\n')
    mensajeClase.value = 'error'
    return
  }

  const cliente = clientes.value.find(c => c.id === Number(clienteId.value))
  if (!cliente) return setError('Cliente no encontrado.')

  try {
    await transaccionService.crearTransaccion({
      clienteId: Number(clienteId.value),
      codigoCripto: codigoCripto.value,
      tipoTransaccion: tipoTransaccion.value,
      cantidadCripto: Number(cantidadCripto.value),
      exchange: exchangeSeleccionado.value
    })

    mensaje.value = 'Transacción realizada con éxito!'
    mensajeClase.value = 'success'

    resetForm()
  } 
  
  catch (error) {
      mensaje.value = error.mensajes?.join('\n') || 'Error inesperado'
      mensajeClase.value = 'error'
  }
}

// =====================
// INIT
// =====================
onMounted(() => {
  cargarClientes()
})
</script>

<template>
  <section class="transaccion-container">

    <!-- FORM -->
    <div class="form-card">

      <h2 class="form-title">Nueva Transacción</h2>
      <p class="form-subtitle">Completá los datos para operar</p>

      <form @submit.prevent="enviarTransaccion" novalidate>

        <div class="form-group">
          <label for="cliente">Cliente</label>
          <select id="cliente" v-model="clienteId">
            <option value="" disabled>Seleccione un cliente</option>
            <option v-for="c in clientes" :key="c.id" :value="c.id">
              {{ c.id }} - {{ c.name }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label for="cripto">Criptomoneda</label>
          <select id="cripto" v-model="codigoCripto">
            <option value="" disabled>Seleccione una criptomoneda</option>
            <option value="BTC">BTC</option>
            <option value="ETH">ETH</option>
            <option value="USDC">USDC</option>
          </select>
        </div>

        <div class="form-group">
          <label for="cantidad">Cantidad</label>
          <input
            id="cantidad"
            type="number"
            step="0.0001"
            v-model="cantidadCripto"
          />
        </div>

        <div class="form-group">
          <label for="tipo">Tipo</label>
          <select id="tipo" v-model="tipoTransaccion">
            <option value="" disabled>Seleccione una opción</option>
            <option value="purchase">Comprar</option>
            <option value="sale">Vender</option>
          </select>
        </div>

        <div class="checkbox-group">
          <label>
            <input type="checkbox" v-model="usarMejorExchange" />
            Usar mejor exchange automáticamente
          </label>
        </div>

        <div v-if="!usarMejorExchange" class="form-group">
          <label for="exchange">Exchange</label>
          <select id="exchange" v-model="exchangeSeleccionado">
            <option value="" disabled>Seleccione un exchange</option>
            <option v-for="r in ranking" :key="r.exchange" :value="r.exchange">
              {{ r.exchange }} - $
              {{ tipoTransaccion === 'purchase'
                ? $formatoARS(r.precioCompra)
                : $formatoARS(r.precioVenta) }}
            </option>
          </select>
        </div>

        <button class="button_guardar" type="submit">Realizar Transaccion</button>
      </form>

      <p
        v-if="mensaje"
        class="form-message"
        :class="mensajeClase"
      >
        {{ mensaje }}
      </p>
    </div>

    <!-- RANKING -->
    <div class="table-card">

      <div class="table-header">
        <h3>Mejores opciones</h3>
        <p>Ranking de exchanges según precio</p>
      </div>

      <div v-if="ranking.length" class="table-wrapper">
        <table class="table">
          <thead>
            <tr>
              <th>Exchange</th>
              <th>
                {{ tipoTransaccion === 'purchase' ? 'Compra' : 'Venta' }}
              </th>
            </tr>
          </thead>

          <tbody>
            <tr
              v-for="(r, index) in ranking"
              :key="r.exchange"
              :class="{ mejor: index === 0 }"
            >
              <td>
                {{ r.exchange }}
                <span v-if="index === 0" class="badge badge--best">
                  Mejor
                </span>
              </td>

              <td>
                ${{ tipoTransaccion === 'purchase'
                  ? $formatoARS(r.precioCompra)
                  : $formatoARS(r.precioVenta) }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-else class="empty-state">
        <p>No hay datos disponibles</p>
      </div>

    </div>
  </section>
</template>