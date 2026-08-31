import { apiCall } from "./apiCall";

export async function createFolderApi(name, parentFolderId = null) {
  return apiCall("/api/Folders", "POST", {
    parentFolderId,
    name,
  });
}

export async function getFoldersApi() {
  return apiCall("/api/Folders", "GET");
}
