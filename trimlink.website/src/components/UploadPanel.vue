<script setup lang="ts">
import { ref } from 'vue'
import { TimeIncrement } from '@/types/TimeIncrement'

const timeIncrementMap = [
  { increment: TimeIncrement.Minutes, display: 'minute(s)' },
  { increment: TimeIncrement.Hours, display: 'hour(s)' },
  { increment: TimeIncrement.Days, display: 'day(s)' },
  { increment: TimeIncrement.Weeks, display: 'week(s)' }
]
const inputScalarUnit = ref(0)
const selectedTimeIncrement = ref(0)
const isNeverExpiresChecked = ref(false)
const isNumber = ref([ (value: any) => isNaN(parseFloat(value)) ? 'You must enter a valid number.' : true ])
</script>

<template>
  <v-container width="100%">
    <v-form>
      <v-row>
        <v-col>
          <h2>Expiration</h2>
        </v-col>
      </v-row>
      <v-row no-gutters>
        <v-col cols="1">
          <v-text-field
            density="compact"
            :rules="isNumber"
            v-model="inputScalarUnit"
            :disabled="isNeverExpiresChecked"
          />
        </v-col>
        <v-col cols="3" class="mx-4">
          <v-select
            :items="timeIncrementMap"
            item-title="display"
            item-value="increment"
            single-line
            density="compact"
            :disabled="isNeverExpiresChecked"
            v-model="selectedTimeIncrement"
          />
        </v-col>
        <v-col>
          <v-checkbox
            density="compact"
            class="text-subtitle-1"
            v-model="isNeverExpiresChecked"
            label="Never Expires"
          />
        </v-col>
      </v-row>
      <v-row>
        <h2>Selected value: {{ selectedTimeIncrement }}</h2>
      </v-row>
    </v-form>
</v-container>
</template>

<style scoped></style>
