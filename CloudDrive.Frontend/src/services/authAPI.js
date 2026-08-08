import { apiCall } from "./apiCall";

export function loginApi(params) {
  return apiCall("/login", "POST", params);
}

export function registerApi(params) {
  return apiCall("/register", "POST", params);
}
