import { createApp } from 'vue'
import App from './App.vue'
import { mdi } from 'vuetify/iconsets/mdi'

import './assets/main.css'
import '@mdi/font/css/materialdesignicons.css'

// Vue Router
import { createRouter, createWebHistory } from 'vue-router'
import router from '@/router'

import axios from 'axios'

// Vuetify setup
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
  icons: {
    defaultSet: 'mdi',
    sets: {
      mdi
    }
  }
})

createApp(App).use(vuetify).use(router).mount('#app')
