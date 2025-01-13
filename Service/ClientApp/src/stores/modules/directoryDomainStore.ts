import { dataLoading, tryExecute } from "../dataLoading";
import { defineStore } from "pinia";

import DomainClient from "@/API/directoryDomainClient";
// import type { DirectoryDomain } from "@/models/models";

import _ from "lodash";
//import type { Option } from "@/types/extended";

export const useDirectoryDomainStore = defineStore("DirectoryDomainStore", {
  state: () => ({
    domainsOption: [] as any[]
  }),

  getters: {
    allDomains(state) {
      return state.domainsOption;
    },

    dataLoading() {
      return dataLoading.value;
    }
  },

  actions: {
    async getDomains() {
      await tryExecute(async () => {
        this.domainsOption = _.map(await DomainClient.getAllDomains(), ({ id, name }) => ({
          value: id,
          display: name
        }));
      });
    }
  }
});
