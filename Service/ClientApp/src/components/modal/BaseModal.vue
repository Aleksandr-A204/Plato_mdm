<template>
  <div class="modal fade" ref="modal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">{{ title }}</h5>
          <slot name="header" />
        </div>
        <div class="modal-body">
          <slot />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Modal } from "bootstrap";
import { onMounted, onUnmounted, ref } from "vue";

interface ModalConfirmDeleteProps {
  title: string;
  // onConfirm: () => void;
}

withDefaults(defineProps<ModalConfirmDeleteProps>(), {
  title: "string"
});

const modal = ref<HTMLElement | null>(null);
let modalInstance: Modal | null = null;

const emit = defineEmits(["close"]);

onMounted(() => {
  if (modal.value) {
    modalInstance = new Modal(modal.value);

    // Обработчик события скрытия модального окна
    modal.value.addEventListener("hidden.bs.modal", () => {
      emit("close");
    });
  }
});

onUnmounted(() => {
  if (modalInstance) {
    modalInstance.dispose();
  }
});

// const confirmDelete = () => {
//   props.onConfirm();
//   close();
// };

const open = () => {
  if (modalInstance) {
    modalInstance.show();
  }
};

const close = () => {
  if (modalInstance) {
    modalInstance.hide();
  }
};

defineExpose({ open, close });
</script>
