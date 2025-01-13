import App from "./App.vue";
import { createApp } from "vue";
import { createPinia } from "pinia";

import "./style.css";

import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";

import "./assets/scss/fonts.scss";
import "splitpanes/dist/splitpanes.css";

import components from "@/components/extended/components.js";
import router from "./router";

const pinia = createPinia();
const app = createApp(App);

app.use(router);
app.use(pinia);
app.use(components);
app.mount("#app");
