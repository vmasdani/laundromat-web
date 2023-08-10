<script setup lang="ts">
import { ref } from "vue";
import { fetchStores } from "../fetchers";
import { ctx } from "../main";
import { Ref } from "vue";
import { formatDateTimeShort } from "../helpers";

const selectedStore = ref(null) as Ref<any>;
const saveLoading = ref(false);
const stores = ref([]) as Ref<any[]>;

const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(`${window.location.origin}/api/stores-save-bulk`, {
      method: "post",
      headers: {
        "content-type": "application/json",
        authorization: ctx.value.apiKey ?? "",
      },
      body: JSON.stringify(stores.value),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    alert("Save settings success.");

    selectedStore.value = null;
    fetchStoresData();
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchStoresData();
</script>
<template>
  <dialog
    class="bg-light"
    style="z-index: 100; width: 75vw"
    :open="selectedStore"
  >
    <div>
      <div>
        <h4>Store Detail ({{ selectedStore?.id }})</h4>
      </div>
    </div>
    <div><hr class="border border-dark" /></div>
    <div>
      <small><strong>Name</strong></small>
    </div>
    <div class="d-flex">
      <input
        class="form-control form-control-sm"
        placeholder="Name..."
        :value="selectedStore?.name"
        @blur="(e) => {
          selectedStore.name = (e.target as HTMLInputElement).value
        }"
      />
    </div>
    <div>
      <small><strong>Address</strong></small>
    </div>
    <div class="d-flex">
      <textarea
        class="form-control form-control-sm"
        placeholder="Address..."
        :value="selectedStore?.address"
        @blur="(e) => {
          selectedStore.address = (e.target as HTMLInputElement).value
        }"
      />
    </div>

    <div class="d-flex justify-content-end">
      <template v-if="!saveLoading">
        <button
          class="btn btn-sm btn-danger px-1 py-0"
          @click="
            () => {
              selectedStore = null;
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
      </template>

      <div v-else>
        <div class="spinner-border spinner-border-sm" />
      </div>
    </div>
  </dialog>
  <div>
    <div class="d-flex align-items-center">
      <div><h4>Stores</h4></div>
      <div class="mx-3">
        <button
          class="px-1 py-0 btn btn-sm btn-primary"
          @click="
            () => {
              handleSave();
            }
          "
        >
          <VIcon class="text-light" icon="mdi-content-save-all" /> Save
        </button>
      </div>
    </div>
  </div>

  <hr class="border border-dark" />

  <div
    class="border border-dark shadow shadow-md overflow-auto"
    style="height: 75vh; resize: vertical"
  >
    <table class="table table-sm" style="border-collapse: separate">
      <tr>
        <th
          class="bg-dark text-light p-0 m-0"
          v-for="h in [
            '#',
            'ID',
            'Name',
            'Address',
            'Price/Weight Unit',
            'Min DropOff Weight',
            'Created',
            'Updated',
          ]"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
      <tr v-for="(s, i_) of stores">
        <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
        <td class="border border-dark p-0 m-0">{{ s?.id }}</td>
        <td class="border border-dark p-0 m-0">{{ s?.name }}</td>
        <td class="border border-dark p-0 m-0">{{ s?.address }}</td>
        <td class="border border-dark p-0 m-0">
          <input
            class="form-control form-control-sm"
            style="width: 75px"
            :value="s.pricePerWeight"
            placeholder="Price..."
            @input="(e) => {
                const v = isNaN(parseFloat((e.target as HTMLInputElement).value)) 
                    ? 0
                    : parseFloat((e.target as HTMLInputElement).value)

                s.pricePerWeight = v
            }"
          />
        </td>
        <td class="border border-dark p-0 m-0">
          <input
            class="form-control form-control-sm"
            style="width: 75px"
            :value="s.minimumDropOffWeight"
            placeholder="minimumDropOffWeight..."
            @input="(e) => {
                const v = isNaN(parseFloat((e.target as HTMLInputElement).value)) 
                    ? 0
                    : parseFloat((e.target as HTMLInputElement).value)

                s.minimumDropOffWeight = v
            }"
          />
        </td>

        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(s?.createdAt) }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(s?.updatedAt) }}
        </td>

        <!-- <td class="border border-dark p-0 m-0">
          <div>
            <button
              @click="
                () => {
                  selectedStore = s;
                }
              "
              class="btn btn-sm btn-primary px-1 py-0"
            >
              <VIcon icon="mdi-note-edit"></VIcon>
            </button>
          </div>
        </td> -->
      </tr>
    </table>
  </div>
</template>
