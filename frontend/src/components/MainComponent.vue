<script setup lang="ts">
import { ctx } from "../main";
import LoginPage from "./LoginPage.vue";
import Nav from "./Nav.vue";
</script>

<template>
  <div v-if="ctx.apiKey">
    <Nav />
    <div class="m-3">
      <!-- <div>Test main componen</div>
      <div><button class="btn btn-sm btn-primary">Button</button></div> -->
      <RouterView />
    </div>
  </div>
  <div v-else>
    <LoginPage />
  </div>
</template>
