const BASE_URL = "http://localhost:5214";

export async function apiCall(route, method = "GET", body = null) {
  const options = {
    method,
    headers: {
      "Content-Type": "application/json",
    },
  };
  const token = localStorage.getItem("CloudDrive Token");
  if (token) {
    options.headers.Authorization = `Bearer ${token}`;
  }
  if (body) {
    options.body = JSON.stringify(body);
  }

  const response = await fetch(`${BASE_URL}${route}`, options);
  if (response.status === 204) {
    return null;
  }
  const data = await response.json();

  if (!response.ok) {
    throw new Error(data.message || "Something went wrong");
  }

  return data;
}
