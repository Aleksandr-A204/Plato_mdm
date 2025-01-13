import _ from "lodash";

import { dataLoading, tryExecute } from "../dataLoading";
import { defineStore } from "pinia";

import DirectoryDataClient from "@/API/directoryDataClient";
import type { MdmTablesData } from "@/models/models";

export const useDirectoryDataStore = defineStore("DirectoryDataStore", {
  state: () => ({
    data: {} as MdmTablesData,
    selectedElements: [] as any[]
  }),

  getters: {
    tablename(state): string {
      return state.data.tableName;
    },

    directoryData(state): any[] {
      return state.data.mainTable || [];
    },

    directoryRelatedData(state): any {
      return state.data.foreignTables || {};
    },

    columns(state): Array<string> {
      const arr = state.data.mainTable && state.data.mainTable.length > 0 ? Object.keys(this.data.mainTable[0]) : [];

      return arr.length ? arr.sort(a => a === "InstanceId" ? -1 : 1) : [];
    },

    dataLoading() {
      return dataLoading.value;
    }
  },

  actions: {
    async getDirectoryData(id: string | string[]) {
      await tryExecute(async () => {
        const response = await DirectoryDataClient.getData(id);
        this.data = response.status === 200 ? response.data : null;
      });
    },

    async createNewRecord(id: string | string[]) {
      await tryExecute(async () => {
        await DirectoryDataClient.createRecord(this.tablename);
      });
      await this.getDirectoryData(id);
    },

    async saveRecords() {
      await tryExecute(async () => {
        const obj = {};
        _.set(obj, this.tablename, this.selectedElements);
        await DirectoryDataClient.updateRecords(obj);
        this.selectedElements = [];
      });
    },

    async deleteRecord(deleteDataByIds: any[], id: string | string[]) {
      await tryExecute(async () => {
        await DirectoryDataClient.deleteRecord({
          tablename: this.tablename,
          ids: deleteDataByIds
        });
      });
      await this.getDirectoryData(id);
    },

    updatedData(element: any, property: string, value: string | null) {
      if (!_.includes(this.selectedElements, element)) {
        this.selectedElements.push(element);
      }

      element[property] = value;
    }
  }
});




// export const useDirectoryDataStore = defineStore("DirectoryDataStore", {
//   state: () => ({
//     tblname: ""
//   }),
