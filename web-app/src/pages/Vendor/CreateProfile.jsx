import React, { useState } from "react";
import axios from "axios";
import { Button, Form, Container, Row, Col, Alert } from "react-bootstrap";

const CreateProfile = () => {
  const [phoneNumber, setPhoneNumber] = useState("");
  const [address, setAddress] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState(null);

  // Hardcoded values for vendorUserId and role
  const vendorUserId = "VENLJ58167"; 
  const role = "Vendor"; 

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();

    // Hardcoded vendorUserId for now
    const vendorUserId = "VENLJ58167";  
    const role = "Vendor";  
    //  hardcoded token 
    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJWRU5MSjU4MTY3IiwibmFtZWlkIjoiVkVOTEo1ODE2NyIsInVuaXF1ZV9uYW1lIjoiYm9va3lhcmRAZ21haWwuY29tIiwicm9sZSI6IlZlbmRvciIsIm5iZiI6MTcyODAyODU3MiwiZXhwIjoxNzI4NjMzMzcyLCJpYXQiOjE3MjgwMjg1NzIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0In0.rzWleP-PbB4UEYmF0WOYo0R_hnllUpD_sa916axJtOk"; 

    const newProfile = {
      vendorUserId,  
      role,           
      phoneNumber,
      address
    };

    try {
      const response = await axios.post(
        "http://localhost:5271/api/Vendor/createprofile",
        newProfile,
        {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
          }
        }
      );
      setMessage("Profile created successfully.");
      setPhoneNumber("");
      setAddress("");
      setError(null);
    } catch (err) {
      setError("Error creating profile.");
      setMessage("");
    }
};


  return (
    <Container className="mt-4">
      <Row>
        <Col md={{ span: 6, offset: 3 }}>
          <h2>Create Vendor Profile</h2>
          {message && <Alert variant="success">{message}</Alert>}
          {error && <Alert variant="danger">{error}</Alert>}

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="phoneNumber" className="mb-3">
              <Form.Label>Phone Number</Form.Label>
              <Form.Control
                type="text"
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
                placeholder="Enter your phone number"
                required
              />
            </Form.Group>

            <Form.Group controlId="address" className="mb-3">
              <Form.Label>Address</Form.Label>
              <Form.Control
                type="text"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                placeholder="Enter your address"
                required
              />
            </Form.Group>

            <Button type="submit" variant="primary">
              Create Profile
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default CreateProfile;
