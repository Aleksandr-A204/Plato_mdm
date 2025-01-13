<template>
  <div class="dropdown">
    <div class="d-flex justify-content-between align-items-center">
      <span class="me-2">{{ title }}</span>
      <button class="btn btn-outline-secondary border-white" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        <i class="fa-solid fa-ellipsis-vertical"></i>
      </button>
        <ul class="dropdown-menu">
          <li
            v-for="(item, index) in items"
            :key="index"
          >
            <MdmButton
              class="dropdown-item btn-primary"
              @click="$emit('showModal', item.modalType)"
            >
              <MdmWrap
                :custom-label="item.label"
                label-margin="mx"
              >
                <template v-slot:icon>
                  <i :class="item.icon"></i>
                </template>
              </MdmWrap>
            </MdmButton>
          </li>
        </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { MenuItem } from "@/types/extended";

defineEmits(["showModal"]);

withDefaults(defineProps<{
  title: string;
  items: Array<MenuItem>
}>(),
{
  title: "Неизвестное наименование"
});
</script>

<style scoped>
/* Оставьте эти стили в отдельном файле или в компоненте Dropdown.vue */
</style>
