import { apiCall } from "./apiCall";

export function getUserProfileApi() {
  return apiCall("/api/user/profile", "GET");
}
