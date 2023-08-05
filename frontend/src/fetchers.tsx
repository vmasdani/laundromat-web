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