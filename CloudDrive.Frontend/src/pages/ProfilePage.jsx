import { useState, useEffect } from "react";
import ProfileCard from "../components/ProfileCard";
import SetTitle from "../hooks/SetTitle";
import { getUserProfileApi } from "../services/userAPI";

export default function ProfilePage() {
  SetTitle("Profile");

  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        setLoading(true);
        const profileData = await getUserProfileApi();
        setUser(profileData);
      } catch (err) {
        setError(err.message || "Failed to fetch user profile");
        console.error("Error fetching profile:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  if (loading) {
    return <div className="p-4">Loading...</div>;
  }

  if (error) {
    return <div className="alert alert-danger p-4">{error}</div>;
  }

  if (!user) {
    return <div className="alert alert-warning p-4">User not found</div>;
  }

  return (
    <div>
      <h1>Profile Page</h1>
      <ProfileCard user={user} setUser={setUser} />
    </div>
  );
}
