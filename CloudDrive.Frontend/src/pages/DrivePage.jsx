import React, { useState } from "react";
import { FaUpload, FaSearch } from "react-icons/fa";

import { uploadFileApi } from "../services/fileAPI";
import UserFolder from "../components/UserFolder";
import UserFile from "../components/UserFile";
import Sidebar from "../components/Sidebar";

export default function DrivePage() {
  const [refreshKey, setRefreshKey] = useState(0);
  const [selectedFolder, setSelectedFolder] = useState(null);
  const [uploading, setUploading] = useState(false);

  const handleFolderCreated = () => {
    // Increase refreshKey so UserFolder fetches again
    setRefreshKey((prev) => prev + 1);
  };

  return (
    <div className="flex-grow-1 d-flex justify-content-center align-items-center">
      <div className="container-fluid">
        <div className="row">
          {/* Sidebar */}
          <Sidebar
            currentFolderId={selectedFolder?.id ?? null}
            onFolderCreated={handleFolderCreated}
          />

          {/* Main content */}
          <div className="col-lg-10 p-4">
            {/* Search + Upload */}
            <div className="d-flex justify-content-between align-items-center mb-4">
              <div className="input-group w-50">
                <span className="input-group-text bg-white border-end-0">
                  <FaSearch />
                </span>

                <input
                  className="form-control border-start-0"
                  placeholder="Search files..."
                />
              </div>

              <label
                className="btn btn-primary"
                style={{
                  pointerEvents: uploading ? "none" : "auto",
                  opacity: uploading ? 0.7 : 1,
                }}
              >
                <FaUpload className="me-2" />

                {uploading ? "Uploading..." : "Upload"}

                <input
                  hidden
                  type="file"
                  disabled={uploading}
                  onChange={async (e) => {
                    const file = e.target.files?.[0];

                    if (!file) return;

                    try {
                      setUploading(true);

                      const result = await uploadFileApi(
                        file,
                        selectedFolder?.id,
                      );

                      console.log("File uploaded:", result);

                      // Refresh files
                      setRefreshKey((prev) => prev + 1);

                      alert(`File "${file.name}" uploaded successfully!`);
                    } catch (error) {
                      console.error("Upload failed:", error);

                      alert(`Upload failed: ${error.message}`);
                    } finally {
                      setUploading(false);
                      e.target.value = "";
                    }
                  }}
                />
              </label>
            </div>

            {/* Folders */}
            <UserFolder
              refreshKey={refreshKey}
              onFolderClick={(folder) => {
                setSelectedFolder(folder);
              }}
            />

            {/* Files */}
            <UserFile refreshKey={refreshKey} folderId={selectedFolder?.id} />
          </div>
        </div>
      </div>
    </div>
  );
}
