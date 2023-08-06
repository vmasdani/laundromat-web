<script setup lang="ts"></script>
<template>
  <nav class="navbar navbar-expand-lg bg-light">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">OpenLaundromat</a>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarNavDropdown"
        aria-controls="navbarNavDropdown"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNavDropdown">
        <ul class="navbar-nav">
          <!-- <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="#">Home</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Features</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Pricing</a>
          </li> -->
          <li class="nav-item">
            <a class="nav-link" href="#">Daily Operation</a>
          </li>
          <li class="nav-item dropdown">
            <a
              class="nav-link dropdown-toggle"
              href="#"
              role="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              Shop Data
            </a>
            <ul class="dropdown-menu">
              <li><a class="dropdown-item" href="/#/stores">Stores</a></li>
              <li><a class="dropdown-item" href="/#/items">Items</a></li>
              <li>
                <a class="dropdown-item" href="/#/inventory">Inventory</a>
              </li>
              <li>
                <a class="dropdown-item" href="/#/customers">Customers</a>
              </li>
            </ul>
          </li>
          <li class="nav-item dropdown">
            <a
              class="nav-link dropdown-toggle"
              href="#"
              role="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              Administration
            </a>
            <ul class="dropdown-menu">
              <li>
                <a class="dropdown-item" href="/#/employees">Employees</a>
              </li>
              <li>
                <a class="dropdown-item" href="/#/shopsettings"
                  >Shop Settings</a
                >
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
