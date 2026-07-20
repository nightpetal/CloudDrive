import SetTitle from "../hooks/SetTitle";

export default function AboutPage() {
  SetTitle("About");

  return (
    <div className="flex-grow-1">
      <section className="py-5 bg-light">
        <div className="container">
          <div className="row align-items-center">

            <div className="col-lg-6 mb-4 mb-lg-0">
              <h1 className="display-5 fw-bold">
                Secure cloud storage built for simplicity
              </h1>

              <p className="lead text-muted mt-3">
                Store, manage, and access your files from anywhere with a
                reliable cloud storage platform designed for speed, security,
                and convenience.
              </p>

              <p className="text-muted">
                Our platform helps individuals and teams keep their important
                files organized while providing secure access across devices.
                Upload documents, manage folders, and keep your data protected
                in one place.
              </p>
            </div>

            <div className="col-lg-6">
              <div className="card shadow border-0 p-4">
                <div className="card-body">
                  <h3 className="fw-bold mb-4">Why choose our storage?</h3>

                  <div className="mb-3">
                    <h5 className="fw-semibold">Secure by design</h5>
                    <p className="text-muted mb-0">
                      Your files are stored with modern security practices to
                      help keep your data private.
                    </p>
                  </div>

                  <div className="mb-3">
                    <h5 className="fw-semibold">Access anywhere</h5>
                    <p className="text-muted mb-0">
                      Access your files anytime from any device with a smooth
                      and responsive experience.
                    </p>
                  </div>

                  <div>
                    <h5 className="fw-semibold">Simple organization</h5>
                    <p className="text-muted mb-0">
                      Manage files and folders easily with an interface built
                      for productivity.
                    </p>
                  </div>

                </div>
              </div>
            </div>

          </div>
        </div>
      </section>

      <section className="py-5">
        <div className="container text-center">
          <h2 className="fw-bold mb-4">Our mission</h2>

          <p className="text-muted mx-auto" style={{ maxWidth: "700px" }}>
            We aim to make cloud storage simple, secure, and accessible for
            everyone. Whether you are saving personal files or collaborating
            with a team, our goal is to provide a dependable place for your
            digital world.
          </p>
        </div>
      </section>
    </div>
  );
}