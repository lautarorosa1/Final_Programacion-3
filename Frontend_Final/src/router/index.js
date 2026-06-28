import { createRouter, createWebHistory } from 'vue-router'

// Layout
import MainLayout from '@/layouts/MainLayout.vue'

const routes = [
  {
    path: '/',
    component: MainLayout,
    children: [
      {
        path: '',
        name: 'principal',
        component: () => import('@/views/Principal.vue')
      },

      // Clientes
      {
        path: 'nuevo-cliente',
        name: 'nuevo-cliente',
        component: () => import('@/views/clientes/NuevoCliente.vue')
      },
      {
        path: 'listado-clientes',
        name: 'listado-clientes',
        component: () => import('@/views/clientes/ListadoClientes.vue')
      },
      {
        path: 'cliente/:id',
        name: 'ver-cliente',
        component: () => import('@/views/clientes/VerCliente.vue'),
        props: true
      },
      {
        path: 'editar-cliente/:id',
        name: 'editar-cliente',
        component: () => import('@/views/clientes/EditarCliente.vue'),
        props: true
      },
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router