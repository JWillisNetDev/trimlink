<script setup lang="ts">
import { ref, reactive } from 'vue'
import { TimeIncrement } from '@/types/TimeIncrement'
import { LinkCreateForm } from '@/types/LinkCreateForm'
import { Duration } from 'ts-luxon'
import axios from 'axios'

const timeIncrements = [
  TimeIncrement.Minutes,
  TimeIncrement.Hours,
  TimeIncrement.Days,
  TimeIncrement.Weeks
]

const formData = reactive(new LinkCreateForm())

const expiresIn = ref(0)
const expiresInIncrement = ref(TimeIncrement.Minutes)

async function submit() {
  if (!formData.isNeverExpires) {
    const format = 'ddd.hh:mm:ss.SSS'
    switch (expiresInIncrement.value) {
      case TimeIncrement.Minutes:
        formData.duration = Duration.fromObject({ minutes: expiresIn.value }).toFormat(format) || ''
        break
      case TimeIncrement.Hours:
        formData.duration = Duration.fromObject({ hours: expiresIn.value }).toFormat(format) || ''
        break
      case TimeIncrement.Days:
        formData.duration = Duration.fromObject({ days: expiresIn.value }).toFormat(format) || ''
        break
    }
    console.log(formData.duration)
  }
  axios({
    method: 'post',
    url: '/api/links',
    data: formData,
    withCredentials: false,
    headers: {
      'Content-Type': 'application/json'
    }
  }).catch((err) => console.log(err))
}
</script>

<template>
  <v-form>
    <v-container>
      <v-row justify="center">
        <v-col cols="8">
          <v-text-field
            hide-details="auto"
            label="Url to Shorten"
            v-model="formData.redirectToUrl"
            variant="solo"
          />
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-col cols="3">
          <v-text-field
            type="number"
            hide-details="auto"
            class="mx-2"
            variant="solo"
            :disabled="formData.isNeverExpires"
            v-model="expiresIn"
          />
        </v-col>
        <v-col cols="3">
          <v-select
            hide-details="auto"
            class="mx-2"
            variant="solo"
            :disabled="formData.isNeverExpires"
            :items="timeIncrements"
            v-model="expiresInIncrement"
            label="Time Increment"
            item-title="display"
            item-value="increment"
          />
        </v-col>
        <v-col cols="2">
          <v-checkbox
            hide-details="auto"
            class="mx-2"
            v-model="formData.isNeverExpires"
            label="Never Expires"
          />
        </v-col>
      </v-row>
      <v-row justify="center" class="pa-2">
        <v-btn color="primary" @click="submit"> Shorten Url </v-btn>
      </v-row>
    </v-container>
  </v-form>
</template>

<style scoped></style>
