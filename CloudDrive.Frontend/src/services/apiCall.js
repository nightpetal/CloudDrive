const BASE_URL = "http://localhost:5214";

export async function apiCall(
  route,
  method = "GET",
  body = null,
  isRetry = false,
) {
  const options = {
    method,
    headers: {
      "Content-Type": "application/json",
    },
  };

  const accessToken = localStorage.getItem("CloudDrive Token");

  if (accessToken) {
    options.headers.Authorization = `Bearer ${accessToken}`;
  }

  if (body !== null) {
    options.body = JSON.stringify(body);
  }

  const response = await fetch(`${BASE_URL}${route}`, options);

  if (response.status === 401 && !isRetry) {
    console.log("Access token expired. Attempting refresh...");

    const refreshToken = localStorage.getItem("CloudDrive RefreshToken");

    if (!refreshToken) {
      console.error("No refresh token found.");
      logout();
      return;
    }

    try {
      const refreshResponse = await fetch(`${BASE_URL}/api/Auth/refresh`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          refreshToken: refreshToken,
        }),
      });

      console.log("Refresh response:", refreshResponse.status);

      if (!refreshResponse.ok) {
        const errorText = await refreshResponse.text();

        console.error("Refresh token rejected:", errorText);

        logout();
        return;
      }

      const refreshData = await refreshResponse.json();

      console.log("Refresh response data:", refreshData);

      if (!refreshData.accessToken || !refreshData.refreshToken) {
        console.error("Refresh response missing tokens:", refreshData);

        logout();
        return;
      }

      localStorage.setItem("CloudDrive Token", refreshData.accessToken);
      localStorage.setItem("CloudDrive RefreshToken", refreshData.refreshToken);

      console.log("Tokens successfully refreshed.");
      return apiCall(route, method, body, true);
    } catch (error) {
      console.error("Token refresh request failed:", error);

      logout();
      return;
    }
  }

  // =========================================================
  // ORIGINAL REQUEST STILL FAILED AFTER REFRESH
  // =========================================================
  if (response.status === 401 && isRetry) {
    console.error("Request still unauthorized after token refresh.");

    logout();
    return;
  }

  // =========================================================
  // NO CONTENT
  // =========================================================
  if (response.status === 204) {
    return null;
  }

  // =========================================================
  // READ RESPONSE
  // =========================================================
  const contentType = response.headers.get("content-type");

  let data = null;

  if (contentType?.includes("application/json")) {
    data = await response.json();
  } else {
    data = await response.text();
  }

  // =========================================================
  // API ERROR
  // =========================================================
  if (!response.ok) {
    throw new Error(data?.message || data || "Something went wrong");
  }

  return data;
}

function logout() {
  localStorage.removeItem("CloudDrive Token");
  localStorage.removeItem("CloudDrive RefreshToken");

  window.location.href = "/login";
}
