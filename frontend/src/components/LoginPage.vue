<script setup lang="ts">
import { ctx } from "../main";
const localStoragex = localStorage;
</script>
<template>
  <div class="vw-100 vh-100 d-flex justify-content-center align-items-center">
    <div
      class="d-flex flex-column align-items-center p-3 border border-dark shadow shadow-md"
    >
      <div><h4>OpenLaundromat</h4></div>
      <div class="my-2">
        <input class="form-control form-control-sm" placeholder="Username..." />
      </div>
      <div class="my-2">
        <input class="form-control form-control-sm" placeholder="Password..." />
      </div>
      <div class="my-2">
        <button
          class="btn btn-sm btn-primary"
          @click="
            () => {
              ctx.apiKey = 'a';
              localStoragex.setItem('apiKey', 'a');
            }
          "
        >
          Login
        </button>
      </div>
    </div>
  </div>
</template>
