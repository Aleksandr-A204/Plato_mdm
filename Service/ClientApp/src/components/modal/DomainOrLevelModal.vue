<template>
  <BaseModal
    title="Предметные области"
    display="d-flex flex-column align-items-start"
  >
    <template v-slot:header>
      <div class="input-group">
        <MdmButton
          @click="addItem"
          class="btn-light"
        >
          Добавить
        </MdmButton>
        <MdmButton
          @click="removeItem"
          class="btn-light"
        >
          Удалить
        </MdmButton>
        <MdmButton
          class="btn-primary"
          @click="save"
        >
          Сохранить
        </MdmButton>
        <MdmButton
          class="btn-secondary"
          data-bs-dismiss="modal"
        >
          Отмена
        </MdmButton>
      </div>
    </template>

    <MdmTable
      :columns="columns"
      :elements="elements"
    >

    </MdmTable>
  </BaseModal>
</template>


<script setup lang="ts">
import MdmButton from "@/components/extended/MdmButton.vue";

import MdmTable from "@/components/extended/MdmTable.vue";
import { ref } from "vue";

const columns = ref(["Предметная область", "Цвет"]);
const elements = ref([
  { name: "Биоразнобразие", description: "#" },
  { name: "ООПТ", description: "#" },
  { name: "Озеление", description: "#" }
]);

const save = () => {
  console.log("Item save");
};

const addItem = () => {
  console.log("Item added");
};

const removeItem = () => {
  console.log("Item removed");
};
</script>

<style scoped>
.input-group{
  word-break: break-word;
}
</style>
