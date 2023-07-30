<script setup lang="ts">

</script>
<template>
  
  <div>
    <div class="d-flex align-items-center">
      <div><h4>Home</h4></div>
    </div>

    <hr class="border border-dark" />

    <div
      v-for="i in ['Stores', 'Items', 'Inventory', 'Customers', 'Employees']"
    >
      <div class="d-flex">
        <div><VIcon class="text-danger" icon="mdi-close-circle" /></div>
        <div class="mx-2">
          <strong>{{ i }}</strong>
        </div>
      </div>
    </div>
  </div>
</template>
