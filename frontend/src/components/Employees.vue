<script setup lang="ts">
import { Ref } from "vue";
import { ref } from "vue";
import { fetchUsers } from "../fetchers";
import { ctx } from "../main";

const detailOpen = ref(false);

const changePassword = ref(false);
const saveLoading = ref(false);

const users = ref([]) as Ref<any[]>;
const selectedUser = ref({}) as Ref<any>;
const newPassword = ref("");
const confirmNewPassword = ref("");

const fetchUsersData = async () => {
  const d = await fetchUsers({ apiKey: ctx.value.apiKey ?? "" });

  if (d) {
    users.value = d;
  }
};

const handleSave = async () => {
  try {
    saveLoading.value = true;
    const resp = await fetch(`${window.location.origin}/api/users-save-bulk`, {
      method: "post",
      headers: {
        "content-type": "application/json",
        authorization: ctx.value.apiKey ?? "",
      },
      body: JSON.stringify([selectedUser.value]),
    });

    if (resp.status !== 200) {
      throw await resp.text();
    }

    selectedUser.value = {};
    fetchUsersData();
  } catch (e) {
    return "";
  } finally {
    saveLoading.value = false;
  }
};

fetchUsersData();
</script>
<template>
  <dialog class="bg-light" style="z-index: 100; width: 75vw" :open="detailOpen">
    <div>
      <div><h4>Employee Detail</h4></div>
    </div>
    <div><hr class="border border-dark" /></div>

    <div>
      <small><strong>Username</strong></small>
    </div>
    <div>
      <input
        placeholder="Username..."
        class="form-control form-control-sm"
        :value="selectedUser?.username"
        @blur="(e) => {
          selectedUser.username = (e.target as HTMLInputElement).value
        }"
      />
    </div>

    <div>
      <small><strong>Name</strong></small>
    </div>
    <div>
      <input
        placeholder="Name..."
        class="form-control form-control-sm"
        :value="selectedUser?.name"
        @blur="(e) => {
          selectedUser.name = (e.target as HTMLInputElement).value
        }"
      />
    </div>

    <div class="d-flex">
      <small><strong>Change password?</strong></small>
      <div>
        <input
          type="checkbox"
          :checked="changePassword"
          @click="
            () => {
              changePassword = !changePassword;
            }
          "
        />
      </div>
    </div>

    <template v-if="changePassword">
      <div>
        <small><strong>New Password</strong></small>
      </div>
      <div>
        <input
          type="password"
          placeholder="Password..."
          class="form-control form-control-sm"
          :value="newPassword"
          @input="(e) => {
            newPassword=(e.target as HTMLInputElement).value

            selectedUser.passwordStr = (e.target as HTMLInputElement).value

        }"
        />
      </div>
      <div>
        <small><strong>Confirm New Password</strong></small>
      </div>
      <div>
        <input
          type="password"
          placeholder="Confirm Password..."
          class="form-control form-control-sm"
          :value="confirmNewPassword"
          @input="(e) => {
            confirmNewPassword=(e.target as HTMLInputElement).value        
          }"
        />
      </div>

      <div
        v-if="
          changePassword &&
          newPassword !== '' &&
          confirmNewPassword !== '' &&
          newPassword !== confirmNewPassword
        "
      >
        <small class="text-danger"
          ><strong>Passwords do not match.</strong></small
        >
      </div>
    </template>

    <div><hr class="border border-dark" /></div>

    <template v-if="saveLoading">
      <div class="spinner-border spinner-border-sm" />
    </template>
    <template v-else></template>
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
      <button class="mx-2 btn btn-sm btn-primary px-1 py-0" @click="handleSave">
        Save
      </button>
    </div>
  </dialog>
  <div>
    <div class="d-flex align-items-center">
      <div><h4>Employee</h4></div>
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
          v-for="h in ['#', 'Name', 'Username', 'Position']"
          style="position: sticky; top: 0"
        >
          {{ h }}
        </th>
      </tr>
    </table>
  </div>
</template>
