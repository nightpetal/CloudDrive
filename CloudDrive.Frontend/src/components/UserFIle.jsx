import { useEffect, useState } from "react";
import { FaFilePdf, FaFileWord, FaFileImage, FaFileAlt } from "react-icons/fa";

import { apiCall } from "../services/apiCall";

export default function UserFile() {
  const [files, setFiles] = useState([]);
  const [error, setError] = useState("");

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

  useEffect(() => {
    async function loadFiles() {
      try {
        setError("");
        const data = await apiCall("/api/Files");
        setFiles(data);
      } catch (err) {
        console.error(err);
        setError("Unable to load files.");
      }
    }

    loadFiles();
  }, []);

  return (
    <div className="card border-0 shadow-sm rounded-4">
      <div className="card-header bg-white border-0 py-3">
        <h5 className="mb-0 fw-bold">Recent Files</h5>
      </div>

      <div className="table-responsive">
        {error && <div className="alert alert-danger m-3">{error}</div>}

        {!error && files.length === 0 && (
          <div className="text-muted p-3">No files found.</div>
        )}

        {!error && files.length > 0 && (
          <table className="table align-middle mb-0">
            <thead className="table-light">
              <tr>
                <th>Name</th>
                <th>Size</th>
                <th>Modified</th>
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
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
