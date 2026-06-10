async function apiGet(endpoint) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    headers: {
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    }
  });
  if (!res.ok) throw new Error(`GET ${endpoint} falló: ${res.status}`);
  return res.json();
}

async function apiPost(endpoint, body) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    method: 'POST',
      headers: {
      'Content-Type': 'application/json',
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    },
    body: JSON.stringify(body)
  });
  if (!res.ok) throw new Error(`POST ${endpoint} falló: ${res.status}`);
  return res.json();
}

async function apiPut(endpoint, body) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    },
    body: JSON.stringify(body)
  });
  if (!res.ok) throw new Error(`PUT ${endpoint} falló: ${res.status}`);
  return res.json();
}

async function apiPatch(endpoint, body) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    },
    body: JSON.stringify(body)
  });
  if (!res.ok) throw new Error(`PATCH ${endpoint} falló: ${res.status}`);
  return res.json();
}

async function apiDelete(endpoint) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    method: 'DELETE',
    headers: {
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    }
  });
  if (!res.ok) throw new Error(`DELETE ${endpoint} falló: ${res.status}`);
  return res.json();
}

async function apiUploadFile(endpoint, formData) {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    method: 'POST',
    headers: {
      'Authorization': localStorage.getItem('session') ? 'Bearer ' + JSON.parse(localStorage.getItem('session')).token : ''
    },
    body: formData
  });
  if (!res.ok) throw new Error(`POST ${endpoint} falló: ${res.status}`);
  return res.json();
}