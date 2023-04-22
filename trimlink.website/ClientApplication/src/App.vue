<script setup lang="ts">
import UploadPanel from '@/components/UploadPanel.vue'
</script>

<template>
  <v-container fluid>
    <header>
      <h1>Hello, world!</h1>
    </header>

    <main>
      <UploadPanel />
    </main>
  </v-container>
</template>

<style scoped>
main {
  width: 100%;
}

header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
