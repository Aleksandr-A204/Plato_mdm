<template>
  <div class="input-group">
    <MdmInput
      :modelValue="modelValue"
      placeholder="Поиск..."
      @update:modelValue="(value: string) => $emit('update:modelValue', value)"
    />
    <slot />
  </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  modelValue: string;
}>(), {
  modelValue: ""
});

defineEmits(["update:modelValue"]);

// const resetSearch = () => {
//   //this.onSearch(this.directories);
//   searchInput.value = "";
// };
</script>

<style scoped>
/* Оставьте эти стили в отдельном файле или в компоненте Search.vue */
</style>
