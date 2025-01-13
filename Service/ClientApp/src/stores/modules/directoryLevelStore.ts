import { dataLoading, tryExecute } from "../dataLoading";
import { defineStore } from "pinia";

import LevelClient from "@/API/directoryLevelClient";

import _ from "lodash";
//import type { Option } from "@/types/extended";

export const useDirectoryLevelStore = defineStore("DirectoryLevelStore", {
  state: () => ({
    levelsOption: [] as any[]
  }),

  getters: {
    allLevels(state) {
      return state.levelsOption;
    },

    dataLoading() {
      return dataLoading.value;
    }
  },

  actions: {
    async getLevels() {
      await tryExecute(async () => {
        this.levelsOption = _.map(await LevelClient.getAllLevels(), ({ id, name }) => ({
          value: id,
          display: name
        }));
      });
    }
  }
});
