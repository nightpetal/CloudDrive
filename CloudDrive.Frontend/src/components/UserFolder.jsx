import { useEffect, useState } from "react";
import { FaFolder } from "react-icons/fa";
import { apiCall } from "../services/apiCall";

export default function UserFolder({ refreshKey, onFolderClick }) {
  const [folders, setFolders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);
  const [hasNextPage, setHasNextPage] = useState(false);

  const pageSize = 5;

  useEffect(() => {
    const loadFolders = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await apiCall(
          `/api/Folders?page=${page}&pageSize=${pageSize}`,
        );
        setFolders(data.data);
        setHasNextPage(data.hasNextPage);
      } catch (err) {
        console.error(err);
        setError("Unable to load folders.");
      } finally {
        setLoading(false);
      }
    };

    loadFolders();
  }, [refreshKey, page]);

  // Virtual root folder.
  const rootFolder = {
    id: null,
    name: "Root",
    parentFolderId: null,
  };

  const allFolders = [rootFolder, ...folders];

  const handlePrevious = () => {
    if (page > 1) {
      setPage((prev) => prev - 1);
    }
  };

  const handleNext = () => {
    if (hasNextPage) {
      setPage((prev) => prev + 1);
    }
  };

  return (
    <>
      <h5 className="fw-bold mb-3">Folders</h5>

      {loading && <div className="text-muted mb-4">Loading folders...</div>}

      {error && <div className="alert alert-danger">{error}</div>}

      {!loading && !error && (
        <>
          <div className="row g-3 mb-4">
            {allFolders.map((folder) => (
              <div className="col-md-6 col-xl-3" key={folder.id ?? "root"}>
                <button
                  type="button"
                  className="card border-0 shadow-sm rounded-4 w-100 text-start p-0 bg-white"
                  onClick={() => onFolderClick(folder)}
                  style={{ cursor: "pointer" }}
                >
                  <div className="card-body d-flex align-items-center">
                    <FaFolder
                      size={45}
                      className={
                        folder.id === null
                          ? "text-primary me-3"
                          : "text-warning me-3"
                      }
                    />

                    <div>
                      <h6 className="mb-1 fw-bold">{folder.name}</h6>

                      <small className="text-muted">
                        {folder.id === null ? "All root files" : "Folder"}
                      </small>
                    </div>
                  </div>
                </button>
              </div>
            ))}
          </div>

          {folders.length === 0 && page === 1 && (
            <div className="text-muted">No folders found.</div>
          )}

          <div className="d-flex justify-content-between align-items-center">
            <button
              type="button"
              className="btn btn-outline-secondary"
              onClick={handlePrevious}
              disabled={page === 1 || loading}
            >
              Previous
            </button>

            <span className="text-muted">Page {page}</span>

            <button
              type="button"
              className="btn btn-outline-primary"
              onClick={handleNext}
              disabled={!hasNextPage || loading}
            >
              Next
            </button>
          </div>
        </>
      )}
    </>
  );
}
