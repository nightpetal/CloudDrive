import React, { useEffect, useState } from "react";
import { FaFolder } from "react-icons/fa";
import { getFoldersApi } from "../services/folderAPI";

export default function UserFolder({ refreshKey, onFolderClick }) {
  const [folders, setFolders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadFolders = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await getFoldersApi();

        console.log("Folders:", data);

        // If API returns an array
        if (Array.isArray(data)) {
          setFolders(data);
        }
        // If API returns { folders: [...] }
        else if (Array.isArray(data.folders)) {
          setFolders(data.folders);
        }
        // If API returns { data: [...] }
        else if (Array.isArray(data.data)) {
          setFolders(data.data);
        }
        // Anything else
        else {
          setFolders([]);
        }
      } catch (err) {
        console.error("Failed to fetch folders:", err);
        setError(err.message || "Failed to fetch folders.");
        setFolders([]);
      } finally {
        setLoading(false);
      }
    };

    loadFolders();
  }, [refreshKey]);

  if (loading) {
    return (
      <div className="mb-4">
        <h5 className="fw-bold mb-3">Folders</h5>
        <p className="text-muted">Loading folders...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="mb-4">
        <h5 className="fw-bold mb-3">Folders</h5>
        <div className="alert alert-danger">{error}</div>
      </div>
    );
  }

  return (
    <div className="mb-4">
      <h5 className="fw-bold mb-3">Folders</h5>

      {folders.length === 0 ? (
        <p className="text-muted">No folders yet.</p>
      ) : (
        <div className="row g-3">
          {folders.map((folder) => (
            <div className="col-md-4 col-xl-3" key={folder.id}>
              <button
                type="button"
                className="btn btn-light border w-100 text-start p-3"
                onClick={() => onFolderClick(folder)}
              >
                <FaFolder className="text-warning me-2" size={20} />

                <span className="fw-semibold">{folder.name}</span>
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
