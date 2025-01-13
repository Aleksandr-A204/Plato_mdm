import type { App } from "vue";

import DropdownMenu from "../DropdownMenu.vue";
import MdmButton from "./MdmButton.vue";
import MdmInput from "./MdmInput.vue";
import MdmSelect from "./MdmSelect.vue";
import MdmTable from "./MdmTable.vue";
import MdmTextarea from "./MdmTextarea.vue";
import MdmWrap from "./MdmWrap.vue";
import SearchBar from "./SearchBar.vue";

const components = {
  DropdownMenu,
  MdmButton,
  MdmInput,
  MdmSelect,
  MdmTable,
  MdmTextarea,
  MdmWrap,
  SearchBar
};

const install = (app: App): void => {
  Object.entries(components).forEach(([key, component]) => {
    app.component(key, component);
  });
};

export default { install };
