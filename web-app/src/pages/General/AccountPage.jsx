import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";  // Import useNavigate from react-router-dom

export default function AccountPage() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();  // Initialize navigate hook
  
  const userId = "VENLJ58167";

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        const response = await axios.get(`http://localhost:5271/api/User/getuser${userId}`);
        setUser(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching user data", error); 
        setError("Error fetching user data");
        setLoading(false);
      }
    };

    fetchUserDetails();
  }, [userId]);

  const handleCreateVendorProfile = () => {
    navigate("/createvendorprofile");  
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <h2 className="my-4">Account Details</h2>

      {user ? (
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">Name - {user.fullName}</h5>
            <h5 className="card-title">Email - {user.email}</h5>
            <button 
              className="btn btn-primary mt-3" 
              onClick={handleCreateVendorProfile}
            >
              Create Vendor Profile
            </button>
          </div>
        </div>
      ) : (
        <div className="alert alert-warning">No user data found</div>
      )}
    </div>
  );
}
