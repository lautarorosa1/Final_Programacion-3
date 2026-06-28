<template>
  <section class="principal">
    <div class="hero">
      <h1 class="title">CryptoApp</h1>
      <p class="subtitle">
        Gestioná clientes y transacciones de forma simple
      </p>
    </div>

    <div class="cards">
      <router-link to="/nuevo-cliente" class="card">
        <h3>Nuevo Cliente</h3>
        <p>Registrar un nuevo cliente</p>
      </router-link>

      <router-link to="/listado-clientes" class="card">
        <h3>Clientes</h3>
        <p>Ver listado de clientes</p>
      </router-link>

      <router-link to="/nueva-transaccion" class="card">
        <h3>Nueva Transacción</h3>
        <p>Registrar una operación</p>
      </router-link>

      <router-link to="/historial-movimientos" class="card">
        <h3>Historial</h3>
        <p>Ver movimientos realizados</p>
      </router-link>
    </div>
  </section>
</template>