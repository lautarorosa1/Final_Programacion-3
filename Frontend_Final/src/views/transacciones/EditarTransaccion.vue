<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { transaccionService } from '@/services/transaccionService'

const route = useRoute()
const router = useRouter()

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

    // 🔥 SOLUCIÓN
    transaccion.value.codigoCripto =
      transaccion.value.codigoCripto?.toUpperCase()

  } catch (error) {
    mensaje.value = error.mensajes?.join('\n') || 'Error inesperado'
    mensajeClase.value = 'error'
  }
}

// Guardar cambios
async function guardar() {
  try {
    const payload = {
      ...transaccion.value,
      montoARS: normalizarNumero(transaccion.value.montoARS),
      cantidadCripto: normalizarNumero(transaccion.value.cantidadCripto)
    }

    await transaccionService.editarTransaccion(
      route.params.id,
      payload
    )

    router.push({ name: 'historial-movimientos' })

  } catch (error) {
    mensaje.value = error.mensajes?.join('\n') || 'Error inesperado'
    mensajeClase.value = 'error'
  }
}

function normalizarNumero(valor) {
  if (valor === null || valor === undefined) return null

  let str = valor.toString().trim()

  // Si tiene coma, asumimos formato argentino
  if (str.includes(',')) {
    return Number(
      str
        .replace(/\./g, '')  // elimina miles
        .replace(',', '.')   // decimal correcto
    )
  }

  // Si NO tiene coma → ya está bien (usa punto decimal)
  return Number(str)
}

onMounted(cargar)
</script>

<template>
  <section class="form-container">

    <div class="form-card">

      <h2 class="form-title">Editar Transacción</h2>
      <p class="form-subtitle">
        Modificá los datos de la operación
      </p>

      <p
        v-if="mensaje"
        class="form-message"
        :class="mensajeClase"
      >
        {{ mensaje }}
      </p>

      <form v-if="transaccion" @submit.prevent="guardar">

        <div class="form-group">
          <label for="cripto">Criptomoneda</label>

          <select id="cripto" v-model="transaccion.codigoCripto">
            <option value="BTC">BTC</option>
            <option value="ETH">ETH</option>
            <option value="USDC">USDC</option>
          </select>
        </div>

        <div class="form-group">
          <label for="tipo">Tipo</label>
          <select id="tipo" v-model="transaccion.tipoTransaccion">
            <option value="purchase">Compra</option>
            <option value="sale">Venta</option>
          </select>
        </div>

        <div class="form-group">
          <label for="cantidad">Cantidad</label>
          <input
            id="cantidad"
            type="number"
            step="0.0001"
            inputmode="decimal"
            v-model="transaccion.cantidadCripto"
          />
        </div>

        <div class="form-group">
          <label for="monto">Monto ARS</label>
          <input
            id="monto"
            type="text"
            inputmode="decimal"
            v-model="transaccion.montoARS"
          />
        </div>

        <button class="button_guardar" type="submit">
          Guardar Cambios
        </button>

      </form>

    </div>

  </section>
</template>