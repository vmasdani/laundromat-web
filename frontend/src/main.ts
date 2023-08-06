import { createApp, ref } from "vue";
// import './style.css'
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "bootstrap/dist/css/bootstrap.min.css";
import App from "./App.vue";
import { createRouter, createWebHashHistory } from "vue-router";
import "@mdi/font/css/materialdesignicons.css"; // Ensure you are using css-loader
import "vue-select/dist/vue-select.css";

import HomeVue from "./components/Home.vue";
import EmployeesVue from "./components/Employees.vue";
import ItemsVue from "./components/Items.vue";
import CustomersVue from "./components/Customers.vue";
import StoresVue from "./components/Stores.vue";
import InventoryVue from "./components/Inventory.vue";

import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import { createVuetify } from "vuetify";
import { aliases, mdi } from "vuetify/iconsets/mdi";
import DropOffDetailVue from "./components/DropOffDetail.vue";
import ShopPriceSettingVue from "./components/ShopPriceSetting.vue";

export const ctx = ref({ apiKey: null as string | null });

const vuetify = createVuetify({
  components,
  directives,
  icons: {
    defaultSet: "mdi", // This is already the default value - only for display purposes
    aliases,
    sets: {
      mdi,
    },
  },
});
const routes = [
  { path: "/", component: HomeVue },
  { path: "/items", component: ItemsVue },
  { path: "/employees", component: EmployeesVue },
  { path: "/customers", component: CustomersVue },
  { path: "/stores", component: StoresVue },
  { path: "/inventory", component: InventoryVue },
  { path: "/dropoffs/:id", component: DropOffDetailVue },
  { path: "/shopsettings", component: ShopPriceSettingVue },
];

const router = createRouter({
  // 4. Provide the history implementation to use. We are using the hash history for simplicity here.
  history: createWebHashHistory(),
  routes, // short for `routes: routes`
});
const app = createApp(App);

app.use(router);
app.use(vuetify);

app.mount("#app");
