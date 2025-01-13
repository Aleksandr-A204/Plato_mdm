<template>
  <div>
    <slot name="icon" />
    <label :class="`${labelMargin}-${margineSize}`">{{ customLabel }}</label>
    <slot />
  </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  customLabel: string | object,
  labelMargin?: string,
  margineSize?: string
}>(),
{
  labelMargin: "m",
  margineSize: "1"
}
);
</script>

<style scoped>
</style>
