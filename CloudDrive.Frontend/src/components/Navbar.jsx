import { NavLink, useNavigate } from "react-router-dom";

export default function Navbar() {
  const navigate = useNavigate();

  const navClass = ({ isActive }) =>
    `nav-link ${isActive ? "active fw-semibold text-white" : "text-white-50"}`;

  const token = localStorage.getItem("CloudDrive Token");

  const handleLogout = () => {
    localStorage.removeItem("CloudDrive Token");
    navigate("/login");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
      <div className="container">
        <NavLink className="navbar-brand fw-bold fs-4" to="/">
          Cloud<span className="text-primary">Drive</span>
        </NavLink>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto align-items-lg-center gap-lg-3">
            {token ? (
              <>
                <li className="nav-item">
                  <NavLink end className={navClass} to="/drive">
                    Drive
                  </NavLink>
                </li>

                <li className="nav-item">
                  <NavLink className={navClass} to="/profile">
                    Profile
                  </NavLink>
                </li>

                <li className="nav-item">
                  <button
                    onClick={handleLogout}
                    className="btn btn-outline-light btn-sm"
                  >
                    Logout
                  </button>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <NavLink className={navClass} to="/login">
                    Login
                  </NavLink>
                </li>

                <li className="nav-item">
                  <NavLink className={navClass} to="/register">
                    Register
                  </NavLink>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
}
