<script setup lang="ts">
import { ref } from "vue";
import {
  fetchInventory,
  fetchInventorySummary,
  fetchItems,
  fetchStores,
} from "../fetchers";
import { Ref } from "vue";
import { ctx } from "../main";
import VueSelect from "vue-select";
import { formatDateTimeShort } from "../helpers";

type InventoryMode = "Detail" | "Summary";
const inventoryModes: InventoryMode[] = ["Detail", "Summary"];

const detailOpen = ref(false);
const items = ref([]) as Ref<any[]>;
const stores = ref([]) as Ref<any[]>;
const inventoryList = ref([]) as Ref<any[]>;
const inventory = ref({}) as Ref<any>;

const inventorySummary = ref({}) as Ref<any>;
const inventoryMode = ref("Detail" as InventoryMode);
const saveLoading = ref(false);

const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;
  }
};
const fetchItemsData = async () => {
  const d = await fetchItems({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    items.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(
      `${window.location.origin}/api/inventory-save-bulk`,
      {
        method: "post",
        headers: {
          "content-type": "application/json",
          authorization: ctx.value.apiKey ?? "",
        },
        body: JSON.stringify([inventory.value]),
      }
    );

    if (resp.status !== 200) {
      throw await resp.text();
    }

    fetchInventoryData();
    fetchInventorySummaryData();
    detailOpen.value = false;
    inventory.value = null;
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

const fetchInventorySummaryData = async () => {
  const d = await fetchInventorySummary({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    inventorySummary.value = d;
  }
};

const fetchInventoryData = async () => {
  const d = await fetchInventory({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    inventoryList.value = d;
  }
};

fetchStoresData();
fetchItemsData();
fetchInventoryData();
fetchInventorySummaryData();
</script>
<template>
  <dialog class="bg-light" style="z-index: 100; width: 75vw" :open="detailOpen">
    <div>
      <div><h4>Inventory Detail</h4></div>
    </div>
    <div><hr class="border border-dark" /></div>
    <div>
      <small><strong>Item</strong></small>
    </div>
    <div>
      <VueSelect
        :options="items"
        :getOptionLabel="(i: any) => `#${i?.id}: ${i?.name}`"
        @update:modelValue="(i: any) => {
          inventory.itemId = i?.id;
        }"
        :modelValue="items.find((i) => `${i?.id}` === `${inventory?.itemId}`)"
      />
    </div>

    <div>
      <small><strong>Store</strong></small>
    </div>
    <div>
      <VueSelect
        :options="stores"
        :getOptionLabel="(s: any) => `#${s?.id}: ${s?.name}`"
        @update:modelValue="(s: any) => {
          inventory.storeId = s?.id;
        }"
        :modelValue="stores.find((s) => `${s?.id}` === `${inventory?.storeId}`)"
      />
    </div>

    <div>
      <small><strong>Qty</strong></small>
    </div>
    <div>
      <input
        class="form-control form-control-sm"
        placeholder="Qty..."
        @input="(e) => {
        if(isNaN(parseFloat((e.target as HTMLInputElement).value))){
          return
        }

        inventory.qty = parseFloat((e.target as HTMLInputElement).value)
      }"
        :value="inventory?.qty"
      />
    </div>

    <div>
      <small><strong>Mode</strong></small>
    </div>
    <div>
      <VueSelect
        :options="['IN', 'OUT']"
        :getOptionLabel="(i: any) => `${i}`"
        @update:modelValue="(i: any) => {
          inventory.mode = i;
        }"
        :modelValue="['IN', 'OUT'].find((i) => `${i}` === `${inventory?.mode}`)"
      />
    </div>

    <div v-if="!saveLoading" class="d-flex justify-content-end">
      <button
        class="btn btn-sm btn-danger px-1 py-0"
        @click="
          () => {
            detailOpen = false;
          }
        "
      >
        Cancel
      </button>
      <button
        class="mx-2 btn btn-sm btn-primary px-1 py-0"
        @click="
          () => {
            handleSave();
          }
        "
      >
        Save
      </button>
    </div>
    <div v-else><div class="spinner-border spinner-border-sm"></div></div>
  </dialog>
  <div>
    <div class="d-flex align-items-center">
      <div><h4>Inventory</h4></div>
      <div class="mx-3">
        <button
          class="px-1 py-0 btn btn-sm btn-primary"
          @click="
            () => {
              detailOpen = !detailOpen;
            }
          "
        >
          <VIcon class="text-light" icon="mdi-plus" /> Add
        </button>
      </div>
    </div>
  </div>

  <div class="d-flex">
    <div class="mx-2" v-for="h in inventoryModes">
      <div>
        <button
          :class="`px-1 py-0 btn btn-sm ${
            inventoryMode === h ? `btn-primary` : `btn-outline-primary`
          }`"
          @click="
            () => {
              inventoryMode = h;
            }
          "
        >
          {{ h }}
        </button>
      </div>
    </div>
  </div>

  <hr class="border border-dark" />

  <div
    v-if="inventoryMode === 'Detail'"
    class="border border-dark shadow shadow-md overflow-auto"
    style="height: 75vh; resize: vertical"
  >
    <table class="table table-sm" style="border-collapse: separate">
      <tr>
        <th
          class="bg-dark text-light p-0 m-0"
          v-for="h in ['#', 'Item', 'Store', 'Mode', 'Qty', 'Created']"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
      <tr v-for="(i, i_) in inventoryList">
        <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
        <td class="border border-dark p-0 m-0">
          #{{ i?.item?.id }}: {{ i?.item?.name }}
        </td>
        <td class="border border-dark p-0 m-0">{{ i?.store?.name }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.mode }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.qty }}</td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(i?.createdAt ?? "") }}
        </td>
      </tr>
    </table>
  </div>

  <div
    v-if="inventoryMode === 'Summary'"
    class="border border-dark shadow shadow-md overflow-auto"
    style="height: 75vh; resize: vertical"
  >
    <table class="table table-sm" style="border-collapse: separate">
      <tr>
        <th
          class="bg-dark text-light p-0 m-0"
          v-for="h in ['#', 'Item', 'Store', 'In Stock']"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
      <tr v-for="(i, i_) in inventorySummary">
        <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
        <td class="border border-dark p-0 m-0">
          #{{ i?.item?.id }}: {{ i?.item?.name }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ i?.store?.name }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ i?.qty }}
        </td>
      </tr>
    </table>
  </div>
</template>
