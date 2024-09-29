import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Table, Modal, Spinner } from "react-bootstrap";

const DeactivatedProfiles = () => {
  const [profiles, setProfiles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [show, setShow] = useState(false);
  const [selectedProfile, setSelectedProfile] = useState(null);
  const [activating, setActivating] = useState(false);

  // Fetch deactivated profiles
  useEffect(() => {
    const fetchDeactivatedProfiles = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5271/api/User/unactivated"
        );
        setProfiles(response.data);
        setLoading(false);
      } catch (err) {
        setError("Error fetching profiles.");
        setLoading(false);
      }
    };

    fetchDeactivatedProfiles();
  }, []);

  // Handle showing profile details
  const handleShow = (profile) => {
    setSelectedProfile(profile);
    setShow(true);
  };

  // Handle closing the modal
  const handleClose = () => setShow(false);

  // Handle activating a profile
  const handleActivate = async (userId) => {
    setActivating(true);
    try {
      await axios.put(`http://localhost:5271/api/User/${userId}/activate`);

      // Update UI after activation
      setProfiles((prevProfiles) =>
        prevProfiles.filter((profile) => profile.userId !== userId)
      );
      setShow(false);
      alert("User profile activated successfully!");
    } catch (err) {
      console.error("Error activating profile:", err);
      alert("Error activating profile.");
    } finally {
      setActivating(false);
    }
  };

  // Render the list of profiles
  return (
    <div className="container mt-4">
      <h2>Deactivated User Profiles</h2>
      {loading ? (
        <Spinner animation="border" variant="primary" />
      ) : error ? (
        <div className="alert alert-danger">{error}</div>
      ) : profiles.length === 0 ? (
        <div className="alert alert-warning">
          No deactivated profiles found.
        </div>
      ) : (
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>User ID</th>
              <th>Email</th>
              <th>Account Activated</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {profiles.map((profile) => (
              <tr key={profile.userId}>
                <td>{profile.userId}</td>
                <td>{profile.email}</td>
                <td>{profile.accountActivated ? "Yes" : "No"}</td>
                <td>
                  <Button variant="primary" onClick={() => handleShow(profile)}>
                    View Details
                  </Button>
                  <Button
                    variant="success"
                    className="ms-2"
                    onClick={() => handleActivate(profile.userId)}
                    disabled={profile.accountActivated || activating}
                  >
                    {activating ? "Activating..." : "Activate"}
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}

      {/* Modal for profile details */}
      {selectedProfile && (
        <Modal show={show} onHide={handleClose}>
          <Modal.Header closeButton>
            <Modal.Title>User Profile Details</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p>
              <strong>User ID:</strong> {selectedProfile.userId}
            </p>
            <p>
              <strong>Email:</strong> {selectedProfile.email}
            </p>
            <p>
              <strong>Account Activated:</strong>{" "}
              {selectedProfile.accountActivated ? "Yes" : "No"}
            </p>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleClose}>
              Close
            </Button>
          </Modal.Footer>
        </Modal>
      )}
    </div>
  );
};

export default DeactivatedProfiles;
