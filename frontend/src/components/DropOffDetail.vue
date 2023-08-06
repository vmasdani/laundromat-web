<script setup lang="ts">
import VueSelect from "vue-select";
import { fetchCustomers, fetchLaundryRecord } from "../fetchers";
import { Ref } from "vue";
import { ctx } from "../main";
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";

const customers = ref([]) as Ref<any[]>;
const record = ref({ recordItems: [] }) as Ref<any>;
const saveLoading = ref(false);
const route = useRoute();
const router = useRouter();

const fetchCustomersData = async () => {
  const d = await fetchCustomers({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    customers.value = d;
  }
};

const fetchDropOffDetailData = async () => {
  if (isNaN(parseInt(route.params?.id as string))) {
    return;
  }
  const d = await fetchLaundryRecord({
    apiKey: ctx.value.apiKey ?? "",
    id: route.params?.id,
  });

  if (d) {
    record.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(
      `${window.location.origin}/api/laundryrecords-save-bulk`,
      {
        method: "post",
        headers: {
          "content-type": "application/json",
          authorization: ctx.value.apiKey ?? "",
        },
        body: JSON.stringify([record.value]),
      }
    );

    if (resp.status !== 200) {
      throw await resp.text();
    }

    router.push("/");
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchDropOffDetailData();
fetchCustomersData();
</script>
<template>
  <div class="container">
    <div class="d-flex align-items-center">
      <div><h4>Drop Off Detail</h4></div>
      <div>
        <button
          class="btn btn-sm btn-primary px-1 py-0"
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
    <div><hr class="border border-dark" /></div>
    <div>
      <div>
        <small><strong>Customer</strong></small>
      </div>
      <VueSelect
        :options="customers"
        :getOptionLabel="(c: any) => `${c?.phone}: ${c?.name}`"
        @update:modelValue="(c: any) => {
          record.customerId = c?.id;
        }"
        :modelValue="
          customers.find((c) => `${c?.id}` === `${record?.customerId}`)
        "
      />
      <div>
        <small><strong>Weight ($0.0 per weight unit)</strong></small>
      </div>
      <div>
        <input
          placeholder="Weight..."
          class="form-control form-control-sm"
          :value="record.weight"
          @input="
            (e) => {
              console.log(e);

              const v = isNaN(parseFloat((e.target as HTMLInputElement).value))
                ? 0
                : parseFloat((e.target as HTMLInputElement).value)

              record.weight = v
            }
          "
        />
      </div>
      <div>
        <small><strong>Remarks/notes</strong></small>
      </div>
      <div>
        <textarea
          placeholder="Remark..."
          class="form-control form-control-sm"
          :value="record.remark"
          @input="e => {
          record.remark = (e.target as HTMLInputElement).value
        }"
        />
      </div>
      <div>
        <div class="d-flex align-items-center">
          <div>
            <small><strong>Additional Items</strong></small>
          </div>
          <div>
            <button
              class="btn btn-sm btn-primary px-1 py-0"
              @click="
                () => {
                  record.recordItems?.push({});
                }
              "
            >
              <VIcon icon="mdi-plus" />
            </button>
          </div>
        </div>
      </div>
      <div>
        <div
          class="overflow-auto border border-dark"
          style="height: 15vh; resize: vertical"
        >
          <table class="table table-sm" style="border-collapse: separate">
            <th
              class=""
              v-for="h in ['#', 'Item Name', 'Desc', 'Store', 'Qty']"
              :class="`bg-dark text-light m-0 p-0`"
              style="position: sticky; top: 0"
            >
              {{ h }}
            </th>
            <tr v-for="(i, i_) in record?.recordItems ?? []">
              <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
              <td class="border border-dark p-0 m-0">{{ i?.name ?? "" }}</td>
            </tr>
          </table>
        </div>
      </div>
      <div>
        <small><strong>Discounted price</strong></small>
      </div>
      <div>
        <input
          placeholder="Discount price..."
          class="form-control form-control-sm"
          @input="
            (e) => {
              console.log(e);
              // record.weight = (e.target as HTMLInputElement).value
            }
          "
        />
      </div>
    </div>
  </div>
</template>
