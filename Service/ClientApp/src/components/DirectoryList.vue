<template>
  <div
    class="card m-2"
    v-for="(directory, index) in directories"
    :key="`card-${index}`"
  >
    <RouterLink
      class="text-dark"
      :to="{ name: 'directory', params: { id: directory.id } }"
    >
      <div class="card-body d-flex justify-content-between align-items-center">
        <div>
          <h6 class="card-title">{{ directory.name }}</h6>
          <span class="card-text">{{ directory.directoryDomain?.name }}</span>
          <span class="card-text ms-2">{{ directory.directoryLevel?.name }}</span>
        </div>
        <a
          href="#"
          @click.stop="$emit('onShowModal', directory )"
        >
          <i class="fa-duotone fa-solid fa-gear"></i>
        </a>
      </div>
    </RouterLink>
  </div>
</template>

<script setup lang="ts">
import type { Directory } from "@/models/models";

defineEmits(["onShowModal", "handleName"]);

defineProps<{
  directories: Array<Directory>;
}>();
</script>

<style scoped>
.card{
  height: 5rem;
}

.card:hover{
	box-shadow: 0px 2px 0px 3px #5efc5e;
}
</style>
