import React from "react";

export default function Loading() {
  return (
    <div className="flex-grow-1 d-flex justify-content-center align-items-center">
      <div className="text-center">
        <div
          className="spinner-border text-primary mb-4"
          role="status"
          style={{ width: "3rem", height: "3rem" }}
        >
          <span className="visually-hidden">Loading...</span>
        </div>

        <h2 className="fw-bold">Wait a sec...</h2>
        <p className="text-muted">We're getting everything ready for you.</p>
      </div>
    </div>
  );
}
