<template>
  <input
    class="form-control"
    ref="mdm-input"
    :value="modelValue"
    :type="type"
    :placeholder="placeholder"
    :disabled=disabled
    @input="input"
    @blur="blur"
  />
</template>

<script setup lang="ts">

withDefaults(defineProps<{
  modelValue: string | number;
  placeholder?: string;
  type?: string;
  disabled?: boolean;
}>(),
{
  modelValue: "",
  placeholder: "(нет данных)",
  type: "text",
  disabled: false
});

const emit = defineEmits(["update:modelValue", "blur"]);

const input = (e: Event) => {
  const target = e.target as HTMLInputElement;
  emit("update:modelValue", target.value);
};

const blur = (e: Event) => {
  emit("blur", e);
};
</script>

<style scoped>
</style>
