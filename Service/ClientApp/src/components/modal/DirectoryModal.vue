<template>
  <BaseModal
    ref="modal"
    title="Параметры справочника"
    :onConfirm="confirmDelete"
    @close="$emit('close')"
  >
    <template v-slot:header>
      <div class="input-group">
        <MdmButton
          class="btn-primary"
          data-bs-dismiss="modal"
          @click="$emit('save', params)"
        >
          Сохранить
        </MdmButton>

        <MdmButton
          class="btn-secondary"
          data-bs-dismiss="modal"
        >
          Отмена
        </MdmButton>

        <MdmButton
          v-if="params.id"
          class="btn-secondary"
          data-bs-toggle="dropdown"
          aria-expanded="false"
        >
          <i class="fa-solid fa-ellipsis-vertical"></i>
          <ul class="dropdown-menu">
            <li>
              <MdmButton
                class="dropdown-item btn-primary"
                data-bs-dismiss="modal"
                @click="confirmDelete"
              >
                <i class="fa-solid fa-file-circle-minus"></i>
                <label class="mx-1">Удалить справочник</label>
              </MdmButton>
            </li>
          </ul>
        </MdmButton>
      </div>
    </template>
    <form>
      <MdmWrap custom-label="Наименование">
        <MdmInput
          :modelValue="params?.name"
          @update:modelValue="(value: string) => updateParam('name', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Предметная область">
        <MdmSelect
          :value="params?.directoryDomainId"
          :options="domainStore.allDomains"
          @change="(value: string) => updateParam('directoryDomainId', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Уровень">
        <MdmSelect
          :value="params?.directoryLevelId"
          :options="levelStore.allLevels"
          @change="(value: string) => updateParam('directoryLevelId', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Описание">
        <MdmTextarea
          :modelValue="params?.description"
          @update:modelValue="(value: string) => updateParam('description', value)"
        />
      </MdmWrap>
    </form>
  </BaseModal>
</template>

<script setup lang="ts">
import _ from "lodash";
import BaseModal from "./BaseModal.vue";
import { useDirectoryDomainStore } from "@/stores/modules/directoryDomainStore";
import { useDirectoryLevelStore } from "@/stores/modules/directoryLevelStore";
import type { Directory } from "@/models/models";

import { onMounted, ref, watch } from "vue";

const domainStore = useDirectoryDomainStore();
const levelStore = useDirectoryLevelStore();

interface ModalConfirmDeleteProps {
  isShowModal: boolean;
  params: Directory;
}

const props = defineProps<ModalConfirmDeleteProps>();
const modal = ref<InstanceType<typeof BaseModal> | null>(null);

onMounted(async () => {
  await Promise.all([domainStore.getDomains(), levelStore.getLevels()]);
});

const emit = defineEmits(["close", "save", "delete"]);

watch(() => props.isShowModal, newValue => {
  newValue ? modal.value?.open() : modal.value?.close();
});

const confirmDelete = () => {
  if (confirm("Вы действительно хотите удалить " + props.params.name)) {
    emit("delete", props.params.id);
  }
};

const updateParam = (property: string, value: string) => {
  if (props.params) {
    _.set(props.params, property, value);
  }
};
</script>
