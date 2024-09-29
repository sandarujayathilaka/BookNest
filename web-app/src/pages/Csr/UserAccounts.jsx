import React, { useState } from "react";
import axios from "axios";
import { Button, Form, Container, Row, Col, Alert } from "react-bootstrap";

// Function to generate a random password
const generatePassword = () => {
  const charset =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
  let password = "";
  for (let i = 0; i < 10; i++) {
    password += charset[Math.floor(Math.random() * charset.length)];
  }
  return password;
};

const CreateUser = () => {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [role, setRole] = useState("Vendor");
  const [message, setMessage] = useState("");
  const [error, setError] = useState(null);

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    const passwordHash = generatePassword();

    const newUser = {
      fullName,
      email,
      passwordHash,
      role,
    };

    try {
      const response = await axios.post(
        "http://localhost:5271/api/User/register",
        newUser
      );
      setMessage(
        `User created successfully.`
      );
      setFullName("");
      setEmail("");
      setRole("Vendor");
      setError(null);
    } catch (err) {
      setError("Error creating user.");
      setMessage("");
    }
  };

  return (
    <Container className="mt-4">
      <Row>
        <Col md={{ span: 6, offset: 3 }}>
          <h2>Create Vendor or CSR Account</h2>
          {message && <Alert variant="success">{message}</Alert>}
          {error && <Alert variant="danger">{error}</Alert>}

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="fullName" className="mb-3">
              <Form.Label>Full Name</Form.Label>
              <Form.Control
                type="text"
                value={fullName}
                onChange={(e) => setFullName(e.target.value)}
                placeholder="Enter full name"
                required
              />
            </Form.Group>

            <Form.Group controlId="email" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Enter email"
                required
              />
            </Form.Group>

            <Form.Group controlId="role" className="mb-3">
              <Form.Label>Role</Form.Label>
              <Form.Control
                as="select"
                value={role}
                onChange={(e) => setRole(e.target.value)}
                required
              >
                <option value="Vendor">Vendor</option>
                <option value="CSR">CSR</option>
              </Form.Control>
            </Form.Group>

            <Button type="submit" variant="primary">
              Create Account
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default CreateUser;
