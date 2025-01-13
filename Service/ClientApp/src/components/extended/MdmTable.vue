<template>
  <div>
    <button class="btn btn-danger" @click="deleteSelected" :disabled="!selectedElements.length">Удалить выделенные</button>
    <table class="table table-striped">
      <thead>
        <tr>
          <th>
            <input type="checkbox" @change="toggleSelectAll" :checked="isAllSelected" />
          </th>
          <th
            v-for="(column, index) in columns"
            :key="`table-head-${index}`"
          >
            {{ column }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(element, index) in elements"
          :key="`table-body-${index}`"
          :class="{ 'table-active': true }"
          @click="toggleSelect(element, $event)"
        >
          <td>
            <input type="checkbox" :checked="isSelected(element)" />
          </td>
          <td
            v-for="(column, secondIndex) in columns"
            :key="secondIndex"
            @click="editCell(element, column)"
          >
            <template v-if="isEditing(element, column)">
              <input v-model="editValue" class="form-control" @blur="saveEdit(element, column)" />
            </template>
            <template v-else>
              {{ getProperty(element, column) }}
            </template>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import _ from "lodash";
import { computed, ref } from "vue";

const props = withDefaults(defineProps<{
  columns: string[];
  elements: any[];
}>(), {});

const editingCell = ref<{ element: any; column: string } | null>(null);
const editValue = ref("");
const selectedElements = ref<any[]>([]);

const getProperty = (element: any, property: string): string => {
  const prop = _.find(element.properties, { propName: property });
  return prop ? prop.propValue : "";
};

const isEditing = (element: any, column: string): boolean | null => {
  return editingCell.value && editingCell.value.element === element && editingCell.value.column === column;
};

const editCell = (element: any, column: string) => {
  editingCell.value = { element, column };
  editValue.value = getProperty(element, column);
};

const saveEdit = (element: any, column: string) => {
  const prop = _.find(element.properties, { propName: column });
  if (prop) {
    prop.propValue = editValue.value;
  }
  editingCell.value = null;
};

const toggleSelect = (element: any, event: MouseEvent) => {
  const isShift = event.shiftKey;
  if (isShift && selectedElements.value.length) {
    const lastSelectedIndex = props.elements.indexOf(selectedElements.value[selectedElements.value.length - 1]);
    const currentIndex = props.elements.indexOf(element);
    const start = Math.min(lastSelectedIndex, currentIndex);
    const end = Math.max(lastSelectedIndex, currentIndex);

    for (let i = start; i <= end; i++) {
      if (!selectedElements.value.includes(props.elements[i])) {
        selectedElements.value.push(props.elements[i]);
      }
    }
  }
  else {
    const index = selectedElements.value.indexOf(element);
    if (index > -1) {
      selectedElements.value.splice(index, 1);
    }
    else {
      selectedElements.value.push(element);
    }
  }
};

const isSelected = (element: any) => {
  console.log(element);
  return selectedElements.value.includes(element);
};

const toggleSelectAll = () => {
  if (isAllSelected.value) {
    selectedElements.value = [];
  }
  else {
    console.log(props.elements);
    selectedElements.value = _.clone(props.elements);
  }
};

const isAllSelected = computed(() => {
  return selectedElements.value.length === props.elements.length;
});

const deleteSelected = () => {
  console.log(selectedElements.value);
  // props.elements = props.elements.filter(element => !selectedElements.value.includes(element));
  // selectedElements.value = []; // Сбросить выделенные строки
};

</script>

<style scoped>
.table-active {
  background-color: #f0f8ff; /* Цвет выделенной строки */
}
</style>
