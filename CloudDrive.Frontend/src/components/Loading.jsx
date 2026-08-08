import React from "react";

export default function Loading() {
  return (
    <div
      className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center"
      style={{
        backgroundColor: "rgba(255, 255, 255, 0.65)",
        backdropFilter: "blur(3px)",
        zIndex: 9999,
      }}
    >
      <div
        className="bg-white rounded-4 shadow-lg text-center p-4"
        style={{
          width: "280px",
        }}
      >
        <div
          className="spinner-border text-primary mb-3"
          role="status"
          style={{
            width: "2rem",
            height: "2rem",
          }}
        >
          <span className="visually-hidden">Loading...</span>
        </div>

        <h6 className="fw-bold mb-1">Wait a sec...</h6>

        <p className="text-muted small mb-0">
          We're getting everything ready for you.
        </p>
      </div>
    </div>
  );
}
