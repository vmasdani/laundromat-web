export const fetchItems = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/items`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchUsers = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/users`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchInventorySummary = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(
      `${window.location.origin}/api/inventory-summary`,
      {
        headers: { authorization: params.apiKey ?? "" },
      }
    );
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchInventory = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/inventory`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchStores = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/stores`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};
export const fetchCustomers = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/customers`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchAppStats = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/appstats`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchLaundryRecords = async (params: { apiKey?: any }) => {
  try {
    const resp = await fetch(`${window.location.origin}/api/laundryrecords`, {
      headers: { authorization: params.apiKey ?? "" },
    });
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const fetchLaundryRecord = async (params: {
  apiKey?: any;
  id?: any;
}) => {
  try {
    const resp = await fetch(
      `${window.location.origin}/api/laundryrecords/${params.id}`,
      {
        headers: { authorization: params.apiKey ?? "" },
      }
    );
    if (resp.status !== 200) {
      throw await resp.text();
    }

    return await resp.json();
  } catch (e) {
    console.error(e);
    return [];
  }
};
