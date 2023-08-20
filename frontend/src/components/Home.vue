<script setup lang="ts">
import { ref } from "vue";
import { fetchAppStats, fetchLaundryRecords, fetchStores } from "../fetchers";
import { ctx } from "../main";
import { Ref } from "vue";
import { formatDateTimeShort } from "../helpers";

const appStats = ref(null) as Ref<any | null>;
const laundryRecords = ref([]) as Ref<any[]>;
const stores = ref([]) as Ref<any[]>;

const fetchAppStatsData = async () => {
  const d = await fetchAppStats({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    appStats.value = d;
  }
};
const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;
  }
};

const fetchLaundryRecordsData = async () => {
  const d = await fetchLaundryRecords({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    laundryRecords.value = d;
  }
};

fetchLaundryRecordsData();
fetchAppStatsData();
fetchStoresData();
</script>
<template>
  <div>
    <!-- {{ JSON.stringify(ctx.user) }} -->
    <div class="d-flex align-items-center">
      <div>
        <div><h4>Drop-off Page</h4></div>
      </div>

      <div>
        <a href="/#/dropoffs/new">
          <button class="btn btn-sm btn-primary px-1 py-0">
            <VIcon class="text-light" icon="mdi-plus"> </VIcon> Add
          </button>
        </a>
      </div>
    </div>

    <hr class="border border-dark" />

    <div
      v-for="i in ['Stores', 'Items', 'Inventory', 'Customers', 'Users'].filter(
        (h) => (appStats?.[h?.toLowerCase() ?? ''] ?? 0) === 0
      )"
    >
      <div class="d-flex">
        <div><VIcon class="text-danger" icon="mdi-close-circle" /></div>
        <div class="mx-2">
          <strong>{{ i }}</strong>
        </div>
      </div>
    </div>
  </div>

  <div><hr class="border border-dark" /></div>

  <div
    class="overflow-auto border border-dark"
    style="height: 65vh; resize: vertical"
  >
    <table class="table table-sm" style="border-collapse: separate">
      <th
        class=""
        v-for="h in [
          '#',
          'Store',

          'Customer',
          'Cust. Phone',
          'Weight',
          'Remarks',
          'Status',
          'Paid',
          'Price',
          'Created at',
          'Updated at',
          'Created by',
          'Addit. Items',
          'Extra svcs.',
          'Action',
          'Receipt',
        ]"
        :class="`bg-dark text-light m-0 p-0`"
        style="position: sticky; top: 0"
      >
        {{ h }}
      </th>
      <tr v-for="(l, i) in laundryRecords">
        <td class="border border-dark p-0 m-0">{{ i + 1 }}</td>
        <td class="border border-dark p-0 m-0">
          {{ stores.find((s) => `${s?.id}` === `${l?.storeId}`)?.name }}
        </td>
        <td class="border border-dark p-0 m-0">{{ l?.customer?.name }}</td>
        <td class="border border-dark p-0 m-0">{{ l?.customer?.phone }}</td>
        <td class="border border-dark p-0 m-0">{{ l?.weight?.toFixed(2) }}</td>
        <td class="border border-dark p-0 m-0">{{ l?.remark }}</td>
        <td :class="`border border-dark p-0 m-0`">
          <small
            ><strong :class="`${l?.status === 'DONE' ? `text-success` : ``}`">{{
              l?.status ?? "PROCESSING"
            }}</strong></small
          >
        </td>
        <td :class="`border border-dark p-0 m-0`">
          <small
            ><strong :class="`${l?.paid ? `text-success` : `text-danger`}`">{{
              l?.paid ? "Paid" : "Unpaid"
            }}</strong></small
          >
        </td>
        <td class="border border-dark p-0 m-0">
          ${{ l?.priceSnapshot?.toFixed(2) }}
        </td>

        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(l?.createdAt) }}
        </td>
        <td class="border border-dark p-0 m-0">
          {{ formatDateTimeShort(l?.updatedAt) }}
        </td>
        <td class="border border-dark p-0 m-0"></td>

        <td class="border border-dark p-0 m-0">{{ l?.recordItems?.length }}</td>
        <td class="border border-dark p-0 m-0">
          {{ l?.recordExtraServices?.length }}
        </td>

        <td class="border border-dark p-0 m-0">
          <div>
            <a :href="`/#/dropoffs/${l?.id}`">
              <button class="btn btn-sm btn-primary px-1 py-0">
                <VIcon icon="mdi-note-edit"></VIcon></button
            ></a>
          </div>
        </td>

        <td class="border border-dark p-0 m-0">
          <div>
            <a :href="`/#/print-dropoffs/${l?.id}`" target="_blank">
              <button class="btn btn-sm btn-primary px-1 py-0">
                <VIcon icon="mdi-printer"></VIcon></button
            ></a>
          </div>
        </td>
      </tr>
    </table>
  </div>
</template>
