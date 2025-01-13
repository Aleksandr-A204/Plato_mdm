import { ref } from "vue";

export const dataLoading = ref(false);

export const tryExecute = async (func: Function) => {
  dataLoading.value = true;
  await func().finally(()=> {
    dataLoading.value = false;
  });
};
