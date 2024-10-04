import React, { useEffect, useState } from "react";
import axios from "axios";

const UnapprovedUsers = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUnapprovedUsers = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5271/api/User/unapproved"
        );
        setUsers(response.data);
        setLoading(false);
      } catch (error) {
        setError("Error fetching unapproved users");
        setLoading(false);
      }
    };

    fetchUnapprovedUsers();
  }, []);

  const handleApproveUser = async (user) => {
    const confirm = window.confirm(
      "Are you sure you want to approve this user?"
    );
    if (confirm) {
      try {
        await axios.patch(`http://localhost:5271/api/User/approve/${user.userId}`, {
            fullName:user.fullName,
            email:user.email,
        }
        );
        setUsers((prevUsers) =>
          prevUsers.filter((user) => user.userId !== user.userId)
        ); 
      } catch (error) {
        alert("Error approving user: " + error.message);
      }
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <h2 className="my-4">Unapproved User Accounts</h2>

      {users.length === 0 ? (
        <div className="alert alert-warning">No unapproved users found</div>
      ) : (
        <table className="table table-striped">
          <thead>
            <tr>
              <th>User ID</th>
              <th>Email</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {users.map((user) => (
              <tr key={user.userId}>
                <td>{user.userId}</td>
                <td>{user.email}</td>
                <td>
                  <button
                    className="btn btn-success"
                    onClick={() => handleApproveUser(user)}
                  >
                    Approve
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default UnapprovedUsers;
