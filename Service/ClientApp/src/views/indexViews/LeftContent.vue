<template>
  <header class="vh-100 d-flex flex-column">
    <div class="sticky-top bg-white mx-2">
      <h2 class="text-center p-3">Плато MDM</h2>
      <DropdownMenu
        title="Справочник"
        :items="menuItems"
        @show-modal="(modalType: string) => showModal(modalType, {})"
      />

      <SearchBar
        :modelValue=store.keyWord
        @update:modelValue="(value: string) => handleSearch(value)"
      >
        <MdmButton class="btn-secondary">
          <i class="fa-solid fa-filter"></i>
        </MdmButton>
        <MdmButton class="btn-secondary">
          <i class="fa-solid fa-arrow-down-wide-short"></i>
        </MdmButton>
      </SearchBar>
    </div>

    <!-- <div v-if="store.dataLoading">Загрузка...</div> -->
    <div class="overflow-auto flex-fill">
      <DirectoryList
        :directories="store.allDirectories"
        @on-show-modal="(directoryObject: Directory) => showModal('directory', directoryObject)"
      />
    </div>

    <DirectoryModal
      :params="currentDirectory || {}"
      :is-show-modal="isShowModal"
      @close="isShowModal = false"
      @save="saveDirectory"
      @delete="deleteDirectory"
    />
  </header>
</template>

<script setup lang="ts">
import DirectoryList from "@/components/DirectoryList.vue";
import DirectoryModal from "@/components/modal/DirectoryModal.vue";

import { onMounted, ref } from "vue";
import { useDirectoryStore } from "@/stores/modules/directoryStore";
import type { Directory } from "@/models/models";
import type { MenuItem } from "@/types/extended";

import _ from "lodash";

const currentDirectory = ref<Directory>();
const isShowModal = ref(false);
const store = useDirectoryStore();
const menuItems = ref<Array<MenuItem>>([
  { label: "Предметные области", icon: "fa-solid fa-list" },
  { label: "Уровни справочников", icon: "fa-solid fa-list-check" },
  { label: "Добавить справочник", icon: "fa-solid fa-file-circle-plus", modalType: "directory" }
]);

defineEmits(["change"]);

onMounted(async () => {
  await store.getDirectories();
});

const handleSearch = (value: string) => {
  store.searchKeyWorld(value);
};

const showModal = (modalType: string, nextDirectory: Directory): void => {
  if (modalType) {
    currentDirectory.value = _.clone(nextDirectory);
    isShowModal.value = true;
  }
};

const saveDirectory = async (directoryParams: Directory) => {
  directoryParams.id ? await store.modifyDirectory(directoryParams) : await store.createDirectory(directoryParams);
};

const deleteDirectory = async (id: string) => {
  await store.deleteDirectory(id);
};
</script>

<style scoped>
</style>
