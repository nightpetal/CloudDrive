import React, { useEffect, useState } from "react";
import { FaUpload, FaSearch, FaCloud } from "react-icons/fa";
import { apiCall } from "../services/apiCall";
import UserFolder from "../components/UserFolder";
import UserFile from "../components/UserFile";
import Sidebar from "../components/Sidebar";

export default function DrivePage() {
  const [folders, setFolders] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  return (
    <div className="flex-grow-1 d-flex justify-content-center align-items-center">
      <div className="container-fluid">
        <div className="row">
          {/* Sidebar */}
          <Sidebar />

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

              <label className="btn btn-primary">
                <FaUpload className="me-2" />
                Upload
                <input hidden type="file" multiple />
              </label>
            </div>

            {/* Folders */}
            <UserFolder />

            {/* Recent Files */}
            <UserFile />
          </div>
        </div>
      </div>
    </div>
  );
}
