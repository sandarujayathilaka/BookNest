// NotFound.js
import React from "react";
import { Container, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const NotFound = () => {
  const navigate = useNavigate();

  // Redirect back to the home or any other page
  const handleGoBack = () => {
    navigate("/dashboard");
  };

  return (
    <div className="d-flex justify-content-center align-items-center vh-100 text-center">
      {/* Center the content using flexbox */}
      <Container>
        <h1 className="display-1 fw-bold">404</h1>
        <p className="lead">Oops! The page you're looking for doesn't exist.</p>
        <Button variant="primary" onClick={handleGoBack}>
          Go Back to Home
        </Button>
      </Container>
    </div>
  );
};

export default NotFound;
