<script setup lang="ts">
import UploadPanel from '@/components/UploadPanel.vue'
</script>

<template>
  <v-layout >
    <v-app-bar elevation="1">
      <v-toolbar-title>Shorten Url</v-toolbar-title>
    </v-app-bar>
    <v-main>
      <UploadPanel />
    </v-main>
  </v-layout>
</template>

<style scoped>
</style>
