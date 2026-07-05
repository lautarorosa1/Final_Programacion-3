export default {
  install(app) {
    app.config.globalProperties.$formatoARS = (valor) => {
      if (isNaN(valor)) return "0,00";

      const parts = Number(valor).toFixed(2).split('.');
      const enteroConMiles = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");

      return `${enteroConMiles},${parts[1]}`;
    };
  }
};