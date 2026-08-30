import { useState, useEffect } from "react";
import { FaEdit, FaCheck, FaTimes } from "react-icons/fa";

export default function ProfileCard({ user }) {
  const [editing, setEditing] = useState(false);
  const [formData, setFormData] = useState(user);

  // Use username or displayName, fallback to email
  const displayName = user.username || user.name || user.email || "User";

  const storagePercentage =
    user.storageLimitBytes > 0
      ? (user.storageUsedBytes / user.storageLimitBytes) * 100
      : 0;

  useEffect(() => {
    setFormData(user);
  }, [user]);

  function handleChange(e) {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  }

  function handleSubmit() {
    // todo edit api
    console.log("Updated user:", formData);
    setEditing(false);
  }

  function handleCancel() {
    setFormData(user);
    setEditing(false);
  }

  return (
    <div className="d-flex justify-content-center align-items-center flex-grow-1">
      <div className="card shadow border-0 p-4" style={{ width: "380px" }}>
        <div className="d-flex justify-content-between align-items-start">
          <div className="d-flex align-items-center">
            <div
              className="rounded-circle bg-primary text-white d-flex justify-content-center align-items-center me-3"
              style={{ width: "60px", height: "60px", fontSize: "24px" }}
            >
              {displayName.charAt(0).toUpperCase()}
            </div>

            {editing ? (
              <div>
                <input
                  type="text"
                  name="username"
                  className="form-control mb-2"
                  value={formData.username || ""}
                  onChange={handleChange}
                />

                <input
                  type="email"
                  name="email"
                  className="form-control"
                  value={formData.email || ""}
                  onChange={handleChange}
                />
              </div>
            ) : (
              <div>
                <h5 className="mb-1 fw-bold">{displayName}</h5>
                <p className="text-muted mb-0">{user.email}</p>
              </div>
            )}
          </div>

          {!editing ? (
            <button
              className="btn btn-light btn-sm"
              onClick={() => setEditing(true)}
            >
              <FaEdit />
            </button>
          ) : (
            <div>
              <button
                className="btn btn-success btn-sm me-2"
                onClick={handleSubmit}
              >
                <FaCheck />
              </button>

              <button className="btn btn-danger btn-sm" onClick={handleCancel}>
                <FaTimes />
              </button>
            </div>
          )}
        </div>

        <hr />

        <div>
          <div className="d-flex justify-content-between mb-2">
            <span className="fw-semibold">Storage Used</span>
            <span className="text-muted">
              {(user.storageUsedBytes / 1024 / 1024).toFixed(2)} MB /{" "}
              {(user.storageLimitBytes / 1024 / 1024).toFixed(2)} MB
            </span>
          </div>

          <div className="progress" style={{ height: "10px" }}>
            <div
              className="progress-bar bg-primary"
              role="progressbar"
              style={{ width: `${storagePercentage}%` }}
            ></div>
          </div>

          <p className="text-muted small mt-2 mb-0">
            {storagePercentage.toFixed(1)}% of your storage space is used
          </p>
        </div>
      </div>
    </div>
  );
}
