<script setup lang="ts">
import { ref } from "vue";

const detailOpen = ref(false);
</script>
<template>
  <dialog class="bg-light" style="z-index: 100; width: 75vw" :open="detailOpen">
    <div>
      <div><h4>Inventory Detail</h4></div>
    </div>
    <div><hr class="border border-dark" /></div>
    <div class="d-flex justify-content-end">
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
      <button class="mx-2 btn btn-sm btn-primary px-1 py-0">Save</button>
    </div>
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

  <hr class="border border-dark" />

  <div
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
    </table>
  </div>
</template>
