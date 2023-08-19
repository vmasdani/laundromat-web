<script setup lang="ts">
import { ref } from "vue";
import { fetchAdminConfig, fetchStores } from "../fetchers";
import { ctx } from "../main";
import { Ref } from "vue";
import VueSelect from "vue-select";

// const selectedStore = ref(null) as Ref<any>;
const saveLoading = ref(false);
const stores = ref([]) as Ref<any[]>;
const adminConfig = ref(null) as Ref<any>;

const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;
  }
};

const fetchAdminConfigData = async () => {
  const d = await fetchAdminConfig({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    adminConfig.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(`${window.location.origin}/api/adminconfig`, {
      method: "post",
      headers: {
        "content-type": "application/json",
        authorization: ctx.value.apiKey ?? "",
      },
      body: JSON.stringify(adminConfig.value),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    alert("Save settings success.");

    fetchAdminConfigData();
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchAdminConfigData();
fetchStoresData();
</script>
<template>
  <div class="container">
    <div class="d-flex">
      <div><h4>Admin Config</h4></div>
      <div>
        <button
          class="btn btn-sm btn-primary"
          @click="
            () => {
              handleSave();
            }
          "
        >
          Save
        </button>
      </div>
    </div>

    <div>
      <small><strong>Default Store</strong></small>
    </div>
    <div>
      <VueSelect
        placeholder="Select store..."
        :options="stores"
        :getOptionLabel="(s: any) => `${s?.name}`"
        @update:modelValue="(s: any) => {
          adminConfig.defaultStoreId = s?.id;
          adminConfig.defaultStore = s;
          
        }"
        :modelValue="
          stores.find((s) => `${s?.id}` === `${adminConfig.defaultStoreId}`)
        "
      />
    </div>
  </div>
</template>
