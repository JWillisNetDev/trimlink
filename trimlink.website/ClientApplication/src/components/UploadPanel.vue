<script setup lang="ts">
import {reactive, ref, computed} from 'vue'
import {TimeIncrement} from '@/types/TimeIncrement'
import {Duration} from 'ts-luxon'
import LinkService from '@/services/LinkService'
import useClipboard from 'vue-clipboard3'

const { toClipboard } = useClipboard() 

const timeIncrements = [
  TimeIncrement.Minutes,
  TimeIncrement.Hours,
  TimeIncrement.Days,
  TimeIncrement.Weeks
]

interface Form {
    url: string,
    timeValue: number,
    timeInterval: TimeIncrement,
    neverExpires: boolean
}

const formData = reactive<Form>({
    url: '',
    timeValue: 0,
    timeInterval: TimeIncrement.Minutes,
    neverExpires: false
})

const expiresIn = ref<number>(0)
const expiresInIncrement = ref<TimeIncrement>(TimeIncrement.Minutes)
const shouldShowSnackbar = ref<boolean>(false)
const link = ref<string>()
const fullUrl = computed(() => `${window.location.origin}/to/${link.value}`)

async function submit() {
    if (formData.neverExpires) {
        link.value = await LinkService.createLink(formData.url) ?? ''
    }
    else {
        let duration: string = '';
        const format = 'ddd.hh:mm:ss.SSS'
        switch (formData.timeInterval) {
            case TimeIncrement.Minutes:
                duration = `0.00:${formData.timeValue}:00.000`
                break
            case TimeIncrement.Hours:
                duration = `0.${formData.timeValue}:00:00.000`
                break
            case TimeIncrement.Days:
                duration = `${formData.timeValue}.00:00:00.000`
                break
            case TimeIncrement.Weeks:
                duration = `${formData.timeValue * 7}.00:00:00.000`
                break
        }
        link.value = await LinkService.createLinkExpires(formData.url, duration) ?? ''
    }
    if (link.value)
      shouldShowSnackbar.value = true
}
</script>

<template>
  <v-snackbar v-model="shouldShowSnackbar">
    Link created at <a v-if="link" :href="`/to/${link}`">{{ fullUrl }}</a>!
    
    <template v-slot:actions>
      <v-btn icon="mdi-content-copy" @click="async () => await toClipboard(fullUrl)" />
      <v-btn icon="mdi-close-box" @click="() => shouldShowSnackbar = false" />
    </template>
  </v-snackbar>
  <v-form>
    <v-container>
      <v-row justify="center">
        <v-col cols="8">
          <v-text-field
            hide-details="auto"
            label="Url to Shorten"
            v-model="formData.url"
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
            :disabled="formData.neverExpires"
            v-model="formData.timeValue"
          />
        </v-col>
        <v-col cols="3">
          <v-select
            hide-details="auto"
            class="mx-2"
            variant="solo"
            :disabled="formData.neverExpires"
            :items="timeIncrements"
            v-model="formData.timeInterval"
            label="Time Increment"
            item-title="display"
            item-value="increment"
          />
        </v-col>
        <v-col cols="2">
          <v-checkbox
            hide-details="auto"
            class="mx-2"
            v-model="formData.neverExpires"
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

<style scoped>
</style>
