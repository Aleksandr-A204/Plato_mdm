<template>
  <div>
    <Splitpanes>
      <Pane min-size="18" max-size="23">
        <LeftContent />
      </Pane>
      <Pane>
        <CenterContent />
      </Pane>
      <Pane min-size="15" max-size="20">
        <RightContent />
      </Pane>
    </Splitpanes>
  </div>
</template>

<script setup lang="ts">
import CenterContent from "@/views/indexViews/CenterContent.vue";
import LeftContent from "@/views/indexViews/LeftContent.vue";
import RightContent from "@/views/indexViews/RightContent.vue";

import { useDirectoryStore } from "@/stores/modules/directoryStore";

import { Pane, Splitpanes } from "splitpanes";
import { useRoute } from "vue-router";
import { watchEffect } from "vue";

const route = useRoute();
const store = useDirectoryStore();

watchEffect(
  async () => {
    if (route.params.id) {
      await store.getDirectoryById(route.params.id);
    }
  }
);
</script>

<style scoped>
/* Add any required styles here */
</style>
