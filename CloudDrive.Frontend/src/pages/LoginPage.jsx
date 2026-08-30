import { useState } from "react";
import SetTitle from "../hooks/SetTitle";
import Loading from "../components/Loading";
import { loginApi } from "../services/authAPI";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
  SetTitle("Login");

  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      setLoading(true);

      const response = await loginApi(formData);

      console.log(response);
      localStorage.setItem("CloudDrive Token", response.accessToken);
      localStorage.setItem("CloudDrive RefreshToken", response.refreshToken);
      navigate("/");
    } catch (error) {
      alert("failed");
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
        <h2 className="text-center mb-4">Login</h2>

        <form onSubmit={handleSubmit}>
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
            Login
          </button>
        </form>
      </div>
    </div>
  );
}
