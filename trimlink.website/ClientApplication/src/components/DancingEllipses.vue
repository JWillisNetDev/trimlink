<script setup lang="ts">
import { reactive } from 'vue'

const props = defineProps<{
  dotSize?: string
  dotColor?: string
}>()

const dotStyle = reactive({
  width: props.dotSize ?? '5px',
  height: props.dotSize ?? '5px',
  backgroundColor: props.dotColor ?? 'black'
})
</script>

<template>
  <div class="ellipses">
    <span class="dot-1" :style="dotStyle"></span>
    <span class="dot-2" :style="dotStyle"></span>
    <span class="dot-3" :style="dotStyle"></span>
  </div>
</template>

<style scoped>
.ellipses {
  position: relative;
}
.dot-1,
.dot-2,
.dot-3 {
  display: inline-block;
  margin-left: 5px;
  border-radius: 50%;
  animation: bounce 2s ease-in-out infinite;
}
.dot-2 {
  animation-delay: 300ms;
}
.dot-3 {
  animation-delay: 600ms;
}
@keyframes bounce {
  0%,
  70%,
  100% {
    transform: translateY(0);
  }
  35% {
    transform: translateY(-10px);
  }
}
</style>
