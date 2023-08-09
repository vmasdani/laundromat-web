<script setup lang="ts">
import VueSelect from "vue-select";
import {
  fetchCustomers,
  fetchInventorySummary,
  fetchItems,
  fetchLaundryRecord,
  fetchStores,
} from "../fetchers";
import { Ref } from "vue";
import { ctx } from "../main";
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { laundryRecordStatuses } from "../helpers";
import { computed } from "vue";

const windowx = window;

const customers = ref([]) as Ref<any[]>;
const record = ref({ recordItems: [] }) as Ref<any>;
const saveLoading = ref(false);
const route = useRoute();
const router = useRouter();
const inventorySummary = ref([]) as Ref<any[]>;
const newRecordItem = ref({}) as Ref<any>;
const items = ref([]) as Ref<any[]>;
const stores = ref([]) as Ref<any[]>;

const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;

    if ((d?.length ?? 0) > 0 && !record?.value?.storeId) {
      record.value.storeId = d?.[0]?.id;
    }
  }
};
const fetchItemsData = async () => {
  const d = await fetchItems({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    items.value = d;
  }
};

const fetchInventorySummaryData = async () => {
  const d = await fetchInventorySummary({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    inventorySummary.value = d;
  }
};

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
fetchInventorySummaryData();
fetchItemsData();
fetchStoresData();

const calculatedSnapshotPrice = computed(() => {
  const foundPricePerWeight =
    stores.value?.find((s) => `${s?.id}` === `${record?.value.storeId}`)
      ?.pricePerWeight ?? 0;

  const finalPricePerweight = foundPricePerWeight * (record.value?.weight ?? 0);

  const recordItemPrice = record.value?.recordItems?.reduce(
    (acc: any, i: any) => {
      const foundItem = items.value.find(
        (ix) => `${ix?.id}` === `${i?.itemId}`
      );

      return acc + (foundItem?.price ?? 0) * (i?.qty ?? 0);
    },
    0.0
  );

  return finalPricePerweight + recordItemPrice;
});

const handleSave = async () => {
  console.log("save");

  const priceSnapshot = calculatedSnapshotPrice.value;

  console.log("snapshotprice =", priceSnapshot);
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
        body: JSON.stringify([
          {
            ...record.value,
            priceSnapshot: priceSnapshot,
          },
        ]),
      }
    );

    if (resp.status !== 200) {
      throw await resp.text();
    }

    router.push("/");
  } catch (e) {
    console.error(e);
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
    <div v-if="!saveLoading" class="d-flex align-items-center">
      <div><h4>Drop Off Detail</h4></div>

      <div class="mx-2">
        <button
          class="btn btn-sm btn-primary px-2 py-1"
          @click="
            () => {
              handleSave();
            }
          "
        >
          Save
        </button>
      </div>

      <div class="mx-2">
        <button
          class="btn btn-sm btn-danger px-2 py-1"
          @click="
            () => {
              router.push('/');
            }
          "
        >
          Cancel
        </button>
      </div>
    </div>
    <div v-else><div class="spinner-border spinner-border-sm"></div></div>
    <div><hr class="border border-dark" /></div>
    <div>
      <div>
        <small><strong>Store</strong></small>
      </div>
      <div>
        <VueSelect
          placeholder="Select store..."
          :options="stores"
          :getOptionLabel="(s: any) => `${s?.name}`"
          @update:modelValue="(s: any) => {
            record.storeId = s?.id;
          }"
          :modelValue="stores.find((s) => `${s?.id}` === `${record?.storeId}`)"
        />
      </div>

      <div>
        <small
          ><strong
            >Customer |
            <span class="text-success"
              >Selected: {{ record?.customer?.name }} ({{
                record?.customer?.phone
              }})</span
            >
          </strong></small
        >
      </div>
      <div class="d-flex">
        <div class="flex-grow-1">
          <div>
            <small><strong>Search customer by phone</strong></small>
          </div>
          <div>
            <VueSelect
              placeholder="By phone..."
              :options="customers"
              :getOptionLabel="(c: any) => `${c?.phone} (${c?.name})`"
              @update:modelValue="(c: any) => {
                record.customerId = c?.id;
                record.customer = c
              }"
            />
          </div>
        </div>
        <div class="flex-grow-1">
          <div>
            <small><strong>Search customer by name</strong></small>
          </div>
          <div>
            <VueSelect
              placeholder="By name..."
              :options="customers"
              :getOptionLabel="(c: any) => `${c?.name}`"
              @update:modelValue="(c: any) => {
                record.customerId = c?.id;
                record.customer = c
              }"
            />
          </div>
        </div>
      </div>

      <div v-if="!record?.storeId">
        <small><strong>No store specified</strong></small>
      </div>
      <div>
        <small
          ><strong
            >Weight (
            <span v-if="record?.storeId"
              >${{
                stores?.find((s) => `${s?.id}` === `${record?.storeId}`)
                  ?.pricePerWeight
              }}</span
            ><span v-else="record?.storeId" class="text-danger"
              >No store specified</span
            >
            per weight unit) =
            {{
              (
                (stores?.find((s) => `${s?.id}` === `${record?.storeId}`)
                  ?.pricePerWeight ?? 0) * (record?.weight ?? 0)
              )?.toFixed(2)
            }}</strong
          ></small
        >
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
        <div>
          <div>
            <small><strong>Additional Items</strong></small>
          </div>
          <div class="d-flex">
            <div>
              <input
                class="form-control form-control-sm"
                placeholder="Qty..."
                style="width: 75px"
                @input="e=>{ 
                  console.log((e.target as HTMLInputElement).value)

                  const v = isNaN(parseFloat((e.target as HTMLInputElement).value))
                    ? 0
                    : parseFloat((e.target as HTMLInputElement).value)
                
                  newRecordItem.qty = v
                }"
              />
            </div>

            <div class="flex-grow-1">
              <VueSelect
                :options="inventorySummary"
                :getOptionLabel="(i: any) => `#${i?.store?.name}: #${i?.item?.id}: ${i?.item?.name} | qty = ${i?.qty}`"
                @update:modelValue="(i: any) => {
                  // windowx.alert(JSON.stringify(i))
                  console.log(i)

                  newRecordItem.storeId = i?.store?.id
                  newRecordItem.itemId = i?.item?.id
                  
                }"
                placeholder="Select item.."
              />
            </div>

            <button
              class="btn btn-sm btn-primary px-1 py-0"
              @click="
                () => {
                  const foundInventorySummary = inventorySummary.find(
                    (s) =>
                      `${s?.store?.id}` === `${newRecordItem?.storeId}` &&
                      `${s?.item?.id}` === `${newRecordItem?.itemId}`
                  );

                  // windowx.alert(JSON.stringify(foundInventorySummary));

                  if (newRecordItem.qty > (foundInventorySummary?.qty ?? 0)) {
                    windowx.alert(
                      `Stock insufficient. In stock: ${foundInventorySummary?.qty}, needed: ${newRecordItem.qty}`
                    );
                    return;
                  }

                  record.recordItems?.push(newRecordItem);
                  newRecordItem = {};
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
              v-for="h in ['#', 'Item', 'Store', 'Qty', 'Price', 'Qty x Price']"
              :class="`bg-dark text-light m-0 p-0`"
              style="position: sticky; top: 0"
            >
              {{ h }}
            </th>
            <tr v-for="(i, i_) in record?.recordItems ?? []">
              <td class="border border-dark p-0 m-0">{{ i_ + 1 }}</td>
              <td class="border border-dark p-0 m-0">
                <template
                  v-for="d in [
                    {
                      foundItem: items.find(
                        (ix) => `${ix?.id}` === `${i?.itemId}`
                      ),
                    },
                  ]"
                >
                  #{{ d?.foundItem?.id }}: {{ d?.foundItem?.name }}
                </template>
              </td>
              <td class="border border-dark p-0 m-0">
                <template
                  v-for="d in [
                    {
                      foundStore: stores.find(
                        (ix) => `${ix?.id}` === `${i?.storeId}`
                      ),
                    },
                  ]"
                >
                  {{ d?.foundStore?.name }}
                </template>
              </td>
              <td class="border border-dark p-0 m-0">
                {{ i?.qty }}
              </td>
              <template
                v-for="d in [
                  {
                    foundItem: items.find(
                      (ix) => `${ix?.id}` === `${i?.itemId}`
                    ),
                  },
                ]"
              >
                <td class="border border-dark p-0 m-0">
                  {{ d?.foundItem?.price }}
                </td>
                <td class="border border-dark p-0 m-0">
                  {{ (d?.foundItem?.price ?? 0) * (i?.qty ?? 0) }}
                </td>
              </template>
            </tr>
          </table>
        </div>
      </div>
      <div class="d-flex">
        <div>
          <small><strong>Is discount?</strong></small>
        </div>
        <div class="mx-2">
          <input
            type="checkbox"
            :checked="record?.isDiscount"
            @click="
              () => {
                record.isDiscount = !record.isDiscount;
              }
            "
          />
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
              const v = isNaN(parseFloat((e.target as HTMLInputElement).value))
                    ? 0
                    : parseFloat((e.target as HTMLInputElement).value)
                
              
              record.discountPrice = v
            }
          "
          :value="record.discountPrice"
        />
      </div>

      <div>
        <small><strong>Status</strong></small>
      </div>

      <div class="d-flex">
        <div v-for="s in laundryRecordStatuses">
          <button
            :class="`btn btn-sm ${
              record?.status === s ? `btn-primary` : `btn-outline-primary`
            }`"
            @click="
              () => {
                record.status = s;
              }
            "
          >
            {{ s }}
          </button>
        </div>
      </div>

      <div>
        <small><strong>Paid</strong></small>
      </div>

      <div class="d-flex">
        <div
          v-for="s in [
            { label: 'Paid', value: true },
            { label: 'Unpaid', value: false },
          ]"
        >
          <button
            :class="`btn btn-sm ${
              record?.paid === s.value ? `btn-primary` : `btn-outline-primary`
            }`"
            @click="
              () => {
                record.paid = s.value;
              }
            "
          >
            {{ s.label }}
          </button>
        </div>
      </div>

      <div><hr class="border border-dark" /></div>
      <div>
        <h5>
          Final price: $<span v-if="record?.isDiscount">{{
            record?.discountPrice
          }}</span>
          <span v-else>{{ calculatedSnapshotPrice?.toFixed(2) }}</span>
        </h5>
      </div>

      <div v-if="!saveLoading" class="d-flex align-items-center">
        <div class="mx-2">
          <button
            class="btn btn-sm btn-primary px-2 py-1"
            @click="
              () => {
                handleSave();
              }
            "
          >
            Save
          </button>
        </div>

        <div class="mx-2">
          <button
            class="btn btn-sm btn-danger px-2 py-1"
            @click="
              () => {
                router.push('/');
              }
            "
          >
            Cancel
          </button>
        </div>
      </div>
      <div v-else><div class="spinner-border spinner-border-sm"></div></div>
    </div>
  </div>
</template>
