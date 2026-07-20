import { useState } from "react";
import ProfileCard from "../components/ProfileCard";
import SetTitle from "../hooks/SetTitle";

export default function ProfilePage() {
  SetTitle("Profile");

  const [user, setUser] = useState({
    name: "John Doe",
    email: "john@example.com",
    storageUsedBytes: 101,
    storageLimitBytes: 120,
  });

  return (
    <div>
      <h1>Profile Page</h1>
      <ProfileCard user={user} setUser={setUser} />
    </div>
  );
}
