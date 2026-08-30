import { useEffect, useState } from "react";
import {
  FaFilePdf,
  FaFileWord,
  FaFileImage,
  FaFileAlt,
  FaTrash,
  FaDownload,
} from "react-icons/fa";

import { apiCall } from "../services/apiCall";
import { downloadFileApi } from "../services/fileAPI";

export default function UserFile({ refreshKey, folderId }) {
  const [files, setFiles] = useState([]);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [loading, setLoading] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [downloadingId, setDownloadingId] = useState(null);

  const pageSize = 5;

  const icon = (extension) => {
    const type = extension?.toLowerCase().replace(".", "");

    switch (type) {
      case "pdf":
        return <FaFilePdf className="text-danger fs-4" />;

      case "jpg":
      case "jpeg":
      case "png":
      case "gif":
      case "webp":
        return <FaFileImage className="text-success fs-4" />;

      case "doc":
      case "docx":
        return <FaFileWord className="text-primary fs-4" />;

      default:
        return <FaFileAlt className="text-secondary fs-4" />;
    }
  };

  const loadFiles = async () => {
    try {
      setLoading(true);
      setError("");

      const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString(),
      });

      if (folderId) {
        params.append("folderId", folderId);
      }

      const data = await apiCall(`/api/Files?${params.toString()}`);

      setFiles(data.data);
      setHasNextPage(data.hasNextPage);
    } catch (err) {
      console.error(err);
      setError("Unable to load files.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadFiles();
  }, [refreshKey, folderId, page]);

  useEffect(() => {
    setPage(1);
  }, [folderId]);

  const handleDelete = async (file) => {
    const confirmed = window.confirm(
      `Are you sure you want to delete "${file.orginalName}"?`,
    );

    if (!confirmed) {
      return;
    }

    try {
      setDeletingId(file.id);
      setError("");

      await apiCall(`/api/Files/${file.id}`, "DELETE");

      setFiles((prevFiles) => prevFiles.filter((item) => item.id !== file.id));

      if (files.length === 1 && page > 1) {
        setPage((prev) => prev - 1);
      } else {
        await loadFiles();
      }
    } catch (err) {
      console.error(err);
      setError("Unable to delete file.");
    } finally {
      setDeletingId(null);
    }
  };

  const handleDownload = async (file) => {
    try {
      setDownloadingId(file.id);
      setError("");

      const response = await downloadFileApi(file.id);
      const blob = await response.blob();

      // Create a temporary URL for the blob
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = file.orginalName;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (err) {
      console.error(err);
      setError("Unable to download file.");
    } finally {
      setDownloadingId(null);
    }
  };

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
    <div className="card border-0 shadow-sm rounded-4 mt-3">
      <h5 className="mb-3">Recent Files</h5>

      <div className="table-responsive">
        {error && <div className="alert alert-danger m-3">{error}</div>}

        {loading && <div className="text-muted p-3">Loading files...</div>}

        {!loading && !error && files.length === 0 && (
          <div className="text-muted p-3">No files found.</div>
        )}

        {!loading && files.length > 0 && (
          <>
            <table className="table align-middle mb-0">
              <thead className="table-light">
                <tr>
                  <th>Name</th>
                  <th>Size</th>
                  <th>Modified</th>
                  <th className="text-end">Action</th>
                </tr>
              </thead>

              <tbody>
                {files.map((file) => (
                  <tr key={file.id}>
                    <td>
                      <div className="d-flex align-items-center">
                        <div className="me-3">{icon(file.extension)}</div>

                        {file.orginalName}
                      </div>
                    </td>

                    <td>{file.sizeBytes} bytes</td>

                    <td>
                      {file.updatedAt
                        ? new Date(file.updatedAt).toLocaleDateString()
                        : "-"}
                    </td>

                    <td className="text-end">
                      <button
                        type="button"
                        className="btn btn-outline-primary btn-sm me-2"
                        onClick={() => handleDownload(file)}
                        disabled={downloadingId === file.id}
                        title="Download file"
                      >
                        <FaDownload className="me-1" />

                        {downloadingId === file.id
                          ? "Downloading..."
                          : "Download"}
                      </button>

                      <button
                        type="button"
                        className="btn btn-outline-danger btn-sm"
                        onClick={() => handleDelete(file)}
                        disabled={deletingId === file.id}
                        title="Delete file"
                      >
                        <FaTrash className="me-1" />

                        {deletingId === file.id ? "Deleting..." : "Delete"}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div className="d-flex justify-content-between align-items-center mt-3">
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
      </div>
    </div>
  );
}
