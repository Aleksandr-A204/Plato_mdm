<template>
  <button
    class="btn"
    :type="type"
    @click="$emit('click')"
  >
    <slot />
  </button>
</template>

<script setup lang="ts">
defineEmits(["click"]);

const props = defineProps<{
  type?: "button" | "submit" | "reset"; // Указываем конкретные допустимые значения
}>();

const type = props.type || "button"; // Если type не задан, используем значение по умолчанию
</script>

<style scoped>
</style>
