<script setup lang="ts">
import { ref } from "vue";
import { ctx } from "../main";
import { Ref } from "vue";
import { fetchItems } from "../fetchers";
import { formatDateTimeShort } from "../helpers";

const selectedItem = ref(null) as Ref<any>;
const saveLoading = ref(false);
const items = ref([]) as Ref<any[]>;

const fetchItemsData = async () => {
  const d = await fetchItems({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    items.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(`${window.location.origin}/api/items-save-bulk`, {
      method: "post",
      headers: {
        "content-type": "application/json",
        authorization: ctx.value.apiKey ?? "",
      },
      body: JSON.stringify([selectedItem.value]),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    selectedItem.value = null;
    fetchItemsData();
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchItemsData();
</script>
<template>
  <dialog
    class="bg-light"
    style="z-index: 100; width: 75vw"
    :open="selectedItem"
  >
    <div>
      <div>
        <h4>Item Detail ({{ selectedItem?.id }})</h4>
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
        :value="selectedItem?.name"
        @blur="(e) => {
          selectedItem.name = (e.target as HTMLInputElement).value
        }"
      />
    </div>
    <div>
      <small><strong>Description</strong></small>
    </div>
    <div class="d-flex">
      <textarea
        class="form-control form-control-sm"
        placeholder="Description..."
        :value="selectedItem?.description"
        @blur="(e) => {
          selectedItem.description = (e.target as HTMLInputElement).value
        }"
      />
    </div>
    <div>
      <small><strong>Price</strong></small>
    </div>
    <div class="d-flex">
      <input
        class="form-control form-control-sm"
        placeholder="Price..."
        :value="selectedItem?.price"
        @blur="
          (e) => {
            console.log(e);
            const price = isNaN(parseFloat((e.target as HTMLInputElement).value ))
              ? 0
              : parseFloat((e.target as HTMLInputElement).value )
            
            selectedItem.price=price
          }
        "
      />
    </div>

    <div class="d-flex justify-content-end">
      <template v-if="!saveLoading">
        <button
          class="btn btn-sm btn-danger px-1 py-0"
          @click="
            () => {
              selectedItem = null;
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
      <div><h4>Items</h4></div>
      <div class="mx-3">
        <button
          class="px-1 py-0 btn btn-sm btn-primary"
          @click="
            () => {
              selectedItem = {};
            }
          "
        >
          <VIcon class="text-light" icon="mdi-plus" /> Add
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
            'Item Name',
            'Description',
            'Price',
            'Created',
            'Updated',
            'Action',
          ]"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
      <tr v-for="(i, i_) of items">
        <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.id }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.name }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.description }}</td>
        <td class="border border-dark p-0 m-0">{{ i?.price }}</td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(i?.createdAt) }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(i?.updatedAt) }}
        </td>
        <td class="border border-dark p-0 m-0">
          <div>
            <button
              @click="
                () => {
                  selectedItem = i;
                }
              "
              class="btn btn-sm btn-primary px-1 py-0"
            >
              <VIcon icon="mdi-note-edit"></VIcon>
            </button>
          </div>
        </td>
      </tr>
    </table>
  </div>
</template>
