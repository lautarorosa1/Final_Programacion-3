import { createApp } from 'vue'

import App from './App.vue'
import router from './router'
import priceNumber from './utils/priceNumber.js'
import './assets/styles/main.css'

const app = createApp(App)

app.use(router)
app.use(priceNumber)

app.mount('#app')
