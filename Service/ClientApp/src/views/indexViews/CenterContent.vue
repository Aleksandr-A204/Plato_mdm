<template>
  <div class="vh-100 d-flex flex-column">
    <div class="sticky-top bg-white mx-2">
      <h4 class="p-3">{{ storeDir.directoryObject.name || 'Неизвестное наименование' }}</h4>
      <SearchBar modelValue="">
        <MdmButton
          class="btn-light"
          @click="createRecord"
        >
          Добавить
        </MdmButton>
        <MdmButton
          class="btn-light"
          :disabled="!selectedIds.length"
          @click=deleteSelected
        >
          Удалить
        </MdmButton>
        <MdmButton
          class="btn-primary"
          @click="saveRecord"
        >
          Сохранить
        </MdmButton>
        <MdmButton
          class="btn-secondary"
          @click="cancelSelection"
        >
          Отмена
        </MdmButton>
      </SearchBar>
    </div>

    <div class="overflow-auto flex-fill position-relative">
      <div v-if="!route.params.varsionId"></div>
      <div v-else-if="!storeDirData.directoryData">Такой таблицы не существует в БД.</div>
      <!-- <DirectoryDataTable
        v-else
        :columns="storeDirData.columns"
        :elements="storeDirData.directoryData"
      /> -->

      <div v-else class="mx-2">
        <table class="table table-striped">
          <thead class="thead-light position-sticky">
            <tr>
              <th>
                <input
                  type="checkbox"
                  :checked="isAllSelected"
                  @change="toggleSelectAll"
                />
              </th>
              <th
                v-for="(column, index) in storeDirData.columns"
                :key="`table-head-${index}`"
              >
                {{ column }}
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(object, index) of storeDirData.directoryData"
              :key="`table-body-${index}`"
              :class="{ 'table-active': isSelected(object.InstanceId) }"
            >
              <td>
                <input
                  type="checkbox"
                  :checked="isSelected(object.InstanceId)"
                  @click="toggleSelect(object.InstanceId, $event)"
                />
              </td>
              <td
                v-for="(column, secondIndex) of storeDirData.columns"
                :key="secondIndex"
                @click="startEdit(object, column)"
              >
                <div v-if="isDowndrop(column)">
                  <div v-if="isEditing(object, column)">
                    <MdmSelect
                      :value="editValue"
                      :options="getMapRelatedData(column)"
                      @change="(value: string) => updateValue(object, column, value, 'downdrop')"
                    />
                  </div>
                  <div v-else>
                    {{ getDisplayValue(object, column, 'dropdown') }}
                  </div>
                </div>
                <div v-else>
                  <div v-if="isEditing(object, column)">
                    <MdmInput
                      :modelValue="editValue"
                      @update:modelValue="(value: string) => updateValue(object, column, value, '')"
                      @blur="saveEdit(object, column)"
                    />
                  </div>
                  <div v-else>
                    {{ getDisplayValue(object, column, '') }}
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import _ from "lodash";
import { computed, ref, watchEffect } from "vue";
import { useDirectoryDataStore } from "@/stores/modules/directoryDataStore";
import { useDirectoryStore } from "@/stores/modules/directoryStore";
import { useRoute } from "vue-router";

const editingCell = ref<{ element: any; column: string } | null>(null);
const editValue = ref<string | null>(null);
const selectedIds = ref<any[]>([]);
const selectedElements = ref<any[]>([]);
const route = useRoute();
const storeDirData = useDirectoryDataStore();
const storeDir = useDirectoryStore();

watchEffect(
  async () => {
    if (route.params.varsionId) {
      await storeDirData.getDirectoryData(route.params.varsionId);
      storeDirData.selectedElements = [];
      selectedIds.value = [];
    }
  }
);

const isAllSelected = computed(() => selectedIds.value.length === storeDirData.directoryData.length);

const createRecord = async () => {
  await storeDirData.createNewRecord(route.params.varsionId);
};

const saveRecord = async () => {
  await storeDirData.saveRecords();
};

const startEdit = async (element: any, column: string) => {
  if (!isEditing(element, column)) {
    editingCell.value = { element, column };
    editValue.value = getDisplayValue(element, column, "");

    if (!_.includes(selectedElements.value, element)) {
      selectedElements.value.push(element);
    }
  }
};

const getMapRelatedData = (property: string): any => {
  return _.map(storeDirData.directoryRelatedData[property], item => ({
    value: item.InstanceId,
    display: item.name
  }));
};

const saveEdit = (element: any, column: string) => {
  if (element) {
    storeDirData.updatedData(element, column, editValue.value);
  }
};

const updateValue = (element: any, property: string, value: string, type: string) => {
  editValue.value = value === "" ? null : value;

  if (type) {
    saveEdit(element, property);
  }
};

const deleteSelected = async () => {
  if (confirm(`Вы действительно хотите удалить одну или более записи из таблицы ${storeDirData.tablename}`)) {
    await storeDirData.deleteRecord(selectedIds.value, route.params.varsionId);
    selectedIds.value = [];
    storeDirData.selectedElements = [];
  }
};

const getDisplayValue = (object: any, property: string, type: string): string => {
  if (type) {
    const obj = _.filter(storeDirData.directoryRelatedData[property], { "InstanceId": object[property] });

    return obj[0]?.name;
  }

  return _.get(object, property);
};

const isDowndrop = (property: string): boolean => {
  return storeDirData.directoryRelatedData[property];
};

const isEditing = (element: any, column: string): boolean | null => {
  return editingCell.value && editingCell.value.element === element && editingCell.value.column === column;
};

const toggleSelect = (id: string, event: MouseEvent): void => {
  const isElement = !_.includes(selectedIds.value, id);
  event.shiftKey && selectedIds.value.length && isElement ? selectRange(id) : toggleSingleSelect(id, isElement);
};

const selectRange = (id: string): void => {
  const lastSelectedIndex = storeDirData.directoryData.findIndex(element => element.InstanceId === selectedIds.value[selectedIds.value.length - 1]);
  const currentIndex = storeDirData.directoryData.findIndex(element => element.InstanceId === id);

  if (lastSelectedIndex === -1 || currentIndex === -1) {
    return;
  }

  const start = Math.min(lastSelectedIndex, currentIndex);
  const end = Math.max(lastSelectedIndex, currentIndex);

  for (let i = start; i <= end; i++) {
    const currentId = storeDirData.directoryData[i].InstanceId;
    if (!_.includes(selectedIds.value, currentId)) {
      selectedIds.value.push(currentId);
    }
  }
};

const toggleSingleSelect = (id: string, isElement: boolean): void => {
  isElement ? selectedIds.value.push(id) : _.remove(selectedIds.value, value => value === id);
};

const toggleSelectAll = () => {
  selectedIds.value = isAllSelected.value ? [] : _.map(storeDirData.directoryData, "InstanceId");
};

const isSelected = (id: string) => _.includes(selectedIds.value, id);

const cancelSelection = () => {
  selectedIds.value = [];
  selectedElements.value = [];
};
</script>

<style scoped>
</style>
