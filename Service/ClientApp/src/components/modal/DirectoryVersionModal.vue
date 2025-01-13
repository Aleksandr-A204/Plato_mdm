<template>
  <BaseModal
    ref="modal"
    title="Параметры версии справочника"
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
      <MdmWrap custom-label="Версия">
        <MdmInput
          :modelValue="params?.version"
          @update:modelValue="(value: string) => updateParam('version', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Дата актуализации">
        <MdmInput
          placeholder="(можно не вести, автоматически появится)"
          :modelValue="params?.versionDate"
          @update:modelValue="(value: string) => updateParam('versionDate', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Источник данных">
        <MdmInput
          :modelValue="params?.dataSourceName"
          @update:modelValue="(value: string) => updateParam('dataSourceName', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Описание">
        <MdmTextarea
          :modelValue="params?.versionDescription"
          @update:modelValue="(value: string) => updateParam('versionDescription', value)"
        />
      </MdmWrap>

      <MdmWrap custom-label="Имя таблицы в БД">
        <MdmInput
          :modelValue="params?.tableName"
          @update:modelValue="(value: string) => updateParam('tableName', value)"
        />
      </MdmWrap>
    </form>
  </BaseModal>
</template>

<script setup lang="ts">
import _ from "lodash";
import BaseModal from "./BaseModal.vue";
import type { DirectoryVersion } from "@/models/models";

import { ref, watch } from "vue";

interface ModalConfirmDeleteProps {
  isShowModal: boolean;
  params: DirectoryVersion;
}

const props = defineProps<ModalConfirmDeleteProps>();
const modal = ref<InstanceType<typeof BaseModal> | null>(null);

const emit = defineEmits(["close", "save", "delete"]);

watch(() => props.isShowModal, newValue => {
  newValue ? modal.value?.open() : modal.value?.close();
});

const updateParam = (property: string, value: string) => {
  if (props.params) {
    _.set(props.params, property, value);
  }
};

const confirmDelete = () => {
  if (confirm("Вы действительно хотите удалить " + props.params.version)) {
    emit("delete", props.params.id, props.params.directoryId);
  }
};
</script>
