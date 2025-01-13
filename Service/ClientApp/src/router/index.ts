import About from "@/views/AboutView.vue";
import DirectoryData from "@/components/DirectoryDataGrid.vue";
import IndexView from "@/views/IndexView.vue";
import NotFound from "@/views/NotFoundView.vue";
import Version from "@/components/DirectoryVersionList.vue";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/",
    meta: { layout: "index" },
    component: IndexView
  },
  {
    path: "/index",
    redirect: "/"
  },
  {
    path: "/directory/:id",
    name: "directory",
    meta: { layout: "index" },
    component: Version,
    children: [{
      path: "data/:varsionId",
      name: "data",
      component: DirectoryData
    }]
  },
  {
    path: "/about",
    meta: { layout: "index" },
    component: About
  },
  {
    path: "/404",
    meta: { layout: "not-found" },
    component: NotFound
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: "/404"
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
