<template>
  <div v-if="modelValue" class="modal-overlay" @click.self="cancelar">

    <div class="modal">

      <h3 class="modal-title">{{ titulo }}</h3>

      <p class="modal-text">
        {{ mensaje }}
      </p>

      <div class="modal-actions">
        <button class="btn-editar" @click="cancelar">
          Cancelar
        </button>

        <button class="btn-borrar" @click="confirmar">
          {{ textoConfirmar }}
        </button>
      </div>

    </div>

  </div>
</template>

<script setup>

const props = defineProps({
  modelValue: Boolean,
  titulo: {
    type: String,
    default: 'Confirmar acción'
  },
  mensaje: {
    type: String,
    default: '¿Estás seguro?'
  },
  textoConfirmar: {
    type: String,
    default: 'Eliminar'
  }
})

const emit = defineEmits(['update:modelValue', 'confirm'])

function cancelar() {
  emit('update:modelValue', false)
}

function confirmar() {
  emit('confirm')
  emit('update:modelValue', false)
}
</script>