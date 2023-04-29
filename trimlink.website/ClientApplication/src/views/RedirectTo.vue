<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'

const route = useRoute()

onMounted(async () => {
    // Request to the API to get the URL for redirection
    const link: string | undefined = await axios.get(`/api/links/${route.params.token}`)
        .then(response => response.data)
        .catch(error => undefined)
    
    if (link) {
        window.location.href = link
    }
})
</script>

<template>
  <div>
    If you  can see this then uh-oh lol.
  </div>
</template>

<style scoped>
</style>