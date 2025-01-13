<template>
  <textarea
    class="form-control"
    :value="modelValue"
    :rows="rows"
    :placeholder="placeholder"
    @input="input"
  ></textarea>
</template>

<script setup lang="ts">

const emit = defineEmits(["update:modelValue"]);

withDefaults(defineProps<{
  modelValue: string | number;
  placeholder?: string;
  rows?: string;
}>(),
{
  modelValue: "",
  placeholder: "(нет данных)",
  rows: "4"
}
);

const input = (e: Event) => {
  const target = e.target as HTMLInputElement;
  emit("update:modelValue", target.value);
};
</script>

<style scoped>

</style>
