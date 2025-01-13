<template>
  <Multiselect
    :value="selected"
    :object="object"
    trackBy="display"
    label="display"
    :mode="multiple ? 'tags' : 'single'"
    :options="options"
    :searchable="true"
    :limit="optionsLimit"
    :canDeselect="allowEmpty"
    :canClear="false"
    :placeholder="placeholder"
    @change="select"
  >
    <template #singlelabel="slotData">
      <slot
        name="singlelabel"
        v-bind="slotData"
      />
    </template>

    <template #nooptions>
      <div class="template">
        Ничего нет
      </div>
    </template>

    <template #noresults>
      <div class="template">
        Ничего не найдено
      </div>
    </template>

    <template #option="slotData">
      <slot
        name="option"
        v-bind="slotData"
      />
    </template>
  </Multiselect>
</template>

<script setup lang="ts">
import Multiselect from "@vueform/multiselect";
import { computed } from "vue";

import _ from "lodash";

import type { Option, SelectEvent } from "@/types/extended";

const emit = defineEmits(["change", "blur"]);

const props = withDefaults(defineProps<{
  value: string | Array<any> | null;
  multiple?: boolean;
  allowEmpty?: boolean;
  showAfter?: boolean;
  optionsLimit?: number;
  object?: boolean;
  placeholder?: string;
  options: Array<Option>;
}>(),
{
  value: null,
  multiple: false,
  allowEmpty: true,
  showAfter: false,
  optionsLimit: 1000,
  object: true,
  placeholder: "(нет данных)"
});

const selected = computed(() => {
  if (props.object) {
    if (props.multiple) {
      return _.map(props.value, v => _.find(props.options, { value: v }));
    }

    return _.find(props.options, { value: props.value });
  }

  return props.value;
});

const select = (event: SelectEvent) => {
  if (props.object) {
    if (props.multiple) {
      let data = _.map(event, v => _.get(v, "value"));
      emit("change", data);
    }
    else {
      let data = _.get(event, "value");
      emit("change", data);
    }

    return;
  }

  emit("change", event);
};
</script>

<style src="@vueform/multiselect/themes/default.css"></style>
<style lang="scss">

.multiselect {
  border: var(--bs-border-width) solid var(--bs-border-color);
  background-color: var(--bs-body-bg);
  border-radius: var(--bs-border-radius);

  &.is-active {
    box-shadow: 0 0 0 .25rem rgba(13, 110, 253, .25);
    border-color: #86b7fe;

    .multiselect-search {
      cursor: auto;
    }
  }
}

.multiselect-search {
  padding: 6px 12px;
}

.multiselect-placeholder {
    color: #595c5f;
}
</style>
