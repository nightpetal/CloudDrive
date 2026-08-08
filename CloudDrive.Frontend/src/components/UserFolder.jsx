import { useEffect, useState } from "react";
import { FaFolder } from "react-icons/fa";
import { apiCall } from "../services/apiCall";
export default function UserFolder() {
  const [folders, setFolders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  // Get folders from API
  useEffect(() => {
    const loadFolders = async () => {
      try {
        setLoading(true);
        setError("");
        const data = await apiCall("/api/Folders");
        setFolders(data);
      } catch (err) {
        console.error(err);
        setError("Unable to load folders.");
      } finally {
        setLoading(false);
      }
    };

    loadFolders();
  }, []);

  return (
    <>
      <h5 className="fw-bold mb-3">Folders</h5>

      {loading && <div className="text-muted mb-4">Loading folders...</div>}

      {error && <div className="alert alert-danger">{error}</div>}

      {!loading && !error && folders.length === 0 && (
        <div className="text-muted mb-4">No folders found.</div>
      )}

      {!loading && !error && folders.length > 0 && (
        <div className="row g-3 mb-5">
          {folders.map((folder) => (
            <div className="col-md-6 col-xl-3" key={folder.id}>
              <div className="card border-0 shadow-sm rounded-4">
                <div className="card-body d-flex align-items-center">
                  <FaFolder size={45} className="text-warning me-3" />

                  <div>
                    <h6 className="mb-1 fw-bold">{folder.name}</h6>

                    <small className="text-muted">Folder</small>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </>
  );
}
