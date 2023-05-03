<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import LinkService from '@/services/LinkService'

const route = useRoute()

const link = ref<string>()

type Greeting = (arg: string) => string
const greetings: Greeting[] = [
    (link) => `What was that? Oh, you must've meant ${link}`,
    (link) => `Sit back and relax. You're being redirected to ${link}`,
    (link) => `You dare bring light into my lair? I banish you to ${link}`,
];
const random = Math.floor(Math.random() * greetings.length)
const greeting = computed(() => greetings[random](link.value ?? ''))

onMounted(async () => {
    // Request to the API to get the URL for redirection
    const token = route.params.token as string
    link.value = await LinkService.getLinkByToken(token)
    
    if (link.value) {
        window.location.href = link.value
    }
})
</script>

<template>
  <div>
    <h1>{{ greeting }}</h1>
    <p>Not being redirected? <a :href="link">Try clicking here.</a></p>
  </div>
</template>

<style scoped>
</style>