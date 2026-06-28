import { http } from './http'

const BASE = '/clientes'

export const clienteService = {
  obtenerClientes() {return http.get(BASE)},

  obtenerCliente(id) {return http.get(`${BASE}/${id}`)},

  crearCliente(cliente) {return http.post(BASE, cliente)},

  editarCliente(id, cliente) {return http.patch(`${BASE}/${id}`, cliente)},

  eliminarCliente(id) {return http.delete(`${BASE}/${id}`)}
}