<script setup lang="ts">
// import VueSelect from "vue-select";
import {
  fetchAdminConfig,
  fetchCustomers,
  fetchInventorySummary,
  fetchItems,
  fetchLaundryRecord,
  fetchStores,
} from "../fetchers";
import { Ref } from "vue";
import { ctx } from "../main";
import { ref } from "vue";
import { useRoute } from "vue-router";
import { formatDateTimeShort } from "../helpers";
import { computed } from "vue";
// import { computed } from "vue";

// const windowx = window;

const customers = ref([]) as Ref<any[]>;
const record = ref({
  recordItems: [],
  recordExtraServices: [],

  status: "PROCESSING",
  paid: false,
}) as Ref<any>;
// const saveLoading = ref(false);
const route = useRoute();
// const router = useRouter();
const inventorySummary = ref([]) as Ref<any[]>;
// const newRecordItem = ref({}) as Ref<any>;
const items = ref([]) as Ref<any[]>;
const stores = ref([]) as Ref<any[]>;
// const searchByName = ref("");
// const searchByPhone = ref("");
// const searchByAddress = ref("");
const adminConfig = ref(null) as Ref<any>;

const showStore = ref(true);
const showCustName = ref(true);
const showCustPhone = ref(true);
const showCustAddr = ref(true);
const showWeight = ref(true);
const showNotes = ref(true);
const showAdditionalItems = ref(true);
const showExtraServices = ref(true);
const showDiscountedPrice = ref(true);
const showLaundryStatus = ref(true);
const showPaidStatus = ref(true);

const foundPricePerWeight = computed(() => {
  const ppw =
    stores.value?.find((s) => `${s?.id}` === `${record?.value.storeId}`)
      ?.pricePerWeight ?? 0;

  return ppw;
});
type ModeType = "Edit" | "Print Preview";
const modeTypes = ["Edit", "Print Preview"] as ModeType[];
const mode = ref("Print Preview") as Ref<ModeType>;

// const saveCustomerLoading = ref(false);

// const checkStoreAndAdminConfig = () => {
//   if ((stores.value?.length ?? 0) > 0 && adminConfig) {
//     if (adminConfig.value?.defaultStoreId) {
//       record.value.storeId = adminConfig.value?.defaultStoreId;
//     } else {
//       record.value.storeId = stores.value?.[0]?.id;
//     }
//   }
// };

