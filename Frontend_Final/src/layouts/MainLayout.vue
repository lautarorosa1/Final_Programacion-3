<script setup>
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <div class="layout">

    <!-- SIDEBAR -->
    
      <aside class="sidebar" :class="{ hidden: route.path === '/' }">

        <div class="sidebar-header">
          <router-link to="/" class="logo">
            💰 CryptoApp
          </router-link>
        </div>

        <nav class="sidebar-nav">

          <router-link to="/nuevo-cliente" class="nav-item">
            <span>👤</span>
            <span>Nuevo Cliente</span>
          </router-link>

          <router-link to="/listado-clientes" class="nav-item">
            <span>📋</span>
            <span>Clientes</span>
          </router-link>

          <router-link to="/nueva-transaccion" class="nav-item">
            <span>💸</span>
            <span>Nueva Transacción</span>
          </router-link>

          <router-link to="/historial-movimientos" class="nav-item">
            <span>📊</span>
            <span>Historial</span>
          </router-link>

        </nav>

      </aside>
    

    <!-- CONTENIDO -->
    <main class="contenido" :class="{ full: route.path === '/' }">
      <router-view v-slot="{ Component }">
        <transition name="fade-slide" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

  </div>
</template>