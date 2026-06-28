<script setup>
import { ref } from 'vue'
import { clienteService } from '@/services/clienteService'

const name = ref('')
const email = ref('')
const mensaje = ref('')
const mensajeClase = ref('')

function validarFormulario() {
  const errores = []

  if (!name.value || name.value.trim() === '') {
    errores.push('El nombre es obligatorio.')
  }

  if (!email.value || !email.value.includes('@')) {
    errores.push('El email es obligatorio y debe ser válido.')
  }

  return errores
}

async function guardarCliente() {
  const errores = validarFormulario()

  if (errores.length > 0) {
    mensaje.value = errores.join('\n')
    mensajeClase.value = 'error'
    return
  }

  const nuevoCliente = {
    name: name.value.trim(),
    email: email.value.trim()
  }

  try {
    await clienteService.crearCliente(nuevoCliente)

    mensaje.value = 'Cliente creado con éxito!'
    mensajeClase.value = 'success'

    name.value = ''
    email.value = ''
  } 
  
  catch (error) {
    mensaje.value = error.mensajes.join('\n')
    mensajeClase.value = 'error'
  }
}
</script>

<template>
  <section class="form-container">
    <div class="form-card">
      <h2 class="form-title">Nuevo Cliente</h2>
      <p class="form-subtitle">
        Completá los datos para registrar un cliente
      </p>

      <form @submit.prevent="guardarCliente" novalidate>
        <div class="form-group">
          <label for="nombre">Nombre</label>
          <input
            id="nombre"
            type="text"
            v-model="name"
            placeholder="Ej: Juan Pérez"
          />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input
            id="email"
            type="email"
            v-model="email"
            placeholder="ejemplo@email.com"
          />
        </div>

        <button class="button_guardar" type="submit">
          Crear Cliente
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