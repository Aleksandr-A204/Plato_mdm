import { dataLoading, tryExecute } from "../dataLoading";
import { defineStore } from "pinia";

import DirectoryClient from "@/API/directoryClient";
import type { Directory } from "@/models/models";

export const useDirectoryStore = defineStore("DirectoryStore", {
  state: () => ({
    keyWord: "",
    directoryObject: {} as Directory,
    directoryList: [] as Directory[]
  }),

  getters: {
    allDirectories(state) {
      return state.directoryList;
    },

    dataLoading() {
      return dataLoading.value;
    }
  },

  actions: {
    async getDirectoryById(id: string | string[]) {
      await tryExecute(async () => {
        this.directoryObject = await DirectoryClient.getDirectoryById(id);
      });
    },

    async getDirectories() {
      await tryExecute(async () => {
        this.directoryList = await DirectoryClient.getDirectories();
      });
    },

    async createDirectory(directoryParams: Directory) {
      await tryExecute(async () => {
        await DirectoryClient.addDirectory(directoryParams);
        await this.getDirectories();
      });
    },

    async modifyDirectory(directoryParams: Directory) {
      await tryExecute(async () => {
        await DirectoryClient.ubdateDirectory(directoryParams);
        await this.getDirectories();
      });
    },

    async deleteDirectory(id: string) {
      await tryExecute(async () => {
        await DirectoryClient.deleteDirectory(id);
        await this.getDirectories();
      });
    },

    async searchKeyWorld(value: string) {
      this.keyWord = value;
      await tryExecute(async () => {
        //await DirectoryClient.getDirectories(this.keyWord);
      });
    }
  }
});
