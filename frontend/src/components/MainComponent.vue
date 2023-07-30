<script setup lang="ts">
import Nav from "./Nav.vue";
</script>

<template>
  <div>
    <Nav />
    <div class="m-3">
      <!-- <div>Test main componen</div>
      <div><button class="btn btn-sm btn-primary">Button</button></div> -->
      <RouterView />

    </div>
  </div>
</template>
