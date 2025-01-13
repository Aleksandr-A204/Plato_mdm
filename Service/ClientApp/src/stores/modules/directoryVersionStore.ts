import { dataLoading, tryExecute } from "../dataLoading";
import { defineStore } from "pinia";

import DirectoryVersionClient from "@/API/directoryVersionClient";
import type { DirectoryVersion } from "@/models/models";

export const useDirectoryVersionStore = defineStore("DirectoryVersionStore", {
  state: () => ({
    versions: [] as DirectoryVersion[],
    versionsByDirectory: [] as DirectoryVersion[]
  }),

  getters: {
    allVersions: state => state.versions,
    allVersionsByDirectory: state => state.versionsByDirectory,
    dataLoading: () => dataLoading.value
  },

  actions: {
    async getVersions() {
      await tryExecute(async () => {
        this.versions = await DirectoryVersionClient.getAllVersions();
      });
    },

    async getVersionsByDirectory(id: string | string[]) {
      await tryExecute(async () => {
        this.versionsByDirectory = await DirectoryVersionClient.getAllVersionsByDirectory(id);
      });
    },

    async createVersion(versionParams: DirectoryVersion) {
      await tryExecute(async () => {
        await DirectoryVersionClient.addVersion(versionParams);
        if (versionParams.directoryId) {
          await this.getVersionsByDirectory(versionParams.directoryId);
        }
      });
    },

    async modifyVersion(versionParams: DirectoryVersion) {
      await tryExecute(async () => {
        await DirectoryVersionClient.ubdateVersion(versionParams);
        if (versionParams.directoryId) {
          await this.getVersionsByDirectory(versionParams.directoryId);
        }
      });
    },

    async deleteVersion(id: string, directoryId: string): Promise<void> {
      await tryExecute(async () => {
        if (id) {
          await DirectoryVersionClient.deleteVersion(id);
        }
        if (directoryId) {
          await this.getVersionsByDirectory(directoryId);
        }
      });
    }
  }
});
