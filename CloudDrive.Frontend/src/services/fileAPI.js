const BASE_URL = "http://localhost:5214";

export async function uploadFileApi(file, folderId = null) {
  const formData = new FormData();
  formData.append("file", file);

  const options = {
    method: "POST",
  };

  const token = localStorage.getItem("CloudDrive Token");
  if (token) {
    options.headers = {
      Authorization: `Bearer ${token}`,
    };
  }

  let url = `${BASE_URL}/api/files/upload`;
  if (folderId) {
    url += `?folderId=${folderId}`;
  }

  const response = await fetch(url, {
    ...options,
    body: formData,
  });

  const data = await response.json();

  if (!response.ok) {
    throw new Error(data.message || "File upload failed");
  }

  return data;
}

export async function getFilesApi(folderId = null, page = 1, pageSize = 5) {
  const queryParams = new URLSearchParams({
    page,
    pageSize,
  });

  if (folderId) {
    queryParams.append("folderId", folderId);
  }

  return fetch(`${BASE_URL}/api/files?${queryParams}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("CloudDrive Token")}`,
    },
  }).then((response) => {
    if (!response.ok) {
      throw new Error("Failed to fetch files");
    }
    return response.json();
  });
}

export async function downloadFileApi(fileId) {
  const token = localStorage.getItem("CloudDrive Token");

  const response = await fetch(`${BASE_URL}/api/files/download/${fileId}`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to download file");
  }

  return response;
}

export async function deleteFileApi(fileId) {
  const token = localStorage.getItem("CloudDrive Token");

  const response = await fetch(`${BASE_URL}/api/files/${fileId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to delete file");
  }

  return null;
}
