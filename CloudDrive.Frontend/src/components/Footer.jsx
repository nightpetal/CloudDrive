export default function Footer() {
  return (
    <footer className="bg-dark text-white mt-autp">
      <div className="container py-4">
        <div className="row">
          <div className="col-md-6 mb-3 mb-md-0">
            <h5 className="fw-bold">
              Cloud<span className="text-primary">Drive</span>
            </h5>
          </div>

          <div className="col-md-6">
            <ul className="list-unstyled d-flex gap-4 justify-content-md-end mb-0">
              <li>
                <a href="/" className="text-white-50 text-decoration-none">
                  Home
                </a>
              </li>

              <li>
                <a href="/about" className="text-white-50 text-decoration-none">
                  About
                </a>
              </li>

              <li>
                <a
                  href="/contact"
                  className="text-white-50 text-decoration-none"
                >
                  Contact
                </a>
              </li>
            </ul>
          </div>
        </div>

        <hr className="border-secondary my-4" />

        <p className="text-center text-white-50 mb-0 small">
          © {new Date().getFullYear()}{" "}
          <span className="text-white-50">CloudDrive</span> •{" "}
          <a
            href="https://github.com/nightpetal"
            target="_blank"
            rel="noopener noreferrer"
            className="text-decoration-none text-primary fw-medium"
          >
            @nightpetal
          </a>
        </p>
      </div>
    </footer>
  );
}
