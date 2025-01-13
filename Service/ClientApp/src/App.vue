<template>
  <KeepAlive>
    <component :is="current" />
  </KeepAlive>
</template>

<script setup lang="ts">
import IndexView from "@/views/IndexView.vue";
import NotFoundView from "@/views/NotFoundView.vue";

import { shallowRef, watch } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const current = shallowRef(IndexView);

watch(() => route.meta.layout, newValue => {
  if (newValue !== "index") {
    current.value = NotFoundView;
  }
});
</script>

<style scoped>

</style>
