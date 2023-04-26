import { createApp } from 'vue'
import App from './App.vue'
import { mdi } from 'vuetify/iconsets/mdi'

import './assets/main.css'
import '@mdi/font/css/materialdesignicons.css'

// Vue Router
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    name: 'home',
    path: '/',
    component: () => import('@/views/TheHome.vue')
  },
  {
    name: 'notfound',
    path: '/:pathMatch(.*)*', // https://router.vuejs.org/guide/migration/#removed-star-or-catch-all-routes
    component: () => import('@/views/NotFound.vue')
  }
]

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

const router = createRouter({
  history: createWebHistory(),
  routes
})

createApp(App).use(vuetify).use(router).mount('#app')
