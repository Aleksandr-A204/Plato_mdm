<template>
  <div class="vh-100 d-flex flex-column">
    <div class="sticky-top bg-white mx-2">
      <DropdownMenu
        title="Версия справочника"
        :items="menuItems"
        @show-modal="showModal({})"
      />
    </div>

    <div class="overflow-auto flex-fill">
      <DirectoryVersionList
        :versions=storeDirVersion.allVersionsByDirectory
        @hindle-version="showModal"
      />
    </div>

    <DirectoryVersionModal
      :params="currentVersion || {}"
      :is-show-modal="isShowModal"
      @close="isShowModal = false"
      @save="saveVersion"
      @delete="deleteVersion"
    />
  </div>
</template>

<script setup lang="ts">
import DirectoryVersionList from "@/components/DirectoryVersionList.vue";
import DirectoryVersionModal from "@/components/modal/DirectoryVersionModal.vue";
import { ref, watchEffect } from "vue";
import { useDirectoryVersionStore } from "@/stores/modules/directoryVersionStore";
import { useRoute } from "vue-router";
import type { MenuItem } from "@/types/extended";

import type { DirectoryVersion } from "@/models/models";

const menuItems = ref<Array<MenuItem>>([{ label: "Добавить версию", icon: "fa-solid fa-file-circle-plus" }]);
const isShowModal = ref(false);
const storeDirVersion = useDirectoryVersionStore();
const route = useRoute();
const currentVersion = ref<DirectoryVersion>();

const showModal = (nextVersion: DirectoryVersion): void => {
  currentVersion.value = nextVersion;
  isShowModal.value = true;
};

watchEffect(
  async () => {
    if (route.params.id) {
      await storeDirVersion.getVersionsByDirectory(route.params.id);
    }
  }
);

const saveVersion = async (versionParams: DirectoryVersion) => {
  versionParams.directoryId = route.params.id;
  versionParams.id ? await storeDirVersion.modifyVersion(versionParams) : await storeDirVersion.createVersion(versionParams);
};

const deleteVersion = async (id: string, directoryId: string): Promise<void> => {
  await storeDirVersion.deleteVersion(id, directoryId);
};
</script>

<style scoped>
</style>
