import { useState } from "react";
import SetTitle from "../hooks/SetTitle";
import Loading from "../components/Loading";
import { registerApi } from "../services/authAPI";
import { useNavigate } from "react-router-dom";

export default function RegisterPage() {
  SetTitle("Register");

  const navigator = useNavigate();
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
  });

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      setLoading(true);

      const response = await registerApi(formData);

      console.log(response);
      alert("Register Successful");

      localStorage.setItem("CloudDrive Token", response.token);

      navigator("/");
    } catch (error) {
      alert(error.message);
    } finally {
      setLoading(false);
    }
  }

  function handleChange(e) {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  }

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="flex-grow-1 d-flex justify-content-center align-items-center">
      <div className="card shadow p-4" style={{ width: "350px" }}>
        <h2 className="text-center mb-4">Register</h2>

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label">User Name</label>
            <input
              type="text"
              name="username"
              className="form-control"
              placeholder="Enter full name"
              value={formData.username}
              onChange={handleChange}
              required
            />
          </div>

          <div className="mb-3">
            <label className="form-label">Email</label>
            <input
              type="email"
              name="email"
              className="form-control"
              placeholder="Enter email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className="mb-3">
            <label className="form-label">Password</label>
            <input
              type="password"
              name="password"
              className="form-control"
              placeholder="Enter password"
              value={formData.password}
              onChange={handleChange}
              required
              minLength={6}
            />
          </div>

          <button type="submit" className="btn btn-primary w-100">
            Register
          </button>
        </form>
      </div>
    </div>
  );
}
