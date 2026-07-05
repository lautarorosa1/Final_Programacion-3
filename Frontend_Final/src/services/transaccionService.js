import { http } from './http'

const BASE = '/transacciones'

export const transaccionService = {
  obtenerTransaccion(id) {
    return http.get(`${BASE}/${id}`)
  },

  obtenerTransacciones(clienteId = '') {
  const query = clienteId ? `?clienteId=${clienteId}` : ''
  return http.get(`${BASE}${query}`)
},

  crearTransaccion(transaccion) {
    return http.post(BASE, transaccion)
  },

  editarTransaccion(id, transaccion) {
    return http.patch(`${BASE}/${id}`, transaccion)
  },

  eliminarTransaccion(id) {
    return http.delete(`${BASE}/${id}`)
  }
}

