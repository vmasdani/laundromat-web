<script setup lang="ts">
import { ref } from "vue";
import { fetchCustomers } from "../fetchers";
import { ctx } from "../main";
import { Ref } from "vue";
import { formatDateTimeShort } from "../helpers";

const selectedCustomer = ref(null) as Ref<any>;
const saveLoading = ref(false);
const customers = ref([]) as Ref<any[]>;

const fetchCustomersData = async () => {
  const d = await fetchCustomers({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    customers.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(`${window.location.origin}/api/customers-save-bulk`, {
      method: "post",
      headers: {
        "content-type": "application/json",
        authorization: ctx.value.apiKey ?? "",
      },
      body: JSON.stringify([selectedCustomer.value]),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    selectedCustomer.value = null;
    fetchCustomersData();
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchCustomersData();
</script>
<template>
  <dialog
    class="bg-light"
    style="z-index: 100; width: 75vw"
    :open="selectedCustomer"
  >
    <div>
      <div>
        <h4>Customer Detail ({{ selectedCustomer?.id }})</h4>
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
        :value="selectedCustomer?.name"
        @blur="(e) => {
          selectedCustomer.name = (e.target as HTMLInputElement).value
        }"
      />
    </div>
    <div>
      <small><strong>Phone</strong></small>
    </div>
    <div class="d-flex">
      <input
        class="form-control form-control-sm"
        placeholder="Phone..."
        :value="selectedCustomer?.phone"
        @blur="(e) => {
          selectedCustomer.phone = (e.target as HTMLInputElement).value
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
        :value="selectedCustomer?.address"
        @blur="(e) => {
          selectedCustomer.address = (e.target as HTMLInputElement).value
        }"
      />
    </div>

    <div class="d-flex justify-content-end">
      <template v-if="!saveLoading">
        <button
          class="btn btn-sm btn-danger px-1 py-0"
          @click="
            () => {
              selectedCustomer = null;
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
      <div><h4>Customer</h4></div>
      <div class="mx-3">
        <button
          class="px-1 py-0 btn btn-sm btn-primary"
          @click="
            () => {
              selectedCustomer = {};
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
            'Name',
            'Phone',
            'Address',
            'Created',
            'Updated',
            'Action',
          ]"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
      <tr v-for="(c, i_) of customers">
        <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
        <td class="border border-dark p-0 m-0">{{ c?.id }}</td>
        <td class="border border-dark p-0 m-0">{{ c?.name }}</td>
        <td class="border border-dark p-0 m-0">{{ c?.phone }}</td>
        <td class="border border-dark p-0 m-0">{{ c?.address }}</td>

        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(c?.createdAt) }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(c?.updatedAt) }}
        </td>

        <td class="border border-dark p-0 m-0">
          <div>
            <button
              @click="
                () => {
                  selectedCustomer = c;
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