const fetchAdminConfigData = async () => {
  const d = await fetchAdminConfig({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    adminConfig.value = d;
    // checkStoreAndAdminConfig();
  }
};

const fetchStoresData = async () => {
  const d = await fetchStores({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    stores.value = d;
    // checkStoreAndAdminConfig();
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

// const handleSaveNewCustomer = async () => {
//   try {
//     saveCustomerLoading.value = true;
//     const resp = await fetch(
//       `${window.location.origin}/api/customers-save-bulk`,
//       {
//         method: "post",
//         headers: {
//           "content-type": "application/json",
//           authorization: ctx.value.apiKey ?? "",
//         },
//         body: JSON.stringify([
//           {
//             name: searchByName.value,
//             phone: searchByPhone.value,
//           },
//         ]),
//       }
//     );

//     if (resp.status !== 200) {
//       throw await resp.text();
//     }

//     window.alert("Customer successfully added.");
//     fetchCustomersData();
//   } catch (e) {
//     return "";
//   } finally {
//     saveCustomerLoading.value = false;
//   }
// };

const hideNav = () => {
  ctx.value.hideNavBar = true;
};

const init = async () => {
  await Promise.all([
    fetchInventorySummaryData(),
    fetchItemsData(),
    fetchStoresData(),
    hideNav(),
  ]);

  setTimeout(() => {
    window.print();
  }, 1000);
};

init();
// const calculatedSnapshotPrice = computed(() => {
//   const foundPricePerWeight =
//     stores.value?.find((s) => `${s?.id}` === `${record?.value.storeId}`)
//       ?.pricePerWeight ?? 0;

//   const finalPricePerweight = foundPricePerWeight * (record.value?.weight ?? 0);

//   const recordItemPrice = record.value?.recordItems?.reduce(
//     (acc: any, i: any) => {
//       const foundItem = items.value.find(
//         (ix) => `${ix?.id}` === `${i?.itemId}`
//       );

//       return acc + (foundItem?.price ?? 0) * (i?.qty ?? 0);
//     },
//     0.0
//   );
//   const recordExtraServicesPrice = record.value?.recordExtraServices?.reduce(
//     (acc: any, s: any) => {
//       return acc + s?.price;
//     },
//     0.0
//   );

//   return 10 + finalPricePerweight + recordItemPrice + recordExtraServicesPrice;
// });

// const handleSave = async () => {
//   console.log("save");

//   // const foundStore = stores.value?.find(
//   //   (s) => `${s?.id}` === `${record.value?.storeId}`
//   // );

//   // if (
//   //   (foundStore?.minimumDropOffWeight ?? 0) > 0 &&
//   //   (record.value?.weight ?? 0) < (foundStore?.minimumDropOffWeight ?? 0)
//   // ) {
//   //   alert(`Minimum weight must be ${foundStore?.minimumDropOffWeight ?? 0}.`);

//   //   return;
//   // }

//   const priceSnapshot = calculatedSnapshotPrice.value;

//   console.log("snapshotprice =", priceSnapshot);

//   let finalSnapshot = priceSnapshot;

//   // if (priceSnapshot < 10 && !record.value?.isDiscount) {
//   //   if (
//   //     window.confirm(
//   //       `Total price is less than $10, round up to $10? (If you click cancel, it'll be $${priceSnapshot})`
//   //     )
//   //   ) {
//   //     finalSnapshot = 10;
//   //   }
//   // }

//   if (record.value?.isDiscount) {
//     finalSnapshot = record.value.discountPrice;
//   }

//   try {
//     saveLoading.value = true;
//     const resp = await fetch(
//       `${window.location.origin}/api/laundryrecords-save-bulk`,
//       {
//         method: "post",
//         headers: {
//           "content-type": "application/json",
//           authorization: ctx.value.apiKey ?? "",
//         },
//         body: JSON.stringify([
//           {
//             ...record.value,
//             priceSnapshot: finalSnapshot,
//           },
//         ]),
//       }
//     );

//     if (resp.status !== 200) {
//       throw await resp.text();
//     }

//     router.push("/");
//   } catch (e) {
//     console.error(e);
//     return "";
//   } finally {
//     saveLoading.value = false;
//   }
// };

// const searchedCustomers = computed(() => {
//   console.log(searchByName.value, searchByPhone.value);
//   if (
//     searchByName.value === "" &&
//     searchByPhone.value === "" &&
//     searchByAddress.value === ""
//   ) {
//     return [];
//   }

//   return customers.value.filter(
//     (c) =>
//       (searchByName.value === ""
//         ? true
//         : `${c?.name?.toLowerCase() ?? ""}`?.includes(
//             searchByName.value.toLowerCase()
//           )) &&
//       (searchByPhone.value === ""
//         ? true
//         : `${c?.phone?.toLowerCase() ?? ""}`?.includes(
//             searchByPhone.value.toLowerCase()
//           )) &&
//       (searchByAddress.value === ""
//         ? true
//         : `${c?.phone?.toLowerCase() ?? ""}`?.includes(
//             searchByAddress.value.toLowerCase()
//           ))
//   );
// });

// const foundIdenticalCustomerData = computed(() => {
//   return customers.value.find(
//     (c) =>
//       `${c?.name?.toLowerCase() ?? ""}` === searchByName.value.toLowerCase() &&
//       `${c?.phone?.toLowerCase() ?? ""}` ===
//         searchByPhone.value.toLowerCase() &&
//       `${c?.address?.toLowerCase() ?? ""}` ===
//         searchByAddress.value.toLowerCase()
//   );
// });

fetchDropOffDetailData();
fetchCustomersData();
fetchAdminConfigData();

const windowx = window;
</script>
<template>
  <div class="container">
    <div class="d-flex">
      <div v-for="t in modeTypes">
        <button
          :class="`btn btn-sm ${
            mode === t ? `btn-primary` : `btn-outline-primary`
          } px-1 py-0`"
          @click="
            () => {
              mode = t;
            }
          "
        >
          {{ t }}
        </button>
      </div>
      <div class="mx-2">
        <button
          class="btn btn-sm btn-outline-primary px-1 py-0"
          @click="
            () => {
              windowx.print();
            }
          "
        >
          <VIcon icon="mdi mdi-printer" /> Print Receipt
        </button>
      </div>
    </div>
    <div v-if="mode === 'Edit'">
      <div><h5>Select items to show:</h5></div>
      <div class="d-flex">
        <div>Store:</div>
        <div>
          <input
            :checked="showStore"
            type="checkbox"
            @click="
              () => {
                showStore = !showStore;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Customer name:</div>
        <div>
          <input
            :checked="showCustName"
            type="checkbox"
            @click="
              () => {
                showCustName = !showCustName;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Customer phone:</div>
        <div>
          <input
            :checked="showCustPhone"
            type="checkbox"
            @click="
              () => {
                showCustPhone = !showCustPhone;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Customer address:</div>
        <div>
          <input
            :checked="showCustAddr"
            type="checkbox"
            @click="
              () => {
                showCustAddr = !showCustAddr;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Weight:</div>
        <div>
          <input
            :checked="showWeight"
            type="checkbox"
            @click="
              () => {
                showWeight = !showWeight;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Notes:</div>
        <div>
          <input
            :checked="showNotes"
            type="checkbox"
            @click="
              () => {
                showNotes = !showNotes;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Additional Items:</div>
        <div>
          <input
            :checked="showAdditionalItems"
            type="checkbox"
            @click="
              () => {
                showAdditionalItems = !showAdditionalItems;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Extra services:</div>
        <div>
          <input
            :checked="showExtraServices"
            type="checkbox"
            @click="
              () => {
                showExtraServices = !showExtraServices;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Discounted price:</div>
        <div>
          <input
            :checked="showDiscountedPrice"
            type="checkbox"
            @click="
              () => {
                showDiscountedPrice = !showDiscountedPrice;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Laundry Status:</div>
        <div>
          <input
            :checked="showLaundryStatus"
            type="checkbox"
            @click="
              () => {
                showLaundryStatus = !showLaundryStatus;
              }
            "
          />
        </div>
      </div>
      <div class="d-flex">
        <div>Paid Status:</div>
        <div>
          <input
            :checked="showPaidStatus"
            type="checkbox"
            @click="
              () => {
                showPaidStatus = !showPaidStatus;
              }
            "
          />
        </div>
      </div>
    </div>
    <div v-if="mode === 'Print Preview'">
      <div class="my-3"><h5>Drop-off Receipt</h5></div>
      <div><hr class="border border-dark" /></div>
      <div>
        <strong>Printed Date: </strong>
        {{
          formatDateTimeShort(new Date().toISOString(), {
            dateStyle: "long",
            timeStyle: "long",
          })
        }}
      </div>
      <div>
        <strong>Drop-off Date: </strong>
        {{
          formatDateTimeShort(record?.createdAt, {
            dateStyle: "long",
            timeStyle: "long",
          })
        }}
      </div>
      <div v-if="showStore">
        <strong>Store:</strong>
        {{ stores.find((s) => `${s?.id}` === `${record?.storeId}`)?.name }}
      </div>
      <template
        v-for="c in [
          {
            foundCustomer: customers.find(
              (c) => `${c?.id}` === `${record?.customerId}`
            ),
          },
        ]"
      >
        <div v-if="showCustName">
          <strong>Customer name:</strong> {{ c.foundCustomer?.name }}
        </div>
        <div v-if="showCustPhone">
          <strong>Customer phone:</strong> {{ c.foundCustomer?.phone }}
        </div>
        <div v-if="showCustAddr">
          <strong>Customer address:</strong> {{ c.foundCustomer?.address }}
        </div>
      </template>

      <div v-if="showWeight">
        <strong>Weight:</strong> {{ record?.weight?.toFixed(2) }} x ${{
          foundPricePerWeight?.toFixed(2)
        }}
        = {{ (foundPricePerWeight * (record?.weight ?? 0)).toFixed(2) }}
      </div>
      <div v-if="showNotes">
        <strong>Additional note:</strong>
        <div>
          {{ record?.note }}
        </div>
      </div>
      <div v-if="showAdditionalItems && (record?.recordItems?.length ?? 0) > 0">
        <strong>Additional items:</strong>
        <ol>
          <li v-for="i in record?.recordItems">
            <template
              v-for="it in [
                {
                  foundItem: items.find((ix) => `${ix?.id}` === `${i?.itemId}`),
                },
              ]"
            >
              {{ it.foundItem?.name }}: ${{
                it?.foundItem?.price?.toFixed(2)
              }}
              x {{ i?.qty }} =
              {{ ((it?.foundItem?.price ?? 0) * (i?.qty ?? 0))?.toFixed(2) }}
            </template>
          </li>
        </ol>
      </div>
      <div
        v-if="
          showExtraServices && (record?.recordExtraServices?.length ?? 0) > 0
        "
      >
        <strong>Extra services:</strong>
        <ol>
          <li v-for="s in record?.recordExtraServices">
            {{ s?.name }} = ${{ s?.price?.toFixed(2) }}
          </li>
        </ol>
      </div>
      <div v-if="showDiscountedPrice && record?.isDiscount">
        <strong>Discounted Price:</strong> {{ record?.discountPrice }}
      </div>
      <div v-if="showLaundryStatus">
        <strong>Laundry status:</strong> {{ record?.status ?? "PROCESSING" }}
      </div>
      <div v-if="showPaidStatus">
        <strong>Paid status:</strong>
        <strong :class="record?.paid ? `text-success` : `text-danger`">
          {{ record?.paid ? `PAID` : `UNPAID` }}</strong
        >
      </div>
      <div v-if="showPaidStatus">
        <strong>Initial price: </strong>${{ 10?.toFixed(2) }}
      </div>
      <div><hr class="border border-dark" /></div>
      <div>
        <h5>Total: ${{ record?.priceSnapshot?.toFixed(2) }}</h5>
      </div>
    </div>
  </div>
</template>
