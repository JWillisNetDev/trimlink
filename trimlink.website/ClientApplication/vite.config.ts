import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from 'vite-plugin-vuetify'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vuetify({ autoImport: true }),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    https: false,
    port: 5173,
    strictPort: true,
    proxy: {
      '/api': { target: 'https://localhost:7295', secure: false },
      '/to': { target: 'https://localhost:7295', secure: false }
    },
  }
})
