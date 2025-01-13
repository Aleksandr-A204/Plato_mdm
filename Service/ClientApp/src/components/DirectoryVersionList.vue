<template>
  <div
    class="card m-2"
    v-for="(version, index) in versions"
    :key="`card-${index}`"
  >
    <RouterLink
      class="text-black"
      :to="{ name: 'data', params: { varsionId: version.id } }"
    >
      <div class="card-body d-flex justify-content-between align-items-center">
        <div>
          <div class="card-text">{{ version.version }}</div>
          <div class="card-text">{{ version.versionDate }}</div>
          <div class="card-text">{{ version.dataSourceName }}</div>
          <div class="card-text">{{ version.dataSourceDate }}</div>
        </div>
        <a
          href="#"
          @click="$emit('hindleVersion', version)"
        >
          <i class="fa-duotone fa-solid fa-gear"></i>
        </a>
      </div>
    </RouterLink>
  </div>
</template>

<script setup lang="ts">
import type { DirectoryVersion } from "@/models/models";

defineEmits(["hindleVersion"]);

defineProps({
  versions: {
    type: Array<DirectoryVersion>,
    required: true
  }
});
</script>

<style scoped>
.card{
  height: 8rem;
}
.card:hover{
	box-shadow: 0px 2px 0px 3px #5efc5e;
}
</style>
