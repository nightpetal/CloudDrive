import { apiCall } from "./apiCall";

export function loginApi(params) {
  return apiCall("/login", "POST", params);
}

export function registerApi(params) {
  return apiCall("/register", "POST", params);
}

export function refreshTokenApi(refreshToken) {
  return apiCall("/api/auth/refresh", "POST", refreshToken);
}

export function revokeTokenApi(refreshToken) {
  return apiCall("/api/auth/revoke", "POST", refreshToken);
}

export function getStoredToken() {
  return localStorage.getItem("CloudDrive Token");
}

export function getStoredRefreshToken() {
  return localStorage.getItem("CloudDrive RefreshToken");
}

export function clearTokens() {
  localStorage.removeItem("CloudDrive Token");
  localStorage.removeItem("CloudDrive RefreshToken");
}

export async function refreshAccessToken() {
  try {
    const refreshToken = getStoredRefreshToken();
    if (!refreshToken) {
      throw new Error("No refresh token available");
    }

    const response = await refreshTokenApi(refreshToken);

    localStorage.setItem("CloudDrive Token", response.accessToken);
    localStorage.setItem("CloudDrive RefreshToken", response.refreshToken);

    return response.accessToken;
  } catch (error) {
    clearTokens();
    throw error;
  }
}
