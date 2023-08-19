<script setup lang="ts">
import { ref } from "vue";
import { ctx } from "../main";
import { fetchUserData } from "../fetchers";
// import { ctx } from "../main";
// const localStoragex = localStorage;
const username = ref("");
const password = ref("");

const login = async () => {
  if (username.value === "") {
    alert("Username must be filled.");

    return;
  }

  if (password.value === "") {
    alert("password must be filled.");

    return;
  }

  try {
    const resp = await fetch(`${window.location.origin}/api/login`, {
      method: "post",
      headers: { "content-type": "application/json" },
      body: JSON.stringify({
        username: username.value,
        password: password.value,
      }),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    const d = await resp.json();

    const tok = d?.token;

    ctx.value.apiKey = tok;
    localStorage.setItem("apiKey", tok);

    fetchUserData(ctx);

  } catch (e) {
    try {
      // alert(e as any)
      alert(JSON.parse(e as any)?.detail);
    } catch (e) {
      console.error(e);
      alert("Error");
    }
  }
};
</script>
<template>
  <div class="vw-100 vh-100 d-flex justify-content-center align-items-center">
    <form
      @submit="
        (e) => {
          // ctx.apiKey = 'a';
          // localStoragex.setItem('apiKey', 'a');
          e?.preventDefault();
          login();
        }
      "
    >
      <div
        class="d-flex flex-column align-items-center p-3 border border-dark shadow shadow-md"
      >
        <div><h4>OpenLaundromat</h4></div>
        <div class="my-2">
          <input
            class="form-control form-control-sm"
            placeholder="Username..."
            @input="e=>{
              username = (e.target as HTMLInputElement).value
            }"
          />
        </div>
        <div class="my-2">
          <input
            type="password"
            class="form-control form-control-sm"
            placeholder="Password..."
            @input="e=>{
              password = (e.target as HTMLInputElement).value
            }"
          />
        </div>
        <div class="my-2">
          <button type="submit" class="btn btn-sm btn-primary">Login</button>
        </div>
      </div>
    </form>
  </div>
</template>
