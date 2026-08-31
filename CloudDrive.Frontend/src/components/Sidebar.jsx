import { useState } from "react";
import { FaPlus } from "react-icons/fa";
import { createFolderApi } from "../services/folderAPI";

export default function Sidebar({ currentFolderId = null, onFolderCreated }) {
  const [showModal, setShowModal] = useState(false);
  const [folderName, setFolderName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleCreateFolder = async (e) => {
    e.preventDefault();

    const name = folderName.trim();

    if (!name) {
      setError("Please enter a folder name.");
      return;
    }

    try {
      setLoading(true);
      setError("");

      const newFolder = await createFolderApi(name, currentFolderId);

      console.log("Folder created:", newFolder);

      setFolderName("");
      setShowModal(false);

      // Tell DrivePage that a new folder was created
      if (onFolderCreated) {
        onFolderCreated(newFolder);
      }
    } catch (err) {
      console.error("Create folder failed:", err);
      setError(err.message || "Failed to create folder.");
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (loading) return;

    setShowModal(false);
    setFolderName("");
    setError("");
  };

  return (
    <>
      <div className="col-lg-2 bg-white border-end p-4">
        <button
          className="btn btn-primary rounded-pill w-100 mb-4"
          onClick={() => setShowModal(true)}
        >
          <FaPlus className="me-2" />
          Add Folder
        </button>

        <div className="list-group border-0">
          <button className="list-group-item list-group-item-action border-0 rounded active">
            My Drive
          </button>

          <button className="list-group-item list-group-item-action border-0 rounded">
            Shared
          </button>

          <button className="list-group-item list-group-item-action border-0 rounded">
            Recent
          </button>

          <button className="list-group-item list-group-item-action border-0 rounded">
            Trash
          </button>
        </div>
      </div>

      {showModal && (
        <div
          className="modal fade show d-block"
          tabIndex="-1"
          style={{
            backgroundColor: "rgba(0, 0, 0, 0.5)",
          }}
        >
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content">
              <form onSubmit={handleCreateFolder}>
                <div className="modal-header">
                  <h5 className="modal-title">Create Folder</h5>

                  <button
                    type="button"
                    className="btn-close"
                    onClick={handleClose}
                    disabled={loading}
                  />
                </div>

                <div className="modal-body">
                  <label htmlFor="folderName" className="form-label">
                    Folder name
                  </label>

                  <input
                    id="folderName"
                    type="text"
                    className={`form-control ${error ? "is-invalid" : ""}`}
                    placeholder="Enter folder name"
                    value={folderName}
                    onChange={(e) => {
                      setFolderName(e.target.value);
                      setError("");
                    }}
                    autoFocus
                    disabled={loading}
                  />

                  {error && <div className="invalid-feedback">{error}</div>}
                </div>

                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={handleClose}
                    disabled={loading}
                  >
                    Cancel
                  </button>

                  <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={loading}
                  >
                    {loading ? "Creating..." : "Create Folder"}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
