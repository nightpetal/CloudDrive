import React, { useState } from "react";
import {
  FaFolder,
  FaFilePdf,
  FaFileWord,
  FaFileImage,
  FaFileAlt,
  FaUpload,
  FaPlus,
  FaSearch,
  FaCloud,
} from "react-icons/fa";

export default function DrivePage() {
  const [folders] = useState([
    { id: 1, name: "Documents", files: 14 },
    { id: 2, name: "Photos", files: 92 },
    { id: 3, name: "Projects", files: 7 },
    { id: 4, name: "Downloads", files: 24 },
  ]);

  const [files] = useState([
    {
      id: 1,
      name: "Resume.pdf",
      type: "pdf",
      size: "1.2 MB",
      modified: "Today",
    },
    {
      id: 2,
      name: "Vacation.jpg",
      type: "image",
      size: "3.4 MB",
      modified: "Yesterday",
    },
    {
      id: 3,
      name: "Report.docx",
      type: "word",
      size: "820 KB",
      modified: "2 days ago",
    },
  ]);

  const icon = (type) => {
    switch (type) {
      case "pdf":
        return <FaFilePdf className="text-danger fs-4" />;
      case "image":
        return <FaFileImage className="text-success fs-4" />;
      case "word":
        return <FaFileWord className="text-primary fs-4" />;
      default:
        return <FaFileAlt className="text-secondary fs-4" />;
    }
  };

  return (
    <div className="flex-grow-1 d-flex justify-content-center align-items-center">
      <div className="container-fluid">
        <div className="row">
          <div className="col-lg-2 bg-white border-end p-4">
            <h4 className="fw-bold mb-5">
              <FaCloud className="text-primary me-2" />
              CloudDrive
            </h4>

            <button className="btn btn-primary rounded-pill w-100 mb-4">
              <FaPlus className="me-2" />
              New
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

          <div className="col-lg-10 p-4">
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

              <label className="btn btn-primary">
                <FaUpload className="me-2" />
                Upload
                <input hidden type="file" multiple />
              </label>
            </div>

            <h5 className="fw-bold mb-3">Folders</h5>

            <div className="row g-3 mb-5">
              {folders.map((folder) => (
                <div className="col-md-6 col-xl-3" key={folder.id}>
                  <div className="card border-0 shadow-sm rounded-4">
                    <div className="card-body d-flex align-items-center">
                      <FaFolder size={45} className="text-warning me-3" />

                      <div>
                        <h6 className="mb-1 fw-bold">{folder.name}</h6>

                        <small className="text-muted">
                          {folder.files} files
                        </small>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>

            <div className="card border-0 shadow-sm rounded-4">
              <div className="card-header bg-white border-0 py-3">
                <h5 className="mb-0 fw-bold">Recent Files</h5>
              </div>

              <div className="table-responsive">
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
                            <div className="me-3">{icon(file.type)}</div>

                            {file.name}
                          </div>
                        </td>

                        <td>{file.size}</td>

                        <td>{file.modified}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
